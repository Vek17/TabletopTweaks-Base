using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.Feats.MetamagicFeats {
    static class RimeSpell {
        public static void AddRimeSpell() {
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var EntangleBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f7f6330726121cf4b90a6086b05d2e38");
            var IcyPrisonEntangledBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c53b286bb06a0544c85ca0f8bcc86950");
            var Icon_RimeSpellFeat = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_RimeSpellFeat.png");
            var Icon_RimeSpellMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_RimeSpellMetamagic.png", size: 128);
            var Icon_MetamagicRodRimeLesser = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodRimeLesser.png", size: 64);
            var Icon_MetamagicRodRimeNormal = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodRimeNormal.png", size: 64);
            var Icon_MetamagicRodRimeGreater = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodRimeGreater.png", size: 64);

            var RimeSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "RimeSpellFeat", bp => {
                bp.SetName(TTTContext, "Metamagic (Rime Spell)");
                bp.SetDescription(TTTContext, "Creatures damaged by your spells with the cold descriptor become entangled.\n" +
                    "Benefit: The frost of your cold spell clings to the target, impeding it for a short time. " +
                    "A rime spell causes creatures that takes cold damage from the spell to become entangled " +
                    "for a number of rounds equal to the original level of the spell.\n" +
                    "This feat only affects spells with the cold descriptor.\n" +
                    "Level Increase: +1 (a rime spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_RimeSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Rime;
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
            var FavoriteMetamagicRime = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoriteMetamagicRime", bp => {
                bp.SetName(TTTContext, "Favorite Metamagic — Rime");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicRime;
                });
                bp.AddPrerequisiteFeature(RimeSpellFeat);
            });
            var RimeEntagledBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "RimeEntagledBuff", bp => {
                bp.m_DisplayName = EntangleBuff.m_DisplayName;
                bp.m_Description = EntangleBuff.m_Description;
                bp.m_Icon = IcyPrisonEntangledBuff.Icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.ResourceAssetIds = IcyPrisonEntangledBuff.ResourceAssetIds;
                bp.FxOnStart = IcyPrisonEntangledBuff.FxOnStart;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Cold | SpellDescriptor.MovementImpairing;
                });
                bp.AddComponent<AddCondition>(c => {
                    c.Condition = UnitCondition.Entangled;
                });
                bp.AddComponent<RemoveWhenCombatEnded>();
            });

            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicRimeSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.Rime,
                    name: "Rime",
                    icon: Icon_RimeSpellMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicRime,
                    metamagicMechanics: RimeSpellMechanics.Instance,
                    metamagicFeat: RimeSpellFeat
                );
            }
            var MetamagicRodsRime = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Rime Metamagic Rod",
                lesserIcon: Icon_MetamagicRodRimeLesser,
                normalIcon: Icon_MetamagicRodRimeNormal,
                greaterIcon: Icon_MetamagicRodRimeGreater,
                metamagic: (Metamagic)CustomMetamagic.Rime,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day rime, " +
                    "as though using the Rime Spell feat.",
                metamagicDescription: "Rime Spell: A rime spell causes creatures that takes cold damage from the spell to become entangled " +
                    "for a number of rounds equal to the original level of the spell. This feat only affects spells with the cold descriptor."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicRimeSpell")) { return; }

            AddRodsToVenders();
            FeatTools.AddAsFeat(RimeSpellFeat);
            FeatTools.AddAsMetamagicFeat(RimeSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicRime);
        }
        public static void UpdateSpells() {
            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicRimeSpell")) { return; }

            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {
                bool isColdSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.GetComponent<SpellDescriptorComponent>()?
                        .Descriptor.HasAnyFlag(SpellDescriptor.Cold) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>();
                if (isColdSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Rime)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Rime;
                        TTTContext.Logger.LogPatch("Enabled Rime Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var RE_Chapter3VendorTableMagic = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e8e384f0e411fab42a69f16991cac161");
            var KrebusSlaveTraderTable = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("d43baa8b603f4604f8e36b048072e759");

            RE_Chapter3VendorTableMagic.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodNormalRime"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            KrebusSlaveTraderTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodGreaterRime"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }
        private class RimeSpellMechanics : IAfterRulebookEventTriggerHandler<RuleDealDamage>, IGlobalSubscriber {

            private RimeSpellMechanics() { }
            public static RimeSpellMechanics Instance = new();

            static BlueprintBuffReference RimeEntagledBuff = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(TTTContext, "RimeEntagledBuff");
            public void OnAfterRulebookEventTrigger(RuleDealDamage evt) {
                var context = evt.Reason.Context;
                if (context == null) { return; }
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Rime)) { return; }
                if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Cold)) { return; }
                if (!evt.DamageBundle
                    .OfType<EnergyDamage>()
                    .Where(damage => damage.EnergyType == DamageEnergyType.Cold)
                    .Any(damage => !damage.Immune)) { return; }
                var rounds = Math.Max(1, context.Params?.SpellLevel ?? context.SpellLevel).Rounds();
                var buff = evt.Target?.Descriptor?.AddBuff(RimeEntagledBuff, context, rounds.Seconds);
                if (buff != null) {
                    buff.IsFromSpell = true;
                }
            }
        }
    }
}
