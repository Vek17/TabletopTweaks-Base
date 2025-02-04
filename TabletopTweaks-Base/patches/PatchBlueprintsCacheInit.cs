using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PatchBlueprintsCacheInit : Attribute {
    public int Priority;
    private static Dictionary<int, Action> PostfixDelegates = new();
    private static List<Action> PostfixDelegatesOrdered = new();
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

                if (method.GetCustomAttribute<PatchBlueprintsCacheInitPostfix>() != null || method.Name.Trim().Equals("Postfix")) {
                    var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
                    if (!PostfixDelegates.ContainsKey(priority))
                        PostfixDelegates[priority] = del;
                    else
                        PostfixDelegates[priority] += del;
                }
            }
        }
    }
    public static void CreateDynamicPatches(Harmony instance) {
        CollectPatches();
        MethodInfo toPatch = AccessTools.Method(typeof(BlueprintsCache), nameof(BlueprintsCache.Init));
        MethodInfo postfix = AccessTools.Method(typeof(PatchBlueprintsCacheInit), nameof(PatchBlueprintsCacheInit.ExecutePostfixes));

        foreach (int priority in PostfixDelegates.Keys.OrderByDescending(k => k)) {
            PostfixDelegatesOrdered.Add(PostfixDelegates[priority]);
            instance.Patch(toPatch, postfix: new(postfix, priority));
        }
    }

    public static void ExecutePostfixes() {
        PostfixDelegatesOrdered[PostfixPriorityCounter]();
        PostfixPriorityCounter++;
    }
}
