using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ShieldMastery {
    static class TowerShieldSpecialist {
        internal static void AddTowerShieldSpecialist() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ShieldFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("ac57069b6bf8c904086171683992a92a");
            var TowerShieldProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6105f450bb2acbd458d277e71e19d835");

            var TowerShieldSpecialistTrainingEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TowerShieldSpecialistTrainingEffect", bp => {
                bp.SetName(TTTContext, "Tower Shield Specialist");
                bp.SetDescription(TTTContext, "You wield tower shields with ease.\n" +
                    "Benefit: You reduce the armor check penalty for tower shields by 3, and if you have the armor training class feature, " +
                    "you modify the armor check penalty and maximum Dexterity bonus of tower shields as if they were armor.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<ArmorCheckPenaltyIncrease>(c => {
                    c.Bonus = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.CheckCategory = true;
                    c.Category = ArmorProficiencyGroup.TowerShield;
                });
                bp.AddComponent<ContextArmorMaxDexBonusIncrease>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.TowerShield };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();
                });
            });
            var TowerShieldSpecialistEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TowerShieldSpecialistEffect", bp => {
                bp.SetName(TowerShieldSpecialistTrainingEffect.m_DisplayName);
                bp.SetDescription(TowerShieldSpecialistTrainingEffect.m_Description);
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<ArmorCheckPenaltyIncrease>(c => {
                    c.Bonus = 3;
                    c.CheckCategory = true;
                    c.Category = ArmorProficiencyGroup.TowerShield;
                });
                bp.AddComponent<HasFactFeatureUnlock>(c => {
                    c.m_CheckedFact = ArmorTraining.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = TowerShieldSpecialistTrainingEffect.ToReference<BlueprintUnitFactReference>();
                });
            });
            var TowerShieldSpecialistFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TowerShieldSpecialistFeature", bp => {
                bp.SetName(TowerShieldSpecialistEffect.m_DisplayName);
                bp.SetDescription(TowerShieldSpecialistEffect.m_Description);
                bp.m_Icon = ShieldFocus.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = TowerShieldSpecialistEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.TowerShield
                    };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 8;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 11;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeature(TowerShieldProficiency);
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ShieldFocus);
            });

            if (TTTContext.AddedContent.ShieldMasteryFeats.IsDisabled("TowerShieldSpecialist")) { return; }
            ShieldMastery.AddToShieldMasterySelection(TowerShieldSpecialistFeature);
        }
    }
}
