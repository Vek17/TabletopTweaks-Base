using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.Feats {
    static class MountedSkirmisher {
        public static void AddMountedSkirmisher() {
            var Icon_MountedSkirmisher = AssetLoader.LoadInternal("Feats", "Icon_MountedSkirmisher.png");
            var MountedCombat = Resources.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
            var TrickRiding = Resources.GetModBlueprint<BlueprintFeature>("TrickRiding");
            var MountedSkirmisher = Helpers.CreateBlueprint<BlueprintFeature>("MountedSkirmisher", bp => {
                bp.SetName("Mounted Skirmisher");
                bp.SetDescription("If your mount moves its speed or less, you can still take a full-attack action.");
                bp.m_Icon = Icon_MountedSkirmisher;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.MountedCombatFeat };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.MountedSkirmisher;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SkillMobility;
                    c.Value = 14;
                });
                bp.AddPrerequisiteFeature(MountedCombat);
                bp.AddPrerequisiteFeature(TrickRiding);
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("MountedSkirmisher")) { return; }
            if (ModSettings.Fixes.BaseFixes.IsDisabled("MountedActions")) { return; }
            FeatTools.AddAsFeat(MountedSkirmisher);
        }
    }
}
