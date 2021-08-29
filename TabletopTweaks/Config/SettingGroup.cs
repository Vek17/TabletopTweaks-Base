using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class SettingGroup {
        public bool DisableAll = false;
        public SortedDictionary<string, bool> Enabled = new SortedDictionary<string, bool>();

        public void LoadSettingGroup(SettingGroup group) {
            DisableAll = group.DisableAll;
            group.Enabled.ForEach(entry => {
                if (Enabled.ContainsKey(entry.Key)) {
                    Enabled[entry.Key] = entry.Value;
                }
            });
        }
        public virtual bool IsEnabled(string key) {
            if (!Enabled.TryGetValue(key, out bool result)) {
                Main.LogDebug($"COULD NOT FIND SETTING KEY: {key}");
            }
            return result && !DisableAll;
        }
        public virtual bool IsDisabled(string key) {
            return !IsEnabled(key);
        }
    }
}
