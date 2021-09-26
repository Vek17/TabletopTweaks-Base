using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static Kingmaker.Blueprints.Classes.BlueprintProgression;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Utilities.Helpers;

namespace TabletopTweaks.NewContent.Feats
{
    static class AnimalAlly
    {
        const string RangerAnimalCompanionProgressionGuid = "152450aedc0788e41b4f9e745c091437";
        static BlueprintFeature AnimalCompanionRank;
        static BlueprintFeature NatureSoul;
        static BlueprintFeatureSelection RangerAnimalCompanionSelection;
        static BlueprintFeature CompanionBoon;
        //static BlueprintFeature AnimalAllyRankFeature;
        static BlueprintProgression AnimalAllyProgression;

        static void InitializeUsedBlueprints()
        {
            AnimalCompanionRank = Resources.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");
            NatureSoul = Resources.GetBlueprint<BlueprintFeature>("181cc2b3-420f-4b60-9e93-57831e1e20f9");
            RangerAnimalCompanionSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7");
            CompanionBoon = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8fc01f06eab4dd946baa5bc658cac556");
            //AnimalAllyRankFeature = CreateAnimalAllyRankFeature();
            AnimalAllyProgression = CreateAnimalAllyProgression();
        }

        public static void AddAnimalAlly()
        {
            try
            {
                InitializeUsedBlueprints();
                var AnimalAlly = CreateAnimalAllySelectionFeat();

                if (ModSettings.AddedContent.Feats.IsDisabled("AnimalAlly")) { return; }
                FeatTools.AddAsFeat(AnimalAlly);
            }
            catch (Exception ex)
            {
                Main.Error($"Exception has occured during Animal Ally initialization! | {ex}");
            }
        }

        private static BlueprintFeatureSelection CreateAnimalAllySelectionFeat()
        {
            var AnimalAlly = CreateBlueprint<BlueprintFeatureSelection>("AnimalAllyFeatureSelection", x =>
            {
                x.m_DisplayName = CreateString("AnimalAlly.Name", "Animal Ally");
                x.m_Description = CreateString("AnimalAlly.Description", "You gain an animal companion as if you were a druid of your character level –3. Unlike normal animals of its kind, an animal companion's Hit Dice, abilities, skills, and feats advance as you advance in level.");
                x.ReapplyOnLevelUp = false;
                x.IsClassFeature = false;
                x.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                x.Mode = SelectionMode.OnlyNew;
                x.Group = FeatureGroup.AnimalCompanion;
                x.Ranks = 1;
                x.m_Features = RangerAnimalCompanionSelection.m_Features.ToArray();
                x.m_AllFeatures = RangerAnimalCompanionSelection.m_AllFeatures.ToArray();
                x.m_Icon = RangerAnimalCompanionSelection.m_Icon;
                x.Components = RangerAnimalCompanionSelection.Components.ToArray();
                x.IsPrerequisiteFor = new List<BlueprintFeatureReference>();
                // GUID != m_GUID !!!!!

                AddFeatureOnApply AddAnimalAllyCompanionProgression = new AddFeatureOnApply
                {
                    m_Feature = AnimalAllyProgression.ToReference<BlueprintFeatureReference>()
                };
                foreach (var comp in x.Components.OfType<AddFeatureOnApply>())
                {
                    Main.Log($"Example blueprint raw guid: {comp.m_Feature.guid}");
                    Main.Log($"Example blueprint non-raw guid: {comp.m_Feature.Guid}");
                }

                if (x.Components.FirstOrDefault(x => x is AddFeatureOnApply add
                    && add.m_Feature.Guid == RangerAnimalCompanionProgressionGuid) == default)
                    Main.Log("Couldn't replace the progression!");
                x.ReplaceComponent(comp =>
                    comp is AddFeatureOnApply add
                    && add.m_Feature.Guid == RangerAnimalCompanionProgressionGuid, AddAnimalAllyCompanionProgression);

                x.AddPrerequisiteFeature(NatureSoul, GroupType.All);
                x.AddPrerequisite<PrerequisiteCharacterLevel>(x =>
                {
                    x.Level = 4;
                    x.Group = GroupType.All;
                });
                x.AddPrerequisite<PrerequisiteNoFeature>(x =>
                {
                    x.m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
                    x.Group = GroupType.All;
                });
            });

            return AnimalAlly;
        }

        private static BlueprintProgression CreateAnimalAllyProgression()
        {
            var AnimalAllyProgression = CreateBlueprint<BlueprintProgression>("AnimalAllyProgression", x =>
            {
                x.Ranks = 1;
                x.IsClassFeature = false;
                x.GiveFeaturesForPreviousLevels = true;
                x.m_DisplayName = new LocalizedString();
                x.m_Description = new LocalizedString();
                x.m_DescriptionShort = new LocalizedString();
                x.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                x.m_FeatureRankIncrease = new BlueprintFeatureReference();
                //x.m_FeatureRankIncrease = AnimalAllyRankFeature.ToReference<BlueprintFeatureReference>();
                //x.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                x.LevelEntries = Enumerable.Range(4, 17)
                    .Select(i => new LevelEntry
                    {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference>
                        {
                            //AnimalAllyRankFeature.ToReference<BlueprintFeatureBaseReference>()
                            AnimalCompanionRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
            });

            return AnimalAllyProgression;
        }

        //private static BlueprintFeature CreateAnimalAllyRankFeature()
        //{
        //    var AnimalAllyRankFeature = CreateBlueprint<BlueprintFeature>("AnimalAllyRank", x =>
        //    {
        //        x.Ranks = 20;
        //        x.IsClassFeature = false;
        //        x.HideInUI = true;
        //        x.m_DisplayName = CreateString("AnimalRankFeature.Name", "Animal Companion");
        //        x.m_Description = CreateString("AnimalRankFeature.Description", "Animal Companion");
        //        x.m_Icon = AnimalCompanionRank.m_Icon;
        //        x.IsPrerequisiteFor = new List<BlueprintFeatureReference>
        //        {
        //            CompanionBoon.ToReference<BlueprintFeatureReference>()
        //        };
        //    });

        //    return AnimalAllyRankFeature;
        //}
    }
}

// TODO - fix prog
// TODO - check the ui
// TODO - check if interactions with other animal allies check out
// TODO - lint
// TODO - reorganize
// TODO - check how to fetch exisiting localized string
// TODO - set up the environment?
// at least debugger and separate installs for dev and stable
