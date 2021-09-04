using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
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
                PatchIndomitableMount();
                PatchMountedCombat();
                PatchPersistantMetamagic();
                PatchSlashingGrace();
                PatchSpiritedCharge();
                PatchWeaponFinesse();
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
            static void PatchMountedCombat() {
                if (!ModSettings.Fixes.Feats.Enabled["MountedCombat"]) { return; }

                var MountedCombat = Resources.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
                var MountedCombatCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
                MountedCombat.AddComponent(Helpers.Create<MountedCombatFixed>(c => {
                    c.m_CooldownBuff = MountedCombatCooldownBuff.ToReference<BlueprintBuffReference>();
                }));
                Main.LogPatch("Patched", MountedCombat);
            }
            static void PatchIndomitableMount() {
                if (!ModSettings.Fixes.Feats.Enabled["IndomitableMount"]) { return; }

                var IndomitableMount = Resources.GetBlueprint<BlueprintFeature>("68e814f1f3ce55942a52c1dd536eaa5b");
                var IndomitableMountCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("34762bab68ec86c45a15884b9a9929fc");
                IndomitableMount.AddComponent(Helpers.Create<IndomitableMountFixed>(c => {
                    c.m_CooldownBuff = IndomitableMountCooldownBuff.ToReference<BlueprintBuffReference>();
                }));
                Main.LogPatch("Patched", IndomitableMount);
            }
            static void PatchPersistantMetamagic() {
                if (!ModSettings.Fixes.Feats.Enabled["PersistantMetamagic"]) { return; }

                var PersistentSpellFeat = Resources.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
                var spells = SpellTools.SpellList.AllSpellLists
                    .SelectMany(list => list.SpellsByLevel.SelectMany(level => level.Spells))
                    .Distinct()
                    .OrderBy(spell => spell.Name)
                    .ToArray();
                Main.LogPatch("Enabling", PersistentSpellFeat);
                foreach (var spell in spells) {
                    bool HasSavingThrow = spell.FlattenAllActions().OfType<ContextActionSavingThrow>().Any();
                    if ((spell?.GetComponent<AbilityEffectRunAction>()?.SavingThrowType ?? SavingThrowType.Unknown) != SavingThrowType.Unknown || HasSavingThrow) {
                        spell.AvailableMetamagic |= Metamagic.Persistent;
                        Main.LogPatch("Enabled Persistant Metamagic", spell);
                    };
                }
            }
            static void PatchSpiritedCharge() {
                if (!ModSettings.Fixes.Feats.Enabled["SpiritedCharge"]) { return; }

                var ChargeBuff = Resources.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                var SpiritedCharge = Resources.GetBlueprint<BlueprintFeature>("95ef0ff14771f2549897f300ce62c95c");
                var SpiritedChargeBuff = Resources.GetBlueprint<BlueprintBuff>("5a191fc6731bd4845bbbcc8ff3ff4c1d");

                SpiritedCharge.RemoveComponents<BuffExtraEffects>();
                SpiritedCharge.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                    c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                    c.CheckFacts = true;
                    c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff.ToReference<BlueprintUnitFactReference>() };
                    c.ExtraEffectBuff = SpiritedChargeBuff.ToReference<BlueprintBuffReference>();
                }));
                SpiritedChargeBuff.RemoveComponents<AddOutgoingDamageBonus>();
                SpiritedChargeBuff.AddComponent(Helpers.Create<AddOutgoingWeaponDamageBonus>(c => {
                    c.BonusDamageMultiplier = 1;
                }));
                SpiritedChargeBuff.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
                Main.LogPatch("Patched", SpiritedCharge);
                Main.LogPatch("Patched", SpiritedChargeBuff);
            }
            static void PatchWeaponFinesse() {
                if (!ModSettings.Fixes.Feats.Enabled["WeaponFinesse"]) { return; }

                var WeaponFinesse = Resources.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");

                WeaponFinesse.ReplaceComponents<AttackStatReplacement>(Helpers.Create<AttackStatReplacementEnforced>(c => {
                    c.ReplacementStat = StatType.Dexterity;
                    c.SubCategory = WeaponSubCategory.Finessable;
                }));
                Main.LogPatch("Patched", WeaponFinesse);
            }
        }
    }
}
