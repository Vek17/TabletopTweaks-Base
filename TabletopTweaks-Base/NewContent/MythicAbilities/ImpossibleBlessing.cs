using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class ImpossibleBlessing {
        public static void AddImpossibleBlessing() {
            var BlessingSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("6d9dcc2a59210a14891aeedb09d406aa");

            var WitchHexAmelioratingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("3cdd3660fb69f3e4db0160fa97dfa85d");

            var ImpossibleBlessingFeature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ImpossibleBlessingFeature", bp => {
                bp.SetName(TTTContext, "Impossible Blessing");
                bp.SetDescription(TTTContext, "You feel a closer connection to your deity you serve.\n" +
                    "You gain one more blessing, ignoring all blessing prerequisites.");
                bp.m_Icon = BlessingSelection.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Mode = SelectionMode.OnlyNew;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.IgnorePrerequisites = true;
                bp.AddFeatures(BlessingSelection.m_AllFeatures);
                bp.AddPrerequisiteFeature(BlessingSelection);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("ImpossibleBlessing")) { return; }
            FeatTools.AddAsMythicAbility(ImpossibleBlessingFeature);
        }
    }
}
