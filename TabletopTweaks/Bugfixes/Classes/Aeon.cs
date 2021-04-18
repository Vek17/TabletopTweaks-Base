using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Aeon {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Settings.Fixes.Aeon.DisableAllFixes) { return; }
                Main.LogHeader("Patching Aeon Resources");
                PatchAeonBaneUses();
                Main.LogHeader("Aeon Resource Patch Complete");
            }
            static void PatchAeonBaneUses() {
                if (!Settings.Fixes.Aeon.Fixes["AeonBaneUses"]) { return; }
                var AeonClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("15a85e67b7d69554cab9ed5830d0268e");
                var AeonBaneFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
                var AeonRankContext = AeonBaneFeature.GetComponent<ContextRankConfig>();
                var InquisitorBaneResource = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityResource>("a708945b17c56fa4196e8d20f8af1b0d");
                var InquistorClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");

                List<BlueprintCharacterClassReference> characterClasses = new List<BlueprintCharacterClassReference>();
                characterClasses.AddRange(ResourcesLibrary.GetRoot().Progression.CharacterMythics.Select(c => c.ToReference<BlueprintCharacterClassReference>()));
                characterClasses.AddRange(ResourcesLibrary.GetRoot().Progression.CharacterClasses.Where(c => c.AssetGuid == InquistorClass.AssetGuid).Select(c => c.ToReference<BlueprintCharacterClassReference>()));

                AeonRankContext.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                AeonRankContext.m_Max = 40;
                AeonRankContext.m_Class = new BlueprintCharacterClassReference[] { InquistorClass.ToReference<BlueprintCharacterClassReference>() };
                AeonRankContext.m_StepLevel = 1;
                AeonRankContext.m_Progression = ContextRankProgression.AsIs;
                AeonRankContext.m_ExceptClasses = true;
                Main.LogPatch("Patched", AeonBaneFeature);
                InquisitorBaneResource.m_MaxAmount.m_Class = InquisitorBaneResource.m_MaxAmount.m_Class.AddItem(ResourcesLibrary.GetRoot().Progression.m_MythicStartingClass).ToArray();
                InquisitorBaneResource.m_MaxAmount.m_Class = InquisitorBaneResource.m_MaxAmount.m_Class.AddItem(AeonClass.ToReference<BlueprintCharacterClassReference>()).ToArray();
                Main.LogPatch("Patched", InquisitorBaneResource);
            }
        }
    }
}
