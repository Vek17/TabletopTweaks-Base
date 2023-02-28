using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class MutatedShape {
        public static void AddMutatedShape() {
            var DemonFirstLevelsProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8afd697daf0d47a4883759a6bc1aff88");
            var Slam1d6 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("767e6932882a99c4b8ca95c88d823137");

            var WildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
            var MajorFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e843ca5ae8e41aea17458fb4c16a15d");
            var FeralChampnionWildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1b60050091002ad458bd49788e84f13a");
            var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
            var ShifterWildShapeFeyFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("24a4fb8991344fd5beb2a1a1a517da87");
            var ShifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
            var ShifterWildShapeManticoreFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("719be33c87f94ed58414ba3eb5a4b664");
         
            var MutatedShapeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MutatedShapeFeature", bp => {
                bp.SetName(TTTContext, "Mutated Shape");
                bp.SetDescription(TTTContext, "Your wild shape form gains an additional appendage you can use to attack your foes.\n" +
                    "When you use wild shape, you gain an additional primary slam attack. This slam can be used as part of a " +
                    "full attack using your highest base attack bonus, and it deals 1d6 bludgeoning damage. " +
                    "This lasts for as long as you stay in the form taken with wild shape.");
                bp.m_Icon = DemonFirstLevelsProgression.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Value = 19;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 6;
                });
                bp.AddPrerequisiteFeaturesFromList(1,
                    WildShapeIWolfFeature,
                    FeralChampnionWildShapeIWolfFeature,
                    MajorFormFeature,
                    ShifterDragonFormFeature,
                    GriffonheartShifterGriffonShapeFakeFeature,
                    ShifterWildShapeFeyFeatureLevelUp,
                    ShifterWildShapeManticoreFeatureLevelUp
                );
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific | FeatureTag.Melee;
                });
                bp.AddComponent<PureRecommendation>(c => {
                    c.Priority = RecommendationPriority.Good;
                });
            });
            var MutatedShapeWeapon = Slam1d6.CreateCopy(TTTContext, "MutatedShapeWeapon", bp => {
                bp.m_DisplayNameText = Helpers.CreateString(TTTContext, $"{bp.name}.Name", "Mutated Shape Slam");
                bp.KeepInPolymorph = true;
            });
            var MutatedShapeEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MutatedShapeEffect", bp => {
                bp.SetName(TTTContext, "Mutated Shape - Gore");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = MutatedShapeFeature.m_Icon;
                bp.HideInUI = true;
                bp.AddComponent<AddAdditionalLimbConditional>(c => {
                    c.m_CheckedFact = MutatedShapeFeature.ToReference<BlueprintUnitFactReference>();
                    c.m_Weapon = MutatedShapeWeapon.ToReference<BlueprintItemWeaponReference>();
                });
            });
            WildShapeTools.WildShapeBuffs.AllBuffs.ForEach(wildShape => {
                wildShape.GetComponents<Polymorph>().ForEach(c => {
                    c.m_Facts = c.m_Facts.AppendToArray(MutatedShapeEffect.ToReference<BlueprintUnitFactReference>());
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("MutatedShape")) { return; }
            FeatTools.AddAsFeat(MutatedShapeFeature);
        }
    }
}
