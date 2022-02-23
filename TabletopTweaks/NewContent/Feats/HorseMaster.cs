using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
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
            var CavalierMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
            var CavalierClass = Resources.GetBlueprint<BlueprintCharacterClass>("3adc3439f98cb534ba98df59838f02c7");
            var CavalierProgression = Resources.GetBlueprint<BlueprintProgression>("aa70326bdaa7015438df585cf2ab93b9");
            var BeastRiderArchetype = Resources.GetBlueprint<BlueprintArchetype>("d287fbf2495ff2e4d88d5c49217bf173");
            var DiscipleOfThePikeArchetype = Resources.GetBlueprint<BlueprintArchetype>("4c4c3f9df00a5e04680d172a290111c4");
            var KnightOfTheWallArchetype = Resources.GetBlueprint<BlueprintArchetype>("112dd0e61f95c3a459ac0a565472e685");

            var ExpertTrainer = Helpers.CreateBlueprint<BlueprintFeature>("ExpertTrainer", bp => {
                bp.SetName("Expert Trainer");
                bp.SetDescription("At 4th level, a cavalier learns to train mounts with speed and unsurpassed expertise.");
                bp.m_Icon = CavalierMountSelection.Icon;
                bp.IsClassFeature = true;
            });

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
                bp.SetDescription("You blend horsemanship skills from disparate traditions into a seamless mounted combat technique.\n" +
                    "Use your character level to determine your effective druid level for determining the powers and abilities of your mount.");
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.MountedCombatFeat };
                bp.m_Icon = AnimalCompanionSelectionRanger.m_Icon;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            HorseMasterRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddPrerequisiteFeature(ExpertTrainer);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillMobility;
                    c.Value = 6;
                });
                bp.AddPrerequisite<PrerequisitePet>();
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("NatureSoul")) { return; }
            FeatTools.AddAsFeat(HorseMaster);
            CavalierProgression.LevelEntries
                .Where(entry => entry.Level == 4)
                .First().Features.Add(ExpertTrainer);
            CavalierProgression.UIGroups = CavalierProgression.UIGroups
                .AppendToArray(Helpers.CreateUIGroup(CavalierMountSelection, ExpertTrainer));

            BeastRiderArchetype.RemoveFeatures = BeastRiderArchetype.RemoveFeatures
                .AppendToArray(Helpers.CreateLevelEntry(4, ExpertTrainer));
            DiscipleOfThePikeArchetype.RemoveFeatures = DiscipleOfThePikeArchetype.RemoveFeatures
                .AppendToArray(Helpers.CreateLevelEntry(4, ExpertTrainer));
            KnightOfTheWallArchetype.RemoveFeatures = KnightOfTheWallArchetype.RemoveFeatures
                .AppendToArray(Helpers.CreateLevelEntry(4, ExpertTrainer));

            SaveGameFix.AddUnitPatch((unit) => {
                if (unit.Progression.GetClassLevel(CavalierClass) >= 4
                && !unit.Progression.GetClassData(CavalierClass).Archetypes
                    .Any(achetype => achetype == BeastRiderArchetype
                        || achetype == DiscipleOfThePikeArchetype
                        || achetype == KnightOfTheWallArchetype)) {
                    if (!unit.HasFact(ExpertTrainer)) {
                        if (unit.AddFact(ExpertTrainer) != null) {
                            Main.Log($"Added: {ExpertTrainer.name} To: {unit.CharacterName}");
                            return;
                        }
                        Main.Log($"Failed Add: {ExpertTrainer.name} To: {unit.CharacterName}");
                    }
                }
            });
        }
    }
}
