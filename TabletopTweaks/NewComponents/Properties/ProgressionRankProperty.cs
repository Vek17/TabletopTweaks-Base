using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace TabletopTweaks.NewComponents.Properties {
    [TypeId("b310257badf44a9d97d8f8fe8b3df3f6")]
    class ProgressionRankProperty : PropertyValueGetter {

        public override int GetBaseValue(UnitEntityData unit) {
            var unitProgression = unit.Progression.GetProgression(Progression);
            return unitProgression?.Level ?? 0;
        }

        public BlueprintProgressionReference Progression;
    }
}
