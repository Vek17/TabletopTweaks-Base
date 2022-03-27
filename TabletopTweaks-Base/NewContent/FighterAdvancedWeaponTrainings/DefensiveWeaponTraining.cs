using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.FighterAdvancedWeaponTrainings {
    public class DefensiveWeaponTraining {
        public static void AddDefensiveWeaponTraining() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");

            var DefensiveWeaponTrainingFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DefensiveWeaponTrainingFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WeaponTraining };
                bp.SetName(TTTContext, "Defensive Weapon Training");
                bp.SetDescription(TTTContext, "The fighter gains a +1 shield bonus to his Armor Class. The fighter adds half his weapon’s enhancement bonus (if any) to this shield bonus. " +
                    "When his weapon training bonus for weapons from the associated fighter weapon group reaches +4, this shield bonus increases to +2. " +
                    "This shield bonus is lost if the fighter is immobilized or helpless.");
                bp.AddComponent<DefensiveWeaponTrainingComponent>();
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                });
            });
            if (TTTContext.AddedContent.FighterAdvancedWeaponTraining.IsDisabled("DefensiveWeaponTraining")) { return; }
            AdvancedWeapontrainingSelection.AddToAdvancedWeaponTrainingSelection(DefensiveWeaponTrainingFeature);
        }
    }
}
