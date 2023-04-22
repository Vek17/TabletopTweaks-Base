using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class ImprovedNaturalAttack {
        public static void AddImprovedNaturalAttack() {

            var ImprovedNaturalAttack = Helpers.CreateBlueprint<BlueprintParametrizedFeature>(TTTContext, "ImprovedNaturalAttack", bp => {
                bp.SetName(TTTContext, "Improved Natural Attack");
                bp.SetDescription(TTTContext, "Attacks made by one of this creature’s natural attacks leave vicious wounds.\n" +
                    "Choose one of the creature’s natural attack forms. The damage for this natural attack increases by one step, as if the creature’s size had increased by one category.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.ParameterType = FeatureParameterType.WeaponCategory;
                bp.WeaponSubCategory = WeaponSubCategory.Natural;
                bp.m_Prerequisite = new BlueprintParametrizedFeatureReference();
                bp.HideNotAvailibleInUI = true;
                bp.AddComponent<WeaponChangeSizeParametrized>(c => {
                    c.SizeChange = 1;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 4;
                });
                bp.AddPrerequisite<PrerequisiteIsPet>(c => {
                    c.HideInUI = true;
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("ImprovedNaturalAttack")) { return; }
            FeatTools.AddAsFeat(ImprovedNaturalAttack);
        }
    }
}
