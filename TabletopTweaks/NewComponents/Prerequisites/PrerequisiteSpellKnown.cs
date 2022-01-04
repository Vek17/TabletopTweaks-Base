using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("e523c74b8da74fec91ae651138ec0ca0")]
    class PrerequisiteSpellKnown : Prerequisite {
        private BlueprintAbility Spell {
            get {
                if (m_Spell == null) {
                    return null;
                }
                return m_Spell.Get();
            }
        }
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            return unit.Spellbooks
                .Any(book => book.IsKnown(Spell));
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            return $"Can cast spell: {Spell.Name}";
        }
        public BlueprintAbilityReference m_Spell;
    }
}
