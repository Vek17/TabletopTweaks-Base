using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    class BladeAdept {
        public static void AddBladeAdept() {
            var ArcanistClass = Resources.GetBlueprint<BlueprintCharacterClass>("52dbfd8505e22f84fad8d702611f60b7");
            var ArcanistExploitSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
            var BlackBladeProgression = Resources.GetModBlueprint<BlueprintProgression>("BlackBladeProgression");

            var BladeAdeptArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("BladeAdeptArchetype", bp => {
                bp.SetName("Blade Adept");
                bp.SetDescription("A small number of arcanists learn to use blades as part of their spellcasting " +
                    "and in combat. While these blade adepts are not as capable with a sword as a true master " +
                    "duelist, their combination of swordplay and arcane power makes them quite deadly.");
            });
            // CREATE ITEM BOND
            BladeAdeptArchetype.RemoveFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, ArcanistExploitSelection),
                Helpers.CreateLevelEntry(3, ArcanistExploitSelection),
                Helpers.CreateLevelEntry(9, ArcanistExploitSelection)
            };
            BladeAdeptArchetype.AddFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, BlackBladeProgression) // FIX
            };
            BlackBladeProgression.AddArchetype(BladeAdeptArchetype);

            //if (ModSettings.AddedContent.Archetypes.IsDisabled("BladeAdept")) { return; }
            //ArcanistClass.m_Archetypes = ArcanistClass.m_Archetypes.AppendToArray(BladeAdeptArchetype.ToReference<BlueprintArchetypeReference>());
            //Main.LogPatch("Added", BladeAdeptArchetype);
        }
    }
}
