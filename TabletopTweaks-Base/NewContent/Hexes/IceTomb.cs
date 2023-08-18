using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Hexes {
    internal class IceTomb {
        public static void AddIceTomb() {

            var WitchMajorHex = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8ac781b33e380c84aa578f1b006dd6c5");
            var WitchHexDCProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("bdc230ce338f427ba74de65597b0d57a");
            var WitchHexCasterLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("2d2243f4f3654512bdda92e80ef65b6d");
            var WitchHexSpellLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("75efe8b64a3a4cd09dda28cef156cfb5");
            var SylvanTricksterArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("490394869f666c141bf8647b1a365220");
            var HexcrafterArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("79ccf7a306a5d5547bebd97299f6fc89");
            var Staggered = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("df3950af5a783bd4d91ab73eb8fa0fd3");

            var WinterWitchWitchHex = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b921af3627142bd4d9cf3aefb5e2610a");
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var IcyPrison = BlueprintTools.GetBlueprint<BlueprintAbility>("65e8d23aef5e7784dbeb27b1fca40931");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
            var HexcrafterMagusHexMagusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            var HexcrafterMagusHexArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");


            var common_coldbuff00 = new PrefabLink() {
                AssetId = "21b65d177b9db1d4ca4961de15645d95"
            };

            var Icon_IceTomb = IcyPrison.Icon;
            //var Icon_IceTomb = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_CloakOfWinds.png");

            var IceTombBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexIceTombBuff", bp => {
                bp.SetName(TTTContext, "Ice Tomb");
                bp.SetDescription(TTTContext, "A storm of ice and freezing wind envelops the target, which takes 3d8 points of cold damage (Fortitude half). " +
                    "If the target fails its save, it is paralyzed and unconscious but does not need to eat or breathe while the ice lasts.\n" +
                    "A creature can break the ice with a successful Strength check (DC 15 + your witch level), or by taking more than 20 damage. " +
                    "A creature who breaks free is staggered for 1d4 rounds after being released.\n" +
                    "Whether or not the target’s saving throw is successful, it cannot be the target of this hex again for 1 day.");
                bp.m_Icon = Icon_IceTomb;
                bp.ResourceAssetIds = new string[] { common_coldbuff00.AssetId };
                bp.FxOnStart = common_coldbuff00;
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Hex | SpellDescriptor.Paralysis | SpellDescriptor.Cold | SpellDescriptor.MovementImpairing;
                });
                bp.AddComponent<AddCondition>(c => {
                    c.Condition = UnitCondition.Paralyzed;
                });
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            IsNotDispelable = true,
                            AsChild = false,
                            m_Buff = Staggered,
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.D4,
                                DiceCountValue = 1,
                                BonusValue = 0
                            }
                        }
                    );
                    c.NewRound = Helpers.CreateActionList(
                        new ContextActionSkillCheck() {
                            m_ConditionalDCIncrease = new ContextActionSkillCheck.ConditionalDCIncrease[] {
                                new ContextActionSkillCheck.ConditionalDCIncrease() {
                                    Condition = new ConditionsChecker(){
                                        Conditions = new Condition[0]
                                    },
                                    Value = 15
                                }
                            },
                            Stat = StatType.Strength,
                            UseCustomDC = true,
                            CustomDC = new ContextValue() {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageDice
                            },
                            Success = Helpers.CreateActionList(
                                new ContextActionRemoveSelf()
                            ),
                            Failure = Helpers.CreateActionList(),
                            FailureDiffMoreOrEqual10 = Helpers.CreateActionList(),
                            FailureDiffMoreOrEqual5Less10 = Helpers.CreateActionList()
                        }
                    );
                });
                bp.AddComponent<TemporaryHitPointsFromAbilityValue>(c => {
                    c.Value = 20;
                    c.RemoveWhenHitPointsEnd = true;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
            });
            var IceTombCooldownBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexIceTombCooldownBuff", bp => {
                bp.SetName(TTTContext, "Ice Tomb Cooldown");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = Icon_IceTomb;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });
            var IceTombAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "WitchHexIceTombAbility", bp => {
                bp.SetName(TTTContext, "Ice Tomb");
                bp.SetDescription(IceTombBuff.m_Description);
                bp.SetLocalizedSavingThrow(TTTContext, "Fortitude negates");
                bp.SetLocalizedDuration(TTTContext, "1 minute/level");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Reach;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Long;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.CanTargetEnemies = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_IceTomb;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Fortitude;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            IsNotDispelable = true,
                            m_Buff = IceTombCooldownBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Days,
                                BonusValue = 1,
                                DiceCountValue = 0
                            }
                        },
                        new ContextActionDealDamage() {
                            DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Cold
                            },
                            Duration = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            },
                            Value = new ContextDiceValue() {
                                DiceType = DiceType.D8,
                                DiceCountValue = 3,
                                BonusValue = 0
                            },
                            HalfIfSaved = true
                        },
                        new ContextActionConditionalSaved() {
                            Succeed = Helpers.CreateActionList(),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = IceTombBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Minutes,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        },
                                        DiceCountValue = 0
                                    }
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[] {
                        ClassTools.ClassReferences.WitchClass,
                        ClassTools.ClassReferences.WinterWitchClass,
                        ClassTools.ClassReferences.MagusClass,
                        ClassTools.ClassReferences.RogueClass,
                    };
                    c.Archetype = SylvanTricksterArchetype;
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[] {
                        HexcrafterArchetype
                    };
                });
                bp.AddComponent<AbilityTargetHasFact>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        IceTombCooldownBuff.ToReference<BlueprintUnitFactReference>()
                    };
                    c.Inverted = true;
                });
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = WitchHexDCProperty
                    };
                    c.CasterLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = WitchHexCasterLevelProperty
                    };
                    c.Concentration = new ContextValue();
                    c.SpellLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = WitchHexSpellLevelProperty
                    };
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Hex | SpellDescriptor.Cold;
                });
            });
            var IceTombFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WitchHexIceTombFeature", bp => {
                bp.SetName(TTTContext, "Ice Tomb");
                bp.SetDescription(IceTombBuff.m_Description);
                bp.m_Icon = Icon_IceTomb;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WitchHex };
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { IceTombAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddPrerequisiteFeature(WitchMajorHex);
            });

            if (TTTContext.AddedContent.Hexes.IsDisabled("IceTomb")) { return; }
            WitchHexSelection.AddFeatures(IceTombFeature);
            WinterWitchWitchHex.AddFeatures(IceTombFeature);
            SylvanTricksterTalentSelection.AddFeatures(IceTombFeature);
            HexcrafterMagusHexMagusSelection.AddFeatures(IceTombFeature);
            HexcrafterMagusHexArcanaSelection.AddFeatures(IceTombFeature);
        }
    }
}
