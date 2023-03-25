using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewEvents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.Feats.MetamagicFeats {
    static class EncouragingSpell {
        public static void AddEncouragingSpell() {
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_EncouragingSpellFeat = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_EncouragingSpellFeat.png");
            var Icon_EncouragingSpellMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_EncouragingSpellMetamagic.png", size: 128);
            var Icon_MetamagicRodEncouragingLesser = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodEncouragingLesser.png", size: 64);
            var Icon_MetamagicRodEncouragingNormal = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodEncouragingNormal.png", size: 64);
            var Icon_MetamagicRodEncouragingGreater = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodEncouragingGreater.png", size: 64);

            var EncouragingSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "EncouragingSpellFeat", bp => {
                bp.SetName(TTTContext, "Metamagic (Encouraging Spell)");
                bp.SetDescription(TTTContext, "Your inspiration provides your allies with greater support.\n" +
                    "Benefit: Any morale bonus granted by an encouraging spell is increased by 1.\n" +
                    "Level Increase: +1 (an encouraging spell uses up a spell slot 1 level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_EncouragingSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Encouraging;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic | FeatureTag.Metamagic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Charisma;
                    c.Value = 13;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillPersuasion;
                    c.Value = 6;
                });
                bp.AddComponent<RecommendationRequiresSpellbook>();
            });
            var FavoriteMetamagicEncouraging = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoriteMetamagicEncouraging", bp => {
                bp.SetName(TTTContext, "Favorite Metamagic — Encouraging");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicEncouraging; // FIX
                });
                bp.AddPrerequisiteFeature(EncouragingSpellFeat);
            });

            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicEncouragingSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.Encouraging,
                    name: "Encouraging",
                    icon: Icon_EncouragingSpellMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicEncouraging,
                    metamagicMechanics: EncouragingSpellMechanics.Instance,
                    metamagicFeat: EncouragingSpellFeat
                );
            }
            var MetamagicRodsPiercing = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Encouraging Metamagic Rod",
                lesserIcon: Icon_MetamagicRodEncouragingLesser,
                normalIcon: Icon_MetamagicRodEncouragingNormal,
                greaterIcon: Icon_MetamagicRodEncouragingGreater,
                metamagic: (Metamagic)CustomMetamagic.Encouraging,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day encouraging, " +
                    "as though using the Encouraging Spell feat.",
                metamagicDescription: "Encouraging Spell: Any morale bonus granted by an encouraging spell is increased by 1."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicEncouragingSpell")) { return; }

            AddRodsToVenders();
            FeatTools.AddAsFeat(EncouragingSpellFeat);
            FeatTools.AddAsMetamagicFeat(EncouragingSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicEncouraging);
        }
        public static void UpdateSpells() {
            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicEncouragingSpell")) { return; }

            var spells = SpellTools.GetAllSpells()
                .Where(s => s.AssetGuid.m_Guid != Guid.Parse("8bc64d869456b004b9db255cdd1ea734") /*Exclude Bane*/);
            foreach (var spell in spells) {
                bool validEncouraging = spell
                    .AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => {
                        return s.FlattenAllActions()
                            .OfType<ContextActionApplyBuff>()
                            .Select(c => c.Buff)
                            .Where(buff => buff is not null)
                            .Any(buff => {
                                foreach (var component in buff.ComponentsArray) {
                                    Type type = component.GetType();
                                    var foundMorale = AccessTools.GetDeclaredFields(type)
                                        .Where(f => f.FieldType == typeof(ModifierDescriptor))
                                        .Select(field => (ModifierDescriptor)field.GetValue(component))
                                        .Any(descriptor => descriptor == ModifierDescriptor.Morale);
                                    if (foundMorale) {
                                        return true;
                                    }
                                }
                                return false;
                            });
                    });
                if (validEncouraging) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Encouraging)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Encouraging;
                        TTTContext.Logger.LogPatch("Enabled Encouraging Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var Scrolls_DefendersHeartVendorTable = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("79b995e5fc910f34ab9dfec3c6b16c8f");
            var WarCamp_ScrollVendorClericTable = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("cdd7aa16e900b9146bc6963ca53b8e71");

            Scrolls_DefendersHeartVendorTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodLesserEncouraging"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            WarCamp_ScrollVendorClericTable.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodNormalEncouraging"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }
        private class EncouragingSpellMechanics : IAddModifierHandler, IGlobalSubscriber {

            private EncouragingSpellMechanics() { }
            public static EncouragingSpellMechanics Instance = new();

            public void OnBeforeStatModifierAdded(ModifiableValue instance, ref int value, [NotNull] EntityFact sourceFact, ModifierDescriptor descriptor) {
                TryApplyAdjustment(ref value, descriptor, sourceFact?.MaybeContext);
            }

            public void OnBeforeRuleModifierAdded(RulebookEvent instance, ref int value, [NotNull] EntityFact sourceFact, ModifierDescriptor descriptor) {
                TryApplyAdjustment(ref value, descriptor, sourceFact?.MaybeContext);
            }

            private void TryApplyAdjustment(ref int value, ModifierDescriptor descriptor, MechanicsContext context) {
                if (context == null) { return; }
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Encouraging)) { return; }
                if (descriptor == ModifierDescriptor.Morale) {
                    value++;
                }
            }
        }
    }
}
