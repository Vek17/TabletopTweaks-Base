using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Units {
    static class CarnivorousCrystal {
        public static void AddCarnivorousCrystalApplyImmunity() {
            var CarnivorousCrystalBuffSubsonicHumImmunity = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("88e345f3233c8024e9d191a807c40223");

            var CarnivorousCrystalApplyImmunityAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "CarnivorousCrystalApplyImmunityAbility", bp => {
                bp.SetName(TTTContext, "Apply Subsonic Hum Immunity");
                bp.SetDescription(TTTContext, "Applies Subsonic Hum Immunity");
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
                            m_Buff = CarnivorousCrystalBuffSubsonicHumImmunity,
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
