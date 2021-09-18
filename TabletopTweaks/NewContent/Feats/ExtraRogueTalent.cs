using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraRogueTalent {
        public static void AddExtraRogueTalent() {
            var RogueTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            var SylvanTricksterTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");

            var ExtraRogueTalent = FeatTools.CreateExtraSelectionFeat("ExtraRogueTalent", RogueTalentSelection, bp => {
                bp.SetName("Extra Rogue Talent");
                bp.SetDescription("You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            var ExtraRogueTalentSylvan = FeatTools.CreateExtraSelectionFeat("ExtraRogueTalentSylvan", SylvanTricksterTalentSelection, bp => {
                bp.SetName("Extra Rogue Talent (Sylvan Trickster)");
                bp.SetDescription("You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraRogueTalent")) { return; }
            FeatTools.AddAsFeat(ExtraRogueTalent);
            FeatTools.AddAsFeat(ExtraRogueTalentSylvan);
        }
    }
}
