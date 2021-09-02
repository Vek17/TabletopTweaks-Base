using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Arcanist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Arcanist Resources");

                PatchBase();
            }
            static void PatchBase() {
                PatchArcaneReservoir();

                void PatchArcaneReservoir() {
                    if (ModSettings.Fixes.Arcanist.Base.IsDisabled("ArcaneReservoir")) { }

                    var ArcanistArcaneReservoirResourceBuff = Resources.GetBlueprint<BlueprintBuff>("1dd776b7b27dcd54ab3cedbbaf440cf3");
                    var Actions = ArcanistArcaneReservoirResourceBuff.GetComponent<AddFactContextActions>().Activated;
                    ArcanistArcaneReservoirResourceBuff.Stacking = StackingType.Replace;
                    ArcanistArcaneReservoirResourceBuff.m_Flags &= ~BlueprintBuff.Flags.RemoveOnRest;
                    ArcanistArcaneReservoirResourceBuff.RemoveComponents<AddFactContextActions>();
                    ArcanistArcaneReservoirResourceBuff.AddComponent<AddRestTrigger>(c => {
                        c.Action = Actions;
                    });
                    Main.LogPatch("Patched", ArcanistArcaneReservoirResourceBuff);
                }
            }
        }

        [HarmonyPatch(typeof(ContextSpendResource), "RunAction")]
        static class FeatureSelectionExtensions_CanSelectAny_Patch {
            static bool Prefix(ref ContextSpendResource __instance) {
                Main.LogDebug($" - {__instance.Resource.name}: {__instance?.Context?.MaybeCaster?.Resources.GetResourceAmount(__instance.Resource)}");
                return true;
            }
            static void Postfix(ref ContextSpendResource __instance) {
                Main.LogDebug($"{__instance.Owner.name} - {__instance?.Value.Calculate(__instance.Context)}");
            }
        }

        [HarmonyPatch(typeof(ActionBarVM), "CollectSpells", new Type[] { typeof(UnitEntityData) })]
        static class Arcanist_SpellbookActionBar_Patch {
            static readonly FieldInfo BlueprintSpellbook_Spontaneous = AccessTools.Field(typeof(BlueprintSpellbook), "Spontaneous");
            static readonly FieldInfo BlueprintSpellbook_IsArcanist = AccessTools.Field(typeof(BlueprintSpellbook), "IsArcanist");
            static readonly FieldInfo Spellbook_BlueprintSpellbook = AccessTools.Field(typeof(Spellbook), "Blueprint");
            //Add an exception to the spontantous spell UI if the spellbook is arcanist
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (ModSettings.Fixes.Arcanist.Base.IsDisabled("PreparedSpellUI")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldloc_S, 5),
                    new CodeInstruction(OpCodes.Ldfld, Spellbook_BlueprintSpellbook),
                    new CodeInstruction(OpCodes.Ldfld, BlueprintSpellbook_IsArcanist),
                    new CodeInstruction(OpCodes.Not),
                    new CodeInstruction(OpCodes.And),
                });
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Ldfld && codes[i].LoadsField(BlueprintSpellbook_Spontaneous)) {
                        return i + 1;
                    }
                }
                Main.Error("ARCANIST SPELLBOOK ACTION BAR PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
