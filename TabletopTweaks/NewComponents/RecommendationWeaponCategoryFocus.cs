using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintAbility), false)]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [TypeId("29c2f378da99404a948453b832deb097")]
    class RecommendationWeaponCategoryFocus : LevelUpRecommendationComponent {
        public override RecommendationPriority GetPriority([CanBeNull] LevelUpState levelUpState) {
            if (levelUpState == null) {
                return RecommendationPriority.Same;
            }
            RecommendationPriority recommendationPriority = this.BadIfNoFocus ? RecommendationPriority.Bad : RecommendationPriority.Same;
            foreach (Feature feature in levelUpState.Unit.Progression.Features) {
                if (feature?.Param?.Value?.WeaponCategory == Catagory) {
                    return HasFocus ? RecommendationPriority.Good : recommendationPriority;
                }
            }
            if (!HasFocus) {
                return RecommendationPriority.Good;
            }
            return recommendationPriority;
        }

#pragma warning disable 0649
        public WeaponCategory Catagory;
        public bool HasFocus;
        public bool BadIfNoFocus;
#pragma warning restore 0649
    }
}
