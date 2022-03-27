using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    static class MythicCombatExpertise {
        public static void AddMythicCombatExpertise() {
            var CombatExpertiseFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("4c44724ffa8844f4d9bedb5bb27d144a");
            var CombatExpertiseBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("e81cd772a7311554090e413ea28ceea1");
            var StalwartBuff = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "StalwartBuff");

            var CombatExpertiseMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "CombatExpertiseMythicFeature", bp => {
                bp.m_Icon = CombatExpertiseFeature.m_Icon;
                bp.SetName(TTTContext, "Combat Expertise (Mythic)");
                bp.SetDescription(TTTContext, "You can dart out of the way of attacks with skill and defiance.\n" +
                    "Whenever you use Combat Expertise, you gain an additional +2 dodge bonus to your Armor Class.");
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.AC;
                    c.Value = 2;
                    c.Descriptor = ModifierDescriptor.Dodge;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { CombatExpertiseBuff };
                    c.m_BlockedFacts = new BlueprintUnitFactReference[] { StalwartBuff };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(CombatExpertiseFeature);
            });

            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicCombatExpertise")) { return; }
            FeatTools.AddAsMythicFeat(CombatExpertiseMythicFeature);
        }
    }
}
