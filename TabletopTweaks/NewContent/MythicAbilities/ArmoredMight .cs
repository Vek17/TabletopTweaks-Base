using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    class ArmoredMight {
        public static void AddArmoredMight() {
            var MythicAbilitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var MagicalVestment = Resources.GetBlueprint<BlueprintAbility>("2d4263d80f5136b4296d6eb43a221d7d");

            var ArmoredMightFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["ArmoredMightFeature"];
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.name = "ArmoredMightFeature";
                bp.m_Icon = MagicalVestment.Icon;
                bp.SetName("Armored Might");
                bp.SetDescription("You treat the armor bonus from your armor as 50% higher than normal, to maximum increase of half your mythic rank plus one.");
                bp.AddComponent(Helpers.Create<ArmoredMightComponent>());
            });
            Resources.AddBlueprint(ArmoredMightFeature);

            if (ModSettings.AddedContent.MythicAbilities.DisableAll || !ModSettings.AddedContent.MythicAbilities.Enabled["ArmorMaster"]) { return; }
            MythicAbilitySelection.m_AllFeatures = MythicAbilitySelection.m_AllFeatures
                .AppendToArray(
                    ArmoredMightFeature.ToReference<BlueprintFeatureReference>()
                );
            ExtraMythicAbilityMythicFeat.m_AllFeatures = ExtraMythicAbilityMythicFeat.m_AllFeatures
                .AppendToArray(
                    ArmoredMightFeature.ToReference<BlueprintFeatureReference>()
                );
        }
    }
}
