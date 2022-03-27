using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.WizardArcaneDiscoveries {
    static class YuelralsBlessing {
        public static void AddYuelralsBlessing() {
            var WizardClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");
            var WizardSpellList = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>("ba0401fdeb4062f40a7aa95b6f07fe89");
            var DruidSpellList = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>("bad8638d40639d04fa2f80a1cac67d6b");

            var YuelralsBlessing = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, $"YuelralsBlessing", bp => {
                bp.SetName(TTTContext, $"Yuelral's Blessing");
                bp.SetDescription(TTTContext, "Whenever you cast a spell that appears on both the wizard and druid spell lists, " +
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
                        DruidSpellList
                    };
                });
                bp.AddComponent<SharedSpellListCLIncrease>(c => {
                    c.Bonus = 1;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.SpellsOnly = true;
                    c.m_SpellLists = new BlueprintSpellListReference[] {
                        WizardSpellList,
                        DruidSpellList
                    };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(p => {
                    p.m_CharacterClass = WizardClass;
                    p.Level = 5;
                    p.Group = Prerequisite.GroupType.All;
                });
            });
            if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("YuelralsBlessing")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(YuelralsBlessing);
        }
    }
}
