using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class HorseMaster {
        public static void AddHorseMaster() {
            var AnimalCompanionSelectionRanger = Resources.GetBlueprint<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7");
            var CavalierClass = Resources.GetBlueprint<BlueprintCharacterClass>("3adc3439f98cb534ba98df59838f02c7");

            var AnimalCompanionRank = Resources.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");
            var HorseMasterRank = Helpers.CreateBlueprint<BlueprintFeature>("HorseMasterRank", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 20;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("Horse Master Rank");
                bp.SetDescription("");
                bp.AddComponent<ConstrainTargetFeatureRank>(c => {
                    c.TargetFeature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
                });
            });

            var HorseMaster = Helpers.CreateBlueprint<BlueprintProgression>("HorseMaster", bp => {
                bp.SetName("Horse Master");
                bp.SetDescription("Use your character level to determine your effective druid level for determining the powers and abilities of your mount.");
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.MountedCombatFeat };
                bp.m_Icon = AnimalCompanionSelectionRanger.m_Icon;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeatureRankIncrease = new BlueprintFeatureReference();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            HorseMasterRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = CavalierClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 4;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillMobility;
                    c.Value = 6;
                });
                bp.AddPrerequisite<PrerequisitePet>();
            });
            
            if (ModSettings.AddedContent.Feats.IsDisabled("NatureSoul")) { return; }
            FeatTools.AddAsFeat(HorseMaster);
        }
    }
}
