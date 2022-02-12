using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.NewContent.Feats {
    static class VarisianTattoo {
        public static void AddVarisianTattoo() {
            var SchoolMasteryMythicFeat = Resources.GetBlueprint<BlueprintParametrizedFeature>("ac830015569352b458efcdfae00a948c");
            var SpellFocus = Resources.GetBlueprint<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");

            var VarisianTattooFeature = Helpers.CreateBlueprint<BlueprintParametrizedFeature>("VarisianTattooFeature", bp => {
                bp.SetName("Varisian Tattoo");
                bp.SetDescription("You bear intricate tattoos that inspire and empower your natural magic ability. " +
                    "These tattoos mark you as a worker of the ancient traditions of Varisian magic. " +
                    "A Varisian tattoo typically consists of a long string of complex characters from the Thassilonian alphabet.\n" +
                    "Select a school of magic in which you have Spell Focus. Spells from this school are cast at +1 caster level.");
                bp.m_Icon = SchoolMasteryMythicFeat.Icon;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.m_Prerequisite = SpellFocus.ToReference<BlueprintParametrizedFeatureReference>();
                bp.ParameterType = FeatureParameterType.SpellSchool;
                bp.AddComponent<BonusCasterLevelParametrized>(c => {
                    c.Bonus = 1;
                    c.Descriptor = (ModifierDescriptor)Untyped.VarisianTattoo;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic;
                });
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("VarisianTattoo")) { return; }
            FeatTools.AddAsFeat(VarisianTattooFeature);
        }
    }
}
