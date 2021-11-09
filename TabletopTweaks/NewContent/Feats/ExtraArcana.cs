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
                bp.SetName("a45a3b0a607d45f6abf763221d6777db", "Extra Arcana");
                bp.SetDescription("fbbb225477514c57a6022013df66b665", "You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            var ExtraArcanaHexcrafter = FeatTools.CreateExtraSelectionFeat("ExtraArcanaHexcrafter", HexcrafterMagusHexArcanaSelection, bp => {
                bp.SetName("38a4fb4c4c4b4b5b897badd785c5016b", "Extra Arcana (Hexcrafter)");
                bp.SetDescription("21f2924f7666453fa5c36c0c883319c3", "You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            var ExtraArcanaEldritchScion = FeatTools.CreateExtraSelectionFeat("ExtraArcanaEldritchScion", EldritchMagusArcanaSelection, bp => {
                bp.SetName("be1dcb142e5e4503a8acce735e082545", "Extra Arcana (Eldritch Scion)");
                bp.SetDescription("ba91e90b7fd74211b0a8e284e767a99b", "You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana." +
                    "\nYou can gain this feat multiple times. Its effects stack, granting a new arcana each time you gain this feat.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraArcana")) { return; }
            FeatTools.AddAsFeat(ExtraArcana);
            FeatTools.AddAsFeat(ExtraArcanaHexcrafter);
            FeatTools.AddAsFeat(ExtraArcanaEldritchScion);
        }
    }
}
