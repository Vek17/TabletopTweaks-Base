using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PatchBlueprintsCacheInit : Attribute {
    public int Priority;
    private static Dictionary<int, Action> PrefixDelegates = new();
    private static Dictionary<int, Action> PostfixDelegates = new();
    // :(
    private static List<Action> PrefixDelegatesOrdered = new();
    private static List<Action> PostfixDelegatesOrdered = new();
    private static int PrefixPriorityCounter = 0;
    private static int PostfixPriorityCounter = 0;
    public PatchBlueprintsCacheInit(int priority) {
        Priority = priority;
    }

    public PatchBlueprintsCacheInit() {
        Priority = HarmonyLib.Priority.Normal;
    }
    public static void CollectPatches() {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Select(t => (t, t.GetCustomAttribute<PatchBlueprintsCacheInit>())).Where(p => p.Item2 != null);

        foreach (var type in types) {
            var methods = type.Item1.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var def = type.Item2.Priority;
            foreach (var method in methods) {
                var priority = method.GetCustomAttribute<PatchBlueprintsCacheInitPriority>()?.Priority ?? def;

                /* We actually want to throw when the parameters don't match since that means the system needs to be expanded 
                if (method.GetParameters().Length > 0)
                    continue;
                */

                var del = (Action)Delegate.CreateDelegate(typeof(Action), method);

                if (method.GetCustomAttribute<HarmonyPrefix>() != null || method.Name == "Prefix") {
                    if (!PrefixDelegates.ContainsKey(priority))
                        PrefixDelegates[priority] = del;
                    else
                        PrefixDelegates[priority] += del;
                }

                if (method.GetCustomAttribute<HarmonyPostfix>() != null || method.Name == "Postfix") {
                    if (!PostfixDelegates.ContainsKey(priority))
                        PostfixDelegates[priority] = del;
                    else
                        PostfixDelegates[priority] += del;
                }
            }
        }
    }
    // :(
    public static void CreateDynamicPatches(Harmony instance) {
        MethodInfo toPatch = AccessTools.Method(typeof(BlueprintsCache), nameof(BlueprintsCache.Init));
        MethodInfo prefix = AccessTools.Method(typeof(PatchBlueprintsCacheInit), nameof(PatchBlueprintsCacheInit.ExecutePrefixes));
        MethodInfo postfix = AccessTools.Method(typeof(PatchBlueprintsCacheInit), nameof(PatchBlueprintsCacheInit.ExecutePostfixes));

        var combinedDelegates = PrefixDelegates.Keys.OrderBy(k => k).Zip(PostfixDelegates.Keys.OrderBy(k => k), (prefixPriority, postfixPriority) => new {
            Prefix = PrefixDelegates[prefixPriority],
            Postfix = PostfixDelegates[postfixPriority],
            PrefixPriority = prefixPriority,
            PostfixPriority = postfixPriority
        }); 
        foreach (var delegatePair in combinedDelegates) {
            PrefixDelegatesOrdered.Add(delegatePair.Prefix);
            PostfixDelegatesOrdered.Add(delegatePair.Postfix);

            instance.Patch(toPatch,
                prefix: new(prefix, delegatePair.PrefixPriority),
                postfix: new(postfix, delegatePair.PostfixPriority)
            );
        }
        if (PrefixDelegates.Count > PostfixDelegates.Count) {
            foreach (int remainingPrefixPriority in PrefixDelegates.Keys.OrderBy(k => k).Skip(PrefixDelegatesOrdered.Count)) {
                instance.Patch(toPatch, prefix: new(prefix, remainingPrefixPriority));
            }
        } else if (PostfixDelegates.Count > PrefixDelegates.Count) {
            foreach (int remainingPostfixPriority in PostfixDelegates.Keys.OrderBy(k => k).Skip(PostfixDelegatesOrdered.Count)) {
                instance.Patch(toPatch, postfix: new(postfix, remainingPostfixPriority));
            }
        }
    }

    public static void ExecutePrefixes() {
        PrefixDelegatesOrdered[PrefixPriorityCounter]();
        PrefixPriorityCounter++;
    }

    public static void ExecutePostfixes() {
        PostfixDelegatesOrdered[PostfixPriorityCounter]();
        PostfixPriorityCounter++;
    }
}
