using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Cavalier {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Cavalier_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Cavalier")) { return; }

                var CavalierSupremeCharge = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("77af3c58e71118d4481c50694bd99e77");
                var CavalierAlternateCapstone = NewContent.AlternateCapstones.Cavalier.CavalierAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                CavalierSupremeCharge.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.CavalierClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == CavalierSupremeCharge.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(CavalierAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(CavalierAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == CavalierSupremeCharge.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(CavalierAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Cavalier");

                PatchBase();
                PatchGendarme();
            }
            static void PatchBase() {
                PatchCavalierMobility();
                PatchCavalierMountSelection();
                PatchSupremeCharge();
                PatchOrderOfTheStar();

                void PatchCavalierMountSelection() {
                    if (TTTContext.Fixes.Cavalier.Base.IsDisabled("CavalierMountSelection")) { return; }

                    var CavalierMountSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
                    var AnimalCompanionEmptyCompanion = BlueprintTools.GetBlueprint<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
                    var AnimalCompanionFeatureHorse = BlueprintTools.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
                    var AnimalCompanionFeatureHorse_PreorderBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");
                    var CavalierMountFeatureWolf = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "CavalierMountFeatureWolf");

                    CavalierMountSelection.SetFeatures(
                        AnimalCompanionEmptyCompanion,
                        AnimalCompanionFeatureHorse,
                        AnimalCompanionFeatureHorse_PreorderBonus,
                        CavalierMountFeatureWolf
                    );
                    CavalierMountSelection.m_Features = CavalierMountSelection.m_AllFeatures;
                    TTTContext.Logger.LogPatch("Patched", CavalierMountSelection);
                }
                void PatchSupremeCharge() {
                    if (TTTContext.Fixes.Cavalier.Base.IsDisabled("SupremeCharge")) { return; }

                    var MountedBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                    var ChargeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                    var CavalierSupremeCharge = BlueprintTools.GetBlueprint<BlueprintFeature>("77af3c58e71118d4481c50694bd99e77");
                    var CavalierSupremeChargeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7e9c5be79cfb3d44586dd650c7c7d198");

                    CavalierSupremeCharge.RemoveComponents<BuffExtraEffects>();
                    CavalierSupremeCharge.AddComponent<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff };
                        c.ExtraEffectBuff = CavalierSupremeChargeBuff.ToReference<BlueprintBuffReference>();
                    });
                    CavalierSupremeChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    CavalierSupremeChargeBuff.AddComponent<AddOutgoingWeaponDamageBonus>(c => {
                        c.BonusDamageMultiplier = 1;
                        c.RemoveAfterTrigger = true;
                    });
                    CavalierSupremeChargeBuff.SetName(CavalierSupremeCharge.m_DisplayName);

                    TTTContext.Logger.LogPatch("Patched", CavalierSupremeCharge);
                    TTTContext.Logger.LogPatch("Patched", CavalierSupremeChargeBuff);
                }
                void PatchCavalierMobility() {
                    if (TTTContext.Fixes.Cavalier.Base.IsDisabled("CavalierMobility")) { return; }

                    var CavalierProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("aa70326bdaa7015438df585cf2ab93b9");
                    var CavalierMobilityFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "CavalierMobilityFeature");
                    var DiscipleOfThePikeArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("4c4c3f9df00a5e04680d172a290111c4");

                    CavalierProgression.LevelEntries.Where(l => l.Level == 1).First().m_Features.Add(CavalierMobilityFeature.ToReference<BlueprintFeatureBaseReference>());
                    DiscipleOfThePikeArchetype.RemoveFeatures.Where(l => l.Level == 1).First().m_Features.Add(CavalierMobilityFeature.ToReference<BlueprintFeatureBaseReference>());
                }
                void PatchOrderOfTheStar() {
                    PatchCalling();

                    void PatchCalling() {
                        if (TTTContext.Fixes.Cavalier.Base.IsDisabled("OrderOfTheStarCalling")) { return; }

                        var CavalierCallingBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6074c0e8bd593024c9866c7b99c6d826");
                        CavalierCallingBuff.TemporaryContext(bp => {
                            bp.RemoveComponents<ModifyD20>();
                            bp.AddComponent<BuffAllSkillsBonus>(c => {
                                c.Value = 1;
                                c.Multiplier = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                };
                                c.Descriptor = ModifierDescriptor.Competence;
                            });
                            bp.AddComponent<AddContextStatBonus>(c => {
                                c.Stat = StatType.AdditionalAttackBonus;
                                c.Descriptor = ModifierDescriptor.Competence;
                                c.Value = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                };
                            });
                            bp.AddComponent<AddInitiatorSkillRollTrigger>(c => {
                                c.Action = Helpers.CreateActionList(new ContextActionRemoveSelf());
                            });
                            bp.AddComponent<RemoveBuffOnAttack>();
                        });
                    }
                }
            }
            static void PatchGendarme() {
                PatchTransfixingCharge();

                void PatchTransfixingCharge() {
                    if (TTTContext.Fixes.Cavalier.Archetypes["Gendarme"].IsDisabled("TransfixingCharge")) { return; }

                    var MountedBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                    var ChargeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                    var GendarmeTransfixingCharge = BlueprintTools.GetBlueprint<BlueprintFeature>("72a0bde01943f824faa98bd55f04c06d");
                    var GendarmeTransfixingChargeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6334e70d212add149909a36340ef5300");

                    GendarmeTransfixingCharge.RemoveComponents<BuffExtraEffects>();
                    GendarmeTransfixingCharge.AddComponent<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff };
                        c.ExtraEffectBuff = GendarmeTransfixingChargeBuff.ToReference<BlueprintBuffReference>();
                    });
                    GendarmeTransfixingChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    GendarmeTransfixingChargeBuff.AddComponent<AddOutgoingWeaponDamageBonus>(c => {
                        c.BonusDamageMultiplier = 2;
                        c.RemoveAfterTrigger = true;
                    });
                    GendarmeTransfixingChargeBuff.SetName(GendarmeTransfixingCharge.m_DisplayName);

                    TTTContext.Logger.LogPatch("Patched", GendarmeTransfixingCharge);
                    TTTContext.Logger.LogPatch("Patched", GendarmeTransfixingChargeBuff);
                }
            }

        }
    }
}
