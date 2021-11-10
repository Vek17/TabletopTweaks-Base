using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedWeaponTrainings {
    class DefensiveWeaponTraining {
        public static void AddDefensiveWeaponTraining() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");

            var DefensiveWeaponTrainingFeature = Helpers.CreateBlueprint<BlueprintFeature>("DefensiveWeaponTrainingFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WeaponTraining };
                bp.SetName("Defensive Weapon Training");
                bp.SetDescription("The fighter gains a +1 shield bonus to his Armor Class. The fighter adds half his weapon’s enhancement bonus (if any) to this shield bonus. " +
                    "When his weapon training bonus for weapons from the associated fighter weapon group reaches +4, this shield bonus increases to +2. " +
                    "This shield bonus is lost if the fighter is immobilized or helpless.");
                bp.AddComponent(Helpers.Create<DefensiveWeaponTrainingComponent>());
                bp.AddPrerequisite(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                }));
            });
            if (ModSettings.AddedContent.FighterAdvancedWeaponTraining.IsDisabled("DefensiveWeaponTraining")) { return; }
            AdvancedWeapontrainingSelection.AddToAdvancedWeaponTrainingSelection(DefensiveWeaponTrainingFeature);
        }
    }
}
