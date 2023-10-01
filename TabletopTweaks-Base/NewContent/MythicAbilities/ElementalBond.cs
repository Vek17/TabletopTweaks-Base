using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class ElementalBond {
        public static void AddElementalBond() {
            var Icon_ElementalBondAir = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalBondAir.png");
            var Icon_ElementalBondEarth = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalBondEarth.png");
            var Icon_ElementalBondFire = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalBondFire.png");
            var Icon_ElementalBondWater = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ElementalBondWater.png");

            var ElementalBondAir = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalBondAir", bp => {
                bp.SetName(TTTContext, "Elemental Bond (Air)");
                bp.SetDescription(TTTContext, "You are connected to the elemental plane of air. " +
                    "Whenever you cast a spell with the electricity descriptor, add your mythic rank to your caster level for that spell. " +
                    "You gain resistance 10 against electricity. " +
                    "At 6th tier, this resistance increases to 20. At 9th tier, this resistance increases to 30.");
                bp.m_Icon = Icon_ElementalBondAir;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.AddComponent<AddContextCasterLevelBonus>(c => {
                    c.SpellsOnly = true;
                    c.Descriptor = SpellDescriptor.Electricity;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.BonusCasterLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterProperty,
                        Property = UnitProperty.MythicLevel
                    };
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Electricity;
                    c.ValueMultiplier = new ContextValue();
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.Pool = new ContextValue();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 5,
                            ProgressionValue = 10
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 6,
                            ProgressionValue = 20
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 30
                        }
                    };
                });
            });
            var ElementalBondEarth = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalBondEarth", bp => {
                bp.SetName(TTTContext, "Elemental Bond (Earth)");
                bp.SetDescription(TTTContext, "You are connected to the elemental plane of earth. " +
                    "Whenever you cast a spell with the acid descriptor, add your mythic rank to your caster level for that spell. " +
                    "You gain resistance 10 against acid. " +
                    "At 6th tier, this resistance increases to 20. At 9th tier, this resistance increases to 30.");
                bp.m_Icon = Icon_ElementalBondEarth;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.AddComponent<AddContextCasterLevelBonus>(c => {
                    c.SpellsOnly = true;
                    c.Descriptor = SpellDescriptor.Acid;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.BonusCasterLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterProperty,
                        Property = UnitProperty.MythicLevel
                    };
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Acid;
                    c.ValueMultiplier = new ContextValue();
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.Pool = new ContextValue();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 5,
                            ProgressionValue = 10
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 6,
                            ProgressionValue = 20
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 30
                        }
                    };
                });
            });
            var ElementalBondFire = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalBondFire", bp => {
                bp.SetName(TTTContext, "Elemental Bond (Fire)");
                bp.SetDescription(TTTContext, "You are connected to the elemental plane of fire. " +
                    "Whenever you cast a spell with the fire descriptor, add your mythic rank to your caster level for that spell. " +
                    "You gain resistance 10 against fire. " +
                    "At 6th tier, this resistance increases to 20. At 9th tier, this resistance increases to 30.");
                bp.m_Icon = Icon_ElementalBondFire;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.AddComponent<AddContextCasterLevelBonus>(c => {
                    c.SpellsOnly = true;
                    c.Descriptor = SpellDescriptor.Fire;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.BonusCasterLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterProperty,
                        Property = UnitProperty.MythicLevel
                    };
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Fire;
                    c.ValueMultiplier = new ContextValue();
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.Pool = new ContextValue();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 5,
                            ProgressionValue = 10
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 6,
                            ProgressionValue = 20
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 30
                        }
                    };
                });
            });
            var ElementalBondWater = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ElementalBondWater", bp => {
                bp.SetName(TTTContext, "Elemental Bond (Water)");
                bp.SetDescription(TTTContext, "You are connected to the elemental plane of water. " +
                    "Whenever you cast a spell with the cold descriptor, add your mythic rank to your caster level for that spell. " +
                    "You gain resistance 10 against cold. " +
                    "At 6th tier, this resistance increases to 20. At 9th tier, this resistance increases to 30.");
                bp.m_Icon = Icon_ElementalBondWater;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.AddComponent<AddContextCasterLevelBonus>(c => {
                    c.SpellsOnly = true;
                    c.Descriptor = SpellDescriptor.Cold;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.BonusCasterLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterProperty,
                        Property = UnitProperty.MythicLevel
                    };
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Cold;
                    c.ValueMultiplier = new ContextValue();
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.Pool = new ContextValue();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 5,
                            ProgressionValue = 10
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 6,
                            ProgressionValue = 20
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 30
                        }
                    };
                });
            });
            var ElementalBondSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ElementalBondSelection", bp => {
                bp.SetName(TTTContext, "Elemental Bond");
                bp.SetDescription(TTTContext, "You are connected to one of the elemental planes. " +
                    "Select one elemental plane: air, earth, fire, or water. " +
                    "Whenever you cast a spell with a descriptor matching that plane, add your mythic rank to your caster level for that spell. " +
                    "You gain resistance 10 against an energy type associated with your chosen plane—electricity for air, " +
                    "acid for earth, fire for fire, and cold for water. " +
                    "At 6th tier, this resistance increases to 20. At 9th tier, this resistance increases to 30.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Group = FeatureGroup.MythicAbility;
                bp.Ranks = 1;
                bp.Mode = SelectionMode.OnlyNew;
                bp.AddFeatures(
                    ElementalBondAir,
                    ElementalBondEarth,
                    ElementalBondFire,
                    ElementalBondWater
                );
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("ElementalBond")) { return; }
            FeatTools.AddAsMythicAbility(ElementalBondSelection);
        }
    }
}
