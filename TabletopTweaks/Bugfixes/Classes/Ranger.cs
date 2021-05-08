using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Ranger {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Ranger.DisableAllFixes) { return; }
                Main.LogHeader("Patching Ranger");
                PatchFavoredEnemy();
            }
            static void PatchFavoredEnemy() {
                if (!ModSettings.Fixes.Ranger.Base.Fixes["FavoredEnemy"]) { return; }
                var FavoriteEnemySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("16cc2c937ea8d714193017780e7d4fc6");
                var FavoriteEnemyOutsider = Resources.GetBlueprint<BlueprintFeature>("f643b38acc23e8e42a3ed577daeb6949");
                var FavoriteEnemyDemonOfMagic = Resources.GetBlueprint<BlueprintFeature>("21328361091fd2c44a3909fcae0dd598");
                var FavoriteEnemyDemonOfSlaughter = Resources.GetBlueprint<BlueprintFeature>("6c450765555b1554294b5556f50d304e");
                var FavoriteEnemyDemonOfStrength = Resources.GetBlueprint<BlueprintFeature>("48e9e7ecca39c4a438d9262a20ab5066");
                var OutsiderType = Resources.GetBlueprint<BlueprintFeature>("9054d3988d491d944ac144e27b6bc318");
                var AasimarRace = Resources.GetBlueprint<BlueprintRace>("b7f02ba92b363064fb873963bec275ee");
                var InstantEnemyBuff = Resources.GetBlueprint<BlueprintBuff>("82574f7d14a28e64fab8867fbaa17715");

                PatchCheckedFacts(FavoriteEnemyOutsider);
                PatchCheckedFacts(FavoriteEnemyDemonOfMagic);
                PatchCheckedFacts(FavoriteEnemyDemonOfSlaughter);
                PatchCheckedFacts(FavoriteEnemyDemonOfStrength);
                AddPrerequisite(FavoriteEnemyDemonOfMagic);
                AddPrerequisite(FavoriteEnemyDemonOfSlaughter);
                AddPrerequisite(FavoriteEnemyDemonOfStrength);
                void PatchCheckedFacts(BlueprintFeature FavoriteEnemy) {
                    var favoredEnemy = FavoriteEnemy.GetComponent<FavoredEnemy>();
                    favoredEnemy.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OutsiderType.ToReference<BlueprintUnitFactReference>(),
                        AasimarRace.ToReference<BlueprintUnitFactReference>(),
                        InstantEnemyBuff.ToReference<BlueprintUnitFactReference>()
                    };
                    Main.LogPatch("Patched", FavoriteEnemy);
                }
                void AddPrerequisite(BlueprintFeature FavoriteEnemy) {
                    FavoriteEnemy.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                        c.Group = Prerequisite.GroupType.All;
                        c.m_Feature = FavoriteEnemy.ToReference<BlueprintFeatureReference>();
                    }));
                }
            }
            static void PatchArchetypes() {
            }
        }
    }
}
