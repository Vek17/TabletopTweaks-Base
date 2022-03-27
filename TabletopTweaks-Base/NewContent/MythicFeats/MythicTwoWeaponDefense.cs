using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    class MythicTwoWeaponDefense {
        public static void AddMythicTwoWeaponDefense() {
            var Icon_TwoWeaponDefense = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_TwoWeaponDefense.png");
            var TwoWeaponDefenseFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "TwoWeaponDefenseFeature");

            var TwoWeaponDefenseMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TwoWeaponDefenseMythicFeature", bp => {
                bp.SetName(TTTContext, "Two-Weapon Defense (Mythic)");
                bp.SetDescription(TTTContext, "Your graceful flow between attack and defense makes you difficult to hit.\n" +
                    "When using Two-Weapon Defense, you apply the highest enhancement " +
                    "bonus from your two weapons to the shield bonus granted by that feat.");
                bp.m_Icon = Icon_TwoWeaponDefense;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(TwoWeaponDefenseFeature);
            });

            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicTwoWeaponDefense")) { return; }
            FeatTools.AddAsMythicFeat(TwoWeaponDefenseMythicFeature);
        }
    }
}
