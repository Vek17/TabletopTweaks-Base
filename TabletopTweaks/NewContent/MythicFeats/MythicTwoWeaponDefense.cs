using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicFeats {
    class MythicTwoWeaponDefense {
        public static void AddMythicTwoWeaponDefense() {
            var Icon_TwoWeaponDefense = AssetLoader.LoadInternal("Feats", "Icon_TwoWeaponDefense.png");
            var TwoWeaponDefenseFeature = Resources.GetModBlueprint<BlueprintFeature>("TwoWeaponDefenseFeature");

            var TwoWeaponDefenseMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>("TwoWeaponDefenseMythicFeature", bp => {
                bp.SetName("Two-Weapon Defense (Mythic)");
                bp.SetDescription("When using Two-Weapon Defense, you apply the highest enhancement " +
                    "bonus from your two weapons to the shield bonus granted by that feat.");
                bp.m_Icon = Icon_TwoWeaponDefense;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(TwoWeaponDefenseFeature);
            });

            if (ModSettings.AddedContent.MythicFeats.IsDisabled("MythicTwoWeaponDefense")) { return; }
            FeatTools.AddAsMythicFeat(TwoWeaponDefenseMythicFeature);
        }
    }
}
