using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class DervishDance {
        public static void AddDervishDance() {
            var WeaponFinesse = BlueprintTools.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");
            var PointBlankShot = BlueprintTools.GetBlueprint<BlueprintFeature>("0da0c194d6e1d43419eb8d990b28e0ab");
            var PreciseShot = BlueprintTools.GetBlueprint<BlueprintFeature>("8f3d1e6b4be006f4d896081f2f889665");
            var SlashingGrace = BlueprintTools.GetBlueprint<BlueprintFeature>("697d64669eb2c0543abb9c9b07998a38");
            var FencingGrace = BlueprintTools.GetBlueprint<BlueprintFeature>("47b352ea0f73c354aba777945760b441");
            var DeftStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("b63a316cb172c7b4e906a318a0621c2c");
            var FinesseTrainingSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b78d146cea711a84598f0acef69462ea");
            var Scimitar = BlueprintTools.GetBlueprint<BlueprintWeaponType>("d9fbec4637d71bd4ebc977628de3daf3");

            var DervishDance = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DervishDance", bp => {
                bp.SetName(TTTContext, "Dervish Dance");
                bp.SetDescription(TTTContext, "When wielding a scimitar with one hand, you can use your Dexterity modifier instead of your Strength modifier " +
                    "on melee attack and damage rolls. You treat the scimitar as a one-handed piercing weapon for all feats and class abilities that " +
                    "require such a weapon (such as a duelist’s precise strike ability). The scimitar must be for a creature of your size. You cannot " +
                    "use this feat if you are carrying a weapon or shield (other than a buckler) in your off hand.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent(Helpers.Create<AttackStatReplacementTTT>(c => {
                    c.ReplacementStat = StatType.Dexterity;
                    c.m_WeaponTypes = new BlueprintWeaponTypeReference[] {
                        Scimitar.ToReference<BlueprintWeaponTypeReference>()
                    };
                    c.CheckWeaponTypes = true;
                }));
                bp.AddComponent(Helpers.Create<DamageGraceTTT>(c => {
                    c.Category = WeaponCategory.Scimitar;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillMobility;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Value = 13;
                }));
                bp.AddComponent(Helpers.Create<RecommendationBaseAttackPart>(c => {
                    c.MinPart = 0.7f;
                }));
                bp.AddComponent(Helpers.Create<RecommendationStatMiminum>(c => {
                    c.Stat = StatType.Dexterity;
                    c.MinimalValue = 16;
                }));
                bp.AddComponent(Helpers.Create<RecommendationNoFeatFromGroup>(c => {
                    c.m_Features = new BlueprintUnitFactReference[] {
                        PointBlankShot.ToReference<BlueprintUnitFactReference>(),
                        PreciseShot.ToReference<BlueprintUnitFactReference>(),
                        SlashingGrace.ToReference<BlueprintUnitFactReference>(),
                        FencingGrace.ToReference<BlueprintUnitFactReference>(),
                        DeftStrike.ToReference<BlueprintUnitFactReference>(),
                        FinesseTrainingSelection.ToReference<BlueprintUnitFactReference>()
                    };
                }));
                bp.AddComponent(Helpers.Create<RecommendationWeaponCategoryFocus>(c => {
                    c.Catagory = WeaponCategory.Scimitar;
                    c.HasFocus = true;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
                }));
                bp.AddPrerequisiteFeature(WeaponFinesse);
            });
            FixRecommendations(SlashingGrace, DervishDance);
            FixRecommendations(FencingGrace, DervishDance);
            if (TTTContext.AddedContent.Feats.IsDisabled("DervishDance")) { return; }
            FeatTools.AddAsFeat(DervishDance);
        }
        private static void FixRecommendations(BlueprintFeature feature, BlueprintFeature avoid) {
            feature.AddComponent(Helpers.Create<RecommendationNoFeatFromGroup>(c => {
                c.m_Features = new BlueprintUnitFactReference[] {
                        avoid.ToReference<BlueprintUnitFactReference>()
                };
            }));
        }
    }
}
