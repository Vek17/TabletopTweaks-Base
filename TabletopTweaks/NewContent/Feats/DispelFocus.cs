using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.Prerequisites;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class DispelFocus {
        public static void AddDispelFocus() {
            var Icon_DispelFocus = AssetLoader.LoadInternal("Feats", "Icon_DispelFocus.png");
            var Icon_GreaterDispelFocus = AssetLoader.LoadInternal("Feats", "Icon_GreaterDispelFocus.png");
            var DispelMagic = Resources.GetBlueprintReference<BlueprintAbilityReference>("92681f181b507b34ea87018e8f7a528a");

            var DispelFocusFeature = Helpers.CreateBlueprint<BlueprintFeature>("DispelFocusFeature", bp => {
                bp.SetName("Dispel Focus");
                bp.SetDescription("You are skilled at the art of dispelling.\n" +
                    "Whenever you attempt a dispel check based on your " +
                    "caster level, you gain a +2 bonus on the check.");
                bp.m_Icon = Icon_DispelFocus;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
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

            var DispelFocusGreaterFeature = Helpers.CreateBlueprint<BlueprintFeature>("DispelFocusGreaterFeature", bp => {
                bp.SetName("Greater Dispel Focus");
                bp.SetDescription("You are a master of the art of dispelling.\n" +
                    "Whenever you attempt a dispel check based on your " +
                    "caster level, you gain a +2 bonus to the check. This stacks with the bonus from Dispel Focus.");
                bp.m_Icon = Icon_GreaterDispelFocus;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
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
                bp.AddPrerequisiteFeature(DispelFocusFeature);
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("DispelFocus")) { return; }
            FeatTools.AddAsFeat(DispelFocusFeature);
            if (ModSettings.AddedContent.Feats.IsDisabled("DispelFocusGreater")) { return; }
            FeatTools.AddAsFeat(DispelFocusGreaterFeature);
        }
    }
}
