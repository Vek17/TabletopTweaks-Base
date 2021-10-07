using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.MechanicsChanges {
    class BlueprintProgressionFix {

        [HarmonyPatch(typeof(BlueprintProgression), nameof(BlueprintProgression.CalcLevel))]
        static class BlueprintProgression_AllOtherClasses_CalcLevel_Fix {

            static void Postfix(BlueprintProgression __instance, ref int __result, [NotNull] UnitDescriptor unit) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("AlternateClassProgressions")) { return; }
                if (!__instance.ForAllOtherClasses) { return; }
                Main.Log($"{__instance.name} - {__result}");
                //Old Progression logic so we can remove it from the total
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
                    Main.Log($"TotalValue: {__result}\n");
                }
                //New correct logic
                int newIncrease = 0;
                foreach (ClassData characterClass in unit.Progression.Classes.Where(c => !c.CharacterClass.IsMythic)) {
                    bool InClasses = __instance.m_Classes.Any(classWithLevel => classWithLevel.Class == characterClass.CharacterClass);
                    bool ClassHasArchetype = __instance.m_Archetypes.Any(archetypeWithLevel => characterClass.CharacterClass.Archetypes.HasReference(archetypeWithLevel.Archetype));
                    bool NotValidArchetype = !__instance.m_Archetypes.Any(archetypeWithLevel => characterClass.Archetypes.Contains(archetypeWithLevel.Archetype));

                    if (!InClasses || (InClasses && ClassHasArchetype && NotValidArchetype)) {
                        newIncrease += characterClass.Level;
                    }
                }
                switch (__instance.AlternateProgressionType) {
                    case AlternateProgressionType.Div2:
                        newIncrease /= 2;
                        break;
                }
                __result += newIncrease;
            }
        }
    }
}
