using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Arcanist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Arcanist_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Arcanist")) { return; }

                var ArcanistMagicalSupremacy = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("261270d064148224fb982590b7a65414");
                var ArcanistAlternateCapstone = NewContent.AlternateCapstones.Arcanist.ArcanistAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                ArcanistMagicalSupremacy.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.ArcanistClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == ArcanistMagicalSupremacy.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(ArcanistAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(ArcanistAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == ArcanistMagicalSupremacy.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(ArcanistAlternateCapstone));
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
                TTTContext.Logger.LogHeader("Patching Arcanist Resources");

                PatchBase();
            }
            static void PatchBase() {
            }
        }

        [HarmonyPatch(typeof(ActionBarVM), "CollectSpells", new Type[] { typeof(UnitEntityData) })]
        static class Arcanist_SpellbookActionBar_Patch {
            static readonly FieldInfo BlueprintSpellbook_Spontaneous = AccessTools.Field(typeof(BlueprintSpellbook), "Spontaneous");
            static readonly FieldInfo BlueprintSpellbook_IsArcanist = AccessTools.Field(typeof(BlueprintSpellbook), "IsArcanist");
            static readonly FieldInfo Spellbook_BlueprintSpellbook = AccessTools.Field(typeof(Spellbook), "Blueprint");
            //Add an exception to the spontantous spell UI if the spellbook is arcanist
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (TTTContext.Fixes.Arcanist.Base.IsDisabled("PreparedSpellUI")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldloc_S, 2),
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

                TTTContext.Logger.Log("ARCANIST SPELLBOOK ACTION BAR PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
