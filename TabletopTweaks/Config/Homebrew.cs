
namespace TabletopTweaks.Config {
    class Homebrew : IUpdatableSettings {
        public bool NewSettingsOffByDefault = false;
        public MythicReworkGroup MythicReworks = new MythicReworkGroup();

        public void Init() {
            MythicReworks.Init();
        }

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Homebrew;
            MythicReworks.LoadMythicReworkGroup(loadedSettings.MythicReworks, NewSettingsOffByDefault);
        }
    }
    public class MythicReworkGroup : IDisableableGroup, ICollapseableGroup {
        public bool IsExpanded = true;
        ref bool ICollapseableGroup.IsExpanded() => ref IsExpanded;
        public void SetExpanded(bool value) => IsExpanded = value;
        public bool DisableAll = false;
        public bool GroupIsDisabled() => DisableAll;
        public void SetGroupDisabled(bool value) => DisableAll = value;
        public NestedSettingGroup Aeon;
        public NestedSettingGroup Angel;
        public NestedSettingGroup Azata;
        public NestedSettingGroup Demon;
        public NestedSettingGroup Lich;
        public NestedSettingGroup Trickster;
        public NestedSettingGroup Devil;
        public NestedSettingGroup GoldDragon;
        public NestedSettingGroup Legend;
        public NestedSettingGroup Swarm;

        public MythicReworkGroup() {
            Aeon = new NestedSettingGroup(this);
            Angel = new NestedSettingGroup(this);
            Azata = new NestedSettingGroup(this);
            Demon = new NestedSettingGroup(this);
            Lich = new NestedSettingGroup(this);
            Trickster = new NestedSettingGroup(this);
            Devil = new NestedSettingGroup(this);
            GoldDragon = new NestedSettingGroup(this);
            Legend = new NestedSettingGroup(this);
            Swarm = new NestedSettingGroup(this);
        }

        public void Init() {
            Aeon.Parent = this;
            Angel.Parent = this;
            Azata.Parent = this;
            Demon.Parent = this;
            Lich.Parent = this;
            Trickster.Parent = this;
            Devil.Parent = this;
            GoldDragon.Parent = this;
            Legend.Parent = this;
            Swarm.Parent = this;
        }

        public void LoadMythicReworkGroup(MythicReworkGroup group, bool frozen) {
            DisableAll = group.DisableAll;
            Aeon.LoadSettingGroup(group.Aeon, frozen);
            Angel.LoadSettingGroup(group.Angel, frozen);
            Azata.LoadSettingGroup(group.Azata, frozen);
            Demon.LoadSettingGroup(group.Demon, frozen);
            Lich.LoadSettingGroup(group.Lich, frozen);
            Trickster.LoadSettingGroup(group.Trickster, frozen);
            Devil.LoadSettingGroup(group.Devil, frozen);
            GoldDragon.LoadSettingGroup(group.GoldDragon, frozen);
            Legend.LoadSettingGroup(group.Legend, frozen);
            Swarm.LoadSettingGroup(group.Swarm, frozen);
        }
    }
}
