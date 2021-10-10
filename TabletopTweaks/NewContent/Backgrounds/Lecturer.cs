using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Backgrounds {
    static class Lecturer {
        public static void AddLecturer() {
            var BackgroundsScholarSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("273fab44409035f42a7e2af0858a463d");

            var BackgroundScholarLecturer = Helpers.CreateBlueprint<BlueprintFeature>("BackgroundScholarLecturer", bp => {
                bp.SetName("Lecturer");
                bp.SetDescription("Lecturer adds Knowledge (World) and Persuasion to the list of her class skills. " +
                    "She can also use her Intelligence instead of Charisma while attempting Persuasion checks\n" +
                    "If the character already has the class skill, {g|Encyclopedia:Weapon_Proficiency}weapon proficiency{/g} or armor " +
                    "proficiency granted by the selected background from her class during character creation, then the corresponding {g|Encyclopedia:Bonus}bonuses{/g} " +
                    "from background change to a +1 competence bonus in case of skills, a +1 enhancement bonus in case of weapon proficiency and a -1 Armor {g|Encyclopedia:Check}" +
                    "Check{/g} {g|Encyclopedia:Penalty}Penalty{/g} reduction in case of armor proficiency.");
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.AddComponent<AddClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                });
                bp.AddComponent<AddClassSkill>(c => {
                    c.Skill = StatType.SkillPersuasion;
                });
                bp.AddComponent<AddBackgroundClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                });
                bp.AddComponent<AddBackgroundClassSkill>(c => {
                    c.Skill = StatType.SkillPersuasion;
                });
                bp.AddComponent<ReplaceStatBaseAttribute>(c => {
                    c.TargetStat = StatType.SkillPersuasion;
                    c.BaseAttributeReplacement = StatType.Intelligence;
                });
            });
            if (ModSettings.AddedContent.Backgrounds.IsDisabled("Lecturer")) { return; }
            BackgroundsScholarSelection.AddFeatures(BackgroundScholarLecturer);
        }
    }
}
