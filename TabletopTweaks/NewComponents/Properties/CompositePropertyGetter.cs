using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.NewComponents.Properties {
    [TypeId("8050a764a88e4f199015f70ddb0c8eee")]
    class CompositePropertyGetter : PropertyValueGetter {
        public override int GetBaseValue(UnitEntityData unit) {
            return Properties
                .Select(property => property.Calculate(unit))
                .Sum();
        }

        public ComplexProperty[] Properties = new ComplexProperty[0];

        public class ComplexProperty {
            public ComplexProperty() { }

            public int Calculate(UnitEntityData unit) {
                return Bonus + Mathf.FloorToInt((Numerator / Denominator) * Property.GetInt(unit));
            }

            public UnitProperty Property;
            public int Bonus;
            public float Numerator = 1;
            public float Denominator = 1;
        }
    }
}
