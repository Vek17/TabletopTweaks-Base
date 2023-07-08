using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class AnimalAlly {

        public static void AddAnimalAlly() {
            var MountTargetFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("cb06f0e72ffb5c640a156bd9f8000c1d");
            var AnimalCompanionArchetypeSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("65af7290b4efd5f418132141aaa36c1b");
            var NatureSoul = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "NatureSoul");
            var AnimalCompanionSelectionRanger = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7");

            var AnimalCompanionFeatureDog = BlueprintTools.GetBlueprint<BlueprintFeature>("f894e003d31461f48a02f5caec4e3359");
            var AnimalCompanionFeatureElk = BlueprintTools.GetBlueprint<BlueprintFeature>("aa92fea676be33d4dafd176d699d7996");
            var AnimalCompanionFeatureHorse = BlueprintTools.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
            var AnimalCompanionFeatureLeopard = BlueprintTools.GetBlueprint<BlueprintFeature>("2ee2ba60850dd064e8b98bf5c2c946ba");
            var AnimalCompanionFeatureMonitor = BlueprintTools.GetBlueprint<BlueprintFeature>("ece6bde3dfc76ba4791376428e70621a");
            var AnimalCompanionFeatureWolf = BlueprintTools.GetBlueprint<BlueprintFeature>("67a9dc42b15d0954ca4689b13e8dedea");
            var AnimalCompanionFeatureHorse_PreorderBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");

            var AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");
            var AnimalAllyRank = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AnimalAllyRank", bp => {
                bp.SetName(TTTContext, "Animal Ally");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 20;
                bp.HideInCharacterSheetAndLevelUp = true;
               
                /*
                bp.AddComponent<ConstrainTargetFeatureRank>(c => {
                    c.TargetFeature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
                });
                */
            });
            var AnimalAllyProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "AnimalAllyProgression", bp => {
                bp.SetName(TTTContext, "Animal Ally Progression");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(4, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            AnimalAllyRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
            });
            var AnimalAllyFeatureSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "AnimalAllyFeatureSelection", bp => {
                bp.SetName(TTTContext, "Animal Ally");
                bp.SetDescription(TTTContext, "You gain an animal companion as if you were a druid of your character level –3. Unlike normal animals of its kind, " +
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
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureDog, AnimalAllyRank),
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureElk, AnimalAllyRank),
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureHorse, AnimalAllyRank),
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureLeopard, AnimalAllyRank),
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureMonitor, AnimalAllyRank),
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureWolf, AnimalAllyRank),
                    CreateAnimalAllyCompanion(AnimalCompanionFeatureHorse_PreorderBonus, AnimalAllyRank)
                );
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalAllyProgression.ToReference<BlueprintFeatureReference>();
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

            BlueprintFeature CreateAnimalAllyCompanion(BlueprintFeature animalCompanion, BlueprintFeature rankFeature) {
                var animalAllyCompanion = animalCompanion.CreateCopy(TTTContext, $"{animalCompanion.name}AnimalAlly", bp => {
                    var oldPet = bp.GetComponent<AddPet>();
                    bp.SetComponents();
                    bp.AddComponent<AddPet>(c => {
                        c.m_Pet = oldPet.m_Pet;
                        c.m_UpgradeFeature = oldPet.m_UpgradeFeature;
                        c.UpgradeLevel = oldPet.UpgradeLevel;
                        c.m_LevelRank = rankFeature.ToReference<BlueprintFeatureReference>();
                        c.m_LevelContextValue = new ContextValue();
                    });
                    bp.AddPrerequisite<PrerequisitePet>(c => {
                        c.NoCompanion = true;
                    });
                });
                return animalAllyCompanion;
            }

            if (TTTContext.AddedContent.Feats.IsDisabled("NatureSoul")) { return; }
            if (TTTContext.AddedContent.Feats.IsDisabled("AnimalAlly")) { return; }
            var CompanionBoon = BlueprintTools.GetBlueprint<BlueprintFeature>("8fc01f06eab4dd946baa5bc658cac556");
            CompanionBoon.TemporaryContext(bp => { 
                bp.AddComponent<CompanionBoon>(c => {
                    c.m_RankFeature = AnimalAllyRank.ToReference<BlueprintFeatureReference>();
                    c.Bonus = 4;
                });
                bp.AddPrerequisiteFeature(AnimalAllyRank, Prerequisite.GroupType.Any);
            });
            FeatTools.AddAsFeat(AnimalAllyFeatureSelection);
        }
    }
}
