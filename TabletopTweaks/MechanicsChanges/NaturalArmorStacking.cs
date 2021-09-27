using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.MechanicsChanges {
    static class NaturalArmorStacking {
        [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
        static class ModifierDescriptorHelper_IsStackable_Patch {

            static void Postfix(ref bool __result, ModifierDescriptor descriptor) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("DisableNaturalArmorStacking")) { return; }
                if (descriptor == ModifierDescriptor.NaturalArmor) {
                    __result = false;
                }
                if (descriptor == (ModifierDescriptor)NaturalArmor.Stackable) {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.BaseFixes.IsDisabled("DisableNaturalArmorStacking")) { return; }
                Main.LogHeader("Patching NaturalArmor");
                PatchNaturalArmorEffects();

            }
            static void PatchNaturalArmorEffects() {
                PatchAnimalCompanionFeatures();
                PatchItemBuffs();
                PatchSpellBuffs();
                PatchClassFeatures();
                PatchFeats();

                void PatchAnimalCompanionFeatures() {
                    var AnimalCompanionSelectionBase = Resources.GetBlueprint<BlueprintFeatureSelection>("90406c575576aee40a34917a1b429254");
                    var AnimalCompanionNaturalArmor = Resources.GetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                    var AnimalCompanionStatFeature = Resources.GetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                    IEnumerable<BlueprintFeature> AnimalCompanionUpgrades = AnimalCompanionSelectionBase.m_AllFeatures.Concat(AnimalCompanionSelectionBase.m_Features)
                        .Select(feature => feature.Get())
                        .Where(feature => feature.GetComponent<AddPet>())
                        .Select(feature => feature.GetComponent<AddPet>())
                        .Where(component => component.m_UpgradeFeature != null)
                        .Select(component => component.m_UpgradeFeature.Get())
                        .Where(feature => feature != null)
                        .Distinct();
                    AnimalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    Main.LogPatch("Patched", AnimalCompanionNaturalArmor);
                    AnimalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                        .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Stackable);
                    Main.LogPatch("Patched", AnimalCompanionStatFeature);
                    AnimalCompanionUpgrades.ForEach(bp => {
                        bp.GetComponents<AddStatBonus>()
                            .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                            .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Stackable);
                        Main.LogPatch("Patched", bp);
                    });
                }
                void PatchClassFeatures() {
                    BlueprintFeature DragonDiscipleNaturalArmor = Resources.GetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
                    DragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    Main.LogPatch("Patched", DragonDiscipleNaturalArmor);
                }
                void PatchFeats() {
                    BlueprintFeature ImprovedNaturalArmor = Resources.GetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");
                    ImprovedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    Main.LogPatch("Patched", ImprovedNaturalArmor);
                }
                void PatchItemBuffs() {
                    //Icy Protector
                    BlueprintBuff ProtectionOfColdBuff = Resources.GetBlueprint<BlueprintBuff>("f592ecdb8045d7047a21b20ffee72afd");
                    ProtectionOfColdBuff.SetName("Iceplant");
                    ProtectionOfColdBuff.GetComponent<AddStatBonus>().Value = 4;
                    Main.LogPatch("Patched", ProtectionOfColdBuff);
                }
                void PatchSpellBuffs() {
                    var LegendaryProportions = Resources.GetBlueprint<BlueprintBuff>("4ce640f9800d444418779a214598d0a3");
                    LegendaryProportions.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmorForm)
                        .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Size);
                    Main.LogPatch("Patched", LegendaryProportions);
                    var FrightfulAspectBuff = Resources.GetBlueprint<BlueprintBuff>("906262fda0fbda442b27f9b0a04e5aa0");
                    FrightfulAspectBuff.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmorForm)
                        .ForEach(c => c.Descriptor = (ModifierDescriptor)NaturalArmor.Bonus);
                    Main.LogPatch("Patched", FrightfulAspectBuff);
                }
            }
        }
    }
}