using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Owlcat.QA.Validation;

namespace TabletopTweaks.NewComponents.Properties {
    [TypeId("5f193022788a43d28c0bdaa913a21117")]
    public class StatValueGetter : PropertyValueGetter {
        public override int GetBaseValue(UnitEntityData unit) {
            return unit.Stats.GetStat(this.Stat).ModifiedValue;
        }

        public override void ApplyValidation(ValidationContext context, int parentIndex) {
            base.ApplyValidation(context, parentIndex);
        }

        public StatType Stat;
    }
}
