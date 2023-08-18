using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
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
    internal class DireProphecy {
        public static void AddDireProphecy() {

            var WitchGrandHex = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d24c2467804ce0e4497d9978bafec1f9");
            var WitchHexDCProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("bdc230ce338f427ba74de65597b0d57a");
            var WitchHexCasterLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("2d2243f4f3654512bdda92e80ef65b6d");
            var WitchHexSpellLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("75efe8b64a3a4cd09dda28cef156cfb5");

            var WinterWitchWitchHex = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b921af3627142bd4d9cf3aefb5e2610a");
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
            var HexcrafterMagusHexMagusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            var HexcrafterMagusHexArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");

            var Icon_DireProphecy = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_DireProphecy.png");

            var deathcurse00_mark = new PrefabLink() {
                AssetId = "f4adcaac22054a042ab43b09bf107dd6"
            };
            var deathcurse00 = new PrefabLink() {
                AssetId = "9a65b11dc100bc34e8313a7249d3d496"
            };
            var WitchHexDireProphecyBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexDireProphecyBuff", bp => {
                bp.SetName(TTTContext, "Dire Prophecy");
                bp.SetDescription(TTTContext, "The witch curses the target so he is doomed to die. " +
                    "As long as the curse persists, the target takes a –4 penalty to his armor class and on attack rolls, saves, ability checks, and skill checks.\n" +
                    "A target can only have one dire prophecy upon him at a time. Whether or not the target’s save against the hex is successful, a creature cannot be the target of this hex for 1 day. This is a curse effect.");
                bp.m_Icon = Icon_DireProphecy;
                bp.Stacking = StackingType.Replace;
                bp.FxOnStart = deathcurse00_mark;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AdditionalAttackBonus;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                });
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                });
                bp.AddComponent<BuffAllSkillsBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                    c.Multiplier = 1;
                });
                bp.AddComponent<BuffAbilityRollsBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                    c.Multiplier = 1;
                    c.AffectAllStats = true;
                });
            });
            var WitchHexDireProphecyBuffCooldownBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexDireProphecyBuffCooldownBuff", bp => {
                bp.SetName(TTTContext, "Dire Prophecy Cooldown");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = Icon_DireProphecy;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });
            var WitchHexDireProphecyAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "WitchHexDireProphecyAbility", bp => {
                bp.SetName(TTTContext, "Dire Prophecy");
                bp.SetDescription(WitchHexDireProphecyBuff.m_Description);
                bp.SetLocalizedSavingThrow(TTTContext, "Will negates");
                bp.SetLocalizedDuration(TTTContext, "1 day");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Reach;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = false;
                bp.CanTargetSelf = false;
                bp.CanTargetEnemies = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_DireProphecy;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionSavingThrow() {
                            Type = SavingThrowType.Will,
                            Actions = Helpers.CreateActionList(
                                new ContextActionConditionalSaved() {
                                    Succeed = Helpers.CreateActionList(
                                        new ContextActionApplyBuff() {
                                            IsFromSpell = true,
                                            IsNotDispelable = true,
                                            m_Buff = WitchHexDireProphecyBuffCooldownBuff.ToReference<BlueprintBuffReference>(),
                                            DurationValue = new ContextDurationValue() {
                                                Rate = DurationRate.Days,
                                                BonusValue = 1,
                                                DiceCountValue = 0
                                            }
                                        }
                                    ),
                                    Failed = Helpers.CreateActionList(
                                        new ContextActionApplyBuff() {
                                            m_Buff = WitchHexDireProphecyBuff.ToReference<BlueprintBuffReference>(),
                                            DurationValue = new ContextDurationValue() {
                                                Rate = DurationRate.Days,
                                                BonusValue = 1,
                                                DiceCountValue = 0
                                            }
                                        },
                                        new ContextActionSpawnFx() {
                                            PrefabLink = deathcurse00
                                        }
                                    )
                                }
                            )
                        }
                    );
                });
                bp.AddComponent<AbilityTargetHasFact>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        WitchHexDireProphecyBuffCooldownBuff.ToReference<BlueprintUnitFactReference>()
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
                    c.Descriptor = SpellDescriptor.Hex | SpellDescriptor.Curse;
                });
            });
            var WitchHexDireProphecyFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WitchHexDireProphecyFeature", bp => {
                bp.SetName(TTTContext, "Dire Prophecy");
                bp.SetDescription(WitchHexDireProphecyBuff.m_Description);
                bp.m_Icon = Icon_DireProphecy;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WitchHex };
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { WitchHexDireProphecyAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddPrerequisiteFeature(WitchGrandHex);
            });

            if (TTTContext.AddedContent.Hexes.IsDisabled("DireProphecy")) { return; }
            WitchHexSelection.AddFeatures(WitchHexDireProphecyFeature);
            WinterWitchWitchHex.AddFeatures(WitchHexDireProphecyFeature);
            SylvanTricksterTalentSelection.AddFeatures(WitchHexDireProphecyFeature);
            HexcrafterMagusHexMagusSelection.AddFeatures(WitchHexDireProphecyFeature);
            HexcrafterMagusHexArcanaSelection.AddFeatures(WitchHexDireProphecyFeature);
        }
    }
}
