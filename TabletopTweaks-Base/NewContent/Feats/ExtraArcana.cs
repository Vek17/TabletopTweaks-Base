using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraArcana {
        public static void AddExtraArcana() {
            var MagusArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
            var HexcrafterMagusHexArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");
            var EldritchMagusArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("d4b54d9db4932454ab2899f931c2042c");
            var ExtraArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("00727883edf145e2a6bce9ad176ecfd8");

            var ExtraArcana = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraArcana", MagusArcanaSelection, bp => {
                bp.SetName(TTTContext, "Extra Arcana");
                bp.SetDescription(TTTContext, "You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            var ExtraArcanaHexcrafter = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraArcanaHexcrafter", HexcrafterMagusHexArcanaSelection, bp => {
                bp.SetName(TTTContext, "Extra Arcana (Hexcrafter)");
                bp.SetDescription(TTTContext, "You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            var ExtraArcanaEldritchScion = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraArcanaEldritchScion", EldritchMagusArcanaSelection, bp => {
                bp.SetName(TTTContext, "Extra Arcana (Eldritch Scion)");
                bp.SetDescription(TTTContext, "You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraArcana")) { return; }
            FeatTools.AddAsFeat(ExtraArcana);
            FeatTools.AddAsFeat(ExtraArcanaHexcrafter);
            FeatTools.AddAsFeat(ExtraArcanaEldritchScion);
            FeatTools.RemoveAsFeat(ExtraArcanaSelection);
        }
    }
}
