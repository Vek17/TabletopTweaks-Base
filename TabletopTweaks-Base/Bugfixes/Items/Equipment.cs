using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Items {
    static class Equipment {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Equipment");
                PatchAspectOfTheAsp();
                PatchMagiciansRing();
                PatchManglingFrenzy();
                PatchMetamagicRods();
                PatchHolySymbolofIomedae();
                PatchHalfOfThePair();
                PatchStormlordsResolve();
                //PatchFlawlessBeltOfPhysicalPerfection8Availability();
                PatchFlawlessBeltOfPhysicalPerfection8CritIncrease();

                void PatchAspectOfTheAsp() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("AspectOfTheAsp")) { return; }

                    var AspectOfTheAspItem = BlueprintTools.GetBlueprint<BlueprintItemEquipmentNeck>("7d55f6615f884bc45b85fdaa45cd7672");
                    var AspectOfTheAspFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9b3f6877efdf29a4e821c33ec830f312");
                    var RayType = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("1d39a22f206840e40b2255fc0175b8d0");
                    AspectOfTheAspFeature.m_DisplayName = AspectOfTheAspItem.m_DisplayNameText;
                    AspectOfTheAspFeature.SetComponents();
                    AspectOfTheAspFeature.AddComponent<IncreaseSpellDescriptorDC>(c => {
                        c.Descriptor = SpellDescriptor.Poison;
                        c.BonusDC = 2;
                    });
                    AspectOfTheAspFeature.AddComponent<AddOutgoingDamageTriggerTTT>(c => {
                        c.IgnoreDamageFromThisFact = true;
                        c.CheckAbilityType = true;
                        c.m_AbilityType = AbilityType.Spell;
                        c.CheckWeaponType = true;
                        c.m_WeaponType = RayType;
                        c.OncePerAttackRoll = true;
                        c.Actions = Helpers.CreateActionList(
                            Helpers.Create<ContextActionDealDamageTTT>(a => {
                                a.DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Acid
                                };
                                a.Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                };
                                a.Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 1,
                                    BonusValue = 5
                                };
                                a.IgnoreCritical = true;
                                a.SetFactAsReason = true;
                                a.IgnoreWeapon = true;
                            })
                        );
                    });
                    TTTContext.Logger.LogPatch(AspectOfTheAspFeature);
                }
                void PatchFlawlessBeltOfPhysicalPerfection8CritIncrease() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("FlawlessBeltOfPhysicalPerfection8CritIncrease")) { return; }

                    var BeltOfPerfection8ExtraFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e1f419f50b5a45158080bb4cb4ff6858");

                    if (BeltOfPerfection8ExtraFeature != null) {
                        BeltOfPerfection8ExtraFeature.SetComponents();
                        BeltOfPerfection8ExtraFeature.AddComponent<AddFlatCriticalRangeIncrease>(c => {
                            c.CriticalRangeIncrease = 1;
                            c.AllWeapons = true;
                        });
                        TTTContext.Logger.LogPatch(BeltOfPerfection8ExtraFeature);
                    }
                }
                void PatchFlawlessBeltOfPhysicalPerfection8Availability() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("FlawlessBeltOfPhysicalPerfection8Availability")) { return; }

                    var DLC1_InevitableDarkness_CoreReward = BlueprintTools.GetBlueprint<BlueprintLoot>("b4ba9f9162694daeabca42b2de9a98d8");
                    var BeltOfPerfection8Extra = BlueprintTools.GetBlueprintReference<BlueprintItemReference>("3c3a3a043b99422480b04940bc1edc73");

                    if (DLC1_InevitableDarkness_CoreReward != null) {
                        DLC1_InevitableDarkness_CoreReward.Items = new LootEntry[] {
                            new LootEntry(){
                                m_Item = BeltOfPerfection8Extra,
                                Count = 1
                            }
                        };
                        TTTContext.Logger.LogPatch(DLC1_InevitableDarkness_CoreReward);
                    }
                }
                void PatchMagiciansRing() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("MagiciansRing")) { return; }

                    var RingOfTheSneakyWizardFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d848f1f1b31b3e143ba4aeeecddb17f4");
                    RingOfTheSneakyWizardFeature.GetComponent<IncreaseSpellSchoolDC>().BonusDC = 2;
                    TTTContext.Logger.LogPatch(RingOfTheSneakyWizardFeature);
                }
                void PatchHalfOfThePair() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("HalfOfThePair")) { return; }

                    var HalfOfPairedPendantArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("8187fd9306b8c4f46824fbba9808f458");
                    var HalfOfPairedPendantBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("066229a41ae97d6439fea81ebf141528");
                    var HalfOfPairedPendantPersonalBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("71a14bfc21b64ad4bbb916a7ad58effb");
                    HalfOfPairedPendantArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .Round = Helpers.CreateActionList();
                    HalfOfPairedPendantArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .UnitEnter
                        .Actions
                        .OfType<Conditional>()
                        .FirstOrDefault()
                        .ConditionsChecker
                        .Conditions = new Condition[] {
                            new ContextConditionHasFact() {
                                m_Fact = HalfOfPairedPendantBuff.ToReference<BlueprintUnitFactReference>(),
                            },
                            new ContextConditionIsCaster() {
                                Not = true
                            }
                        };
                    HalfOfPairedPendantArea.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .ForEach(c => {
                            c.ToCaster = false;
                            c.AsChild = false;
                        });
                    HalfOfPairedPendantArea.FlattenAllActions()
                        .OfType<ContextActionRemoveBuff>()
                        .ForEach(c => c.ToCaster = false);
                    TTTContext.Logger.LogPatch(HalfOfPairedPendantArea);
                }
                void PatchHolySymbolofIomedae() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("HolySymbolofIomedae")) { return; }

                    var Artifact_HolySymbolOfIomedaeArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("e6dff35442f00ab4fa2468804c15efc0");
                    var Artifact_HolySymbolOfIomedaeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c8b1c0f5cd21f1d4e892f7440ec28e24");
                    Artifact_HolySymbolOfIomedaeArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .UnitExit = Helpers.CreateActionList(
                            Helpers.Create<ContextActionRemoveBuff>(a => a.m_Buff = Artifact_HolySymbolOfIomedaeBuff.ToReference<BlueprintBuffReference>())
                    );
                    TTTContext.Logger.LogPatch(Artifact_HolySymbolOfIomedaeArea);
                }
                // Fix Mangling Frenzy does not apply to Bloodrager's Rage
                void PatchManglingFrenzy() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("ManglingFrenzy")) { return; }

                    var ManglingFrenzyFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("29e2f51e6dd7427099b015de88718990");
                    var ManglingFrenzyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1581c5ceea24418cadc9f26ce4d391a9");
                    var BloodragerStandartRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");

                    ManglingFrenzyFeature.AddComponent(Helpers.Create<BuffExtraEffects>(c => {
                        c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                        c.m_ExtraEffectBuff = ManglingFrenzyBuff.ToReference<BlueprintBuffReference>();
                    }));

                    TTTContext.Logger.LogPatch(ManglingFrenzyFeature);
                }
                void PatchStormlordsResolve() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("StormlordsResolve")) { return; }

                    var StormlordsResolveActivatableAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ae908f59269c54c4d83ca51a63be8db4");
                    StormlordsResolveActivatableAbility.DeactivateImmediately = true;

                    TTTContext.Logger.LogPatch(StormlordsResolveActivatableAbility);
                }
                void PatchMetamagicRods() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("MetamagicRods")) { return; }

                    BlueprintActivatableAbility[] MetamagicRodAbilities = new BlueprintActivatableAbility[] {
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ccffef1193d04ad1a9430a8009365e81"), //MetamagicRodGreaterBolsterToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("cc266cfb106a5a3449b383a25ab364f0"), //MetamagicRodGreaterEmpowerToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("c137a17a798334c4280e1eb811a14a70"), //MetamagicRodGreaterExtendToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("78b5971c7a0b7f94db5b4d22c2224189"), //MetamagicRodGreaterMaximizeToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5016f110e5c742768afa08224d6cde56"), //MetamagicRodGreaterPersistentToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("fca35196b3b23c346a7d1b1ce20c6f1c"), //MetamagicRodGreaterQuickenToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("cc116b4dbb96375429107ed2d88943a1"), //MetamagicRodGreaterReachToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f0d798f5139440a8b2e72fe445678d29"), //MetamagicRodGreaterSelectiveToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("056b9f1aa5c54a7996ca8c4a00a88f88"), //MetamagicRodLesserBolsterToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ed10ddd385a528944bccbdc4254f8392"), //MetamagicRodLesserEmpowerToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("605e64c0b4586a34494fc3471525a2e5"), //MetamagicRodLesserExtendToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("868673cd023f96945a2ee61355740a96"), //MetamagicRodLesserKineticToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("485ffd3bd7877fb4d81409b120a41076"), //MetamagicRodLesserMaximizeToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5a87350fcc6b46328a2b345f23bbda44"), //MetamagicRodLesserPersistentToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("b8b79d4c37981194fa91771fc5376c5e"), //MetamagicRodLesserQuickenToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("7dc276169f3edd54093bf63cec5701ff"), //MetamagicRodLesserReachToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("66e68fd0b661413790e3000ede141f16"), //MetamagicRodLesserSelectiveToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("afb2e1f96933c22469168222f7dab8fb"), //MetamagicRodMasterpieceToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("6cc31148ae2d48359c02712308cb4167"), //MetamagicRodNormalBolsterToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("077ec9f9394b8b347ba2b9ec45c74739"), //MetamagicRodNormalEmpowerToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("69de70b88ca056440b44acb029a76cd7"), //MetamagicRodNormalExtendToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("3b5184a55f98f264f8b39bddd3fe0e88"), //MetamagicRodNormalMaximizeToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("9ae2e56b24404144bd911378fe541597"), //MetamagicRodNormalPersistentToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("1f390e6f38d3d5247aacb25ab3a2a6d2"), //MetamagicRodNormalQuickenToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f0b05e39b82c3be408009e26be40bc91"), //MetamagicRodNormalReachToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("04f768c59bb947e3948ce2e7e72feecb"), //MetamagicRodNormalSelectiveToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("afb2e1f96933c22469168222f7dab8fb"), //MetamagicRodMasterpieceToggleAbility - Grandmaster's Rod
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("c43746bc20a151b4eaf1836cc6eb9a92"), //AmberLoveRodToggleAbility - Passion's Sweet Poison
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("afca601615e344446a24433202567c39"), //RodOfMysticismToggleAbility - Rod of Mysticism
                    };
                    MetamagicRodAbilities.ForEach(ability => {
                        ability.IsOnByDefault = false;
                        ability.DoNotTurnOffOnRest = false;
                        TTTContext.Logger.LogPatch("Patched", ability);
                    });
                }
            }
        }
        [HarmonyPatch(typeof(ItemStatHelper), nameof(ItemStatHelper.GetUseMagicDeviceDC))]
        static class ItemStatHelper_GetUseMagicDeviceDC_Patch {
            static void Postfix(ItemEntity item, UnitEntityData caster, ref int? __result) {
                if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("FixScrollUMDDCs")) { return; }
                BlueprintItemEquipmentUsable blueprintItemEquipmentUsable = item.Blueprint as BlueprintItemEquipmentUsable;
                if (blueprintItemEquipmentUsable == null) {
                    __result = null;
                    return;
                }
                if (caster != null && !blueprintItemEquipmentUsable.IsUnitNeedUMDForUse(caster)) {
                    __result = null;
                    return;
                }
                __result = blueprintItemEquipmentUsable.Type == UsableItemType.Scroll ? 20 + item.GetCasterLevel() : 20;
            }
        }
    }
}
