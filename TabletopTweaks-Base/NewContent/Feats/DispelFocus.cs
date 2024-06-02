using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class DispelFocus {
        public static void AddDispelFocus() {
            var Icon_DispelFocus = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_DispelFocus.png");
            var Icon_GreaterDispelFocus = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_GreaterDispelFocus.png");
            var DispelMagic = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("92681f181b507b34ea87018e8f7a528a");
            var DispelFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("c39576f8842e4505b14aa918b3a36a0a");
            var GreaterDispelFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("d0cf79c8e0a44325b00dc8fa6ad37d7c");

            var DispelFocusFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DispelFocusFeature", bp => {
                bp.SetName(TTTContext, "Dispel Focus");
                bp.SetDescription(TTTContext, "You are skilled at the art of dispelling.\n" +
                    "Whenever you attempt a dispel check based on your " +
                    "caster level, you gain a +2 bonus on the check.");
                bp.m_Icon = Icon_DispelFocus;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<ContextDispelBonusOnType>(c => {
                    c.Bonus = 2;
                    c.Type = Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic;
                });
                bp.AddPrerequisite<PrerequisiteSpellKnown>(c => {
                    c.m_Spell = DispelMagic;
                });
            });

            var DispelFocusGreaterFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DispelFocusGreaterFeature", bp => {
                bp.SetName(TTTContext, "Greater Dispel Focus");
                bp.SetDescription(TTTContext, "You are a master of the art of dispelling.\n" +
                    "Whenever you attempt a dispel check based on your " +
                    "caster level, you gain a +2 bonus to the check. This stacks with the bonus from Dispel Focus.");
                bp.m_Icon = Icon_GreaterDispelFocus;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<ContextDispelBonusOnType>(c => {
                    c.Bonus = 2;
                    c.Type = Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel;
                });
                bp.AddComponent<RecommendationHasFeature>(c => {
                    c.m_Feature = DispelFocusFeature.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic;
                });
                bp.AddPrerequisite<PrerequisiteSpellKnown>(c => {
                    c.m_Spell = DispelMagic;
                });
                bp.AddPrerequisiteFeature(DispelFocusFeature);
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("DispelFocus")) { return; }
            FeatTools.AddAsFeat(DispelFocusFeature);
            if (TTTContext.AddedContent.Feats.IsDisabled("DispelFocusGreater")) { return; }
            FeatTools.AddAsFeat(DispelFocusGreaterFeature);
        }
    }
}
