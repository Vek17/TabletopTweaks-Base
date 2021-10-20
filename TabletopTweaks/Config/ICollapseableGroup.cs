namespace TabletopTweaks.Config {
    public interface ICollapseableGroup {
        ref bool IsExpanded();
        void SetExpanded(bool value);
    }
}
