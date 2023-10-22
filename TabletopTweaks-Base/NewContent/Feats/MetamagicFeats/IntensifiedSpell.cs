using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.Feats.MetamagicFeats {
    static class IntensifiedSpell {
        public static void AddIntensifiedSpell() {
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_IntensifiedSpellFeat = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_IntensifiedSpellFeat.png");
            var Icon_IntensifiedSpellMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_IntensifiedSpellMetamagic.png", size: 128);
            var Icon_MetamagicRodIntensifiedLesser = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodIntensifiedLesser.png", size: 64);
            var Icon_MetamagicRodIntensifiedNormal = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodIntensifiedNormal.png", size: 64);
            var Icon_MetamagicRodIntensifiedGreater = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodIntensifiedGreater.png", size: 64);

            var IntensifiedSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "IntensifiedSpellFeat", bp => {
                bp.SetName(TTTContext, "Metamagic (Intensified Spell)");
                bp.SetDescription(TTTContext, "Your spells can go beyond several normal limitations.\n" +
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
            var FavoriteMetamagicIntensified = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoriteMetamagicIntensified", bp => {
                bp.SetName(TTTContext, "Favorite Metamagic — Intensified");
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

            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicIntensifiedSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.Intensified,
                    name: "Intensified",
                    icon: Icon_IntensifiedSpellMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicIntensified,
                    metamagicFeat: IntensifiedSpellFeat
                );
            }
            var MetamagicRodsIntensified = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Intensified Metamagic Rod",
                lesserIcon: Icon_MetamagicRodIntensifiedLesser,
                normalIcon: Icon_MetamagicRodIntensifiedNormal,
                greaterIcon: Icon_MetamagicRodIntensifiedGreater,
                metamagic: (Metamagic)CustomMetamagic.Intensified,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day intensified, " +
                    "as though using the Intensified Spell feat.",
                metamagicDescription: "Intensified Spell: An intensified spell increases the maximum number of damage dice by 5 levels. " +
                    "You must actually have sufficient caster levels to surpass the maximum in order " +
                    "to benefit from this feat."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicIntensifiedSpell")) { return; }

            AddRodsToVenders();
            FeatTools.AddAsFeat(IntensifiedSpellFeat);
            FeatTools.AddAsMetamagicFeat(IntensifiedSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicIntensified);
        }

        public static void UpdateSpells() {
            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicIntensifiedSpell")) { return; }

            var spells = SpellTools.GetAllSpells()
                .SelectMany(s => s.AbilityAndVariants())
                .SelectMany(s => s.AbilityAndStickyTouch())
                .ToArray();
            foreach (var spell in spells) {
                bool isIntensifiedSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.FlattenAllActions()
                        .OfType<ContextActionDealDamage>()?
                        .Any(a => a.Value.DiceCountValue.ValueType == ContextValueType.Rank) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>()
                    || spell.AbilityAndVariants()
                        .SelectMany(s => s.AbilityAndStickyTouch())
                        .Any(s => s.FlattenAllActions()
                            .OfType<ContextActionSpawnAreaEffect>()
                            .Where(a => a.AreaEffect.FlattenAllActions()
                                .OfType<ContextActionDealDamage>()
                                .Any(a => a.Value?.DiceCountValue?.ValueType == ContextValueType.Rank))
                            .Any());
                if (isIntensifiedSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Intensified)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Intensified;
                        TTTContext.Logger.LogPatch("Enabled Intensified Metamagic", spell);
                    }
                };
            }
        }
        private static void AddRodsToVenders() {
            var WarCamp_REVendorTableMagic = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("f02cf582e915ae343aa489f11dba42aa");
            var RE_Chapter3VendorTableMagic = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e8e384f0e411fab42a69f16991cac161");
            var RE_Chapter5VendorTableMagic = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("e1d21a0e6c9177d42a1b0fac1d6f8b21");

            WarCamp_REVendorTableMagic.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodLesserIntensified"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            RE_Chapter3VendorTableMagic.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodNormalIntensified"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
            RE_Chapter5VendorTableMagic.AddComponent<LootItemsPackFixed>(c => {
                c.m_Item = new LootItem() {
                    m_Item = BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(TTTContext, "MetamagicRodGreaterIntensified"),
                    m_Loot = new BlueprintUnitLootReference()
                };
                c.m_Count = 1;
            });
        }

        [HarmonyPatch(typeof(ContextActionDealDamage), nameof(ContextActionDealDamage.GetDamageInfo))]
        static class ContextActionDealDamage_IntensifyMetamagic_Patch {
            static void Postfix(ContextActionDealDamage __instance, ref ContextActionDealDamage.DamageInfo __result) {
                if (!MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Intensified)) { return; }

                var context = __instance.Context;
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Intensified)) { return; }

                var Sources = context.m_RankSources ?? context.AssociatedBlueprint?.GetComponents<ContextRankConfig>();
                if (__instance.Value.DiceCountValue.ValueType == ContextValueType.Rank) {
                    var rankConfig = Sources.Where(crc => crc.m_Type == __instance.Value.DiceCountValue.ValueRank).FirstOrDefault();
                    if (rankConfig && rankConfig.m_BaseValueType == ContextRankBaseValueType.CasterLevel) {
                        var baseValue = rankConfig.ApplyProgression(rankConfig.GetBaseValue(__instance.Context));
                        var FinalCount = Math.Min(baseValue, rankConfig.m_Max + GetMultiplierIncrease(rankConfig));
                        __result.Dices = new DiceFormula(FinalCount, __instance.Value.DiceType);
                    }
                }

                int GetMultiplierIncrease(ContextRankConfig rankConfig) {
                    switch (rankConfig.m_Progression) {
                        case ContextRankProgression.Div2:
                            return 2;
                        case ContextRankProgression.Div2PlusStep:
                            return 5 / 2;
                        case ContextRankProgression.DivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.OnePlusDivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.StartPlusDivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.DelayedStartPlusDivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.DoublePlusBonusValue:
                            return 5 * 2;
                        case ContextRankProgression.MultiplyByModifier:
                            return 5 * rankConfig.m_StepLevel;
                    }
                    return 5;
                }
            }
        }
    }
}
