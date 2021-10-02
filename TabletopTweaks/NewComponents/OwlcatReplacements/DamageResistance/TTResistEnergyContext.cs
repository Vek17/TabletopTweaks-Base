using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [AllowMultipleComponents]
    [TypeId("4b2963ec396849b580f268d139ad3b22")]
    public class TTResistEnergyContext : TTAddDamageResistanceEnergy {
    }
}
