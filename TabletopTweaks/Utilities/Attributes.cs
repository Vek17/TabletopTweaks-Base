using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace TabletopTweaks.Utilities {
    class PostPatchInitializeAttribute : Attribute {
    }

    static class PostPatchInitializer { 
        public static void Initialize() {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass)
                .SelectMany(x => AccessTools.GetDeclaredMethods(x))
                .Where(x => x.GetCustomAttributes(typeof(PostPatchInitializeAttribute), false).FirstOrDefault() != null);

            foreach (var method in methods){
                method.Invoke(null, null); // invoke the method
            }
        }
    }
}
