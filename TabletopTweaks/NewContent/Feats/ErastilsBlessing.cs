using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    class ErastilsBlessing {
        private static readonly BlueprintFeature ErastilFeature = Resources.GetBlueprint<BlueprintFeature>("afc775188deb7a44aa4cbde03512c671");
        private static readonly BlueprintFeature WeaponFocus = Resources.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
        private static readonly BlueprintWeaponType CompositeLongbow = Resources.GetBlueprint<BlueprintWeaponType>("1ac79088a7e5dde46966636a3ac71c35");
        private static readonly BlueprintWeaponType CompositeShortbow = Resources.GetBlueprint<BlueprintWeaponType>("011f6f86a0b16df4bbf7f40878c3e80b");
        private static readonly BlueprintWeaponType Longbow = Resources.GetBlueprint<BlueprintWeaponType>("7a1211c05ec2c46428f41e3c0db9423f");
        private static readonly BlueprintWeaponType Shortbow = Resources.GetBlueprint<BlueprintWeaponType>("99ce02fb54639b5439d07c99c55b8542");

        public static void AddErastilsBlessing() {
            var ErastilsBlessingFeature = Helpers.CreateBlueprint<BlueprintFeature>("ErastilsBlessingFeature", bp => {
                bp.SetName("Erastil's Blessing");
                bp.SetDescription("Your deity grants you prowess with a bow that far exceeds your own physical capabilities.\n" +
                    "You can use your Wisdom modifier instead of your Dexterity modifier on ranged attack rolls when using a bow.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<AttackStatReplacementEnforced>(c => {
                    c.ReplacementStat = StatType.Wisdom;
                    c.SubCategory = WeaponSubCategory.Ranged;
                    c.CheckWeaponTypes = true;
                    c.m_WeaponTypes = new BlueprintWeaponTypeReference[] {
                        CompositeLongbow.ToReference<BlueprintWeaponTypeReference>(),
                        CompositeShortbow.ToReference<BlueprintWeaponTypeReference>(),
                        Longbow.ToReference<BlueprintWeaponTypeReference>(),
                        Shortbow.ToReference<BlueprintWeaponTypeReference>()
                    };
                });
                bp.AddPrerequisiteFeature(ErastilFeature);
                bp.AddPrerequisite<PrerequisiteParametrizedFeature>(c => {
                    c.m_Feature = WeaponFocus.ToReference<BlueprintFeatureReference>();
                    c.ParameterType = FeatureParameterType.WeaponCategory;
                    c.WeaponCategory = WeaponCategory.Longbow;
                });
                bp.AddComponent(Helpers.Create<RecommendationBaseAttackPart>(c => {
                    c.MinPart = 0.7f;
                }));
                bp.AddComponent(Helpers.Create<RecommendationStatComparison>(c => {
                    c.HigherStat = StatType.Wisdom;
                    c.LowerStat = StatType.Dexterity;
                    c.Diff = 2;
                }));
                bp.AddComponent(Helpers.Create<RecommendationStatMiminum>(c => {
                    c.Stat = StatType.Wisdom;
                    c.MinimalValue = 16;
                }));
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Ranged;
                });
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("ErastilsBlessing")) { return; }
            FeatTools.AddAsFeat(ErastilsBlessingFeature);
        }
    }
}
