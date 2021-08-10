using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    class IndomitableMountFixed : UnitFactComponentDelegate, IGlobalRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IGlobalSubscriber {

        public BlueprintBuff CooldownBuff {
            get {
                BlueprintBuffReference cooldownBuff = m_CooldownBuff;
                if (cooldownBuff == null) {
                    return null;
                }
                return cooldownBuff.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
            Main.LogDebug("IndomitableMount - Saving Throw Check");
            if (base.Fact?.Owner == null) {
                Main.LogDebug("IndomitableMount - Owner == null");
                return;
            }
            if (evt.Initiator != Owner.GetSaddledUnit()) {
                Main.LogDebug("IndomitableMount - Target != Owner.GetSaddledUnit()");
                return;
            }
            if (evt.IsPassed) {
                Main.LogDebug($"IndomitableMount - evt.IsPassed - {evt.IsPassed}");
                return;
            }
            if (Owner.HasFact(CooldownBuff)) {
                Main.LogDebug($"IndomitableMount - hasFact - CooldownBuff");
                return;
            }
            Main.LogDebug("IndomitableMount - Triggered");
            int difficultyClass = evt.DifficultyClass;
            bool success = GameHelper.TriggerSkillCheck(new RuleSkillCheck(Owner, StatType.SkillMobility, difficultyClass), null, false).Success;
            GameHelper.ApplyBuff(Owner, CooldownBuff, new Rounds?(1.Rounds()));
            if (success) {
                evt.IsAlternativePassed = new bool?(true);
                Main.LogDebug("IndomitableMount - Result");
                Main.LogDebug($"IsAlternativePassed: {evt.IsAlternativePassed}");
            }
        }

        [SerializeField]
        [FormerlySerializedAs("CooldownBuff")]
        public BlueprintBuffReference m_CooldownBuff;
    }
}
