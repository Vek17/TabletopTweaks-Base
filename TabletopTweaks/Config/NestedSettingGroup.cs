using JetBrains.Annotations;

namespace TabletopTweaks.Config {
    public class NestedSettingGroup : SettingGroup {
        private IDisableableGroup parent;
        public IDisableableGroup Parent { set { parent = value; } }

        public NestedSettingGroup([NotNull] IDisableableGroup parent) {
            this.parent = parent;
        }

        public override bool GroupIsDisabled() => parent.GroupIsDisabled() || base.GroupIsDisabled();
        public override void SetGroupDisabled(bool value) {
            if (!parent.GroupIsDisabled()) {
                base.SetGroupDisabled(value);
            }
        }

        public override bool IsEnabled(string key) {
            return base.IsEnabled(key) && !GroupIsDisabled();
        }
        public override bool IsDisabled(string key) {
            return !IsEnabled(key);
        }
        public override void ChangeSetting(string key, bool value) {
            if (GroupIsDisabled()) {
                return;
            }
            Settings[key].Enabled = value;
        }
    }
}
