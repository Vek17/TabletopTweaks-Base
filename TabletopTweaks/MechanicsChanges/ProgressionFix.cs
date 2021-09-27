using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Utility;
using Kingmaker.Blueprints;

namespace TabletopTweaks.MechanicsChanges {
    class ProgressionFix {

        [HarmonyPatch(typeof(BlueprintProgression), "CalcLevel")]
        static class Fix {

            static bool Prefix(BlueprintProgression __instance, ref int __result, [NotNull] UnitDescriptor unit) {
                if (__instance.m_Classes.Empty<BlueprintProgression.ClassWithLevel>() && __instance.m_Archetypes.Empty<BlueprintProgression.ArchetypeWithLevel>()) {
                    return true;
                }
                if (!__instance.ForAllOtherClasses)
                    return true;
                else {
                    int num = 0;
                    List<BlueprintCharacterClassReference> checklist = new List<BlueprintCharacterClassReference>();
                    List<BlueprintArchetypeReference> checklist2 = new List<BlueprintArchetypeReference>();
                    if (__instance.FeatureRankIncrease != null && unit.HasFact(__instance.FeatureRankIncrease)) {
                        num += unit.GetFact(__instance.FeatureRankIncrease).GetRank();
                    }
                    BlueprintProgression.ClassWithLevel[] array = __instance.m_Classes;
                    int i;
                    for (i = 0; i < array.Length; i++) {
                        BlueprintProgression.ClassWithLevel classWithLevel = array[i];
                        if (!classWithLevel.Class.Archetypes.Any((BlueprintArchetype a) => __instance.m_Archetypes.Contains((BlueprintProgression.ArchetypeWithLevel i) => i.Archetype == a))) {
                            num += Math.Max(0, unit.Progression.GetClassLevel(classWithLevel.Class) + classWithLevel.AdditionalLevel);
                            if (unit.Progression.GetClassLevel(classWithLevel.Class) > 0) {
                                checklist.Add(classWithLevel.Class.ToReference<BlueprintCharacterClassReference>());
                            }
                        }
                    }
                    BlueprintProgression.ArchetypeWithLevel[] archetypes = __instance.m_Archetypes;
                    for (i = 0; i < archetypes.Length; i++) {
                        BlueprintProgression.ArchetypeWithLevel archetypeWithLevel = archetypes[i];
                        foreach (ClassData classData in unit.Progression.Classes) {
                            if (classData.Archetypes.HasItem(archetypeWithLevel.Archetype)) {
                                num += Math.Max(0, classData.Level + archetypeWithLevel.AdditionalLevel);
                                if (classData.Level + archetypeWithLevel.AdditionalLevel > 0) {

                                    checklist2.Add(archetypeWithLevel.Archetype.ToReference<BlueprintArchetypeReference>());
                                }
                            }
                        }
                    }
                    int num2 = 0;
                    if (__instance.ForAllOtherClasses) {
                        using (List<ClassData>.Enumerator enumerator = unit.Progression.Classes.GetEnumerator()) {
                            while (enumerator.MoveNext()) {
                                ClassData c = enumerator.Current;
                                if (checklist.Contains(c.CharacterClass.ToReference<BlueprintCharacterClassReference>())) {

                                } else {

                                    if (!__instance.m_Classes.HasItem((BlueprintProgression.ClassWithLevel i) => i.Class == c.CharacterClass) || !c.Archetypes.HasItem((BlueprintArchetype a) => __instance.m_Archetypes.HasItem((BlueprintProgression.ArchetypeWithLevel i) => i.Archetype == a))) {
                                        num2 += c.Level;
                                    }
                                }
                            }

                        }
                    }
                    if (__instance.AlternateProgressionType == AlternateProgressionType.Div2) {
                        num += num2 / 2;
                    }
                    __result = num;
                    return false;
                }
            }





        }
    }
}
