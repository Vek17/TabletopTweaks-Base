using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class MountedSkirmisher {
        public static void AddMountedSkirmisher() {
            var Icon_MountedSkirmisher = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_MountedSkirmisher.png");
            var MountedCombat = BlueprintTools.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
            var TrickRiding = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "TrickRiding");
            var MountedSkirmisher = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MountedSkirmisher", bp => {
                bp.SetName(TTTContext, "Mounted Skirmisher");
                bp.SetDescription(TTTContext, "If your mount moves its speed or less, you can still take a full-attack action.");
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

            if (TTTContext.AddedContent.Feats.IsDisabled("MountedSkirmisher")) { return; }
            if (TTTContext.Fixes.BaseFixes.IsDisabled("MountedActions")) { return; }
            FeatTools.AddAsFeat(MountedSkirmisher);
        }
    }
}
