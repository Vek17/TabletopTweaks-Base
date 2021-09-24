using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Config {
    public class SettingGroup {
        public bool DisableAll = false;
        public SortedDictionary<string, bool> Enabled = new SortedDictionary<string, bool>();
        public virtual bool this[string key] => IsEnabled(key);

        public void LoadSettingGroup(SettingGroup group, bool frozen) {
            DisableAll = group.DisableAll;
            if (frozen) {
                this.Enabled.Keys.ToList().ForEach(key => {
                    Enabled[key] = false;
                });
            }
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
