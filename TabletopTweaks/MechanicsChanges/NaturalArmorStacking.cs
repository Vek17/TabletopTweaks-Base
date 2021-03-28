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
            patchNaturalArmorEffects();
            //Do Stuff
        }
        static void patchNaturalArmorEffects() {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            patchAnimalCompanionFeatures();
            patchItemBuffs();
            patchClassFeatures();
            patchFeats();

            void patchAnimalCompanionFeatures() {
                BlueprintFeature animalCompanionNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                BlueprintFeature animalCompanionStatFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                BlueprintFeature[] animalCompanionUpgrades = Resources.GetBlueprints<BlueprintFeature>()
                    .Where(bp => bp.name.Contains("AnimalCompanionUpgrade"))
                    .ToArray();

                animalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                animalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                animalCompanionUpgrades.ForEach(bp => {
                    bp.GetComponents<AddStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                });
            }
            void patchClassFeatures() {
                BlueprintFeature dragonDiscipleNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
                dragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
            void patchFeats() {
                BlueprintFeature improvedNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");
                improvedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
            void patchItemBuffs() {
                //Icy Protector
                BlueprintBuff protectionOfColdBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f592ecdb8045d7047a21b20ffee72afd");
                protectionOfColdBuff.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
        }
    }
}