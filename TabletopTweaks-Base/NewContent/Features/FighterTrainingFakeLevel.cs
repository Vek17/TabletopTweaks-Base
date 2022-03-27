using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    public class FighterTrainingFakeLevel {
        public static void AddFighterTrainingFakeLevel() {
            var FighterTrainingFakeLevel = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FighterTrainingFakeLevel", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 40;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Fighter Training Fake Level");
            });
        }
    }
}
