using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicFeats {
    class MythicShatterDefenses {
        public static void AddMythicShatterDefenses() {
            var ShatterDefenses = Resources.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");
            var ShatterDefensesMythicBuff = Helpers.CreateBuff("ShatterDefensesMythicBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName("Shattered Defenses (Mythic)");
                bp.SetDescription("An opponent affected by Shatter Defenses is flat-footed to all attacks.");
            });
            var ShatterDefensesMythicFeat = Helpers.CreateBlueprint<BlueprintFeature>("ShatterDefensesMythicFeat", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName("Shatter Defenses (Mythic)");
                bp.SetDescription("An opponent you affect with Shatter Defenses is flat-footed to all attacks, not just yours.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(ShatterDefenses);
            });
            if (ModSettings.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }
            if (ModSettings.AddedContent.MythicFeats.IsDisabled("MythicShatterDefenses")) { return; }
            FeatTools.AddAsMythicFeat(ShatterDefensesMythicFeat);
        }
    }
}
