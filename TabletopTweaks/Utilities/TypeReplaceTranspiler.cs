using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TabletopTweaks.Utilities {
    public class TypeReplaceTranspiler {
        private Dictionary<Type, Type> m_typeMapping;
        private Dictionary<MethodInfo, MethodInfo> m_staticCallMapping;

        public TypeReplaceTranspiler(Dictionary<Type, Type> typeMapping, Dictionary<MethodInfo, MethodInfo> staticCallMapping) {
            m_typeMapping = typeMapping;
            m_staticCallMapping = staticCallMapping;
        }

        public IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> codes, ILGenerator il) {
            bool replaceLoc0 = false;
            LocalBuilder loc0replacement = null;

            bool replaceLoc1 = false;
            LocalBuilder loc1replacement = null;

            bool replaceLoc2 = false;
            LocalBuilder loc2replacement = null;

            bool replaceLoc3 = false;
            LocalBuilder loc3replacement = null;

            Dictionary<Type, LocalBuilder> locSReplacements = new Dictionary<Type, LocalBuilder>();

            foreach (LocalVariableInfo local in original.GetMethodBody().LocalVariables) {
                if (m_typeMapping.ContainsKey(local.LocalType)) {
                    LocalBuilder newLocal = il.DeclareLocal(m_typeMapping[local.LocalType]);
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
                    if (m_typeMapping.ContainsKey(method.DeclaringType)) {
                        replaceMethod = true;
                        newMethod = AccessTools.Method(
                            m_typeMapping[method.DeclaringType],
                            method.Name,
                            method.GetParameters().Select(pi => pi.ParameterType).ToArray(),
                            method.GetGenericArguments());
                    }
                    if (method.IsGenericMethod) {
                        Type[] genericArguments = method.GetGenericArguments();
                        List<Type> replacementGenerics = new List<Type>();
                        bool replaceGenerics = false;
                        for (int i = 0; i < genericArguments.Length; i++) {
                            if (m_typeMapping.ContainsKey(genericArguments[i])) {
                                replaceGenerics = true;
                                replacementGenerics.Add(m_typeMapping[genericArguments[i]]);
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
                } else if (code.opcode == OpCodes.Call && m_staticCallMapping.ContainsKey((MethodInfo)code.operand)) {
                    yield return new CodeInstruction(OpCodes.Call, m_staticCallMapping[(MethodInfo)code.operand]);
                } else if (code.opcode == OpCodes.Ldfld) {
                    FieldInfo field = (FieldInfo)code.operand;
                    if (m_typeMapping.ContainsKey(field.DeclaringType)) {
                        FieldInfo newField = AccessTools.Field(
                            m_typeMapping[field.DeclaringType],
                            field.Name);

                        yield return new CodeInstruction(OpCodes.Ldfld, newField);
                    } else {
                        yield return code;
                    }
                } else if (code.opcode == OpCodes.Castclass && m_typeMapping.ContainsKey((Type)code.operand)) {
                    yield return new CodeInstruction(OpCodes.Castclass, m_typeMapping[(Type)code.operand]);
                } else {
                    yield return code;
                }
            }
        }
    }
}
