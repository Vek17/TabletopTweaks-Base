using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;


namespace TabletopTweaks.Base.NewContent.Feats.MetamagicFeats {
    static class PiercingSpell {
        public static void AddPiercingSpell() {
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_PiercingSpellFeat = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_PiercingSpellFeat.png");
            var Icon_PiercingSpellMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_PiercingSpellMetamagic.png", size: 128);
            var Icon_MetamagicRodPiercingLesser = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodPiercingLesser.png", size: 64);
            var Icon_MetamagicRodPiercingNormal = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodPiercingNormal.png", size: 64);
            var Icon_MetamagicRodPiercingGreater = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodPiercingGreater.png", size: 64);

            var PiercingSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PiercingSpellFeat", bp => {
                bp.SetName(TTTContext, "Metamagic (Piercing Spell)");
                bp.SetDescription(TTTContext, "Your studies have helped you develop methods to overcome spell resistance.\n" +
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
            var FavoriteMetamagicPiercing = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoriteMetamagicPiercing", bp => {
                bp.SetName(TTTContext, "Favorite Metamagic — Piercing");
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

            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicPiercingSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.Piercing,
                    name: "Piercing",
                    icon: Icon_PiercingSpellMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicPiercing,
                    metamagicMechanics: PiercingSpellMechanics.Instance,
                    metamagicFeat: PiercingSpellFeat
                );
            }
            var MetamagicRodsPiercing = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Piercing Metamagic Rod",
                lesserIcon: Icon_MetamagicRodPiercingLesser,
                normalIcon: Icon_MetamagicRodPiercingNormal,
                greaterIcon: Icon_MetamagicRodPiercingGreater,
                metamagic: (Metamagic)CustomMetamagic.Piercing,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day piercing, " +
                    "as though using the Piercing Spell feat.",
                metamagicDescription: "Piercing Spell: When you cast a piercing spell against a target with spell resistance, it treats " +
                    "the spell resistance of the target as 5 lower than its actual SR."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicPiercingSpell")) { return; }

            AddRodsToVenders();
            FeatTools.AddAsFeat(PiercingSpellFeat);
            FeatTools.AddAsMetamagicFeat(PiercingSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicPiercing);
        }
        public static void UpdateSpells() {
            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicPiercingSpell")) { return; }

            var spells = SpellTools.GetAllSpells()
                .SelectMany(s => s.AbilityAndVariants())
                .SelectMany(s => s.AbilityAndStickyTouch())
                .ToArray();
            foreach (var spell in spells) {
                bool validPiercing = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.SpellResistance || s.GetComponent<AbilityShadowSpell>());
                if (validPiercing) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Piercing)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Piercing;
                        TTTContext.Logger.LogPatch("Enabled Piercing Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var Scrolls_DefendersHeartVendorTable = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("79b995e5fc910f34ab9dfec3c6b16c8f");
            var WarCamp_ScrollVendorClericTable = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("cdd7aa16e900b9146bc6963ca53b8e71");

            Scrolls_DefendersHeartVendorTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodLesserPiercing"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            WarCamp_ScrollVendorClericTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodNormalPiercing"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }
        private class PiercingSpellMechanics : IAfterRulebookEventTriggerHandler<RuleSpellResistanceCheck>, IGlobalSubscriber {

            private PiercingSpellMechanics() { }
            public static PiercingSpellMechanics Instance = new();

            public void OnAfterRulebookEventTrigger(RuleSpellResistanceCheck evt) {
                var isPiercing = evt.Context?.HasMetamagic((Metamagic)CustomMetamagic.Piercing) ?? false;
                if (!isPiercing) { return; }
                evt.SpellResistance -= 5;
            }
        }
    }
}
