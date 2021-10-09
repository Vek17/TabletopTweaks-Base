using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Templates {
    static class AlignmentTemplates {
        private static readonly BlueprintAbility SmiteEvilAbility = Resources.GetBlueprint<BlueprintAbility>("7bb9eb2042e67bf489ccd1374423cdec");
        private static readonly BlueprintBuff SmiteEvilBuff = Resources.GetBlueprint<BlueprintBuff>("b6570b8cbb32eaf4ca8255d0ec3310b0");
        private static readonly BlueprintAbility FiendishSmiteGoodAbility = Resources.GetBlueprint<BlueprintAbility>("478cf0e6c5f3a4142835faeae3bd3e04");
        private static readonly BlueprintBuff FiendishSmiteGoodBuff = Resources.GetBlueprint<BlueprintBuff>("a9035e49d6d79a64eaec321f2cb629a8");

        public static void AddCelestialTemplate() {
            var CelestialTemplate = CreateAlignmentTemplate("TemplateCelestial", DamageAlignment.Evil, DamageEnergyType.Cold, DamageEnergyType.Acid, DamageEnergyType.Electricity);
            CelestialTemplate.SetDescription("Creature gains spell resistance equal to its level +5. It also gains:\n" +
                "1 — 4 HD: resistance 5 to cold, acid, and electricity.\n" +
                "5 — 10 HD: resistance 10 to cold, acid, and electricity, DR 5/evil\n" +
                "11+ HD: resistance 15 to cold, acid, and electricity, DR 10/evil\n" +
                "Smite Evil (Su): Once per day, the celestial creature may smite a evil-aligned creature. As a swift action, " +
                "the creature chooses one target within sight to smite. If this target is evil, the creature adds its Charisma bonus (if any) to " +
                "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            var CelestialTemplateSmiteEvilResource = CreateAlignmentResource("TemplateCelestialResource");
            var CelestialTemplateSmiteEvilBuff = CreateAlignmentSmiteBuff("TemplateCelestialSmiteEvilBuff", SmiteEvilBuff, bp => {
                bp.SetName("Smite Evil");
                bp.SetDescription("Once per day, the celestial creature may smite an evil-aligned creature. As a swift action, " +
                "the creature chooses one target within sight to smite. If this target is evil, the creature adds its Charisma bonus (if any) to " +
                "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            });
            var CelestialTemplateSmiteEvilAbility = CreateAlignmentSmite(
                "TemplateCelestialSmiteEvilAbility",
                AlignmentComponent.Evil,
                CelestialTemplateSmiteEvilResource,
                CelestialTemplateSmiteEvilBuff,
                SmiteEvilAbility
            );
            CelestialTemplate.AddComponent<AddAbilityResources>(c => {
                c.m_Resource = CelestialTemplateSmiteEvilResource.ToReference<BlueprintAbilityResourceReference>();
            });
            CelestialTemplate.AddComponent<AddFacts>(c => {
                c.m_Facts = new BlueprintUnitFactReference[] {
                    CelestialTemplateSmiteEvilAbility.ToReference<BlueprintUnitFactReference>()
                };
            });
        }
        public static void AddFiendishTemplate() {
            var FiendishTemplate = CreateAlignmentTemplate("TemplateFiendish", DamageAlignment.Good, DamageEnergyType.Cold, DamageEnergyType.Fire);
            FiendishTemplate.SetDescription("Creature gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to cold and fire.\n" +
                    "5 — 10 HD: resistance 10 to cold and fire, DR 5/good\n" +
                    "11+ HD: resistance 15 to cold and fire, DR 10/good\n" +
                    "Smite Good (Su): Once per day, the fiendish creature may smite a good-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is good, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            var FiendishTemplateSmiteGoodResource = CreateAlignmentResource("TemplateFiendishResource");
            var FiendishTemplateSmiteGoodBuff = CreateAlignmentSmiteBuff("TemplateFiendishSmiteGoodBuff", FiendishSmiteGoodBuff, bp => {
                bp.SetName("Smite Good");
                bp.SetDescription("Once per day, the fiendish creature may smite a good-aligned creature. As a swift action, " +
                "the creature chooses one target within sight to smite. If this target is good, the creature adds its Charisma bonus (if any) to " +
                "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            });
            var FiendishTemplateSmiteGoodAbility = CreateAlignmentSmite(
                "TemplateFiendishSmiteGoodAbility",
                AlignmentComponent.Good,
                FiendishTemplateSmiteGoodResource,
                FiendishTemplateSmiteGoodBuff,
                FiendishSmiteGoodAbility
            );
            FiendishTemplate.AddComponent<AddAbilityResources>(c => {
                c.m_Resource = FiendishTemplateSmiteGoodResource.ToReference<BlueprintAbilityResourceReference>();
            });
            FiendishTemplate.AddComponent<AddFacts>(c => {
                c.m_Facts = new BlueprintUnitFactReference[] {
                    FiendishTemplateSmiteGoodAbility.ToReference<BlueprintUnitFactReference>()
                };
            });
        }
        public static void AddEntropicTemplate() {
            var EntropicTemplate = CreateAlignmentTemplate("TemplateEntropic", DamageAlignment.Lawful, DamageEnergyType.Acid, DamageEnergyType.Fire);
            EntropicTemplate.SetDescription("Creature gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to acid and fire.\n" +
                    "5 — 10 HD: resistance 10 to acid and fire, DR 5/lawful\n" +
                    "11+ HD: resistance 15 to acid and fire, DR 10/lawful\n" +
                    "Smite Law (Su): Once per day, the entropic creature may smite a law-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is lawful, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            var EntropicTemplateSmiteLawResource = CreateAlignmentResource("TemplateEntropicResource");
            var EntropicTemplateSmiteLawBuff = CreateAlignmentSmiteBuff("TemplateEntropicSmiteLawBuff", FiendishSmiteGoodBuff, bp => {
                bp.SetName("Smite Law");
                bp.SetDescription("Once per day, the entropic creature may smite a law-aligned creature. As a swift action, " +
                "the creature chooses one target within sight to smite. If this target is lawful, the creature adds its Charisma bonus (if any) to " +
                "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            });
            var TemplateEntropicSmiteLawAbility = CreateAlignmentSmite(
                "TemplateEntropicSmiteLawAbility",
                AlignmentComponent.Lawful,
                EntropicTemplateSmiteLawResource,
                EntropicTemplateSmiteLawBuff,
                FiendishSmiteGoodAbility
            );
            EntropicTemplate.AddComponent<AddAbilityResources>(c => {
                c.m_Resource = EntropicTemplateSmiteLawResource.ToReference<BlueprintAbilityResourceReference>();
            });
            EntropicTemplate.AddComponent<AddFacts>(c => {
                c.m_Facts = new BlueprintUnitFactReference[] {
                    TemplateEntropicSmiteLawAbility.ToReference<BlueprintUnitFactReference>()
                };
            });
        }
        public static void AddResoluteTemplate() {
            var ResoluteTemplate = CreateAlignmentTemplate("TemplateResolute", DamageAlignment.Chaotic, DamageEnergyType.Acid, DamageEnergyType.Cold, DamageEnergyType.Fire);
            ResoluteTemplate.SetDescription("Creature gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to acid, cold and fire.\n" +
                    "5 — 10 HD: resistance 10 to acid, cold and fire, DR 5/chaotic\n" +
                    "11+ HD: resistance 15 to acid, cold and fire, DR 10/chaotic\n" +
                    "Smite Chaos (Su): Once per day, the resolute creature may smite a chaos-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is chaotic, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            var ResoluteTemplateSmiteChaosResource = CreateAlignmentResource("TemplateResoluteResource");
            var ResoluteTemplateSmiteChaosBuff = CreateAlignmentSmiteBuff("TemplateResoluteSmiteChaosBuff", FiendishSmiteGoodBuff, bp => {
                bp.SetName("Smite Chaos");
                bp.SetDescription("Once per day, the entropic creature may smite a chaos-aligned creature. As a swift action, " +
                "the creature chooses one target within sight to smite. If this target is chaotic, the creature adds its Charisma bonus (if any) to " +
                "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
            });
            var ResoluteTemplateSmiteChaosAbility = CreateAlignmentSmite(
                "TemplateResoluteSmiteChaosAbility",
                AlignmentComponent.Chaotic,
                ResoluteTemplateSmiteChaosResource,
                ResoluteTemplateSmiteChaosBuff,
                SmiteEvilAbility
            );
            ResoluteTemplate.AddComponent<AddAbilityResources>(c => {
                c.m_Resource = ResoluteTemplateSmiteChaosResource.ToReference<BlueprintAbilityResourceReference>();
            });
            ResoluteTemplate.AddComponent<AddFacts>(c => {
                c.m_Facts = new BlueprintUnitFactReference[] {
                    ResoluteTemplateSmiteChaosAbility.ToReference<BlueprintUnitFactReference>()
                };
            });
        }

        private static BlueprintFeature CreateAlignmentTemplate(string name, DamageAlignment dr, params DamageEnergyType[] resists) {
            return Helpers.CreateBlueprint<BlueprintFeature>(name, bp => {
                var Name = Regex.Split(name, @"(?<!^)(?=[A-Z])");
                bp.SetName($"{Name[1]} {Name[0]}");
                bp.SetDescription("");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddSpellResistance>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent(Helpers.CreateContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.BonusValue;
                    c.m_StepLevel = 5;
                }));
                resists.ForEach(resist => {
                    bp.AddComponent<AddDamageResistanceEnergy>(c => {
                        c.Type = resist;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageDice
                        };
                        c.ValueMultiplier = new ContextValue();
                        c.Pool = new ContextValue();
                    });
                });
                bp.AddComponent(Helpers.CreateContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem(){
                            BaseValue = 4,
                            ProgressionValue = 5
                        },
                        new ContextRankConfig.CustomProgressionItem(){
                            BaseValue = 10,
                            ProgressionValue = 10
                        },
                        new ContextRankConfig.CustomProgressionItem(){
                            BaseValue = 100,
                            ProgressionValue = 15
                        }
                    };
                }));
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Alignment = dr;
                    c.BypassedByAlignment = true;
                    c.MinEnhancementBonus = 1;
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.Reality = DamageRealityType.Ghost;
                    c.m_CheckedFactMythic = new BlueprintUnitFactReference();
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    };
                    c.Pool = new ContextValue();
                });
                bp.AddComponent(Helpers.CreateContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem(){
                            BaseValue = 4,
                            ProgressionValue = 0
                        },
                        new ContextRankConfig.CustomProgressionItem(){
                            BaseValue = 10,
                            ProgressionValue = 5
                        },
                        new ContextRankConfig.CustomProgressionItem(){
                            BaseValue = 100,
                            ProgressionValue = 10
                        }
                    };
                }));
            });
        }
        private static BlueprintBuff CreateAlignmentSmiteBuff(string name, BlueprintBuff buffEffects, Action<BlueprintBuff> init = null) {
            var smiteBuff = Helpers.CreateBuff(name, bp => {
                bp.IsClassFeature = true;
                bp.Stacking = StackingType.Stack;
                bp.m_Icon = buffEffects.Icon;
                bp.ResourceAssetIds = buffEffects.ResourceAssetIds;
                bp.FxOnStart = buffEffects.FxOnStart;
                bp.FxOnRemove = buffEffects.FxOnRemove;
                bp.AddComponent<AttackBonusAgainstTarget>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.StatBonus
                    };
                    c.CheckCaster = true;
                });
                bp.AddComponent<DamageBonusAgainstTarget>(c => {
                    c.CheckCaster = true;
                    c.ApplyToSpellDamage = true;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.DamageBonus
                    };
                });
                //bp.AddComponent<UniqueBuff>();
            });
            init?.Invoke(smiteBuff);
            return smiteBuff;
        }
        private static BlueprintAbility CreateAlignmentSmite(
            string name,
            AlignmentComponent alignment,
            BlueprintAbilityResource smiteResource,
            BlueprintBuff smiteBuff,
            BlueprintAbility abilityEffects) {

            return Helpers.CreateBlueprint<BlueprintAbility>(name, bp => {
                bp.m_DisplayName = smiteBuff.m_DisplayName;
                bp.m_Description = smiteBuff.m_Description;
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "Until the target of the smite is dead");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.SavingThrow", "");
                bp.m_Icon = SmiteEvilAbility.Icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Medium;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.ResourceAssetIds = abilityEffects.ResourceAssetIds;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional {
                            ConditionsChecker = new ConditionsChecker {
                                Conditions = new Condition[] {
                                    new ContextConditionAlignment() {
                                        Alignment = alignment
                                    },
                                    new ContextConditionHasBuffFromCaster() {
                                        m_Buff = smiteBuff.ToReference<BlueprintBuffReference>(),
                                        Not = true
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = smiteBuff.ToReference<BlueprintBuffReference>(),
                                    Permanent = true,
                                    DurationValue = new ContextDurationValue() {
                                        m_IsExtendable = true,
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = new ContextValue()
                                    }
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(),
                        }
                    );
                });
                bp.AddComponent(Helpers.CreateContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Stat = StatType.Charisma;
                    c.m_Min = 0;
                    c.m_UseMin = true;
                }));
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.StatBonus;
                    c.Value = new ContextDiceValue() {
                        DiceCountValue = 0,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.StatBonus
                        },
                    };
                    c.Modifier = 1;
                });
                bp.AddComponent(Helpers.CreateContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                }));
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.DamageBonus;
                    c.Value = new ContextDiceValue() {
                        DiceCountValue = 0,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus
                        },
                    };
                    c.Modifier = 1;
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                    c.m_RequiredResource = smiteResource.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = abilityEffects.GetComponent<AbilitySpawnFx>().PrefabLink;
                    c.Anchor = abilityEffects.GetComponent<AbilitySpawnFx>().Anchor;
                    c.PositionAnchor = abilityEffects.GetComponent<AbilitySpawnFx>().PositionAnchor;
                    c.OrientationAnchor = abilityEffects.GetComponent<AbilitySpawnFx>().OrientationAnchor;
                });
            });
        }
        private static BlueprintAbilityResource CreateAlignmentResource(string name) {
            return Helpers.CreateBlueprint<BlueprintAbilityResource>(name, bp => {
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount() {
                    BaseValue = 1,
                    IncreasedByLevel = false,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                };
                bp.m_Min = 1;
            });
        }
    }
}
