﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class AbundantIncense {
        public static void AddAbundantIncense() {
            var IncenseFogFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("7614401346b64a8409f7b8c367db488f");
            var IncenseFog30Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("27c7f9e491a540243a535ac9aabb8ea5");
            var IncenseFogToggleAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("b62231e54e07068419a420f2988157b3");
            var IncenseFogResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("d03d97aac38e798479b81dfa9eda55c6");

            var AbundantIncenseFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AbundantIncenseFeature", bp => {
                bp.SetName(TTTContext, "Abundant Incense");
                bp.SetDescription(TTTContext, "You've learned a way to increase the number of rounds per day you can use Incense Fog.\n" +
                    "Benefit: The number of rounds per day you can use Incense Fog increases by a number of rounds equal to your mythic rank.");
                bp.m_Icon = IncenseFogToggleAbility.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_Resource = IncenseFogResource;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddPrerequisiteFeature(IncenseFogFeature, GroupType.Any);
                bp.AddPrerequisiteFeature(IncenseFog30Feature, GroupType.Any);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AbundantIncense")) { return; }
            FeatTools.AddAsMythicAbility(AbundantIncenseFeature);
        }
    }
}
