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
    class Dwarf {
        public static void AddDwarfHeritage() {
            var KitsuneHeritageSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ec40cc350b18c8c47a59b782feb91d1f");
            var DwarfRace = Resources.GetBlueprint<BlueprintRace>("c4faf439f0e70bd40b5e36ee80d06be7");
            var DestinyBeyondBirthMythicFeat = Resources.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

            var DwarfHeritageDefaultFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("DwarfHeritageDefaultFeature");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.name = "DwarfHeritageDefaultFeature";
                bp.SetName("Dwarf");
                bp.SetDescription("Dwarves are both tough and wise, but also a bit gruff. They gain +2 Constitution, +2 Wisdom, and –2 Charisma.");
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Wisdom;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
            });
            var RemoveDefaultHeritage = Helpers.Create<RemoveFeatureOnApply>(c => {
                c.m_Feature = DwarfHeritageDefaultFeature.ToReference<BlueprintUnitFactReference>();
            });
            var DwarfHeritageClassicFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("DwarfHeritageClassicFeature");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "DwarfHeritageClassicFeature";
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Dwarf");
                bp.SetDescription(DwarfHeritageDefaultFeature.Description);
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Wisdom;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Constitution;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(RemoveDefaultHeritage);
            });
            var DwarfHeritageStoutheartFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("DwarfHeritageStoutheartFeature");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "DwarfHeritageStoutheartFeature";
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Stoutheart Dwarf");
                bp.SetDescription("Not all dwarves are as standoffish and distrusting as their peers, though they can be seen as foolhardy and brash by "
                    + "their kin. Dwarves with this racial trait gain +2 Constitution, +2 Charisma, and -2 Intelligence. "
                    + "This racial trait alters the dwarves’ ability score modifiers.");
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
                    c.Stat = StatType.Intelligence;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(Helpers.Create<RecalculateOnFactsChange>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                }));
                bp.AddComponent(RemoveDefaultHeritage);
            });
            var DwarfHeritageSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("DwarfHeritageSelection");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "DwarfHeritageSelection";
                bp.SetName("Dwarf Heritage");
                bp.SetDescription("The following alternate heritages may be selected for Dwarf {g|Encyclopedia:Race}race{/g}.");
                bp.m_Icon = KitsuneHeritageSelection.Icon;
                bp.m_Features = new BlueprintFeatureReference[] {
                    DwarfHeritageClassicFeature.ToReference<BlueprintFeatureReference>(),
                    DwarfHeritageStoutheartFeature.ToReference<BlueprintFeatureReference>(),
                };
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    DwarfHeritageClassicFeature.ToReference<BlueprintFeatureReference>(),
                    DwarfHeritageStoutheartFeature.ToReference<BlueprintFeatureReference>(),
                };
                bp.Group = FeatureGroup.KitsuneHeritage;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
            });
            Resources.AddBlueprint(DwarfHeritageDefaultFeature);
            Resources.AddBlueprint(DwarfHeritageClassicFeature);
            Resources.AddBlueprint(DwarfHeritageStoutheartFeature);
            Resources.AddBlueprint(DwarfHeritageSelection);

            if (ModSettings.AddedContent.Races.DisableAll || !ModSettings.AddedContent.Races.Enabled["DwarfHomebrewHeritage"]) { return; }
            DwarfRace.ComponentsArray = new BlueprintComponent[0];
            DwarfRace.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = DwarfHeritageDefaultFeature.ToReference<BlueprintFeatureReference>();
            }));
            DwarfRace.m_Features = DwarfRace.m_Features.AppendToArray(DwarfHeritageSelection.ToReference<BlueprintFeatureBaseReference>());
        }
    }
}
