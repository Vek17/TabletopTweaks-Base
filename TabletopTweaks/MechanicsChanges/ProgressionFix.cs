using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.MechanicsChanges {
    class ProgressionFix {

        [HarmonyPatch(typeof(BlueprintProgression), nameof(BlueprintProgression.CalcLevel))]
        static class BlueprintProgression_AllOtherClasses_CalcLevel_Fix {

            static void Postfix(BlueprintProgression __instance, ref int __result, [NotNull] UnitDescriptor unit) {
                if (!__instance.ForAllOtherClasses) { return; }
                Main.Log($"{__instance.name} - {__result}");
                using (List<ClassData>.Enumerator enumerator = unit.Progression.Classes.GetEnumerator()) {
                    int originalIncrease = 0;
                    while (enumerator.MoveNext()) {
                        ClassData c = enumerator.Current;
                        if (!__instance.m_Classes.HasItem((BlueprintProgression.ClassWithLevel i) => i.Class == c.CharacterClass) 
                            || !c.Archetypes.HasItem((BlueprintArchetype a) => __instance.m_Archetypes.HasItem((BlueprintProgression.ArchetypeWithLevel i) => i.Archetype == a))) {
                            originalIncrease += c.Level;
                        }
                    }
                    switch (__instance.AlternateProgressionType) {
                        case AlternateProgressionType.Div2:
                            originalIncrease /= 2;
                            break;
                    }
                    Main.Log($"Original Increase: {originalIncrease}");
                    __result -= originalIncrease;
                    Main.Log($"TotalValue: {__result}");
                }
                int newIncrease = 0;
                foreach (ClassData characterClass in unit.Progression.Classes.Where(c => !c.CharacterClass.IsMythic)) {
                    if (!__instance.m_Classes.HasItem((BlueprintProgression.ClassWithLevel i) => i.Class == characterClass.CharacterClass)
                        && !characterClass.Archetypes.HasItem((BlueprintArchetype a) => __instance.m_Archetypes.HasItem((BlueprintProgression.ArchetypeWithLevel i) => i.Archetype == a))) {
                        newIncrease += 1;
                    }
                }
                switch (__instance.AlternateProgressionType) {
                    case AlternateProgressionType.Div2:
                        newIncrease /= 2;
                        break;
                }
                Main.Log($"New Increase: {newIncrease}");
                __result += newIncrease;
                Main.Log($"TotalValue: {__result}");
            }
        }
    }
}
