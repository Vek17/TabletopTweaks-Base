using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes
{
    static class Magus
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;



            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Magus");
               
                PatchArmoredBattlemage();
            }

            

            static void PatchArmoredBattlemage()
            {

                if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                var ArmoredBattlemageArchetype = Resources.GetBlueprint<BlueprintArchetype>("67ec8dcae6fb3d3439e5ae874ddc7b9b");

                Helpers.CreateBlueprint<BlueprintFeature>("ArmoredBattlemageArmorTrainingProgression", x =>
                    {

                        PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                        x =>
                        {

                            x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                            x.m_ActualClass = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                        });
                        x.IsClassFeature = true;
                        x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                        x.SetName("Armored Battlemage Armor Training Progression");
                        x.SetDescription("Increases your armor training rank by your Armored Battlemage Armor level, progressing Advanced Armor Training abilities.");
                        x.Ranks = 1;
                        x.AddComponent(progression);

                    });



                
                
               
                ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 1).m_Features.Add(Resources.GetModBlueprint<BlueprintFeature>("ArmoredBattlemageArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());
                var ArmoredBattlemageArmorTraining = Resources.GetBlueprint<BlueprintFeature>("7be523d531bb17449bdba98df0e197ff");

                var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");

                ArmoredBattlemageArmorTraining.RemoveComponents<AddFacts>(x => true);//wipes all the armor trainings - couldn't find a syntax that's more specific that would boot, sorry

                //3c380607706f209499d951b29d3c44f3

                //3c380607706f209499d951b29d3c44f3

                // 354f1a4426d24ea38718905108f48e72

                ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 8).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 13).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 18).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                
                ArmoredBattlemageArmorTraining.AddComponent<AddFeatureOnClassLevel>(x =>
                {
                    x.Level = 3;
                    x.m_Class = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();
                    //x.m_Feature = ArmorTrainingSelection.ToReference<BlueprintFeatureReference>();



                });
                /*
                ArmoredBattlemageArmorTraining.AddComponent<AddFeatureOnClassLevel>(x =>
                {
                    x.Level = 8;
                    x.m_Class = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.m_Feature = ArmorTrainingSelection.ToReference<BlueprintFeatureReference>();



                });
                ArmoredBattlemageArmorTraining.AddComponent<AddFeatureOnClassLevel>(x =>
                {
                    x.Level = 13;
                    
                    x.m_Class = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.m_Feature = ArmorTrainingSelection.ToReference<BlueprintFeatureReference>();



                });
                ArmoredBattlemageArmorTraining.AddComponent<AddFeatureOnClassLevel>(x =>
                {
                    x.Level = 18;
                    x.m_Class = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.m_Feature = ArmorTrainingSelection.ToReference<BlueprintFeatureReference>();



                });

                */
                Main.LogPatch("Patched", ArmoredBattlemageArmorTraining);
            }
        }
    }
}
