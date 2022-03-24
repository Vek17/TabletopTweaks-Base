using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    class SpellSpecializationGreater {
        public static void AddSpellSpecializationGreater() {
            var SpellFocus = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
            var SpellSpecializationFirst = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("f327a765a4353d04f872482ef3e48c35");
            var SpellSpecializationProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("fe9220cdc16e5f444a84d85d5fa8e3d5");
            var SpellSpecializationSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("fe67bc3b04f1cd542b4df6e28b6e0ff5");
            var GreaterElementalFocusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("1c17446a3eb744f438488711b792ca4d");

            var SpellSpecializationGreater = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SpellSpecializationGreater", bp => {
                bp.SetName(TTTContext, "Greater Spell Specialization");
                bp.SetDescription(TTTContext, "By sacrificing a prepared spell of the same or higher level than your specialized spell, " +
                    "you may spontaneously cast your specialized spell. The specialized spell is treated as its normal level, " +
                    "regardless of the spell slot used to cast it. You may add a metamagic feat to the spell by increasing the " +
                    "spell slot and casting time, just like a cleric spontaneously casting a cure or inflict spell with a metamagic feat.");
                bp.m_Icon = GreaterElementalFocusSelection.m_Icon;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<SpellSpecializationGreaterComponent>();
                bp.AddComponent<RecommendationHasFeature>(c => {
                    c.m_Feature = SpellSpecializationProgression.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 13;
                });
                bp.AddPrerequisite<PrerequisiteSpellBookType>(c => {
                    c.Type = PrerequisiteSpellBookType.SpellbookType.Prepared;
                    c.RequiredSpellLevel = 5;
                });
                bp.AddPrerequisiteFeature(SpellFocus);
                bp.AddPrerequisiteFeaturesFromList(1, SpellSpecializationSelection.Features.Append(SpellSpecializationFirst).ToArray());
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("SpellSpecializationGreater")) { return; }

            SpellSpecializationSelection
                .Features
                .Append(SpellSpecializationFirst)
                .ForEach(feature => feature.AddComponent<SpellSpecializationParametrizedExtension>());
            FeatTools.AddAsFeat(SpellSpecializationGreater);
            FeatTools.AddAsFeat(SpellSpecializationGreater);
        }
    }
}
