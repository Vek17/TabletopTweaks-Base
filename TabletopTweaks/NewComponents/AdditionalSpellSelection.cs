using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;

namespace TabletopTweaks.NewComponents {
    [TypeId("070fd2a4a2cb4f198a44ae036082818c")]
    class AdditionalSpellSelection : UnitFactComponentDelegate {

        private Spellbook spellbook { get => Owner.DemandSpellbook(SpellCastingClass); }
        private int adjustedMaxLevel {
            get {
                if (!UseOffset) { return MaxSpellLevel; }
                return Math.Max((spellbook?.MaxSpellLevel ?? 0) - SpellLevelOffset, 1);
            }
        }
        public override void OnFactAttached() {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            if (spellbook == null) { return; }
            spellSelection = controller.State.DemandSpellSelection(spellbook.Blueprint, SpellList);
            spellSelection.SetExtraSpells(Count, adjustedMaxLevel);
        }
        public override void OnDeactivate() {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            if (spellbook == null) { return; }
            controller.State.SpellSelections.Remove(spellSelection);
        }

        private SpellSelectionData spellSelection;

        public BlueprintSpellListReference SpellList;
        public BlueprintCharacterClassReference SpellCastingClass;
        public int MaxSpellLevel;
        public bool UseOffset;
        public int SpellLevelOffset;
        public int Count;
    }
}
