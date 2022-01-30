using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
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
    static class PiercingSpell {
        public static void AddPiercingSpell() {
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_PiercingSpellFeat = AssetLoader.LoadInternal("Feats", "Icon_PiercingSpellFeat.png");
            var Icon_PiercingSpellMetamagic = AssetLoader.LoadInternal("Metamagic", "Icon_PiercingSpellMetamagic.png", 128);
            var Icon_MetamagicRodPiercingLesser = AssetLoader.LoadInternal("Equipment", "Icon_MetamagicRodPiercingLesser.png", 64);
            var Icon_MetamagicRodPiercingNormal = AssetLoader.LoadInternal("Equipment", "Icon_MetamagicRodPiercingNormal.png", 64);
            var Icon_MetamagicRodPiercingGreater = AssetLoader.LoadInternal("Equipment", "Icon_MetamagicRodPiercingGreater.png", 64);

            var PiercingSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>("PiercingSpellFeat", bp => {
                bp.SetName("Metamagic (Piercing Spell)");
                bp.SetDescription("Your studies have helped you develop methods to overcome spell resistance.\n" +
                    "Benefit: When you cast a piercing spell against a target with spell resistance, it treats " +
                    "the spell resistance of the target as 5 lower than its actual SR.\n" +
                    "Level Increase: +1 (a piercing spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_PiercingSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Piercing;
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
            var FavoriteMetamagicFlaring = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicPiercing", bp => {
                bp.SetName("Favorite Metamagic — Piercing");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicPiercing;
                });
                bp.AddPrerequisiteFeature(PiercingSpellFeat);
            });

            if (ModSettings.AddedContent.Feats.IsEnabled("MetamagicPiercingSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    metamagic: (Metamagic)CustomMetamagic.Piercing,
                    name: "Piercing",
                    icon: Icon_PiercingSpellMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicPiercing
                );
            }
            var MetamagicRodsPiercing = ItemTools.CreateAllMetamagicRods(
                rodName: "Piercing Metamagic Rod",
                lesserIcon: Icon_MetamagicRodPiercingLesser,
                normalIcon: Icon_MetamagicRodPiercingNormal,
                greaterIcon: Icon_MetamagicRodPiercingGreater,
                metamagic: (Metamagic)CustomMetamagic.Piercing,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day piercing, " +
                    "as though using the Piercing Spell feat.",
                metamagicDescription: "Piercing Spell: When you cast a piercing spell against a target with spell resistance, it treats " +
                    "the spell resistance of the target as 5 lower than its actual SR."
            );

            if (ModSettings.AddedContent.Feats.IsDisabled("MetamagicPiercingSpell")) { return; }

            UpdateSpells();
            AddRodsToVenders();
            FeatTools.AddAsFeat(PiercingSpellFeat);
            FeatTools.AddAsMetamagicFeat(PiercingSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicFlaring);
        }
        private static void UpdateSpells() {
            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {
                bool validPiercing = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.SpellResistance || s.GetComponent<AbilityShadowSpell>());
                if (validPiercing) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Piercing)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Piercing;
                        Main.LogPatch("Enabled Piercing Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var WarCamp_ScrollVendorClericTable = Resources.GetBlueprint<BlueprintSharedVendorTable>("cdd7aa16e900b9146bc6963ca53b8e71");

            WarCamp_ScrollVendorClericTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = Resources.GetModBlueprintReference<BlueprintItemReference>("MetamagicRodNormalPiercing"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }
    }
}
