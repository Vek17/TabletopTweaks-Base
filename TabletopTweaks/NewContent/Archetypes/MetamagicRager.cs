using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace TabletopTweaks.NewContent.Archetypes {
    static class MetamagicRager {
        private static BlueprintCharacterClass BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
        private static BlueprintSpellbook BloodragerSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
        private static BlueprintAbilityResource BloodragerRageResource = Resources.GetBlueprint<BlueprintAbilityResource>("4aec9ec9d9cd5e24a95da90e56c72e37");
        private static BlueprintFeature ImprovedUncannyDodge = Resources.GetBlueprint<BlueprintFeature>("485a18c05792521459c7d06c63128c79");

        private static BlueprintFeature EmpowerSpellFeat = Resources.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
        private static BlueprintFeature ExtendSpellFeat = Resources.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
        private static BlueprintFeature MaximizeSpellFeat = Resources.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
        private static BlueprintFeature PersistentSpellFeat = Resources.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
        private static BlueprintFeature QuickenSpellFeat = Resources.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
        private static BlueprintFeature ReachSpellFeat = Resources.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78");
        private static BlueprintFeature SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");

        public static void AddMetamagicRager() {

            var MetaRageEmpowerBuff = CreateMetamagicBuff(EmpowerSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageEmpowerBuff"];
                bp.name = "MetaRageEmpowerBuff";
                bp.SetName("Meta-Rage (Empower)");
                bp.SetDescription("The metamagic rager can spend 4 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Empowered as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageExtendBuff = CreateMetamagicBuff(ExtendSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageExtendBuff"];
                bp.name = "MetaRageExtendBuff";
                bp.SetName("Meta-Rage (Extend)");
                bp.SetDescription("The metamagic rager can spend 2 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Extended as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageMaximizeBuff = CreateMetamagicBuff(MaximizeSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageMaximizeBuff"];
                bp.name = "MetaRageMaximizeBuff";
                bp.SetName("Meta-Rage (Maximize)");
                bp.SetDescription("The metamagic rager can spend 6 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Maximized as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRagePersistentBuff = CreateMetamagicBuff(PersistentSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRagePersistentBuff"];
                bp.name = "MetaRagePersistentBuff";
                bp.SetName("Meta-Rage (Persistent)");
                bp.SetDescription("The metamagic rager can spend 4 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Persistent as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageQuickenBuff = CreateMetamagicBuff(QuickenSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageQuickenBuff"];
                bp.name = "MetaRageQuickenBuff";
                bp.SetName("Meta-Rage (Quicken)");
                bp.SetDescription("The metamagic rager can spend 6 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Quickened as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageReachBuff = CreateMetamagicBuff(ReachSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageReachBuff"];
                bp.name = "MetaRageReachBuff";
                bp.SetName("Meta-Rage (Reach)");
                bp.SetDescription("The metamagic rager can spend 2 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Reach as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageSelectiveBuff = CreateMetamagicBuff(SelectiveSpellFeat, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageSelectiveBuff"];
                bp.name = "MetaRageSelectiveBuff";
                bp.SetName("Meta-Rage (Selective)");
                bp.SetDescription("The metamagic rager can spend 2 rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Selective as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });

            var MetaRageBuffs = new BlueprintUnitFactReference[] {
                MetaRageEmpowerBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageExtendBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageMaximizeBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRagePersistentBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageQuickenBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageReachBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageSelectiveBuff.ToReference<BlueprintUnitFactReference>(),
            };

            var MetaRageEmpowerAbility = CreateMetamagicAbility(MetaRageEmpowerBuff, 4, EmpowerSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageEmpowerAbility"];
                bp.name = "MetaRageEmpowerAbility";
                bp.SetName("Meta-Rage (Empower)");
                bp.SetDescription(MetaRageEmpowerBuff.Description);
            });
            var MetaRageExtendAbility = CreateMetamagicAbility(MetaRageExtendBuff, 2, ExtendSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageExtendAbility"];
                bp.name = "MetaRageExtendAbility";
                bp.SetName("Meta-Rage (Extend)");
                bp.SetDescription(MetaRageExtendBuff.Description);
            });
            var MetaRageMaximizeAbility = CreateMetamagicAbility(MetaRageMaximizeBuff, 6, MaximizeSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageMaximizeAbility"];
                bp.name = "MetaRageMaximizeAbility";
                bp.SetName("Meta-Rage (Maximize)");
                bp.SetDescription(MetaRageMaximizeBuff.Description);
            });
            var MetaRagePersistentAbility = CreateMetamagicAbility(MetaRagePersistentBuff, 4, PersistentSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRagePersistentAbility"];
                bp.name = "MetaRagePersistentAbility";
                bp.SetName("Meta-Rage (Persistent)");
                bp.SetDescription(MetaRagePersistentBuff.Description);
            });
            var MetaRageQuickenAbility = CreateMetamagicAbility(MetaRageQuickenBuff, 6, QuickenSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageQuickenAbility"];
                bp.name = "MetaRageQuickenAbility";
                bp.SetName("Meta-Rage (Quicken)");
                bp.SetDescription(MetaRageQuickenBuff.Description);
            });
            var MetaRageReachAbility = CreateMetamagicAbility(MetaRageReachBuff, 2, ReachSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageReachAbility"];
                bp.name = "MetaRageReachAbility";
                bp.SetName("Meta-Rage (Reach)");
                bp.SetDescription(MetaRageReachBuff.Description);
            });
            var MetaRageSelectiveAbility = CreateMetamagicAbility(MetaRageSelectiveBuff, 2, SelectiveSpellFeat, MetaRageBuffs, bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageSelectiveAbility"];
                bp.name = "MetaRageSelectiveAbility";
                bp.SetName("Meta-Rage (Selective)");
                bp.SetDescription(MetaRageSelectiveBuff.Description);
            });

            var MetaRageBaseAbility = Helpers.Create<BlueprintAbility>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageBaseAbility"];
                bp.name = "MetaRageBaseAbility";
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
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Icon_MetaRage.png");
                bp.AddComponent(Helpers.Create<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        MetaRageEmpowerAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageExtendAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageMaximizeAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRagePersistentAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageQuickenAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageReachAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageSelectiveAbility.ToReference<BlueprintAbilityReference>(),
                    };
                }));
            });
            var MetaRageFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetaRageFeature"];
                bp.name = "MetaRageFeature";
                bp.SetName("Meta-Rage");
                bp.SetDescription(MetaRageBaseAbility.Description);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Icon_MetaRage.png");
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { MetaRageBaseAbility.ToReference<BlueprintUnitFactReference>() };
                }));
            });
            var MetamagicRagerArchetype = Helpers.Create<BlueprintArchetype>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["MetamagicRagerArchetype"];
                bp.name = "MetamagicRagerArchetype";
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

            Resources.AddBlueprint(MetaRageEmpowerBuff);
            Resources.AddBlueprint(MetaRageExtendBuff);
            Resources.AddBlueprint(MetaRageMaximizeBuff);
            Resources.AddBlueprint(MetaRagePersistentBuff);
            Resources.AddBlueprint(MetaRageQuickenBuff);
            Resources.AddBlueprint(MetaRageReachBuff);
            Resources.AddBlueprint(MetaRageSelectiveBuff);

            Resources.AddBlueprint(MetaRageEmpowerAbility);
            Resources.AddBlueprint(MetaRageExtendAbility);
            Resources.AddBlueprint(MetaRageMaximizeAbility);
            Resources.AddBlueprint(MetaRagePersistentAbility);
            Resources.AddBlueprint(MetaRageQuickenAbility);
            Resources.AddBlueprint(MetaRageReachAbility);
            Resources.AddBlueprint(MetaRageSelectiveAbility);

            Resources.AddBlueprint(MetaRageBaseAbility);
            Resources.AddBlueprint(MetaRageFeature);
            Resources.AddBlueprint(MetamagicRagerArchetype);

            //if (ModSettings.AddedContent.Archetypes.DisableAll || !ModSettings.AddedContent.Archetypes.Enabled["ElementalMasterArchetype"]) { return; }

            BloodragerClass.m_Archetypes = BloodragerClass.m_Archetypes.AppendToArray(MetamagicRagerArchetype.ToReference<BlueprintArchetypeReference>());
            Main.LogPatch("Added", MetamagicRagerArchetype);
        }
        private static BlueprintBuff CreateMetamagicBuff(BlueprintFeature metamagicFeat, Action<BlueprintBuff> init = null) {
            var result = Helpers.CreateBuff(bp => {
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
                    c.MaxSpellLevel = 10;
                    c.CheckSpellbook = true;
                }));
            });
            init?.Invoke(result);
            return result;
        }
        private static BlueprintAbility CreateMetamagicAbility(BlueprintBuff buff, int cost, BlueprintFeature metamagicFeat, BlueprintUnitFactReference[] blockedBuffs, Action<BlueprintAbility> init = null) {
            var result = Helpers.Create<BlueprintAbility>(bp => {
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
                    c.Amount = cost;
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

        [HarmonyPatch(typeof(MechanicActionBarSlotActivableAbility), "GetResource")]
        static class MechanicActionBarSlotActivableAbility_Limitless_Patch {
            static void Postfix(ref int __result, MechanicActionBarSlotActivableAbility __instance) {
                //if (ModSettings.Fixes.MythicAbilities.DisableAll || !ModSettings.Fixes.MythicAbilities.Enabled["DomainZealot"]) { return true; }
                var resourceLogic = __instance.ActivatableAbility.Blueprint.GetComponent<ActivatableAbilityResourceLogic>();
                if (resourceLogic != null && __result == 0) {
                    if (__instance.ActivatableAbility.Owner.HasFact(resourceLogic.FreeBlueprint)) {
                        __result = 1;
                    }
                }
            }
        }
    }
}
