using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Linq;

namespace TabletopTweaks.Bugfixes.General {
    static class ContextRankConfigFix {

        [HarmonyPatch(typeof(ContextRankConfig), nameof(ContextRankConfig.GetBaseValue), new Type[] { typeof(MechanicsContext) })]
        static class AbilityData_RequireFullRoundAction_QuickStudy_Patch {
            static void Postfix(ContextRankConfig __instance, MechanicsContext context, ref int __result) {
                switch (__instance.m_BaseValueType) {
                    case ContextRankBaseValueType.SummClassLevelWithArchetype: {
                            int value = 0;
                            var additionalArchetypes = __instance.m_AdditionalArchetypes ?? new BlueprintArchetypeReference[0];
                            var archetypes = additionalArchetypes.AddToArray(__instance.Archetype);
                            foreach (ClassData characterClass in context.MaybeCaster.Descriptor.Progression.Classes) {
                                if (__instance.m_ExceptClasses ?
                                    !__instance.m_Class.HasReference(characterClass.CharacterClass) : __instance.m_Class.HasReference(characterClass.CharacterClass)) {

                                    if (archetypes.Length > 0) {

                                        if (archetypes.Any(archetype => characterClass.CharacterClass.Archetypes.HasReference(archetype))) {

                                            if (__instance.Archetypes.Any(archetype => characterClass.Archetypes.Contains(archetype) || characterClass.Archetypes.Contains(__instance.Archetype))) {
                                                value += characterClass.Level + context.Params.RankBonus;
                                            }
                                        }
                                    } else {
                                        value += characterClass.Level + context.Params.RankBonus;
                                    }
                                }
                            }
                            __result = value;
                        }
                        break;
                    case ContextRankBaseValueType.MaxClassLevelWithArchetype: {
                            int value = 0;
                            var additionalArchetypes = __instance.m_AdditionalArchetypes ?? new BlueprintArchetypeReference[0];
                            var archetypes = additionalArchetypes.AddToArray(__instance.Archetype);
                            foreach (ClassData characterClass in context.MaybeCaster.Descriptor.Progression.Classes) {
                                if (__instance.m_ExceptClasses ?
                                    !__instance.m_Class.HasReference(characterClass.CharacterClass) : __instance.m_Class.HasReference(characterClass.CharacterClass)) {

                                    if (archetypes.Length > 0) {

                                        if (archetypes.Any(archetype => characterClass.CharacterClass.Archetypes.HasReference(archetype))) {

                                            if (__instance.Archetypes.Any(archetype => characterClass.Archetypes.Contains(archetype) || characterClass.Archetypes.Contains(__instance.Archetype))) {
                                                value = Math.Max(value, characterClass.Level + context.Params.RankBonus);
                                            }
                                        }
                                    } else {
                                        value = Math.Max(value, characterClass.Level + context.Params.RankBonus);
                                    }
                                }
                            }
                            __result = value;
                        }
                        break;
                }
            }
        }
    }
}
