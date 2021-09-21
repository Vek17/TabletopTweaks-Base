using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents
{
    [ComponentName("Buffs/AddEffect/EnergyResistance")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("539a6b8bf234438c918ad81fc89dcf5c")]
    public class TTAddDamageResistanceForce : TTAddDamageResistanceBase
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
            return !(damage is ForceDamage);
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            if (vanillaResistance is Kingmaker.UnitLogic.FactLogic.AddDamageResistanceForce vanillaForceResist)
            {
                this.Type = vanillaForceResist.Type;
                this.UseValueMultiplier = vanillaForceResist.UseValueMultiplier;
                this.ValueMultiplier = vanillaForceResist.ValueMultiplier;
            }
        }
    }
}
