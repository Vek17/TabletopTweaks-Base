using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Warpriest {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Warpriest_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Warpriest")) { return; }

                var WarpriestAspectOfWar = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("65cc7abc21826a344aa156e2a40dcecc");
                var WarpriestAlternateCapstone = NewContent.AlternateCapstones.Warpriest.WarpriestAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                WarpriestAspectOfWar.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.WarpriestClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == WarpriestAspectOfWar.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(WarpriestAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(WarpriestAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == WarpriestAspectOfWar.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(WarpriestAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Warpriest");
                PatchBase();
                PatchArchetypes();
            }
            static void PatchBase() {
                PatchFighterTraining();
                PatchWarBlessing();

                void PatchFighterTraining() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("FighterTraining")) { return; }

                    var WarpriestClassAsBABFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("2e134d80fef14a44aae9c087215c15af");
                    QuickFixTools.ReplaceClassLevelsForPrerequisites(WarpriestClassAsBABFeature, TTTContext, FeatureGroup.Feat);

                    TTTContext.Logger.LogPatch(WarpriestClassAsBABFeature);
                }
                void PatchWarBlessing() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("WarBlessing")) { return; }

                    var WarBlessingMajorAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("b25af29679004b2085277bb8979b2912");
                    var WarBlessingMinorAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("77b232a88ab04671b44712232e63077d");

                    WarBlessingMinorAbility.AbilityAndVariants().ForEach(ability => ability.ActionType = CommandType.Standard);
                    WarBlessingMajorAbility.ActionType = CommandType.Standard;

                    TTTContext.Logger.LogPatch(WarBlessingMinorAbility);
                    TTTContext.Logger.LogPatch(WarBlessingMajorAbility);
                }
            }

            static void PatchArchetypes() {
                PatchMantisZealot();

                void PatchMantisZealot() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("FighterTraining")) { return; }

                    var MantisZealotDeadlyFascinationFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("823221f892e24568b1e5b111222d5b45");
                    var MantisZealotDeadlyFascinationAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("c2cd38949dad4756900b378767ca90c9");
                    var MantisZealotDeadlyFascinationBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("fabd37ce1b244503a4ad7235327950f9");
                    var DazzledBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("df6d1025da07524429afbae248845ecc");

                    MantisZealotDeadlyFascinationFeature.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Dazzling Bladework");
                        bp.SetDescription(TTTContext, "At 3rd level, a mantis zealot's deadly motions become dazzling. " +
                            "Whenever he kills an enemy with a sawtooth saber, " +
                            "other enemies in a 30-foot radius are dazzled for 2 rounds unless they succeed at a Will save " +
                            "(DC 10 + half the mantis zealot's class level + his Dexterity modifier; " +
                            "if he is wielding two sawtooth sabers, the DC increases by 2; if the red shroud is activated, " +
                            "the DC is increased by an additional 2).");
                    });
                    MantisZealotDeadlyFascinationAbility.TemporaryContext(bp => {
                        bp.SetName(MantisZealotDeadlyFascinationFeature.m_DisplayName);
                        bp.SetDescription(MantisZealotDeadlyFascinationFeature.m_Description);
                        bp.FlattenAllActions().OfType<ContextActionApplyBuff>().ForEach(c => {
                            c.m_Buff = DazzledBuff;
                            c.Permanent = false;
                            c.DurationValue = new ContextDurationValue() { 
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = 2
                            };
                        });
                    });

                    TTTContext.Logger.LogPatch(MantisZealotDeadlyFascinationFeature);
                }
            }
        }
    }
}
