using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Lich {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Lich Resources");

                PatchDeathRush();
                PatchSpellbookMerging();

                void PatchDeathRush() {
                    if (TTTContext.Fixes.Lich.IsDisabled("DeathRush")) { return; }

                    var DeathRushFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("ef847913c29a3cf44825eb30ae6f7c38");
                    DeathRushFeature.TemporaryContext(bp => {
                        bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                        bp.AddComponent<AdditionalDiceOnAttack>(c => {
                            c.OnHit = true;
                            c.OnCharge = true;
                            c.InitiatorConditions = new ConditionsChecker();
                            c.TargetConditions = new ConditionsChecker();
                            c.Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                },
                                BonusValue = 0
                            };
                            c.DamageType = new DamageTypeDescription() {
                                Type = DamageType.Direct
                            };
                        });
                        bp.GetComponents<ManeuverTrigger>().ForEach(c => {
                            c.Action = Helpers.CreateActionList(
                                Helpers.Create<ContextActionDealDamageTTT>(a => {
                                    a.DamageType = new DamageTypeDescription() {
                                        Type = DamageType.Direct,
                                        Common = new DamageTypeDescription.CommomData(),
                                        Physical = new DamageTypeDescription.PhysicalData()
                                    };
                                    a.Duration = new ContextDurationValue() {
                                        m_IsExtendable = true,
                                        DiceCountValue = 0,
                                        BonusValue = 0
                                    };
                                    a.Value = new ContextDiceValue() {
                                        DiceType = DiceType.D6,
                                        DiceCountValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        },
                                        BonusValue = new ContextValue()
                                    };
                                    a.IgnoreWeapon = true;
                                    a.IgnoreCritical = true;
                                })
                            );
                        });
                    });
                    TTTContext.Logger.LogPatch("Patched", DeathRushFeature);
                }

                void PatchSpellbookMerging() {

                    if (TTTContext.Fixes.Lich.IsDisabled("SpellbookMerging")) { return; }

                    var LichIncorporateSpellbookFeature = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("3f16e9caf7c683c40884c7c455ed26af");
                    var NatureMageSpellbook = BlueprintTools.GetBlueprint<BlueprintSpellbook>("3ed7e38dc8134af28e1a2b105f74fb7b");

                    LichIncorporateSpellbookFeature.m_AllowedSpellbooks = LichIncorporateSpellbookFeature.m_AllowedSpellbooks
                        .AddItem(NatureMageSpellbook.ToReference<BlueprintSpellbookReference>())
                        .Distinct()
                        .ToArray();

                    TTTContext.Logger.LogPatch("Patched", LichIncorporateSpellbookFeature);
                }
            }
        }
    }
}
