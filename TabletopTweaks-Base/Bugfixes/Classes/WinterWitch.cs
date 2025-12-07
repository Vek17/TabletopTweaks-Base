using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Core;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.Utilities.ClassTools;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class WinterWitch {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Winter Witch Resources");
                AddColdFlesh();
                AddUnnaturalCold();
                PatchUnearthlyCold();
                PatchWitchFakeLevels();
            }
            static void AddColdFlesh() {
                if (TTTContext.Fixes.WinterWitch.IsDisabled("ColdFlesh")) { return; }

                var WinterWitchProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("2703dc14f69421a44866e9046a95f348");
                var WinterWitchColdFlesh5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "WinterWitchColdFlesh5");
                var WinterWitchColdFlesh10 = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "WinterWitchColdFlesh10");
                var WinterWitchColdFleshImmunity = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "WinterWitchColdFleshImmunity");

                WinterWitchProgression.TemporaryContext(bp => {
                    bp.LevelEntries.Where(e => e.Level == 1).First().Features.Add(WinterWitchColdFlesh5.ToReference<BlueprintFeatureBaseReference>());
                    bp.LevelEntries.Where(e => e.Level == 5).First().Features.Add(WinterWitchColdFlesh10.ToReference<BlueprintFeatureBaseReference>());
                    bp.LevelEntries.Where(e => e.Level == 10).First().Features.Add(WinterWitchColdFleshImmunity.ToReference<BlueprintFeatureBaseReference>());
                });
                ClassTools.Classes.WinterWitchClass.Progression.UIGroups = ClassTools.Classes.WinterWitchClass.Progression.UIGroups.AppendToArray(
                    Helpers.CreateUIGroup(
                        WinterWitchColdFlesh5,
                        WinterWitchColdFlesh10,
                        WinterWitchColdFleshImmunity
                    )
                );
                SaveGameFix.AddRetroactiveClassFeature(TTTContext, ClassTools.Classes.WinterWitchClass, 1, WinterWitchColdFlesh5);
                SaveGameFix.AddRetroactiveClassFeature(TTTContext, ClassTools.Classes.WinterWitchClass, 5, WinterWitchColdFlesh10);
                SaveGameFix.AddRetroactiveClassFeature(TTTContext, ClassTools.Classes.WinterWitchClass, 10, WinterWitchColdFleshImmunity);
            }
            static void AddUnnaturalCold() {
                if (TTTContext.Fixes.WinterWitch.IsDisabled("UnnaturalCold")) { return; }

                var WinterWitchProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("2703dc14f69421a44866e9046a95f348");
                var WinterWitchUnnaturalCold = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "WinterWitchUnnaturalCold");
                var WinterWitchUnearthlyCold = BlueprintTools.GetBlueprint<BlueprintFeature>("d50282428eb0a9b489bff9f687dd208c");

                WinterWitchProgression.TemporaryContext(bp => {
                    bp.LevelEntries.Where(e => e.Level == 3).First().Features.Add(WinterWitchUnnaturalCold.ToReference<BlueprintFeatureBaseReference>());
                });
                ClassTools.Classes.WinterWitchClass.Progression.UIGroups = ClassTools.Classes.WinterWitchClass.Progression.UIGroups.AppendToArray(
                    Helpers.CreateUIGroup(
                        WinterWitchUnnaturalCold,
                        WinterWitchUnearthlyCold
                    )
                );
                SaveGameFix.AddRetroactiveClassFeature(TTTContext, ClassTools.Classes.WinterWitchClass, 3, WinterWitchUnnaturalCold);
            }
            static void PatchUnearthlyCold() {
                if (TTTContext.Fixes.WinterWitch.IsDisabled("UnearthlyCold")) { return; }

                var WinterWitchUnearthlyCold = BlueprintTools.GetBlueprint<BlueprintFeature>("d50282428eb0a9b489bff9f687dd208c");
                WinterWitchUnearthlyCold.TemporaryContext(bp => {
                    bp.RemoveComponents<ChangeSpellElementalDamageHalfUntyped>();
                    bp.AddComponent<ChangeSpellHalfDamageIgnoreImmunityAndResist>(c => {
                        c.EnergyType = DamageEnergyType.Cold;
                        c.CheckAbilityType = true;
                        c.ValidAbilityTypes = new AbilityType[] { AbilityType.Spell, AbilityType.SpellLike, AbilityType.Supernatural };
                    });
                });
                TTTContext.Logger.LogPatch(WinterWitchUnearthlyCold);
            }
            static void PatchWitchFakeLevels() {
                if (TTTContext.Fixes.WinterWitch.IsDisabled("WitchProgression")) { return; }

                var WinterWitchFrostPower = BlueprintTools.GetBlueprint<BlueprintFeature>("a593f3f1f8137c74da7569dbdac62949");
                WinterWitchFrostPower.TemporaryContext(bp => {
                    bp.AddComponent<ClassLevelsForPrerequisites>(c => {
                        c.m_FakeClass = ClassReferences.WitchClass;
                        c.m_ActualClass = ClassReferences.WinterWitchClass;
                        c.Modifier = 1.0;
                        c.Summand = 0;
                    });
                });
                TTTContext.Logger.LogPatch(WinterWitchFrostPower);
            }
        }
    }
}
