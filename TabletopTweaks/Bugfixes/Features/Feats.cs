using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class Feats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Feats.DisableAll) { return; }
                Main.LogHeader("Patching Feats");
                PatchCraneWing();
                PatchEndurance();
                PatchFencingGrace();
                PatchSlashingGrace();
            }

            static void PatchCraneWing() {
                if (!ModSettings.Fixes.Feats.Enabled["CraneWing"]) { return; }
                BlueprintBuff CraneStyleBuff = Resources.GetBlueprint<BlueprintBuff>("e8ea7bd10136195478d8a5fc5a44c7da");
                var FightingDefensivlyTrigger = CraneStyleBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>();
                var Conditionals = FightingDefensivlyTrigger.Action.Actions.OfType<Conditional>();

                var newConditonal = Helpers.Create<ContextConditionHasFreeHand>();
                Conditionals.First().ConditionsChecker.Conditions = Conditionals.First().ConditionsChecker.Conditions.AddItem(newConditonal).ToArray();

                Main.LogPatch("Patched", CraneStyleBuff);
            }
            static void PatchFencingGrace() {
                if (!ModSettings.Fixes.Feats.Enabled["FencingGrace"]) { return; }
                var FencingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("47b352ea0f73c354aba777945760b441");
                FencingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceEnforced>());
                Main.LogPatch("Patched", FencingGrace);
            }
            static void PatchSlashingGrace() {
                if (!ModSettings.Fixes.Feats.Enabled["SlashingGrace"]) { return; }
                var SlashingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("697d64669eb2c0543abb9c9b07998a38");
                SlashingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceEnforced>());
                Main.LogPatch("Patched", SlashingGrace);
            }
            static void PatchEndurance() {
                if (!ModSettings.Fixes.Feats.Enabled["Endurance"]) { return; }
                var Endurance = Resources.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174");
                Endurance.SetDescription("Harsh conditions or long exertions do not easily tire you.\nBenefit: You gain +4 bonus on Fortitude " +
                    "{g|Encyclopedia:Saving_Throw}saves{/g} against fatigue and exhaustion and +2 " +
                    "{g|Encyclopedia:Bonus}bonus{/g} on {g|Encyclopedia:Athletics}Athletics checks{/g}. " +
                    "If you have 10 or more ranks in {g|Encyclopedia:Athletics}Athletics{/g}, the bonus increases to +4 for that skill." +
                    "\nYou may sleep in light or medium armor without becoming fatigued.");
                Endurance.RemoveComponents<AddStatBonus>();
                Endurance.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Stat = StatType.SkillAthletics;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus,
                        Value = 2
                    };
                }));
                Endurance.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = StatType.SkillAthletics;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] { 
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                    c.m_StepLevel = 3;
                    c.m_Min = 10;
                    c.m_Max = 20;
                }));
                Main.LogPatch("Patched", Endurance);
            }
        }
    }
}
