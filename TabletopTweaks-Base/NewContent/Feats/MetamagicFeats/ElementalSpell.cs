using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
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
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.Feats.MetamagicFeats {
    static class ElementalSpell {
        public static void AddElementalSpell() {
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");

            var Icon_ElementalSpellFeat = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalSpellFeat.png");
            var Icon_ElementalSpellFeatAcid = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalSpellFeatAcid.png");
            var Icon_ElementalSpellFeatCold = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalSpellFeatCold.png");
            var Icon_ElementalSpellFeatElectricity = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalSpellFeatElectricity.png");
            var Icon_ElementalSpellFeatFire = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalSpellFeatFire.png");

            var Icon_ElementalSpellAcidMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_ElementalSpellAcidMetamagic.png", size: 128);
            var Icon_ElementalSpellColdMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_ElementalSpellColdMetamagic.png", size: 128);
            var Icon_ElementalSpellElectricityMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_ElementalSpellElectricityMetamagic.png", size: 128);
            var Icon_ElementalSpellFireMetamagic = AssetLoader.LoadInternal(TTTContext, folder: "Metamagic", file: "Icon_ElementalSpellFireMetamagic.png", size: 128);

            var Icon_MetamagicRodElementalLesser = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodIntensifiedLesser.png", size: 64);
            var Icon_MetamagicRodElementalNormal = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodIntensifiedNormal.png", size: 64);
            var Icon_MetamagicRodElementalGreater = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MetamagicRodIntensifiedGreater.png", size: 64);

            var ElementalSpellFeatAcid = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatAcid", bp => {
                bp.SetName(TTTContext, "Metamagic (Elemental Spell — Acid)");
                bp.SetDescription(TTTContext, "You can manipulate the elemental nature of your spells.\n" +
                    "Benefit: You may replace a spell’s normal damage with acid or split the spell’s damage, so that half is acid and half is of its normal type.\n" +
                    "Level Increase: +1 (an elemental spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_ElementalSpellFeatAcid;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[0];
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.ElementalAcid;
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
            var ElementalSpellFeatCold = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatCold", bp => {
                bp.SetName(TTTContext, "Metamagic (Elemental Spell — Cold)");
                bp.SetDescription(TTTContext, "You can manipulate the elemental nature of your spells.\n" +
                    "Benefit: You may replace a spell’s normal damage with cold or split the spell’s damage, so that half is cold and half is of its normal type.\n" +
                    "Level Increase: +1 (an elemental spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_ElementalSpellFeatCold;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[0];
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.ElementalCold;
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
            var ElementalSpellFeatElectricity = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatElectricity", bp => {
                bp.SetName(TTTContext, "Metamagic (Elemental Spell — Electricity)");
                bp.SetDescription(TTTContext, "You can manipulate the elemental nature of your spells.\n" +
                    "Benefit: You may replace a spell’s normal damage with electricity or split the spell’s damage, so that half is electricity and half is of its normal type.\n" +
                    "Level Increase: +1 (an elemental spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_ElementalSpellFeatElectricity;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[0];
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.ElementalElectricity;
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
            var ElementalSpellFeatFire = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatFire", bp => {
                bp.SetName(TTTContext, "Metamagic (Elemental Spell — Fire)");
                bp.SetDescription(TTTContext, "You can manipulate the elemental nature of your spells.\n" +
                    "Benefit: You may replace a spell’s normal damage with fire or split the spell’s damage, so that half is fire and half is of its normal type.\n" +
                    "Level Increase: +1 (an elemental spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_ElementalSpellFeatFire;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[0];
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.ElementalFire;
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

            var ElementalSpellSplitBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "ElementalSpellSplitBuff", bp => {
                bp.m_Icon = Icon_ElementalSpellFeat;
                bp.SetName(TTTContext, "Elemental Spell Split Damage");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.ElementalSpellSplitDamage;
                });
            });
            var ElementalSpellSplitAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "ElementalSpellSplitAbility", bp => {
                bp.m_Icon = Icon_ElementalSpellFeat;
                bp.SetName(TTTContext, "Elemental Spell Split Damage");
                bp.SetDescription(TTTContext, "When using a spell with the Elemental Spell Metamagic you may choose to deal half of your damage with the chosen elemental type and half with the original type.");
                bp.m_Buff = ElementalSpellSplitBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = false;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
            });

            var ElementalSpellFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ElementalSpellFeatSelection", bp => {
                bp.SetName(TTTContext, "Metamagic (Elemental Spell)");
                bp.SetDescription(TTTContext, "You can manipulate the elemental nature of your spells.\n" +
                    "Benefit: Choose one energy type: acid, cold, electricity, or fire. You may replace a spell’s normal damage with that energy type " +
                    "or split the spell’s damage, so that half is of that energy type and half is of its normal type.\n" +
                    "Level Increase: +1 (an elemental spell uses up a spell slot one level higher than the spell’s actual level.)\n" +
                    "Special: You can gain this feat multiple times. Each time you must choose a different energy type.");
                bp.m_Icon = Icon_ElementalSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddFeatures(
                    ElementalSpellFeatAcid,
                    ElementalSpellFeatCold,
                    ElementalSpellFeatElectricity,
                    ElementalSpellFeatFire
                );
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        ElementalSpellSplitAbility.ToReference<BlueprintUnitFactReference>(),
                    };
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
            var FavoriteMetamagicElemental = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoriteMetamagicElemental", bp => {
                bp.SetName(TTTContext, "Favorite Metamagic — Elemental Spell");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicElemental;
                });
                bp.AddPrerequisiteFeature(ElementalSpellFeatSelection);
            });

            if (TTTContext.AddedContent.Feats.IsEnabled("MetamagicElementalSpell")) {
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.ElementalAcid,
                    name: "Elemental — Acid",
                    icon: Icon_ElementalSpellAcidMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicElemental
                );
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.ElementalCold,
                    name: "Elemental — Cold",
                    icon: Icon_ElementalSpellColdMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicElemental
                );
                MetamagicExtention.RegisterMetamagic(
                    context: TTTContext,
                    metamagic: (Metamagic)CustomMetamagic.ElementalElectricity,
                    name: "Elemental — Electricity",
                    icon: Icon_ElementalSpellElectricityMetamagic,
                    defaultCost: 1,
                    favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicElemental
                );
                MetamagicExtention.RegisterMetamagic(
                   context: TTTContext,
                   metamagic: (Metamagic)CustomMetamagic.ElementalFire,
                   name: "Elemental — Fire",
                   icon: Icon_ElementalSpellFireMetamagic,
                   defaultCost: 1,
                   favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicElemental,
                   metamagicMechanics: new ElementalSpellMechanics(ElementalSpellFeatAcid, ElementalSpellFeatCold, ElementalSpellFeatElectricity, ElementalSpellFeatFire)
               );
            }
            var MetamagicRodsElementalFire = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Elemental — Fire Metamagic Rod",
                lesserIcon: Icon_MetamagicRodElementalLesser,
                normalIcon: Icon_MetamagicRodElementalNormal,
                greaterIcon: Icon_MetamagicRodElementalGreater,
                metamagic: (Metamagic)CustomMetamagic.ElementalFire,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day elemental, " +
                    "as though using the Elemental Spell feat.",
                metamagicDescription: "Elemental Spell: You may replace a spell’s normal damage with fire or split the spell’s damage, so that half is fire and half is of its normal type."
            );
            var MetamagicRodsElementalCold = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Elemental — Cold Metamagic Rod",
                lesserIcon: Icon_MetamagicRodElementalLesser,
                normalIcon: Icon_MetamagicRodElementalNormal,
                greaterIcon: Icon_MetamagicRodElementalGreater,
                metamagic: (Metamagic)CustomMetamagic.ElementalCold,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day elemental, " +
                    "as though using the Elemental Spell feat.",
                metamagicDescription: "You may replace a spell’s normal damage with cold or split the spell’s damage, so that half is cold and half is of its normal type."
            );
            var MetamagicRodsElementalElectricity = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Elemental — Electricity Metamagic Rod",
                lesserIcon: Icon_MetamagicRodElementalLesser,
                normalIcon: Icon_MetamagicRodElementalNormal,
                greaterIcon: Icon_MetamagicRodElementalGreater,
                metamagic: (Metamagic)CustomMetamagic.ElementalElectricity,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day elemental, " +
                    "as though using the Elemental Spell feat.",
                metamagicDescription: "You may replace a spell’s normal damage with electricity or split the spell’s damage, so that half is electricity and half is of its normal type."
            );
            var MetamagicRodsElementalAcid = ItemTools.CreateAllMetamagicRods(
                TTTContext, rodName: "Elemental — Acid Metamagic Rod",
                lesserIcon: Icon_MetamagicRodElementalLesser,
                normalIcon: Icon_MetamagicRodElementalNormal,
                greaterIcon: Icon_MetamagicRodElementalGreater,
                metamagic: (Metamagic)CustomMetamagic.ElementalAcid,
                rodDescriptionStart: "This rod grants its wielder the ability to make up to three spells they cast per day elemental, " +
                    "as though using the Elemental Spell feat.",
                metamagicDescription: "You may replace a spell’s normal damage with acid or split the spell’s damage, so that half is acid and half is of its normal type."
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicElementalSpell")) { return; }

            //AddRodsToVenders();
            FeatTools.AddAsFeat(ElementalSpellFeatSelection);
            FeatTools.AddAsMetamagicFeat(ElementalSpellFeatSelection);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicElemental);
        }

        public static void UpdateSpells() {
            if (TTTContext.AddedContent.Feats.IsDisabled("MetamagicElementalSpell")) { return; }
            var elementalMetamagicMask = (Metamagic)(CustomMetamagic.ElementalAcid | CustomMetamagic.ElementalCold | CustomMetamagic.ElementalElectricity | CustomMetamagic.ElementalFire);
            var spells = SpellTools.GetAllSpells();
            foreach (var spell in spells) {
                bool isDamageSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.FlattenAllActions()
                        .OfType<ContextActionDealDamage>()
                        .Any())
                    || spell.GetComponent<AbilityShadowSpell>()
                    || spell.AbilityAndVariants()
                        .SelectMany(s => s.AbilityAndStickyTouch())
                        .Any(s => s.FlattenAllActions()
                            .OfType<ContextActionSpawnAreaEffect>()
                            .Where(c => c.AreaEffect.FlattenAllActions()
                                .OfType<ContextActionDealDamage>()
                                .Any())
                            .Any());
                if (isDamageSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic(elementalMetamagicMask)) {
                        spell.AvailableMetamagic |= elementalMetamagicMask;
                        TTTContext.Logger.LogPatch("Enabled Elemental Spell Metamagic", spell);
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

        private class ElementalSpellMechanics : IAfterRulebookEventTriggerHandler<RulePrepareDamage>, IAfterRulebookEventTriggerHandler<RuleDealDamage>, IGlobalSubscriber {

            BlueprintFeature ElementalSpellFeatAcid;
            BlueprintFeature ElementalSpellFeatCold;
            BlueprintFeature ElementalSpellFeatElectricity;
            BlueprintFeature ElementalSpellFeatFire;
            private readonly Metamagic elementalMetamagicMask = (Metamagic)(CustomMetamagic.ElementalAcid | CustomMetamagic.ElementalCold | CustomMetamagic.ElementalElectricity | CustomMetamagic.ElementalFire);

            internal ElementalSpellMechanics(BlueprintFeature acid, BlueprintFeature cold, BlueprintFeature electricity, BlueprintFeature fire) {
                this.ElementalSpellFeatAcid = acid;
                this.ElementalSpellFeatCold = cold;
                this.ElementalSpellFeatElectricity = electricity;
                this.ElementalSpellFeatFire = fire;
            }

            public void OnAfterRulebookEventTrigger(RulePrepareDamage evt) {
                var ability = evt.Reason.Ability;
                if (ability == null || !ability.Blueprint.IsSpell) {
                    return;
                }
                if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalAcid)) {
                    ApplyDamageEffect(DamageEnergyType.Acid, ElementalSpellFeatAcid, evt);
                } else if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalCold)) {
                    ApplyDamageEffect(DamageEnergyType.Cold, ElementalSpellFeatCold, evt);
                } else if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalElectricity)) {
                    ApplyDamageEffect(DamageEnergyType.Electricity, ElementalSpellFeatElectricity, evt);
                } else if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalFire)) {
                    ApplyDamageEffect(DamageEnergyType.Fire, ElementalSpellFeatFire, evt);
                }

                void ApplyDamageEffect(DamageEnergyType element, BlueprintFeature sourceBlueprint, RulePrepareDamage evt) {
                    var caster = ability.Caster;
                    var sourceFact = caster?.GetFact(sourceBlueprint);
                    foreach (BaseDamage baseDamage in evt.DamageBundle) {
                        if (baseDamage.Type == DamageType.Energy && !baseDamage.Precision) {
                            EnergyDamage energyDamage = baseDamage as EnergyDamage;
                            if (energyDamage is null) { continue; }
                            if (energyDamage.EnergyType == element) { continue; }
                            if (caster?.CustomMechanicsFeature(CustomMechanicsFeature.ElementalSpellSplitDamage) ?? false) {
                                if (energyDamage.Half) {
                                    energyDamage.Durability *= 0.5f;
                                } else {
                                    energyDamage.Half.Set(true, sourceFact);
                                }
                            } else {
                                energyDamage.ReplaceEnergy(element);
                            }
                        }
                    }
                };
            }

            public void OnAfterRulebookEventTrigger(RuleDealDamage evt) {
                var ability = evt.Reason.Ability;
                if (ability == null || !ability.Blueprint.IsSpell) {
                    return;
                }
                if (ability.Caster?.CustomMechanicsFeature(CustomMechanicsFeature.ElementalSpellSplitDamage) ?? false) {
                }
                if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalAcid)) {
                    ApplyDamageEffect(DamageEnergyType.Acid, ElementalSpellFeatAcid, evt);
                } else if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalCold)) {
                    ApplyDamageEffect(DamageEnergyType.Cold, ElementalSpellFeatCold, evt);
                } else if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalElectricity)) {
                    ApplyDamageEffect(DamageEnergyType.Electricity, ElementalSpellFeatElectricity, evt);
                } else if (ability.HasMetamagic((Metamagic)CustomMetamagic.ElementalFire)) {
                    ApplyDamageEffect(DamageEnergyType.Fire, ElementalSpellFeatFire, evt);
                }

                void ApplyDamageEffect(DamageEnergyType element, BlueprintFeature sourceBlueprint, RuleDealDamage evt) {
                    var caster = ability.Caster;
                    var sourceFact = caster?.GetFact(sourceBlueprint);
                    var context = evt.Reason.Context;
                    if (context == null) { return; }
                    var DamageBundle = new DamageBundle();
                    foreach (BaseDamage baseDamage in evt.DamageBundle) {
                        if (baseDamage.Type == DamageType.Energy && !baseDamage.Precision) {
                            EnergyDamage energyDamage = baseDamage as EnergyDamage;
                            if (energyDamage is null) { continue; }
                            if (energyDamage.EnergyType == element) { continue; }
                            var newDamage = new EnergyDamage(energyDamage.Dice.ModifiedValue, element) {
                                SourceFact = sourceFact
                            };
                            newDamage.CopyFrom(baseDamage);
                            DamageBundle.Add(newDamage);
                        }
                    }
                    if (!DamageBundle.m_Chunks.Any()) { return; }
                    var fakeContext = new MechanicsContext(
                        caster: evt.Initiator,
                        owner: evt.Target,
                        blueprint: context.SourceAbility
                    );
                    fakeContext.SetParams(context.Params.Clone());
                    fakeContext.Params.Metamagic &= ~elementalMetamagicMask;
                    fakeContext.TriggerRule<RuleDealDamage>(new RuleDealDamage(caster, evt.Target, DamageBundle) {
                        DisablePrecisionDamage = true,
                        HalfBecauseSavingThrow = evt.HalfBecauseSavingThrow,
                        SourceAbility = context.SourceAbility,
                        SourceArea = (context.AssociatedBlueprint as BlueprintAbilityAreaEffect),
                        Half = evt.Half
                    });
                };
            }
        }
    }
}
