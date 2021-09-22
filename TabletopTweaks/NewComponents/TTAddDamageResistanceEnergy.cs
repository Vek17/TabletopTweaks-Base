using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewComponents
{
    [ComponentName("Buffs/AddEffect/EnergyResistance")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("e0d77b86ac824bf487e508ad06831911")]
    public class TTAddDamageResistanceEnergy : TTAddDamageResistanceBase
    {
        public DamageEnergyType Type;
        public bool UseValueMultiplier;
        [ShowIf("UseValueMultiplier")]
        public ContextValue ValueMultiplier;

        protected override int CalculateValue(TTAddDamageResistanceBase.ComponentRuntime runtime) => runtime.Fact.GetRank() * (this.UseValueMultiplier ? base.CalculateValue(runtime) * this.ValueMultiplier.Calculate(runtime.Fact.MaybeContext) : base.CalculateValue(runtime));

        protected override bool Bypassed(
          TTAddDamageResistanceBase.ComponentRuntime runtime,
          BaseDamage damage,
          ItemEntityWeapon weapon)
        {
            return !(damage is EnergyDamage energyDamage) || energyDamage.EnergyType != this.Type || (Immunity && runtime.Owner.State.HasCondition(UnitCondition.SuppressedEnergyImmunity)) || (!Immunity && runtime.Owner.State.HasCondition(UnitCondition.SuppressedEnergyResistance));
        }

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other)
        {
            return other is TTAddDamageResistanceEnergy otherEnergyResist && this.Type == otherEnergyResist.Type;
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            if (vanillaResistance is Kingmaker.UnitLogic.FactLogic.AddDamageResistanceEnergy vanillaEnergyResistance)
            {
                this.Type = vanillaEnergyResistance.Type;
                this.UseValueMultiplier = vanillaEnergyResistance.UseValueMultiplier;
                this.ValueMultiplier = vanillaEnergyResistance.ValueMultiplier;
            }
        }
    }
}
