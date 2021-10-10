using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class QuickChannel {
        public static void AddQuickChannel() {
            var SelectiveChannel = Resources.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
            var ExtraChannel = Resources.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");

            var QuickChannel = Helpers.CreateBlueprint<BlueprintFeature>("QuickChannel", bp => {
                bp.SetName("Quick Channel");
                bp.SetDescription("You may channel energy as a move action by spending 2 daily uses of that ability.");
                bp.m_Icon = SelectiveChannel.Icon;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<QuickChannelComponent>();
                bp.AddComponent(Helpers.Create<PureRecommendation>(c => {
                    c.Priority = RecommendationPriority.Good;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                }));
                SelectiveChannel.GetComponents<PrerequisiteFeature>().ForEach(p => {
                    bp.AddPrerequisiteFeature(p.Feature, p.Group);
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(p => {
                    p.Stat = StatType.SkillLoreReligion;
                    p.Value = 5;
                });
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("QuickChannel")) { return; }
            FeatTools.AddAsFeat(QuickChannel);
        }
    }
}
