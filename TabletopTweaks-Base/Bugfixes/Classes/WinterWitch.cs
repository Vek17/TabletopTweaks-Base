using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class WinterWitch {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Winter Witch Resources");
            }
        }

        [HarmonyPatch(typeof(ChangeSpellElementalDamageHalfUntyped), "OnEventAboutToTrigger", new[] { typeof(RulePrepareDamage) })]
        static class ModifierDescriptorHelper_IsStackable_Patch {

            static bool  Prefix(ChangeSpellElementalDamageHalfUntyped __instance, RulePrepareDamage evt) {
                if (TTTContext.Fixes.WinterWitch.IsDisabled("UnearthlyCold")) { return true; }

                AbilityData ability = evt.Reason.Ability;
                if (ability is null || !ability.Blueprint.IsSpell) {
                    return false;
                }
                foreach (BaseDamage baseDamage in evt.DamageBundle) {
                    if (baseDamage.Type == DamageType.Energy) {
                        EnergyDamage energyDamage = baseDamage as EnergyDamage;
                        if (energyDamage is null) { continue; }
                        if (energyDamage.EnergyType == __instance.Element && !baseDamage.Precision) {
                            DirectDamage directDamage = new DirectDamage(baseDamage.Dice.ModifiedValue, 0);
                            baseDamage.Half.Set(true, __instance.Fact);
                            
                            directDamage.CopyFrom(baseDamage);
                            directDamage.SourceFact = __instance.Fact;
                            evt.Add(directDamage);
                        }
                    }
                }
                return false;
            }
        }
    }
}
