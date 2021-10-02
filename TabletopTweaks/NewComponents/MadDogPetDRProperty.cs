using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance;

namespace TabletopTweaks.NewComponents {
    [TypeId("bb71b6a071064d13b28aa71d626085d2")]
    public class MadDogPetDRProperty : PropertyValueGetter {
        private static BlueprintFeature MadDogMasterDamageReduction = Resources.GetBlueprint<BlueprintFeature>("a0d4a3295224b8f4387464a4447c31d5");

        public override int GetBaseValue(UnitEntityData unit) {
            if (!unit.IsPet || unit.Master == null)
                return 0;

            int value = 0;
            EntityFact masterDamageReduction = unit.Master.GetFact(MadDogMasterDamageReduction);
            if (masterDamageReduction != null) {
                foreach (BlueprintComponentAndRuntime<TTAddDamageResistancePhysical> componentAndRuntime in masterDamageReduction.SelectComponentsWithRuntime<TTAddDamageResistancePhysical>()) {
                    value += ((TTAddDamageResistancePhysical.ComponentRuntime)componentAndRuntime.Runtime).GetCurrentValue();
                }
            }

            return value;
        }
    }
}
