using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Enums;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.WizardArcaneDiscoveries {
    static class AlchemicalAffinity {
        public static void AddAlchemicalAffinity() {
            var WizardClass = Resources.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");
            var WizardSpellList = Resources.GetBlueprintReference<BlueprintSpellListReference>("ba0401fdeb4062f40a7aa95b6f07fe89");
            var AlchemistSpellList = Resources.GetBlueprintReference<BlueprintSpellListReference>("f60d0cd93edc65c42ad31e34a905fb2f");

            var AlchemicalAffinity = Helpers.CreateBlueprint<BlueprintFeature>($"AlchemicalAffinity", bp => {
                bp.SetName($"Alchemical Affinity");
                bp.SetDescription("Having studied alongside alchemists, you’ve learned to use their methodologies to enhance your spellcraft.\n" +
                    "Whenever you cast a spell that appears on both the wizard and alchemist spell lists, " +
                    "you treat your caster level as 1 higher than normal and the save DC of such spells increases by 1.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<SharedSpellListDCIncrease>(c => {
                    c.Bonus = 1;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.SpellsOnly = true;
                    c.m_SpellLists = new BlueprintSpellListReference[] {
                        WizardSpellList,
                        AlchemistSpellList
                    };
                });
                bp.AddComponent<SharedSpellListCLIncrease>(c => {
                    c.Bonus = 1;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.SpellsOnly = true;
                    c.m_SpellLists = new BlueprintSpellListReference[] {
                        WizardSpellList,
                        AlchemistSpellList
                    };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(p => {
                    p.m_CharacterClass = WizardClass;
                    p.Level = 5;
                    p.Group = Prerequisite.GroupType.All;
                });
            });
            if (ModSettings.AddedContent.WizardArcaneDiscoveries.IsDisabled("AlchemicalAffinity")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(AlchemicalAffinity);
        }
    }
}
