using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System.Linq;
using HarmonyLib;

namespace TabletopTweaks.MechanicsChanges {
    static class NaturalArmorStacking {
        public static void patchPolymorphEffects() {
            if (!Resources.Settings.DisablePolymorphStacking) { return; }
        }
    }

    //Patch Natural Armor Stacking
    [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
    static class ModifierDescriptorHelper_IsStackable_Patch {

        static void Postfix(ref bool __result, ModifierDescriptor descriptor) {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            if (descriptor == ModifierDescriptor.NaturalArmor) {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
    static class ResourcesLibrary_InitializeLibrary_Patch {
        static bool Initialized;
        static bool Prefix() {
            if (Initialized) {
                // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                // to prevent blueprints from being garbage collected.
                return false;
            }
            else {
                return true;
            }
        }
        static void Postfix() {
            if (Initialized) return;
            Initialized = true;
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            Main.Log("Patching NaturalArmor Resources");
            patchNaturalArmorEffects();
            Main.Log("NaturalArmor Resource Patch Complete");
            
        }
        static void patchNaturalArmorEffects() {
            patchAnimalCompanionFeatures();
            patchItemBuffs();
            patchSpellBuffs();
            patchClassFeatures();
            patchFeats();

            void patchAnimalCompanionFeatures() {
                BlueprintFeature AnimalCompanionNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                BlueprintFeature AnimalCompanionStatFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                BlueprintFeature[] AnimalCompanionUpgrades = Resources.GetBlueprints<BlueprintFeature>()
                    .Where(bp => !string.IsNullOrEmpty(bp.name))
                    .Where(bp => bp.name.Contains("AnimalCompanionUpgrade"))
                    .ToArray();
                AnimalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                Main.Log($"{AnimalCompanionNaturalArmor.name} Patched");
                AnimalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                Main.Log($"{AnimalCompanionStatFeature.name} Patched");
                AnimalCompanionUpgrades.ForEach(bp => {
                    bp.GetComponents<AddStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                        .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                    Main.Log($"{bp.name} Patched");
                });
            }
            void patchClassFeatures() {
                BlueprintFeature DragonDiscipleNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
                DragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                Main.Log($"{DragonDiscipleNaturalArmor.name} Patched");
            }
            void patchFeats() {
                BlueprintFeature ImprovedNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");
                ImprovedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                Main.Log($"{ImprovedNaturalArmor.name} Patched");
            }
            void patchItemBuffs() {
                //Icy Protector
                BlueprintBuff ProtectionOfColdBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f592ecdb8045d7047a21b20ffee72afd");
                ProtectionOfColdBuff.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                Main.Log($"{ProtectionOfColdBuff.name} Patched");
            }
            void patchSpellBuffs() {
                BlueprintBuff LegendaryProportions = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("4ce640f9800d444418779a214598d0a3");
                LegendaryProportions.GetComponents<AddContextStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmorEnhancement)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.NaturalArmor);
                Main.Log($"{LegendaryProportions.name} Patched");
            }
        }
    }
}