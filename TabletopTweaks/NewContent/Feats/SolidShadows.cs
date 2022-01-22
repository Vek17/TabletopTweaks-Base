using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewContent.MechanicsChanges;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewContent.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.Feats {
    static class SolidShadows {
        public static void AddSolidShadows() {
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var SpellFocus = Resources.GetBlueprintReference<BlueprintFeatureReference>("16fa59cc9a72a6043b566b49184f53fe");
            var Icon_SolidShadowsFeat = AssetLoader.LoadInternal("Feats", "Icon_SolidShadowsFeat.png");
            var Icon_SolidShadowsMetamagic = AssetLoader.LoadInternal("Metamagic", "Icon_SolidShadowsMetamagic.png", 128);

            var SolidShadowsSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>("SolidShadowsSpellFeat", bp => {
                bp.SetName("Metamagic (Solid Shadows)");
                bp.SetDescription("Your shadowy illusions are more potent.\n" +
                    "Benefit: When casting a shadow spell, that spell is 20% more real than normal.\n" +
                    "Level Increase: +1 (a solid shadows spell uses up a spell slot 1 level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_SolidShadowsFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.SolidShadows;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic | FeatureTag.Metamagic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 3;
                });
                bp.AddPrerequisite<PrerequisiteParametrizedFeature>(c => {
                    c.m_Feature = SpellFocus;
                    c.ParameterType = FeatureParameterType.SpellSchool;
                    c.SpellSchool = SpellSchool.Illusion;
                });
                bp.AddComponent<RecommendationRequiresSpellbook>();
            });

            var FavoriteMetamagicSolidShadows = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicSolidShadows", bp => {
                bp.SetName("Favorite Metamagic — Solid Shadows");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicSolidShadows;
                });
                bp.AddPrerequisiteFeature(SolidShadowsSpellFeat);
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("MetamagicSolidShadows")) { return; }
            MetamagicExtention.RegisterMetamagic(
                metamagic: (Metamagic)CustomMetamagic.SolidShadows,
                name: "Solid Shadows",
                icon: Icon_SolidShadowsMetamagic,
                defaultCost: 1,
                favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicSolidShadows
            );
            UpdateSpells();
            FeatTools.AddAsFeat(SolidShadowsSpellFeat);
            FeatTools.AddAsMetamagicFeat(SolidShadowsSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicSolidShadows);
        }
        private static void UpdateSpells() {
            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {
                bool validShadow = spell.GetComponent<AbilityShadowSpell>();
                if (validShadow) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.SolidShadows)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.SolidShadows;
                        Main.LogPatch("Enabled Solid Shadows Metamagic", spell);
                    }
                };
            }
        }
    }
}
