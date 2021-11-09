using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicFeats {
    class MythicShatterDefenses {
        public static void AddMythicShatterDefenses() {
            var ShatterDefenses = Resources.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");
            var ShatterDefensesMythicBuff = Helpers.CreateBuff("ShatterDefensesMythicBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName("16e17f11cdfd4546831377b38f7428b1", "Shattered Defenses (Mythic)");
                bp.SetDescription("9c84b3539dd340ce911ca6d739922662", "An opponent affected by Shatter Defenses is flat-footed to all attacks.");
                bp.AddComponent<ForceFlatFooted>();
            });
            var ShatterDefensesMythicFeat = Helpers.CreateBlueprint<BlueprintFeature>("ShatterDefensesMythicFeat", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName("1c66f9151364472e830c48c1e4321f75", "Shatter Defenses (Mythic)");
                bp.SetDescription("4d0bc1e66f914df29ac006ec249dcb03", "An opponent you affect with Shatter Defenses is flat-footed to all attacks, not just yours.");
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
