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
    class Halfling {
        public static void AddHalflingHeritage() {
            var KitsuneHeritageSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ec40cc350b18c8c47a59b782feb91d1f");
            var Halfling = Resources.GetBlueprint<BlueprintRace>("b0c3ef2729c498f47970bb50fa1acd30");
            var DestinyBeyondBirthMythicFeat = Resources.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

            var HalflingHeritageDefaultFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["HalflingHeritageDefaultFeature"];
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.name = "HalflingHeritageDefaultFeature";
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Halfling");
                bp.SetDescription("Halflings are nimble and strong-willed, but their small stature makes them weaker than other "
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
            var RemoveDefaultHeritage = Helpers.Create<RemoveFeatureOnApply>(c => {
                c.m_Feature = HalflingHeritageDefaultFeature.ToReference<BlueprintUnitFactReference>();
            });
            var HalflingHeritageClassicFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["HalflingHeritageClassicFeature"];
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "HalflingHeritageClassicFeature";
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Halfling");
                bp.SetDescription(HalflingHeritageDefaultFeature.Description);
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
                bp.AddComponent(RemoveDefaultHeritage);
            });
            var HalflingHeritageBruiserFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["HalflingHeritageBruiserFeature"];
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "HalflingHeritageBruiserFeature";
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("Halfling Bruiser");
                bp.SetDescription("A lifetime of brutal survival, either under the heavy burdens of slavery or on the "
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
                bp.AddComponent(RemoveDefaultHeritage);
            });
            var HalflingHeritageSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["HalflingHeritageSelection"];
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "HalflingHeritageSelection";
                bp.SetName("Halfling Heritage");
                bp.SetDescription("The following alternate heritages may be selected for halfling {g|Encyclopedia:Race}race{/g}.");
                bp.m_Icon = KitsuneHeritageSelection.Icon;
                bp.m_Features = new BlueprintFeatureReference[] {
                    HalflingHeritageClassicFeature.ToReference<BlueprintFeatureReference>(),
                    HalflingHeritageBruiserFeature.ToReference<BlueprintFeatureReference>(),
                };
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    HalflingHeritageClassicFeature.ToReference<BlueprintFeatureReference>(),
                    HalflingHeritageBruiserFeature.ToReference<BlueprintFeatureReference>(),
                };
                bp.Group = FeatureGroup.KitsuneHeritage;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
            });
            Resources.AddBlueprint(HalflingHeritageDefaultFeature);
            Resources.AddBlueprint(HalflingHeritageClassicFeature);
            Resources.AddBlueprint(HalflingHeritageBruiserFeature);
            Resources.AddBlueprint(HalflingHeritageSelection);

            if (ModSettings.AddedContent.Races.DisableAll || !ModSettings.AddedContent.Races.Enabled["HalflingHomebrewHeritage"]) { return; }
            Halfling.ComponentsArray = new BlueprintComponent[0];
            Halfling.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = HalflingHeritageDefaultFeature.ToReference<BlueprintFeatureReference>();
            }));
            Halfling.m_Features = Halfling.m_Features.AppendToArray(HalflingHeritageSelection.ToReference<BlueprintFeatureBaseReference>());
        }
    }
}
