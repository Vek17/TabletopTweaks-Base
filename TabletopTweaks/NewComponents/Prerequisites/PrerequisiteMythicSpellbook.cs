using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.NewComponents.Prerequisites {
    class PrerequisiteMythicSpellbook : Prerequisite {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Spellbooks
                .Where(book => book.IsMythic)
                .Any(book => book.MaxSpellLevel >= RequiredSpellLevel);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            if (RequiredSpellLevel > 0) {
                return $"Can cast spells of level {RequiredSpellLevel} or higher from spellbook";
            }
            return $"Has spellbook";
        }

        public int RequiredSpellLevel;
    }
}
