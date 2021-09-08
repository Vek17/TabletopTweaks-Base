using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Clases {
    class Rogue {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Rogue");

                PatchBase();
                PatchEldritchScoundrel();
                PatchRowdy();
            }
            static void PatchBase() {
                PatchTrapfinding();
                PatchRogueTalentSelection();
                PatchSlipperyMind();

                void PatchTrapfinding() {
                    if (ModSettings.Fixes.Rogue.Base.IsDisabled("Trapfinding")) { return; }
                    var Trapfinding = Resources.GetBlueprint<BlueprintFeature>("dbb6b3bffe6db3547b31c3711653838e");
                    Trapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    Trapfinding.SetDescription("A rogue adds 1/2 her level on {g|Encyclopedia:Perception}Perception checks{/g} and {g|Encyclopedia:Trickery}Trickery checks{/g}.");
                    Main.LogPatch("Patched", Trapfinding);
                }
                void PatchRogueTalentSelection() {
                    if (ModSettings.Fixes.Rogue.Base.IsDisabled("RogueTalentSelection")) { return; }
                    var RogueTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    RogueTalentSelection.Mode = SelectionMode.OnlyNew;
                    Main.LogPatch("Patched", RogueTalentSelection);
                }
                void PatchSlipperyMind() {
                    if (ModSettings.Fixes.Rogue.Base.IsDisabled("SlipperyMind")) { return; }
                    var SlipperyMind = Resources.GetBlueprint<BlueprintFeature>("a14e8c1801911334f96d410f10eab7bf");
                    SlipperyMind.AddComponent(Helpers.Create<RecalculateOnStatChange>(c => {
                        c.Stat = StatType.Dexterity;
                    }));
                    Main.LogPatch("Patched", SlipperyMind);
                }
            }
            static void PatchEldritchScoundrel() {
                PatchSneakAttackProgression();
                PatchRogueTalentProgression();

                void PatchSneakAttackProgression() {
                    if (ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].IsDisabled("SneakAttackProgression")) { return; }
                    var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                    var SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                    EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures.AppendToArray(Helpers.LevelEntry(1, SneakAttack));

                    Main.LogPatch("Patched", EldritchScoundrelArchetype);
                }
                void PatchRogueTalentProgression() {
                    if (ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].IsDisabled("RogueTalentProgression")) { return; }
                    var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                    var SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                    var RogueTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    var UncannyDodgeChecker = Resources.GetBlueprint<BlueprintFeature>("8f800ed6ce8c42e8a01fd8f3e990c459");

                    EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures
                        .Where(entry => entry.Level != 4)
                        .AddItem(new LevelEntry() {
                            m_Features = new List<BlueprintFeatureBaseReference>() { RogueTalentSelection.ToReference<BlueprintFeatureBaseReference>() },
                            Level = 2
                        })
                        .AddItem(new LevelEntry() {
                            m_Features = new List<BlueprintFeatureBaseReference>() { UncannyDodgeChecker.ToReference<BlueprintFeatureBaseReference>() },
                            Level = 4
                        }).ToArray();
                    Main.LogPatch("Patched", EldritchScoundrelArchetype);
                }
            }
            static void PatchRowdy() {
                if (ModSettings.Fixes.Rogue.Archetypes["Rowdy"].IsDisabled("VitalForce")) { return; }
                var VitalStrikeAbility = Resources.GetBlueprint<BlueprintAbility>("efc60c91b8e64f244b95c66b270dbd7c");
                var VitalStrikeAbilityImproved = Resources.GetBlueprint<BlueprintAbility>("c714cd636700ac24a91ca3df43326b00");
                var VitalStrikeAbilityGreater = Resources.GetBlueprint<BlueprintAbility>("11f971b6453f74d4594c538e3c88d499");

                AttachRowdyFeature(VitalStrikeAbility);
                AttachRowdyFeature(VitalStrikeAbilityImproved);
                AttachRowdyFeature(VitalStrikeAbilityGreater);

                static void AttachRowdyFeature(BlueprintAbility VitalStrike) {
                    var RowdyVitalDamage = Resources.GetBlueprint<BlueprintFeature>("6ce0dd0cd1ef43eda9e62cdf483e05c3");

                    VitalStrike.GetComponent<AbilityCustomMeleeAttack>().m_RowdyFeature = RowdyVitalDamage.ToReference<BlueprintFeatureReference>();
                    Main.LogPatch("Patched", VitalStrike);
                }
            }
        }
        [HarmonyPatch(typeof(AbilityCustomMeleeAttack.VitalStrike), "OnEventDidTrigger", new Type[] { typeof(RuleCalculateWeaponStats) })]
        static class VitalStrike_OnEventDidTrigger_Rowdy_Patch {

            static bool Prefix(AbilityCustomMeleeAttack.VitalStrike __instance, RuleCalculateWeaponStats evt) {
                if (ModSettings.Fixes.Rogue.Archetypes["Rowdy"].IsDisabled("VitalForce")) { return true; }

                DamageDescription damageDescription = evt.DamageDescription.FirstItem();
                if (damageDescription != null && damageDescription.TypeDescription.Type == DamageType.Physical) {
                    if (ModSettings.Fixes.Feats.Enabled["VitalStrike"] && !ModSettings.Fixes.Feats.DisableAll) {
                        var vitalDamage = new DamageDescription() {
                            Dice = new DiceFormula(damageDescription.Dice.Rolls * Math.Max(1, __instance.m_DamageMod - 1), damageDescription.Dice.Dice),
                            Bonus = __instance.m_Mythic ? damageDescription.Bonus * Math.Max(1, __instance.m_DamageMod - 1) : 0,
                            TypeDescription = damageDescription.TypeDescription,
                            IgnoreReduction = damageDescription.IgnoreReduction,
                            IgnoreImmunities = damageDescription.IgnoreImmunities,
                            SourceFact = damageDescription.SourceFact,
                            CausedByCheckFail = damageDescription.CausedByCheckFail,
                            m_BonusWithSource = 0
                        };
                        evt.DamageDescription.Insert(1, vitalDamage);
                    } else {
                        damageDescription.Dice = new DiceFormula(damageDescription.Dice.Rolls * __instance.m_DamageMod, damageDescription.Dice.Dice);
                        if (__instance.m_Mythic) {
                            damageDescription.Bonus *= __instance.m_DamageMod;
                        }
                    }
                    if (__instance.m_Rowdy) {
                        DamageDescription damageDescription2 = new DamageDescription {
                            TypeDescription = evt.DamageDescription.FirstItem().TypeDescription,
                            Dice = new DiceFormula(evt.Initiator.Descriptor.Stats.SneakAttack * 2, DiceType.D6),

                        };
                        evt.DamageDescription.Add(damageDescription2);
                    }
                }
                return false;
            }
        }
    }
}
