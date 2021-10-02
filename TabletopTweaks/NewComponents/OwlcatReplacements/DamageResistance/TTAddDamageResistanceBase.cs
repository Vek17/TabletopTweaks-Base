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
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("3543db673ac347bda9afc1f58793e6a0")]
    public abstract class TTAddDamageResistanceBase :
        BlueprintComponent,
        IRuntimeEntityFactComponentProvider {
        public ContextValue Value = 5;
        public bool UsePool;
        [ShowIf("UsePool")]
        public ContextValue Pool = 12;

        public bool Immunity = false;

        // Is this absolute immunity, without using a pool
        public virtual bool IsImmunity => Immunity && !UsePool;

        // Is this immunity using a pool, such as Protection From Energy
        public bool IsImmunityPool => Immunity && UsePool;

        public bool IsIncreasedByArmor = false;

        public bool IsIncreasedByClassFeatures = false;

        public BlueprintUnitFactReference[] IncreasedByFacts = null;

        public bool IsIncreasedByFacts => IncreasedByFacts != null && IncreasedByFacts.Length > 0;

        public BlueprintUnitFactReference[] IncreasesFacts = null;

        public bool IsIncreasesFacts => IncreasesFacts != null && IncreasesFacts.Length > 0;

        public bool IsStacksWithArmor = false;

        public bool IsStacksWithClassFeatures = false;

        public BlueprintUnitFactReference[] StacksWithFacts = null;

        public bool IsStacksWithFacts => StacksWithFacts != null && StacksWithFacts.Length > 0;

        // So remember how we did a super complex rework of DR mechanics to make sure they don't stack with everything anymore. Well setting this to true will cause
        // this DR to stack with everything. It will get added to the list of stacking resistances for every compatible resistance (same DR type and same Priority)
        public bool AddToAllStacks = false;

        // Is the source of this DR armor based
        public bool SourceIsArmor = false;

        // Is the source of this DR a class feature
        public bool SourceIsClassFeature = false;

        // This isn't really useful anymore, but I've overridden it with something that kinda sorta makes sense, as I haven't gotten around to properly updating
        // the character info screens that still make use of this
        public virtual bool IsStackable => IsStacksWithArmor || IsStacksWithClassFeatures || StacksWithFacts != null && StacksWithFacts.Length > 0;

        // The priority of this resistance. This is used for cases where damage might "spill over" from one (usually pool-based) resistance to another resistance
        // The *vast* majority of resistances should have a priority of Normal. Currently, only Protection From Energy has a High priority, and only the Abjuration
        // school's Energy Absorption feature has a Low priority.
        public DRPriority Priority = DRPriority.Normal;

        // This should check if a given other resistance is of the same type as this resistance. For energy resistance, this probably means protecting against the same
        // energy type. For DR, it should mean having the same thing "after the slash". Regardless of other settings, resistances can only stack with each other if
        // this method returns true.
        public abstract bool IsSameDRTypeAs(TTAddDamageResistanceBase other);

        protected virtual bool ShouldBeRemoved(ComponentRuntime runtime) => UsePool && CalculateRemainingPool(runtime) < 1;

        protected virtual int CalculateValue(ComponentRuntime runtime) => Value.Calculate(runtime.Fact.MaybeContext);

        protected virtual int CalculateRemainingPool(ComponentRuntime runtime) => ((IDamageResistanceRuntimeInternal)runtime).RemainPool;

        protected virtual void OnSpendPool(ComponentRuntime runtime, int damage) {
            if (!UsePool)
                return;
            ((IDamageResistanceRuntimeInternal)runtime).ReducePool(damage);
        }

        protected abstract bool Bypassed(
          ComponentRuntime runtime,
          BaseDamage damage,
          ItemEntityWeapon weapon);

        public virtual EntityFactComponent CreateRuntimeFactComponent() => new ComponentRuntime();

        // This creates a TTAddDamageResistanceBase from a vanilla AddDamageResistanceBase component.
        public void InitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance) {
            // BlueprintComponent
            m_Flags = vanillaResistance.m_Flags;
            name = vanillaResistance.name;
            m_PrototypeLink = vanillaResistance.m_PrototypeLink;
            OwnerBlueprint = vanillaResistance.OwnerBlueprint;
            Disabled = vanillaResistance.Disabled;

            // AddDamageResistanceBase
            Value = vanillaResistance.Value;
            UsePool = vanillaResistance.UsePool;
            Pool = vanillaResistance.Pool;

            AdditionalInitFromVanillaDamageResistance(vanillaResistance);
        }
        // Additional initialization specific to the implementation of this base class, that should be done when such an implementation is created from its
        // vanilla counterpart.
        protected abstract void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance);

        private interface IDamageResistanceRuntimeInternal {
            int RemainPool { get; }

            void ReducePool(int value);
        }

        public class ComponentRuntime :
          UnitFactComponent<TTAddDamageResistanceBase>,
          IDamageResistanceRuntimeInternal {
            [JsonProperty]
            private int m_RemainPool;
            public int RemainPool => m_RemainPool;

            public bool ShouldBeRemoved => Settings.ShouldBeRemoved(this);

            public override void OnActivate() => m_RemainPool = Settings.Pool.Calculate(Fact.MaybeContext);

            public override void OnTurnOn() {
                Main.LogDebug("DR Fact turned on: " + Fact.Blueprint.name + ":" + Fact.Blueprint.AssetGuid.ToString());
                Owner.Ensure<TTUnitPartDamageReduction>().Add(Fact);
            }

            public override void OnTurnOff() => Owner.Get<TTUnitPartDamageReduction>()?.Remove(Fact);

            public void SpendPool(int value) => Settings.OnSpendPool(this, value);

            public int GetValue() => Settings.CalculateValue(this);

            public int GetCurrentValue() {
                int val2 = GetValue();
                return !Settings.UsePool ? val2 : Math.Min(Settings.CalculateRemainingPool(this), val2);
            }

            public bool Bypassed(BaseDamage damage, ItemEntityWeapon weapon) => Settings.Bypassed(this, damage, weapon);

            int IDamageResistanceRuntimeInternal.RemainPool => m_RemainPool;

            void IDamageResistanceRuntimeInternal.ReducePool(
              int value) {
                m_RemainPool = Math.Max(0, m_RemainPool - value);
            }
        }

        // The possible priorities that a resistance can have. See TTAddDamageResistanceBase.Priority
        public enum DRPriority {
            Low = 0,
            Normal = 10,
            High = 20,
        }
    }
}
