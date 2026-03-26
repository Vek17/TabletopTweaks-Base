using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    class MythicAbilities {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Mythic Abilities");
                PatchBestJokes();
                PatchBloodlineAscendance();
                PatchSecondBloodline();
                PatchBloodragerSecondBloodline();
                PatchExposeVulnerability();
                PatchCloseToTheAbyss();
            }
            static void PatchBestJokes() {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("BestJokes")) { return; }

                var HideousLaughter = BlueprintTools.GetBlueprint<BlueprintAbility>("fd4d9fd7f87575d47aafe2a64a6e2d8d");
                var HideousLaughterTiefling = BlueprintTools.GetBlueprint<BlueprintAbility>("ae9e3a143e40f20419aa2b1ec92e2e06");
                var BestJokes = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("ec739ff2292290f43b20689ff32de112");
                var HideousLaughterBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("4b1f07a71a982824988d7f48cd49f3f8");

                PatchHideouLaughter(HideousLaughter);
                PatchHideouLaughter(HideousLaughterTiefling);

                void PatchHideouLaughter(BlueprintAbility spell) {
                    spell.FlattenAllActions().OfType<Conditional>()
                        .Where(conditional => conditional.ConditionsChecker.Conditions
                            .OfType<ContextConditionCasterHasFact>()
                            .Any(c => c.m_Fact.Guid == BestJokes.Guid))
                        .ForEach(conditional => {
                            conditional.IfTrue = Helpers.CreateActionList(
                                new ContextDuplicateCastSpellOnNewTarget() {
                                    SameFaction = true,
                                    NumberOfTargets = 1,
                                    Radius = 30.Feet(),
                                    m_FilterNoFact = HideousLaughterBuff
                                }
                            );
                        });
                    TTTContext.Logger.LogPatch(spell);
                }
            }
            static void PatchCloseToTheAbyss() {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("CloseToTheAbyss")) { return; }

                var MythicDemonGore = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("bd4417c15511afe42850fb4d3a6b4a32");
                var TwoHandedDamageMultiplierEnchantment = BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TwoHandedDamageMultiplierEnchantment");

                MythicDemonGore.m_Enchantments = MythicDemonGore.m_Enchantments
                    .AppendToArray(TwoHandedDamageMultiplierEnchantment.ToReference<BlueprintWeaponEnchantmentReference>());
                TTTContext.Logger.LogPatch("Patched", MythicDemonGore);
            }
            static void PatchBloodlineAscendance() {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("BloodlineAscendance")) { return; }

                var BloodlineAscendance = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
                var SeekerBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");

                SeekerBloodlineSelection.m_Features.ForEach(bloodline => {
                    var capstone = ((BlueprintProgression)bloodline.Get()).LevelEntries.Where(entry => entry.Level == 20)
                        .SelectMany(entry => entry.Features.Select(f => f))
                        .Where(f => f.GetComponent<Prerequisite>())
                        .OfType<BlueprintFeature>()
                        .First();
                    capstone.GetComponents<Prerequisite>().ForEach(c => c.Group = Prerequisite.GroupType.Any);
                    if (!capstone.GetComponents<PrerequisiteFeature>().Any(c => c.m_Feature.Get() == bloodline.Get())) {
                        capstone.AddPrerequisiteFeature(bloodline, Prerequisite.GroupType.Any);
                    }
                });
                BloodlineAscendance.RemoveComponents<PrerequisiteFeature>();
                BloodlineAscendance.AddPrerequisites(Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                        BlueprintTools.GetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                        BlueprintTools.GetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                        BlueprintTools.GetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                        BlueprintTools.GetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                        BlueprintTools.GetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>(),    // EmpyrealBloodlineProgression
                        BlueprintTools.GetBlueprint<BlueprintFeature>("da48f9d7f697ae44ca891bfc50727988").ToReference<BlueprintFeatureReference>(),    // BloodOfDragonsSelection - Dragon Disciple
                        BlueprintTools.GetBlueprint<BlueprintFeature>("7c813fb495d74246918a690ba86f9c86").ToReference<BlueprintFeatureReference>(),    // NineTailedHeirBloodlineSelection
                        BlueprintTools.GetBlueprint<BlueprintFeature>("94c29f69cdc34594a6a4677441ed7375").ToReference<BlueprintFeatureReference>()     // EldritchScionBloodlineSelection
                    };
                    c.Amount = 1;
                }));
                TTTContext.Logger.LogPatch("Patched", BloodlineAscendance);
            }
            static void PatchSecondBloodline() {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("SecondBloodline")) { return; }

                var SecondBloodline = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");

                var SorcererBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914");
                var SeekerBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc");
                var SylvanBloodlineProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece");
                var SageBloodlineProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2");
                var EmpyrealBloodlineProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1");
                var BloodOfDragonsSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("da48f9d7f697ae44ca891bfc50727988");
                var NineTailedHeirBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("7c813fb495d74246918a690ba86f9c86");
                var EldritchScionBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("94c29f69cdc34594a6a4677441ed7375");

                SecondBloodline.TemporaryContext(bp => {
                    bp.IgnorePrerequisites = true;
                    bp.Mode = SelectionMode.OnlyNew;
                    bp.RemoveComponents<PrerequisiteFeature>();
                    bp.AddPrerequisiteFeaturesFromList(1,
                        SorcererBloodlineSelection,
                        SeekerBloodlineSelection,
                        SylvanBloodlineProgression,
                        SageBloodlineProgression,
                        EmpyrealBloodlineProgression,
                        BloodOfDragonsSelection,
                        NineTailedHeirBloodlineSelection,
                        EldritchScionBloodlineSelection
                    );
                });

                TTTContext.Logger.LogPatch("Patched", SecondBloodline);
            }
            static void PatchBloodragerSecondBloodline() {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("SecondBloodragerBloodline")) { return; }

                var ReformedFiendBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("dd62cb5011f64cd38b8b08abb19ba2cc");
                var BloodragerBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
                var SecondBloodragerBloodline = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");
                var SecondBloodragerBloodlineReformedFiend = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("5e4089c46a9f47cdadac7b19d69d11e1");

                SecondBloodragerBloodline.TemporaryContext(bp => {
                    bp.IgnorePrerequisites = true;
                    bp.Mode = SelectionMode.OnlyNew;
                    bp.RemoveComponents<PrerequisiteFeature>();
                    bp.AddPrerequisiteFeaturesFromList(1, ReformedFiendBloodlineSelection, BloodragerBloodlineSelection);
                });

                FeatTools.Selections.MythicAbilitySelection.RemoveFeatures(SecondBloodragerBloodlineReformedFiend);
            }
            static void PatchExposeVulnerability() {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("ExposeVulnerability")) { return; }

                var ExposeVulnerability = BlueprintTools.GetBlueprint<BlueprintFeature>("8ce3c4b3c1ad24f4dbb6cb4c72e1ec53");
                var ExposeVulnerabilityBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4edf0af9fd0ebb94ba5ef08b38768e06");

                ExposeVulnerability.FlattenAllActions()
                    .OfType<Conditional>()
                    .ForEach(c => {
                        c.IfTrue = Helpers.CreateActionList(
                            new ContextActionDealDamageTTT() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Divine
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank,
                                    },
                                    BonusValue = new ContextValue()
                                },
                                IgnoreWeapon = true,
                                IgnoreCritical = true
                            },
                            new ContextActionRemoveBuff() {
                                m_Buff = ExposeVulnerabilityBuff
                            },
                            new ContextActionRemoveBuff() {
                                m_Buff = ExposeVulnerabilityBuff
                            }
                        );
                    });
                TTTContext.Logger.LogPatch(ExposeVulnerability);
            }
        }
        [HarmonyPatch(typeof(ItemEntity), "AddEnchantment")]
        static class ItemEntity_AddEnchantment_EnduringSpells_Patch {
            private static readonly BlueprintFeature EnduringSpells = BlueprintTools.GetBlueprint<BlueprintFeature>("2f206e6d292bdfb4d981e99dcf08153f");
            private static readonly BlueprintFeature EnduringSpellsGreater = BlueprintTools.GetBlueprint<BlueprintFeature>("13f9269b3b48ae94c896f0371ce5e23c");

            static bool Prefix(MechanicsContext parentContext, ref Rounds? duration, BlueprintItemEnchantment blueprint) {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("EnduringSpells")) { return true; }

                if (parentContext != null && parentContext.MaybeOwner != null && duration != null) {
                    var abilityData = parentContext.SourceAbilityContext?.Ability;
                    if (abilityData == null || abilityData.Spellbook == null || abilityData.SourceItem != null) { return true; }
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
        [HarmonyPatch(typeof(AscendantElement), "OnEventAboutToTrigger")]
        static class AscendantElement_OnEventAboutToTrigger_Patch {

            static bool Prefix(AscendantElement __instance, RuleCalculateDamage evt) {
                if (Main.TTTContext.Fixes.MythicAbilities.IsDisabled("AscendantElement")) { return true; }

                foreach (BaseDamage baseDamage in evt.DamageBundle) {
                    EnergyDamage energyDamage;
                    if ((energyDamage = (baseDamage as EnergyDamage)) != null && energyDamage.EnergyType == __instance.Element) {
                        baseDamage.AddDecline(new DamageDecline(DamageDeclineType.None, __instance.Fact));
                        energyDamage.IgnoreReduction = true;
                        energyDamage.IgnoreImmunities = true;
                    }
                }
                return false;
            }
        }
    }
}