using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
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

		public override bool Check(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
			return unit.Progression.GetClassLevel(CharacterClass) < 1;
		}

		public override string GetUIText() {
			return $"Has no levels in the class {CharacterClass.Name}";
		}

		[NotNull]
		[SerializeField]
		[FormerlySerializedAs("CharacterClass")]
		public BlueprintCharacterClassReference m_CharacterClass;
	}
}
