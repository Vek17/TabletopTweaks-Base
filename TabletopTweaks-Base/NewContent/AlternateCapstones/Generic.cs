using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal static class Generic {
        public static BlueprintFeature GreatBeastMasterFeature = null;
        public static BlueprintProgression OldDogNewTricksProgression = null;
        public static BlueprintProgression PerfectBodyFlawlessMindProgression = null;
        public static void AddAlternateCapstones() {
            var PerfectBodyFlawlessMindStrengthIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectBodyFlawlessMindStrengthIncrease", bp => {
                bp.SetName(TTTContext, "Strength");
                bp.SetDescription(TTTContext, "Increase your strength score by 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 16;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Strength;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                });
            });
            var PerfectBodyFlawlessMindDexterityIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectBodyFlawlessMindDexterityIncrease", bp => {
                bp.SetName(TTTContext, "Dexterity");
                bp.SetDescription(TTTContext, "Increase your dexterity score by 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 16;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                });
            });
            var PerfectBodyFlawlessMindConstitutionIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectBodyFlawlessMindConstitutionIncrease", bp => {
                bp.SetName(TTTContext, "Constitution");
                bp.SetDescription(TTTContext, "Increase your constitution score by 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 16;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Constitution;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                });
            });
            var PerfectBodyFlawlessMindIntelligenceIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectBodyFlawlessMindIntelligenceIncrease", bp => {
                bp.SetName(TTTContext, "Intelligence");
                bp.SetDescription(TTTContext, "Increase your intelligence score by 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 16;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                });
            });
            var PerfectBodyFlawlessMindWisdomIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectBodyFlawlessMindWisdomIncrease", bp => {
                bp.SetName(TTTContext, "Wisdom");
                bp.SetDescription(TTTContext, "Increase your wisdom score by 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 16;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                });
            });
            var PerfectBodyFlawlessMindCharismaIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectBodyFlawlessMindCharismaIncrease", bp => {
                bp.SetName(TTTContext, "Charisma");
                bp.SetDescription(TTTContext, "Increase your charisma score by 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 16;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Charisma;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                });
            });
            var PerfectBodyFlawlessMindSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "PerfectBodyFlawlessMindSelection", bp => {
                bp.SetName(TTTContext, "Stat Increase");
                bp.SetDescription(TTTContext, "Select bonus stat.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddFeatures(
                    PerfectBodyFlawlessMindStrengthIncrease,
                    PerfectBodyFlawlessMindDexterityIncrease,
                    PerfectBodyFlawlessMindConstitutionIncrease,
                    PerfectBodyFlawlessMindIntelligenceIncrease,
                    PerfectBodyFlawlessMindWisdomIncrease,
                    PerfectBodyFlawlessMindCharismaIncrease
                );
            });
            PerfectBodyFlawlessMindProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "PerfectBodyFlawlessMindProgression", bp => {
                bp.SetName(TTTContext, "Perfect Body, Flawless Mind");
                bp.SetDescription(TTTContext, "At 20th level, the character’s endless training and study has resulted in an unmatched mastery of the self. " +
                    "The character increases her ability scores by a collective total of 8. " +
                    "For example, she can increase one score by 8, or one score by 5 and another by 3, or four scores by 2, and so on. " +
                    "Characters of any class can select this ability.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(13, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(14, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(15, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(16, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(17, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(18, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(19, PerfectBodyFlawlessMindSelection),
                    Helpers.CreateLevelEntry(20, PerfectBodyFlawlessMindSelection)
                };
            });

            var GreatBeastPetFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "GreatBeastPetFeature", bp => {
                bp.SetName(TTTContext, "Great Beast");
                bp.SetDescription(TTTContext, "At 20th level, the character’s animal companion is a paragon of its kind—a hero and legend in its own right.\n" +
                    "The animal companion’s Strength, Dexterity, Constitution, and Wisdom scores each increase by 4. This capstone is available to any class with an animal companion.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Strength;
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Constitution;
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Charisma;
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            GreatBeastMasterFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "GreatBeastMasterFeature", bp => {
                bp.SetName(TTTContext, "Great Beast");
                bp.SetDescription(TTTContext, "At 20th level, the character’s animal companion is a paragon of its kind—a hero and legend in its own right.\n" +
                    "The animal companion’s Strength, Dexterity, Constitution, and Wisdom scores each increase by 4. This capstone is available to any class with an animal companion.");
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;
                bp.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = GreatBeastPetFeature.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisite<PrerequisitePet>();
            });

            var FighterFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
            OldDogNewTricksProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "OldDogNewTricksProgression", bp => {
                bp.SetName(TTTContext, "Old Dog, New Tricks");
                bp.SetDescription(TTTContext, "At 20th level, the character shows that a true warrior always has one more surprise the enemy hasn’t seen.\n" +
                    "The character gains four combat feats. This capstone is available to characters of any class that gains at least four bonus combat feats.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(17, FighterFeatSelection),
                    Helpers.CreateLevelEntry(18, FighterFeatSelection),
                    Helpers.CreateLevelEntry(19, FighterFeatSelection),
                    Helpers.CreateLevelEntry(20, FighterFeatSelection)
                };
            });
        }
    }
}
