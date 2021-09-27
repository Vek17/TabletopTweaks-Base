using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Utility;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeatureSelection))]
    [TypeId("2476cd4ea3674fc58402daf57bf72562")]
    class SelectionDefaultFeature : UnitFactComponentDelegate,
        IGlobalSubscriber, ISubscriber,
        IUnitLevelUpHandler, IAreaLoadingStagesHandler, ICompanionChangeHandler, IUnitSpawnHandler {

        public void OnAreaScenesLoaded() {
        }

        public void OnAreaLoadingComplete() {
            ApplyDefaultIfMissing();
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
            ApplyDefaultIfMissing();
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
        }

        public void HandleRecruit(UnitEntityData companion) {
            ApplyDefaultIfMissing();
        }

        public void HandleUnrecruit(UnitEntityData companion) {
        }

        public void HandleUnitSpawned(UnitEntityData unit) {
            ApplyDefaultIfMissing();
        }

        private void ApplyDefaultIfMissing() {
            var Selection = Fact.Blueprint as BlueprintFeatureSelection;
            if (Selection == null) { Main.Log($"{Fact.Blueprint.AssetGuid} - {Fact.Blueprint.name}: SelectionDefaultFeature Applied on Null Selection"); return; }
            if (!Owner.Progression.GetSelectionData(Selection).IsEmpty) { return; }
            Main.LogDebug($"Apply Default: {Owner.CharacterName} - {Owner.Progression.CharacterLevel} - {Owner.Blueprint.AssetGuid} - {Owner.UniqueId}");

            var Progressions = Owner.Progression.m_Progressions;
            foreach (var Progression in Progressions) {
                for (int level = 1; level <= Progression.Value.Level; level++) {
                    foreach (var entry in Progression.Key.LevelEntries.Where(e => e.Level == level)) {
                        Main.LogDebug($"Checking Level {level} - {Progression.Key.name}");
                        if (entry.Features.Contains(Selection)) {
                            var Selections = Owner.Progression.GetSelections(Selection, level);
                            if (Selections.Empty()) {
                                Owner.Progression.AddSelection(Selection, Progression.Key, level, DefaultFeature.Get());
                                Owner.AddFact(DefaultFeature.Get(), null, null);
                                Main.LogDebug($"Added Default to: {Owner.CharacterName} - {level}");
                            }
                        }
                    }
                }
            }
        }

        [SerializeField]
        [FormerlySerializedAs("NewFact")]
        public BlueprintFeatureReference DefaultFeature;
        public ArmorProficiencyGroup[] RequiredArmor = new ArmorProficiencyGroup[0];
        public bool Invert = false;
    }
}
