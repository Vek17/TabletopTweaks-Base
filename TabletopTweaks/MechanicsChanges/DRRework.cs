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
                if (ModSettings.Fixes.DRRework.IsEnabled("Base")) {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintFact), nameof(BlueprintFact.CollectComponents))]
        static class BlueprintFact_CollectComponents_Patch
        {
            static void Postfix(ref List<BlueprintComponent> __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }

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
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
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
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().RemovePenaltyEntry(__instance.Fact);
                return false;
            }
        }

        [HarmonyPatch(typeof(CharInfoDamageReductionVM), nameof(CharInfoDamageReductionVM.GetDamageReduction))]
        static class CharInfoDamageReductionVM_GetDamageReduction_Patch
        {
            static void Postfix(CharInfoDamageReductionVM __instance, UnitDescriptor unit, ref List<CharInfoDamageReductionEntryVM> __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                List<CharInfoDamageReductionEntryVM> reductionEntryVmList = new List<CharInfoDamageReductionEntryVM>();
                IEnumerable<TTUnitPartDamageReduction.ReductionDisplay> allSources = unit.Get<TTUnitPartDamageReduction>()?.AllSources;
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                foreach(TTUnitPartDamageReduction.ReductionDisplay reduction in allSources.EmptyIfNull())
                {
                    if (reduction.ReferenceRuntime.Settings is TTAddDamageResistancePhysical settings1)
                    {
                        CharInfoDamageReductionEntryVM reductionEntryVm = new CharInfoDamageReductionEntryVM()
                        {
                            Value = reduction.TotalReduction.ToString()
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
                __result = reductionEntryVmList;
            }
        }

        [HarmonyPatch(typeof(CharSMartial), nameof(CharSMartial.GetDamageReduction))]
        static class CharSMartial_GetDamageReduction_Patch
        {
            static void Postfix(CharSMartial __instance, UnitDescriptor unit, ref List<CharSMartial.DRdata> __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                List<CharSMartial.DRdata> drdataList = new List<CharSMartial.DRdata>();
                TTUnitPartDamageReduction partDamageReduction = unit.Get<TTUnitPartDamageReduction>();
                IEnumerable<TTUnitPartDamageReduction.ReductionDisplay> list = partDamageReduction != null ? partDamageReduction.AllSources.Where(c => c.ReferenceRuntime.Settings is TTAddDamageResistancePhysical) : null;
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                foreach (TTUnitPartDamageReduction.ReductionDisplay reduction in list.EmptyIfNull())
                {
                    TTAddDamageResistancePhysical settings = (TTAddDamageResistancePhysical)reduction.ReferenceRuntime.Settings;

                    CharSMartial.DRdata drdata = new CharSMartial.DRdata();
                    drdata.value = reduction.TotalReduction.ToString();
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
                __result = drdataList;
            }
        }

        [HarmonyPatch(typeof(TutorialTriggerDamageReduction), nameof(TutorialTriggerDamageReduction.ShouldTrigger))]
        static class TutorialTriggerDamageReduction_ShouldTrigger_Patch
        {
            static void Postfix(TutorialTriggerDamageReduction __instance, RuleDealDamage rule, ref bool __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
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
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().AddImmunity(__instance.Fact, __instance, __instance.Type);
                return false;
            }
        }

        [HarmonyPatch(typeof(AddEnergyImmunity), nameof(AddEnergyImmunity.OnTurnOff))]
        static class AddEnergyImmunity_OnTurnOff_Patch
        {
            static bool Prefix(AddEnergyImmunity __instance)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                __instance.Owner.Get<TTUnitPartDamageReduction>()?.RemoveImmunity(__instance.Fact, __instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(TutorialSolverSpellWithDamage), nameof(TutorialSolverSpellWithDamage.GetBasePriority))]
        static class TutorialSolverSpellWithDamage_GetBasePriority_Patch
        {
            static void Postfix(TutorialSolverSpellWithDamage __instance, BlueprintAbility ability, UnitEntityData caster, ref int __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
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

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                Main.LogHeader("Patching Blueprints for DR Rework");

                PatchArmorDR();
                PatchStalwartDefender();
                PatchBarbariansDR();
            }

            static void PatchArmorDR()
            {
                BlueprintUnitFact[] armorFactsWithPhysicalDR = new BlueprintUnitFact[]
                {
                    Resources.GetBlueprint<BlueprintFeature>("e93a376547629e2478d6f50e5f162efb"), // AdamantineArmorLightFeature
                    Resources.GetBlueprint<BlueprintFeature>("74a80c42774045f4d916dc0d990b7738"), // AdamantineArmorMediumFeature
                    Resources.GetBlueprint<BlueprintFeature>("dbbf704bfcc78854ab149597ef9aae7c"), // AdamantineArmorHeavyFeature
                    Resources.GetBlueprint<BlueprintFeature>("b99c50dd771a36d4f913bf1f56ba77a2"), // ArmorOfWealthFeature
                    Resources.GetBlueprint<BlueprintFeature>("a8ea2027afa333246a86b8085c23fbfd"), // BeaconOfCarnageFeature
                    Resources.GetBlueprint<BlueprintBuff>("42ab909d597f1734cb9bf65a74db7424"),    // BeaconOfCarnageEffectBuff
                    Resources.GetBlueprint<BlueprintFeature>("06d2f00616ad40c3b136d06dffc8f0b5"), // ColorlessRemainsBreastplate_SolidFeature
                    Resources.GetBlueprint<BlueprintFeature>("ff2d26e87b5f2bc4ba1823e242f10890"), // ForMounted_HalfplateOfSynergyFeature
                    Resources.GetBlueprint<BlueprintFeature>("e19008b823a221043b9184ef3c271db1"), // RealmProtectorFeature
                    Resources.GetBlueprint<BlueprintFeature>("79babe38a7306ba4c81f2fa3c88d1bae")  // StuddedArmorOfTrinityFeature
                };

                foreach (BlueprintUnitFact armorBlueprint in armorFactsWithPhysicalDR)
                {
                    armorBlueprint.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                    {
                        newRes.SourceIsArmor = true;
                    });
                }
            }

            static void PatchBarbariansDR()
            {
                BlueprintFeature barbarianDR = Resources.GetBlueprint<BlueprintFeature>("cffb5cddefab30140ac133699d52a8f8");
                BlueprintFeature invulnerableRagerDR = Resources.GetBlueprint<BlueprintFeature>("e71bd204a2579b1438ebdfbf75aeefae");
                BlueprintFeature madDogMasterDamageReduction = Resources.GetBlueprint<BlueprintFeature>("a0d4a3295224b8f4387464a4447c31d5");
                BlueprintFeature madDogPetDamageReduction = Resources.GetBlueprint<BlueprintFeature>("2edbf059fd033974bbff67960f15974d");

                BlueprintFeature bloodragerDR = Resources.GetBlueprint<BlueprintFeature>("07eba4bb72c2e3845bb442dce85d3b58");

                BlueprintFeature skaldDR = Resources.GetBlueprint<BlueprintFeature>("d9446a35d1401cf418bb9b5e0e199d57");

                BlueprintFeature increasedDamageReductionRagePower = Resources.GetBlueprint<BlueprintFeature>("ddaee203ee4dcb24c880d633fbd77db6");

                BlueprintBuff manglingFrenzyBuff = Resources.GetBlueprint<BlueprintBuff>("1581c5ceea24418cadc9f26ce4d391a9");

                barbarianDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                invulnerableRagerDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                madDogMasterDamageReduction.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                bloodragerDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                skaldDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                // Fix Skald DR not increasing with Increased Damage ResistanCce Rage Power
                ContextRankConfig barbarianDRConfig = barbarianDR.GetComponent<ContextRankConfig>();
                ContextRankConfig skaldDRRankConfig = Helpers.CreateCopy(barbarianDRConfig, crc =>
                {
                    crc.m_FeatureList = new BlueprintFeatureReference[]
                    {
                        skaldDR.ToReference<BlueprintFeatureReference>(),
                        increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>(),
                        increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>()
                    };
                });

                skaldDR.RemoveComponents<ContextRankConfig>();
                skaldDR.AddComponent(skaldDRRankConfig);

                Main.Log($"Patched: ContextRankConfig on {skaldDR.AssetGuid} - {skaldDR.NameSafe()}");

                // Allow Mangling Frenzy to stack with Barbarian DR's
                manglingFrenzyBuff.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.StacksWithFacts = new BlueprintUnitFactReference[]
                    {
                        barbarianDR.ToReference<BlueprintUnitFactReference>(),
                        invulnerableRagerDR.ToReference<BlueprintUnitFactReference>(),
                        madDogMasterDamageReduction.ToReference<BlueprintUnitFactReference>(),
                        madDogPetDamageReduction.ToReference<BlueprintUnitFactReference>(),
                        bloodragerDR.ToReference<BlueprintUnitFactReference>(),
                        skaldDR.ToReference<BlueprintUnitFactReference>()
                    };
                });

                // Fix Bloodrager (Primalist) DR not being increased by the Improved Damage Reduction rage power
                ContextRankConfig bloodRagerDRContextRankConfig = bloodragerDR.GetComponent<ContextRankConfig>();
                bloodRagerDRContextRankConfig.m_FeatureList = bloodRagerDRContextRankConfig.m_FeatureList.AddRangeToArray(new BlueprintFeatureReference[]
                {
                    increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>(),
                    increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>()
                });

                // Fix Mad Dog's pet DR not being improved by master's Increased Damage Resistance Rage Power(s)
                BlueprintUnitProperty madDogPetDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("MadDogPetDRProperty", bp =>
                {
                    bp.AddComponent(Helpers.Create<MadDogPetDRProperty>());
                });

                madDogPetDamageReduction.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                madDogPetDamageReduction.RemoveComponents<ContextRankConfig>();
                madDogPetDamageReduction.AddComponent(Helpers.Create<ContextRankConfig>(crc =>
                {
                    crc.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    crc.m_CustomProperty = madDogPetDRProperty.ToReference<BlueprintUnitPropertyReference>();
                }));

                // Fix Increased Damage Reduction Rage Power not checking if the character actual has the DamageReduction class feature
                increasedDamageReductionRagePower.AddComponent<PrerequisiteFeaturesFromListFormatted>(p =>
                {
                    p.m_Features = new BlueprintFeatureReference[]
                    {
                        barbarianDR.ToReference<BlueprintFeatureReference>(),
                        invulnerableRagerDR.ToReference<BlueprintFeatureReference>(),
                        madDogMasterDamageReduction.ToReference<BlueprintFeatureReference>(),
                        bloodragerDR.ToReference<BlueprintFeatureReference>(),
                        skaldDR.ToReference<BlueprintFeatureReference>()
                    };
                    p.Amount = 1;
                });
            }

            static void PatchStalwartDefender()
            {
                BlueprintFeature stalwartDefenderDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("4d4f48f401d5d8b408c2e7a973fba9ea");

                stalwartDefenderDamageReductionFeature.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                    newRes.IsIncreasedByArmor = true;
                });

                BlueprintFeature increasedDamageReductionDefensivePower = Resources.GetBlueprint<BlueprintFeature>("d10496e92d0799a40bb3930b8f4fda0d");

                increasedDamageReductionDefensivePower.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                    newRes.IncreasesFacts = new BlueprintUnitFactReference[]
                    {
                        stalwartDefenderDamageReductionFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            }
        }

        public class MadDogPetDRProperty : PropertyValueGetter
        {
            private static BlueprintFeature MadDogMasterDamageReduction = Resources.GetBlueprint<BlueprintFeature>("a0d4a3295224b8f4387464a4447c31d5");

            public override int GetBaseValue(UnitEntityData unit)
            {
                if (!unit.IsPet || unit.Master == null)
                    return 0;

                int value = 0;
                EntityFact masterDamageReduction = unit.Master.GetFact(MadDogMasterDamageReduction);
                if (masterDamageReduction != null)
                {
                    foreach (BlueprintComponentAndRuntime<TTAddDamageResistancePhysical> componentAndRuntime in masterDamageReduction.SelectComponentsWithRuntime<TTAddDamageResistancePhysical>())
                    {
                        value += ((TTAddDamageResistancePhysical.ComponentRuntime)componentAndRuntime.Runtime).GetCurrentValue();
                    }
                }

                return value;
            }
        }
    }
}
