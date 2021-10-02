using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("282fa36ad9784f639bbdec2e281e7bed")]
    public class PrerequisiteStatBonus : Prerequisite {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Stats.GetStat(Stat)?.Modifiers.Any(m => m.ModDescriptor == Descriptor && m.ModValue > 0) ?? false;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            return $"Has {Stat} {Descriptor} bonus";
        }

        public StatType Stat = StatType.Dexterity;
        public ModifierDescriptor Descriptor = ModifierDescriptor.Racial;
    }
}
