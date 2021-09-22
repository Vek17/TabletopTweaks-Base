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

        // Is this absolute immunity, without using a pool
        public virtual bool IsImmunity => Immunity && !UsePool;

        // Is this immunity using a pool, such as Protection From Energy
        public bool IsImmunityPool => Immunity && UsePool;

        /*
         * The StacksWithX properties below refer to how a "parent" or "base" DR stacks with other DR.
         * For example, Stalwart Defender's DR is a class feature DR that stacks with armor. 
         * This means that any DR that has SourceIsArmor == true will be added to the DR stack that has 
         * the Stalwart Defender feature as the base DR
         * 
         * This relationship is not transitive, however. If a DR fact has StacksWithClassFeatures == true,
         * for example, then the Stalwart Defender class feature DR would be added to that fact's stack, but
         * any armor DR would not. You would then have two stacks that have the Stalwart Defender's DR in them,
         * one stack with Stalwart Defender's DR as base, and any armor DR as "children", and one stack with
         * our StacksWithClassFeatures DR as base and Stalwart Defender's DR as a child, but *not* any armor DR.
         * One of these would then be the highest total DR, which would end up being the one that is applied.
         */

        // Does this DR stack with DRs that have SourceIsArmor == true (e.g. the Stalwart Defender's Damage Reduction feature)
        public bool StacksWithArmor = false;

        // Does this DR stack with DRs that have SourceIsClassFeature == true (e.g. the Stalwart and Improved Stalwart feats)
        public bool StacksWithClassFeatures = false;

        // Does this DR stack with DRs provided by specific facts  (e.g. Armored Juggernaut, which specifically only stacks with DR provided by adamantine armor)
        public BlueprintUnitFactReference[] StacksWithFacts = null;

        // Is the source of this DR armor based
        public bool SourceIsArmor = false;

        // Is the source of this DR a class feature
        public bool SourceIsClassFeature = false;

        // This isn't really useful anymore, but I've overridden it with something that kinda sorta makes sense, as I haven't gotten around to properly updating
        // the character info screens that still make use of this
        public virtual bool IsStackable => StacksWithArmor || StacksWithClassFeatures || (StacksWithFacts != null && StacksWithFacts.Length > 0);

        // The priority of this resistance. This is used for cases where damage might "spill over" from one (usually pool-based) resistance to another resistance
        // The *vast* majority of resistances should have a priority of Normal. Currently, only Protection From Energy has a High priority, and only the Abjuration
        // school's Energy Absorption feature has a Low priority.
        public DRPriority Priority = DRPriority.Normal;

        // This should check if a given other resistance is of the same type as this resistance. For energy resistance, this probably means protecting against the same
        // energy type. For DR, it should mean having the same thing "after the slash". Regardless of other settings, resistances can only stack with each other if
        // this method returns true.
        public abstract bool IsSameDRTypeAs(TTAddDamageResistanceBase other);

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

        // This creates a TTAddDamageResistanceBase from a vanilla AddDamageResistanceBase component.
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
        // Additional initialization specific to the implementation of this base class, that should be done when such an implementation is created from its
        // vanilla counterpart.
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

        // The possible priorities that a resistance can have. See TTAddDamageResistanceBase.Priority
        public enum DRPriority
        {
            Low = 0,
            Normal = 10,
            High = 20,
        }
    }
}
