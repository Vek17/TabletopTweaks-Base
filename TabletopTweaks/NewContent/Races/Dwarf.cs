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
    static class Dwarf {

        private static readonly BlueprintRace DwarfRace = Resources.GetBlueprint<BlueprintRace>("c4faf439f0e70bd40b5e36ee80d06be7");
        private static readonly BlueprintFeatureSelection DwarfHeritageSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("fd6e1f53589049cbbbc6a8e058d83b74");
        private static readonly BlueprintFeature DwarvenWeaponFamiliarity = Resources.GetBlueprint<BlueprintFeature>("a1619e8d27fe97c40ba443f6f8ab1763");
        private static readonly BlueprintFeature KeenSenses = Resources.GetBlueprint<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
        private static readonly BlueprintFeature HatredGoblinoidOrc = Resources.GetBlueprint<BlueprintFeature>("6cde66a7da5a2024c906d887db735223");
        private static readonly BlueprintFeature DwarfDefensiveTrainingGiants = Resources.GetBlueprint<BlueprintFeature>("f268a00e42618144e86c9db76af7f3e9");
        private static readonly BlueprintFeature Hardy = Resources.GetBlueprint<BlueprintFeature>("f75d3b6110f04d1409564b9d7647db60");
        private static readonly BlueprintFeature Stability = Resources.GetBlueprint<BlueprintFeature>("2f254c6068d58b643b8de2fc7ec32dbb");
        private static readonly BlueprintFeature SlowAndSteady = Resources.GetBlueprint<BlueprintFeature>("786588ad1694e61498e77321d4b07157");

        private static readonly BlueprintFeature BarrowDwarf = Resources.GetBlueprint<BlueprintFeature>("8a1a0f9f397144ce9445efe86b7af722");
        private static readonly BlueprintFeature UnstoppableDwarf = Resources.GetBlueprint<BlueprintFeature>("109dca82e31a4b1093c501f8914ca1a8");

        private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = Resources.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

        public static void AddDwarfHeritage() {

            var DwarfAbilityModifiers = Helpers.CreateBlueprint<BlueprintFeature>("DwarfAbilityModifiers", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("e9f071c568014d9cbd0c945b7272461f", "Dwarf Ability Modifiers");
                bp.SetDescription("54f8a00877ac43898460e2b2c9ab3114", "Dwarves are both tough and wise, but also a bit gruff. They gain +2 Constitution, +2 Wisdom, and –2 Charisma.");
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
            var DwarfNoAlternateTrait = Helpers.CreateBlueprint<BlueprintFeature>("DwarfNoAlternateTrait", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("e095f08673f3496ea10a3fc57f65b4f5", "None");
                bp.SetDescription("20c3711c002b4a2f8d1e7beee4c470c7", "No Alternate Trait");
            });
            var DwarfStoutheartFeature = Helpers.CreateBlueprint<BlueprintFeature>("DwarfStoutheartFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("376fc07aa9ce4a6e8eb884ebf55825ba", "Stoutheart Dwarf");
                bp.SetDescription("4e65351931cd4a7693e094a724e7c2fb", "Not all dwarves are as standoffish and distrusting as their peers, though they can be seen as foolhardy and brash by "
                    + "their kin. Dwarves with this racial trait gain +2 Constitution, +2 Charisma, and -2 Intelligence."
                    + "\nThis racial trait alters the dwarves’ ability score modifiers.");
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
                bp.AddTraitReplacment(DwarfAbilityModifiers);
                bp.AddSelectionCallback(DwarfHeritageSelection);
            });
            var DwarfStoicNegotiatorFeature = Helpers.CreateBlueprint<BlueprintFeature>("DwarfStoicNegotiatorFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial };
                bp.SetName("bf91efe239e04c8fb99a10e814bb1f8b", "Stoic Negotiator");
                bp.SetDescription("b5c93e8cd55a49358c74ab915ed8ccf8", "Some dwarves use their unwavering stubbornness to get what they want in negotiations and other business matters. " +
                    "They gain a +2 racial bonus on Persuasion checks and Persuasion is a class skill for them.\nThis racial trait replaces defensive training, hatred.");
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.SkillPersuasion;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<AddClassSkill>(c => {
                    c.Skill = StatType.SkillPersuasion;
                }));
                bp.AddTraitReplacment(DwarfDefensiveTrainingGiants);
                bp.AddTraitReplacment(HatredGoblinoidOrc);
                bp.AddSelectionCallback(DwarfHeritageSelection);
            });

            BarrowDwarf.RemoveComponents<RemoveFeatureOnApply>();
            BarrowDwarf.AddTraitReplacment(KeenSenses);
            BarrowDwarf.AddTraitReplacment(Stability);
            BarrowDwarf.AddTraitReplacment(DwarfDefensiveTrainingGiants);
            BarrowDwarf.AddSelectionCallback(DwarfHeritageSelection);

            UnstoppableDwarf.RemoveComponents<RemoveFeatureOnApply>();
            UnstoppableDwarf.AddTraitReplacment(KeenSenses);
            UnstoppableDwarf.AddTraitReplacment(DwarfDefensiveTrainingGiants);
            UnstoppableDwarf.AddTraitReplacment(HatredGoblinoidOrc);
            UnstoppableDwarf.AddSelectionCallback(DwarfHeritageSelection);

            if (ModSettings.AddedContent.Races.IsDisabled("DwarfAlternateTraits")) { return; }
            DwarfRace.SetComponents(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = DwarfAbilityModifiers.ToReference<BlueprintFeatureReference>();
            }));
            DwarfHeritageSelection.SetName("0f7da22307924df8b660cb0e6bd7c451", "Alternate Traits");
            DwarfHeritageSelection.SetDescription("f22811e7d03f46b8aa366d6e552061cf", "The following alternate traits are available.");
            DwarfHeritageSelection.Group = FeatureGroup.KitsuneHeritage;
            DwarfHeritageSelection.SetFeatures(
                DwarfNoAlternateTrait,
                DwarfStoutheartFeature,
                DwarfStoicNegotiatorFeature,
                BarrowDwarf,
                UnstoppableDwarf
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
