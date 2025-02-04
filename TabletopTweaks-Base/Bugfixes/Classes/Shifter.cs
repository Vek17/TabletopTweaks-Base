using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Shifter {
        [PatchBlueprintsCacheInit]
        static class Paladin_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }

            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Shifter")) { return; }

                var FinalShifterAspectFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("5a155f5c3f834a319feab52dc66ee185");
                var ShifterAspectSelectionFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("121829d239124685b430f5263031bf83");
                var ShifterAlternateCapstone = NewContent.AlternateCapstones.Shifter.ShifterAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                FinalShifterAspectFeature.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.ShifterClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == FinalShifterAspectFeature.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(ShifterAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => {
                            entry.m_Features.RemoveAll(f => f.deserializedGuid == ShifterAspectSelectionFeature.deserializedGuid);
                            entry.m_Features.Add(ShifterAlternateCapstone);
                        });
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == FinalShifterAspectFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(ShifterAlternateCapstone));
                    });
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == ShifterAspectSelectionFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.RemoveAll(f => f.deserializedGuid == ShifterAspectSelectionFeature.deserializedGuid));
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
                TTTContext.Logger.LogHeader("Patching Shifter");
                PatchBase();
                PatchArchetypes();
            }
            static void PatchBase() {
                PatchDefensiveInstinct();
                PatchShifterClaws();

                void PatchDefensiveInstinct() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("FixMonkAcBonusNames")) { return; }

                    var ShifterACBonusUnlock = BlueprintTools.GetBlueprint<BlueprintFeature>("43295870ce5344ffa718d100b742438e");
                    var ShifterACBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("c07b95dcb8164fb6ad8847ff6df91ba3");
                    var ShifterACBonusBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("06cb4711975e4607a66ea6338cdb8c7d");
                    var ShifterACBonusHalfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c9caeee561bd461cbb48b2f911fc3d98");
                    var ShifterACBonusHalfBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("571be0c75b714afaace65af1e3b4862d");
                    var MonkACBonusBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f132c4c4279e4646a05de26635941bfe");
                    var MonkACBonusBuffUnarmored = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("d7ff7a9f1fe84e679f98b36e4bacd63c");

                    ShifterACBonus.TemporaryContext(bp => {
                        bp.GetComponent<AddFacts>()?.TemporaryContext(c => {
                            c.m_Facts = new BlueprintUnitFactReference[] {
                                MonkACBonusBuff.ToReference<BlueprintUnitFactReference>(),
                                MonkACBonusBuffUnarmored
                            };
                        });
                    });
                    ShifterACBonusHalfFeature.TemporaryContext(bp => {
                        bp.GetComponent<AddFacts>()?.TemporaryContext(c => {
                            c.m_Facts = new BlueprintUnitFactReference[] {
                                ShifterACBonusHalfBuff.ToReference<BlueprintUnitFactReference>(),
                                MonkACBonusBuffUnarmored
                            };
                        });
                    });
                    ShifterACBonusBuff.TemporaryContext(bp => {
                        bp.SetName(MonkACBonusBuff.m_DisplayName);
                        bp.SetDescription(TTTContext, "");
                        bp.IsClassFeature = true;
                        bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                        bp.SetComponents();
                        bp.AddComponent<AddContextStatBonus>(c => {
                            c.Stat = StatType.AC;
                            c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Wisdom;
                            c.Value = new ContextValue() {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.StatBonus
                            };
                            c.Multiplier = 1;
                        });
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = StatType.Wisdom;
                        });
                        bp.AddComponent<RecalculateOnFactsChange>();
                        bp.AddContextRankConfig(c => {
                            c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                            c.m_Type = AbilityRankType.StatBonus;
                            c.m_Stat = StatType.Wisdom;
                            c.m_Progression = ContextRankProgression.AsIs;
                            c.m_UseMin = true;
                            c.m_Min = 0;
                        });
                    });
                    ShifterACBonusHalfBuff.TemporaryContext(bp => {
                        bp.SetName(MonkACBonusBuff.m_DisplayName);
                        bp.SetComponents();
                        bp.AddComponent<AddContextStatBonus>(c => {
                            c.Stat = StatType.AC;
                            c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Wisdom;
                            c.Value = new ContextValue() {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.StatBonus
                            };
                            c.Multiplier = 1;
                        });
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = StatType.Wisdom;
                        });
                        bp.AddComponent<RecalculateOnFactsChange>();
                        bp.AddContextRankConfig(c => {
                            c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                            c.m_Type = AbilityRankType.StatBonus;
                            c.m_Stat = StatType.Wisdom;
                            c.m_Progression = ContextRankProgression.Div2;
                            c.m_UseMin = true;
                            c.m_Min = 0;
                        });
                    });

                    TTTContext.Logger.LogPatch(ShifterACBonus);
                    TTTContext.Logger.LogPatch(ShifterACBonusHalfBuff);
                }
                void PatchShifterClaws() {
                    if (TTTContext.Fixes.Shifter.Base.IsDisabled("ShifterClaws")) { return; }

                    var ShifterClawBuffLevel1 = BlueprintTools.GetBlueprint<BlueprintBuff>("02070af90de345c6a82a8cf469a65080");
                    var ShifterClawBuffLevel11 = BlueprintTools.GetBlueprint<BlueprintBuff>("13243d59d212463d9ab3f36e646aa40c");
                    var ShifterClawBuffLevel13 = BlueprintTools.GetBlueprint<BlueprintBuff>("6e31c78ce801444aad398248b66a22b8");
                    var ShifterClawBuffLevel17 = BlueprintTools.GetBlueprint<BlueprintBuff>("cb51194e75ca45bc9fedf9a09c50b827");
                    var ShifterClawBuffLevel19 = BlueprintTools.GetBlueprint<BlueprintBuff>("494d127890c3498fb3dbf3a53dcb4fe6");
                    var ShifterClawBuffLevel3 = BlueprintTools.GetBlueprint<BlueprintBuff>("1bb67316c37e400888e0489ee8d64067");
                    var ShifterClawBuffLevel7 = BlueprintTools.GetBlueprint<BlueprintBuff>("c9441167a3b84fb48729e55f29a9df64");

                    PatchOutgoingDamageProperties(ShifterClawBuffLevel1);
                    PatchOutgoingDamageProperties(ShifterClawBuffLevel3);
                    PatchOutgoingDamageProperties(ShifterClawBuffLevel7);
                    PatchOutgoingDamageProperties(ShifterClawBuffLevel11);
                    PatchOutgoingDamageProperties(ShifterClawBuffLevel13);
                    PatchOutgoingDamageProperties(ShifterClawBuffLevel17);
                    PatchOutgoingDamageProperties(ShifterClawBuffLevel19);

                    void PatchOutgoingDamageProperties(BlueprintBuff buff) {
                        buff.TemporaryContext(bp => {
                            bp.GetComponent<AddOutgoingPhysicalDamageProperty>()?.TemporaryContext(c => {
                                c.AffectAnyPhysicalDamage = true;
                            });
                        });
                    }
                }
            }

            static void PatchArchetypes() {
                PatchGriffonheartShifter();
                PatchWildEffigy();

                void PatchGriffonheartShifter() {
                    PatchPlayerAvailablilty();
                    PatchFighterTraining();

                    void PatchFighterTraining() {
                        if (TTTContext.Fixes.Shifter.Archetypes["GriffonheartShifter"].IsDisabled("FighterTraining")) { return; }

                        var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
                        QuickFixTools.ReplaceClassLevelsForPrerequisites(GriffonheartShifterGriffonShapeFakeFeature, TTTContext, FeatureGroup.Feat);

                        TTTContext.Logger.LogPatch(GriffonheartShifterGriffonShapeFakeFeature);
                    }
                    void PatchPlayerAvailablilty() {
                        if (TTTContext.Fixes.Shifter.Archetypes["GriffonheartShifter"].IsDisabled("PlayerUsable")) { return; }

                        var GriffonheartShifterArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("aed5b306ad734a6da5d5638edcb667c9");
                        GriffonheartShifterArchetype.m_HiddenInUI = false;

                        TTTContext.Logger.LogPatch(GriffonheartShifterArchetype);
                    }
                }

                void PatchWildEffigy() {
                    PatchArmorPlating();
                    PatchHeartOfEarth();
                    PatchStoneclawStrike();

                    void PatchArmorPlating() {
                        if (TTTContext.Fixes.Shifter.Archetypes["WildEffigy"].IsDisabled("ArmorPlating")) { return; }

                        var ArmorPlatingShifterFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0b09ce94b5fe48dfa81b9a6f9d55a351");
                        var HeartOfEarthShifterBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("c3ad4aca75764e82ab047533a8d5aea6");

                        var DragonbloodShifterBlackBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ace35704bf744216b142adcbd5c58d13");
                        var DragonbloodShifterBlueBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("627c3f3256494000b3cba6f461b2c44c");
                        var DragonbloodShifterBrassBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ed9ac87aaac7419095395304c7b5d813");
                        var DragonbloodShifterBronzeBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("233617f6876a42ef973de8af502d4fed");
                        var DragonbloodShifterCopperBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("566474477be14eaeb860086b82e5e9cf");
                        var DragonbloodShifterGoldBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e36d67f4b25a4d5ebf9add1e2bc52e74");
                        var DragonbloodShifterGreenBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e850db860994442b83e964d3a43e9358");
                        var DragonbloodShifterRedBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("fb71be7bcbc648539d57171b0d0baf79");
                        var DragonbloodShifterSilverBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d9de1c4f6e68416196c193ac87962993");
                        var DragonbloodShifterWhiteBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("cd34e74f0d2844e3ab5580c1dbe3ff7d");

                        var ShifterDragonFormBlackBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8fb6bf56c9174d5e8cf24069e6b0c965");
                        var ShifterDragonFormBlackBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("662bdcd3eef541fb91d88b9ee79d0d37");
                        var ShifterDragonFormBlueBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b9c75c14fe6d48e0962e1ce9f42d4c9e");
                        var ShifterDragonFormBlueBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("abaa7d56f843410e97c61ff2c87d39c6");
                        var ShifterDragonFormBrassBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("445d70781c2848dc9c63d80718a6c26f");
                        var ShifterDragonFormBrassBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("205e7bae0d7c428b8f2a451f7934219a");
                        var ShifterDragonFormBronzeBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0ff1819f465140068e02aaf87c17ec2c");
                        var ShifterDragonFormBronzeBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("cd72e19154f143269e48caff753eab63");
                        var ShifterDragonFormCopperBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e9736d47de3643009a5514668a48ffe0");
                        var ShifterDragonFormCopperBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3f5625345d0c481abec69c0241d50019");
                        var ShifterDragonFormGoldBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5a679cd137d64c629995c626616dbb17");
                        var ShifterDragonFormGoldBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f5ac253cbee44744a7399f17765160d5");
                        var ShifterDragonFormGreenBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3d887a79a7384149bd38b4d9d97c44b5");
                        var ShifterDragonFormGreenBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ab25d91564a04b3fa0ae84d52b6407d5");
                        var ShifterDragonFormRedBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8242311f5c3c4cad90e67ef79cf5a6c2");
                        var ShifterDragonFormRedBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a1f0de0190ce40e19d97c6967a9693c3");
                        var ShifterDragonFormSilverBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("2de04456ce2d4e79804f899498ab31cc");
                        var ShifterDragonFormSilverBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3c4bf82676d345dca2718cac680f5906");
                        var ShifterDragonFormWhiteBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b9b1fbf0ec224ccfac3dc5451d00a26a");
                        var ShifterDragonFormWhiteBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8b82ee0ca203452a952a25c0f867b2fe");

                        var ShifterAspectManticoreBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5a21b25be7bb43e0b94646511afadfc6");
                        var ShifterWildShapeManticoreBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("91f12a442b374bd7bfdfb05f5ab80f4c");
                        var ShifterWildShapeManticoreBuff8 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("28861899db294aa593ada213a8d1fd36");
                        var ShifterWildShapeManticoreBuff15 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f5331d1b15ac4c4a833e2928ce3bf18d");

                        var ShifterAspectFeyBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("727381f8a2ee46a19ab96eedfe1d9daa");
                        var ShifterWildShapeFeyBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("477259fe81a647ad9a38b47140e38de6");
                        var ShifterWildShapeFeyBuff8 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6fc7df0ddb9a466d976a3808a8f1437a");
                        var ShifterWildShapeFeyBuff15 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("dee544177d0148edbaa7ca6a0aee03c0");

                        var ShifterAspectGriffonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7440316282184da7b909c8ee8e737730");
                        var ShifterWildShapeGriffonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e76d475eb1f1470e9950a5fee99ddb40");
                        var ShifterWildShapeGriffonBuff_Cutscene = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("af4af06317b94a39bd3fd80ebde86070");
                        var ShifterWildShapeGriffonBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3a7511f1b8a94b11bbb21245e150c0b6");
                        var ShifterWildShapeGriffonBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("821fa7f586ca44238a0894115824035c");

                        var ShifterWildShapeGriffonDemonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d1153efaf92f46499e9f9a8613a7274c");
                        var ShifterWildShapeGriffonDemonBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7d4798e5fe5f4a349b56686340008824");
                        var ShifterWildShapeGriffonDemonBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("431ca9188d6f401f9f8df8079c526e59");

                        var ShifterWildShapeGriffonGodBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4b95ed9a351e4effbb2a83e246ee6334");
                        var ShifterWildShapeGriffonGodBuff_Cutscene = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("78ee750ff66c47dc9cbf82d59013ae8f");
                        var ShifterWildShapeGriffonGodBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d8b979bf19554b85bbed05e6369c0f63");
                        var ShifterWildShapeGriffonGodBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("10c913c645364bafbde759f83d103ce6");

                        ArmorPlatingShifterFeature.TemporaryContext(bp => {
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlackBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlackBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlueBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlueBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBrassBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBrassBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBronzeBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBronzeBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormCopperBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormCopperBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGoldBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGoldBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGreenBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGreenBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormRedBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormRedBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormSilverBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormSilverBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormWhiteBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormWhiteBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBlackBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBlueBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBrassBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBronzeBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterCopperBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterGoldBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterGreenBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterRedBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterSilverBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterWhiteBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterAspectManticoreBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeManticoreBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeManticoreBuff8;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeManticoreBuff15;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterAspectFeyBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeFeyBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeFeyBuff8;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeFeyBuff15;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterAspectGriffonBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff_Cutscene;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonDemonBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonDemonBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonDemonBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff_Cutscene;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff9;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff14;
                                c.m_ExtraEffectBuff = HeartOfEarthShifterBuff;
                            });
                        });

                        TTTContext.Logger.LogPatch(ArmorPlatingShifterFeature);
                    }
                    void PatchHeartOfEarth() {
                        if (TTTContext.Fixes.Shifter.Archetypes["WildEffigy"].IsDisabled("HeartOfEarth")) { return; }

                        var ArmorPlatingShifterFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("76c244c29fac4efba240c7a942468ff5");
                        var ArmorPlatingShifterBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b00fbc84b7ae4a45883c0ac8bb070db6");

                        var DragonbloodShifterBlackBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ace35704bf744216b142adcbd5c58d13");
                        var DragonbloodShifterBlueBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("627c3f3256494000b3cba6f461b2c44c");
                        var DragonbloodShifterBrassBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ed9ac87aaac7419095395304c7b5d813");
                        var DragonbloodShifterBronzeBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("233617f6876a42ef973de8af502d4fed");
                        var DragonbloodShifterCopperBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("566474477be14eaeb860086b82e5e9cf");
                        var DragonbloodShifterGoldBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e36d67f4b25a4d5ebf9add1e2bc52e74");
                        var DragonbloodShifterGreenBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e850db860994442b83e964d3a43e9358");
                        var DragonbloodShifterRedBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("fb71be7bcbc648539d57171b0d0baf79");
                        var DragonbloodShifterSilverBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d9de1c4f6e68416196c193ac87962993");
                        var DragonbloodShifterWhiteBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("cd34e74f0d2844e3ab5580c1dbe3ff7d");

                        var ShifterDragonFormBlackBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8fb6bf56c9174d5e8cf24069e6b0c965");
                        var ShifterDragonFormBlackBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("662bdcd3eef541fb91d88b9ee79d0d37");
                        var ShifterDragonFormBlueBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b9c75c14fe6d48e0962e1ce9f42d4c9e");
                        var ShifterDragonFormBlueBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("abaa7d56f843410e97c61ff2c87d39c6");
                        var ShifterDragonFormBrassBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("445d70781c2848dc9c63d80718a6c26f");
                        var ShifterDragonFormBrassBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("205e7bae0d7c428b8f2a451f7934219a");
                        var ShifterDragonFormBronzeBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0ff1819f465140068e02aaf87c17ec2c");
                        var ShifterDragonFormBronzeBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("cd72e19154f143269e48caff753eab63");
                        var ShifterDragonFormCopperBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e9736d47de3643009a5514668a48ffe0");
                        var ShifterDragonFormCopperBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3f5625345d0c481abec69c0241d50019");
                        var ShifterDragonFormGoldBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5a679cd137d64c629995c626616dbb17");
                        var ShifterDragonFormGoldBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f5ac253cbee44744a7399f17765160d5");
                        var ShifterDragonFormGreenBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3d887a79a7384149bd38b4d9d97c44b5");
                        var ShifterDragonFormGreenBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ab25d91564a04b3fa0ae84d52b6407d5");
                        var ShifterDragonFormRedBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8242311f5c3c4cad90e67ef79cf5a6c2");
                        var ShifterDragonFormRedBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a1f0de0190ce40e19d97c6967a9693c3");
                        var ShifterDragonFormSilverBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("2de04456ce2d4e79804f899498ab31cc");
                        var ShifterDragonFormSilverBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3c4bf82676d345dca2718cac680f5906");
                        var ShifterDragonFormWhiteBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b9b1fbf0ec224ccfac3dc5451d00a26a");
                        var ShifterDragonFormWhiteBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8b82ee0ca203452a952a25c0f867b2fe");

                        var ShifterAspectManticoreBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5a21b25be7bb43e0b94646511afadfc6");
                        var ShifterWildShapeManticoreBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("91f12a442b374bd7bfdfb05f5ab80f4c");
                        var ShifterWildShapeManticoreBuff8 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("28861899db294aa593ada213a8d1fd36");
                        var ShifterWildShapeManticoreBuff15 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f5331d1b15ac4c4a833e2928ce3bf18d");

                        var ShifterAspectFeyBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("727381f8a2ee46a19ab96eedfe1d9daa");
                        var ShifterWildShapeFeyBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("477259fe81a647ad9a38b47140e38de6");
                        var ShifterWildShapeFeyBuff8 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6fc7df0ddb9a466d976a3808a8f1437a");
                        var ShifterWildShapeFeyBuff15 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("dee544177d0148edbaa7ca6a0aee03c0");

                        var ShifterAspectGriffonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7440316282184da7b909c8ee8e737730");
                        var ShifterWildShapeGriffonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e76d475eb1f1470e9950a5fee99ddb40");
                        var ShifterWildShapeGriffonBuff_Cutscene = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("af4af06317b94a39bd3fd80ebde86070");
                        var ShifterWildShapeGriffonBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3a7511f1b8a94b11bbb21245e150c0b6");
                        var ShifterWildShapeGriffonBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("821fa7f586ca44238a0894115824035c");

                        var ShifterWildShapeGriffonDemonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d1153efaf92f46499e9f9a8613a7274c");
                        var ShifterWildShapeGriffonDemonBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("7d4798e5fe5f4a349b56686340008824");
                        var ShifterWildShapeGriffonDemonBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("431ca9188d6f401f9f8df8079c526e59");

                        var ShifterWildShapeGriffonGodBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4b95ed9a351e4effbb2a83e246ee6334");
                        var ShifterWildShapeGriffonGodBuff_Cutscene = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("78ee750ff66c47dc9cbf82d59013ae8f");
                        var ShifterWildShapeGriffonGodBuff9 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d8b979bf19554b85bbed05e6369c0f63");
                        var ShifterWildShapeGriffonGodBuff14 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("10c913c645364bafbde759f83d103ce6");

                        ArmorPlatingShifterFeature.TemporaryContext(bp => {
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlackBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlackBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlueBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBlueBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBrassBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBrassBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBronzeBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormBronzeBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormCopperBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormCopperBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGoldBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGoldBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGreenBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormGreenBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormRedBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormRedBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormSilverBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormSilverBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormWhiteBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterDragonFormWhiteBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBlackBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBlueBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBrassBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterBronzeBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterCopperBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterGoldBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterGreenBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterRedBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterSilverBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = DragonbloodShifterWhiteBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterAspectManticoreBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeManticoreBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeManticoreBuff8;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeManticoreBuff15;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterAspectFeyBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeFeyBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeFeyBuff8;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeFeyBuff15;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterAspectGriffonBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff_Cutscene;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonDemonBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonDemonBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonDemonBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff_Cutscene;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff9;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = ShifterWildShapeGriffonGodBuff14;
                                c.m_ExtraEffectBuff = ArmorPlatingShifterBuff;
                            });
                        });

                        TTTContext.Logger.LogPatch(ArmorPlatingShifterFeature);
                    }
                    void PatchStoneclawStrike() {
                        if (TTTContext.Fixes.Shifter.Archetypes["WildEffigy"].IsDisabled("StoneclawStrike")) { return; }

                        var StoneclawStrikeShifterBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("eadc5e452c384c25b67216f6a49a7f13");
                        var StoneclawStrikeShifterBuffEffect = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("86d20a42fa8843a8a55ae0af68f2bc6b");

                        var GrowingSpikesBuffLevel1 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b0fdcd99db4042a984ae56bc9683ebb1");
                        var GrowingSpikesBuffLevel3 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0f5f887cbf4d4ec08c948b3d1dcea15a");
                        var GrowingSpikesBuffLevel7 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("dddcb3c2ca9f455ea85332fdbf4ad28c");
                        var GrowingSpikesBuffLevel11 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6d7e0abeb85948c4bec6bd02a4b4eedf");
                        var GrowingSpikesBuffLevel13 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("93d0c6d102e44afb81367ce49b9522b8");
                        var GrowingSpikesBuffLevel17 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0a9eb20620684af98fa7581ba37305ac");
                        var GrowingSpikesBuffLevel19 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5b8feaf01cb244f98e9b2e335e0d6b26");

                        StoneclawStrikeShifterBuffEffect.Get().TemporaryContext(bp => {
                            bp.GetComponent<PartialDRIgnore>().m_WeaponCategories = new WeaponCategory[] {
                                WeaponCategory.Claw,
                                WeaponCategory.Bite,
                                WeaponCategory.Gore,
                                WeaponCategory.Slam,
                                WeaponCategory.Hoof,
                                WeaponCategory.Spike,
                                WeaponCategory.Wing,
                                WeaponCategory.Tail,
                                WeaponCategory.Talon,
                                WeaponCategory.Tentacle,
                                WeaponCategory.Sting,
                            };
                        });
                        StoneclawStrikeShifterBuff.TemporaryContext(bp => {

                        });
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PolymorphDamageTransfer), nameof(PolymorphDamageTransfer.TransferPhysicalProperties))]
        static class PolymorphDamageTransfer_TransferPhysicalProperties_Patch {
            static bool Prefix(PolymorphDamageTransfer __instance, RulePrepareDamage evt) {
                if (TTTContext.Fixes.Shifter.Base.IsDisabled("ShifterClaws")) { return true; }

                AddOutgoingPhysicalDamageProperty component = __instance.Fact.Blueprint.GetComponent<AddOutgoingPhysicalDamageProperty>();
                if (component == null) {
                    return false;
                }
                evt.DamageBundle.OfType<PhysicalDamage>().ForEach(damage => {
                    component.ApplyProperties(damage);
                });
                return false;
            }
        }
    }
}
