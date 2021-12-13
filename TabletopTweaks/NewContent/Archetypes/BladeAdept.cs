using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewComponents.Properties;
using TabletopTweaks.Utilities;
using UnityEngine;
using static TabletopTweaks.NewUnitParts.CustomStatTypes;

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

            if (ModSettings.AddedContent.Archetypes.IsDisabled("BladeAdept")) { return; }
            ArcanistClass.m_Archetypes = ArcanistClass.m_Archetypes.AppendToArray(BladeAdeptArchetype.ToReference<BlueprintArchetypeReference>());
            Main.LogPatch("Added", BladeAdeptArchetype);
        }
    }
}
