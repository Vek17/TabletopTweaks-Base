using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Linq;

namespace TabletopTweaks.Base.Bugfixes.General {
    class ConcealmentFix {
        [HarmonyPatch(typeof(UnitPartConcealment), nameof(UnitPartConcealment.Calculate))]
        class UnitDescriptor_FixSizeModifiers_Patch {
            static bool Prefix(UnitPartConcealment __instance,
                UnitEntityData initiator,
                UnitEntityData target,
                bool attack,
                ref Concealment __result) {

                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("FixConcealment")) { return true; }

                UnitPartConcealment initiatorConcealmentPart = initiator.Get<UnitPartConcealment>();
                UnitPartConcealment targetConcealmentPart = target.Get<UnitPartConcealment>();
                if (initiatorConcealmentPart != null && initiatorConcealmentPart.IgnoreAll) {
                    __result = Concealment.None;
                    return false;
                }
                if (((initiatorConcealmentPart != null) ? initiatorConcealmentPart.m_BlindsightRanges : null) != null) {
                    Feet blindsightRange = 0.Feet();
                    foreach (ValueTuple<Feet, UnitConditionExceptions> valueTuple in initiatorConcealmentPart.m_BlindsightRanges) {
                        UnitConditionExceptionsTargetHasFacts unitConditionExceptionsTargetHasFacts = valueTuple.Item2 as UnitConditionExceptionsTargetHasFacts;
                        if ((valueTuple.Item2 == null || unitConditionExceptionsTargetHasFacts == null || !unitConditionExceptionsTargetHasFacts.IsExceptional(target)) && blindsightRange < valueTuple.Item1) {
                            blindsightRange = valueTuple.Item1;
                        }
                    }
                    float num = initiator.View.Corpulence + target.View.Corpulence;
                    if (initiator.DistanceTo(target) - num <= blindsightRange.Meters) {
                        __result = Concealment.None;
                        return false;
                    }
                }
                Concealment concealment = (targetConcealmentPart != null && targetConcealmentPart.IsConcealedFor(initiator)) ? Concealment.Total : Concealment.None;
                if (target.Descriptor.State.HasCondition(UnitCondition.Invisible)
                    && !HasValidConditionAgainst(initiator, target, UnitCondition.SeeInvisibility)
                    && !HasValidConditionAgainst(initiator, target, UnitCondition.TrueSeeing)
                ) {
                    concealment = Concealment.Total;
                }
                if (concealment < Concealment.Total && targetConcealmentPart?.m_Concealments != null) {
                    foreach (UnitPartConcealment.ConcealmentEntry concealmentEntry in targetConcealmentPart.m_Concealments) {
                        if (!concealmentEntry.OnlyForAttacks || attack) {
                            if (concealmentEntry.DistanceGreater > 0.Feet()) {
                                float distanceToTarget = initiator.DistanceTo(target);
                                float corpulence = initiator.View.Corpulence + target.View.Corpulence;
                                if (distanceToTarget <= concealmentEntry.DistanceGreater.Meters + corpulence) {
                                    continue;
                                }
                            }
                            if (concealmentEntry.RangeType != null) {
                                RuleAttackRoll ruleAttackRoll = Rulebook.CurrentContext.LastEvent<RuleAttackRoll>();
                                ItemEntityWeapon itemEntityWeapon = (ruleAttackRoll != null) ? ruleAttackRoll.Weapon : initiator.GetFirstWeapon();
                                if (itemEntityWeapon == null || !concealmentEntry.RangeType.Value.IsSuitableWeapon(itemEntityWeapon)) {
                                    continue;
                                }
                            }
                            if ((concealmentEntry.Descriptor == ConcealmentDescriptor.Blur || concealmentEntry.Descriptor == ConcealmentDescriptor.Displacement)
                                    && HasValidConditionAgainst(initiator, target, UnitCondition.TrueSeeing)) {
                                continue;
                            }
                            concealment = UnitPartConcealment.Max(concealment, concealmentEntry.Concealment);
                        }
                    }
                }
                if (targetConcealmentPart != null && targetConcealmentPart.Disable) {
                    concealment = Concealment.None;
                }
                if (initiator.Descriptor.State.HasCondition(UnitCondition.Blindness)) {
                    concealment = Concealment.Total;
                }
                if (concealment == Concealment.None && initiator.Descriptor.State.HasCondition(UnitCondition.PartialConcealmentOnAttacks)) {
                    concealment = Concealment.Partial;
                }
                if (concealment == Concealment.None && Game.Instance.Player.Weather.ActualWeather >= BlueprintRoot.Instance.WeatherSettings.ConcealmentBeginsOn) {
                    RuleAttackRoll ruleAttackRoll2 = Rulebook.CurrentContext.LastEvent<RuleAttackRoll>();
                    ItemEntityWeapon itemEntityWeapon2 = (ruleAttackRoll2 != null) ? ruleAttackRoll2.Weapon : initiator.GetFirstWeapon();
                    if (itemEntityWeapon2 != null && WeaponRangeType.Ranged.IsSuitableWeapon(itemEntityWeapon2)) {
                        concealment = Concealment.Partial;
                    }
                }
                if (initiatorConcealmentPart != null && initiatorConcealmentPart.IgnorePartial && concealment == Concealment.Partial) {
                    concealment = Concealment.None;
                }
                if (initiatorConcealmentPart != null && initiatorConcealmentPart.TreatTotalAsPartial && concealment == Concealment.Total) {
                    concealment = Concealment.Partial;
                }
                __result = concealment;

                return false;
            }

            static bool HasValidConditionAgainst(UnitEntityData initiator, UnitEntityData target, UnitCondition condition) {

                return initiator.Descriptor.State.HasCondition(condition)
                    && !initiator.Descriptor.State.GetConditionExceptions(condition)
                        .EmptyIfNull<UnitConditionExceptions>()
                        .OfType<UnitConditionExceptionsTargetHasFacts>()
                        .Any(exception => exception.IsExceptional(target));
            }
        }
    }
}
