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

                BlueprintFeature feature = null;
                

                if (__instance.OwnerBlueprint is BlueprintFeature blueprintFeature)
                {
                    feature = blueprintFeature;

                    foreach (FeatureSpecificClassLevelsForPrerequisites featureFake in unit.Progression.Features.SelectFactComponents<FeatureSpecificClassLevelsForPrerequisites>())
                    {

                        if (featureFake.FakeClass == __instance.CharacterClass)
                        {
                            if (feature != null)
                            {
                                if (featureFake.IsApplicable(feature))
                                {
                                    __result += (int)(featureFake.Modifier * (double)unit.Progression.GetClassLevel(featureFake.ActualClass) + (double)featureFake.Summand);


                                }

                            }
                        }

                    }
                }



            }
        }

    }
}
