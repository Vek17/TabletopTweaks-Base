using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewContent.MechanicsChanges;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewContent.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.Feats {
    static class IntensifiedSpell {
        public static void AddIntensifiedSpell() {
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_IntensifiedSpellFeat = AssetLoader.LoadInternal("Feats", "Icon_IntensifiedSpellFeat.png");
            var Icon_IntensifiedSpellMetamagic = AssetLoader.LoadInternal("Metamagic", "Icon_IntensifiedSpellMetamagic.png", 128);

            var IntensifiedSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>("IntensifiedSpellFeat", bp => {
                bp.SetName("Metamagic (Intensified Spell)");
                bp.SetDescription("Your spells can go beyond several normal limitations.\n" +
                    "Benefit: An intensified spell increases the maximum number of damage dice by 5 levels. " +
                    "You must actually have sufficient caster levels to surpass the maximum in order " +
                    "to benefit from this feat. No other variables of the spell are affected, and spells " +
                    "that inflict damage that is not modified by caster level are not affected by this feat.\n" +
                    "Level Increase: +1 (an intensified spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Intensified;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic | FeatureTag.Metamagic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 3;
                });
                bp.AddComponent<RecommendationRequiresSpellbook>();
            });

            var FavoriteMetamagicIntensified = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicIntensified", bp => {
                bp.SetName("Favorite Metamagic — Intensified");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicIntensified;
                });
                bp.AddPrerequisiteFeature(IntensifiedSpellFeat);
            });



            if (ModSettings.AddedContent.Feats.IsDisabled("MetamagicIntensifiedSpell")) { return; }
            MetamagicExtention.RegisterMetamagic(
                metamagic: (Metamagic)CustomMetamagic.Intensified,
                name: "Intensified",
                icon: Icon_IntensifiedSpellMetamagic,
                defaultCost: 1,
                favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicIntensified
            );
            UpdateSpells();
            FeatTools.AddAsFeat(IntensifiedSpellFeat);
            FeatTools.AddAsMetamagicFeat(IntensifiedSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicIntensified);
        }

        private static void UpdateSpells() {
            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {
                bool isIntensifiedSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.FlattenAllActions()
                        .OfType<ContextActionDealDamage>()?
                        .Any(a => a.Value.DiceCountValue.ValueType == ContextValueType.Rank) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>();
                if (isIntensifiedSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Intensified)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Intensified;
                        Main.LogPatch("Enabled Intensified Metamagic", spell);
                    }
                };
            }
        }
    }
}
