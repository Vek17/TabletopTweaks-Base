using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class MythicAbilities {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.MythicAbilities.DisableAll) { return; }
                Main.LogHeader("Patching Mythic Abilities");
                PatchBloodlineAscendance();
                PatchEverlastingJudgement();
                PatchSecondBloodline();
                PatchBloodragerSecondBloodline();
                PatchSecondMystery();
                PatchSecondSpirit();
            }
            static void PatchBloodlineAscendance() {
                if (!ModSettings.Fixes.MythicAbilities.Enabled["BloodlineAscendance"]) { return; }
                BlueprintFeatureSelection BloodlineAscendance = Resources.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
                var newPrerequisites = Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                        Resources.GetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                        Resources.GetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                        Resources.GetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                        Resources.GetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                        Resources.GetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>(),    // EmpyrealBloodlineProgression
                        Resources.GetBlueprint<BlueprintFeature>("da48f9d7f697ae44ca891bfc50727988").ToReference<BlueprintFeatureReference>()     // BloodOfDragonsSelection - Dragon Disciple
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
                if (!ModSettings.Fixes.MythicAbilities.Enabled["SecondBloodline"]) { return; }
                BlueprintFeatureSelection SecondBloodline = Resources.GetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");

                var SeekerBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");
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
                        Resources.GetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                        Resources.GetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                        Resources.GetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                        Resources.GetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                        Resources.GetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>()     // EmpyrealBloodlineProgression
                    };
                    c.Amount = 1;
                });
                SecondBloodline.RemoveComponents<PrerequisiteFeature>();
                SecondBloodline.AddComponent(newPrerequisites);
                Main.LogPatch("Patched", SecondBloodline);
            }
            static void PatchBloodragerSecondBloodline() {
                if (!ModSettings.Fixes.MythicAbilities.Enabled["SecondBloodragerBloodline"]) { return; }
                var ReformedFiendBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("dd62cb5011f64cd38b8b08abb19ba2cc");
                var BloodragerBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
                var SecondBloodragerBloodline = Resources.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");
                SecondBloodragerBloodline.RemoveComponents<PrerequisiteFeature>();
                SecondBloodragerBloodline.AddComponent(Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                    ReformedFiendBloodlineSelection.ToReference<BlueprintFeatureReference>(),
                    BloodragerBloodlineSelection.ToReference<BlueprintFeatureReference>()
                };
                    c.Amount = 1;
                }));
            }
            static void PatchSecondMystery() {
                if (!ModSettings.Fixes.MythicAbilities.Enabled["SecondMystery"]) { return; }
                var SecondMystery = Resources.GetBlueprint<BlueprintFeatureSelection>("277b0164740b97945a3f8022bd572f48");
                SecondMystery.m_Features = SecondMystery.m_AllFeatures;
                SecondMystery.Group = FeatureGroup.None;
                Main.LogPatch("Patched", SecondMystery);
            }
            static void PatchSecondSpirit() {
                if (!ModSettings.Fixes.MythicAbilities.Enabled["SecondSpirit"]) { return; }
                var SecondSpirit = Resources.GetBlueprint<BlueprintFeatureSelection>("2faa80662a56ab644aec2f875a68597f");
                SecondSpirit.m_Features = SecondSpirit.m_AllFeatures;
                SecondSpirit.Group = FeatureGroup.None;
                Main.LogPatch("Patched", SecondSpirit);
            }
            static void PatchEverlastingJudgement() {
                if (!ModSettings.Fixes.MythicAbilities.Enabled["EverlastingJudgement"]) { return; }
                var EverlastingJudgement = Resources.GetBlueprint<BlueprintFeature>("4a6dc772c9a7fe742a65820007107f03");
                var JudgmentWatcherBuff = Resources.GetBlueprint<BlueprintBuff>("9b8bb2ce8f67e5b4fa634ed6a6671f7a");

                var judgmentAddfacts = JudgmentWatcherBuff.GetComponent<AddFactContextActions>();
                var condition = new Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional() {
                    Comment = "EverlastingJudgement",
                    ConditionsChecker = new ConditionsChecker {
                            Conditions = new Condition[] { new ContextConditionHasFact() {
                                m_Fact = EverlastingJudgement.ToReference<BlueprintUnitFactReference>()
                            }
                        }
                    },
                    IfTrue = new ActionList(),
                    IfFalse = judgmentAddfacts.Activated
                };
                judgmentAddfacts.Activated = new ActionList() { 
                    Actions = new GameAction[] { condition }
                };

                Main.LogPatch("Patched", JudgmentWatcherBuff);
            }
        }
        [HarmonyPatch(typeof(ItemEntity), "AddEnchantment")]
        static class ItemEntity_AddEnchantment_EnduringSpells_Patch {
            static BlueprintFeature EnduringSpells = Resources.GetBlueprint<BlueprintFeature>("2f206e6d292bdfb4d981e99dcf08153f");
            static BlueprintFeature EnduringSpellsGreater = Resources.GetBlueprint<BlueprintFeature>("13f9269b3b48ae94c896f0371ce5e23c");
            static bool Prefix(MechanicsContext parentContext, ref Rounds? duration, BlueprintItemEnchantment blueprint) {
                if (ModSettings.Fixes.MythicAbilities.DisableAll || !ModSettings.Fixes.MythicAbilities.Enabled["EnduringSpells"]) { return true; }
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
        [HarmonyPatch(typeof(AutoMetamagic), "ShouldApplyTo")]
        static class AutoMetamagic_ShouldApplyTo_DomainZealot_Patch {
            static bool Prefix(ref bool __result, AutoMetamagic c, BlueprintAbility ability, AbilityData data) {
                if (ModSettings.Fixes.MythicAbilities.DisableAll || !ModSettings.Fixes.MythicAbilities.Enabled["DomainZealot"]) { return true; }
                BlueprintAbility parentAbility = data?.ConvertedFrom?.Blueprint ?? ability?.Parent ?? ability;

                var test = (parentAbility != ability && AutoMetamagic.ShouldApplyTo(c, parentAbility, data?.ConvertedFrom))
                    || (c?.Abilities?.HasItem((BlueprintAbilityReference r) => r.Is(ability)) ?? false)
                    || c.IsSuitableAbility(ability, data)
                        && (c?.Abilities?.Empty() ?? true)
                        && (c.Descriptor == SpellDescriptor.None | ability.SpellDescriptor.HasAnyFlag(c.Descriptor))
                        && c.School != SpellSchool.None
                        && ability.School == c.School
                        && c.MaxSpellLevel > 0
                        && data != null
                        && data.SpellLevel <= c.MaxSpellLevel
                        && (!c.CheckSpellbook || ability.IsInSpellList(c.Spellbook.SpellList));

                __result = test;
                return false;
            }
        }
    }
}