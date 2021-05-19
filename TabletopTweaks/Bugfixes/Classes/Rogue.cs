using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Clases {
    class Rogue {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Rogue.DisableAll) { return; }
                Main.LogHeader("Patching Rogue");
                PatchBase();
                PatchEldritchScoundrel();
                PatchRowdy();
                PatchMasterOfAll();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Rogue.Base.DisableAll) { return; }
                PatchTrapfinding();

                void PatchTrapfinding() {
                    if (!ModSettings.Fixes.Rogue.Base.Enabled["Trapfinding"]) { return; }
                    var Trapfinding = Resources.GetBlueprint<BlueprintFeature>("dbb6b3bffe6db3547b31c3711653838e");
                    Trapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    Trapfinding.SetDescription("A rogue adds 1/2 her level on {g|Encyclopedia:Perception}Perception checks{/g} and {g|Encyclopedia:Trickery}Trickery checks{/g}.");
                    Main.LogPatch("Patched", Trapfinding);
                }
            }
            static void PatchEldritchScoundrel() {
                if (ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].DisableAll) { return; }
                if (!ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].Enabled["SneakAttack"]) { return; }
                var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                var SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures.AppendToArray(Helpers.LevelEntry(1, SneakAttack));
                Main.LogPatch("Patched", EldritchScoundrelArchetype);
            }
            static void PatchRowdy() {
                if (ModSettings.Fixes.Rogue.Archetypes["Rowdy"].DisableAll) { return; }
                if (!ModSettings.Fixes.Rogue.Archetypes["Rowdy"].Enabled["VitalForce"]) { return; }
                var VitalStrikeAbility = Resources.GetBlueprint<BlueprintAbility>("efc60c91b8e64f244b95c66b270dbd7c");
                var VitalStrikeAbilityImproved = Resources.GetBlueprint<BlueprintAbility>("c714cd636700ac24a91ca3df43326b00");
                var VitalStrikeAbilityGreater = Resources.GetBlueprint<BlueprintAbility>("11f971b6453f74d4594c538e3c88d499");

                AttachRowdyFeature(VitalStrikeAbility);
                AttachRowdyFeature(VitalStrikeAbilityImproved);
                AttachRowdyFeature(VitalStrikeAbilityGreater);

                void AttachRowdyFeature(BlueprintAbility VitalStrike) {
                    var RowdyVitalDamage = Resources.GetBlueprint<BlueprintFeature>("6ce0dd0cd1ef43eda9e62cdf483e05c3");

                    VitalStrike.GetComponent<AbilityCustomMeleeAttack>().m_RowdyFeature = RowdyVitalDamage.ToReference<BlueprintFeatureReference>();
                    Main.LogPatch("Patched", VitalStrike);
                }

                var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                Main.LogPatch("Patched", EldritchScoundrelArchetype);
            }
            static void PatchMasterOfAll() {
                if (ModSettings.Fixes.Rogue.Archetypes["MasterOfAll"].DisableAll) { return; }
                PatchSkillFocus();
                PatchBardicKnowledge();

                void PatchBardicKnowledge() {
                    if (!ModSettings.Fixes.Rogue.Archetypes["MasterOfAll"].Enabled["BardicKnowledge"]) { return; }
                    var RogueClass = Resources.GetBlueprint<BlueprintCharacterClass>("299aa766dee3cbf4790da4efb8c72484");
                    var MasterOfAll = Resources.GetBlueprint<BlueprintArchetype>("bd4e70bfb89a452b876713d61b9b8eb2");
                    var BardicKnowledge = Resources.GetBlueprint<BlueprintFeature>("65cff8410a336654486c98fd3bacd8c5");

                    var RankConfig = BardicKnowledge.GetComponent<ContextRankConfig>();
                    RankConfig.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    RankConfig.m_Class = RankConfig.m_Class.AppendToArray(RogueClass.ToReference<BlueprintCharacterClassReference>());
                    RankConfig.Archetype = MasterOfAll.ToReference<BlueprintArchetypeReference>();

                    Main.LogPatch("Patched", BardicKnowledge);
                }
                void PatchSkillFocus() {
                    if (!ModSettings.Fixes.Rogue.Archetypes["MasterOfAll"].Enabled["SkillFocus"]) { return; }
                    var MasterOfAllSkillFocus = Resources.GetBlueprint<BlueprintFeatureSelection>("f2d2c1702d8a4cc6adfcbd4ebff8eee4");
                    var Adaptability = Resources.GetBlueprint<BlueprintFeatureSelection>("26a668c5a8c22354bac67bcd42e09a3f");
                    MasterOfAllSkillFocus.IsClassFeature = true;
                    MasterOfAllSkillFocus.m_Features = Adaptability.m_Features;
                    MasterOfAllSkillFocus.m_AllFeatures = Adaptability.m_AllFeatures;

                    Main.LogPatch("Patched", MasterOfAllSkillFocus);
                }
            }
        }
        [HarmonyPatch(typeof(AbilityCustomMeleeAttack.VitalStrike), "OnEventDidTrigger", new Type[] { typeof(RuleCalculateWeaponStats) })]
        static class VitalStrike_OnEventDidTrigger_Rowdy_Patch {

            static bool Prefix(AbilityCustomMeleeAttack.VitalStrike __instance, RuleCalculateWeaponStats evt) {
                if (ModSettings.Fixes.Rogue.Archetypes["Rowdy"].DisableAll) { return true; }
                if (!ModSettings.Fixes.Rogue.Archetypes["Rowdy"].Enabled["VitalForce"]) { return true; }

                DamageDescription damageDescription = evt.DamageDescription.FirstItem();
                if (damageDescription != null && damageDescription.TypeDescription.Type == DamageType.Physical) {
                    damageDescription.Dice = new DiceFormula(damageDescription.Dice.Rolls * __instance.m_DamageMod, damageDescription.Dice.Dice);
                    if (__instance.m_Mythic) {
                        damageDescription.Bonus *= __instance.m_DamageMod;
                    }
                    if (__instance.m_Rowdy) {
                        DamageDescription damageDescription2 = new DamageDescription {
                            TypeDescription = evt.DamageDescription.FirstItem().TypeDescription,
                            Dice = new DiceFormula(evt.Initiator.Descriptor.Stats.SneakAttack * 2, DiceType.D6),

                        };
                        evt.DamageDescription.Add(damageDescription2);
                    }
                }
                return false;
            }
        }
    }
}
