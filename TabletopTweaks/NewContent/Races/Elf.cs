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
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Races {
    class Elf {
        public static void AddElfHeritage() {
            var KitsuneHeritageSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ec40cc350b18c8c47a59b782feb91d1f");
            var ElfRace = Resources.GetBlueprint<BlueprintRace>("25a5878d125338244896ebd3238226c8");
            var DestinyBeyondBirthMythicFeat = Resources.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

            var ElfHeritageDefaultFeature = Helpers.CreateBlueprint<BlueprintFeature>("ElfHeritageDefaultFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Elf");
                bp.SetDescriptionTagged("Elves are nimble, both in body and mind, but their form is frail. They gain +2 Dexterity, +2 Intelligence, and –2 Constitution.");
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
            });
            var RemoveDefaultHeritage = Helpers.Create<RemoveFeatureOnApply>(c => {
                c.m_Feature = ElfHeritageDefaultFeature.ToReference<BlueprintUnitFactReference>();
            });
            var ElfHeritageClassicFeature = Helpers.CreateBlueprint<BlueprintFeature>("ElfHeritageClassicFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Elf");
                bp.SetDescription(ElfHeritageDefaultFeature.Description);
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(RemoveDefaultHeritage);
            });
            var ElfHeritageFieraniFeature = Helpers.CreateBlueprint<BlueprintFeature>("ElfHeritageFieraniFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Fierani Elf");
                bp.SetDescriptionTagged("Having returned to Golarion to reclaim their ancestral homeland, some elves of the Fierani Forest have a closer bond "
                    + "to nature than most of their kin. Elves with this racial trait gain +2 Dexterity, +2 Wisdom, and -2 Constitution. "
                    + "This racial trait alters the elves’ ability score modifiers.");
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Wisdom;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(RemoveDefaultHeritage);
            });
            var ElfHeritageSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("ElfHeritageSelection", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.SetName("Elf Heritage");
                bp.SetDescriptionTagged("The following alternate heritages may be selected for Elf {g|Encyclopedia:Race}race{/g}.");
                bp.m_Icon = KitsuneHeritageSelection.Icon;
                bp.m_Features = new BlueprintFeatureReference[] {
                    ElfHeritageClassicFeature.ToReference<BlueprintFeatureReference>(),
                    ElfHeritageFieraniFeature.ToReference<BlueprintFeatureReference>(),
                };
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    ElfHeritageClassicFeature.ToReference<BlueprintFeatureReference>(),
                    ElfHeritageFieraniFeature.ToReference<BlueprintFeatureReference>(),
                };
                bp.Group = FeatureGroup.KitsuneHeritage;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
            });

            if (ModSettings.AddedContent.Races.DisableAll || !ModSettings.AddedContent.Races.Enabled["ElfHomebrewHeritage"]) { return; }
            ElfRace.ComponentsArray = new BlueprintComponent[0];
            ElfRace.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = ElfHeritageDefaultFeature.ToReference<BlueprintFeatureReference>();
            }));
            ElfRace.m_Features = new BlueprintFeatureBaseReference[] { ElfHeritageSelection.ToReference<BlueprintFeatureBaseReference>() }.AppendToArray(ElfRace.m_Features);
        }
    }
}
