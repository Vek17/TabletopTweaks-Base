using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class Blueprints: IUpdatableSettings {
        public bool OverrideIds = false;
        public SortedDictionary<string, string> NewBlueprints = new SortedDictionary<string, string>();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Blueprints;
            if (loadedSettings.OverrideIds) {
                OverrideIds = loadedSettings.OverrideIds;
                loadedSettings.NewBlueprints.ForEach(entry => {
                    if (NewBlueprints.ContainsKey(entry.Key)) {
                        NewBlueprints[entry.Key] = entry.Value;
                    }
                });
            }
        }
    }
}
