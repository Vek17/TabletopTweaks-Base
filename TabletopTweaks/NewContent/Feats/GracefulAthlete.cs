using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    class GracefulAthlete {
        public static void AddGracefulAthlete() {

            var GracefulAthlete = Helpers.Create<BlueprintFeature>(bp => {
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("GracefulAthlete");
                bp.name = "GracefulAthlete";
                bp.SetName("Graceful Athlete");
                bp.SetDescriptionTagged("Add your Dexterity modifier instead of your Strength bonus to Athletics checks. This feat grants no benefit " +
                    "to creatures that already add their Dexterity modifier to Athletics checks (such as all Tiny or smaller creatures).");
                bp.AddComponent(Helpers.Create<ReplaceStatBaseAttribute>(c => {
                    c.TargetStat = StatType.SkillAthletics;
                    c.BaseAttributeReplacement = StatType.Dexterity;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteStatBonus>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Descriptor = ModifierDescriptor.Racial;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillAthletics;
                    c.Value = 1;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillMobility;
                    c.Value = 1;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Skills;
                }));
            });
            Resources.AddBlueprint(GracefulAthlete);
            if (ModSettings.AddedContent.Feats.DisableAll || !ModSettings.AddedContent.Feats.Enabled["GracefulAthlete"]) { return; }
            FeatTools.AddAsFeat(GracefulAthlete);
            FeatTools.AddAsRogueTalent(GracefulAthlete);
        }
    }
}
