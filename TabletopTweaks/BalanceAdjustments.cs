using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Enums;

namespace TabletopTweaks {
    static class BalanceAdjustments {

        public static void patchNaturalArmorEffects() {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            BlueprintFeature animalCompanionNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
            BlueprintFeature dragonDiscipleNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
            BlueprintFeature improvedNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8"); 
            //Icy Protector
            BlueprintBuff protectionOfColdBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f592ecdb8045d7047a21b20ffee72afd");

            animalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.Racial;
            dragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.Racial;
            improvedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.Racial;
            
            protectionOfColdBuff.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.UntypedStackable;
        }
    }
}
