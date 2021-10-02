using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("cb76145587814eabbbbaed3d2a9b5d99")]
    public class PrerequisiteNoClassLevelVisible : Prerequisite {
        public BlueprintCharacterClass CharacterClass {
            get {
                BlueprintCharacterClassReference characterClass = m_CharacterClass;
                if (characterClass == null) {
                    return null;
                }
                return characterClass.Get();
            }
        }
        public override string GetUITextInternal(UnitDescriptor unit) {
            return $"Has no levels in the class {CharacterClass.Name}";
        }

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Progression.GetClassLevel(CharacterClass) < 1;
        }

        [NotNull]
        [SerializeField]
        [FormerlySerializedAs("CharacterClass")]
        public BlueprintCharacterClassReference m_CharacterClass;
    }
}
