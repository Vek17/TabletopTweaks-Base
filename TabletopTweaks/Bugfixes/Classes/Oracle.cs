using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
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
    class Oracle
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
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
                var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
                var CelestialArmor = Resources.GetBlueprint<BlueprintFeature>("7dc8d7dede2704640956f7bc4102760a");
                var FighterRef = FighterClass.ToReference<BlueprintCharacterClassReference>();
                
                var CelestialArmorMastery = Helpers.CreateBlueprint<BlueprintFeature>("CelestialArmorMastery", c =>
                {
                    c.SetName("Celestial Armor Mastery");
                    c.SetDescription("Celestial Armor Mastery Effect");
                    c.IsClassFeature = true;
                    c.HideInCharacterSheetAndLevelUp = true;
                    c.Ranks = 1;
                    c.m_Icon = CelestialArmor.Icon;
                    
                    
                    FeatureSpecificClassLevelsForPrerequisites FighterSplice = Helpers.Create<FeatureSpecificClassLevelsForPrerequisites>(
                        
                        b =>{
                            b.Modifier = 1;
                            b.Summand = -4;
                            b.m_ActualClass = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                            b.m_FakeClass = FighterRef;
                            b._Applicable = (x => x.Name.Contains("Armor")&& x.Name.Contains("Advanced") && x.name.Contains("Training"));
                        }
                        );
                    
                    c.AddComponent(FighterSplice);
                });
                







                PatchLevel3Revelation();
                PatchCelestialArmor();


                void PatchLevel3Revelation()
                {

                    if (ModSettings.Fixes.Oracle.Archetypes["Purifier"].IsDisabled("Level3Revelation")) { return; }

                    var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");

                    LevelEntry target = PuriferArchetype.RemoveFeatures.FirstOrDefault(x => x.Level == 3);
                    Main.LogPatch("Patched", PuriferArchetype);


                }



                void PatchCelestialArmor()
                {
                    if (ModSettings.AddedContent.PurifierCelestialArmor.IsDisabled("UnlockFeats")) { return; }



                    CelestialArmor.AddComponent<AddFeatureOnClassLevel>(x =>
                    {
                        x.Level = 7;
                        x.m_Class = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                        x.m_Feature = CelestialArmorMastery.ToReference<BlueprintFeatureReference>();



                    });
                    Main.LogPatch("Patched", CelestialArmor);
                }






            }


        }
    }
}


