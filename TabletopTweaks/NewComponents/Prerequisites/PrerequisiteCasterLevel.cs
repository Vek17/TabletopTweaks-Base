using System.Text;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("59c3025feecf4113bef90196f2ce4ef9")]
    public class PrerequisiteCasterLevel : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString HasCasterLevelOf = Helpers.CreateString("PrerequisiteCasterLevel.UI", "Has a caster level of:");

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return GetCasterLevel(unit) >= RequiredCasterLevel;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(HasCasterLevelOf);
            stringBuilder.Append(" ");
            stringBuilder.Append(RequiredCasterLevel);
            stringBuilder.Append("\n");
            stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.CurrentValue, GetCasterLevel(unit)));

            return stringBuilder.ToString();
        }

        private int GetCasterLevel(UnitDescriptor unit) {
            var result = 0;
            foreach (ClassData classData in unit.Progression.Classes) {
                var blueprint = classData.Spellbook;
                if (blueprint == null) { continue; }
                var spellbook = unit.DemandSpellbook(classData.CharacterClass);
                if (spellbook != null && !blueprint.IsAlchemist && !blueprint.IsAlchemist && !blueprint.IsMythic) {
                    result = result < spellbook.CasterLevel ? spellbook.CasterLevel : result;
                }
            }
            return result;
        }

        public int RequiredCasterLevel;
    }
}
