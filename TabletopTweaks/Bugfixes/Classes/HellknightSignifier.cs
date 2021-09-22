using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes
{
    class HellKnightSignifier
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Hellknight Signifier Resources");
                BlueprintCharacterClass Signifier = Resources.GetBlueprint<BlueprintCharacterClass>("ee6425d6392101843af35f756ce7fefd");

                PatchArmorTraining();
                void PatchArmorTraining()
                {
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }

                    var BaseProgression = Signifier.Progression;
                    LevelEntry level1 = BaseProgression.LevelEntries.FirstOrDefault(x => x.Level == 1);
                    level1.m_Features.Add(Resources.GetModBlueprint<BlueprintFeature>("HellknightSigniferArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());

                    var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                    var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");


                    BaseProgression.LevelEntries
                        .Where(entry => entry.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                            entry.m_Features.Remove(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                        });

                    Main.LogPatch("Patched", BaseProgression);

                }
            }
        }
    }
}
