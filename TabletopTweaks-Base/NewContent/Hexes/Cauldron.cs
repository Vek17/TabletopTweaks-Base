using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Hexes {
    internal class Cauldron {
        public static void AddCauldron() {
            //Used Assets
            var WinterWitchWitchHex = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b921af3627142bd4d9cf3aefb5e2610a");
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var ShamanHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4223fe18c75d4d14787af196a04e14e7");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
            var HexcrafterMagusHexMagusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            var HexcrafterMagusHexArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");
            var BrewPotions = BlueprintTools.GetBlueprint<BlueprintFeature>("c0f8c4e513eb493408b8070a1de93fc0");

            var CauldronHexFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WitchHexCauldronFeature", bp => {
                bp.SetName(TTTContext, "Cauldron");
                bp.SetDescription(TTTContext, "The witch receives Brew Potions as a bonus feat and a +4 bonus on skill checks to brew potions.");
                bp.m_Icon = BrewPotions.Icon;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WitchHex, FeatureGroup.ShamanHex };
                bp.IsClassFeature = true;
                bp.AddComponent<CraftBonus>(c => {
                    c.m_BonusFor = UsableItemType.Potion;
                    c.m_Value = 4;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BrewPotions.ToReference<BlueprintUnitFactReference>()
                    };
                });
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.m_Feature = BrewPotions.ToReference<BlueprintFeatureReference>();
                });
            });
            if (TTTContext.AddedContent.Hexes.IsDisabled("Cauldron")) { return; }
            WinterWitchWitchHex.AddFeatures(CauldronHexFeature);
            WitchHexSelection.AddFeatures(CauldronHexFeature);
            ShamanHexSelection.AddFeatures(CauldronHexFeature);
            SylvanTricksterTalentSelection.AddFeatures(CauldronHexFeature);
            HexcrafterMagusHexMagusSelection.AddFeatures(CauldronHexFeature);
            HexcrafterMagusHexArcanaSelection.AddFeatures(CauldronHexFeature);
        }
    }
}
