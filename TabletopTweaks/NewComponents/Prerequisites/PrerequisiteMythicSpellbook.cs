using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using System.Text;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("08d2e61c79c64ee1afdca9fc834ffc32")]
    public class PrerequisiteMythicSpellbook : Prerequisite {

        [InitializeStaticString]
        private static readonly LocalizedString CastCastSpellsOfLevel = Helpers.CreateString("PrerequisiteMythicSpellbook.UI", "Can cast spells of level");
        [InitializeStaticString]
        private static readonly LocalizedString FromMythicSpellbook = Helpers.CreateString("PrerequisiteMythicSpellbook.UI", "or higher from mythic spellbook");
        [InitializeStaticString]
        private static readonly LocalizedString HasMythicSpellbook = Helpers.CreateString("PrerequisiteMythicSpellbook.UI", "Has mythic spellbook");
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Spellbooks
                .Where(book => book.IsMythic)
                .Any(book => book.MaxSpellLevel >= RequiredSpellLevel);
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            if (RequiredSpellLevel > 0) {
                stringBuilder.Append(CastCastSpellsOfLevel);
                stringBuilder.Append(" ");
                stringBuilder.Append(RequiredSpellLevel);
                stringBuilder.Append(" ");
                stringBuilder.Append(FromMythicSpellbook);
            } else {
                stringBuilder.Append(HasMythicSpellbook);
            }
            return stringBuilder.ToString();
        }

        public int RequiredSpellLevel;
    }
}
