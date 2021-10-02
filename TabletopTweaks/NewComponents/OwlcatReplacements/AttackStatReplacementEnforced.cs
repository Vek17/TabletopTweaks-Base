using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using UnityEngine;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [ComponentName("Replace attack stat")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("a2f09aa00803458fb96caa907389a8e7")]
    class AttackStatReplacementEnforced : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber,
        IInitiatorRulebookSubscriber {

        public ReferenceArrayProxy<BlueprintWeaponType, BlueprintWeaponTypeReference> WeaponTypes {
            get {
                return this.m_WeaponTypes;
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
            ModifiableValueAttributeStat OwnerAttackBonusStat = Owner.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat OwnerReplacementStat = Owner.Stats.GetStat(ReplacementStat) as ModifiableValueAttributeStat;
            bool ReplaceIsGreater = OwnerReplacementStat != null && OwnerAttackBonusStat != null && OwnerReplacementStat.Bonus >= OwnerAttackBonusStat.Bonus;
            if (((!CheckWeaponTypes) ? evt.Weapon.Blueprint.Category.HasSubCategory(SubCategory) : WeaponTypes.HasReference(evt.Weapon.Blueprint.Type)) && ReplaceIsGreater) {
                evt.AttackBonusStat = ReplacementStat;
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
        }

        public StatType ReplacementStat;

        [HideIf("CheckWeaponTypes")]
        public WeaponSubCategory SubCategory;

        public bool CheckWeaponTypes;

        [ShowIf("CheckWeaponTypes")]
        [SerializeField]
        public BlueprintWeaponTypeReference[] m_WeaponTypes = new BlueprintWeaponTypeReference[0];
    }
}
