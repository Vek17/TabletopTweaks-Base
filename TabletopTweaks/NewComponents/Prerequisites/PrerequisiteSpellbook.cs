using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("7686e2d0ab864daaaf01150c62741aba")]
    public class PrerequisiteSpellbook : Prerequisite {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Spellbooks
                .Where(book => book.Blueprint.AssetGuid.Equals(Spellbook.Guid))
                .Any(book => book.MaxSpellLevel >= RequiredSpellLevel);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            if (RequiredSpellLevel > 0) {
                return $"Can cast Mythic spells of level {RequiredSpellLevel} or higher";
            }
            return $"Has a Mythic spellbook";
        }
        public BlueprintSpellbookReference Spellbook;
        public int RequiredSpellLevel;
    }
}
