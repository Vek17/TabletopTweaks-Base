using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;
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
                var SpellImmunity = Helpers.Create<SpellImmunityToSpellDescriptorAgainstAlignment>(c => {
                    c.Alignment = AlignmentComponent.Good;
                    c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                });
                var BuffImmunity = Helpers.Create<BuffDescriptorImmunityAgainstAlignment>(c => {
                    c.Alignment = AlignmentComponent.Good;
                    c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                });
                UnholyAuraBuff.SetComponents(UnholyAuraBuff.ComponentsArray
                    .Where(c => !(c is SpellImmunityToSpellDescriptor || c is BuffDescriptorImmunity))
                    .Append(SpellImmunity)
                    .Append(BuffImmunity)
                    .ToArray());
                UnholyAuraBuff.SetDescription("A malevolent darkness surrounds the subjects, protecting them from attacks, "
                    +"granting them resistance to spells cast by good creatures, and weakening good creatures when they strike the subjects. "
                    +"This abjuration has four effects.\nFirst, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves."
                    +"[LONGSTART] Unlike the effect of protection from good, this benefit applies against all attacks, not just against attacks by good creatures."
                    +"[LONGEND]\nSecond, warded creatures gain spell resistance 25 against good spells and spells cast by good creatures.\n"
                    + "Third, the abjuration protects from all charm or compulsion good spells and spells cast by good creatures.\n"
                    + "Finally, if a good creature succeeds on a melee attack against a warded creature, "
                    +"the offending attacker takes 1d6 points of Strength damage (Fortitude negates).");

                Main.LogPatch("Patched", UnholyAuraBuff);
            }
        }
    }
}
