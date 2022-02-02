using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace TabletopTweaks.Utilities {
    class PostPatchInitializeAttribute : Attribute {
    }
    class InitializeStaticStringAttribute : Attribute {
    }

    static class PostPatchInitializer {
        public static void Initialize() {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass)
                .SelectMany(x => AccessTools.GetDeclaredMethods(x))
                .Where(x => x.GetCustomAttributes(typeof(PostPatchInitializeAttribute), false).FirstOrDefault() != null);

            foreach (var method in methods) {
                Main.LogDebug($"Executing Post Patch: {method.Name}");
                method.Invoke(null, null); // invoke the method
            }

            var fields = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass)
                .SelectMany(x => AccessTools.GetDeclaredFields(x))
                .Where(x => x.IsStatic)
                .Where(x => x.GetCustomAttributes(typeof(PostPatchInitializeAttribute), false).FirstOrDefault() != null);

            foreach (var field in fields) {
                Main.LogDebug($"Loading Static String: {field.Name}");
                field.GetValue(null); // load the field
            }
        }
    }
}
