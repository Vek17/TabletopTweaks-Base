using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.Bugfixes.Classes {
    class Angel {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Angel Resources");

                var AngelSwordEffectBuff = Resources.GetBlueprint<BlueprintBuff>("f5f500d6a2a39fc4181af32ad79af488");

                PatchAngelCloak();
                PatchHeavenlyHost();

                void PatchAngelCloak() {
                    if (ModSettings.Fixes.Angel.IsDisabled("MythicCloak")) { return; }

                    var AngelCloakEnchantment = Resources.GetBlueprint<BlueprintEquipmentEnchantment>("10b62f56302c49c887d53787948c5cda");
                    var AngelCloakFeature = Resources.GetBlueprint<BlueprintFeature>("58b832df3c4a4ccba1f0c64138ca95fc");
                    var AngelSwordEnchantment = Resources.GetBlueprint<BlueprintWeaponEnchantment>("8a24f0fa51f3a2843be5aec58befefb6");

                    if (AngelCloakEnchantment.GetComponent<AddUnitFeatureEquipment>() is AddUnitFeatureEquipment effect) {
                        // replace aeon feature with angel one
                        effect.m_Feature = AngelCloakFeature.ToReference<BlueprintFeatureReference>();
                    }

                    if (AngelSwordEffectBuff.GetComponent<AngelSwordAdditionalDamageAndHeal>() is AngelSwordAdditionalDamageAndHeal handler) {
                        // replace aeon feature with angel one
                        handler.m_CloakFact = AngelCloakFeature.ToReference<BlueprintUnitFactReference>();
                    }

                    if (AngelSwordEnchantment.GetComponent<WeaponAngelDamageDice>() is WeaponAngelDamageDice enchantment) {
                        // apply correct angel cloak dice to weapon attacks
                        AngelSwordEnchantment.RemoveComponent(enchantment);
                        AngelSwordEnchantment.AddComponent(new WeaponAngelDamageDiceWithCloak() {
                            OwnerBlueprint = AngelSwordEffectBuff,
                            name = "$WeaponAngelDamageDiceWithCloak$7d3a258c-c2fa-4661-a454-cecf60f89e43",
                            m_CloakFact = AngelCloakFeature.ToReference<BlueprintUnitFactReference>(),
                            m_MaximizeFact = enchantment.m_MaximizeFeature,
                            Element = enchantment.Element,
                            EnergyDamageDice = enchantment.EnergyDamageDice,
                            m_PrototypeLink = enchantment.m_PrototypeLink,
                            m_Flags = enchantment.m_Flags
                        });
                    }

                    Main.LogPatch("Patched", AngelCloakFeature);
                }

                void PatchHeavenlyHost() {
                    if (ModSettings.Fixes.Angel.IsDisabled("HeavenlyHost")) { return; }

                    var AngelSwordDepersonalizeFeature = Resources.GetBlueprint<BlueprintFeature>("432c96835aee50341a4ff0b364cdb85a");
                    var AngelSwordHeavenlyHostFeature = Resources.GetBlueprint<BlueprintFeature>("131fc914b66b62c49b9f9b5f750998f5");
                    var AngelSwordSpeedOfLightFeature = Resources.GetBlueprint<BlueprintFeature>("826e402bae1aca242bfe9919da8d94d7");
                    var AngelSwordOverwhelmingFlamesFeature = Resources.GetBlueprint<BlueprintFeature>("477fcaa2b00a1964da43472ddbc3c7de");
                    var AngelSwordSpeedOfLightBuff = Resources.GetBlueprint<BlueprintBuff>("58d3b0b98ce4f9346b3c1fb4c7dbc9bf");
                    var AngelSwordOverwhelmingFlamesBuff = Resources.GetBlueprint<BlueprintBuff>("685c8db84fcaf8045b140bf7bc5ad49f");
                    var AngelSwordAbility = Resources.GetBlueprint<BlueprintAbility>("9efd605503f248a428df32e20b3152a6");
                    var AngelSwordAoEAbility = Resources.GetBlueprint<BlueprintAbility>("736cffbe95b994a4d86b0caad6e310fa");

                    if (AngelSwordAbility.GetComponent<AbilityEffectRunAction>() is AbilityEffectRunAction effect) {
                        // stop the single-target buff from applying when Heavenly Host is active
                        effect.Actions.ReplaceAction("353a1f2d-6a13-48b4-a35c-129f73498ff6", applyUniqueBuff => {
                            return new Conditional() {
                                Owner = AngelSwordAbility,
                                name = "$Conditional$86bcdba7-eb16-4d58-811b-4d44373a34f8",
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionCasterHasFact() {
                                            name = "$ContextConditionCasterHasFact$b310e648-816c-47ff-93ae-79481d5d67ce",
                                            m_Fact = AngelSwordHeavenlyHostFeature.ToReference<BlueprintUnitFactReference>(),
                                            Not = true
                                        }
                                    }
                                },
                                IfTrue = Helpers.CreateActionList(applyUniqueBuff),
                                IfFalse = Helpers.CreateActionList()
                            };
                        });
                    }

                    // duplicate sword buff, without UniqueBuff restriction
                    // (an unused AngelSwordEffectNonUniqueBuff exists, but is actually unique)
                    var AngelSwordEffectAoEBuff = Helpers.Create<BlueprintBuff>(bp => {
                        bp.Become(AngelSwordEffectBuff);
                        bp.name = "AngelSwordEffectAoEBuff";
                        bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                        bp.RemoveComponents<UniqueBuff>();
                        Resources.AddBlueprint(bp);
                    });

                    if (AngelSwordAoEAbility.GetComponent<AbilityEffectRunAction>() is AbilityEffectRunAction effects) {
                        // replace aoe ability's unique buff with non-unique buff
                        foreach (var action in effects.Actions.GetAllConditionalActions().OfType<ContextActionApplyBuff>()) {
                            action.m_Buff = AngelSwordEffectAoEBuff.ToReference<BlueprintBuffReference>();
                        }
                    }

                    // allow targeting party
                    AngelSwordAoEAbility.CanTargetFriends = true;

                    // TODO: mythic self harm should target caster, not target

                    // apply Speed of Light to aoe buff
                    if (AngelSwordSpeedOfLightFeature.GetComponent<BuffExtraEffects>() is BuffExtraEffects speedOfLightExtraEffects) {
                        var aoeExtraEffects = Helpers.CreateCopy(speedOfLightExtraEffects, bp => {
                            bp.name = "AngelSwordSpeedOfLightExtraEffects";
                            bp.m_CheckedBuff = AngelSwordEffectAoEBuff.ToReference<BlueprintBuffReference>();
                        });
                        AngelSwordSpeedOfLightFeature.AddComponent(aoeExtraEffects);
                    }

                    // apply Overwhelming Flames to aoe buff
                    if (AngelSwordOverwhelmingFlamesFeature.GetComponent<BuffExtraEffects>() is BuffExtraEffects overwhelmingFlamesExtraEffects) {
                        var aoeExtraEffects = Helpers.CreateCopy(overwhelmingFlamesExtraEffects, bp => {
                            bp.name = "AngelSwordOverwhelmingFlamesExtraEffects";
                            bp.m_CheckedBuff = AngelSwordEffectAoEBuff.ToReference<BlueprintBuffReference>();
                        });
                        AngelSwordOverwhelmingFlamesFeature.AddComponent(aoeExtraEffects);
                    }

                    // BuffExtraEffects only applies to owner's buffs
                    // convert to component which augments buffs cast by owner
                    ConvertToCasterExtraEffects(AngelSwordSpeedOfLightFeature);
                    ConvertToCasterExtraEffects(AngelSwordOverwhelmingFlamesFeature);

                    Main.LogPatch("Patched", AngelSwordHeavenlyHostFeature);
                }

                void ConvertToCasterExtraEffects(BlueprintScriptableObject feature) {
                    foreach (var targetExtraEffects in feature.GetComponents<BuffExtraEffects>().ToList()) {
                        var casterExtraEffects = new BuffCasterExtraEffects() {
                            name = targetExtraEffects.name.Replace("BuffExtraEffects", "BuffCasterExtraEffects"),
                            m_CheckedBuff = targetExtraEffects.m_CheckedBuff,
                            m_ExtraEffectBuff = targetExtraEffects.m_ExtraEffectBuff
                        };

                        feature.RemoveComponent(targetExtraEffects);
                        feature.AddComponent(casterExtraEffects);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AngelSwordAdditionalDamageAndHeal), "OnEventAboutToTrigger", argumentTypes: new[] { typeof(RuleHealDamage) })]
        static class AngelSwordAdditionalDamageAndHeal_RuleCalculateDamage_Patch {
            static bool Prefix(AngelSwordAdditionalDamageAndHeal __instance, RuleHealDamage evt) {
                if (ModSettings.Fixes.Angel.IsDisabled("MythicCloak")) { return true; }

                var count = __instance.Context.MaybeCaster?.HasFact(__instance.CloakFact) ?? false ? 4 : 2;

                if (__instance.Context.MaybeCaster?.HasFact(__instance.MaximizeFact) ?? false) {
                    evt.AdditionalBonus += new DiceFormula(count, evt.HealFormula.Dice).MaxValue(0);
                } else {
                    evt.BonusDice = count;
                }

                __instance.SpawnPrefab(evt.Target, __instance.HealingPrefab);

                return false;
            }
        }

        [TypeId("1bea9fa7b11c4453bc5e0bd231fa9e67")]
        class WeaponAngelDamageDiceWithCloak : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleCalculateWeaponStats> {
            public DiceFormula EnergyDamageDice;
            public DamageEnergyType Element;

            [SerializeField]
            public BlueprintUnitFactReference m_MaximizeFact;

            [SerializeField]
            public BlueprintUnitFactReference m_CloakFact;

            public BlueprintUnitFact MaximizeFact => m_MaximizeFact?.Get();

            public BlueprintUnitFact CloakFact => m_CloakFact?.Get();

            public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
                if (evt.Weapon != Owner) {
                    return;
                }

                var bonus = 0;
                var roll = EnergyDamageDice;

                if (Context.MaybeCaster?.HasFact(CloakFact) ?? false) {
                    roll = new DiceFormula(4, roll.Dice);
                }
                if (Context.MaybeCaster?.HasFact(MaximizeFact) ?? false) {
                    bonus = roll.MaxValue(0);
                    roll = DiceFormula.Zero;
                }

                evt.DamageDescription.Add(new DamageDescription {
                    TypeDescription = new DamageTypeDescription {
                        Type = DamageType.Energy,
                        Energy = Element
                    },
                    Dice = roll,
                    Bonus = bonus,
                    SourceFact = Fact
                });
            }

            public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
            }
        }

        [AllowMultipleComponents]
        [ComponentName("Add extra buff to casted buff")]
        [AllowedOn(typeof(BlueprintFeature), false)]
        [AllowedOn(typeof(BlueprintBuff), false)]
        [TypeId("88bb245b300f4dc3bc62b8826d810214")]
        class BuffCasterExtraEffects : UnitFactComponentDelegate<BuffCasterExtraEffects.AppliedBuffsData>, IUnitBuffHandler {
            [AllowMultipleComponents]
            public class AppliedBuffsData {
                [JsonProperty]
                public Dictionary<string, Buff> AppliedBuffs = new Dictionary<string, Buff>();
            }

            [SerializeField]
            [FormerlySerializedAs("CheckedBuff")]
            internal BlueprintBuffReference m_CheckedBuff;

            [SerializeField]
            [FormerlySerializedAs("ExtraEffectBuff")]
            internal BlueprintBuffReference m_ExtraEffectBuff;

            public BlueprintBuff CheckedBuff => m_CheckedBuff?.Get();

            public BlueprintBuff ExtraEffectBuff => m_ExtraEffectBuff?.Get();

            public void HandleBuffDidAdded(Buff buff) {
                if (buff.Blueprint != CheckedBuff) {
                    // wrong buff
                    return;
                }
                var caster = buff.Context.MaybeCaster;
                var target = buff.Owner.Unit;
                if (caster != Owner) {
                    // wrong caster
                    return;
                }
                if (Data.AppliedBuffs.TryGetValue(target.UniqueId, out _)) {
                    // extra effect already applied
                    return;
                }

                // apply extra effects; track for later removal
                var applied = target.AddBuff(ExtraEffectBuff, Context);
                Data.AppliedBuffs.Add(target.UniqueId, applied);
            }

            public void HandleBuffDidRemoved(Buff buff) {
                if (buff.Blueprint != CheckedBuff) {
                    // wrong buff
                    return;
                }
                var caster = buff.Context.MaybeCaster;
                var target = buff.Owner.Unit;
                if (caster != Owner) {
                    // wrong caster
                    return;
                }
                if (!Data.AppliedBuffs.TryGetValue(target.UniqueId, out var applied)) {
                    // didn't apply this buff; no extra effects to remove
                    return;
                }

                // TODO: test after savegame load

                // remove and cleanup extra effects
                applied.Remove();
                Data.AppliedBuffs.Remove(target.UniqueId);
            }
        }
    }
}
