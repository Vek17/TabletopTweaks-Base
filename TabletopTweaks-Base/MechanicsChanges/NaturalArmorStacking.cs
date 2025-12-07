using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Base.MechanicsChanges {
    internal static class NaturalArmorStacking {
        [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
        static class ModifierDescriptorHelper_IsStackable_Patch {

            static void Postfix(ref bool __result, ModifierDescriptor descriptor) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("DisableNaturalArmorStacking")) { return; }
                if (descriptor == ModifierDescriptor.NaturalArmor) {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(Polymorph), nameof(Polymorph.OnActivate))]
        static class Polymorphr_OnActivate_NaturalArmorStacking_Patch {
            static readonly MethodInfo Modifier_AddModifier = AccessTools.Method(typeof(ModifiableValue), "AddModifier", new Type[] {
                typeof(int),
                typeof(EntityFactComponent),
                typeof(ModifierDescriptor)
            });
            //Change bonus descriptor to NaturalArmor.Bonus instead of ModifierDescriptor.NaturalArmorForm
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("DisableNaturalArmorStacking")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Ldc_I4, (int)NaturalArmor.Bonus);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    //Find the modifier using Natural Armor Form
                    if (codes[i].OperandIs((int)ModifierDescriptor.NaturalArmorForm)
                        && codes[i + 1].Calls(Modifier_AddModifier)) {
                        return i;
                    }
                }
                TTTContext.Logger.Log("POLYMORPH PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }

        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("DisableNaturalArmorStacking")) { return; }
                TTTContext.Logger.LogHeader("Patching NaturalArmor");
                PatchNaturalArmorEffects();

            }
            static void PatchNaturalArmorEffects() {
                PatchAnimalCompanionFeatures();
                PatchItemBuffs();
                PatchSpellBuffs();
                PatchClassFeatures();
                PatchFeats();
                PatchWildShape();

                void PatchAnimalCompanionFeatures() {
                    var AnimalCompanionSelectionBase = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("90406c575576aee40a34917a1b429254");
                    var AnimalCompanionFeatureHorse_PreorderBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");
                    var GhostRiderSpiritedMountPetFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("362167dc391341ce80e402501ac893db");
                    var AnimalCompanionNaturalArmor = BlueprintTools.GetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                    var AnimalCompanionStatFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                    IEnumerable<BlueprintFeature> AnimalCompanionUpgrades = AnimalCompanionSelectionBase.m_AllFeatures.Concat(AnimalCompanionSelectionBase.m_Features)
                        .Select(feature => feature.Get())
                        .AddItem(AnimalCompanionFeatureHorse_PreorderBonus)
                        .AddItem(GhostRiderSpiritedMountPetFeature)
                        .Where(feature => feature.GetComponent<AddPet>())
                        .Select(feature => feature.GetComponent<AddPet>())
                        .Where(component => component.m_UpgradeFeature != null)
                        .Select(component => component.m_UpgradeFeature.Get())
                        .Where(feature => feature != null)
                        .Distinct();
                    AnimalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    TTTContext.Logger.LogPatch("Patched", AnimalCompanionNaturalArmor);
                    AnimalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                        .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Stackable);
                    TTTContext.Logger.LogPatch("Patched", AnimalCompanionStatFeature);
                    AnimalCompanionUpgrades.ForEach(bp => {
                        bp.GetComponents<AddStatBonus>()
                            .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                            .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Stackable);
                        TTTContext.Logger.LogPatch("Patched", bp);
                    });
                }
                void PatchClassFeatures() {
                    BlueprintFeature DragonDiscipleNaturalArmor = BlueprintTools.GetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
                    DragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    TTTContext.Logger.LogPatch("Patched", DragonDiscipleNaturalArmor);
                }
                void PatchFeats() {
                    BlueprintFeature ImprovedNaturalArmor = BlueprintTools.GetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");
                    ImprovedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    TTTContext.Logger.LogPatch("Patched", ImprovedNaturalArmor);
                }
                void PatchItemBuffs() {
                    //Icy Protector
                    var ProtectionOfColdEnchantment = BlueprintTools.GetBlueprint<BlueprintEquipmentEnchantment>("789af679bec72a647afd0e4c956e542a");
                    var ProtectionOfColdFeature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("75e471cd478242c4bab2a170c1851f46");
                    var ShamanHexIceplantFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("999e367bbabc42d46bf864d3f4bda86d");
                    var WitchHexIceplantFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9af6dc2697c8ef24eab8ce1c697b05c4");

                    ProtectionOfColdEnchantment.RemoveComponents<AddFactContextActions>();
                    ShamanHexIceplantFeature.ReapplyOnLevelUp = true;
                    ShamanHexIceplantFeature.RemoveComponents<AddFactContextActions>();
                    ShamanHexIceplantFeature.GetComponent<AddContextStatBonus>().TemporaryContext(c => {
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                        c.Multiplier = 2;
                    });
                    ShamanHexIceplantFeature.AddComponent<RecalculateOnOwnerFactUpdated>(c => {
                        c.m_Fact = ProtectionOfColdFeature;
                    });
                    ShamanHexIceplantFeature.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                        c.m_Feature = ProtectionOfColdFeature.Get().ToReference<BlueprintFeatureReference>();
                        c.m_Progression = ContextRankProgression.StartPlusDivStep;
                        c.m_StepLevel = 1;
                    });
                    WitchHexIceplantFeature.ReapplyOnLevelUp = true;
                    WitchHexIceplantFeature.RemoveComponents<AddFactContextActions>();
                    WitchHexIceplantFeature.GetComponent<AddContextStatBonus>().TemporaryContext(c => {
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                        c.Multiplier = 2;
                    });
                    WitchHexIceplantFeature.AddComponent<RecalculateOnOwnerFactUpdated>(c => {
                        c.m_Fact = ProtectionOfColdFeature;
                    });
                    WitchHexIceplantFeature.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                        c.m_Feature = ProtectionOfColdFeature.Get().ToReference<BlueprintFeatureReference>();
                        c.m_Progression = ContextRankProgression.StartPlusDivStep;
                        c.m_StepLevel = 1;
                    });

                    TTTContext.Logger.LogPatch("Patched", ShamanHexIceplantFeature);
                    TTTContext.Logger.LogPatch("Patched", WitchHexIceplantFeature);
                }
                void PatchSpellBuffs() {
                    var buffComponents = SpellTools.SpellList.AllSpellLists
                        .SelectMany(list => list.SpellsByLevel)
                        .SelectMany(level => level.Spells)
                        .SelectMany(spell => spell.AbilityAndVariants())
                        .Distinct()
                        .SelectMany(spell => spell.FlattenAllActions())
                        .OfType<ContextActionApplyBuff>()
                        .Select(action => action.Buff)
                        .SelectMany(buff => buff.ComponentsArray)
                        .ToArray();
                    buffComponents
                        .OfType<AddContextStatBonus>()
                        .Where(component => component.Descriptor == ModifierDescriptor.NaturalArmorForm)
                        .ForEach(component => {
                            component.Descriptor = (ModifierDescriptor)NaturalArmor.Bonus;
                            TTTContext.Logger.LogPatch($"Patched", component.OwnerBlueprint);
                        });
                    buffComponents
                        .OfType<AddStatBonus>()
                        .Where(component => component.Descriptor == ModifierDescriptor.NaturalArmorForm)
                        .ForEach(component => {
                            component.Descriptor = (ModifierDescriptor)NaturalArmor.Bonus;
                            TTTContext.Logger.LogPatch($"Patched", component.OwnerBlueprint);
                        });
                    var AnimalGrowthBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("3fca5d38053677044a7ffd9a872d3a0a");
                    var LegendaryProportionsBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4ce640f9800d444418779a214598d0a3");
                    LegendaryProportionsBuff.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == (ModifierDescriptor)NaturalArmor.Bonus)
                        .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Size);
                    TTTContext.Logger.LogPatch("Patched", LegendaryProportionsBuff);
                    AnimalGrowthBuff.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                        .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Stackable);
                }
                void PatchWildShape() {
                    WildShapeTools.WildShapeBuffs.AllBuffs.ForEach(buff => {
                        buff.GetComponents<AddStatBonus>()
                            .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor || c.Descriptor == ModifierDescriptor.NaturalArmorForm)
                            .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Racial);
                        TTTContext.Logger.LogPatch("Patched", buff);
                    });
                }
            }
        }
    }
}