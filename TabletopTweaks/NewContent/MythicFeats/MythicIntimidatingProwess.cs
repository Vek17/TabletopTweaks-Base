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
    static class MythicIntimidatingProwess {
        public static void AddMythicIntimidatingProwess() {
            var IntimidatingProwess = Resources.GetBlueprint<BlueprintFeature>("d76497bfc48516e45a0831628f767a0f");
            var IntimidatingProwessMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>("IntimidatingProwessMythicFeature", bp => {
                bp.m_Icon = IntimidatingProwess.m_Icon;
                bp.SetName("Intimidating Prowess (Mythic)");
                bp.SetDescription("You gain a bonus on Intimidate checks equal to your mythic rank.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.CheckIntimidate;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<RecalculateOnLevelUp>();
                bp.AddPrerequisiteFeature(IntimidatingProwess);
            });
            if (ModSettings.AddedContent.MythicFeats.IsDisabled("MythicIntimidatingProwess")) { return; }
            FeatTools.AddAsMythicFeat(IntimidatingProwessMythicFeature);
        }
    }
}
