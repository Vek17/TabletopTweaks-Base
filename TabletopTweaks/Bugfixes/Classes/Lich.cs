using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

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
            }
        }
    }
}
