using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [TypeId("282fa36ad9784f639bbdec2e281e7bed")]
    public class PrerequisiteAttributeRacialBonus: Prerequisite {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Stats.GetAttribute(Attribute)?.Modifiers.Any(m => m.ModDescriptor == Kingmaker.Enums.ModifierDescriptor.Racial) ?? false;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            return $"Has {Attribute} racial bonus";
        }

        public StatType Attribute = StatType.Dexterity;
    }
}
