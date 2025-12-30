using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PatchBlueprintsCacheInitPriority : Attribute {
    public int Priority;
    public PatchBlueprintsCacheInitPriority(int priority) {
        Priority = priority;
    }
}