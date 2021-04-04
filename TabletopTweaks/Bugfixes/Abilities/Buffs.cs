using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.NewComponents;
using UnityEngine;

namespace TabletopTweaks.Bugfixes.Abilities {
    class Buffs {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Buff Resources");
                patchUnholyAura();
                Main.LogHeader("Patching Buffs Complete");
            }
            static void patchUnholyAura() {
                BlueprintBuff UnholyAuraBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("9eda82a1f78558747a03c17e0e9a1a68");

                var SpellImmunity = ScriptableObject.CreateInstance<SpellImmunityToSpellDescriptorAgainstAlignment>();
                SpellImmunity.Alignment = AlignmentComponent.Good;
                SpellImmunity.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                SpellImmunity.name = $"{SpellImmunity.GetType().Name}";
                var BuffImmunity = ScriptableObject.CreateInstance<BuffDescriptorImmunityAgainstAlignment>();
                BuffImmunity.Alignment = AlignmentComponent.Good;
                BuffImmunity.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                BuffImmunity.name = $"{BuffImmunity.GetType().Name}";

                UnholyAuraBuff.ComponentsArray = UnholyAuraBuff.ComponentsArray
                    .Where(c => !(c is SpellImmunityToSpellDescriptor || c is BuffDescriptorImmunity))
                    .Append(SpellImmunity)
                    .Append(BuffImmunity)
                    .ToArray();
                Main.LogPatch("Patched", UnholyAuraBuff);
            }
        }
    }
}
