using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.MechanicsChanges
{
    class FeatureSpecificEffectiveClassLevel
    {
        [HarmonyPatch(typeof(PrerequisiteClassLevel), "GetClassLevel")]
        [HarmonyPatch(new Type[] { typeof(UnitDescriptor) })]
        static class CalcEffectiveClassLevelPatch
        {
            static void Postfix(ref int __result, PrerequisiteClassLevel __instance, UnitDescriptor unit)
            {

              
                

                if (__instance.OwnerBlueprint is BlueprintFeature blueprintFeature)
                {
                 

                    foreach (FeatureSpecificClassLevelsForPrerequisites featureFake in unit.Progression.Features.SelectFactComponents<FeatureSpecificClassLevelsForPrerequisites>())
                    {

                        if (featureFake.FakeClass == __instance.CharacterClass)
                        {
                            if (blueprintFeature != null)
                            {
                                if (featureFake.IsApplicable(blueprintFeature))
                                { int prev = __result;
                                    __result += (int)(featureFake.Modifier * (double)unit.Progression.GetClassLevel(featureFake.ActualClass) + (double)featureFake.Summand);
                                    Main.Log($"Cross class feature boost called on: {blueprintFeature.Name} by {unit.CharacterName} boosting level from {prev} to {__result}");

                                }

                            }
                        }

                    }
                }



            }
        }

    }
}
