namespace TabletopTweaks.Config {
    public interface IDisableableGroup : ICollapseableGroup {
        bool GroupIsDisabled();
        bool SetGroupDisabled(bool value);
    }
}
