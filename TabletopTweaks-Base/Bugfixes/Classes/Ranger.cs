using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Ranger {
        [PatchBlueprintsCacheInit]
        static class Ranger_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Ranger")) { return; }

                var MasterHunter = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("9d53ef63441b5d84297587d75f72fc17");
                var RangerAlternateCapstone = NewContent.AlternateCapstones.Ranger.RangerAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MasterHunter.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.RangerClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MasterHunter.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(RangerAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(RangerAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MasterHunter.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(RangerAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Ranger");

                PatchBase();
                PatchEspionageExpert();
            }
            static void PatchBase() {
                PatchFavoredEnemy();


                void PatchFavoredEnemy() {
                    if (TTTContext.Fixes.Ranger.Base.IsDisabled("FavoredEnemy")) { return; }
                    var FavoriteEnemySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("16cc2c937ea8d714193017780e7d4fc6");
                    var FavoriteEnemyOutsider = BlueprintTools.GetBlueprint<BlueprintFeature>("f643b38acc23e8e42a3ed577daeb6949");
                    var FavoriteEnemyDemonOfMagic = BlueprintTools.GetBlueprint<BlueprintFeature>("21328361091fd2c44a3909fcae0dd598");
                    var FavoriteEnemyDemonOfSlaughter = BlueprintTools.GetBlueprint<BlueprintFeature>("6c450765555b1554294b5556f50d304e");
                    var FavoriteEnemyDemonOfStrength = BlueprintTools.GetBlueprint<BlueprintFeature>("48e9e7ecca39c4a438d9262a20ab5066");
                    var OutsiderType = BlueprintTools.GetBlueprint<BlueprintFeature>("9054d3988d491d944ac144e27b6bc318");
                    var AasimarRace = BlueprintTools.GetBlueprint<BlueprintRace>("b7f02ba92b363064fb873963bec275ee");
                    var InstantEnemyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("82574f7d14a28e64fab8867fbaa17715");

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
                        TTTContext.Logger.LogPatch("Patched", FavoriteEnemy);
                    }
                    void AddPrerequisite(BlueprintFeature FavoriteEnemy) {
                        FavoriteEnemy.AddPrerequisites(Helpers.Create<PrerequisiteFeature>(c => {
                            c.Group = Prerequisite.GroupType.All;
                            c.m_Feature = FavoriteEnemy.ToReference<BlueprintFeatureReference>();
                        }));
                        FavoriteEnemy.SetDescription(TTTContext, "This works exactly like Favorite Enemy (Outsider) and exists for existing " +
                        "build compatability. If you already have this feature continue taking ranks to progress Favored Enemy Outsider properly.");
                    }
                }
            }
            static void PatchEspionageExpert() {
                PatchTrapfinding();

                void PatchTrapfinding() {
                    if (TTTContext.Fixes.Ranger.Archetypes["EspionageExpert"].IsDisabled("Trapfinding")) { return; }
                    var MasterSpyTrapfindingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d55acf213bf709c40b2bc72b997fb345");
                    MasterSpyTrapfindingFeature.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    MasterSpyTrapfindingFeature.SetDescription(TTTContext, "An espionage expert gets a bonus equal to 1/2 her "
                        + "level on Perception checks and Trickery checks.");
                    TTTContext.Logger.LogPatch("Patched", MasterSpyTrapfindingFeature);
                }
            }
        }
    }
}
