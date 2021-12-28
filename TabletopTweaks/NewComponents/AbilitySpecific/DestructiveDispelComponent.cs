using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintFeature), false)]
    [TypeId("ea475e4be98f4eabb361ed8ce58870ad")]
    class DestructiveDispelComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDispelMagic>, IRulebookHandler<RuleDispelMagic>, ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleDispelMagic evt) {
        }

        public void OnEventDidTrigger(RuleDispelMagic evt) {
            if (evt.Initiator.IsAlly(evt.Target)) { return; }
            MechanicsContext maybeContext = base.Fact.MaybeContext;
            if (maybeContext != null && evt.Success && (this.TriggerOnAreaEffectsDispell || evt.AreaEffect == null)) {
                using (maybeContext.GetDataScope(evt.Target)) {
                    int dc = 10 + ((evt.CasterLevel + evt.Bonus) / 2) + getHighestStatBonus(evt.Initiator, StatType.Intelligence, StatType.Wisdom, StatType.Charisma);
                    RuleSavingThrow ruleSavingThrow = base.Context.TriggerRule<RuleSavingThrow>(new RuleSavingThrow(evt.Target, SavingThrowType.Fortitude, dc));

                    ActionOnTarget.Run();
                }
            }
        }

        static private int getHighestStatBonus(UnitEntityData unit, params StatType[] stats) {
            StatType highestStat = StatType.Unknown;
            int highestValue = -1;
            foreach (StatType stat in stats) {
                var value = unit.Stats.GetStat(stat).ModifiedValue;
                if (value > highestValue) {
                    highestStat = stat;
                    highestValue = value;
                }
            }
            return unit.Stats.GetStat<ModifiableValueAttributeStat>(highestStat).Bonus;
        }

        [InfoBox("Use this bool if you want to trigger action on a caster of an AOE effect. Eg: Ember cast Grease, Nenio dispells it -> Ember is target, hence received 1d6 damage ")]
        public bool TriggerOnAreaEffectsDispell;

        public ActionList ActionOnTarget;
    }
}
