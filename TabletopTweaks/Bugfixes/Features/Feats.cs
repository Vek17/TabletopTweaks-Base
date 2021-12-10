using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class Feats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Feats");
                PatchCraneWing();
                PatchEndurance();
                PatchFencingGrace();
                PatchIndomitableMount();
                PatchMountedCombat();
                PatchPersistantMetamagic();
                PatchBolsteredMetamagic();
                PatchEmpowerMetamagic();
                PatchMaximizeMetamagic();
                PatchShatterDefenses();
                PatchSlashingGrace();
                PatchSpellSpecialization();
                PatchSpiritedCharge();
                PatchWeaponFinesse();
                PatchMagicalTail();
                PatchLunge();
                PatchSelectiveMetamagic();
                PatchSelectiveMetamagicPrerequisites();
            }
            static void PatchMagicalTail() {
                if (ModSettings.Fixes.Feats.IsDisabled("MagicalTail")) { return; }

                BlueprintFeature magicalTail1 = Resources.GetBlueprint<BlueprintFeature>("5114829572da5a04f896a8c5b67be413");
                BlueprintFeature magicalTail2 = Resources.GetBlueprint<BlueprintFeature>("c032f65c0bd9f6048a927fb07fc0195d"); // Abilities change for this one
                BlueprintFeature magicalTail3 = Resources.GetBlueprint<BlueprintFeature>("d5050e13742d9b64da20921aaf7c2b2a");
                BlueprintFeature magicalTail4 = Resources.GetBlueprint<BlueprintFeature>("342b6aed6b2eaab4786de243f0bcbcb8");
                BlueprintFeature magicalTail5 = Resources.GetBlueprint<BlueprintFeature>("044cd84818c36854abf61064ade542a1"); // Abilites change for this one
                BlueprintFeature magicalTail6 = Resources.GetBlueprint<BlueprintFeature>("053e37697a0d20547b06c3dbd8b71702");
                BlueprintFeature magicalTail7 = Resources.GetBlueprint<BlueprintFeature>("041f91c25586d48469dce6b4575053f6");
                BlueprintFeature magicalTail8 = Resources.GetBlueprint<BlueprintFeature>("df186ef345849d149bdbf4ddb45aee35");

                var hideousLaughterKitsune = Resources.GetModBlueprint<BlueprintAbility>("HideousLaughterKitsune");
                var heroismKitsune = Resources.GetModBlueprint<BlueprintAbility>("HeroismKitsune");

                var magicalTailDescription = "You gain a new {g|Encyclopedia:Special_Abilities}spell-like ability{/g}, each usable twice per day," +
                                             " from the following list, in order:\n1. vanish\n2. hideous laughter\n3. blur\n4. invisibility\n5. heroism\n6." +
                                             " displacement\n7. confusion\n8. dominate person.\nFor example, the first time you select this {g|Encyclopedia:Feat}feat{/g}," +
                                             " you gain vanish 2/day; the second time you select this feat, you gain hideous laughter 2/day. Your {g|Encyclopedia:Caster_Level}caster level{/g}" +
                                             " for these {g|Encyclopedia:Spell}spells{/g} is equal to your {g|Encyclopedia:Hit_Dice}Hit Dice{/g}. The DCs for these abilities are" +
                                             " {g|Encyclopedia:Charisma}Charisma{/g}-based.\nYou may select this feat up to eight times. Each time you take it, you gain an additional ability as described above.";

                magicalTail1.SetDescription(magicalTailDescription);
                magicalTail2.SetDescription(magicalTailDescription);
                magicalTail3.SetDescription(magicalTailDescription);
                magicalTail4.SetDescription(magicalTailDescription);
                magicalTail5.SetDescription(magicalTailDescription);
                magicalTail6.SetDescription(magicalTailDescription);
                magicalTail7.SetDescription(magicalTailDescription);
                magicalTail8.SetDescription(magicalTailDescription);

                BlueprintFeature v = new BlueprintFeature();

                magicalTail2.RemoveComponents<AddFacts>();

                magicalTail2.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    hideousLaughterKitsune.ToReference<BlueprintUnitFactReference>()
                };
                });

                magicalTail5.RemoveComponents<AddFacts>();

                magicalTail5.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    heroismKitsune.ToReference<BlueprintUnitFactReference>()
                };
                });

                Main.LogPatch("Patched", magicalTail1);
                Main.LogPatch("Patched", magicalTail2);
                Main.LogPatch("Patched", magicalTail3);
                Main.LogPatch("Patched", magicalTail4);
                Main.LogPatch("Patched", magicalTail5);
                Main.LogPatch("Patched", magicalTail6);
                Main.LogPatch("Patched", magicalTail7);
                Main.LogPatch("Patched", magicalTail8);
            }

            static void PatchCraneWing() {
                if (ModSettings.Fixes.Feats.IsDisabled("CraneWing")) { return; }

                BlueprintBuff CraneStyleBuff = Resources.GetBlueprint<BlueprintBuff>("e8ea7bd10136195478d8a5fc5a44c7da");
                var FightingDefensivlyTrigger = CraneStyleBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>();
                var Conditionals = FightingDefensivlyTrigger.Action.Actions.OfType<Conditional>();

                var newConditonal = Helpers.Create<ContextConditionHasFreeHand>();
                Conditionals.First().ConditionsChecker.Conditions = Conditionals.First().ConditionsChecker.Conditions.AddItem(newConditonal).ToArray();

                Main.LogPatch("Patched", CraneStyleBuff);
            }
            static void PatchFencingGrace() {
                if (ModSettings.Fixes.Feats.IsDisabled("FencingGrace")) { return; }

                var FencingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("47b352ea0f73c354aba777945760b441");
                FencingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceTTT>());
                Main.LogPatch("Patched", FencingGrace);
            }
            static void PatchSlashingGrace() {
                if (ModSettings.Fixes.Feats.IsDisabled("SlashingGrace")) { return; }

                var SlashingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("697d64669eb2c0543abb9c9b07998a38");
                SlashingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceTTT>());
                Main.LogPatch("Patched", SlashingGrace);
            }
            static void PatchEndurance() {
                if (ModSettings.Fixes.Feats.IsDisabled("Endurance")) { return; }
                var Endurance = Resources.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174");
                Endurance.SetDescription("Harsh conditions or long exertions do not easily tire you.\nBenefit: You gain +4 bonus on Fortitude " +
                    "saves against fatigue and exhaustion and +2 " +
                    "bonus on Athletics checks. If you have 10 or more ranks in Athletics, the bonus increases to +4 for that skill." +
                    "\nYou may sleep in light or medium armor without becoming fatigued.");
                Endurance.RemoveComponents<AddStatBonus>();
                Endurance.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SkillAthletics;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus,
                        Value = 2
                    };
                });
                Endurance.AddContextRankConfig(c => {
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
                });
                Main.LogPatch("Patched", Endurance);
            }
            static void PatchMountedCombat() {
                if (ModSettings.Fixes.Feats.IsDisabled("MountedCombat")) { return; }

                var MountedCombatBuff = Resources.GetBlueprint<BlueprintBuff>("5008df9965da43c593c98ed7e6cacfc6");
                var MountedCombatCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
                var TrickRiding = Resources.GetBlueprint<BlueprintFeature>("5008df9965da43c593c98ed7e6cacfc6");
                var TrickRidingCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
                MountedCombatBuff.RemoveComponents<MountedCombat>();
                MountedCombatBuff.RemoveComponents<MountedCombatTTT>();
                MountedCombatBuff.AddComponent<MountedCombatTTT>(c => {
                    c.m_CooldownBuff = MountedCombatCooldownBuff.ToReference<BlueprintBuffReference>();
                    c.m_TrickRidingCooldownBuff = TrickRidingCooldownBuff.ToReference<BlueprintBuffReference>();
                    c.m_TrickRidingFeature = TrickRiding.ToReference<BlueprintFeatureReference>();
                });
                Main.LogPatch("Patched", MountedCombatBuff);
            }
            static void PatchIndomitableMount() {
                if (ModSettings.Fixes.Feats.IsDisabled("IndomitableMount")) { return; }

                var IndomitableMount = Resources.GetBlueprint<BlueprintFeature>("68e814f1f3ce55942a52c1dd536eaa5b");
                var IndomitableMountCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("34762bab68ec86c45a15884b9a9929fc");
                IndomitableMount.RemoveComponents<IndomitableMount>();
                IndomitableMount.AddComponent<IndomitableMountTTT>(c => {
                    c.m_CooldownBuff = IndomitableMountCooldownBuff.ToReference<BlueprintBuffReference>();
                });
                Main.LogPatch("Patched", IndomitableMount);
            }
            static void PatchPersistantMetamagic() {
                if (ModSettings.Fixes.Feats.IsDisabled("PersistantMetamagic")) { return; }

                var PersistentSpellFeat = Resources.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
                var spells = SpellTools.SpellList.AllSpellLists
                    .Where(list => !list.IsMythic)
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .OrderBy(spell => spell.Name)
                    .ToArray();
                Main.LogPatch("Enabling", PersistentSpellFeat);
                foreach (var spell in spells) {
                    bool HasSavingThrow = spell.FlattenAllActions().OfType<ContextActionSavingThrow>().Any();
                    if ((spell?.GetComponent<AbilityEffectRunAction>()?.SavingThrowType ?? SavingThrowType.Unknown) != SavingThrowType.Unknown || HasSavingThrow) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Persistent)) {
                            spell.AvailableMetamagic |= Metamagic.Persistent;
                            Main.LogPatch("Enabled Persistant Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchBolsteredMetamagic() {
                if (ModSettings.Fixes.Feats.IsDisabled("BolsteredMetamagic")) { return; }

                var BolsteredSpellFeat = Resources.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");
                var spells = SpellTools.SpellList.AllSpellLists
                    .Where(list => !list.IsMythic)
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .OrderBy(spell => spell.Name)
                    .ToArray();
                Main.LogPatch("Enabling", BolsteredSpellFeat);
                foreach (var spell in spells) {
                    bool dealsDamage = spell.FlattenAllActions()
                        .OfType<ContextActionDealDamage>().Any()
                        || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                        .TouchDeliveryAbility?
                        .FlattenAllActions()?
                        .OfType<ContextActionDealDamage>()?
                        .Any() ?? false);
                    if (dealsDamage) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Bolstered)) {
                            spell.AvailableMetamagic |= Metamagic.Bolstered;
                            Main.LogPatch("Enabled Bolstered Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchEmpowerMetamagic() {
                if (ModSettings.Fixes.Feats.IsDisabled("EmpowerMetamagic")) { return; }

                var EmpowerSpellFeat = Resources.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
                var spells = SpellTools.SpellList.AllSpellLists
                    .Where(list => !list.IsMythic)
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .OrderBy(spell => spell.Name)
                    .ToArray();
                Main.LogPatch("Enabling", EmpowerSpellFeat);
                foreach (var spell in spells) {
                    bool dealsDamage = spell.FlattenAllActions()
                        .OfType<ContextActionDealDamage>().Any()
                        || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                        .TouchDeliveryAbility?
                        .FlattenAllActions()?
                        .OfType<ContextActionDealDamage>()?
                        .Any() ?? false);
                    if (dealsDamage) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Empower)) {
                            spell.AvailableMetamagic |= Metamagic.Empower;
                            Main.LogPatch("Enabled Empower Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchMaximizeMetamagic() {
                if (ModSettings.Fixes.Feats.IsDisabled("MaximizeMetamagic")) { return; }

                var MaximizeSpellFeat = Resources.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
                var spells = SpellTools.SpellList.AllSpellLists
                    .Where(list => !list.IsMythic)
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .OrderBy(spell => spell.Name)
                    .ToArray();
                Main.LogPatch("Enabling", MaximizeSpellFeat);
                foreach (var spell in spells) {
                    bool dealsDamage = spell.FlattenAllActions()
                        .OfType<ContextActionDealDamage>().Any()
                        || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                        .TouchDeliveryAbility?
                        .FlattenAllActions()?
                        .OfType<ContextActionDealDamage>()?
                        .Any() ?? false);
                    if (dealsDamage) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Maximize)) {
                            spell.AvailableMetamagic |= Metamagic.Maximize;
                            Main.LogPatch("Enabled Maximize Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchSelectiveMetamagic() {
                if (ModSettings.Fixes.Feats.IsDisabled("SelectiveMetamagic")) { return; }

                var SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
                var spells = SpellTools.SpellList.AllSpellLists
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .OrderBy(spell => spell.Name)
                    .ToArray();
                Main.LogPatch("Updating", SelectiveSpellFeat);
                foreach (var spell in spells) {
                    bool isAoE = spell.AbilityAndVariants().Any(v => v.GetComponent<AbilityTargetsAround>());
                    isAoE |= spell.AbilityAndVariants().Any(v => v.GetComponent<AbilityDeliverProjectile>()?.Type == AbilityProjectileType.Cone 
                        || v.GetComponent<AbilityDeliverProjectile>()?.Type == AbilityProjectileType.Line);
                    if (isAoE) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Selective)) {
                            spell.AvailableMetamagic |= Metamagic.Selective;
                            Main.LogPatch("Enabled Selective Metamagic", spell);
                        }
                    } else {
                        if (spell.AvailableMetamagic.HasMetamagic(Metamagic.Selective)) {
                            spell.AvailableMetamagic &= ~Metamagic.Selective;
                            Main.LogPatch("Disabled Selective Metamagic", spell);
                        }
                    }
                }
            }
            static void PatchShatterDefenses() {
                if (ModSettings.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }

                var ShatterDefenses = Resources.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");
                var ShatterDefensesBuff = Resources.GetModBlueprint<BlueprintBuff>("ShatterDefensesBuff");
                var ShatterDefensesMythicFeat = Resources.GetModBlueprint<BlueprintFeature>("ShatterDefensesMythicFeat");
                var ShatterDefensesMythicBuff = Resources.GetModBlueprint<BlueprintBuff>("ShatterDefensesMythicBuff");

                ShatterDefenses.RemoveComponents<AddMechanicsFeature>();
                ShatterDefenses.RemoveComponents<AddFacts>();
                ShatterDefenses.AddComponent<ShatterDefensesInitiator>(c => {
                    c.Action = Helpers.CreateActionList(
                        new Conditional {
                            ConditionsChecker = new ConditionsChecker {
                                Conditions = new Condition[] {
                                    new ContextConditionHasCondition() {
                                        Conditions = new Kingmaker.UnitLogic.UnitCondition[]{
                                            Kingmaker.UnitLogic.UnitCondition.Shaken,
                                            Kingmaker.UnitLogic.UnitCondition.Frightened
                                        }
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new Conditional {
                                    ConditionsChecker = new ConditionsChecker {
                                        Conditions = new Condition[] {
                                            new ContextConditionCasterHasFact {
                                                m_Fact = ShatterDefensesMythicFeat.ToReference<BlueprintUnitFactReference>(),
                                            }
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(a => {
                                            a.m_Buff = ShatterDefensesMythicBuff.ToReference<BlueprintBuffReference>();
                                            a.DurationValue = new ContextDurationValue() {
                                                m_IsExtendable = false,
                                                Rate = DurationRate.Rounds,
                                                DiceCountValue = 0,
                                                BonusValue = 2
                                            };
                                        })
                                    ),
                                    IfFalse = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(a => {
                                            a.m_Buff = ShatterDefensesBuff.ToReference<BlueprintBuffReference>();
                                            a.DurationValue = new ContextDurationValue() {
                                                m_IsExtendable = false,
                                                Rate = DurationRate.Rounds,
                                                DiceCountValue = 0,
                                                BonusValue = 2
                                            };
                                        })
                                    ),
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(),
                        }
                    );
                });
                Main.LogPatch("Patched", ShatterDefenses);
            }
            static void PatchSpellSpecialization() {
                if (ModSettings.Fixes.Feats.IsDisabled("SpellSpecialization")) { return; }

                var SpellSpecializationProgression = Resources.GetBlueprint<BlueprintProgression>("fe9220cdc16e5f444a84d85d5fa8e3d5");

                Game.Instance.BlueprintRoot.Progression.CharacterClasses.ForEach(characterClass => {
                    SpellSpecializationProgression.AddClass(characterClass);
                });
                //SpellSpecializationProgression.AddClass(LoremasterClass);
                Main.LogPatch("Patched", SpellSpecializationProgression);
            }
            static void PatchSpiritedCharge() {
                if (ModSettings.Fixes.Feats.IsDisabled("SpiritedCharge")) { return; }

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
                if (ModSettings.Fixes.Feats.IsDisabled("WeaponFinesse")) { return; }

                var WeaponFinesse = Resources.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");

                WeaponFinesse.ReplaceComponents<AttackStatReplacement>(Helpers.Create<AttackStatReplacementTTT>(c => {
                    c.ReplacementStat = StatType.Dexterity;
                    c.SubCategory = WeaponSubCategory.Finessable;
                }));
                Main.LogPatch("Patched", WeaponFinesse);
            }
            static void PatchLunge() {
                if (ModSettings.Fixes.Feats.IsDisabled("Lunge")) { return; }

                var LungeFeature = Resources.GetBlueprint<BlueprintFeature>("d41d5bd9a775d7245929256d58a3e03e");

                LungeFeature.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                FeatTools.AddAsFeat(LungeFeature);
                Main.LogPatch("Patched", LungeFeature);
            }
            static void PatchSelectiveMetamagicPrerequisites() {
                if (ModSettings.Fixes.Feats.IsDisabled("SelectivePrerequisites")) { return; }

                var SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
                SelectiveSpellFeat.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillKnowledgeArcana;
                    c.Value = 10;
                });
                Main.LogPatch("Patched", SelectiveSpellFeat);
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "GetBolsteredAreaEffectUnits", new Type[] { typeof(TargetWrapper) })]
        static class MetamagicHelper_GetBolsteredAreaEffectUnits_Patch {
            static void Postfix(TargetWrapper origin, ref List<UnitEntityData> __result) {
                __result = __result.Where(unit => unit.AttackFactions.IsPlayerEnemy).ToList();
            }
        }

        [HarmonyPatch]
        static class VitalStrike_OnEventDidTrigger_Rowdy_Patch {
            private static Type _type = typeof(AbilityCustomVitalStrike).GetNestedType("<Deliver>d__7", AccessTools.all);
            internal static MethodInfo TargetMethod(Harmony instance) {
                return AccessTools.Method(_type, "MoveNext");
            }

            static readonly MethodInfo AbilityCustomVitalStrike_get_RowdyFeature = AccessTools.PropertyGetter(
                typeof(AbilityCustomVitalStrike),
                "RowdyFeature"
            );
            static readonly ConstructorInfo VitalStrikeEventHandler_Constructor = AccessTools.Constructor(
                typeof(VitalStrikeEventHandler),
                new Type[] {
                    typeof(UnitEntityData),
                    typeof(int),
                    typeof(bool),
                    typeof(bool)
                }
            );
            // ------------before------------
            // eventHandlers.Add(new AbilityCustomVitalStrike.VitalStrike(maybeCaster, this.VitalStrikeMod, maybeCaster.HasFact(this.MythicBlueprint), maybeCaster.HasFact(this.RowdyFeature)));
            // ------------after-------------
            // eventHandlers.Add(new VitalStrikeEventHandler(maybeCaster, this.VitalStrikeMod, maybeCaster.HasFact(this.MythicBlueprint), maybeCaster.HasFact(this.RowdyFeature)));
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                var codes = new List<CodeInstruction>(instructions);
                if (ModSettings.Fixes.Feats.IsDisabled("VitalStrike")) { return instructions; }
                int target = FindInsertionTarget(codes);
                //Main.Log($"OpperandType: {codes[71].operand.GetType()}");
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Newobj, VitalStrikeEventHandler_Constructor);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                //Looking for the arguments that define the object creation because searching for the object creation itself is hard
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(AbilityCustomVitalStrike_get_RowdyFeature)) {
                        if (codes[i + 2].opcode == OpCodes.Newobj) {
                            return i + 2;
                        }
                    }
                }
                Main.Log("VITALSTRIKEPATCH: COULD NOT FIND TARGET");
                return -1;
            }

            private class VitalStrikeEventHandler : IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
            IRulebookHandler<RuleCalculateWeaponStats>,
            IInitiatorRulebookHandler<RulePrepareDamage>,
            IRulebookHandler<RulePrepareDamage>,
            IInitiatorRulebookHandler<RuleAttackWithWeapon>,
            IRulebookHandler<RuleAttackWithWeapon>,
            ISubscriber, IInitiatorRulebookSubscriber {

                public VitalStrikeEventHandler(UnitEntityData unit, int damageMod, bool mythic, bool rowdy) {
                    this.m_Unit = unit;
                    this.m_DamageMod = damageMod;
                    this.m_Mythic = mythic;
                    this.m_Rowdy = rowdy;
                }

                public UnitEntityData GetSubscribingUnit() {
                    return this.m_Unit;
                }

                public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
                }

                public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
                    DamageDescription damageDescription = evt.DamageDescription.FirstItem();
                    if (damageDescription != null && damageDescription.TypeDescription.Type == DamageType.Physical) {
                        var vitalDamage = new DamageDescription() {
                            Dice = new DiceFormula(damageDescription.Dice.Rolls * Math.Max(1, this.m_DamageMod - 1), damageDescription.Dice.Dice),
                            Bonus = this.m_Mythic ? damageDescription.Bonus * Math.Max(1, this.m_DamageMod - 1) : 0,
                            TypeDescription = damageDescription.TypeDescription,
                            IgnoreReduction = damageDescription.IgnoreReduction,
                            IgnoreImmunities = damageDescription.IgnoreImmunities,
                            SourceFact = damageDescription.SourceFact,
                            CausedByCheckFail = damageDescription.CausedByCheckFail,
                            m_BonusWithSource = 0
                        };
                        evt.DamageDescription.Insert(1, vitalDamage);
                    }
                }
                public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
                }

                //For Ranged - Handling of damage calcs does not occur the same due to projectiles
                public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
                    if (!m_Rowdy) { return; }
                    RuleAttackRoll ruleAttackRoll = evt.AttackRoll;
                    if (ruleAttackRoll == null) { return; }
                    if (evt.Initiator.Stats.SneakAttack < 1) { return; }
                    if (!ruleAttackRoll.TargetUseFortification) {
                        UnitPartFortification unitPartFortification = ruleAttackRoll.Target.Get<UnitPartFortification>();
                        ruleAttackRoll.FortificationChance = ((unitPartFortification != null) ? unitPartFortification.Value : 0);
                    }
                    if (!ruleAttackRoll.TargetUseFortification || ruleAttackRoll.FortificationOvercomed) {
                        DamageTypeDescription damageTypeDescription = evt.ResolveRules
                            .Select(e => e.Damage).First()
                            .DamageBundle.First<BaseDamage>().CreateTypeDescription();
                        int rowdyDice = evt.Initiator.Stats.SneakAttack * 2;
                        DiceFormula dice = new DiceFormula(rowdyDice, DiceType.D6);
                        BaseDamage baseDamage = damageTypeDescription.GetDamageDescriptor(dice, 0).CreateDamage();
                        baseDamage.Precision = true;
                        evt.ResolveRules.Select(e => e.Damage)
                            .ForEach(e => e.Add(baseDamage));
                    }
                }

                //For Melee
                public void OnEventAboutToTrigger(RulePrepareDamage evt) {
                    if (!m_Rowdy) { return; }
                    RuleAttackRoll ruleAttackRoll = evt.ParentRule.AttackRoll;
                    if (ruleAttackRoll == null) { return; }
                    if (evt.Initiator.Stats.SneakAttack < 1) { return; }
                    if (!ruleAttackRoll.TargetUseFortification) {
                        UnitPartFortification unitPartFortification = ruleAttackRoll.Target.Get<UnitPartFortification>();
                        ruleAttackRoll.FortificationChance = ((unitPartFortification != null) ? unitPartFortification.Value : 0);
                    }
                    if (!ruleAttackRoll.TargetUseFortification || ruleAttackRoll.FortificationOvercomed) {
                        DamageTypeDescription damageTypeDescription = evt.DamageBundle
                            .First()
                            .CreateTypeDescription();
                        int rowdyDice = evt.Initiator.Stats.SneakAttack * 2;
                        DiceFormula dice = new DiceFormula(rowdyDice, DiceType.D6);
                        BaseDamage baseDamage = damageTypeDescription.GetDamageDescriptor(dice, 0).CreateDamage();
                        baseDamage.Precision = true;
                        evt.Add(baseDamage);
                    }
                }

                public void OnEventDidTrigger(RulePrepareDamage evt) {
                }

                private readonly UnitEntityData m_Unit;
                private int m_DamageMod;
                private bool m_Mythic;
                private bool m_Rowdy;
            }
        }
    }
}
