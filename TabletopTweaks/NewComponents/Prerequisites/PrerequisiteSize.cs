using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("e5db1c3ea30a44559f1f7542ed3b9d0d")]
    public class PrerequisiteSize : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString IsSize = Helpers.CreateString("PrerequisiteSize.UI", "Is Size");
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.OriginalSize == Size;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(IsSize);
            stringBuilder.Append(": ");
            stringBuilder.Append(Size);

            return stringBuilder.ToString();
        }

        public Kingmaker.Enums.Size Size = Kingmaker.Enums.Size.Medium;
    }
}
