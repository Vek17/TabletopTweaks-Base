using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using System;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Items {
    static class Armor {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Armor");
                PatchHaramaki();
                PatchSingingSteel();

                void PatchHaramaki() {
                    if (TTTContext.Fixes.Items.Armor.IsDisabled("Haramaki")) { return; }

                    var HaramakiType = BlueprintTools.GetBlueprint<BlueprintArmorType>("9511d62bcfc57c245bf64350a5933470");
                    HaramakiType.m_ProficiencyGroup = ArmorProficiencyGroup.Light;
                    TTTContext.Logger.LogPatch("Patched", HaramakiType);
                }
                void PatchSingingSteel() {
                    if (TTTContext.Fixes.Items.Armor.IsDisabled("SingingSteel")) { return; }

                    var SingingSteelFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("793af594fa45e6a47b89719dda7d5f7a");
                    SingingSteelFeature.AddComponent<AddMechanicsFeature>(c => {
                        c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.SingingSteel;
                    });
                    TTTContext.Logger.LogPatch("Patched", SingingSteelFeature);
                }
            }
        }
        [HarmonyPatch(typeof(ItemEntityArmor), nameof(ItemEntityArmor.CanBeEquippedInternal), new Type[] { typeof(UnitDescriptor) })]
        static class IncreaseSpellSchoolDC_OnEventAboutToTrigger_Shadow_Patch {
            static BlueprintArmorType HaramakiType = BlueprintTools.GetBlueprint<BlueprintArmorType>("9511d62bcfc57c245bf64350a5933470");
            static void Postfix(ItemEntityArmor __instance, UnitDescriptor owner, ref bool __result) {
                if (TTTContext.Fixes.Items.Armor.IsDisabled("Haramaki")) { return; }
                if (__instance.Blueprint.Type == HaramakiType) { __result = true; }
            }
        }
    }
}
