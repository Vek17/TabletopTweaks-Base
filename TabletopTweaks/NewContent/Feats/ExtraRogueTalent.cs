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
                bp.SetName("66eb9d584d1a45739a2b8a1e7c1b4cbf", "Extra Rogue Talent");
                bp.SetDescription("6e801a3c7a874d6a834eec3a31dd918c", "You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            var ExtraRogueTalentSylvan = FeatTools.CreateExtraSelectionFeat("ExtraRogueTalentSylvan", SylvanTricksterTalentSelection, bp => {
                bp.SetName("f3f41d1cbc024d218e435e4f056f0957", "Extra Rogue Talent (Sylvan Trickster)");
                bp.SetDescription("94a07091a80e4a7bbbbb219e3bdaaec0", "You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent." +
                    "\nYou can gain Extra Rogue Talent multiple times.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraRogueTalent")) { return; }
            FeatTools.AddAsFeat(ExtraRogueTalent);
            FeatTools.AddAsFeat(ExtraRogueTalentSylvan);
        }
    }
}
