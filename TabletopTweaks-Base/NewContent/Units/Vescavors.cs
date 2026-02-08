using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Units {
    public static class Vescavors {
        public static void AddVescavorApplyGibberImmunity() {
            var VescavorQueenGibberImmunityBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("15957616e7b46b34d9a4a92daf3c3ac8");
            var VescavorSwarmGibberImmunityBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("91d3d7a6a409c32418c98859bcd58844");

            var VescavorSwarmApplyGibberImmunityAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "VescavorSwarmApplyGibberImmunityAbility", bp => {
                bp.SetName(TTTContext, "Apply Gibber Immunity");
                bp.SetDescription(TTTContext, "Applies Gibber Immunity");
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.LocalizedDuration", "24 Hours");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Long;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                bp.DisableLog = true;

                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = VescavorSwarmGibberImmunityBuff,
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                Rate = DurationRate.Hours,
                                DiceType = DiceType.One,
                                DiceCountValue = 0,
                                BonusValue = 24
                            }
                        }
                    );
                });
            });
            var VescavorQueenApplyGibberImmunityAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "VescavorQueenApplyGibberImmunityAbility", bp => {
                bp.SetName(TTTContext, "Apply Gibber Queen Immunity");
                bp.SetDescription(TTTContext, "Applies Gibber Queen Immunity");
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.LocalizedDuration", "24 Hours");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Long;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                bp.DisableLog = true;

                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = VescavorQueenGibberImmunityBuff,
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                Rate = DurationRate.Hours,
                                DiceType = DiceType.One,
                                DiceCountValue = 0,
                                BonusValue = 24
                            }
                        }
                    );
                });
            });
        }
    }
}
