using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Extensions {
    static class ExtentionMethods {
        public static GameAction[] FlattenAllActions(this BlueprintAbility Ability) {
            return
                Ability.GetComponents<AbilityExecuteActionOnCast>()
                    .SelectMany(a => a.FlattenAllActions())
                .Concat(
                Ability.GetComponents<AbilityEffectRunAction>()
                    .SelectMany(a => a.FlattenAllActions()))
                .ToArray();
        }

        public static GameAction[] FlattenAllActions(this AbilityExecuteActionOnCast Action) {
            return FlattenAllActions(Action.Actions.Actions);
        }

        public static GameAction[] FlattenAllActions(this AbilityEffectRunAction Action) {
            return FlattenAllActions(Action.Actions.Actions);
        }

        public static GameAction[] FlattenAllActions(GameAction[] Actions) {
            List<GameAction> NewActions = new List<GameAction>();
            NewActions.AddRange(Actions.OfType<ContextActionConditionalSaved>().SelectMany(a => a.Failed.Actions));
            NewActions.AddRange(Actions.OfType<ContextActionConditionalSaved>().SelectMany(a => a.Succeed.Actions));
            NewActions.AddRange(Actions.OfType<Conditional>().SelectMany(a => a.IfFalse.Actions));
            NewActions.AddRange(Actions.OfType<Conditional>().SelectMany(a => a.IfTrue.Actions));
            if (NewActions.Count > 0) {
                return Actions.Concat(FlattenAllActions(NewActions.ToArray())).ToArray();
            }
            return Actions.ToArray();
        }
    }
}
