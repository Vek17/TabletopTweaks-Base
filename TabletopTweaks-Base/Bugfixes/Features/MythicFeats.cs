using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Base.Bugfixes.Features {
    static class MythicFeats {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Mythic Feats");
                PatchExpandedArsenal();
                PatchExtraMythicAbility();
                PatchExtraFeat();
                PatchMythicLightArmorFocus();
                PatchMythicMediumArmorFocus();
                PatchMythicHeavyArmorFocus();
            }
            static void PatchExpandedArsenal() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("ExpandedArsenal")) { return; }
                var ExpandedArsenalSchool = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("f137089c48364014aa3ec3b92ccaf2e2");
                var SpellFocus = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
                var SpellFocusGreater = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf");
                var SchoolMasteryMythicFeat = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("ac830015569352b458efcdfae00a948c");

                SpellFocus.GetComponent<SpellFocusParametrized>().Descriptor = (ModifierDescriptor)Untyped.SpellFocus;
                SpellFocusGreater.GetComponent<SpellFocusParametrized>().Descriptor = (ModifierDescriptor)Untyped.SpellFocusGreater;
                SchoolMasteryMythicFeat.TemporaryContext(bp => {
                    bp.RemoveComponents<SchoolMasteryParametrized>();
                    bp.AddComponent<BonusCasterLevelParametrized>(c => {
                        c.Bonus = 1;
                        c.Descriptor = (ModifierDescriptor)Untyped.SchoolMastery;
                    });
                });

                TTTContext.Logger.LogPatch(SpellFocus);
                TTTContext.Logger.LogPatch(SpellFocusGreater);
                TTTContext.Logger.LogPatch(SchoolMasteryMythicFeat);
                TTTContext.Logger.LogPatch(ExpandedArsenalSchool);
            }
            static void PatchExtraMythicAbility() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("ExtraMythicAbility")) { return; }
                FeatTools.Selections.ExtraMythicAbilityMythicFeat
                    .AddPrerequisite<PrerequisiteNoFeature>(p => {
                        p.m_Feature = FeatTools.Selections.ExtraMythicAbilityMythicFeat.ToReference<BlueprintFeatureReference>();
                    }
                );
            }
            static void PatchExtraFeat() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("ExtraFeat")) { return; }
                FeatTools.Selections.ExtraFeatMythicFeat
                    .AddPrerequisite<PrerequisiteNoFeature>(p => {
                        p.m_Feature = FeatTools.Selections.ExtraFeatMythicFeat.ToReference<BlueprintFeatureReference>();
                    }
                );
            }
            static void PatchMythicLightArmorFocus() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("MythicArmorFocus")) { return; }

                var ArmorFocusLightMythicOffenceFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("48168449f8ba465fab2843ba2dada063");
                var ArmorFocusLightMythicOffenceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b4583316ff014878809e57fc24d19229");
                var ArmorFocusLightMythicOffenceSubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("dc9702301ee7464d99c95e847e4d94f6");

                var ArmorFocusLightMythicFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("40be409a44084b2998eda225dd9c544a");
                var ArmorFocusLightMythicBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b1217085b7004b11a719ea075fba8ea7");
                var ArmorFocusLightMythicSubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("18a94772f0634cb1bd73ca41b86b8354");

                var ArmorFocusLightMythicFeatureVar2 = BlueprintTools.GetBlueprint<BlueprintFeature>("3260cacb9fa64d3bba6f179cfb026abd");
                var ArmorFocusLightMythicVar2Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("7c483825698f45bd8c1c1a6a9054e963");
                var ArmorFocusLightMythicVar2SubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("80bd5c6d5e0a483793a9ddff9ff42a95");
                var ArmorFocusLightMythicVar2BuffExtra = BlueprintTools.GetBlueprint<BlueprintBuff>("f0079e9247e54be3a13d69f6e57cfc20");

                PatchAssault();
                PatchAvoidance();
                PatchEndurance();

                void PatchAssault() {
                    ArmorFocusLightMythicOffenceFeature.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Light Armor — Assault");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusLightMythicOffenceSubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                        });
                    });
                    ArmorFocusLightMythicOffenceBuff.SetComponents();
                    ArmorFocusLightMythicOffenceSubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusLightMythicOffenceFeature.m_DisplayName);
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        });
                    });
                }
                void PatchAvoidance() {
                    ArmorFocusLightMythicFeature.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Light Armor — Avoidance");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusLightMythicSubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                        });
                    });
                    ArmorFocusLightMythicBuff.SetComponents();
                    ArmorFocusLightMythicSubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusLightMythicFeature.m_DisplayName);
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        });
                    });
                }
                void PatchEndurance() {
                    ArmorFocusLightMythicFeatureVar2.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Light Armor — Endurance");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusLightMythicVar2SubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                        });
                    });
                    ArmorFocusLightMythicVar2Buff.SetComponents();
                    ArmorFocusLightMythicVar2BuffExtra.SetComponents();
                    ArmorFocusLightMythicVar2SubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusLightMythicFeatureVar2.m_DisplayName);
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        });
                    });
                }

            }
            static void PatchMythicMediumArmorFocus() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("MythicArmorFocus")) { return; }

                var ArmorFocusMediumMythicFeatureOffence = BlueprintTools.GetBlueprint<BlueprintFeature>("6c6b8ba0ad1141df9316ab0ee28cbb77");
                var ArmorFocusMediumMythicFeatureOffenceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a9446282d567471496566a3b464559dc");
                var ArmorFocusMediumMythicFeatureOffenceSubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("8ddbd82754ac44c1990c212a3ed1f1c8");

                var ArmorFocusMediumMythicFeatureVar1 = BlueprintTools.GetBlueprint<BlueprintFeature>("b152e7fdfc6a452c8c47dd76de83e1aa");
                var ArmorFocusMediumMythicFeatureVar1Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("262ffe91ee5749c19005657c6aa5e818");
                var ArmorFocusMediumMythicFeatureVar1SubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("61c15df2702845589fec0b77fda158fe");

                var ArmorFocusMediumMythicFeatureVar2 = BlueprintTools.GetBlueprint<BlueprintFeature>("4243c96836bf4b54aa4b36e1ab0c7fb1");
                var ArmorFocusMediumMythicFeatureVar2Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("93e80467a5fc4e68927b99732484fbd4");
                var ArmorFocusMediumMythicFeatureVar2SubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("457d5cde294c435ea7b4ee66f4949956");

                PatchAssault();
                PatchAvoidance();
                PatchEndurance();

                void PatchAssault() {
                    ArmorFocusMediumMythicFeatureOffence.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Medium Armor — Assault");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusMediumMythicFeatureOffenceSubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                        });
                    });
                    ArmorFocusMediumMythicFeatureOffenceBuff.SetComponents();
                    ArmorFocusMediumMythicFeatureOffenceSubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusMediumMythicFeatureOffence.m_DisplayName);
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        });
                    });
                }
                void PatchAvoidance() {
                    ArmorFocusMediumMythicFeatureVar1.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Medium Armor — Avoidance");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusMediumMythicFeatureVar1SubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                        });
                    });
                    ArmorFocusMediumMythicFeatureVar1Buff.SetComponents();
                    ArmorFocusMediumMythicFeatureVar1SubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusMediumMythicFeatureVar1.m_DisplayName);
                        bp.GetComponent<MaxDexBonusIncrease>().TemporaryContext(c => {
                            c.CheckCategory = false;
                        });
                    });
                }
                void PatchEndurance() {
                    ArmorFocusMediumMythicFeatureVar2.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Medium Armor — Endurance");
                        //bp.SetDescription(TTTContext, "While wearing medium armor, you gain an equipped armor bonus to AC equal to half your armor's base AC and Enhancement bonus. " +
                        //    "Medium armor also no longer reduces your speed");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<MythicMediumArmorFocusEndurance>(c => {
                            c.Descriptor = ModifierDescriptor.ArmorFocus;
                            c.CheckArmorType = true;
                            c.ArmorTypes = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                        });
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusMediumMythicFeatureVar2SubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                        });
                    });
                    ArmorFocusMediumMythicFeatureVar2Buff.SetComponents();
                    ArmorFocusMediumMythicFeatureVar2SubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusMediumMythicFeatureVar2.m_DisplayName);
                        bp.SetComponents();
                        bp.AddComponent<ArmorSpeedPenaltyRemoval>();
                    });
                }
            }
            static void PatchMythicHeavyArmorFocus() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("MythicArmorFocus")) { return; }

                var ArmorFocusHeavyMythicFeatureOffence = BlueprintTools.GetBlueprint<BlueprintFeature>("037c3fde2ad14767a92768c9aa6d6de2");
                var ArmorFocusHeavyMythicFeatureOffenceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("fcd913568e1544108eceb8db2a90cd0f");
                var ArmorFocusHeavyMythicFeatureOffenceSubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5ec6fdb6fa4741798e3264c09e91c949");

                var ArmorFocusHeavyMythicFeatureVar1 = BlueprintTools.GetBlueprint<BlueprintFeature>("b28a0e962e3c40ee84411cccef31d4d0");
                var ArmorFocusHeavyMythicFeatureVar1Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("af7a83e16d1442cb87e84f879bf2141b");
                var ArmorFocusHeavyMythicFeatureVar1SubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6610a6ded3fa4cc8bd7fc86ccbfa722f");

                var ArmorFocusHeavyMythicFeatureVar2 = BlueprintTools.GetBlueprint<BlueprintFeature>("9b83bd09e7154b5f9c22a172efaaef4e");
                var ArmorFocusHeavyMythicFeatureVar2Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("95a2856c0369449db24000d2fb4e9277");
                var ArmorFocusHeavyMythicFeatureVar2SubBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5d4e71c6f33b4f7fa41f7db0af3ac320");

                PatchAssault();
                PatchEndurance();
                PatchAvoidance();

                void PatchAssault() {
                    ArmorFocusHeavyMythicFeatureOffence.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Heavy Armor — Assault");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusHeavyMythicFeatureOffenceSubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                        });
                    });
                    ArmorFocusHeavyMythicFeatureOffenceBuff.SetComponents();
                    ArmorFocusHeavyMythicFeatureOffenceSubBuff.TemporaryContext(bp => {
                        bp.SetName(ArmorFocusHeavyMythicFeatureOffence.m_DisplayName);
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        });
                    });
                }
                void PatchEndurance() {
                    ArmorFocusHeavyMythicFeatureVar1.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Heavy Armor — Endurance");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusHeavyMythicFeatureVar1SubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                        });
                        bp.AddComponent<RecalculateOnStatChange>(c => {
                            c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        });
                    });
                    ArmorFocusHeavyMythicFeatureVar1Buff.SetComponents();
                    ArmorFocusHeavyMythicFeatureVar1SubBuff.TemporaryContext(bp => {
                         bp.SetName(ArmorFocusHeavyMythicFeatureVar1.m_DisplayName);
                     });
                }
                void PatchAvoidance() {
                    ArmorFocusHeavyMythicFeatureVar2.TemporaryContext(bp => {
                        bp.SetName(TTTContext, "Mythic Heavy Armor — Avoidance");
                        bp.RemoveComponents<HasArmorFeatureUnlock>();
                        bp.AddComponent<ArmorFeatureUnlock>(c => {
                            c.NewFact = ArmorFocusHeavyMythicFeatureVar2SubBuff.ToReference<BlueprintUnitFactReference>();
                            c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                        });
                    });
                    ArmorFocusHeavyMythicFeatureVar2Buff.SetComponents();
                    ArmorFocusHeavyMythicFeatureVar2SubBuff.TemporaryContext(bp => {
                         bp.SetName(ArmorFocusHeavyMythicFeatureVar2.m_DisplayName);
                     });
                }
            }
        }
    }
}
