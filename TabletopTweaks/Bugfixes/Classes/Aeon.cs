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
    class Aeon {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Aeon Resources");
                
                PatchAeonBaneUses();
                PatchAeonGreaterBane();
            }
            static void PatchAeonBaneUses() {
                if (ModSettings.Fixes.Aeon.IsDisabled("AeonBaneUses"])){ return; }
                var AeonClass = Resources.GetBlueprint<BlueprintCharacterClass>("15a85e67b7d69554cab9ed5830d0268e");
                var AeonBaneFeature = Resources.GetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
                var AeonBaneIncreaseResourceFeature = Resources.GetModBlueprint<BlueprintFeature>("AeonBaneIncreaseResourceFeature");
                AeonBaneFeature.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = AeonBaneIncreaseResourceFeature.ToReference<BlueprintFeatureReference>();
                }));
                Main.LogPatch("Patched", AeonBaneFeature);
            }
            static void PatchAeonGreaterBane() {
                if (ModSettings.Fixes.Aeon.IsDisabled("AeonGreaterBane")) { return; }
                var AeonGreaterBaneBuff = Resources.GetBlueprint<BlueprintBuff>("cdcc13884252b2c4d8dac57cb5f46555");
                AeonGreaterBaneBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>(c => c.Action.Actions.OfType<ContextActionDealDamage>().Any());
                AeonGreaterBaneBuff.AddComponent(Helpers.Create<AdditionalDiceOnAttack>(c => {
                    c.OnHit = true;
                    c.InitiatorConditions = new ConditionsChecker();
                    c.TargetConditions = new ConditionsChecker();
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D6,
                        DiceCountValue = 2,
                        BonusValue = 0
                    };
                    c.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Divine
                    };
                }));
                Main.LogPatch("Patched", AeonGreaterBaneBuff);
            }
        }
    }
}
