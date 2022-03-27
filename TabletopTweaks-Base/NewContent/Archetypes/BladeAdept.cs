using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    class BladeAdept {
        public static void AddBladeAdept() {
            var ArcanistClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("52dbfd8505e22f84fad8d702611f60b7");
            var ArcanistExploitSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
            var BlackBladeProgression = BlueprintTools.GetModBlueprint<BlueprintProgression>(TTTContext, "BlackBladeProgression");

            var BladeAdeptArchetype = Helpers.CreateBlueprint<BlueprintArchetype>(TTTContext, "BladeAdeptArchetype", bp => {
                bp.SetName(TTTContext, "Blade Adept");
                bp.SetDescription(TTTContext, "A small number of arcanists learn to use blades as part of their spellcasting " +
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

            //if (Context.AddedContent.Archetypes.IsDisabled("BladeAdept")) { return; }
            //ArcanistClass.m_Archetypes = ArcanistClass.m_Archetypes.AppendToArray(BladeAdeptArchetype.ToReference<BlueprintArchetypeReference>());
            //TTTContext.Logger.LogPatch("Added", BladeAdeptArchetype);
        }
    }
}
