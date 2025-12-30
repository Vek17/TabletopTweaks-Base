using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Oracle {
        [PatchBlueprintsCacheInit]
        static class Oracle_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Oracle")) { return; }

                var OracleFinalRevelation = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("0336dc22538ba5f42b73da4fb3f50849");
                var OracleAlternateCapstone = NewContent.AlternateCapstones.Oracle.OracleAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();
                var DiverseMysteries = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DiverseMysteries");
                var DiverseMysteriesRevelationSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "DiverseMysteriesRevelationSelection");
                var OracleRevelationSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("60008a10ad7ad6543b1f63016741a5d2");
                var OracleMysterySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("5531b975dcdf0e24c98f1ff7e017e741");

                OracleFinalRevelation.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                foreach (var mystery in OracleMysterySelection.AllFeatures) {
                    var capstone = mystery.GetComponent<AddFeatureOnClassLevel>(c => c.Level == 20).m_Feature;
                    mystery.RemoveComponents<AddFeatureOnClassLevel>(c => c.Level == 20);
                    mystery.AddComponent<AddFeatureOnClassLevelToPlayers>(c => {
                        c.Not = true;
                        c.Level = 20;
                        c.m_Class = ClassTools.Classes.OracleClass.ToReference<BlueprintCharacterClassReference>();
                        c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
                        c.m_Archetypes = new BlueprintArchetypeReference[0];
                        c.m_Feature = capstone;
                    });
                    capstone.Get().AddPrerequisiteFeature(mystery);
                    capstone.Get().HideNotAvailibleInUI = true;
                    OracleRevelationSelection.AllFeatures.ForEach(revelation => {
                        var prerequisite = revelation.GetComponent<PrerequisiteFeaturesFromList>(c => c.m_Features.Any(f => f.deserializedGuid == mystery.AssetGuid));
                        if (prerequisite == null) { return; }
                        revelation.AddComponent<PrerequisiteOracleMystery>(c => {
                            c.m_BypassSelections = new BlueprintFeatureSelectionReference[] {
                                DiverseMysteriesRevelationSelection.ToReference<BlueprintFeatureSelectionReference>()
                            };
                            c.m_Features = prerequisite.m_Features.ToArray();
                            c.Amount = 1;
                        });
                        revelation.RemoveComponent(prerequisite);
                    });
                    DiverseMysteriesRevelationSelection.m_AllFeatures = OracleRevelationSelection.m_AllFeatures;
                    DiverseMysteriesRevelationSelection.m_Features = OracleRevelationSelection.m_Features;
                }
                ClassTools.Classes.OracleClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == OracleFinalRevelation.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(OracleAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(OracleAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == OracleFinalRevelation.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(OracleAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Oracle");
                PatchBase();
            }
            static void PatchBase() {
                PatchNaturesWhisper();
                PatchFlameMystery();
                PatchWavesMystery();

                void PatchNaturesWhisper() {
                    if (TTTContext.Fixes.Oracle.Base.IsDisabled("NaturesWhisperMonkStacking")) { return; }

                    var OracleRevelationNatureWhispers = BlueprintTools.GetBlueprint<BlueprintFeature>("3d2cd23869f0d98458169b88738f3c32");
                    var NaturesWhispersACConversion = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "NaturesWhispersACConversion");
                    var ScaledFistACBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("3929bfd1beeeed243970c9fc0cf333f8");

                    OracleRevelationNatureWhispers.TemporaryContext(bp => {
                        bp.RemoveComponents<ReplaceStatBaseAttribute>();
                        bp.RemoveComponents<ReplaceCMDDexterityStat>();
                        bp.RemoveComponents<AddFactContextActions>();
                        bp.AddComponent<HasFactFeatureUnlock>(c => {
                            c.m_CheckedFact = ScaledFistACBonus.ToReference<BlueprintUnitFactReference>();
                            c.m_Feature = NaturesWhispersACConversion.ToReference<BlueprintUnitFactReference>();
                            c.Not = true;
                        });
                    });

                    TTTContext.Logger.LogPatch("Patched", OracleRevelationNatureWhispers);
                }

                void PatchFlameMystery() {
                    PatchRevelationBurningMagic();

                    void PatchRevelationBurningMagic() {
                        if (TTTContext.Fixes.Oracle.Base.IsDisabled("RevelationBurningMagic")) { return; }

                        var OracleRevelationBurningMagic = BlueprintTools.GetBlueprint<BlueprintFeature>("125294de0a922c34db4cd58ca7a200ac");
                        var OracleRevelationBurningMagicBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4ae27ae7c3d758041b25e9a3aff73592");
                        OracleRevelationBurningMagic.TemporaryContext(bp => {
                            bp.SetDescription(TTTContext, "Whenever a creature fails a saving throw and takes fire damage from one of your spells, " +
                                "it catches on fire. This fire deals 1 point of fire damage per spell level " +
                                "at the beginning of the burning creature’s turn. The fire lasts for 1d4 rounds.");
                            bp.RemoveComponents<AddAbilityUseTrigger>();
                            bp.RemoveComponents<ContextCalculateSharedValue>();
                            bp.RemoveComponents<ContextRankConfig>();
                            bp.AddComponent<BurningMagic>(c => {
                                c.EnergyType = DamageEnergyType.Fire;
                                c.m_Buff = OracleRevelationBurningMagicBuff.ToReference<BlueprintBuffReference>();
                                c.Duration = new ContextDurationValue() {
                                    DiceType = DiceType.D4,
                                    DiceCountValue = 1,
                                    BonusValue = 0
                                };
                            });
                        });
                        OracleRevelationBurningMagicBuff.TemporaryContext(bp => {
                            bp.SetName(OracleRevelationBurningMagic.m_DisplayName);
                            bp.SetDescription(OracleRevelationBurningMagic.m_Description);
                            bp.Stacking = StackingType.Replace;
                            bp.Ranks = 1;
                            bp.RemoveComponents<AddFactContextActions>();
                            bp.AddComponent<AddFactContextActions>(c => {
                                c.Activated = Helpers.CreateActionList();
                                c.NewRound = Helpers.CreateActionList(
                                    new ContextActionDealDamageTTT() {
                                        DamageType = new DamageTypeDescription() {
                                            Type = DamageType.Energy,
                                            Energy = DamageEnergyType.Fire
                                        },
                                        Duration = new ContextDurationValue() {
                                            DiceCountValue = new ContextValue(),
                                            BonusValue = new ContextValue(),
                                        },
                                        Value = new ContextDiceValue() {
                                            DiceCountValue = 0,
                                            BonusValue = new ContextValue() {
                                                ValueType = ContextValueType.Rank
                                            }
                                        }
                                    }
                                );
                                c.Deactivated = Helpers.CreateActionList();
                            });
                            bp.GetComponent<ContextRankConfig>()?.TemporaryContext(c => {
                                c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                                c.m_Progression = ContextRankProgression.AsIs;
                            });
                        });
                        TTTContext.Logger.LogPatch(OracleRevelationBurningMagic);
                        TTTContext.Logger.LogPatch(OracleRevelationBurningMagicBuff);
                    }
                }
                void PatchWavesMystery() {
                    PatchRevelationFreezingSpells();

                    void PatchRevelationFreezingSpells() {
                        if (TTTContext.Fixes.Oracle.Base.IsDisabled("RevelationFreezingSpells")) { return; }

                        var OracleRevelationFreezingSpellsFeature1 = BlueprintTools.GetBlueprint<BlueprintFeature>("4fe07207483321e4cb7b81e2eaeb9cec");
                        var OracleRevelationFreezingSpellsFeature11 = BlueprintTools.GetBlueprint<BlueprintFeature>("75dc65756f4c51c40932ac2ffdf66b94");
                        var OracleRevelationFreezingSpellsBuff1 = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(TTTContext, "OracleRevelationFreezingSpellsBuff1");
                        var OracleRevelationFreezingSpellsBuff11 = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(TTTContext, "OracleRevelationFreezingSpellsBuff11");
                        OracleRevelationFreezingSpellsFeature1.TemporaryContext(bp => {
                            bp.RemoveComponents<AddAbilityUseTrigger>();
                            bp.RemoveComponents<ContextCalculateSharedValue>();
                            bp.RemoveComponents<ContextRankConfig>();
                            bp.AddComponent<BurningMagic>(c => {
                                c.EnergyType = DamageEnergyType.Cold;
                                c.m_Buff = OracleRevelationFreezingSpellsBuff1;
                                c.Duration = new ContextDurationValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                };
                            });
                        });
                        OracleRevelationFreezingSpellsFeature11.TemporaryContext(bp => {
                            bp.RemoveComponents<AddAbilityUseTrigger>();
                            bp.RemoveComponents<ContextCalculateSharedValue>();
                            bp.RemoveComponents<ContextRankConfig>();
                            bp.AddComponent<BurningMagic>(c => {
                                c.EnergyType = DamageEnergyType.Cold;
                                c.m_Buff = OracleRevelationFreezingSpellsBuff11;
                                c.Duration = new ContextDurationValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                };
                            });
                        });
                        TTTContext.Logger.LogPatch(OracleRevelationFreezingSpellsFeature1);
                        TTTContext.Logger.LogPatch(OracleRevelationFreezingSpellsFeature11);
                    }
                }
            }
            static void PatchArchetypes() {
            }
        }
    }
}
