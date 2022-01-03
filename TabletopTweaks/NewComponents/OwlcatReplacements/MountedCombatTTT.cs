using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using UnityEngine;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [TypeId("6529138e5e96494f958a758ee21e451e")]
    class MountedCombatTTT : UnitFactComponentDelegate, IRulebookHandler<RuleAttackRoll>, IGlobalRulebookHandler<RuleAttackRoll>, ISubscriber, IGlobalSubscriber {

        public BlueprintBuff CooldownBuff {
            get {
                if (m_CooldownBuff == null) {
                    return null;
                }
                return m_CooldownBuff.Get();
            }
        }
        public BlueprintFeature TrickRidingFeature {
            get {
                if (m_TrickRidingFeature == null) {
                    return null;
                }
                return m_TrickRidingFeature.Get();
            }
        }
        public BlueprintBuff TrickRidingCooldownBuff {
            get {
                if (m_TrickRidingCooldownBuff == null) {
                    return null;
                }
                return m_TrickRidingCooldownBuff.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
            if (base.Fact?.Owner == null) {
                return;
            }
            if (evt.RuleAttackWithWeapon == null) {
                return;
            }
            if (evt.RuleAttackWithWeapon.Target != Owner.GetSaddledUnit()) {
                return;
            }
            if (!evt.IsHit) {
                return;
            }
            var hasTrickRiding = Owner.HasFact(TrickRidingFeature);
            var hasTrickRidingCooldown = Owner.HasFact(TrickRidingCooldownBuff);
            var hasMountedCombatCooldown = Owner.HasFact(CooldownBuff);

            if (hasMountedCombatCooldown && !hasTrickRiding) {
                return;
            }
            if (hasMountedCombatCooldown && hasTrickRidingCooldown) {
                return;
            }
            Main.Log($"IsHit: {evt.IsHit} - {evt.Result}");
            bool success = GameHelper.TriggerSkillCheck(new RuleSkillCheck(base.Owner, StatType.SkillMobility, evt.Roll), null, false).Success;
            if (!hasMountedCombatCooldown) {
                GameHelper.ApplyBuff(Owner, CooldownBuff, new Rounds?(1.Rounds()));
            } else {
                GameHelper.ApplyBuff(Owner, TrickRidingCooldownBuff, new Rounds?(1.Rounds()));
            }
            if (success) {
                evt.AutoMiss = true;
            }
        }

        [SerializeField]
        public BlueprintBuffReference m_CooldownBuff;
        [SerializeField]
        public BlueprintFeatureReference m_TrickRidingFeature;
        [SerializeField]
        public BlueprintBuffReference m_TrickRidingCooldownBuff;
    }
}
