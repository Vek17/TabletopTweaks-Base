using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicFeats {
    static class MythicWarriorPriest {
        public static void AddMythicWarriorPriest() {
            var WarriorPriest = Resources.GetBlueprint<BlueprintFeature>("b9bee4e4e15573546b76a8d942ce914b");
            var WarriorPriestMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>("WarriorPriestMythicFeature", bp => {
                bp.m_Icon = WarriorPriest.m_Icon;
                bp.SetName("Warrior Priest (Mythic)");
                bp.SetDescription("Your faith speeds you in battle and further strengthens your mind and confidence.\n" +
                    "You gain a bonus equal to half your mythic rank both on initiative checks and on concentration checks.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.Initiative;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<ConcentrationBonus>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                });
                bp.AddComponent<RecalculateOnLevelUp>();
                bp.AddPrerequisiteFeature(WarriorPriest);
            });
            if (ModSettings.AddedContent.MythicFeats.IsDisabled("MythicWarriorPriest")) { return; }
            FeatTools.AddAsMythicFeat(WarriorPriestMythicFeature);
        }
    }
}
