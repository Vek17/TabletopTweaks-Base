using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Hunter {
        public static BlueprintFeatureSelection HunterAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var AnimalCompanionSelectionHunter = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("715ac15eb8bd5e342bc8a0a3c9e3e38f");
            var MountTargetFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("cb06f0e72ffb5c640a156bd9f8000c1d");
            var AnimalCompanionArchetypeSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("65af7290b4efd5f418132141aaa36c1b");
            var CompanionBoon = BlueprintTools.GetBlueprint<BlueprintFeature>("8fc01f06eab4dd946baa5bc658cac556");
            var MasterHunter = BlueprintTools.GetBlueprint<BlueprintFeature>("d8a126a3ed3b62943a597c937a4bf840");

            var AnimalCompanionFeatureBear = BlueprintTools.GetBlueprint<BlueprintFeature>("f6f1cdcc404f10c4493dc1e51208fd6f");
            var AnimalCompanionFeatureBoar = BlueprintTools.GetBlueprint<BlueprintFeature>("afb817d80b843cc4fa7b12289e6ebe3d");
            var AnimalCompanionFeatureCentipede = BlueprintTools.GetBlueprint<BlueprintFeature>("f9ef7717531f5914a9b6ecacfad63f46");
            var AnimalCompanionFeatureDog = BlueprintTools.GetBlueprint<BlueprintFeature>("f894e003d31461f48a02f5caec4e3359");
            var AnimalCompanionFeatureElk = BlueprintTools.GetBlueprint<BlueprintFeature>("aa92fea676be33d4dafd176d699d7996");
            var AnimalCompanionFeatureHorse = BlueprintTools.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
            var AnimalCompanionFeatureLeopard = BlueprintTools.GetBlueprint<BlueprintFeature>("2ee2ba60850dd064e8b98bf5c2c946ba");
            var AnimalCompanionFeatureMammoth = BlueprintTools.GetBlueprint<BlueprintFeature>("6adc3aab7cde56b40aa189a797254271");
            var AnimalCompanionFeatureMonitor = BlueprintTools.GetBlueprint<BlueprintFeature>("ece6bde3dfc76ba4791376428e70621a");
            var AnimalCompanionFeatureSmilodon = BlueprintTools.GetBlueprint<BlueprintFeature>("126712ef923ab204983d6f107629c895");
            var AnimalCompanionFeatureTriceratops = BlueprintTools.GetBlueprint<BlueprintFeature>("2d3f409bb0956d44187e9ec8340163f8");
            var AnimalCompanionFeatureVelociraptor = BlueprintTools.GetBlueprint<BlueprintFeature>("89420de28b6bb9443b62ce489ae5423b");
            var AnimalCompanionFeatureWolf = BlueprintTools.GetBlueprint<BlueprintFeature>("67a9dc42b15d0954ca4689b13e8dedea");
            var AnimalCompanionFeatureHorse_PreorderBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");
            var AnimalCompanionFeatureSmilodon_PreorderBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("44f4d77689434e07a5a44dcb65b25f71");
            var AnimalCompanionFeatureTriceratops_PreorderBonu = BlueprintTools.GetBlueprint<BlueprintFeature>("52c854f77105445a9457572ab5826c00");

            var HuntMasterAnimalCompanionRank = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "HuntMasterAnimalCompanionRank", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 20;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Huntmaster Rank");
                CompanionBoon.AddComponent<CompanionBoon>(c => {
                    c.m_RankFeature = bp.ToReference<BlueprintFeatureReference>();
                    c.Bonus = 4;
                });
            });
            var HuntMasterAnimalCompanionProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "HuntMasterAnimalCompanionProgression", bp => {
                bp.SetName(TTTContext, "");
                bp.SetDescription(TTTContext, "");
                //bp.m_Icon = GrandDiscoverySelection.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(5, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            HuntMasterAnimalCompanionRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
            });
            var HuntMasterAnimalCompanionSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "HuntMasterAnimalCompanionSelection", bp => {
                bp.SetName(TTTContext, "Huntmaster");
                bp.SetDescription(TTTContext, "At 20th level, the hunter learns to control all manner of beasts.\n" +
                    "The hunter gains a second animal companion. Her level is considered four lower for the purposes of her second animal companion.");
                bp.m_Icon = AnimalCompanionSelectionHunter.Icon;
                bp.Ranks = 1;
                bp.Group = FeatureGroup.AnimalCompanion;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.AddFeatures(
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureBear, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureBoar, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureCentipede, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureDog, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureElk, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureHorse, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureLeopard, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureMammoth, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureMonitor, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureSmilodon, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureTriceratops, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureVelociraptor, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureWolf, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureHorse_PreorderBonus, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureSmilodon_PreorderBonus, HuntMasterAnimalCompanionRank),
                    CreaterHuntmasterCompanion(AnimalCompanionFeatureTriceratops_PreorderBonu, HuntMasterAnimalCompanionRank)
                );
                bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                    c.Not = true;
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = HuntMasterAnimalCompanionProgression.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = MountTargetFeature.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalCompanionArchetypeSelection.ToReference<BlueprintFeatureReference>();
                });
            });
            HunterAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "HunterAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.m_Icon = MasterHunter.Icon;
                bp.Ranks = 1;
                bp.IgnorePrerequisites = true;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(MasterHunter, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
            BlueprintFeature CreaterHuntmasterCompanion(BlueprintFeature companionFeature, BlueprintFeature rank) {
                return companionFeature.CreateCopy(TTTContext, $"Huntmaster{companionFeature.name}", bp => {
                    bp.RemoveComponents<PrerequisitePet>();
                    bp.GetComponents<AddPet>().ForEach(c => {
                        c.m_LevelRank = rank.ToReference<BlueprintFeatureReference>();
                        c.Type = PetType.AnimalCompanion;
                    });
                });
            }
        }
    }
}
