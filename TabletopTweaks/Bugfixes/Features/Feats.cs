using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
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
                PatchSpiritedCharge();
                PatchWeaponFinesse();
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
                FencingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceEnforced>());
                Main.LogPatch("Patched", FencingGrace);
            }
            static void PatchSlashingGrace() {
                if (ModSettings.Fixes.Feats.IsDisabled("SlashingGrace")) { return; }

                var SlashingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("697d64669eb2c0543abb9c9b07998a38");
                SlashingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceEnforced>());
                Main.LogPatch("Patched", SlashingGrace);
            }
            static void PatchEndurance() {
                if (ModSettings.Fixes.Feats.IsDisabled("Endurance")) { return; }
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
                if (ModSettings.Fixes.Feats.IsDisabled("MountedCombat")) { return; }

                var MountedCombat = Resources.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
                var MountedCombatCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
                MountedCombat.AddComponent(Helpers.Create<MountedCombatFixed>(c => {
                    c.m_CooldownBuff = MountedCombatCooldownBuff.ToReference<BlueprintBuffReference>();
                }));
                Main.LogPatch("Patched", MountedCombat);
            }
            static void PatchIndomitableMount() {
                if (ModSettings.Fixes.Feats.IsDisabled("IndomitableMount")) { return; }

                var IndomitableMount = Resources.GetBlueprint<BlueprintFeature>("68e814f1f3ce55942a52c1dd536eaa5b");
                var IndomitableMountCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("34762bab68ec86c45a15884b9a9929fc");
                IndomitableMount.AddComponent(Helpers.Create<IndomitableMountFixed>(c => {
                    c.m_CooldownBuff = IndomitableMountCooldownBuff.ToReference<BlueprintBuffReference>();
                }));
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

                WeaponFinesse.ReplaceComponents<AttackStatReplacement>(Helpers.Create<AttackStatReplacementEnforced>(c => {
                    c.ReplacementStat = StatType.Dexterity;
                    c.SubCategory = WeaponSubCategory.Finessable;
                }));
                Main.LogPatch("Patched", WeaponFinesse);
            }
        }

        [HarmonyPatch(typeof(RuleCheckTargetFlatFooted), "OnTrigger", new Type[] { typeof(RulebookEventContext) })]
        static class RuleCheckTargetFlatFooted_ShatterDefenses_Patch {
            private static readonly BlueprintBuff ShatterDefensesBuff = Resources.GetModBlueprint<BlueprintBuff>("ShatterDefensesBuff");
            private static readonly BlueprintBuff ShatterDefensesMythicBuff = Resources.GetModBlueprint<BlueprintBuff>("ShatterDefensesMythicBuff");

            static void Postfix(RuleCheckTargetFlatFooted __instance, RulebookEventContext context) {
                if (ModSettings.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }
                if (TacticalCombatHelper.IsActive) { return; }
                bool HasShatterFromCaster = __instance.Target.Buffs.Enumerable
                    .Any(buff => buff.Blueprint == ShatterDefensesBuff && buff.Context.MaybeCaster == __instance.Initiator);
                bool HasMythicShatter = __instance.Target.Buffs.HasFact(ShatterDefensesMythicBuff);
                __instance.IsFlatFooted |= HasShatterFromCaster || HasMythicShatter;
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
                Main.Log($"OpperandType: {codes[71].operand.GetType()}");
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
