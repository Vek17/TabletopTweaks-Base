using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraArcana {
        public static void AddExtraArcana() {
            var MagusArcanaSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
            var HexcrafterMagusHexArcanaSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");
            var EldritchMagusArcanaSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("d4b54d9db4932454ab2899f931c2042c");

            var ExtraArcana = FeatTools.CreateExtraSelectionFeat("ExtraArcana", MagusArcanaSelection, bp => {
                bp.SetName("Extra Arcana");
                bp.SetDescription("You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            var ExtraArcanaHexcrafter = FeatTools.CreateExtraSelectionFeat("ExtraArcanaHexcrafter", HexcrafterMagusHexArcanaSelection, bp => {
                bp.SetName("Extra Arcana (Hexcrafter)");
                bp.SetDescription("You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            var ExtraArcanaEldritchScion = FeatTools.CreateExtraSelectionFeat("ExtraArcanaEldritchScion", EldritchMagusArcanaSelection, bp => {
                bp.SetName("Extra Arcana (Eldritch Scion)");
                bp.SetDescription("You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraArcana")) { return; }
            FeatTools.AddAsFeat(ExtraArcana);
            FeatTools.AddAsFeat(ExtraArcanaHexcrafter);
            FeatTools.AddAsFeat(ExtraArcanaEldritchScion);
        }
    }
}
