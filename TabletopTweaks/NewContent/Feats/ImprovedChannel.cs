using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.Enums;
using Kingmaker.Utility;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ImprovedChannel {
        public static void AddImprovedChannel() {
            var SelectiveChannel = Resources.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
            var ExtraChannel = Resources.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");

            var ImprovedChannel = Helpers.CreateBlueprint<BlueprintFeature>("ImprovedChannel", bp => {
                bp.SetName("Improved Channel");
                bp.SetDescription("Add 2 to the DC of saving throws made to resist the effects of your channel energy ability.");
                bp.m_Icon = ExtraChannel.Icon;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<IncreaseSpellDescriptorDC>(c => {
                    c.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.ChannelNegativeHarm | SpellDescriptor.ChannelNegativeHarm | SpellDescriptor.ChannelNegativeHeal | SpellDescriptor.ChannelPositiveHeal);
                    c.BonusDC = 2;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent(Helpers.Create<PureRecommendation>(c => {
                    c.Priority = RecommendationPriority.Good;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                }));
                SelectiveChannel.GetComponents<PrerequisiteFeature>().ForEach(p => {
                    bp.AddPrerequisiteFeature(p.Feature, p.Group);
                });
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ImprovedChannel")) { return; }
            FeatTools.AddAsFeat(ImprovedChannel);
        }
    }
}
