using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.MechanicsChanges;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewComponents.Properties;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Reworks {
    class Aeon {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Aeon Rework");

                PatchAeonBaneIcon();
                //PatchAeonBaneAction();
                PatchAeonBaneSpellResistance();
                PatchAeonBaneUses();
                PatchAeonImprovedBaneDispelLimit();
                PatchAeonGreaterBaneDamage();
                PatchAeonGreaterBaneDispel();
                PatchAeonGazeAction();
                PatchAeonGazeDC();
                PatchAeonGazeSelection();
            }

            static void PatchAeonBaneIcon() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonBaneIcon")) { return; }
                var Icon_AeonBane = AssetLoader.LoadInternal("Abilities", "Icon_AeonBane.png");
                var AeonBaneFeature = Resources.GetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
                var AeonBaneAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("67fb31f553f2bb14bbfae0b1040169f1");
                AeonBaneFeature.m_Icon = Icon_AeonBane;
                AeonBaneAbility.m_Icon = Icon_AeonBane;
                Main.LogPatch("Patched", AeonBaneFeature);
                Main.LogPatch("Patched", AeonBaneAbility);
            }
            static void PatchAeonBaneAction() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonBaneAction")) { return; }
                var AeonBaneAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("67fb31f553f2bb14bbfae0b1040169f1");
                AeonBaneAbility.m_ActivateWithUnitCommand = UnitCommand.CommandType.Free;
                Main.LogPatch("Patched", AeonBaneAbility);
            }
            static void PatchAeonBaneSpellResistance() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonBaneSpellResistance")) { return; }
                var AeonBaneBuff = Resources.GetBlueprint<BlueprintBuff>("345160619fc2ddc44b8ad98c94dde448");
                AeonBaneBuff.RemoveComponents<ModifyD20>();
                AeonBaneBuff.AddComponent<SpellPenetrationBonus>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                Main.LogPatch("Patched", AeonBaneBuff);
            }
            static void PatchAeonBaneUses() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonBaneUses")) { return; }
                var AeonClass = Resources.GetBlueprint<BlueprintCharacterClass>("15a85e67b7d69554cab9ed5830d0268e");
                var AeonBaneFeature = Resources.GetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
                var AeonBaneIncreaseResourceFeature = Resources.GetModBlueprint<BlueprintFeature>("AeonBaneIncreaseResourceFeature");
                AeonBaneFeature.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = AeonBaneIncreaseResourceFeature.ToReference<BlueprintFeatureReference>();
                }));
                AeonBaneFeature.SetDescription("At 4th rank, Aeon gains an ability to make his weapons and spells especially deadly " +
                    "against his enemies. For a number of rounds per day equal to twice your mythic rank plus your character level, " +
                    "you can make any weapon you wield count as having Bane quality, your spells gain a bonus equal to your mythic rank " +
                    "to caster level checks to overcome spell resistance, and every hit you make with weapons or spells dispel effects " +
                    "from the target, as per the dispel magic spell.\nAdditionally, if you have inquisitor's bane ability, you gain the " +
                    "same bonuses while using inquisitor's bane ability.");
                Main.LogPatch("Patched", AeonBaneFeature);
            }
            static void PatchAeonImprovedBaneDispelLimit() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonImprovedBaneDispelLimit")) { return; }

                var AeonBaneBuff = Resources.GetBlueprint<BlueprintBuff>("345160619fc2ddc44b8ad98c94dde448");
                AeonBaneBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>()
                    .Action
                    .Actions
                    .OfType<Conditional>()
                    .ForEach(conditional => {
                        conditional.IfTrue.Actions
                            .OfType<ContextActionDispelMagic>()
                            .ForEach(a => {
                                a.m_StopAfterCountRemoved = true;
                                a.m_CountToRemove = new ContextValue() {
                                    ValueType = ContextValueType.Shared,
                                    ValueShared = AbilitySharedValue.StatBonus
                                };
                            });
                    });
                AeonBaneBuff.GetComponent<AddAbilityUseTrigger>()
                    .Action
                    .Actions
                    .OfType<Conditional>()
                    .ForEach(conditional => {
                        conditional.IfTrue.Actions
                            .OfType<ContextActionDispelMagic>()
                            .ForEach(a => {
                                a.m_StopAfterCountRemoved = true;
                                a.m_CountToRemove = new ContextValue() {
                                    ValueType = ContextValueType.Shared,
                                    ValueShared = AbilitySharedValue.StatBonus
                                };
                            });
                    });
                AeonBaneBuff.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.StatBonus;
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.One,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        },
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                        },
                    };
                    c.Modifier = 0.25;
                });

                Main.LogPatch("Patched", AeonBaneBuff);
            }
            static void PatchAeonGreaterBaneDamage() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGreaterBaneDamage")) { return; }
                var AeonGreaterBaneBuff = Resources.GetBlueprint<BlueprintBuff>("cdcc13884252b2c4d8dac57cb5f46555");
                AeonGreaterBaneBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>(c => c.Action.Actions.OfType<ContextActionDealDamage>().Any());
                Main.LogPatch("Patched", AeonGreaterBaneBuff);
            }
            static void PatchAeonGreaterBaneDispel() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("PatchAeonGreaterBaneDispel")) { return; }

                var AeonGreaterBaneBuff = Resources.GetBlueprint<BlueprintBuff>("cdcc13884252b2c4d8dac57cb5f46555");
                AeonGreaterBaneBuff.GetComponents<AddInitiatorAttackWithWeaponTrigger>()
                    .Where(action => action.Action.Actions.OfType<ContextActionDispelMagic>().Any())
                    .First().OnlyOnFirstHit = true;
                AeonGreaterBaneBuff.FlattenAllActions()
                    .OfType<ContextActionDispelMagic>()
                    .ForEach(c => c.m_BuffType = ContextActionDispelMagic.BuffType.FromSpells);
                Main.LogPatch("Patched", AeonGreaterBaneBuff);
            }
            static void PatchAeonGazeDC() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGazeDC")) { return; }
                var AeonGazeDCProperty = Resources.GetBlueprint<BlueprintUnitProperty>("4358468cba854a6db8f60909dacf8203");
                var AeonAttackGazeAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("79d287eefba48424b9073a5cabba2a63");

                AeonGazeDCProperty.RemoveComponents<SimplePropertyGetter>();
                AeonGazeDCProperty.AddComponent<CompositePropertyGetter>(c => {
                    c.CalculationMode = CompositePropertyGetter.Mode.Sum;
                    c.Properties = new CompositePropertyGetter.ComplexProperty[] {
                        new CompositePropertyGetter.ComplexProperty {
                            Property = UnitProperty.Level,
                            Numerator = 1,
                            Denominator = 2
                        },
                        new CompositePropertyGetter.ComplexProperty {
                            Property = UnitProperty.MythicLevel,
                            Numerator = 2,
                            Denominator = 1
                        }
                    };
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.AsIs,
                        m_CustomProgression = new PropertySettings.CustomProgressionItem[0]
                    };
                });
                AeonGazeDCProperty.BaseValue = 0;
                Main.LogPatch("Patched", AeonGazeDCProperty);
            }
            static void PatchAeonGazeSelection() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGazeSelection")) { return; }
                var AeonGazeFirstSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7e468a5266235454e9dcefee315ee6d5");
                var AeonGazeSecondSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7d498e034d18ca94baea19d7edee7403");
                AeonGazeFirstSelection.m_AllFeatures = AeonGazeSecondSelection.m_AllFeatures;
                AeonGazeFirstSelection.m_Features = AeonGazeSecondSelection.m_Features;
                Main.LogPatch("Patched", AeonGazeFirstSelection);
            }
            static void PatchAeonGazeAction() {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGazeActionSystem")) { return; }
                var AeonGazeSecondSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7d498e034d18ca94baea19d7edee7403");
                AeonGazeSecondSelection.AllFeatures
                    .SelectMany(feature => feature.GetComponent<AddFacts>()?.m_Facts)
                    .Where(gaze => gaze != null)
                    .Select(gaze => gaze.Get() as BlueprintActivatableAbility)
                    .Where(gaze => gaze != null)
                    .ForEach(gaze => {
                        var spendLogic = gaze.GetComponent<ActivatableAbilityResourceLogic>();
                        if (spendLogic != null) {
                            spendLogic.SpendType = ActivatableAbilitySpendLogic.StandardSpendType.AeonGaze.ToResourceType();
                            Main.LogPatch("Patched", gaze);
                        }
                        var gazeBuff = gaze.Buff;
                        if (gazeBuff != null) {
                            gazeBuff.AddComponent<AeonGazeResouceLogic>();
                            Main.LogPatch("Patched", gazeBuff);
                        }
                    });
            }
        }
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.RuntimeActionType), MethodType.Getter)]
        static class AbilityData_GetRuntimeActionType_MythicMoveAction_Patch {
            static readonly BlueprintBuff AeonGreaterBaneBuff = Resources.GetBlueprint<BlueprintBuff>("cdcc13884252b2c4d8dac57cb5f46555");
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGreaterBaneActionBoost")) { return; }
                UnitCommand.CommandType commandType = __instance.ActionType;
                if (commandType == UnitCommand.CommandType.Swift) {
                    UnitDescriptor caster = __instance.Caster;
                    if (((caster != null) ? caster.State.Features.MythicAbilitiesAsMoveAction : null)
                        && caster.Buffs.HasFact(AeonGreaterBaneBuff)
                        && __instance.Blueprint.Type != AbilityType.Physical
                        && caster.Unit.CombatState.HasCooldownForCommand(UnitCommand.CommandType.Swift)
                        && !caster.Unit.CombatState.HasCooldownForCommand(UnitCommand.CommandType.Move)) {
                        __result = UnitCommand.CommandType.Move;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(ActivatableAbility), nameof(ActivatableAbility.IsActivateWithSameCommand), new Type[] { typeof(ActivatableAbility) })]
        static class ActivatableAbility_IsActivateWithSameCommand_Aeon_Patch {
            static void Postfix(ActivatableAbility __instance, ActivatableAbility other, ref bool __result) {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGazeActionSystem")) { return; }
                if (__instance.Blueprint.Group == ActivatableAbilityGroup.AeonGaze && __instance.Blueprint.Group == other.Blueprint.Group) {
                    __result = true;
                }
            }
        }
        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.OnActivatedWithCommand))]
        static class UnitPartActivatableAbility_OnActivatedWithCommand_Aeon_Patch {
            static void Postfix(UnitPartActivatableAbility __instance, ActivatableAbility ability) {
                if (ModSettings.Homebrew.MythicReworks.Aeon.IsDisabled("AeonGazeActionSystem")) { return; }
                int groupSize = __instance.GetGroupSize(ability.Blueprint.Group);
                if (groupSize >= 2 || ability.Blueprint.Group != ActivatableAbilityGroup.Judgment) {
                    UnitPartActivatableAbility.ActivatedWithCommandData activatedWithCommandData =
                        __instance.m_ActivatedWithCommand.FirstItem((UnitPartActivatableAbility.ActivatedWithCommandData i) => i.Ability == ability);
                    if (activatedWithCommandData == null) {
                        activatedWithCommandData = new UnitPartActivatableAbility.ActivatedWithCommandData {
                            Ability = ability
                        };
                        __instance.m_ActivatedWithCommand.Add(activatedWithCommandData);
                    }
                    activatedWithCommandData.Time = Game.Instance.TimeController.GameTime;
                    HashSet<ActivatableAbility> activated = activatedWithCommandData.Activated;
                    if (activated != null) {
                        activated.Clear();
                    }
                    activatedWithCommandData.GroupSize = groupSize;
                }
            }
        }
    }
}
