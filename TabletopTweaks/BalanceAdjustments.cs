using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks {
    static class BalanceAdjustments {

        public static void patchNaturalArmorEffects() {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            patchAnimalCompanionFeatures();
            patchItemBuffs();
            patchClassFeatures();
            patchFeats();

            void patchAnimalCompanionFeatures() {
                BlueprintFeature animalCompanionNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                BlueprintFeature animalCompanionStatFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                BlueprintFeature[] animalCompanionUpgrades = Resources.Blueprints
                    .OfType<BlueprintFeature>()
                    .Where(bp => bp.name.Contains("AnimalCompanionUpgrade"))
                    .ToArray();

                animalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                animalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                Main.Log($"animalCompanionStatFeature NaturalArmor: {animalCompanionStatFeature.GetComponents<AddContextStatBonus>().Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor).Count()}");
                Main.Log($"animalCompanionStatFeature ArmorFocus: {animalCompanionStatFeature.GetComponents<AddContextStatBonus>().Where(c => c.Descriptor == ModifierDescriptor.ArmorFocus).Count()}");
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
