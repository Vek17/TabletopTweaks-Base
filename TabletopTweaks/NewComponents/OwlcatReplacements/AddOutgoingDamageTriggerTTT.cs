using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Armies.TacticalCombat.Parts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [AllowMultipleComponents]
    [TypeId("d9558dff3102481dbd918c2abdd0c95b")]
    class AddOutgoingDamageTriggerTTT : UnitFactComponentDelegate<AddOutgoingDamageTriggerTTT.ComponentData>,
        IInitiatorRulebookHandler<RuleDealDamage>,
        IRulebookHandler<RuleDealDamage>,
        IInitiatorRulebookHandler<RuleDealStatDamage>,
        IRulebookHandler<RuleDealStatDamage>,
        IInitiatorRulebookHandler<RuleDrainEnergy>,
        IRulebookHandler<RuleDrainEnergy>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public BlueprintWeaponType WeaponType {
            get {
                BlueprintWeaponTypeReference weaponType = m_WeaponType;
                if (weaponType == null) {
                    return null;
                }
                return weaponType.Get();
            }
        }

        public ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> AbilityList {
            get {
                return m_AbilityList;
            }
        }

        private void RunAction(RulebookEvent e, UnitEntityData target) {
            if ((!IgnoreDamageFromThisFact || e.Reason.Fact != base.Fact) && Actions.HasActions) {
                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner == null) {
                    return;
                }
                factContextOwner.RunActionInContext(Actions, target);
            }
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt) {
            if (TacticalCombatHelper.IsActive) {
                UnitPartTacticalCombat unitPartTacticalCombat = evt.Target.Get<UnitPartTacticalCombat>();
                int num = (unitPartTacticalCombat != null) ? unitPartTacticalCombat.Count : 1;
                base.Data.WasTargetAlive = (num > TacticalCombatHelper.GetDeathCount(evt.Target, evt.Target.HPLeft, num));
                return;
            }
            base.Data.WasTargetAlive = !evt.Target.Descriptor.State.IsDead;
        }

        public void OnEventAboutToTrigger(RuleDealStatDamage evt) {
        }

        public void OnEventAboutToTrigger(RuleDrainEnergy evt) {
        }

        public void OnEventDidTrigger(RuleDealDamage evt) {
            if (CheckDamageValue(evt.Result)) {
                Apply(evt);
            }
            base.Data.LastAttack = evt.AttackRoll;
        }

        public void OnEventDidTrigger(RuleDealStatDamage evt) {
            if (TriggerOnStatDamageOrEnergyDrain && CheckDamageValue(evt.Result)) {
                RunAction(evt, evt.Target);
            }
        }

        public void OnEventDidTrigger(RuleDrainEnergy evt) {
            if (TriggerOnStatDamageOrEnergyDrain && CheckDamageValue(evt.DrainValue)) {
                RunAction(evt, evt.Target);
            }
        }

        private void Apply(RuleDealDamage evt) {
            if (OncePerAttackRoll && base.Data.LastAttack != null) {
                if (base.Data.LastAttack == evt.AttackRoll) {
                    return;
                }
            }
            if (CheckWeaponType) {
                ItemEntityWeapon weapon = evt.DamageBundle.Weapon;
                if (((weapon != null) ? weapon.Blueprint.Type : null) != WeaponType) {
                    return;
                }
            }
            if (CheckAbilityType) {
                AbilityType? abilityType = evt.Reason.Ability?.Blueprint?.Type ?? evt.Reason.Context?.SourceAbility?.Type;
                AbilityType abilityType2 = m_AbilityType;
                if (!(abilityType.GetValueOrDefault() == abilityType2 & abilityType != null)) {
                    return;
                }
            }
            if (CheckSpellDescriptor && (evt.Reason.Ability == null || !evt.Reason.Ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptorsList))) {
                return;
            }
            if (CheckEnergyDamageType) {
                bool flag = false;
                foreach (BaseDamage baseDamage in evt.DamageBundle) {
                    if (baseDamage.Type == DamageType.Energy && (baseDamage as EnergyDamage).EnergyType == EnergyType) {
                        flag = true;
                        break;
                    }
                }
                if (!flag) {
                    return;
                }
            }
            bool flag2 = evt.Reason.Ability != null && (AbilityList.Contains(evt.Reason.Ability.Blueprint) || AbilityList.Contains(evt.Reason.Ability.Blueprint.Parent));
            bool flag3 = (evt.SourceAbility != null && (AbilityList.Contains(evt.SourceAbility) || AbilityList.Contains(evt.SourceAbility.Parent))) || flag2;
            if (!ApplyToAreaEffectDamage && evt.SourceArea) {
                return;
            }
            if (CheckSpellParent && !flag3) {
                return;
            }
            bool flag4;
            if (TacticalCombatHelper.IsActive) {
                UnitPartTacticalCombat unitPartTacticalCombat = evt.Target.Get<UnitPartTacticalCombat>();
                int num = (unitPartTacticalCombat != null) ? unitPartTacticalCombat.Count : 1;
                flag4 = (num <= TacticalCombatHelper.GetDeathCount(evt.Target, evt.Target.HPLeft, num));
            } else {
                flag4 = (evt.Target.Descriptor.Stats.HitPoints <= evt.Target.Descriptor.Damage);
            }
            if (TargetKilledByThisDamage && (!base.Data.WasTargetAlive || !flag4)) {
                return;
            }
            RunAction(evt, evt.Target);
        }

        private bool CheckDamageValue(int damageValue) {
            return !CheckDamageDealt || CompareType.CheckCondition(damageValue, TargetValue.Calculate(base.Fact.MaybeContext));
        }

        public class ComponentData {
            public bool WasTargetAlive;
            public RuleAttackRoll LastAttack;
        }

        public ActionList Actions;
        public bool TriggerOnStatDamageOrEnergyDrain;
        public bool CheckWeaponType;
        public bool CheckAbilityType;
        public AbilityType m_AbilityType;
        public bool CheckSpellDescriptor;
        public bool CheckSpellParent;
        public bool NotZeroDamage;
        public bool CheckDamageDealt;
        public CompareOperation.Type CompareType;
        public ContextValue TargetValue;
        public bool CheckEnergyDamageType;
        public DamageEnergyType EnergyType;
        public bool ApplyToAreaEffectDamage;
        public bool TargetKilledByThisDamage;
        public bool IgnoreDamageFromThisFact = true;
        public bool OncePerAttackRoll;
        public BlueprintWeaponTypeReference m_WeaponType;
        public BlueprintAbilityReference[] m_AbilityList;
        public SpellDescriptorWrapper SpellDescriptorsList;
    }
}
