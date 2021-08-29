using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Items {
    static class Equipment {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Equipment");
                PatchMagiciansRing();

                void PatchMagiciansRing() {
                    if (ModSettings.Fixes.Items.Equipment.IsDisabled("MagiciansRing")) { return; }

                    var RingOfTheSneakyWizardFeature = Resources.GetBlueprint<BlueprintFeature>("d848f1f1b31b3e143ba4aeeecddb17f4");
                    RingOfTheSneakyWizardFeature.GetComponent<IncreaseSpellSchoolDC>().BonusDC = 2;
                    Main.LogPatch("Patched", RingOfTheSneakyWizardFeature);
                }
            }
        }
    }
}
