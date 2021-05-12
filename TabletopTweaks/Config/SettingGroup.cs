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
    }
}
