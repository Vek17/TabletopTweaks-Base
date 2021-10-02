using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("a07e963bb1e74da1a615e3a426004c47")]
    class TrainedGraceComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {

        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
            UnitPartWeaponTraining unitPartWeaponTraining = Owner.Get<UnitPartWeaponTraining>();
            if (IsSuitable(evt, unitPartWeaponTraining)) {
                evt.DamageDescription[0].AddModifier(new Modifier(unitPartWeaponTraining.GetWeaponRank(evt.Weapon), base.Fact, Descriptor));
            }
        }

        private bool IsSuitable(RuleCalculateWeaponStats evt, UnitPartWeaponTraining unitPartWeaponTraining) {
            var weapon = evt.Weapon;
            var ruleCalculateAttackBonus = new RuleCalculateAttackBonusWithoutTarget(evt.Initiator, weapon, 0);
            ruleCalculateAttackBonus.WeaponStats.m_Triggered = true;
            Rulebook.Trigger(ruleCalculateAttackBonus);

            return unitPartWeaponTraining.IsSuitableWeapon(weapon)
                && (!MeleeOnly || !weapon.Blueprint.IsRanged)
                && (!EnforceGroup || weapon.Blueprint.FighterGroup.Contains(WeaponGroup))
                && (evt.DamageBonusStat == StatType.Strength)
                && ruleCalculateAttackBonus.AttackBonusStat == StatType.Dexterity;
        }

        public bool EnforceGroup;
        public WeaponFighterGroup WeaponGroup;
        public bool MeleeOnly;
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
