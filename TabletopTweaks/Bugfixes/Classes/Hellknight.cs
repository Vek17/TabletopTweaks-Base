using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Hellknight {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Hellknight Resources");
                
                BlueprintCharacterClass Hellknight = Resources.GetBlueprint<BlueprintCharacterClass>("ed246f1680e667b47b7427d51e651059");
                PatchPentamicFaith();

                PatchArmorTraining();
                void PatchPentamicFaith() {
                    if (ModSettings.Fixes.Hellknight.IsDisabled("PentamicFaith")) { return; }

                    var HellKnightOrderOfTheGodclaw = Resources.GetBlueprint<BlueprintFeature>("5636564c278583342aec54eb2b409029");
                    var HellknightDisciplinePentamicFaith = Resources.GetBlueprint<BlueprintFeatureSelection>("b9750875e9d7454e85347d739a1bc894");

                    HellknightDisciplinePentamicFaith.RemovePrerequisites<PrerequisiteFeature>();
                    HellknightDisciplinePentamicFaith.AddPrerequisiteFeature(HellKnightOrderOfTheGodclaw);
                    Main.LogPatch("Patched", HellknightDisciplinePentamicFaith);
                }   

                void PatchArmorTraining()
                {
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                    void HellknightArmorTrainingProgression()
                    {

                       

                    }
                    HellknightArmorTrainingProgression();

                   
                    var BaseProgression = Hellknight.Progression;
                    LevelEntry level1 = BaseProgression.LevelEntries.FirstOrDefault(x => x.Level == 1);
                    level1.m_Features.Add(Resources.GetModBlueprint<BlueprintFeature>("HellknightArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());
                    
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
