using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraRogueTalent {
        public static void AddExtraRogueTalent() {
            var RogueTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");

            var ExtraRogueTalent = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraRogueTalent", RogueTalentSelection, bp => {
                bp.SetName(TTTContext, "Extra Rogue Talent");
                bp.SetDescription(TTTContext, "You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            var ExtraRogueTalentSylvan = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraRogueTalentSylvan", SylvanTricksterTalentSelection, bp => {
                bp.SetName(TTTContext, "Extra Rogue Talent (Sylvan Trickster)");
                bp.SetDescription(TTTContext, "You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraRogueTalent")) { return; }
            FeatTools.AddAsFeat(ExtraRogueTalent);
            FeatTools.AddAsFeat(ExtraRogueTalentSylvan);
        }
    }
}
