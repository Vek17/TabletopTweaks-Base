using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
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
    class Oracle{
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch{
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Purifier Resources");

                PatchPurifier();
            }


            /// <summary>
            /// Purifier's lost revelations are fixed archetype specific picks on tabletop
            /// The level 3 one isn't implemented and is probably unimplementable in the context of WotR but they don't get the pick back
            /// This hurts them super hard in WotR
            /// Let's fix that
            /// </summary>
            static void PatchPurifier()
            {
                var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");

                var CelestialArmor = Resources.GetBlueprint<BlueprintFeature>("7dc8d7dede2704640956f7bc4102760a");

                var CelestialArmorMastery = Resources.GetModBlueprint<BlueprintFeature>("CelestialArmorMastery");

                PatchLevel3Revelation();
                PatchCelestialArmor();
                PatchRestoreCure();


                void PatchLevel3Revelation()
                {

                    if (ModSettings.Fixes.Oracle.Archetypes["Purifier"].IsDisabled("Level3Revelation")) { return; }

                    var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");
                    LevelEntry target = PuriferArchetype.RemoveFeatures.FirstOrDefault(x => x.Level == 3);
                    PuriferArchetype.RemoveFeatures = PuriferArchetype.RemoveFeatures.RemoveFromArray(target);
                    Main.LogPatch("Patched", PuriferArchetype);
                }



                void PatchCelestialArmor()
                {

                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }

                    var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                    var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                    CelestialArmor.RemoveComponents<AddFacts>(x => true);//This is ugly but I can't get a conditonal to work here

                    CelestialArmor.AddComponent<AddFeatureOnClassLevel>(x =>
                    {
                        x.Level = 7;
                        x.m_Class = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                        x.m_Feature = CelestialArmorMastery.ToReference<BlueprintFeatureReference>();

                    });
                    CelestialArmor.AddComponent<AddFeatureOnClassLevel>(x =>
                    {
                        x.Level = 7;
                        x.m_Class = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                        x.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();

                    });

                    void AddSelectionToLevel(int level)
                    {
                        LevelEntry l = PuriferArchetype.AddFeatures.FirstOrDefault(x => x.Level == level);
                        if (l == null)
                        {
                            l = new LevelEntry
                            {
                                Level = level



                            };
                            l.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                            PuriferArchetype.AddFeatures.AddItem(l);

                        }
                        else
                        {
                            l.Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                        }
                    }

                    AddSelectionToLevel(11);
                    AddSelectionToLevel(15);
                    AddSelectionToLevel(19);

                    Main.LogPatch("Patched", CelestialArmor);
                }


                void PatchRestoreCure()
                {

                    if (ModSettings.Fixes.Oracle.Archetypes["Purifier"].IsDisabled("RestoreEarlyCure")) { return; }

                    var earlycure = Resources.GetModBlueprint<BlueprintFeature>("PurifierLimitedCures");



                    LevelEntry l = PuriferArchetype.AddFeatures.FirstOrDefault(x => x.Level == 1);
                    if (l == null)
                    {
                        l = new LevelEntry
                        {
                            Level = 1,
                            Features = { earlycure }



                        };

                        //l.Features.Add(earlycure);
                        PuriferArchetype.AddFeatures = PuriferArchetype.AddFeatures.AddToArray(l);


                    }
                    else
                    {

                        l.Features.Add(earlycure);
                    }
                    Main.LogPatch("Patched", earlycure);







                }
            }
        }
    }
}