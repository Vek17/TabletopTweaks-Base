using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;

namespace TabletopTweaks.NewComponents.Properties {
    [TypeId("b310257badf44a9d97d8f8fe8b3df3f6")]
    class ProgressionRankGetter : PropertyValueGetter {

        public override int GetBaseValue(UnitEntityData unit) {
            var unitProgression = unit.Progression.GetProgression(Progression);
            int value = unitProgression?.Level ?? 0;
            return UseMax ? Math.Min(value, Max) : value;
        }

        public BlueprintProgressionReference Progression;
        public int Max;
        public bool UseMax;
    }
}
