using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Hexes {
    internal class Retribution {
        public static void AddRetribution() {

            var WitchMajorHex = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8ac781b33e380c84aa578f1b006dd6c5");
            var WitchHexDCProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("bdc230ce338f427ba74de65597b0d57a");
            var WitchHexCasterLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("2d2243f4f3654512bdda92e80ef65b6d");
            var WitchHexSpellLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("75efe8b64a3a4cd09dda28cef156cfb5");
            var WitchCastingStatProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("f47851a7b8c3e6b46b57aa7e06052589");
            var Staggered = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("df3950af5a783bd4d91ab73eb8fa0fd3");

            var WinterWitchWitchHex = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b921af3627142bd4d9cf3aefb5e2610a");
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
            var HexcrafterMagusHexMagusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            var HexcrafterMagusHexArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");

            var witch_commondebuff00 = new PrefabLink() {
                AssetId = "c9f48b149f8dad342a7191ca616326b1"
            };

            var Icon_Retribution = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_Retribution.png");

            var RetributionHexBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexRetributionBuff", bp => {
                bp.SetName(TTTContext, "Retribution");
                bp.SetDescription(TTTContext, "A witch can place a retribution hex on a creature within 60 feet, " +
                    "causing terrible wounds to open across the target’s flesh whenever it deals damage to another creature in melee. " +
                    "Immediately after the hexed creature deals with a melee attack, it takes half that damage (round down). " +
                    "This damage bypasses any resistances, immunities, or damage reduction the creature possesses. " +
                    "This effect lasts for a number of rounds equal to the witch’s Intelligence modifier. " +
                    "A Will save negates this effect.");
                bp.m_Icon = Icon_Retribution;
                bp.ResourceAssetIds = new string[] { witch_commondebuff00.AssetId };
                bp.FxOnStart = witch_commondebuff00;
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Hex;
                });
                bp.AddComponent<DamageRetributionInitiatorComponent>(c => {
                    c.PercentRedirected = 50;
                    c.CheckRangeType = true;
                    c.RangeType = WeaponRangeType.Melee;
                });
            });
            var RetributionHexAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "WitchHexRetributionAbility", bp => {
                bp.SetName(RetributionHexBuff.m_DisplayName);
                bp.SetDescription(RetributionHexBuff.m_Description);
                bp.SetLocalizedSavingThrow(TTTContext, "Will negates");
                bp.SetLocalizedDuration(TTTContext, "1 round/int mod");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Reach;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Long;
                bp.CanTargetFriends = false;
                bp.CanTargetSelf = false;
                bp.CanTargetEnemies = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_Retribution;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionConditionalSaved() {
                            Succeed = Helpers.CreateActionList(),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = RetributionHexBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Rounds,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.CasterCustomProperty,
                                            m_CustomProperty = WitchCastingStatProperty
                                        },
                                        DiceCountValue = 0
                                    }
                                }
                            )
                        }
                    );
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
                    c.Descriptor = SpellDescriptor.Hex;
                });
            });
            var RetributionHexFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WitchHexRetributionFeature", bp => {
                bp.SetName(RetributionHexBuff.m_DisplayName);
                bp.SetDescription(RetributionHexBuff.m_Description);
                bp.m_Icon = Icon_Retribution;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WitchHex };
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { RetributionHexAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddPrerequisiteFeature(WitchMajorHex);
            });

            if (TTTContext.AddedContent.Hexes.IsDisabled("Retribution")) { return; }
            WitchHexSelection.AddFeatures(RetributionHexFeature);
            WinterWitchWitchHex.AddFeatures(RetributionHexFeature);
            SylvanTricksterTalentSelection.AddFeatures(RetributionHexFeature);
            HexcrafterMagusHexMagusSelection.AddFeatures(RetributionHexFeature);
            HexcrafterMagusHexArcanaSelection.AddFeatures(RetributionHexFeature);
        }
    }
}
