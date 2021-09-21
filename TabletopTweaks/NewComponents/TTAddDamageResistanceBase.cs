using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("3543db673ac347bda9afc1f58793e6a0")]
    public abstract class TTAddDamageResistanceBase : 
        BlueprintComponent,
        IRuntimeEntityFactComponentProvider
    {
        public ContextValue Value = (ContextValue)5;
        public bool UsePool;
        [ShowIf("UsePool")]
        public ContextValue Pool = (ContextValue)12;

        public bool Immunity = false;

        public virtual bool IsImmunity => Immunity && !UsePool;

        public bool IsImmunityPool => Immunity && UsePool;

        public bool StacksWithArmor = false;

        public bool StacksWithClassFeatures = false;

        public BlueprintUnitFactReference[] StacksWithFacts = null;

        public bool SourceIsArmor = false;

        public bool SourceIsClassFeature = false;

        public virtual bool IsStackable => StacksWithArmor || StacksWithClassFeatures || (StacksWithFacts != null && StacksWithFacts.Length > 0);

        public DRPriority Priority = DRPriority.Normal;

        public bool IsSameDRTypeAs(TTAddDamageResistanceBase other)
        {
            return this.GetType() == other.GetType() && this.DRTypeFlags == other.DRTypeFlags;
        }

        protected virtual int DRTypeFlags => 0;

        protected virtual bool ShouldBeRemoved(TTAddDamageResistanceBase.ComponentRuntime runtime) => this.UsePool && this.CalculateRemainingPool(runtime) < 1;

        protected virtual int CalculateValue(TTAddDamageResistanceBase.ComponentRuntime runtime) => this.Value.Calculate(runtime.Fact.MaybeContext);

        protected virtual int CalculateRemainingPool(TTAddDamageResistanceBase.ComponentRuntime runtime) => ((TTAddDamageResistanceBase.IDamageResistanceRuntimeInternal)runtime).RemainPool;

        protected virtual void OnSpendPool(TTAddDamageResistanceBase.ComponentRuntime runtime, int damage)
        {
            if (!this.UsePool)
                return;
            ((TTAddDamageResistanceBase.IDamageResistanceRuntimeInternal)runtime).ReducePool(damage);
        }

        protected abstract bool Bypassed(
          TTAddDamageResistanceBase.ComponentRuntime runtime,
          BaseDamage damage,
          ItemEntityWeapon weapon);

        public virtual EntityFactComponent CreateRuntimeFactComponent() => (EntityFactComponent)new TTAddDamageResistanceBase.ComponentRuntime();

        public void InitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            // BlueprintComponent
            this.m_Flags = vanillaResistance.m_Flags;
            this.name = vanillaResistance.name;
            this.m_PrototypeLink = vanillaResistance.m_PrototypeLink;
            this.OwnerBlueprint = vanillaResistance.OwnerBlueprint;
            this.Disabled = vanillaResistance.Disabled;

            // AddDamageResistanceBase
            this.Value = vanillaResistance.Value;
            this.UsePool = vanillaResistance.UsePool;
            this.Pool = vanillaResistance.Pool;

            this.AdditionalInitFromVanillaDamageResistance(vanillaResistance);
        }
        protected abstract void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance);

        private interface IDamageResistanceRuntimeInternal
        {
            int RemainPool { get; }

            void ReducePool(int value);
        }

        public class ComponentRuntime :
          UnitFactComponent<TTAddDamageResistanceBase>,
          TTAddDamageResistanceBase.IDamageResistanceRuntimeInternal
        {
            [JsonProperty]
            private int m_RemainPool;
            public int RemainPool => m_RemainPool;

            public bool ShouldBeRemoved => this.Settings.ShouldBeRemoved(this);

            public override void OnActivate() => this.m_RemainPool = this.Settings.Pool.Calculate(this.Fact.MaybeContext);

            public override void OnTurnOn() => this.Owner.Ensure<TTUnitPartDamageReduction>().Add(this.Fact);

            public override void OnTurnOff() => this.Owner.Get<TTUnitPartDamageReduction>()?.Remove(this.Fact);

            public void SpendPool(int value) => this.Settings.OnSpendPool(this, value);

            public int GetValue() => this.Settings.CalculateValue(this);

            public int GetCurrentValue()
            {
                int val2 = this.GetValue();
                return !this.Settings.UsePool ? val2 : Math.Min(this.Settings.CalculateRemainingPool(this), val2);
            }

            public bool Bypassed(BaseDamage damage, ItemEntityWeapon weapon) => this.Settings.Bypassed(this, damage, weapon);

            int TTAddDamageResistanceBase.IDamageResistanceRuntimeInternal.RemainPool => this.m_RemainPool;

            void TTAddDamageResistanceBase.IDamageResistanceRuntimeInternal.ReducePool(
              int value)
            {
                this.m_RemainPool = Math.Max(0, this.m_RemainPool - value);
            }
        }

        public enum DRPriority
        {
            Low = 0,
            Normal = 10,
            High = 20,
        }
    }
}
