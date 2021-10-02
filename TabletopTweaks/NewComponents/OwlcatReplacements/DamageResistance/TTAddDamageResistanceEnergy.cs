using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [ComponentName("Buffs/AddEffect/EnergyResistance")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("e0d77b86ac824bf487e508ad06831911")]
    public class TTAddDamageResistanceEnergy : TTAddDamageResistanceBase {
        public DamageEnergyType Type;
        public bool UseValueMultiplier;
        [ShowIf("UseValueMultiplier")]
        public ContextValue ValueMultiplier;

        protected override int CalculateValue(ComponentRuntime runtime) => runtime.Fact.GetRank() * (UseValueMultiplier ? base.CalculateValue(runtime) * ValueMultiplier.Calculate(runtime.Fact.MaybeContext) : base.CalculateValue(runtime));

        protected override bool Bypassed(
          ComponentRuntime runtime,
          BaseDamage damage,
          ItemEntityWeapon weapon) {
            return !(damage is EnergyDamage energyDamage) || energyDamage.EnergyType != Type || Immunity && runtime.Owner.State.HasCondition(UnitCondition.SuppressedEnergyImmunity) || !Immunity && runtime.Owner.State.HasCondition(UnitCondition.SuppressedEnergyResistance);
        }

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other) {
            return other is TTAddDamageResistanceEnergy otherEnergyResist && Type == otherEnergyResist.Type;
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance) {
            if (vanillaResistance is Kingmaker.UnitLogic.FactLogic.AddDamageResistanceEnergy vanillaEnergyResistance) {
                Type = vanillaEnergyResistance.Type;
                UseValueMultiplier = vanillaEnergyResistance.UseValueMultiplier;
                ValueMultiplier = vanillaEnergyResistance.ValueMultiplier;
            }
        }
    }
}
