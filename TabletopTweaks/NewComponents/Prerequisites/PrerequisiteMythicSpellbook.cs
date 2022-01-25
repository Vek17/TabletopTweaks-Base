using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("08d2e61c79c64ee1afdca9fc834ffc32")]
    public class PrerequisiteMythicSpellbook : Prerequisite {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Spellbooks
                .Where(book => book.IsMythic)
                .Any(book => book.MaxSpellLevel >= RequiredSpellLevel);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            if (RequiredSpellLevel > 0) {
                return $"Can cast spells of level {RequiredSpellLevel} or higher from mythic spellbook";
            }
            return $"Has mythic spellbook";
        }

        public int RequiredSpellLevel;
    }
}
