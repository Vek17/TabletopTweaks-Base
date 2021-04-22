using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class MythicAbilities {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Settings.Fixes.MythicAbilities.DisableAllFixes) { return; }
                Main.LogHeader("Patching Mythic Ability Resources");
                PatchBloodlineAscendance();
                PatchSecondBloodline();
                Main.LogHeader("Patching Mythic Ability Resources Complete");
            }
            static void PatchBloodlineAscendance() {
                if (!Settings.Fixes.MythicAbilities.Fixes["BloodlineAscendance"]) { return; }
                BlueprintFeatureSelection BloodlineAscendance = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
                var newPrerequisites = Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>(),    // EmpyrealBloodlineProgression
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("da48f9d7f697ae44ca891bfc50727988").ToReference<BlueprintFeatureReference>()     // BloodOfDragonsSelection - Dragon Disciple
                    };
                    c.Amount = 1;
                });
                BloodlineAscendance.SetComponents(BloodlineAscendance.ComponentsArray
                    .Where(c => c.GetType() != typeof(PrerequisiteFeature))
                    .Append(newPrerequisites)
                );
                Main.LogPatch("Patched", BloodlineAscendance);
            }
            static void PatchSecondBloodline() {
                if (!Settings.Fixes.MythicAbilities.Fixes["SecondBloodline"]) { return; }
                BlueprintFeatureSelection SecondBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");

                var SeekerBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");
                SeekerBloodlineSelection.m_Features.ForEach(bloodline => {
                    var capstone = ((BlueprintProgression)bloodline.Get()).LevelEntries.Where(entry => entry.Level == 20)
                        .SelectMany(entry => entry.Features.Select(f => f))
                        .Where(f => f.GetComponent<Prerequisite>())
                        .First();
                    capstone.GetComponents<Prerequisite>().ForEach(c => c.Group = Prerequisite.GroupType.Any);
                    if (!capstone.GetComponents<PrerequisiteFeature>().Any(c => c.m_Feature.Get() == bloodline.Get())) {
                        capstone.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                            c.m_Feature = bloodline;
                            c.Group = Prerequisite.GroupType.Any;
                        }));
                    }
                });

                var newPrerequisites = Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                        ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>()     // EmpyrealBloodlineProgression
                    };
                    c.Amount = 1;
                });
                SecondBloodline.RemoveComponents<PrerequisiteFeature>();
                SecondBloodline.AddComponent(newPrerequisites);
                Main.LogPatch("Patched", SecondBloodline);
            }

        }
        [HarmonyPatch(typeof(ItemEntity), "AddEnchantment")]
        static class ItemEntity_AddEnchantment_EnduringSpells_Patch {
            static BlueprintFeature EnduringSpells = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("2f206e6d292bdfb4d981e99dcf08153f");
            static BlueprintFeature EnduringSpellsGreater = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("13f9269b3b48ae94c896f0371ce5e23c");
            static bool Prefix(MechanicsContext parentContext, ref Rounds? duration, BlueprintItemEnchantment blueprint) {
                if (Settings.Fixes.MythicAbilities.DisableAllFixes || !Settings.Fixes.MythicAbilities.Fixes["EnduringSpells"]) { return true; }
                if (parentContext != null && parentContext.MaybeOwner != null && duration != null) {

                    var owner = parentContext.MaybeOwner;
                    if (owner.Descriptor.HasFact(EnduringSpells)) {
                        if (owner.Descriptor.HasFact(EnduringSpellsGreater) && duration >= (DurationRate.Minutes.ToRounds() * 5)) {
                            duration = DurationRate.Days.ToRounds();
                        } else if (duration >= DurationRate.Hours.ToRounds()) {
                            duration = DurationRate.Days.ToRounds();
                        }
                    }
                }
                return true;
            }
        }
    }
}
