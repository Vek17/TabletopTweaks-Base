using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    static class MythicShatterDefenses {
        public static void AddMythicShatterDefenses() {
            var ShatterDefenses = BlueprintTools.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");
            var ShatterDefensesMythicBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "ShatterDefensesMythicBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName(TTTContext, "Shattered Defenses (Mythic)");
                bp.SetDescription(TTTContext, "An opponent affected by Shatter Defenses is flat-footed to all attacks.");
                bp.AddComponent<ForceFlatFooted>();
            });
            var ShatterDefensesMythicFeat = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ShatterDefensesMythicFeat", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName(TTTContext, "Shatter Defenses (Mythic)");
                bp.SetDescription(TTTContext, "Your dazzling attacks leave your opponents flummoxed and bewildered, unable to attack you or to defend themselves effectively.\n" +
                    "An opponent you affect with Shatter Defenses is flat-footed to all attacks, not just yours.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(ShatterDefenses);
            });
            if (TTTContext.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }
            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicShatterDefenses")) { return; }
            FeatTools.AddAsMythicFeat(ShatterDefensesMythicFeat);
        }
    }
}
