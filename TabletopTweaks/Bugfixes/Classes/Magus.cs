using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Magus {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Magus Resources");

                PatchBase();
                PatchArmoredBattlemage();
            }
            static void PatchBase() {
            }
        }

        static void PatchArmoredBattlemage()
        {
            var ArmoredBattlemageArchetype = Resources.GetBlueprint<BlueprintArchetype>("67ec8dcae6fb3d3439e5ae874ddc7b9b");
            if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }

            ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 1).m_Features.Add(Resources.GetModBlueprint<BlueprintFeature>("ArmoredBattlemageArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());
            var ArmoredBattlemageArmorTraining = Resources.GetBlueprint<BlueprintFeature>("7be523d531bb17449bdba98df0e197ff");

            var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");

            ArmoredBattlemageArmorTraining.RemoveComponents<AddFacts>(x => true);//wipes all the armor trainings - couldn't find a syntax that's more specific that would boot, sorry

            ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 8).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
            ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 13).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
            ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 18).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());

            ArmoredBattlemageArmorTraining.AddComponent<AddFeatureOnClassLevel>(x =>
            {
                x.Level = 3;
                x.m_Class = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                x.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();
            });

            Main.LogPatch("Patched", ArmoredBattlemageArmorTraining);
        }

        [HarmonyPatch(typeof(UnitPartMagus), "IsSpellFromMagusSpellList", new Type[] { typeof(AbilityData) })]
        class UnitPartMagus_IsSpellFromMagusSpellList_VarriantAbilities_Patch {
            static void Postfix(UnitPartMagus __instance, ref bool __result, AbilityData spell) {
                if (ModSettings.Fixes.Magus.Base.IsDisabled("SpellCombatAbilityVariants")) { return; }
                if (spell.ConvertedFrom != null) {
                    __result |= __instance.IsSpellFromMagusSpellList(spell.ConvertedFrom);
                }
            }
        }
    }
}