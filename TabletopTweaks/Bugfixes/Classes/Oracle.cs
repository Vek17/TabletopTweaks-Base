using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

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
                PatchLevel3Revelation();

                void PatchLevel3Revelation()
                {
                    
                    if (ModSettings.Fixes.Oracle.Archetypes["Purifier"].IsEnabled("Level3Revelation"))
                    {
                        var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");
                        try
                        {
                            LevelEntry target = PuriferArchetype.RemoveFeatures.FirstOrDefault(x => x.Level == 3);
                            if (target == null)
                            {
                                Main.LogDebug("Failed to get level 3 removal");
                            }
                            else
                            {
                               PuriferArchetype.RemoveFeatures = PuriferArchetype.RemoveFeatures.RemoveFromArray(target);
                            }
                            Main.LogPatch("Patched", PuriferArchetype);
                        }
                        catch (Exception e)
                        {
                            Main.LogDebug(e.Message);
                        }
                    }
                }
                

            }
        }
    }

}
