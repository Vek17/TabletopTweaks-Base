using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [ComponentName("Buffs/AddEffect/EnergyResistance")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("539a6b8bf234438c918ad81fc89dcf5c")]
    public class TTAddDamageResistanceForce : TTAddDamageResistanceBase {
        public DamageEnergyType Type;
        public bool UseValueMultiplier;
        [ShowIf("UseValueMultiplier")]
        public ContextValue ValueMultiplier;

        protected override int CalculateValue(ComponentRuntime runtime) => runtime.Fact.GetRank() * (UseValueMultiplier ? base.CalculateValue(runtime) * ValueMultiplier.Calculate(runtime.Fact.MaybeContext) : base.CalculateValue(runtime));

        protected override bool Bypassed(
          ComponentRuntime runtime,
          BaseDamage damage,
          ItemEntityWeapon weapon) {
            return !(damage is ForceDamage);
        }

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other) {
            return other is TTAddDamageResistanceForce otherForceResist && Type == otherForceResist.Type;
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance) {
            if (vanillaResistance is Kingmaker.UnitLogic.FactLogic.AddDamageResistanceForce vanillaForceResist) {
                Type = vanillaForceResist.Type;
                UseValueMultiplier = vanillaForceResist.UseValueMultiplier;
                ValueMultiplier = vanillaForceResist.ValueMultiplier;
            }
        }
    }
}
