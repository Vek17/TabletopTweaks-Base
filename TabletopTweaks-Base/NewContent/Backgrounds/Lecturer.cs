using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Backgrounds {
    static class Lecturer {
        public static void AddLecturer() {
            var BackgroundsScholarSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("273fab44409035f42a7e2af0858a463d");

            var BackgroundScholarLecturer = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BackgroundScholarLecturer", bp => {
                bp.SetName(TTTContext, "Lecturer");
                bp.SetDescription(TTTContext, "Lecturer adds Knowledge (World) and Persuasion to the list of her class skills. " +
                    "She can also use her Intelligence instead of Charisma while attempting Persuasion checks.\n" +
                    "If the character already has the class skill, weapon proficiency or armor " +
                    "proficiency granted by the selected background from her class during character creation, then the corresponding bonuses " +
                    "from background change to a +1 competence bonus in case of skills, a +1 enhancement bonus in case of weapon proficiency and a -1 Armor " +
                    "Check Penalty reduction in case of armor proficiency.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                });
                bp.AddComponent<AddClassSkill>(c => {
                    c.Skill = StatType.SkillPersuasion;
                });
                bp.AddComponent<ReplaceStatBaseAttribute>(c => {
                    c.TargetStat = StatType.SkillPersuasion;
                    c.BaseAttributeReplacement = StatType.Intelligence;
                });
                bp.AddComponent<AddBackgroundClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                });
                bp.AddComponent<AddBackgroundClassSkill>(c => {
                    c.Skill = StatType.SkillPersuasion;
                });
            });
            if (TTTContext.AddedContent.Backgrounds.IsDisabled("Lecturer")) { return; }
            BackgroundsScholarSelection.AddFeatures(BackgroundScholarLecturer);
        }
    }
}
