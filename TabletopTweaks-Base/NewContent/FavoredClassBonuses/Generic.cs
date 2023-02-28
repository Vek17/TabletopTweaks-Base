using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.FavoredClassBonuses {
    internal class Generic {
        public static BlueprintFeature FavoredClassBonusHP = null;
        public static BlueprintFeature FavoredClassSkillPoint = null;
        public static void AddFavoredClassBonuses() {
            var FavoredClassBonusBaseProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "FavoredClassBonusBaseProgression", bp => {
                bp.SetName(TTTContext, "FavoredClassBonusBaseProgression");
                bp.SetDescription(TTTContext, "");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    //Helpers.CreateLevelEntry(1, null)
                };
            });
            FavoredClassBonusHP = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoredClassBonusHP", bp => {
                bp.SetName(TTTContext, "HP Bonus");
                bp.SetDescription(TTTContext, "");
                bp.Ranks = 40;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.HitPoints;
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.FavouredClassBonus;
                });
            });
            FavoredClassSkillPoint = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoredClassSkillPoint", bp => {
                bp.SetName(TTTContext, "Extra Skill Point");
                bp.SetDescription(TTTContext, "");
                bp.Ranks = 40;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;
                bp.AddComponent<AddExtraSkillPoint>(c => {
                    c.Value = 1;
                });
            });
        }
    }
}
