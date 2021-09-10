using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Lich {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Lich Resources");

                PatchDeathRush();
                PatchSpellbookMerging();

                void PatchDeathRush() {
                    if (ModSettings.Fixes.Lich.IsDisabled("DeathRush")) { return; }

                    var DeathRushFeature = Resources.GetBlueprint<BlueprintFeature>("ef847913c29a3cf44825eb30ae6f7c38");
                    DeathRushFeature.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    DeathRushFeature.AddComponent<AdditionalDiceOnAttack>(c => {
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
                    Main.LogPatch("Patched", DeathRushFeature);
                }

                void PatchSpellbookMerging() {

                    if (ModSettings.Fixes.Lich.IsDisabled("SpellbookMerging")) { return; }

                    var LichIncorporateSpellbookFeature = Resources.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("3f16e9caf7c683c40884c7c455ed26af");
                    var ExploiterWizardSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("d09794fb6f93e4a40929a965b434070d");
                    var NatureMageSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("3ed7e38dc8134af28e1a2b105f74fb7b");

                    LichIncorporateSpellbookFeature.m_AllowedSpellbooks = LichIncorporateSpellbookFeature.m_AllowedSpellbooks
                        .AddItem(ExploiterWizardSpellbook.ToReference<BlueprintSpellbookReference>())
                        .AddItem(NatureMageSpellbook.ToReference<BlueprintSpellbookReference>())
                        .Distinct()
                        .ToArray();

                    Main.LogPatch("Patched", LichIncorporateSpellbookFeature);
                }
            }
        }
    }
}
