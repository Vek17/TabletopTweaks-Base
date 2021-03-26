using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System.Linq;
using Kingmaker.RuleSystem.Rules;
using HarmonyLib;
using Kingmaker.RuleSystem;
using System;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Blueprints.Classes.Spells;

namespace TabletopTweaks {
    static class BalanceAdjustments {

        public static void patchNaturalArmorEffects() {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            patchAnimalCompanionFeatures();
            patchItemBuffs();
            patchClassFeatures();
            patchFeats();

            void patchAnimalCompanionFeatures() {
                BlueprintFeature animalCompanionNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                BlueprintFeature animalCompanionStatFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                BlueprintFeature[] animalCompanionUpgrades = Resources.Blueprints
                    .OfType<BlueprintFeature>()
                    .Where(bp => bp.name.Contains("AnimalCompanionUpgrade"))
                    .ToArray();

                animalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                animalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                animalCompanionUpgrades.ForEach(bp => {
                    bp.GetComponents<AddStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                });
            }
            void patchClassFeatures() {
                BlueprintFeature dragonDiscipleNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
                dragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
            void patchFeats() {
                BlueprintFeature improvedNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");
                improvedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
            void patchItemBuffs() {
                //Icy Protector
                BlueprintBuff protectionOfColdBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f592ecdb8045d7047a21b20ffee72afd");
                protectionOfColdBuff.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
        }
        public static void patchPolymorphEffects() {
            if (!Resources.Settings.DisablePolymorphStacking) { return; }
        }
    }

    //Patch Natural Armor Stacking
    [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
    static class ModifierDescriptorHelper_IsStackable_Patch {

        static void Postfix(ref bool __result, ref ModifierDescriptor descriptor) {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            if (descriptor == ModifierDescriptor.NaturalArmor) {
                __result = false;
            }
        }
    }

    //Patch Polymorph Stacking
    [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
    static class RuleCanApplyBuff_OnTrigger_Patch {
        static private BlueprintBuff[] polymorphBuffs;
        static private BlueprintBuff[] PolymorphBuffs {
            get {
                if (polymorphBuffs == null) {
                    BlueprintBuff[] taggedPolyBuffs = Resources.Blueprints.OfType<BlueprintBuff>()
                        .Where(bp => bp.GetComponents<SpellDescriptorComponent>()
                            .Where(c => (c.Descriptor & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0)
                        .ToArray();
                    polymorphBuffs = Resources.Blueprints.OfType<BlueprintAbility>()
                        .Where(bp =>
                            (bp.GetComponents<SpellDescriptorComponent>()
                                .Where(c => c != null)
                                .Where(d => (d.Descriptor.Value & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0)
                            || (bp.GetComponents<AbilityExecuteActionOnCast>()
                                .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>())
                                .Where(c => (c.SpellDescriptor.Value & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0)
                            || (bp.GetComponents<AbilityEffectRunAction>()
                                .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>()
                                    .Concat(c.Actions.Actions.OfType<ContextActionConditionalSaved>()
                                        .SelectMany(a => a.Failed.Actions
                                        .OfType<ContextActionRemoveBuffsByDescriptor>())))
                                .Where(c => (c.SpellDescriptor.Value & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0))
                        .SelectMany(bp => bp.GetComponents<AbilityEffectRunAction>())
                        .SelectMany(c => c.Actions.Actions.OfType<ContextActionApplyBuff>()
                            .Concat(c.Actions.Actions.OfType<ContextActionConditionalSaved>()
                                .SelectMany(a => a.Failed.Actions
                                .OfType<ContextActionApplyBuff>())))
                        .Where(c => c.Buff != null)
                        .Select(c => c.Buff)
                        .Concat(taggedPolyBuffs)
                        .Where(bp => bp.AssetGuid != "e6f2fc5d73d88064583cb828801212f4") // Fatigued
                        .Distinct()
                        .ToArray();
                    polymorphBuffs.ForEach(c => Main.Log($"PolymorphBuff - Grabbed ID: {c.AssetGuid} - Grabbed Name: {c.name} "));
                }
                return polymorphBuffs;
            }
        }

        static void Postfix(RuleCanApplyBuff __instance) {
            if (!Resources.Settings.DisablePolymorphStacking) { return; }
            if (!Array.Exists(PolymorphBuffs, bp => bp.Equals(__instance.Blueprint))) { return; }
            if (__instance.CanApply && (__instance.Context.MaybeCaster.Faction == __instance.Initiator.Faction)) {
                BlueprintBuff[] intesection = __instance.Initiator
                    .Buffs
                    .Enumerable
                    .Select(b => b.Blueprint)
                    .Intersect(PolymorphBuffs)
                    .ToArray();
                if (intesection.Any()) {
                    foreach (BlueprintBuff buffToRemove in intesection) {
                        __instance.Initiator
                            .Buffs
                            .GetBuff(buffToRemove)
                            .Remove();
                        Main.Log($"Removed Polymorph Buff: {buffToRemove.Name}");
                    }
                    Main.Log($"Applied Polymorph Buff: {__instance.Context.Name}");
                }
            }
        }
    }
}