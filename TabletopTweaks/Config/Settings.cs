using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TabletopTweaks.Extensions;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Config {
    static class Settings {
        public static ModEntry ModEntry;
        private static Fixes fixes;
        public static Fixes Fixes {
            get {
                if (fixes == null) {
                    LoadSettings();
                }
                return fixes;
            }
        }
        private static AddedContent addedContent;
        public static AddedContent AddedContent {
            get {
                if (addedContent == null) {
                    LoadSettings();
                }
                return addedContent;
            }
        }
        private static IEnumerable<BlueprintScriptableObject> blueprints;
        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
            }
            return blueprints.OfType<T>();
        }
        static private IEnumerable<BlueprintBuff> polymorphBuffs;
        static public IEnumerable<BlueprintBuff> PolymorphBuffs {
            get {
                if (polymorphBuffs == null) {
                    Main.LogHeader($"Identifying Polymorph Buffs");
                    IEnumerable<BlueprintBuff> taggedPolyBuffs = Settings.GetBlueprints<BlueprintBuff>()
                        .Where(bp => bp.GetComponents<SpellDescriptorComponent>()
                            .Where(c => (c.Descriptor & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0);
                    polymorphBuffs = Settings.GetBlueprints<BlueprintAbility>()
                        .Where(bp =>
                            (bp.GetComponents<SpellDescriptorComponent>()
                                .Where(c => c != null)
                                .Where(d => d.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0)
                            || (bp.GetComponents<AbilityExecuteActionOnCast>()
                                .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>())
                                .Where(c => c.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0)
                            || (bp.GetComponents<AbilityEffectRunAction>()
                                .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>()
                                    .Concat(c.Actions.Actions.OfType<ContextActionConditionalSaved>()
                                        .SelectMany(a => a.Failed.Actions
                                        .OfType<ContextActionRemoveBuffsByDescriptor>())))
                                .Where(c => c.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0))
                        .SelectMany(a => a.FlattenAllActions())
                        .OfType<ContextActionApplyBuff>()
                        .Where(c => c.Buff != null)
                        .Select(c => c.Buff)
                        .Concat(taggedPolyBuffs)
                        .Where(bp => bp.AssetGuid != "e6f2fc5d73d88064583cb828801212f4") // Fatigued
                        .Where(bp => !bp.HasFlag(BlueprintBuff.Flags.HiddenInUi))
                        .Distinct();

                    polymorphBuffs
                        .OrderBy(c => c.name)
                        .ForEach(c => Main.LogPatch("PolymorphBuff Found", c));
                    Main.LogHeader($"Identified: {polymorphBuffs.Count()} Polymorph Buffs");
                }
                return polymorphBuffs;
            }
        }

        public static void LoadSettings() {
            var assembly = Assembly.GetExecutingAssembly();
            var fixesResource = "TabletopTweaks.Config.Fixes.json";
            var addedContentResource = "TabletopTweaks.Config.AddedContent.json";
            string userConfigFolder = ModEntry.Path + "UserSettings";
            string userFixPath = userConfigFolder + $"{Path.DirectorySeparatorChar}Fixes.json";
            string userAddedContentPath = userConfigFolder + $"{Path.DirectorySeparatorChar}AddedContent.json";

            using (Stream stream = assembly.GetManifestResourceStream(fixesResource))
            using (StreamReader reader = new StreamReader(stream)) {
                fixes = JsonConvert.DeserializeObject<Fixes>(reader.ReadToEnd());
            }
            using (Stream stream = assembly.GetManifestResourceStream(addedContentResource))
            using (StreamReader reader = new StreamReader(stream)) {
                addedContent = JsonConvert.DeserializeObject<AddedContent>(reader.ReadToEnd());
            }
            Directory.CreateDirectory(userConfigFolder);

            if (File.Exists(userFixPath)) {
                using (StreamReader reader = File.OpenText(userFixPath)) {
                    try {
                        Fixes userFixes = JsonConvert.DeserializeObject<Fixes>(reader.ReadToEnd());
                        fixes.OverrideFixes(userFixes);
                    }
                    catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userFixPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_Fixes.json", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userFixPath, JsonConvert.SerializeObject(fixes, Formatting.Indented));

            if (File.Exists(userAddedContentPath)) {
                using (StreamReader reader = File.OpenText(userAddedContentPath)) {
                    try {
                        AddedContent userAddedContent = JsonConvert.DeserializeObject<AddedContent>(reader.ReadToEnd());
                        addedContent.OverrideFixes(userAddedContent);
                    }
                    catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userAddedContentPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_AddedContent.json", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userAddedContentPath, JsonConvert.SerializeObject(addedContent, Formatting.Indented));
        }
    }
}
