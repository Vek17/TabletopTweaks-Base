using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Utilities.Helpers;

namespace TabletopTweaks.NewContent.Feats
{
    static class AnimalAlly
    {
        public static void AddAnimalAlly()
        {
            var NatureSoul = Resources.GetBlueprint<BlueprintFeature>("181cc2b3-420f-4b60-9e93-57831e1e20f9");
            var RangerAnimalCompanionSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7");
            var RangerAnimalCompanionProgression = Resources.GetBlueprint<BlueprintProgression>("152450aedc0788e41b4f9e745c091437");
            BlueprintFeature AnimalCompanionRank = Resources.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

            BlueprintProgression AnimalAllyCompanionProgression = CreateBlueprint<BlueprintProgression>("AnimalAllyCompanionProgression", bp =>
            {
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Ranks = 1;
                bp.LevelEntries = Enumerable.Range(4, 16).Select(i => new LevelEntry
                {
                    Level = i - 3,
                    m_Features = new List<BlueprintFeatureBaseReference>
                    {
                        AnimalCompanionRank.ToReference<BlueprintFeatureBaseReference>()
                    }
                }).ToArray();
            });
            AddFeatureOnApply AddAnimalAllyCompanionProgression = new AddFeatureOnApply
            {
                m_Feature = AnimalAllyCompanionProgression.ToReference<BlueprintFeatureReference>()
            };

            var AnimalAlly = FeatTools.CreateExtraSelectionFeat("AnimalAlly", RangerAnimalCompanionSelection, bp =>
            {
                bp.SetName("Animal Ally");
                bp.SetDescription("You gain an animal companion as if you were a druid of your character level –3. Unlike normal animals of its kind, an animal companion's Hit Dice, abilities, skills, and feats advance as you advance in level.");
                bp.RemovePrerequisites<Prerequisite>(x => true);
                bp.AddPrerequisiteFeature(NatureSoul, GroupType.All);
                var fourthLevelRequired = new PrerequisiteCharacterLevel
                {
                    Level = 4,
                    Group = GroupType.All
                };
                bp.AddComponent(fourthLevelRequired);
                var noAnimalCompanionRequired = new PrerequisiteNoFeature
                {
                    m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>(),
                    Group = GroupType.All
                };
                bp.AddComponent(noAnimalCompanionRequired);
                bp.ReplaceComponent(x =>
                    x is AddFeatureOnApply apl
                    && apl.m_Feature == RangerAnimalCompanionProgression.ToReference<BlueprintFeatureReference>(), AddAnimalAllyCompanionProgression);
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("AnimalAlly")) { return; }
            FeatTools.AddAsFeat(AnimalAlly);
        }
    }
}
