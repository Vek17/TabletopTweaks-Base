using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using Kingmaker.Visual.Particles;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [TypeId("ae9e3debc1c64fc5bd2ba05e34541293")]
    public class WeaponBuffOnConfirmedCritTTT : WeaponEnchantmentLogic, 
        IInitiatorRulebookHandler<RuleAttackWithWeapon>, 
        IRulebookHandler<RuleAttackWithWeapon>, 
        ISubscriber, IInitiatorRulebookSubscriber, IResourcesHolder {
        public BlueprintBuff Buff {
            get {
                BlueprintBuffReference buff = this.m_Buff;
                if (buff == null) {
                    return null;
                }
                return buff.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
            if (!this.OnlyNatural20 || (evt.AttackRoll.D20.Result == 20 && evt.AttackRoll.IsCriticalConfirmed)) {
                if (OnTarget) {
                    evt.Target.Descriptor.AddBuff(Buff, base.Owner.Wielder.Unit, new TimeSpan?(Duration.Seconds), null);
                    FxHelper.SpawnFxOnUnit(Fx.Load(false), evt.Target.View, null, default);
                }
                evt.Initiator.Descriptor.AddBuff(Buff, base.Owner.Wielder.Unit, new TimeSpan?(Duration.Seconds), null);
                FxHelper.SpawnFxOnUnit(Fx.Load(false), evt.Initiator.View, null, default);
            }
        }

        [SerializeField]
        [FormerlySerializedAs("Buff")]
        public BlueprintBuffReference m_Buff;

        public Rounds Duration;
        public PrefabLink Fx;
        public bool OnlyNatural20;
        public bool OnTarget;
    }
}
