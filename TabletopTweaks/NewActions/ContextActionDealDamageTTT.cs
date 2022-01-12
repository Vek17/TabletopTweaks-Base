using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Validation;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.Utility;
using UnityEngine;

namespace TabletopTweaks.NewActions {
    class ContextActionDealDamageTTT : ContextAction, IValidated, IDealDamageProvider {
        private bool IsSimpleDamage {
            get {
                return m_Type == ContextActionDealDamage.Type.Damage;
            }
        }

        private bool IsAbilityDamage {
            get {
                return m_Type == ContextActionDealDamage.Type.AbilityDamage;
            }
        }

        private bool IsEnergyDrain {
            get {
                return m_Type == ContextActionDealDamage.Type.EnergyDrain;
            }
        }

        private bool IsTemporaryEnergyDrain {
            get {
                return m_Type == ContextActionDealDamage.Type.EnergyDrain && EnergyDrainType != EnergyDrainType.Permanent;
            }
        }

        private bool ShowAlreadyHalved {
            get {
                return IsSimpleDamage && Half && ReadPreRolledFromSharedValue;
            }
        }

        public override string GetCaption() {
            string text;
            switch (m_Type) {
                case ContextActionDealDamage.Type.Damage:
                    text = (Half ? string.Format("Deal half {0} of {1}", Value, DamageType) : string.Format("Deal {0} of {1}", Value, DamageType));
                    break;
                case ContextActionDealDamage.Type.AbilityDamage:
                    text = (Drain ? string.Format("Drain {0} of {1}", Value, AbilityType) : string.Format("Deal {0} damage to {1}", Value, AbilityType));
                    break;
                case ContextActionDealDamage.Type.EnergyDrain: {
                        string arg = (EnergyDrainType == EnergyDrainType.SaveOrBecamePermanent) ? " (save or become permanent)" : "";
                        text = ((EnergyDrainType == EnergyDrainType.Permanent) ? string.Format("{0} negative levels", Value) : string.Format("{0} negative levels on {1}{2}", Value, Duration, arg));
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (WriteResultToSharedValue) {
                text += string.Format(" >> {0}", ResultSharedValue);
            }
            return text;
        }

        public override void RunAction() {
            if (base.Target.Unit == null) {
                PFLog.Default.Error(this, "Invalid target for effect '{0}'", new object[]
                {
                    base.GetType().Name
                });
                return;
            }
            if (base.Context.SavingThrow != null && (base.Context.SavingThrow.IsPassed || base.Context.SavingThrow.ImprovedEvasion) && HalfIfSaved && base.Context.SavingThrow.Evasion && base.Context.SavingThrow.IsPassed) {
                EventBus.RaiseEvent<IUnitEvasionHandler>(delegate (IUnitEvasionHandler h) {
                    h.HandleEffectEvaded(base.Target.Unit, base.Context);
                }, true);
                return;
            }
            ContextAttackData contextAttackData = ContextData<ContextAttackData>.Current;
            RuleAttackRoll ruleAttackRoll = (contextAttackData != null) ? contextAttackData.AttackRoll : null;
            ContextActionDealDamage.DamageInfo damageInfo = GetDamageInfo();
            ContextActionDealDamage.DamageResult damageResult;
            switch (m_Type) {
                case ContextActionDealDamage.Type.Damage:
                    damageResult = DealHitPointsDamage(damageInfo);
                    break;
                case ContextActionDealDamage.Type.AbilityDamage:
                    damageResult = DealAbilityScoreDamage(damageInfo);
                    break;
                case ContextActionDealDamage.Type.EnergyDrain:
                    damageResult = DrainEnergy(damageInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (WriteResultToSharedValue) {
                base.Context[ResultSharedValue] = (WriteRawResultToSharedValue ? damageResult.RawResult : damageResult.Result);
            }
            if (WriteCriticalToSharedValue && ruleAttackRoll != null && ruleAttackRoll.IsCriticalConfirmed) {
                base.Context[CriticalSharedValue] = 1;
            }
        }

        private ContextActionDealDamage.DamageInfo GetDamageInfo() {
            bool halfBecauseSavingThrow = base.Context.SavingThrow != null && (base.Context.SavingThrow.IsPassed || base.Context.SavingThrow.ImprovedEvasion) && HalfIfSaved;
            ContextAttackData contextAttackData = ContextData<ContextAttackData>.Current;
            RuleAttackRoll ruleAttackRoll = (contextAttackData != null) ? contextAttackData.AttackRoll : null;
            return new ContextActionDealDamage.DamageInfo {
                Dices = new DiceFormula(Value.DiceCountValue.Calculate(base.Context), Value.DiceType),
                Bonus = Value.BonusValue.Calculate(base.Context),
                PreRolledValue = (ReadPreRolledFromSharedValue ? new int?(base.Context[PreRolledSharedValue]) : null),
                HalfBecauseSavingThrow = halfBecauseSavingThrow,
                Empower = base.Context.HasMetamagic(Metamagic.Empower),
                Maximize = base.Context.HasMetamagic(Metamagic.Maximize),
                CriticalModifier = ((!IgnoreCritical && ruleAttackRoll != null && ruleAttackRoll.IsCriticalConfirmed) ? ((base.AbilityContext != null) ? DamageCriticalModifierTypeExtension.FromInt(ruleAttackRoll.WeaponStats.CriticalMultiplier) : new DamageCriticalModifierType?(ruleAttackRoll.Weapon.Blueprint.CriticalModifier)) : null)
            };
        }

        private ContextActionDealDamage.DamageResult DrainEnergy(ContextActionDealDamage.DamageInfo info) {
            if (base.Context.MaybeCaster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return default(ContextActionDealDamage.DamageResult);
            }
            RuleDrainEnergy ruleDrainEnergy = new RuleDrainEnergy(base.Context.MaybeCaster, base.Target.Unit, EnergyDrainType, IsTemporaryEnergyDrain ? new TimeSpan?(Duration.Calculate(base.Context).Seconds) : null, new DiceFormula(Value.DiceCountValue.Calculate(base.Context), Value.DiceType), Value.BonusValue.Calculate(base.Context));
            ruleDrainEnergy.CriticalModifier = info.CriticalModifier;
            ruleDrainEnergy.Empower = info.Empower;
            ruleDrainEnergy.Maximize = info.Maximize;
            ruleDrainEnergy.ParentContext = base.Context;
            RuleSavingThrow savingThrow = base.Context.SavingThrow;
            ruleDrainEnergy.SavingThrowType = ((savingThrow != null) ? savingThrow.Type : SavingThrowType.Fortitude);
            RuleDrainEnergy ruleDrainEnergy2 = ruleDrainEnergy;
            base.Context.TriggerRule<RuleDrainEnergy>(ruleDrainEnergy2);
            return new ContextActionDealDamage.DamageResult {
                Result = ruleDrainEnergy2.Result,
                RawResult = ruleDrainEnergy2.Result
            };
        }

        private ContextActionDealDamage.DamageResult DealHitPointsDamage(ContextActionDealDamage.DamageInfo info) {
            if (base.Context.MaybeCaster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return default(ContextActionDealDamage.DamageResult);
            }
            int bonus;
            RuleDealDamage damageRule = GetDamageRule(info, out bonus);
            base.Context.TriggerRule<RuleDealDamage>(damageRule);
            AbilityExecutionContext abilityContext = base.AbilityContext;
            if (abilityContext != null && abilityContext.HasMetamagic(Metamagic.Bolstered)) {
                abilityContext.BolsteredMetamagicTargets.Add(base.Target.Unit);
                foreach (UnitEntityData unitEntityData in MetamagicHelper.GetBolsteredAreaEffectUnits(base.Target.Unit.Position)) {
                    if (!abilityContext.BolsteredMetamagicTargets.Contains(unitEntityData)) {
                        abilityContext.BolsteredMetamagicTargets.Add(unitEntityData);
                        BaseDamage baseDamage = DamageType.GetDamageDescriptor(DiceFormula.Zero, bonus).CreateDamage();
                        base.Context.TriggerRule<RuleDealDamage>(new RuleDealDamage(base.Context.MaybeCaster, unitEntityData, baseDamage) {
                            DisablePrecisionDamage = true,
                            HalfBecauseSavingThrow = info.HalfBecauseSavingThrow,
                            SourceAbility = base.Context.SourceAbility,
                            SourceArea = (base.Context.AssociatedBlueprint as BlueprintAbilityAreaEffect),
                            Half = baseDamage.Half,
                            AlreadyHalved = baseDamage.AlreadyHalved
                        });
                    }
                }
            }
            return new ContextActionDealDamage.DamageResult {
                Result = damageRule.Result,
                RawResult = damageRule.RawResult
            };
        }

        private ContextActionDealDamage.DamageResult DealAbilityScoreDamage(ContextActionDealDamage.DamageInfo info) {
            if (base.Context.MaybeCaster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return default(ContextActionDealDamage.DamageResult);
            }
            RuleDealStatDamage ruleDealStatDamage = new RuleDealStatDamage(base.Context.MaybeCaster, base.Target.Unit, AbilityType, info.Dices, info.Bonus) {
                Empower = info.Empower,
                Maximize = info.Maximize,
                CriticalModifier = info.CriticalModifier,
                IsDrain = Drain,
                HalfBecauseSavingThrow = info.HalfBecauseSavingThrow,
                MinStatScoreAfterDamage = (UseMinHPAfterDamage ? new int?(MinHPAfterDamage) : null)
            };
            base.Context.TriggerRule<RuleDealStatDamage>(ruleDealStatDamage);
            return new ContextActionDealDamage.DamageResult {
                Result = ruleDealStatDamage.Result,
                RawResult = ruleDealStatDamage.Result
            };
        }

        public void Validate(ValidationContext context, string objectPath) {
            if (IsAbilityDamage && !AbilityType.IsAttribute()) {
                context.AddError("Can't deal damage to stat {0}", new object[]
                {
                    AbilityType
                });
            }
        }

        public RuleDealDamage GetDamageRule(UnitEntityData initiator, UnitEntityData target) {
            if (m_Type != ContextActionDealDamage.Type.Damage) {
                return null;
            }
            ContextActionDealDamage.DamageInfo damageInfo = GetDamageInfo();
            int num;
            return GetDamageRule(damageInfo, out num);
        }

        private RuleDealDamage GetDamageRule(ContextActionDealDamage.DamageInfo info, out int bolsteredBonus) {
            bolsteredBonus = MetamagicHelper.GetBolsteredDamageBonus(base.Context, info.Dices);
            BaseDamage baseDamage = DamageType.GetDamageDescriptor(info.Dices, info.Bonus + bolsteredBonus).CreateDamage();
            baseDamage.EmpowerBonus = (info.Empower ? 1.5f : baseDamage.EmpowerBonus);
            if (info.Maximize) {
                baseDamage.CalculationType = DamageCalculationType.Maximized;
            }
            baseDamage.CriticalModifier = ((info.CriticalModifier != null) ? new int?(info.CriticalModifier.GetValueOrDefault().IntValue()) : null);
            bool flag = info.PreRolledValue != null && Half && AlreadyHalved;
            if (info.PreRolledValue != null) {
                baseDamage.PreRolledValue = info.PreRolledValue + (flag ? (bolsteredBonus / 2) : bolsteredBonus);
            }
            baseDamage.SourceFact = ContextDataHelper.GetFact();
            ContextAttackData contextAttackData = ContextData<ContextAttackData>.Current;
            DamageBundle damageBundle = new DamageBundle(new BaseDamage[]
            {
                baseDamage
            });
            ItemEntityWeapon weapon;
            if (contextAttackData == null) {
                weapon = null;
            } else {
                RuleAttackRoll attackRoll = contextAttackData.AttackRoll;
                weapon = ((attackRoll != null) ? attackRoll.Weapon : null);
            }
            damageBundle.Weapon = !IgnoreWeapon ? weapon : null;
            DamageBundle damage = damageBundle;
            RuleDealDamage ruleDealDamage = new RuleDealDamage(base.Context.MaybeCaster, base.Target.Unit, damage) {
                DisablePrecisionDamage = base.Context.HasMetamagic(Metamagic.Bolstered),
                Projectile = ((contextAttackData != null) ? contextAttackData.Projectile : null),
                AttackRoll = ((contextAttackData != null) ? contextAttackData.AttackRoll : null),
                HalfBecauseSavingThrow = info.HalfBecauseSavingThrow,
                MinHPAfterDamage = (UseMinHPAfterDamage ? new int?(MinHPAfterDamage) : null),
                SourceAbility = base.Context.SourceAbility,
                SourceArea = (base.Context.AssociatedBlueprint as BlueprintAbilityAreaEffect),
                Half = Half,
                AlreadyHalved = flag
            };
            RuleReason ruleReason = null;
            bool flag2;
            if (contextAttackData == null) {
                flag2 = (null != null);
            } else {
                RuleAttackRoll attackRoll2 = contextAttackData.AttackRoll;
                flag2 = (((attackRoll2 != null) ? attackRoll2.Reason : null) != null);
            }
            if (flag2 && !SetFactAsReason) {
                RuleReason proto;
                if (contextAttackData == null) {
                    proto = null;
                } else {
                    RuleAttackRoll attackRoll3 = contextAttackData.AttackRoll;
                    proto = ((attackRoll3 != null) ? attackRoll3.Reason : null);
                }
                ruleReason = new RuleReason(proto, null, baseDamage.SourceFact);
            } else if (baseDamage.SourceFact != null) {
                ruleReason = baseDamage.SourceFact;
            }
            if (ruleReason != null) {
                ruleDealDamage.Reason = ruleReason;
            }
            return ruleDealDamage;
        }

        [SerializeField]
        private ContextActionDealDamage.Type m_Type;
        public DamageTypeDescription DamageType;
        public bool Drain;
        public StatType AbilityType;
        public EnergyDrainType EnergyDrainType;
        public ContextDurationValue Duration;
        public bool ReadPreRolledFromSharedValue;
        public AbilitySharedValue PreRolledSharedValue;
        public ContextDiceValue Value;
        public bool Half;
        public bool AlreadyHalved;
        public bool HalfIfSaved;
        public bool IgnoreCritical;
        public bool UseMinHPAfterDamage;
        public int MinHPAfterDamage;
        public bool WriteResultToSharedValue;
        public bool WriteRawResultToSharedValue;
        public AbilitySharedValue ResultSharedValue;
        public bool WriteCriticalToSharedValue;
        public AbilitySharedValue CriticalSharedValue;
        public bool SetFactAsReason;
        public bool IgnoreWeapon;
        private enum Type {
            Damage,
            AbilityDamage,
            EnergyDrain
        }
        private struct DamageInfo {
            public DiceFormula Dices;
            public int Bonus;
            public int? PreRolledValue;
            public bool HalfBecauseSavingThrow;
            public bool Empower;
            public bool Maximize;
            public DamageCriticalModifierType? CriticalModifier;
        }
        private struct DamageResult {
            public int Result;
            public int RawResult;
        }
    }
}
