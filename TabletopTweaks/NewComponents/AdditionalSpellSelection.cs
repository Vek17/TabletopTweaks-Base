using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Spells;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [TypeId("070fd2a4a2cb4f198a44ae036082818c")]
    class AdditionalSpellSelection : UnitFactComponentDelegate, IUnitCompleteLevelUpHandler {

        private Spellbook SpellBook { get => Owner.DemandSpellbook(m_SpellCastingClass); }
        private BlueprintSpellList SpellList { get => new ProxyBlueprintSpellList(SpellBook?.Blueprint?.SpellList); }
        public int AdjustedMaxLevel {
            get {
                if (!UseOffset) { return MaxSpellLevel; }
                return Math.Max((SpellBook?.MaxSpellLevel ?? 0) - SpellLevelOffset, 1);
            }
        }

        public override void OnActivate() {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            if (SpellBook == null) { return; }
            var selectionCount = controller
                .State?
                .Selections?
                .Select(s => s.SelectedItem?.Feature)
                .Where(f => f == Fact.Blueprint)
                .Count();
            for (int i = 0; i < selectionCount; i++) {
                var selection = controller.State.DemandSpellSelection(SpellBook.Blueprint, SpellList);
                selection.SetExtraSpells(Count, AdjustedMaxLevel);
                spellSelections.Add(selection);
            }
        }
        public override void OnDeactivate() {
            if (spellSelections.Empty()) { return; }
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            if (SpellBook == null) { return; }
            spellSelections.ForEach(selection => controller.State.SpellSelections.Remove(selection));
            spellSelections.Clear();
        }

        public void HandleUnitCompleteLevelup(UnitEntityData unit) {
            spellSelections.Clear();
        }

        private List<SpellSelectionData> spellSelections = new List<SpellSelectionData>();
        public BlueprintSpellListReference m_SpellList;
        public BlueprintCharacterClassReference m_SpellCastingClass;
        public int MaxSpellLevel;
        public bool UseOffset;
        public int SpellLevelOffset;
        public int Count = 1;

        private class ProxyBlueprintSpellList : BlueprintSpellList {
            public ProxyBlueprintSpellList(BlueprintSpellList referenced) : base() {
                AssetGuid = BlueprintGuid.NewGuid();
                referenceList = referenced.ToReference<BlueprintSpellListReference>();
                IsMythic = referenced.IsMythic;
                SpellsByLevel = referenced.SpellsByLevel;
                m_FilteredList = referenced.m_FilteredList;
                FilterByMaxLevel = referenced.FilterByMaxLevel;
                FilterByDescriptor = referenced.FilterByDescriptor;
                Descriptor = referenced.Descriptor;
                FilterBySchool = referenced.FilterBySchool;
                ExcludeFilterSchool = referenced.ExcludeFilterSchool;
                FilterSchool = referenced.FilterSchool;
                FilterSchool2 = referenced.FilterSchool2;
                m_MaxLevel = referenced.m_MaxLevel;
            }
            public BlueprintSpellListReference referenceList;
        }

        [HarmonyPatch(typeof(SpellSelectionData), nameof(SpellSelectionData.CanSelectAnything), new Type[] { typeof(UnitDescriptor) })]
        static class SpellSelectionData_CanSelectAnything_AdditionalSpellSelection_Patch {
            static void Postfix(SpellSelectionData __instance, ref bool __result, UnitDescriptor unit) {
                Spellbook spellbook = unit.Spellbooks.FirstOrDefault((Spellbook s) => s.Blueprint == __instance.Spellbook);
                if (spellbook == null) {
                    __result = false;
                }
                if (!__instance.Spellbook.AllSpellsKnown) { return; }
                if (__instance.ExtraSelected != null && __instance.ExtraSelected.Length != 0) {
                    if (__instance.ExtraSelected.HasItem((BlueprintAbility i) => i == null) && !__instance.ExtraByStat) {
                        for (int level = 0; level <= __instance.ExtraMaxLevel; level++) {
                            if (__instance.SpellList.SpellsByLevel[level].SpellsFiltered.HasItem((BlueprintAbility sb) => !sb.IsCantrip
                            && !__instance.SpellbookContainsSpell(spellbook, level, sb) && !__instance.ExtraSelected.Contains(sb))) {
                                __result = true;
                            }
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(CharGenSpellsPhaseVM), nameof(CharGenSpellsPhaseVM.DefinePhaseMode), new Type[] { typeof(SpellSelectionData), typeof(SpellSelectionData.SpellSelectionState) })]
        static class CharGenSpellsPhaseVM_DefinePhaseMode_AdditionalSpellSelection_Patch {
            static void Postfix(CharGenSpellsPhaseVM __instance, ref CharGenSpellsPhaseVM.SpellSelectorMode __result, SpellSelectionData selectionData) {
                if (!selectionData.Spellbook.AllSpellsKnown) { return; }
                if (selectionData.ExtraSelected.Any<BlueprintAbility>() && !selectionData.ExtraByStat) {
                    __result = CharGenSpellsPhaseVM.SpellSelectorMode.AnyLevels;
                }
            }
        }
        [HarmonyPatch(typeof(CharGenSpellsPhaseVM), nameof(CharGenSpellsPhaseVM.OrderPriority), MethodType.Getter)]
        static class CharGenSpellsPhaseVM_OrderPriority_AdditionalSpellSelection_Patch {
            static void Postfix(CharGenSpellsPhaseVM __instance, ref int __result) {
                if (__instance?.m_SelectionData == null) { return; }
                if (__instance.m_SelectionData.Spellbook?.SpellList?.AssetGuid != __instance.m_SelectionData.SpellList?.AssetGuid) {
                    __result -= 500;
                }
            }
        }
    }
}
