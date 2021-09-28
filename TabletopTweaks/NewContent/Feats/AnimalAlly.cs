using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class AnimalAlly {

        public static void AddAnimalAlly() {
            var MountTargetFeature = Resources.GetBlueprint<BlueprintFeature>("cb06f0e72ffb5c640a156bd9f8000c1d");
            var AnimalCompanionArchetypeSelection = Resources.GetBlueprint<BlueprintFeature>("65af7290b4efd5f418132141aaa36c1b");
            var AnimalCompanionRank = Resources.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");
            var NatureSoul = Resources.GetModBlueprint<BlueprintFeature>("NatureSoul");
            var AnimalCompanionSelectionRanger = Resources.GetBlueprint<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7");

            var AnimalCompanionFeatureDog = Resources.GetBlueprint<BlueprintFeature>("f894e003d31461f48a02f5caec4e3359");
            var AnimalCompanionFeatureElk = Resources.GetBlueprint<BlueprintFeature>("aa92fea676be33d4dafd176d699d7996");
            var AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
            var AnimalCompanionFeatureLeopard = Resources.GetBlueprint<BlueprintFeature>("2ee2ba60850dd064e8b98bf5c2c946ba");
            var AnimalCompanionFeatureMonitor = Resources.GetBlueprint<BlueprintFeature>("ece6bde3dfc76ba4791376428e70621a");
            var AnimalCompanionFeatureWolf = Resources.GetBlueprint<BlueprintFeature>("67a9dc42b15d0954ca4689b13e8dedea");
            var AnimalCompanionFeatureHorse_PreorderBonus = Resources.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");

            var AnimalAllyProgression = Helpers.CreateBlueprint<BlueprintProgression>("AnimalAllyProgression", bp => {
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.m_DisplayName = new LocalizedString();
                bp.m_Description = new LocalizedString();
                bp.m_DescriptionShort = new LocalizedString();
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeatureRankIncrease = new BlueprintFeatureReference();
                bp.LevelEntries = Enumerable.Range(5, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            AnimalCompanionRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
            });

            var AnimalAlly = Helpers.CreateBlueprint<BlueprintFeatureSelection>("AnimalAllyFeatureSelection", bp => {
                bp.SetName("Animal Ally");
                bp.SetDescription("You gain an animal companion as if you were a druid of your character level –3. Unlike normal animals of its kind, " +
                    "an animal companion's Hit Dice, abilities, skills, and feats advance as you advance in level.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.Mode = SelectionMode.OnlyNew;
                bp.Group = FeatureGroup.AnimalCompanion;
                bp.Ranks = 1;
                bp.m_Icon = AnimalCompanionSelectionRanger.m_Icon;
                bp.IsPrerequisiteFor = new List<BlueprintFeatureReference>();
                bp.AddFeatures(
                    AnimalCompanionFeatureDog,
                    AnimalCompanionFeatureElk,
                    AnimalCompanionFeatureHorse,
                    AnimalCompanionFeatureLeopard,
                    AnimalCompanionFeatureMonitor,
                    AnimalCompanionFeatureWolf,
                    AnimalCompanionFeatureHorse_PreorderBonus
                );
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalAllyProgression.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = MountTargetFeature.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalCompanionArchetypeSelection.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisiteFeature(NatureSoul);
                bp.AddPrerequisite<PrerequisiteCharacterLevel>(p => {
                    p.Level = 4;
                });
                bp.AddPrerequisite<PrerequisitePet>(p => {
                    p.NoCompanion = true;
                });
                bp.AddPrerequisite<PrerequisiteIsPet>(p => {
                    p.Not = true;
                    p.HideInUI = true;
                });
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("NatureSoul")) { return; }
            if (ModSettings.AddedContent.Feats.IsDisabled("AnimalAlly")) { return; }
            FeatTools.AddAsFeat(AnimalAlly);
        }
    }
}
