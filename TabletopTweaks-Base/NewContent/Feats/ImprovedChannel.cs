using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ImprovedChannel {
        public static void AddImprovedChannel() {
            var SelectiveChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
            var ExtraChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");

            var ImprovedChannel = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ImprovedChannel", bp => {
                bp.SetName(TTTContext, "Improved Channel");
                bp.SetDescription(TTTContext, "Add 2 to the DC of saving throws made to resist the effects of your channel energy ability.");
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
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                }));
                SelectiveChannel.GetComponents<PrerequisiteFeature>().ForEach(p => {
                    bp.AddPrerequisiteFeature(p.Feature, p.Group);
                });
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ImprovedChannel")) { return; }
            FeatTools.AddAsFeat(ImprovedChannel);
        }
    }
}
