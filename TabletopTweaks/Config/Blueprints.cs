using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    class Blueprints {
        public bool OverrideIds = false;
        public SortedDictionary<string, string> NewBlueprints = new SortedDictionary<string, string>();

        public void OverrideSettings(Blueprints userSettings) {
            if (userSettings.OverrideIds) {
                OverrideIds = userSettings.OverrideIds;
                userSettings.NewBlueprints.ForEach(entry => {
                    if (NewBlueprints.ContainsKey(entry.Key)) {
                        NewBlueprints[entry.Key] = entry.Value;
                    }
                });
            }
        }
    }
}
