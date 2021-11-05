using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.NewComponents.Properties {
    [TypeId("762c31d6c5284ff5964a4af007ec5325")]
    class CompositeCustomPropertyGetter : PropertyValueGetter {
        public override int GetBaseValue(UnitEntityData unit) {
            switch (CalculationMode) {
                case Mode.Sum:
                    return Properties.Select(property => property.Calculate(unit)).Sum();
                case Mode.Highest:
                    return Properties.Select(property => property.Calculate(unit)).Max();
                case Mode.Lowest:
                    return Properties.Select(property => property.Calculate(unit)).Min();
                default:
                    return 0;
            }
        }

        public ComplexCustomProperty[] Properties = new ComplexCustomProperty[0];
        public Mode CalculationMode;

        public enum Mode : int {
            Sum,
            Highest,
            Lowest
        }

        public class ComplexCustomProperty {
            public ComplexCustomProperty() { }

            public int Calculate(UnitEntityData unit) {
                return Bonus + Mathf.FloorToInt((Numerator / Denominator) * Property.GetBaseValue(unit));
            }

            public PropertyValueGetter Property;
            public int Bonus;
            public float Numerator = 1;
            public float Denominator = 1;
        }
    }
}
