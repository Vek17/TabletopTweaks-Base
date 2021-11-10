using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace TabletopTweaks.NewContent.Archetypes {
    static class MetamagicRager {
        private static readonly BlueprintCharacterClass BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
        private static readonly BlueprintSpellbook BloodragerSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
        private static readonly BlueprintAbilityResource BloodragerRageResource = Resources.GetBlueprint<BlueprintAbilityResource>("4aec9ec9d9cd5e24a95da90e56c72e37");
        private static readonly BlueprintFeature ImprovedUncannyDodge = Resources.GetBlueprint<BlueprintFeature>("485a18c05792521459c7d06c63128c79");

        private static readonly BlueprintFeature EmpowerSpellFeat = Resources.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
        private static readonly BlueprintFeature ExtendSpellFeat = Resources.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
        private static readonly BlueprintFeature HeightenSpellFeat = Resources.GetBlueprint<BlueprintFeature>("2f5d1e705c7967546b72ad8218ccf99c");
        private static readonly BlueprintFeature MaximizeSpellFeat = Resources.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
        private static readonly BlueprintFeature PersistentSpellFeat = Resources.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
        private static readonly BlueprintFeature QuickenSpellFeat = Resources.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
        private static readonly BlueprintFeature ReachSpellFeat = Resources.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78");
        private static readonly BlueprintFeature SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
        private static readonly BlueprintFeature BolsteredSpellFeat = Resources.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");
        private static readonly BlueprintFeature CompletelyNormalSpellFeat = Resources.GetBlueprint<BlueprintFeature>("094b6278f7b570f42aeaa98379f07cf2");

        public static void AddMetamagicRager() {

            var MetaRageFeature = Helpers.CreateBlueprint<BlueprintFeature>("MetaRageFeature", bp => {
                bp.SetName("Meta-Rage");
                bp.SetDescription("At 5th level, a metamagic rager can sacrifice additional rounds of " +
                    "bloodrage to apply a metamagic feat he knows to a bloodrager spell. This costs a number of rounds of bloodrage equal to twice what the spell’s " +
                    "adjusted level would normally be with the metamagic feat applied (minimum 2 rounds). The metamagic rager does not have to be bloodraging " +
                    "to use this ability. The metamagic effect is applied without increasing the level of the spell slot expended, though the casting time is " +
                    "increased as normal. The metamagic rager can apply only one metamagic feat he knows in this manner with each casting.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Icon_MetaRage.png");
                bp.AddComponent<MetaRageComponent>(c => {
                    c.ConvertSpellbook = BloodragerSpellbook.ToReference<BlueprintSpellbookReference>();
                    c.RequiredResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();
                });
            });
            var MetamagicRagerArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("MetamagicRagerArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString("MetamagicRagerArchetype.Name", "Metamagic Rager");
                bp.LocalizedDescription = Helpers.CreateString("MetamagicRagerArchetype.Description", "While metamagic is difficult for many bloodragers to utilize, " +
                    "a talented few are able to channel their bloodrage in ways that push their spells to impressive ends.");
                bp.AddFeatures = new LevelEntry[] {
                    new LevelEntry() {
                        Level = 5,
                        m_Features = new System.Collections.Generic.List<BlueprintFeatureBaseReference>() {
                            MetaRageFeature.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }
                };
                bp.RemoveFeatures = new LevelEntry[] {
                    new LevelEntry() {
                        Level = 5,
                        m_Features = new System.Collections.Generic.List<BlueprintFeatureBaseReference>() {
                            ImprovedUncannyDodge.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }
                };
            });
            PatchBloodlines(MetamagicRagerArchetype);
            // These abilities are deprecated but are still around for save compatability
            var MetaRageBaseAbility1 = CreateMetaRageLevel(1);
            var MetaRageBaseAbility2 = CreateMetaRageLevel(2);
            var MetaRageBaseAbility3 = CreateMetaRageLevel(3);
            var MetaRageBaseAbility4 = CreateMetaRageLevel(4);
            if (ModSettings.AddedContent.Archetypes.IsDisabled("MetamagicRager")) { return; }
            BloodragerClass.m_Archetypes = BloodragerClass.m_Archetypes.AppendToArray(MetamagicRagerArchetype.ToReference<BlueprintArchetypeReference>());
            Main.LogPatch("Added", MetamagicRagerArchetype);
        }
        private static BlueprintBuff CreateMetamagicBuff(string name, BlueprintFeature metamagicFeat, int level, Action<BlueprintBuff> init = null) {
            var result = Helpers.CreateBuff(name, bp => {
                bp.m_Icon = metamagicFeat.Icon;
                bp.AddComponent(Helpers.Create<AddAbilityUseTrigger>(c => {
                    c.m_Spellbooks = new BlueprintSpellbookReference[] { BloodragerSpellbook.ToReference<BlueprintSpellbookReference>() };
                    c.m_Ability = new BlueprintAbilityReference();
                    c.Action = new ActionList() {
                        Actions = new GameAction[] {
                            Helpers.Create<ContextActionRemoveSelf>()
                        }
                    };
                    c.AfterCast = true;
                    c.FromSpellbook = true;
                }));
                bp.AddComponent(Helpers.Create<AutoMetamagic>(c => {
                    c.m_Spellbook = BloodragerSpellbook.ToReference<BlueprintSpellbookReference>();
                    c.Metamagic = metamagicFeat.GetComponent<AddMetamagicFeat>().Metamagic;
                    c.School = SpellSchool.None;
                    c.MaxSpellLevel = level;
                    c.CheckSpellbook = true;
                }));
            });
            init?.Invoke(result);
            return result;
        }
        private static BlueprintAbility CreateMetamagicAbility(string name, BlueprintBuff buff, int cost, BlueprintFeature metamagicFeat, BlueprintUnitFactReference[] blockedBuffs, Action<BlueprintAbility> init = null) {
            var result = Helpers.CreateBlueprint<BlueprintAbility>(name, bp => {
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = CommandType.Free;
                bp.LocalizedDuration = new LocalizedString();
                bp.LocalizedSavingThrow = new LocalizedString();
                bp.m_Icon = metamagicFeat.Icon;
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList() {
                        Actions = new GameAction[] {
                            Helpers.Create<ContextActionApplyBuff>(a => {
                                a.m_Buff = buff.ToReference<BlueprintBuffReference>();
                                a.DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue(){
                                        Value = 2
                                    }
                                };
                                a.AsChild = true;
                            })
                        }
                    };

                }));
                bp.AddComponent(Helpers.Create<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = buff.GetComponent<AutoMetamagic>().MaxSpellLevel * 2 + cost;
                }));
                bp.AddComponent(Helpers.Create<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = metamagicFeat.ToReference<BlueprintUnitFactReference>();
                }));
                bp.AddComponent(Helpers.Create<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = blockedBuffs;
                }));
            });
            init?.Invoke(result);
            return result;
        }
        private static BlueprintAbility CreateMetaRageLevel(int level) {
            var MetaRageEmpowerBuff = CreateMetamagicBuff($"MetaRageEmpowerBuff{level}", EmpowerSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Empower)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 4} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Empowered as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageExtendBuff = CreateMetamagicBuff($"MetaRageExtendBuff{level}", ExtendSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Extend)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Extended as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageMaximizeBuff = CreateMetamagicBuff($"MetaRageMaximizeBuff{level}", MaximizeSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Maximize)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 6} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Maximized as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRagePersistentBuff = CreateMetamagicBuff($"MetaRagePersistentBuff{level}", PersistentSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Persistent)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 4} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Persistent as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageQuickenBuff = CreateMetamagicBuff($"MetaRageQuickenBuff{level}", QuickenSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Quicken)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 8} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Quickened as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageReachBuff = CreateMetamagicBuff($"MetaRageReachBuff{level}", ReachSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Reach)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Reach as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageSelectiveBuff = CreateMetamagicBuff($"MetaRageSelectiveBuff{level}", SelectiveSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Selective)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Selective as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageBolsteredBuff = CreateMetamagicBuff($"MetaRageBolsteredBuff{level}", BolsteredSpellFeat, level, bp => {
                bp.SetName("Meta-Rage (Bolstered)");
                bp.SetDescription($"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Bolstered as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });

            var MetaRageBuffs = new BlueprintUnitFactReference[] {
                MetaRageEmpowerBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageExtendBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageMaximizeBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRagePersistentBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageQuickenBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageReachBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageSelectiveBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageBolsteredBuff.ToReference<BlueprintUnitFactReference>()
            };

            var MetaRageEmpowerAbility = CreateMetamagicAbility($"MetaRageEmpowerAbility{level}", MetaRageEmpowerBuff, 4, EmpowerSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Empower)");
                bp.SetDescription(MetaRageEmpowerBuff.m_Description);
            });
            var MetaRageExtendAbility = CreateMetamagicAbility($"MetaRageExtendAbility{level}", MetaRageExtendBuff, 2, ExtendSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Extend)");
                bp.SetDescription(MetaRageExtendBuff.m_Description);
            });
            var MetaRageMaximizeAbility = CreateMetamagicAbility($"MetaRageMaximizeAbility{level}", MetaRageMaximizeBuff, 6, MaximizeSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Maximize)");
                bp.SetDescription(MetaRageMaximizeBuff.m_Description);
            });
            var MetaRagePersistentAbility = CreateMetamagicAbility($"MetaRagePersistentAbility{level}", MetaRagePersistentBuff, 4, PersistentSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Persistent)");
                bp.SetDescription(MetaRagePersistentBuff.m_Description);
            });
            var MetaRageQuickenAbility = CreateMetamagicAbility($"MetaRageQuickenAbility{level}", MetaRageQuickenBuff, 8, QuickenSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Quicken)");
                bp.SetDescription(MetaRageQuickenBuff.m_Description);
            });
            var MetaRageReachAbility = CreateMetamagicAbility($"MetaRageReachAbility{level}", MetaRageReachBuff, 2, ReachSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Reach)");
                bp.SetDescription(MetaRageReachBuff.m_Description);
            });
            var MetaRageSelectiveAbility = CreateMetamagicAbility($"MetaRageSelectiveAbility{level}", MetaRageSelectiveBuff, 2, SelectiveSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Selective)");
                bp.SetDescription(MetaRageSelectiveBuff.m_Description);
            });
            var MetaRageBolsteredAbility = CreateMetamagicAbility($"MetaRageBolsteredAbility{level}", MetaRageBolsteredBuff, 2, BolsteredSpellFeat, MetaRageBuffs, bp => {
                bp.SetName("Meta-Rage (Bolstered)");
                bp.SetDescription(MetaRageBolsteredBuff.m_Description);
            });

            var MetaRageBaseAbility = Helpers.CreateBlueprint<BlueprintAbility>($"MetaRageBaseAbility{level}", bp => {
                bp.SetName("Meta-Rage");
                bp.SetDescription("At 5th level, a metamagic rager can sacrifice additional rounds of " +
                    "bloodrage to apply a metamagic feat he knows to a bloodrager spell. This costs a number of rounds of bloodrage equal to twice what the spell’s " +
                    "adjusted level would normally be with the metamagic feat applied (minimum 2 rounds). The metamagic rager does not have to be bloodraging " +
                    "to use this ability. The metamagic effect is applied without increasing the level of the spell slot expended, though the casting time is " +
                    "increased as normal. The metamagic rager can apply only one metamagic feat he knows in this manner with each casting.");
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.LocalizedDuration = new LocalizedString();
                bp.LocalizedSavingThrow = new LocalizedString();
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", $"Icon_MetaRage{level}.png");
                bp.AddComponent(Helpers.Create<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        MetaRageEmpowerAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageExtendAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageMaximizeAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRagePersistentAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageQuickenAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageReachAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageSelectiveAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageBolsteredAbility.ToReference<BlueprintAbilityReference>()
                    };
                }));
                bp.AddComponent(Helpers.Create<AbilityShowIfCasterCanCastSpells>(c => {
                    c.Class = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = level;
                }));
            });

            return MetaRageBaseAbility;
        }
        private static void PatchBloodlines(BlueprintArchetype archetype) {
            var basicBloodlines = new BlueprintProgression[] {
                BloodlineTools.Bloodline.BloodragerAberrantBloodline,
                BloodlineTools.Bloodline.BloodragerAbyssalBloodline,
                BloodlineTools.Bloodline.BloodragerArcaneBloodline,
                BloodlineTools.Bloodline.BloodragerCelestialBloodline,
                BloodlineTools.Bloodline.BloodragerDestinedBloodline,
                BloodlineTools.Bloodline.BloodragerFeyBloodline,
                BloodlineTools.Bloodline.BloodragerInfernalBloodline,
                BloodlineTools.Bloodline.BloodragerSerpentineBloodline,
                BloodlineTools.Bloodline.BloodragerUndeadBloodline,
            };
            var draconicBloodlines = new BlueprintProgression[] {
                BloodlineTools.Bloodline.BloodragerDragonBlackBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBlueBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBrassBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBronzeBloodline,
                BloodlineTools.Bloodline.BloodragerDragonCopperBloodline,
                BloodlineTools.Bloodline.BloodragerDragonGoldBloodline,
                BloodlineTools.Bloodline.BloodragerDragonGreenBloodline,
                BloodlineTools.Bloodline.BloodragerDragonRedBloodline,
                BloodlineTools.Bloodline.BloodragerDragonSilverBloodline,
                BloodlineTools.Bloodline.BloodragerDragonWhiteBloodline,
            };
            var elementalBloodlines = new BlueprintProgression[] {
                BloodlineTools.Bloodline.BloodragerElementalAcidBloodline,
                BloodlineTools.Bloodline.BloodragerElementalColdBloodline,
                BloodlineTools.Bloodline.BloodragerElementalElectricityBloodline,
                BloodlineTools.Bloodline.BloodragerElementalFireBloodline
            };
            int[] featLevels = { 6, 9, 12, 15, 18 };
            var metamagicFeats = new BlueprintFeature[] {
                EmpowerSpellFeat,
                ExtendSpellFeat,
                HeightenSpellFeat,
                MaximizeSpellFeat,
                PersistentSpellFeat,
                QuickenSpellFeat,
                ReachSpellFeat,
                SelectiveSpellFeat,
                CompletelyNormalSpellFeat
            };
            foreach (var bloodline in basicBloodlines) {
                BlueprintFeatureSelection MetamagicRagerFeatSelection = null;
                foreach (var levelEntry in bloodline.LevelEntries.Where(entry => featLevels.Contains(entry.Level))) {
                    foreach (var selection in levelEntry.Features.Where(f => f is BlueprintFeatureSelection)) {
                        if (selection.GetComponents<PrerequisiteNoArchetype>().Any(c => c.m_Archetype.Get().AssetGuid == archetype.AssetGuid)) { continue; }
                        var featSelect = selection as BlueprintFeatureSelection;
                        if (MetamagicRagerFeatSelection == null) {
                            MetamagicRagerFeatSelection = Helpers.CreateCopy(featSelect, bp => {
                                bp.name = GenerateName(bloodline);
                                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                                bp.HideNotAvailibleInUI = true;
                                bp.AddFeatures(metamagicFeats);
                                bp.AddComponent(Helpers.Create<PrerequisiteArchetypeLevel>(c => {
                                    c.HideInUI = true;
                                    c.CheckInProgression = true;
                                    c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                                    c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                                }));
                            });
                            Resources.AddBlueprint(MetamagicRagerFeatSelection);
                        }
                        selection.AddComponent(Helpers.Create<PrerequisiteNoArchetype>(c => {
                            c.HideInUI = true;
                            c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                            c.CheckInProgression = true;
                        }));
                    }
                    levelEntry.m_Features.Add(MetamagicRagerFeatSelection.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            BlueprintFeatureSelection DraconicMetamagicRagerFeatSelection = null;
            foreach (var bloodline in draconicBloodlines) {
                foreach (var levelEntry in bloodline.LevelEntries.Where(entry => featLevels.Contains(entry.Level))) {
                    foreach (var selection in levelEntry.Features.Where(f => f is BlueprintFeatureSelection)) {
                        if (selection.GetComponents<PrerequisiteNoArchetype>().Any(c => c.m_Archetype.Get().AssetGuid == archetype.AssetGuid)) { continue; }
                        var featSelect = selection as BlueprintFeatureSelection;
                        if (DraconicMetamagicRagerFeatSelection == null) {
                            DraconicMetamagicRagerFeatSelection = Helpers.CreateCopy(featSelect, bp => {
                                bp.name = GenerateName(bloodline);
                                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                                bp.AddFeatures(metamagicFeats);
                                bp.AddComponent(Helpers.Create<PrerequisiteArchetypeLevel>(c => {
                                    c.HideInUI = true;
                                    c.CheckInProgression = true;
                                    c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                                    c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                                }));
                            });
                            Resources.AddBlueprint(DraconicMetamagicRagerFeatSelection);
                        }
                        selection.AddComponent(Helpers.Create<PrerequisiteNoArchetype>(c => {
                            c.HideInUI = true;
                            c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                            c.CheckInProgression = true;
                        }));
                    }
                    levelEntry.m_Features.Add(DraconicMetamagicRagerFeatSelection.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            BlueprintFeatureSelection ElementalMetamagicRagerFeatSelection = null;
            foreach (var bloodline in elementalBloodlines) {
                foreach (var levelEntry in bloodline.LevelEntries.Where(entry => featLevels.Contains(entry.Level))) {
                    foreach (var selection in levelEntry.Features.Where(f => f is BlueprintFeatureSelection)) {
                        if (selection.GetComponents<PrerequisiteNoArchetype>().Any(c => c.m_Archetype.Get().AssetGuid == archetype.AssetGuid)) { continue; }
                        var featSelect = selection as BlueprintFeatureSelection;
                        if (ElementalMetamagicRagerFeatSelection == null) {
                            ElementalMetamagicRagerFeatSelection = Helpers.CreateCopy(featSelect, bp => {
                                bp.name = GenerateName(bloodline); ;
                                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                                bp.AddFeatures(metamagicFeats);
                                bp.AddComponent(Helpers.Create<PrerequisiteArchetypeLevel>(c => {
                                    c.HideInUI = true;
                                    c.CheckInProgression = true;
                                    c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                                    c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                                }));
                            });
                            Resources.AddBlueprint(ElementalMetamagicRagerFeatSelection);
                        }
                        selection.AddComponent(Helpers.Create<PrerequisiteNoArchetype>(c => {
                            c.HideInUI = true;
                            c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                            c.CheckInProgression = true;
                        }));
                    }
                    levelEntry.m_Features.Add(ElementalMetamagicRagerFeatSelection.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            string GenerateName(BlueprintFeature bloodline) {
                string[] split = Regex.Split(bloodline.name, @"(?<!^)(?=[A-Z])");
                return $"{split[0]}{split[1]}MetamagicRagerFeatSelection";
            }
        }
    }
}
