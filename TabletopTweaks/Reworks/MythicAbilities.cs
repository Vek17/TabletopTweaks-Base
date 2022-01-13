using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Reworks {
    class MythicAbilities {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Reworking Mythic Abilities");
                PatchElementalBarrage();
            }
            static void PatchElementalBarrage() {
                if (ModSettings.Homebrew.MythicAbilities.IsDisabled("ElementalBarrage")) { return; }
                var ElementalBarrage = Resources.GetBlueprint<BlueprintFeature>("da56a1b21032a374783fdf46e1a92adb");
                var ElementalBarrageAcidBuff = Resources.GetBlueprint<BlueprintBuff>("823d33bdb23e7c64d9cc1cce9b78fdea");
                var ElementalBarrageColdBuff = Resources.GetBlueprint<BlueprintBuff>("c5e9031099d3e8d4788d3e51f7ffb8a0");
                var ElementalBarrageElectricityBuff = Resources.GetBlueprint<BlueprintBuff>("0b8ed343b989bbb4c8d059366a7c2d01");
                var ElementalBarrageFireBuff = Resources.GetBlueprint<BlueprintBuff>("7db8ad7b035c2f244951cbef3c9909df");

                ElementalBarrage.SetDescription("You've mastered the art of raining elemental spells on your foes, " +
                    "and found a way to empower them by combining different elements.\n" +
                    "Benefit: Every time you deal elemental damage to a creature with a spell, you apply an elemental mark to it. " +
                    "If during the next three rounds the marked target takes elemental damage from any source " +
                    "with a different element, the target takes additional Divine damage and consume the mark. " +
                    "The damage is 1d6 per mythic rank of your character.");
                ElementalBarrage.GetComponents<AddOutgoingDamageTrigger>().ForEach(c => {
                    c.CheckAbilityType = true;
                    c.m_AbilityType = AbilityType.Spell;
                });
                ElementalBarrage.SetComponents();
                AddOutgoingDamageTrigger(ElementalBarrage, ElementalBarrageAcidBuff, DamageEnergyType.Acid);
                AddOutgoingDamageTrigger(ElementalBarrage, ElementalBarrageColdBuff, DamageEnergyType.Cold);
                AddOutgoingDamageTrigger(ElementalBarrage, ElementalBarrageElectricityBuff, DamageEnergyType.Electricity);
                AddOutgoingDamageTrigger(ElementalBarrage, ElementalBarrageFireBuff, DamageEnergyType.Fire);

                AddIncomingDamageTriggers(ElementalBarrageAcidBuff, DamageEnergyType.Cold, DamageEnergyType.Electricity, DamageEnergyType.Fire);
                AddIncomingDamageTriggers(ElementalBarrageColdBuff, DamageEnergyType.Acid, DamageEnergyType.Electricity, DamageEnergyType.Fire);
                AddIncomingDamageTriggers(ElementalBarrageElectricityBuff, DamageEnergyType.Acid, DamageEnergyType.Cold, DamageEnergyType.Fire);
                AddIncomingDamageTriggers(ElementalBarrageFireBuff, DamageEnergyType.Acid, DamageEnergyType.Cold, DamageEnergyType.Electricity);

                UpdateBuffVisability(ElementalBarrageAcidBuff, "acid");
                UpdateBuffVisability(ElementalBarrageColdBuff, "cold");
                UpdateBuffVisability(ElementalBarrageElectricityBuff, "electricity");
                UpdateBuffVisability(ElementalBarrageFireBuff, "fire");

                Main.LogPatch("Patched", ElementalBarrage);
                void AddOutgoingDamageTrigger(BlueprintFeature barrage, BlueprintBuff barrageBuff, DamageEnergyType trigger) {
                    barrage.AddComponent<AddOutgoingDamageTriggerTTT>(c => {
                        c.IgnoreDamageFromThisFact = true;
                        c.CheckEnergyDamageType = true;
                        c.EnergyType = trigger;
                        c.CheckAbilityType = true;
                        c.ApplyToAreaEffectDamage = true;
                        c.m_AbilityType = AbilityType.Spell;
                        c.Actions = Helpers.CreateActionList(
                            Helpers.Create<ContextActionApplyBuff>(a => {
                                a.m_Buff = barrageBuff.ToReference<BlueprintBuffReference>();
                                a.DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = 3
                                };
                                a.IsNotDispelable = true;
                                a.AsChild = false;
                            })
                        );
                    });
                }
                void UpdateBuffVisability(BlueprintBuff barrageBuff, string element) {
                    barrageBuff.SetDescription($"If this creature takes elemental damage from a " +
                        $"type other than {element} it will take additional damage and consume the mark.");
                    barrageBuff.m_Flags = 0;
                }
                void AddIncomingDamageTriggers(BlueprintBuff barrageBuff, params DamageEnergyType[] triggers) {
                    foreach (var trigger in triggers) {
                        barrageBuff.AddComponent<AddIncomingDamageTrigger>(c => {
                            c.IgnoreDamageFromThisFact = true;
                            c.CheckEnergyDamageType = true;
                            c.EnergyType = trigger;
                            c.Actions = Helpers.CreateActionList(
                                Helpers.Create<ContextActionDealDamageTTT>(a => {
                                    a.DamageType = new DamageTypeDescription() {
                                        Type = DamageType.Energy,
                                        Energy = DamageEnergyType.Divine
                                    };
                                    a.Duration = new ContextDurationValue() {
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = new ContextValue()
                                    };
                                    a.Value = new ContextDiceValue() {
                                        DiceType = DiceType.D6,
                                        DiceCountValue = new ContextValue() {
                                            ValueType = ContextValueType.CasterProperty,
                                            Property = UnitProperty.MythicLevel
                                        },
                                        BonusValue = 0
                                    };
                                    a.IgnoreCritical = true;
                                    a.SetFactAsReason = true;
                                    a.IgnoreWeapon = true;
                                }),
                                Helpers.Create<ContextActionRemoveSelf>()
                            );
                        });
                    }
                }
            }
        }
    }
}
