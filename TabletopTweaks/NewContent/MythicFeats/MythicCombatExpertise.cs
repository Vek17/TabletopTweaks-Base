using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicFeats {
    static class MythicCombatExpertise {
        public static void AddMythicCombatExpertise() {
            var CombatExpertiseFeature = Resources.GetBlueprint<BlueprintFeature>("4c44724ffa8844f4d9bedb5bb27d144a");
            var CombatExpertiseBuff = Resources.GetBlueprintReference<BlueprintUnitFactReference>("e81cd772a7311554090e413ea28ceea1");

            var CombatExpertiseMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>("CombatExpertiseMythicFeature", bp => {
                bp.m_Icon = CombatExpertiseFeature.m_Icon;
                bp.SetName("Combat Expertise (Mythic)");
                bp.SetDescription("You can dart out of the way of attacks with skill and defiance.\n" +
                    "Whenever you use Combat Expertise, you gain an additional +2 dodge bonus to your Armor Class.");
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.AC;
                    c.Value = 2;
                    c.Descriptor = ModifierDescriptor.Dodge;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { CombatExpertiseBuff };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(CombatExpertiseFeature);
            });

            if (ModSettings.AddedContent.MythicFeats.IsDisabled("MythicCombatExpertise")) { return; }
            FeatTools.AddAsMythicFeat(CombatExpertiseMythicFeature);
        }
    }
}
