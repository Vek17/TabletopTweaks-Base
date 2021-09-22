using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Tutorial;
using Kingmaker.Tutorial.Solvers;
using Kingmaker.Tutorial.Triggers;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Martial.DamageReduction;
using Kingmaker.UI.ServiceWindow.CharacterScreen;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MechanicsChanges
{
    class DRRework {

        [HarmonyPatch(typeof(AddDamageResistancePhysical), nameof(AddDamageResistancePhysical.IsStackable), MethodType.Getter)]
        static class AddDamageResistancePhysical_IsStackable_Patch {

            static void Postfix(ref bool __result) {
                if (ModSettings.Fixes.DRRework) {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintFact), nameof(BlueprintFact.CollectComponents))]
        static class BlueprintFact_CollectComponents_Patch
        {
            static void Postfix(ref List<BlueprintComponent> __result)
            {
                for (int i = 0; i < __result.Count; i++)
                {
                    BlueprintComponent component = __result[i];
                    if (component is AddDamageResistanceBase resistanceComponent)
                    {
                        TTAddDamageResistanceBase replacementComponent = CreateFromVanillaDamageResistance(resistanceComponent);
                        // https://c.tenor.com/eqLNYv0A9TQAAAAC/swap-indiana-jones.gif
                        __result[i] = replacementComponent;
                        Main.LogDebug("Replaced " + resistanceComponent.GetType().ToString() + " with " + replacementComponent.GetType().ToString());
                    }
                }
            }

            static TTAddDamageResistanceBase CreateFromVanillaDamageResistance(AddDamageResistanceBase vanillaResistance)
            {
                TTAddDamageResistanceBase result = null;
                switch (vanillaResistance)
                {
                    case ResistEnergy:
                        result = Helpers.Create<TTResistEnergy>();
                        break;
                    case ResistEnergyContext:
                        result = Helpers.Create<TTResistEnergyContext>();
                        break;
                    case ProtectionFromEnergy:
                        result = Helpers.Create<TTProtectionFromEnergy>();
                        break;
                    case WizardAbjurationResistance:
                        result = Helpers.Create<TTWizardAbjurationResistance>();
                        break;
                    case WizardEnergyAbsorption:
                        result = Helpers.Create<TTWizardEnergyAbsorption>();
                        break;
                    case AddDamageResistancePhysical:
                        result = Helpers.Create<TTAddDamageResistancePhysical>();
                        break;
                    case AddDamageResistanceEnergy:
                        result = Helpers.Create<TTAddDamageResistanceEnergy>();
                        break;
                    case AddDamageResistanceForce:
                        result = Helpers.Create<TTAddDamageResistanceForce>();
                        break;
                    default:
                        Main.Log("ERROR: Called CreateFromVanillaDamageResistance for unsupported type: " + vanillaResistance.GetType().ToString());
                        return null;
                }

                result.InitFromVanillaDamageResistance(vanillaResistance);
                return result;
            }
        }

        [HarmonyPatch(typeof(ReduceDamageReduction), nameof(ReduceDamageReduction.OnTurnOn))]
        static class ReduceDamageReduction_OnTurnOn_Patch
        {
            static bool Prefix(ReduceDamageReduction __instance)
            {
                int penalty = __instance.Value.Calculate(__instance.Context) * __instance.Multiplier;
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().AddPenaltyEntry(penalty, __instance.Fact);
                return false;
            }
        }

        [HarmonyPatch(typeof(ReduceDamageReduction), nameof(ReduceDamageReduction.OnTurnOff))]
        static class ReduceDamageReduction_OnTurnOff_Patch
        {
            static bool Prefix(ReduceDamageReduction __instance)
            {
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().RemovePenaltyEntry(__instance.Fact);
                return false;
            }
        }

        [HarmonyPatch(typeof(CharInfoDamageReductionVM), nameof(CharInfoDamageReductionVM.GetDamageReduction))]
        static class CharInfoDamageReductionVM_GetDamageReduction_Patch
        {
            static void Postfix(CharInfoDamageReductionVM __instance, UnitDescriptor unit, ref List<CharInfoDamageReductionEntryVM> __result)
            {
                List<CharInfoDamageReductionEntryVM> reductionEntryVmList = new List<CharInfoDamageReductionEntryVM>();
                IEnumerable<TTAddDamageResistanceBase.ComponentRuntime> allSources = unit.Get<TTUnitPartDamageReduction>()?.AllSources;
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                int num = 0;
                foreach(TTAddDamageResistanceBase.ComponentRuntime componentRuntime in allSources.EmptyIfNull())
                {
                    if (componentRuntime.Settings is TTAddDamageResistancePhysical settings1)
                    {
                        if (settings1.IsStackable)
                        {
                            num += componentRuntime.GetValue();
                        }
                        else
                        {
                            CharInfoDamageReductionEntryVM reductionEntryVm = new CharInfoDamageReductionEntryVM()
                            {
                                Value = componentRuntime.GetValue().ToString()
                            };
                            if (settings1.BypassedByAlignment)
                                reductionEntryVm.Exceptions.Add(ls.DamageAlignment.GetTextFlags(settings1.Alignment));
                            if (settings1.BypassedByForm)
                                reductionEntryVm.Exceptions.AddRange(settings1.Form.Components().Select<PhysicalDamageForm, string>((Func<PhysicalDamageForm, string>)(f => ls.DamageForm.GetText(f))));
                            if (settings1.BypassedByMagic)
                                reductionEntryVm.Exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MagicDRDescriptor);
                            if (settings1.BypassedByMaterial)
                                reductionEntryVm.Exceptions.Add(ls.DamageMaterial.GetTextFlags(settings1.Material));
                            if (settings1.BypassedByReality)
                                reductionEntryVm.Exceptions.Add(ls.DamageReality.GetText(settings1.Reality));
                            if (settings1.BypassedByMeleeWeapon)
                                reductionEntryVm.Exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MeleeDRDescriptor);
                            if (settings1.BypassedByWeaponType)
                                reductionEntryVm.Exceptions.Add((string)settings1.WeaponType.TypeName);
                            if (reductionEntryVm.Exceptions.Count == 0)
                                reductionEntryVm.Exceptions.Add("-");
                            reductionEntryVmList.Add(reductionEntryVm);
                        }
                    }
                }
                if (num > 0)
                {
                    CharInfoDamageReductionEntryVM reductionEntryVm = new CharInfoDamageReductionEntryVM()
                    {
                        Value = num.ToString()
                    };
                    reductionEntryVm.Exceptions.Add("-");
                    reductionEntryVmList.Add(reductionEntryVm);
                }
                __result = reductionEntryVmList;
            }
        }

        [HarmonyPatch(typeof(CharSMartial), nameof(CharSMartial.GetDamageReduction))]
        static class CharSMartial_GetDamageReduction_Patch
        {
            static void Postfix(CharSMartial __instance, UnitDescriptor unit, ref List<CharSMartial.DRdata> __result)
            {
                List<CharSMartial.DRdata> drdataList = new List<CharSMartial.DRdata>();
                TTUnitPartDamageReduction partDamageReduction = unit.Get<TTUnitPartDamageReduction>();
                IEnumerable<TTAddDamageResistanceBase.ComponentRuntime> list = partDamageReduction != null ? partDamageReduction.AllSources.Where(c => c.Settings is TTAddDamageResistancePhysical) : null;
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                int num = 0;
                foreach (TTAddDamageResistanceBase.ComponentRuntime componentRuntime in list.EmptyIfNull())
                {
                    TTAddDamageResistancePhysical settings = (TTAddDamageResistancePhysical)componentRuntime.Settings;
                    if (settings.IsStackable)
                    {
                        num += componentRuntime.GetCurrentValue();
                    }
                    else
                    {
                        CharSMartial.DRdata drdata = new CharSMartial.DRdata();
                        drdata.value = componentRuntime.GetCurrentValue().ToString();
                        if (settings.BypassedByAlignment)
                            drdata.exceptions.Add(ls.DamageAlignment.GetTextFlags(settings.Alignment));
                        if (settings.BypassedByForm)
                            drdata.exceptions.AddRange(settings.Form.Components().Select<PhysicalDamageForm, string>((Func<PhysicalDamageForm, string>)(f => ls.DamageForm.GetText(f))));
                        if (settings.BypassedByMagic)
                            drdata.exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MagicDRDescriptor);
                        if (settings.BypassedByMaterial)
                            drdata.exceptions.Add(ls.DamageMaterial.GetTextFlags(settings.Material));
                        if (settings.BypassedByReality)
                            drdata.exceptions.Add(ls.DamageReality.GetText(settings.Reality));
                        if (settings.BypassedByMeleeWeapon)
                            drdata.exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MeleeDRDescriptor);
                        if (settings.BypassedByWeaponType)
                            drdata.exceptions.Add((string)settings.WeaponType.TypeName);
                        if (drdata.exceptions.Count == 0)
                            drdata.exceptions.Add("-");
                        drdataList.Add(drdata);
                    }
                }
                if (num > 0)
                {
                    CharSMartial.DRdata drdata = new CharSMartial.DRdata()
                    {
                        value = num.ToString()
                    };
                    drdata.exceptions.Add("-");
                    drdataList.Add(drdata);
                }
                __result = drdataList;
            }
        }

        [HarmonyPatch(typeof(TutorialTriggerDamageReduction), nameof(TutorialTriggerDamageReduction.ShouldTrigger))]
        static class TutorialTriggerDamageReduction_ShouldTrigger_Patch
        {
            static void Postfix(TutorialTriggerDamageReduction __instance, RuleDealDamage rule, ref bool __result)
            {
                if (!__result && !rule.IgnoreDamageReduction)
                {
                    TTUnitPartDamageReduction partDamageReduction = rule.Target.Get<TTUnitPartDamageReduction>();
                    if (partDamageReduction != null && rule.ResultList != null && __instance.AbsoluteDR == partDamageReduction.HasAbsolutePhysicalDR)
                    {
                        foreach(DamageValue res in rule.ResultList)
                        {
                            if (res.Source is PhysicalDamage && res.Reduction > 0)
                            {
                                __result = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AddEnergyImmunity), nameof(AddEnergyImmunity.OnTurnOn))]
        static class AddEnergyImmunity_OnTurnOn_Patch
        {
            static bool Prefix(AddEnergyImmunity __instance)
            {
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().AddImmunity(__instance.Fact, __instance, __instance.Type);
                return false;
            }
        }

        [HarmonyPatch(typeof(AddEnergyImmunity), nameof(AddEnergyImmunity.OnTurnOff))]
        static class AddEnergyImmunity_OnTurnOff_Patch
        {
            static bool Prefix(AddEnergyImmunity __instance)
            {
                __instance.Owner.Get<TTUnitPartDamageReduction>()?.RemoveImmunity(__instance.Fact, __instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(TutorialSolverSpellWithDamage), nameof(TutorialSolverSpellWithDamage.GetBasePriority))]
        static class TutorialSolverSpellWithDamage_GetBasePriority_Patch
        {
            static void Postfix(TutorialSolverSpellWithDamage __instance, BlueprintAbility ability, UnitEntityData caster, ref int __result)
            {
                if (__result != -1)
                {
                    TTUnitPartDamageReduction partDamageReduction = ContextData<TutorialContext>.Current.TargetUnit.Get<TTUnitPartDamageReduction>();
                    if (partDamageReduction != null)
                    {
                        foreach(Element elements in ability.ElementsArray)
                        {
                            if (elements is ContextActionDealDamage actionDealDamage)
                            {
                                if (actionDealDamage.DamageType.Type == DamageType.Energy && partDamageReduction.IsImmune(actionDealDamage.DamageType.Energy))
                                {
                                    __result = -1;
                                    return;
                                }
                                BaseDamage damage = actionDealDamage.DamageType.CreateDamage(DiceFormula.Zero, 0);
                                if (!partDamageReduction.CanBypass(damage, null))
                                {
                                    __result = -1;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

#if false
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (!ModSettings.Fixes.DisableDRStacking) { return; }
                Main.LogHeader("Patching DR instances that should stack");

                //PatchStalwartDefenderDR();
            }

            static void PatchStalwartDefenderDR() {
                BlueprintCharacterClass stalwartDefenderClass = Resources.GetBlueprint<BlueprintCharacterClass>("d5917881586ff1d4d96d5b7cebda9464");
                BlueprintProgression stalwartDefenderProgression = stalwartDefenderClass.m_Progression.Get();

                BlueprintUnitProperty stalwartDefenderHiddenDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("SDHiddenDRProperty", bp => {
                    bp.AddComponent(Helpers.Create<StalwartDefenderDRProperty>());
                });

                /*BlueprintBuff stalwartDefenderDRBuff = Helpers.CreateBuff("SDHiddenDRBuff", bp => {
                    bp.IsClassFeature = true;
                    bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                    bp.AddComponent(Helpers.Create<AddFacts>(c => {
                        Helpers.Create<ContextRankConfig>(crc => {
                            crc.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                            crc.m_CustomProperty = stalwartDefenderHiddenDRProperty.ToReference<BlueprintUnitPropertyReference>();
                        });
                    }));
                });*/

                BlueprintFeature stalwartDefenderDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("4d4f48f401d5d8b408c2e7a973fba9ea");

                BlueprintFeature stalwartDefenderHiddenDRFeature = Helpers.CreateCopy(stalwartDefenderDamageReductionFeature, bp => {
                    bp.name = "StalwartDefenderStackingDRFeature";
                    bp.AssetGuid = ModSettings.Blueprints.GetGUID("StalwartDefenderStackingDRFeature");
                    bp.HideInUI = true;
                    bp.Ranks = 1;
                    bp.ReapplyOnLevelUp = true;
                    bp.SetName("Stalwart Defender Hidden Stacking DR Calculation");
                    bp.SetDescription("This should be hidden in the UI");
                    bp.RemoveComponents<ContextRankConfig>();
                    bp.AddComponent(Helpers.Create<ContextRankConfig>(crc => {
                        crc.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        crc.m_CustomProperty = stalwartDefenderHiddenDRProperty.ToReference<BlueprintUnitPropertyReference>();
                    }));
                    bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(rofc => {
                        rofc.m_CheckedFacts = new BlueprintUnitFactReference[] {
                            Resources.GetBlueprint<BlueprintFeature>("4d4f48f401d5d8b408c2e7a973fba9ea").ToReference<BlueprintUnitFactReference>(),
                            Resources.GetBlueprint<BlueprintFeature>("dbbf704bfcc78854ab149597ef9aae7c").ToReference<BlueprintUnitFactReference>()
                        };
                    }));
                });

                /*stalwartDefenderDamageReductionFeature.RemoveComponents<ContextRankConfig>();
                stalwartDefenderDamageReductionFeature.RemoveComponents<AddDamageResistancePhysical>();*/

                Resources.AddBlueprint(stalwartDefenderHiddenDRFeature);

                stalwartDefenderProgression.LevelEntries.Where(le => le.Level == 5).First().m_Features.Add(
                    stalwartDefenderHiddenDRFeature.ToReference<BlueprintFeatureBaseReference>()
                );
                Main.LogPatch("Patched", stalwartDefenderProgression);
            }
        }

        public class StalwartDefenderDRProperty : PropertyValueGetter {

            internal static readonly BlueprintFeature StalwartDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("4d4f48f401d5d8b408c2e7a973fba9ea");

            private static readonly BlueprintFeature AdamantineArmorLightFeature = Resources.GetBlueprint<BlueprintFeature>("e93a376547629e2478d6f50e5f162efb");
            private static readonly BlueprintFeature AdamantineArmorMediumFeature = Resources.GetBlueprint<BlueprintFeature>("74a80c42774045f4d916dc0d990b7738");
            private static readonly BlueprintFeature AdamantineArmorHeavyFeature = Resources.GetBlueprint<BlueprintFeature>("dbbf704bfcc78854ab149597ef9aae7c");
            private static readonly BlueprintFeature ArmorOfWealthFeature = Resources.GetBlueprint<BlueprintFeature>("b99c50dd771a36d4f913bf1f56ba77a2");
            private static readonly BlueprintBuff BeaconOfCarnageEffectBuff = Resources.GetBlueprint<BlueprintBuff>("42ab909d597f1734cb9bf65a74db7424");
            private static readonly BlueprintFeature BeaconOfCarnageFeature = Resources.GetBlueprint<BlueprintFeature>("a8ea2027afa333246a86b8085c23fbfd");
            private static readonly BlueprintFeature ColorlessRemainsBreastplate_SolidFeature = Resources.GetBlueprint<BlueprintFeature>("06d2f00616ad40c3b136d06dffc8f0b5");
            private static readonly BlueprintFeature ForMounted_HalfplateOfSynergyFeature = Resources.GetBlueprint<BlueprintFeature>("ff2d26e87b5f2bc4ba1823e242f10890");
            private static readonly BlueprintBuff RealmProtectorCountBuff = Resources.GetBlueprint<BlueprintBuff>("e48413be052ec004ca7f1c8d4fa4a008");
            private static readonly BlueprintFeature StuddedArmorOfTrinityFeature = Resources.GetBlueprint<BlueprintFeature>("79babe38a7306ba4c81f2fa3c88d1bae");

            internal static Dictionary<BlueprintUnitFactReference, int> StalwartDefenderStackingDRFactsRankMultiplier = new Dictionary<BlueprintUnitFactReference, int>
            {
                { AdamantineArmorLightFeature.ToReference<BlueprintUnitFactReference>(), 1 },
                { AdamantineArmorMediumFeature.ToReference<BlueprintUnitFactReference>(), 2 },
                { AdamantineArmorHeavyFeature.ToReference<BlueprintUnitFactReference>(), 3 },
                { ArmorOfWealthFeature.ToReference<BlueprintUnitFactReference>(), 5 },
                { BeaconOfCarnageEffectBuff.ToReference<BlueprintUnitFactReference>(), 1 },
                { BeaconOfCarnageFeature.ToReference<BlueprintUnitFactReference>(), 2 },
                { ColorlessRemainsBreastplate_SolidFeature.ToReference<BlueprintUnitFactReference>(), 3 },
                { ForMounted_HalfplateOfSynergyFeature.ToReference<BlueprintUnitFactReference>(), 1 },
                { RealmProtectorCountBuff.ToReference<BlueprintUnitFactReference>(), 2 },
                { StuddedArmorOfTrinityFeature.ToReference<BlueprintUnitFactReference>(), 3 },

            };


            public override int GetBaseValue(UnitEntityData unit) {
                int num = 0;
                // Stalwart Defender Base DR
                /*EntityFact stalwartDefenderDRFact = unit.Descriptor.GetFact(StalwartDamageReductionFeature.ToReference<BlueprintFeatureReference>());
                if (stalwartDefenderDRFact != null) {
                    // 1, 3, 5
                    // num += 1 + (stalwartDefenderDRFact.GetRank() - 1) * 2;
                    num += ((AddDamageResistancePhysical.ComponentRuntime)stalwartDefenderDRFact.GetComponentWithRuntime<AddDamageResistancePhysical>().Runtime).GetCurrentValue();
                }
                // DR from armor
                EntityFact adamantineArmorHeavyFact = unit.Descriptor.GetFact(AdamantineArmorHeavyFeature.ToReference<BlueprintFeatureReference>());
                if (adamantineArmorHeavyFact != null) {
                    Main.LogDebug("Adding +3 to stalwart defender DR/- because of heavy adamantine armor");
                    num += ((AddDamageResistancePhysical.ComponentRuntime)adamantineArmorHeavyFact.GetComponentWithRuntime<AddDamageResistancePhysical>().Runtime).GetCurrentValue();
                }*/
                EntityFact stalwartDefenderDRFact = unit.Descriptor.GetFact(StalwartDamageReductionFeature.ToReference<BlueprintUnitFactReference>());
                EntityFact adamantineArmorHeavyFact = unit.Descriptor.GetFact(AdamantineArmorHeavyFeature.ToReference<BlueprintUnitFactReference>());
                List<EntityFact> list = new List<EntityFact>
                {
                    stalwartDefenderDRFact,
                    adamantineArmorHeavyFact
                }.Where(x => x != null).ToList();
                foreach (AddDamageResistanceBase.ComponentRuntime dr in unit.Ensure<UnitPartDamageReduction>().AllSources)
                {
                    if (list.Contains(dr.Fact))
                    {
                        num += dr.GetCurrentValue();
                    }
                }
                return num;
            }
        }
#endif
    }
}
