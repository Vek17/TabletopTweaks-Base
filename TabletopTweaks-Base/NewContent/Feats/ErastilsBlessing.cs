﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    class ErastilsBlessing {
        public static void AddErastilsBlessing() {
            var ErastilFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("afc775188deb7a44aa4cbde03512c671");
            var WeaponFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            var ZenArcherZenArcheryFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("379c0da9f384e7547a70c259445377f5");
            var CompositeLongbow = BlueprintTools.GetBlueprint<BlueprintWeaponType>("1ac79088a7e5dde46966636a3ac71c35");
            var CompositeShortbow = BlueprintTools.GetBlueprint<BlueprintWeaponType>("011f6f86a0b16df4bbf7f40878c3e80b");
            var Longbow = BlueprintTools.GetBlueprint<BlueprintWeaponType>("7a1211c05ec2c46428f41e3c0db9423f");
            var Shortbow = BlueprintTools.GetBlueprint<BlueprintWeaponType>("99ce02fb54639b5439d07c99c55b8542");
            var Icon_ErastilsBlessingFeat = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ErastilsBlessingFeat.png");

            var ErastilsBlessingFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ErastilsBlessingFeature", bp => {
                bp.SetName(TTTContext, "Erastil's Blessing");
                bp.SetDescription(TTTContext, "Your deity grants you prowess with a bow that far exceeds your own physical capabilities.\n" +
                    "You can use your Wisdom modifier instead of your Dexterity modifier on ranged attack rolls when using a bow.");
                bp.m_Icon = Icon_ErastilsBlessingFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<AttackStatReplacementTTT>(c => {
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
                bp.AddComponent<RecommendationBaseAttackPart>(c => {
                    c.MinPart = 0.7f;
                });
                bp.AddComponent<RecommendationStatComparison>(c => {
                    c.HigherStat = StatType.Wisdom;
                    c.LowerStat = StatType.Dexterity;
                    c.Diff = 2;
                });
                bp.AddComponent<RecommendationStatMiminum>(c => {
                    c.Stat = StatType.Wisdom;
                    c.MinimalValue = 16;
                });
                bp.AddComponent<RecommendationNoFeatFromGroup>(c => {
                    c.m_Features = new BlueprintUnitFactReference[] {
                        ZenArcherZenArcheryFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Ranged;
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("ErastilsBlessing")) { return; }
            FeatTools.AddAsFeat(ErastilsBlessingFeature);
        }
    }
}
