using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    class ImpossibleSpeed {
        public static void AddImpossibleSpeed() {
            var MythicAbilitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var FastMovement = BlueprintTools.GetBlueprint<BlueprintFeature>("d294a5dddd0120046aae7d4eb6cbc4fc");
            var ImpossibleSpeedFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ImpossibleSpeedFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = FastMovement.Icon;
                bp.SetName(TTTContext, "Impossible Speed");
                bp.SetDescription(TTTContext, "Your base land speed increases by 30 feet plus an additional 5 feet for every mythic rank.");
                bp.AddComponent<BuffMovementSpeed>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 30;
                    c.ContextBonus = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.MultiplyByModifier;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 5;
                    c.m_Max = 20;
                    c.m_Min = 1;
                });
            });
            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("ImpossibleSpeed")) { return; }
            FeatTools.AddAsMythicAbility(ImpossibleSpeedFeature);
        }
    }
}
