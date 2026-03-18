using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Wizard {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Wizard_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Wizard")) { return; }

                var WizardAlternateCapstone = NewContent.AlternateCapstones.Wizard.WizardAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                ClassTools.Classes.WizardClass.TemporaryContext(bp => {
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(WizardAlternateCapstone));
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
                TTTContext.Logger.LogHeader("Patching Wizard");

                PatchHandOfTheApprentice();
            }

            static void PatchHandOfTheApprentice() {
                if (Main.TTTContext.Fixes.Wizard.Base.IsDisabled("HandOfTheApprentice")) { return; }

                string[] handOfTheApprenticeGUIDs = new string[] {
                    "864146bb3e41e3644b18e1ee4cc26acf", // Capacité à volonté
                    "38aab7423d96de84d8e6ab2cdbccce63"  // Seconde capacité
                };

                foreach (var guid in handOfTheApprenticeGUIDs) {
                    var Ability = BlueprintTools.GetBlueprint<BlueprintAbility>(guid);

                    if (Ability != null) {
                        Ability.TemporaryContext(bp => {
                            bp.CanTargetFriends = false;
                            bp.CanTargetEnemies = true;
                            bp.CanTargetPoint = false;
                            bp.CanTargetSelf = false;

                            TTTContext.Logger.LogPatch("Patched HandOfTheApprentice (Wizard)", bp);
                        });
                    }
                }
            }
        }
    }
}
