using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Races {
    static class Halfling {

        private static readonly BlueprintRace HalflingRace = Resources.GetBlueprint<BlueprintRace>("b0c3ef2729c498f47970bb50fa1acd30");
        private static readonly BlueprintFeatureSelection HalflingHeritageSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b3bebe76e6c64e2ca11585f9e3e2554a");
        private static readonly BlueprintFeature HalflingLuck = Resources.GetBlueprint<BlueprintFeature>("84ffa66048d26b14c800a425199f9886");
        private static readonly BlueprintFeature KeenSenses = Resources.GetBlueprint<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
        private static readonly BlueprintFeature SureFooted = Resources.GetBlueprint<BlueprintFeature>("0fe5db70b50cd894c849fc764c80bbb9");
        private static readonly BlueprintFeature SlowSpeedHalfling = Resources.GetBlueprint<BlueprintFeature>("b8926aeaac17dc7408a5059788255819");
        private static readonly BlueprintFeature Fearless = Resources.GetBlueprint<BlueprintFeature>("39144c817b70467499cc32e3cff59d81");

        private static readonly BlueprintFeature CravenHalfling = Resources.GetBlueprint<BlueprintFeature>("889fa46af27148f9b9aefa27b7a29a2e");
        private static readonly BlueprintFeature HastyHalfling = Resources.GetBlueprint<BlueprintFeature>("f2570b5becc54d659ca786488d107e69");

        private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = Resources.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

        public static void AddHalflingHeritage() {

            var HalflingAbilityModifiers = Helpers.CreateBlueprint<BlueprintFeature>("HalflingAbilityModifiers", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Halfling Ability Modifiers");
                bp.SetDescriptionTagged("Halflings are nimble and strong-willed, but their small stature makes them weaker than other "
                    + "races. They gain +2 Dexterity, +2 Charisma, and –2 Strength.");
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
            });
            var HalflingNoAlternateTrait = Helpers.CreateBlueprint<BlueprintFeature>("HalflingNoAlternateTrait", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("None");
                bp.SetDescription("No Alternate Trait");
            });
            var HalflingBruiserFeature = Helpers.CreateBlueprint<BlueprintFeature>("HalflingBruiserFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Halfling Bruiser");
                bp.SetDescriptionTagged("A lifetime of brutal survival, either under the heavy burdens of slavery or on the "
                    + "streets, has made some halflings more adept at taking blows than dodging them. Halflings with this racial "
                    + "trait gain +2 Constitution, +2 Charisma, and -2 Dexterity. This racial trait alters the halflings’ ability score modifiers.");
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddTraitReplacment(HalflingAbilityModifiers);
                bp.AddSelectionCallback(HalflingHeritageSelection);
            });

            CravenHalfling.RemoveComponents<RemoveFeatureOnApply>();
            CravenHalfling.AddTraitReplacment(Fearless);
            CravenHalfling.AddTraitReplacment(HalflingLuck);
            CravenHalfling.AddSelectionCallback(HalflingHeritageSelection);

            HastyHalfling.RemoveComponents<RemoveFeatureOnApply>();
            HastyHalfling.AddTraitReplacment(SureFooted);
            HastyHalfling.AddTraitReplacment(Fearless);
            HastyHalfling.AddTraitReplacment(SlowSpeedHalfling);
            HastyHalfling.AddSelectionCallback(HalflingHeritageSelection);

            if (ModSettings.AddedContent.Races.DisableAll || !ModSettings.AddedContent.Races.Enabled["HalflingHomebrewHeritage"]) { return; }
            HalflingRace.SetComponents(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = HalflingAbilityModifiers.ToReference<BlueprintFeatureReference>();
            }));
            HalflingHeritageSelection.SetName("Alternate Traits");
            HalflingHeritageSelection.SetDescription("The following alternate traits are available.");
            HalflingHeritageSelection.Group = FeatureGroup.KitsuneHeritage;
            HalflingHeritageSelection.SetFeatures(
                HalflingNoAlternateTrait,
                HalflingBruiserFeature,
                CravenHalfling,
                HastyHalfling
            );
        }

        private static void AddTraitReplacment(this BlueprintFeature feature, BlueprintFeature replacement) {
            feature.AddComponent(Helpers.Create<RemoveFeatureOnApply>(c => {
                c.m_Feature = replacement.ToReference<BlueprintUnitFactReference>();
            }));
            feature.AddPrerequisiteFeature(replacement);
        }

        private static void AddSelectionCallback(this BlueprintFeature feature, BlueprintFeatureSelection selection) {
            feature.AddComponent(Helpers.Create<AddAdditionalRacialFeatures>(c => {
                c.Features = new BlueprintFeatureBaseReference[] { selection.ToReference<BlueprintFeatureBaseReference>() };
            }));
        }
    }
}
