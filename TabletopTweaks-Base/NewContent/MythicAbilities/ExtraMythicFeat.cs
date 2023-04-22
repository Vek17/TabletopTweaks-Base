using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class ExtraMythicFeat {
        public static void AddExtraMythicFeat() {
            var MythicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9ee0f6745f555484299b0a1563b99d81");
            var ExtraMythicAbilityMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("8a6a511c55e67d04db328cc49aaad2b8");

            var ExtraMythicFeatMythicAbility = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ExtraMythicFeatMythicAbility", bp => {
                bp.SetName(TTTContext, "Extra Mythic Feat");
                bp.SetDescription(TTTContext, "You gain a bonus mythic feat. You can take this mythic ability once.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Mode = SelectionMode.OnlyNew;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddFeatures(MythicFeatSelection.m_AllFeatures.Where(f => f.Guid != ExtraMythicAbilityMythicFeat.AssetGuid).ToArray());
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                });
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("ExtraMythicFeat")) { return; }
            FeatTools.AddAsMythicAbility(ExtraMythicFeatMythicAbility);
        }
    }
}
