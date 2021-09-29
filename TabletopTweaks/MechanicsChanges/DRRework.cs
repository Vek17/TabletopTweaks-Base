using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Tutorial;
using Kingmaker.Tutorial.Solvers;
using Kingmaker.Tutorial.Triggers;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Martial.DamageReduction;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Martial.EnergyResistance;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.ServiceWindow.CharacterScreen;
using Kingmaker.UI.Tooltip;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
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
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MechanicsChanges
{
    class DRRework {

        [HarmonyPatch(typeof(AddDamageResistancePhysical), nameof(AddDamageResistancePhysical.IsStackable), MethodType.Getter)]
        static class AddDamageResistancePhysical_IsStackable_Patch {

            static void Postfix(ref bool __result) {
                if (ModSettings.Fixes.DRRework.IsEnabled("Base")) {
                    __result = false;
                }
            }
        }

#if DEBUG
        [HarmonyPatch(typeof(AddDamageResistanceBase.ComponentRuntime), nameof(AddDamageResistanceBase.ComponentRuntime.OnTurnOn))]
        static class AddDamageResistanceBase_OnTurnOn_LogPatch {

            static bool Prefix(AddDamageResistanceBase.ComponentRuntime __instance) {
                if (ModSettings.Fixes.DRRework.IsEnabled("Base")) {
                    Main.LogDebug($"WARNING: Vanilla Damage Resistance turned on for fact: {__instance.Fact.Blueprint.AssetGuid} - {__instance.Fact.Blueprint.NameSafe()}");
                }
                return true;
            }
        }
#endif

        [HarmonyPatch(typeof(BlueprintFact), nameof(BlueprintFact.CollectComponents))]
        static class BlueprintFact_CollectComponents_Patch
        {
            static void Postfix(ref List<BlueprintComponent> __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }

                for (int i = 0; i < __result.Count; i++)
                {
                    BlueprintComponent component = __result[i];
                    if (component is AddDamageResistanceBase resistanceComponent)
                    {
                        TTAddDamageResistanceBase replacementComponent = CreateFromVanillaDamageResistance(resistanceComponent);
                        // https://c.tenor.com/eqLNYv0A9TQAAAAC/swap-indiana-jones.gif
                        __result[i] = replacementComponent;
                        Main.LogDebug("Replaced " + resistanceComponent.GetType().ToString() + " with " + replacementComponent.GetType().ToString());
                    }
                }
            }

            static TTAddDamageResistanceBase CreateFromVanillaDamageResistance(AddDamageResistanceBase vanillaResistance)
            {
                TTAddDamageResistanceBase result = null;
                switch (vanillaResistance)
                {
                    case ResistEnergy:
                        result = Helpers.Create<TTResistEnergy>();
                        break;
                    case ResistEnergyContext:
                        result = Helpers.Create<TTResistEnergyContext>();
                        break;
                    case ProtectionFromEnergy:
                        result = Helpers.Create<TTProtectionFromEnergy>();
                        break;
                    case WizardAbjurationResistance:
                        result = Helpers.Create<TTWizardAbjurationResistance>();
                        break;
                    case WizardEnergyAbsorption:
                        result = Helpers.Create<TTWizardEnergyAbsorption>();
                        break;
                    case AddDamageResistancePhysical:
                        result = Helpers.Create<TTAddDamageResistancePhysical>();
                        break;
                    case AddDamageResistanceEnergy:
                        result = Helpers.Create<TTAddDamageResistanceEnergy>();
                        break;
                    case AddDamageResistanceForce:
                        result = Helpers.Create<TTAddDamageResistanceForce>();
                        break;
                    default:
                        Main.Log("ERROR: Called CreateFromVanillaDamageResistance for unsupported type: " + vanillaResistance.GetType().ToString());
                        return null;
                }

                result.InitFromVanillaDamageResistance(vanillaResistance);
                return result;
            }
        }

        [HarmonyPatch(typeof(ReduceDamageReduction), nameof(ReduceDamageReduction.OnTurnOn))]
        static class ReduceDamageReduction_OnTurnOn_Patch
        {
            static bool Prefix(ReduceDamageReduction __instance)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                int penalty = __instance.Value.Calculate(__instance.Context) * __instance.Multiplier;
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().AddPenaltyEntry(penalty, __instance.Fact);
                return false;
            }
        }

        [HarmonyPatch(typeof(ReduceDamageReduction), nameof(ReduceDamageReduction.OnTurnOff))]
        static class ReduceDamageReduction_OnTurnOff_Patch
        {
            static bool Prefix(ReduceDamageReduction __instance)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().RemovePenaltyEntry(__instance.Fact);
                return false;
            }
        }

        [HarmonyPatch(typeof(CharInfoDamageReductionVM), nameof(CharInfoDamageReductionVM.GetDamageReduction))]
        static class CharInfoDamageReductionVM_GetDamageReduction_Patch
        {
            static void Postfix(CharInfoDamageReductionVM __instance, UnitDescriptor unit, ref List<CharInfoDamageReductionEntryVM> __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                List<CharInfoDamageReductionEntryVM> reductionEntryVmList = new List<CharInfoDamageReductionEntryVM>();
                IEnumerable<TTUnitPartDamageReduction.ReductionDisplay> allSources = unit.Get<TTUnitPartDamageReduction>()?.AllSources;
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                foreach(TTUnitPartDamageReduction.ReductionDisplay reduction in allSources.EmptyIfNull())
                {
                    if (reduction.ReferenceDamageResistance is TTAddDamageResistancePhysical settings1)
                    {
                        CharInfoDamageReductionEntryVM reductionEntryVm = new CharInfoDamageReductionEntryVM()
                        {
                            Value = reduction.TotalReduction.ToString()
                        };
                        if (settings1.BypassedByAlignment)
                            reductionEntryVm.Exceptions.Add(ls.DamageAlignment.GetTextFlags(settings1.Alignment));
                        if (settings1.BypassedByForm)
                            reductionEntryVm.Exceptions.AddRange(settings1.Form.Components().Select<PhysicalDamageForm, string>((Func<PhysicalDamageForm, string>)(f => ls.DamageForm.GetText(f))));
                        if (settings1.BypassedByMagic)
                            reductionEntryVm.Exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MagicDRDescriptor);
                        if (settings1.BypassedByMaterial)
                            reductionEntryVm.Exceptions.Add(ls.DamageMaterial.GetTextFlags(settings1.Material));
                        if (settings1.BypassedByReality)
                            reductionEntryVm.Exceptions.Add(ls.DamageReality.GetText(settings1.Reality));
                        if (settings1.BypassedByMeleeWeapon)
                            reductionEntryVm.Exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MeleeDRDescriptor);
                        if (settings1.BypassedByWeaponType)
                            reductionEntryVm.Exceptions.Add((string)settings1.WeaponType.TypeName);
                        if (reductionEntryVm.Exceptions.Count == 0)
                            reductionEntryVm.Exceptions.Add("-");
                        reductionEntryVmList.Add(reductionEntryVm);
                    }
                }
                __result = reductionEntryVmList;
            }
        }

        [HarmonyPatch(typeof(CharInfoEnergyResistanceVM), nameof(CharInfoEnergyResistanceVM.GetEnergyResistance))]
        static class CharInfoEnergyResistanceVM_GetEnergyResistance_Patch {
            static void Postfix(CharInfoEnergyResistanceVM __instance, UnitDescriptor unit, ref List<CharInfoEnergyResistanceEntryVM> __result) {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                List<CharInfoEnergyResistanceEntryVM> resistanceEntryVMList = new List<CharInfoEnergyResistanceEntryVM>();
                LocalizedTexts localizedTexts = Game.Instance.BlueprintRoot.LocalizedTexts;
                foreach (TTUnitPartDamageReduction.ReductionDisplay reduction in 
                    unit.Get<TTUnitPartDamageReduction>()?.AllSources?.EmptyIfNull().OrderByDescending(rd => rd.ReferenceDamageResistance?.Priority ?? TTAddDamageResistanceBase.DRPriority.Normal)) {
                    if (reduction.ReferenceDamageResistance is TTAddDamageResistanceEnergy settings1) {
                        CharInfoEnergyResistanceEntryVM resistanceEntryVM = new CharInfoEnergyResistanceEntryVM() {
                            Value = reduction.TotalReduction,
                            Type = localizedTexts.DamageEnergy.GetText(settings1.Type)
                        };
                        resistanceEntryVMList.Add(resistanceEntryVM);
                    }
                }
                __result = resistanceEntryVMList;
            }
        }

        [HarmonyPatch(typeof(CharSMartial), nameof(CharSMartial.GetDamageReduction))]
        static class CharSMartial_GetDamageReduction_Patch
        {
            static void Postfix(CharSMartial __instance, UnitDescriptor unit, ref List<CharSMartial.DRdata> __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                List<CharSMartial.DRdata> drdataList = new List<CharSMartial.DRdata>();
                TTUnitPartDamageReduction partDamageReduction = unit.Get<TTUnitPartDamageReduction>();
                IEnumerable<TTUnitPartDamageReduction.ReductionDisplay> list = partDamageReduction != null ? partDamageReduction.AllSources.Where(c => c.ReferenceDamageResistance is TTAddDamageResistancePhysical) : null;
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                foreach (TTUnitPartDamageReduction.ReductionDisplay reduction in list.EmptyIfNull())
                {
                    TTAddDamageResistancePhysical settings = (TTAddDamageResistancePhysical)reduction.ReferenceDamageResistance;

                    CharSMartial.DRdata drdata = new CharSMartial.DRdata();
                    drdata.value = reduction.TotalReduction.ToString();
                    if (settings.BypassedByAlignment)
                        drdata.exceptions.Add(ls.DamageAlignment.GetTextFlags(settings.Alignment));
                    if (settings.BypassedByForm)
                        drdata.exceptions.AddRange(settings.Form.Components().Select<PhysicalDamageForm, string>((Func<PhysicalDamageForm, string>)(f => ls.DamageForm.GetText(f))));
                    if (settings.BypassedByMagic)
                        drdata.exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MagicDRDescriptor);
                    if (settings.BypassedByMaterial)
                        drdata.exceptions.Add(ls.DamageMaterial.GetTextFlags(settings.Material));
                    if (settings.BypassedByReality)
                        drdata.exceptions.Add(ls.DamageReality.GetText(settings.Reality));
                    if (settings.BypassedByMeleeWeapon)
                        drdata.exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MeleeDRDescriptor);
                    if (settings.BypassedByWeaponType)
                        drdata.exceptions.Add((string)settings.WeaponType.TypeName);
                    if (drdata.exceptions.Count == 0)
                        drdata.exceptions.Add("-");
                    drdataList.Add(drdata);
                }
                __result = drdataList;
            }
        }

        [HarmonyPatch(typeof(CharSMartial), nameof(CharSMartial.GetEnergyResistance))]
        static class CharSMartial_GetEnergyResistance_Patch {
            static void Postfix(CharSMartial __instance, UnitDescriptor unit, List<CharSMartial.ERdata> __result) {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                List<CharSMartial.ERdata> erdataList = new List<CharSMartial.ERdata>();
                LocalizedTexts localizedTexts = Game.Instance.BlueprintRoot.LocalizedTexts;
                foreach (TTUnitPartDamageReduction.ReductionDisplay reduction in
                    unit.Get<TTUnitPartDamageReduction>()?.AllSources?.EmptyIfNull().OrderByDescending(rd => rd.ReferenceDamageResistance?.Priority ?? TTAddDamageResistanceBase.DRPriority.Normal)) {
                    if (reduction.ReferenceDamageResistance is TTAddDamageResistanceEnergy settings1) {
                        CharSMartial.ERdata erdata = new CharSMartial.ERdata();
                        erdata.value = reduction.TotalReduction.ToString();
                        erdata.type = localizedTexts.DamageEnergy.GetText(settings1.Type);
                        erdataList.Add(erdata);
                    }
                }
                __result = erdataList;
            }
        }

        [HarmonyPatch(typeof(TutorialTriggerDamageReduction), nameof(TutorialTriggerDamageReduction.ShouldTrigger))]
        static class TutorialTriggerDamageReduction_ShouldTrigger_Patch
        {
            static void Postfix(TutorialTriggerDamageReduction __instance, RuleDealDamage rule, ref bool __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                if (!__result && !rule.IgnoreDamageReduction)
                {
                    TTUnitPartDamageReduction partDamageReduction = rule.Target.Get<TTUnitPartDamageReduction>();
                    if (partDamageReduction != null && rule.ResultList != null && __instance.AbsoluteDR == partDamageReduction.HasAbsolutePhysicalDR)
                    {
                        foreach(DamageValue res in rule.ResultList)
                        {
                            if (res.Source is PhysicalDamage && res.Reduction > 0)
                            {
                                __result = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AddEnergyImmunity), nameof(AddEnergyImmunity.OnTurnOn))]
        static class AddEnergyImmunity_OnTurnOn_Patch
        {
            static bool Prefix(AddEnergyImmunity __instance)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                __instance.Owner.Ensure<TTUnitPartDamageReduction>().AddImmunity(__instance.Fact, __instance, __instance.Type);
                return false;
            }
        }

        [HarmonyPatch(typeof(AddEnergyImmunity), nameof(AddEnergyImmunity.OnTurnOff))]
        static class AddEnergyImmunity_OnTurnOff_Patch
        {
            static bool Prefix(AddEnergyImmunity __instance)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return true; }
                __instance.Owner.Get<TTUnitPartDamageReduction>()?.RemoveImmunity(__instance.Fact, __instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(TutorialSolverSpellWithDamage), nameof(TutorialSolverSpellWithDamage.GetBasePriority))]
        static class TutorialSolverSpellWithDamage_GetBasePriority_Patch
        {
            static void Postfix(TutorialSolverSpellWithDamage __instance, BlueprintAbility ability, UnitEntityData caster, ref int __result)
            {
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                if (__result != -1)
                {
                    TTUnitPartDamageReduction partDamageReduction = ContextData<TutorialContext>.Current.TargetUnit.Get<TTUnitPartDamageReduction>();
                    if (partDamageReduction != null)
                    {
                        foreach(Element elements in ability.ElementsArray)
                        {
                            if (elements is ContextActionDealDamage actionDealDamage)
                            {
                                if (actionDealDamage.DamageType.Type == DamageType.Energy && partDamageReduction.IsImmune(actionDealDamage.DamageType.Energy))
                                {
                                    __result = -1;
                                    return;
                                }
                                BaseDamage damage = actionDealDamage.DamageType.CreateDamage(DiceFormula.Zero, 0);
                                if (!partDamageReduction.CanBypass(damage, null))
                                {
                                    __result = -1;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UIUtilityItem), nameof(UIUtilityItem.FillShieldEnchantments), typeof(ItemTooltipData), typeof(ItemEntityShield), typeof(string))]
        [HarmonyDebug]
        static class UIUtilityItem_FillShieldEnchantments2_Patch {

            private static Dictionary<Type, Type> _typeMapping = new Dictionary<Type, Type>() {
                { typeof(AddDamageResistanceEnergy), typeof(TTAddDamageResistanceEnergy) },
                { typeof(BlueprintComponentAndRuntime<AddDamageResistanceEnergy>), typeof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>) },
                { typeof(AddDamageResistanceBase.ComponentRuntime), typeof(TTAddDamageResistanceBase.ComponentRuntime) }
            };

            private static Dictionary<MethodInfo, MethodInfo> _staticCallMapping = new Dictionary<MethodInfo, MethodInfo> {
                { 
                    AccessTools.Method(typeof(UIUtilityItem), nameof(UIUtilityItem.GetEnergyResistanceText)), 
                    AccessTools.Method(typeof(UIUtilityItem_FillShieldEnchantments2_Patch), nameof(UIUtilityItem_FillShieldEnchantments2_Patch.TTGetEnergyResistanceText)) 
                }
            };

            static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> codes, ILGenerator il) {

                bool replaceLoc0 = false;
                LocalBuilder loc0replacement = null;

                bool replaceLoc1 = false;
                LocalBuilder loc1replacement = null;

                bool replaceLoc2 = false;
                LocalBuilder loc2replacement = null;

                bool replaceLoc3 = false;
                LocalBuilder loc3replacement = null;

                Dictionary<Type, LocalBuilder> locSReplacements = new Dictionary<Type, LocalBuilder>();
                
                foreach(LocalVariableInfo local in original.GetMethodBody().LocalVariables) {
                    if (_typeMapping.ContainsKey(local.LocalType)) {
                        LocalBuilder newLocal = il.DeclareLocal(_typeMapping[local.LocalType]);
                        switch (local.LocalIndex) {
                            case 0:
                                replaceLoc0 = true;
                                loc0replacement = newLocal;
                                break;
                            case 1:
                                replaceLoc1 = true;
                                loc1replacement = newLocal;
                                break;
                            case 2:
                                replaceLoc2 = true;
                                loc2replacement = newLocal;
                                break;
                            case 3:
                                replaceLoc3 = true;
                                loc3replacement = newLocal;
                                break;
                            default:
                                locSReplacements.Add(local.LocalType, newLocal);
                                break;
                        }
                    }
                }

                foreach (CodeInstruction code in codes) {
                    if (code.opcode == OpCodes.Stloc_0 && replaceLoc0) {
                        yield return new CodeInstruction(OpCodes.Stloc_S, loc0replacement);
                    } else if (code.opcode == OpCodes.Stloc_1 && replaceLoc1) {
                        yield return new CodeInstruction(OpCodes.Stloc_S, loc1replacement);
                    } else if (code.opcode == OpCodes.Stloc_2 && replaceLoc2) {
                        yield return new CodeInstruction(OpCodes.Stloc_S, loc2replacement);
                    } else if (code.opcode == OpCodes.Stloc_3 && replaceLoc3) {
                        yield return new CodeInstruction(OpCodes.Stloc_S, loc3replacement);
                    } else if (code.opcode == OpCodes.Stloc_S && locSReplacements.ContainsKey(((LocalBuilder)code.operand).LocalType)) {
                        yield return new CodeInstruction(OpCodes.Stloc_S, locSReplacements[((LocalBuilder)code.operand).LocalType]);
                    } else if (code.opcode == OpCodes.Ldloc_0 && replaceLoc0) {
                        yield return new CodeInstruction(OpCodes.Ldloc_S, loc0replacement);
                    } else if (code.opcode == OpCodes.Ldloc_1 && replaceLoc1) {
                        yield return new CodeInstruction(OpCodes.Ldloc_S, loc1replacement);
                    } else if (code.opcode == OpCodes.Ldloc_2 && replaceLoc2) {
                        yield return new CodeInstruction(OpCodes.Ldloc_S, loc2replacement);
                    } else if (code.opcode == OpCodes.Ldloc_3 && replaceLoc3) {
                        yield return new CodeInstruction(OpCodes.Ldloc_S, loc3replacement);
                    } else if (code.opcode == OpCodes.Ldloc_S && locSReplacements.ContainsKey(((LocalBuilder)code.operand).LocalType)) {
                        yield return new CodeInstruction(OpCodes.Ldloc_S, locSReplacements[((LocalBuilder)code.operand).LocalType]);
                    } else if (code.opcode == OpCodes.Callvirt) {
                        MethodInfo method = (MethodInfo)code.operand;
                        bool replaceMethod = false;
                        MethodInfo newMethod = null;
                        if (_typeMapping.ContainsKey(method.DeclaringType)) {
                            replaceMethod = true;
                            newMethod = AccessTools.Method(
                                _typeMapping[method.DeclaringType], 
                                method.Name, 
                                method.GetParameters().Select(pi => pi.ParameterType).ToArray(), 
                                method.GetGenericArguments());
                        }
                        if (method.IsGenericMethod) {
                            Type[] genericArguments = method.GetGenericArguments();
                            List<Type> replacementGenerics = new List<Type>();
                            bool replaceGenerics = false;
                            for (int i = 0; i < genericArguments.Length; i++) {
                                if (_typeMapping.ContainsKey(genericArguments[i])) {
                                    replaceGenerics = true;
                                    replacementGenerics.Add(_typeMapping[genericArguments[i]]);
                                } else {
                                    replacementGenerics.Add(genericArguments[i]);
                                }
                            }
                            if (replaceGenerics) {
                                replaceMethod = true;
                                if (newMethod == null) {
                                    newMethod = AccessTools.Method(
                                        method.DeclaringType,
                                        method.Name,
                                        method.GetParameters().Select(pi => pi.ParameterType).ToArray(),
                                        replacementGenerics.ToArray());
                                } else {
                                    newMethod = AccessTools.Method(
                                        newMethod.DeclaringType,
                                        newMethod.Name,
                                        newMethod.GetParameters().Select(pi => pi.ParameterType).ToArray(),
                                        replacementGenerics.ToArray());
                                }
                            }
                        }
                        if (replaceMethod) {
                            yield return new CodeInstruction(OpCodes.Callvirt, newMethod);
                        } else {
                            yield return code;
                        }
                    } else if (code.opcode == OpCodes.Call && _staticCallMapping.ContainsKey((MethodInfo)code.operand)) {
                        yield return new CodeInstruction(OpCodes.Call, _staticCallMapping[(MethodInfo)code.operand]);
                    } else if (code.opcode == OpCodes.Ldfld) {
                        FieldInfo field = (FieldInfo)code.operand;
                        if (_typeMapping.ContainsKey(field.DeclaringType)) {
                            FieldInfo newField = AccessTools.Field(
                                _typeMapping[field.DeclaringType],
                                field.Name);

                            yield return new CodeInstruction(OpCodes.Ldfld, newField);
                        } else {
                            yield return code;
                        }
                    } else if (code.opcode == OpCodes.Castclass && _typeMapping.ContainsKey((Type)code.operand)) {
                        yield return new CodeInstruction(OpCodes.Castclass, _typeMapping[(Type)code.operand]);
                    } else {
                        yield return code;
                    }
                }
            }

            private static string TTGetEnergyResistanceText(TTAddDamageResistanceBase.ComponentRuntime damageEnergyResist) {
                return damageEnergyResist == null ? "" : "+" + (object)damageEnergyResist.GetValue();
            }

        }

        [HarmonyPatch(typeof(UIUtilityItem), nameof(UIUtilityItem.FillShieldEnchantments), typeof(TooltipData), typeof(ItemEntityShield), typeof(string))]
        static class UIUtilityItem_FillShieldEnchantments_Patch {
            static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> codes, ILGenerator il) {
                // -------- before --------
                // visibleEnchantment.GetComponentWithRuntime<AddDamageResistanceEnergy>();
                // -------- after ---------
                // visibleEnchantment.GetComponentWithRuntime<TTAddDamageResistanceEnergy>();
                MethodInfo get_component_with_runtime_vanilla_energy_resist =
                    typeof(EntityFact).GetMethod(nameof(EntityFact.GetComponentWithRuntime))
                    .MakeGenericMethod(typeof(AddDamageResistanceEnergy));
                MethodInfo get_component_with_runtime_rework_energy_resist = 
                    typeof(EntityFact).GetMethod(nameof(EntityFact.GetComponentWithRuntime))
                    .MakeGenericMethod(typeof(TTAddDamageResistanceEnergy));

                // -------- before --------
                // ((BlueprintComponent<AddDamageResistanceEnergy>)componentWithRuntime).Component
                // -------- after ---------
                // ((BlueprintComponent<TTAddDamageResistanceEnergy>)componentWithRuntime).Component
                FieldInfo component_field_component_with_runtime_vanilla_energy_resist =
                    typeof(BlueprintComponentAndRuntime<AddDamageResistanceEnergy>)
                    .GetField(nameof(BlueprintComponentAndRuntime<AddDamageResistanceEnergy>.Component));
                FieldInfo component_field_component_with_runtime_rework_energy_resist =
                    typeof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>)
                    .GetField(nameof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>));

                // -------- before --------
                // AddDamageResistanceBase.ComponentRuntime runtime = (AddDamageResistanceBase.ComponentRuntime) componentWithRuntime.Runtime;
                // -------- after ---------
                // TTAddDamageResistanceBase.ComponentRuntime runtime = (TTAddDamageResistanceBase.ComponentRuntime) componentWithRuntime.Runtime;
                FieldInfo runtime_field_component_with_runtime_vanilla_energy_resist =
                    typeof(BlueprintComponentAndRuntime<AddDamageResistanceEnergy>)
                    .GetField(nameof(BlueprintComponentAndRuntime<AddDamageResistanceEnergy>.Runtime));
                FieldInfo runtime_field_component_with_runtime_rework_energy_resist =
                    typeof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>)
                    .GetField(nameof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>.Runtime));

                // -------- before --------
                // componentWithRuntime.Component.Type // (vanilla types)
                // -------- after ---------
                // componentWithRuntime.Component.Type // (rework types)
                FieldInfo type_field_vanilla_energy_resist =
                    typeof(AddDamageResistanceEnergy)
                    .GetField(nameof(AddDamageResistanceEnergy.Type));
                FieldInfo type_field_rework_energy_resist =
                    typeof(TTAddDamageResistanceEnergy)
                    .GetField(nameof(TTAddDamageResistanceEnergy.Type));

                LocalBuilder newLoc2 = il.DeclareLocal(typeof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>));
                LocalBuilder newLoc4 = il.DeclareLocal(typeof(TTAddDamageResistanceBase.ComponentRuntime));

                var locals = original.GetMethodBody().LocalVariables;
                foreach(var local in locals) {
                    Main.LogDebug($"Local: type = {local.LocalType} ; index = {local.LocalIndex} ; isPinned = {local.IsPinned}");
                }

                foreach (var code in codes) {
                    if (code.opcode == OpCodes.Stloc_2) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found stloc.2");
                        CodeInstruction newCodeInstruction = new CodeInstruction(OpCodes.Stloc_S, newLoc2);
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yield stloc_s newLoc2");
                        yield return newCodeInstruction;
                    } else if (code.opcode == OpCodes.Ldloc_2) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found ldloc.2");
                        CodeInstruction newCodeInstruction = new CodeInstruction(OpCodes.Ldloc_S, newLoc2);
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yield ldloc_s newLoc2");
                        yield return newCodeInstruction;
                    } else if (code.opcode == OpCodes.Stloc_S && ((LocalBuilder)code.operand).LocalType == typeof(AddDamageResistanceBase.ComponentRuntime)) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found stloc.s 4");
                        CodeInstruction newCodeInstruction = new CodeInstruction(OpCodes.Stloc_S, newLoc4);
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yield stloc_s newloc4");
                        yield return newCodeInstruction;
                    } else if (code.opcode == OpCodes.Ldloc_S && ((LocalBuilder)code.operand).LocalType == typeof(AddDamageResistanceBase.ComponentRuntime)) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found ldloc.s 4");
                        CodeInstruction newCodeInstruction = new CodeInstruction(OpCodes.Ldloc_S, newLoc4);
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yield ldloc_s newloc4");
                        yield return newCodeInstruction;
                    } else if (code.opcode == OpCodes.Callvirt && (MethodInfo)code.operand == get_component_with_runtime_vanilla_energy_resist) {
                    //if (code.Calls(get_component_with_runtime_vanilla_energy_resist)) { 
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found call to GetComponentWithRuntime<AddDamageResistanceEnergy>");
                        CodeInstruction newCodeInstruction = new CodeInstruction(OpCodes.Callvirt, get_component_with_runtime_rework_energy_resist);
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yielding GetComponentWithRuntime<TTAddDamageResistanceEnergy>");
                        yield return newCodeInstruction;
                    } else if (code.LoadsField(component_field_component_with_runtime_vanilla_energy_resist)) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found loadfield Component of BlueprintComponentAndRuntime<AddDamageResistanceEnergy>");
                        CodeInstruction newCodeInstruction = CodeInstruction.LoadField(
                            typeof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>),
                            nameof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>.Component));
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yielding loadfield Component of BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>");
                        yield return newCodeInstruction;
                    } else if (code.LoadsField(runtime_field_component_with_runtime_vanilla_energy_resist)) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found loadfield Runtime of BlueprintComponentAndRuntime<AddDamageResistanceEnergy>");
                        CodeInstruction newCodeInstruction = CodeInstruction.LoadField(
                            typeof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>),
                            nameof(BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>.Runtime));
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yielding loadfield Runtime of BlueprintComponentAndRuntime<TTAddDamageResistanceEnergy>");
                        yield return newCodeInstruction;
                    } else if (code.opcode == OpCodes.Castclass && (Type)code.operand == typeof(AddDamageResistanceBase.ComponentRuntime)) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found castclass to AddDamageResistanceBase.ComponentRuntime");
                        CodeInstruction newCodeInstruction = new CodeInstruction(OpCodes.Castclass, typeof(TTAddDamageResistanceBase.ComponentRuntime));
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yielding castclass to TTAddDamageResistanceBase.ComponentRuntime");
                        yield return newCodeInstruction;
                    } else if (code.Calls(typeof(UIUtilityItem).GetMethod(nameof(UIUtilityItem.GetEnergyResistanceText), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))) {
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: found call to UIUtilityItem::GetEnergyResistance");
                        CodeInstruction newCodeInstruction = CodeInstruction.Call(
                            typeof(UIUtilityItem_FillShieldEnchantments_Patch),
                            nameof(UIUtilityItem_FillShieldEnchantments_Patch.TTGetEnergyResistanceText));
                        Main.LogDebug("UIUtilityItem.FillShieldEnchantments Patch: yielding call to UIUtilityItem_FillShieldEnchantments_Patch.TTGetEnergyResistanceText");
                        yield return newCodeInstruction;
                    } else {
                        yield return code;
                    }
                }
            }

            private static string TTGetEnergyResistanceText(TTAddDamageResistanceBase.ComponentRuntime damageEnergyResist) {
                return damageEnergyResist == null ? "" : "+" + (object)damageEnergyResist.GetValue();
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.DRRework.IsDisabled("Base")) { return; }
                Main.LogHeader("Patching Blueprints for DR Rework");

                PatchArmorDR();
                PatchStalwartDefender();
                PatchBarbariansDR();
                PatchLichIndestructibleBonesDR();
                PatchBrokenDRSettings();
            }

            static void PatchArmorDR()
            {
                BlueprintUnitFact[] armorFactsWithPhysicalDR = new BlueprintUnitFact[]
                {
                    Resources.GetBlueprint<BlueprintFeature>("e93a376547629e2478d6f50e5f162efb"), // AdamantineArmorLightFeature
                    Resources.GetBlueprint<BlueprintFeature>("74a80c42774045f4d916dc0d990b7738"), // AdamantineArmorMediumFeature
                    Resources.GetBlueprint<BlueprintFeature>("dbbf704bfcc78854ab149597ef9aae7c"), // AdamantineArmorHeavyFeature
                    Resources.GetBlueprint<BlueprintFeature>("b99c50dd771a36d4f913bf1f56ba77a2"), // ArmorOfWealthFeature
                    Resources.GetBlueprint<BlueprintFeature>("a8ea2027afa333246a86b8085c23fbfd"), // BeaconOfCarnageFeature
                    Resources.GetBlueprint<BlueprintBuff>("42ab909d597f1734cb9bf65a74db7424"),    // BeaconOfCarnageEffectBuff
                    Resources.GetBlueprint<BlueprintFeature>("06d2f00616ad40c3b136d06dffc8f0b5"), // ColorlessRemainsBreastplate_SolidFeature
                    Resources.GetBlueprint<BlueprintFeature>("ff2d26e87b5f2bc4ba1823e242f10890"), // ForMounted_HalfplateOfSynergyFeature
                    Resources.GetBlueprint<BlueprintFeature>("e19008b823a221043b9184ef3c271db1"), // RealmProtectorFeature
                    Resources.GetBlueprint<BlueprintFeature>("79babe38a7306ba4c81f2fa3c88d1bae")  // StuddedArmorOfTrinityFeature
                };

                foreach (BlueprintUnitFact armorBlueprint in armorFactsWithPhysicalDR)
                {
                    armorBlueprint.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                    {
                        newRes.SourceIsArmor = true;
                    });
                }
            }

            static void PatchBarbariansDR()
            {
                BlueprintFeature barbarianDR = Resources.GetBlueprint<BlueprintFeature>("cffb5cddefab30140ac133699d52a8f8");
                BlueprintFeature invulnerableRagerDR = Resources.GetBlueprint<BlueprintFeature>("e71bd204a2579b1438ebdfbf75aeefae");
                BlueprintFeature madDogMasterDamageReduction = Resources.GetBlueprint<BlueprintFeature>("a0d4a3295224b8f4387464a4447c31d5");
                BlueprintFeature madDogPetDamageReduction = Resources.GetBlueprint<BlueprintFeature>("2edbf059fd033974bbff67960f15974d");

                BlueprintFeature bloodragerDR = Resources.GetBlueprint<BlueprintFeature>("07eba4bb72c2e3845bb442dce85d3b58");

                BlueprintFeature skaldDR = Resources.GetBlueprint<BlueprintFeature>("d9446a35d1401cf418bb9b5e0e199d57");

                BlueprintFeature increasedDamageReductionRagePower = Resources.GetBlueprint<BlueprintFeature>("ddaee203ee4dcb24c880d633fbd77db6");

                BlueprintBuff manglingFrenzyBuff = Resources.GetBlueprint<BlueprintBuff>("1581c5ceea24418cadc9f26ce4d391a9");

                barbarianDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                invulnerableRagerDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                madDogMasterDamageReduction.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                bloodragerDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                skaldDR.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                // Fix Skald DR not increasing with Increased Damage ResistanCce Rage Power
                ContextRankConfig barbarianDRConfig = barbarianDR.GetComponent<ContextRankConfig>();
                ContextRankConfig skaldDRRankConfig = Helpers.CreateCopy(barbarianDRConfig, crc =>
                {
                    crc.m_FeatureList = new BlueprintFeatureReference[]
                    {
                        skaldDR.ToReference<BlueprintFeatureReference>(),
                        increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>(),
                        increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>()
                    };
                });

                skaldDR.RemoveComponents<ContextRankConfig>();
                skaldDR.AddComponent(skaldDRRankConfig);

                Main.Log($"Patched: ContextRankConfig on {skaldDR.AssetGuid} - {skaldDR.NameSafe()}");

                // Allow Mangling Frenzy to stack with Barbarian DR's
                manglingFrenzyBuff.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.StacksWithFacts = new BlueprintUnitFactReference[]
                    {
                        barbarianDR.ToReference<BlueprintUnitFactReference>(),
                        invulnerableRagerDR.ToReference<BlueprintUnitFactReference>(),
                        madDogMasterDamageReduction.ToReference<BlueprintUnitFactReference>(),
                        madDogPetDamageReduction.ToReference<BlueprintUnitFactReference>(),
                        bloodragerDR.ToReference<BlueprintUnitFactReference>(),
                        skaldDR.ToReference<BlueprintUnitFactReference>()
                    };
                });

                // Fix Bloodrager (Primalist) DR not being increased by the Improved Damage Reduction rage power
                bloodragerDR.AddComponent(Helpers.CreateContextRankConfig(crc => {
                    crc.m_BaseValueType = ContextRankBaseValueType.FeatureListRanks;
                    crc.m_FeatureList = new BlueprintFeatureReference[] {
                        bloodragerDR.ToReference<BlueprintFeatureReference>(),
                        increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>(),
                        increasedDamageReductionRagePower.ToReference<BlueprintFeatureReference>()
                    };
                }));
                

                // Fix Mad Dog's pet DR not being improved by master's Increased Damage Resistance Rage Power(s)
                BlueprintUnitProperty madDogPetDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("MadDogPetDRProperty", bp =>
                {
                    bp.AddComponent(Helpers.Create<MadDogPetDRProperty>());
                });

                madDogPetDamageReduction.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                });

                madDogPetDamageReduction.RemoveComponents<ContextRankConfig>();
                madDogPetDamageReduction.AddComponent(Helpers.Create<ContextRankConfig>(crc =>
                {
                    crc.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    crc.m_CustomProperty = madDogPetDRProperty.ToReference<BlueprintUnitPropertyReference>();
                }));

                // Fix Increased Damage Reduction Rage Power not checking if the character actual has the DamageReduction class feature
                increasedDamageReductionRagePower.AddComponent<PrerequisiteFeaturesFromListFormatted>(p =>
                {
                    p.m_Features = new BlueprintFeatureReference[]
                    {
                        barbarianDR.ToReference<BlueprintFeatureReference>(),
                        invulnerableRagerDR.ToReference<BlueprintFeatureReference>(),
                        madDogMasterDamageReduction.ToReference<BlueprintFeatureReference>(),
                        bloodragerDR.ToReference<BlueprintFeatureReference>(),
                        skaldDR.ToReference<BlueprintFeatureReference>()
                    };
                    p.Amount = 1;
                });
            }

            static void PatchStalwartDefender()
            {
                BlueprintFeature stalwartDefenderDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("4d4f48f401d5d8b408c2e7a973fba9ea");

                stalwartDefenderDamageReductionFeature.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                    newRes.IsIncreasedByArmor = true;
                });

                BlueprintFeature increasedDamageReductionDefensivePower = Resources.GetBlueprint<BlueprintFeature>("d10496e92d0799a40bb3930b8f4fda0d");

                increasedDamageReductionDefensivePower.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes =>
                {
                    newRes.SourceIsClassFeature = true;
                    newRes.IncreasesFacts = new BlueprintUnitFactReference[]
                    {
                        stalwartDefenderDamageReductionFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            }

            static void PatchLichIndestructibleBonesDR() {
                BlueprintFeature lichIndestructibleBonesFeature = Resources.GetBlueprint<BlueprintFeature>("42274a4428cb43b40acf771a7f5ddfac");

                lichIndestructibleBonesFeature.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes => {
                    newRes.AddToAllStacks = true;
                });
            }


            static void PatchBrokenDRSettings() {
                // Fix: Winter Oracle Ice Armor revelation should be DR 5/piercing, but is DR 5/- due to missing BypassedByForm flag
                BlueprintBuff oracleRevelationIceArmorDRBuff = Resources.GetBlueprint<BlueprintBuff>("448e35444e80e24438a5ad0a3114aee3");
                oracleRevelationIceArmorDRBuff.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes => {
                    newRes.BypassedByForm = true;
                });

                // Fix: Bruiser's Chainshirt DR should be DR 3/piercing, but is DR 3/- due to missing BypassedByForm flag
                BlueprintFeature bruisersChainshirtFeature = Resources.GetBlueprint<BlueprintFeature>("2f08e4d39c1c568478c43aba81c42525");
                bruisersChainshirtFeature.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes => {
                    newRes.BypassedByForm = true;
                });

                // Fix: Warden of Darkness (Tower Shield) should be DR 5/good, but was DR 5/-
                BlueprintFeature towerShieldWardenOfDarknessShieldFeature = Resources.GetBlueprint<BlueprintFeature>("4211cdbf0bf04a540a366ba1d1c7dcc2");
                towerShieldWardenOfDarknessShieldFeature.ConvertVanillaDamageResistanceToRework<AddDamageResistancePhysical, TTAddDamageResistancePhysical>(newRes => {
                    newRes.BypassedByAlignment = true;
                });
            }
        }

        public class MadDogPetDRProperty : PropertyValueGetter
        {
            private static BlueprintFeature MadDogMasterDamageReduction = Resources.GetBlueprint<BlueprintFeature>("a0d4a3295224b8f4387464a4447c31d5");

            public override int GetBaseValue(UnitEntityData unit)
            {
                if (!unit.IsPet || unit.Master == null)
                    return 0;

                int value = 0;
                EntityFact masterDamageReduction = unit.Master.GetFact(MadDogMasterDamageReduction);
                if (masterDamageReduction != null)
                {
                    foreach (BlueprintComponentAndRuntime<TTAddDamageResistancePhysical> componentAndRuntime in masterDamageReduction.SelectComponentsWithRuntime<TTAddDamageResistancePhysical>())
                    {
                        value += ((TTAddDamageResistancePhysical.ComponentRuntime)componentAndRuntime.Runtime).GetCurrentValue();
                    }
                }

                return value;
            }
        }
    }
}
