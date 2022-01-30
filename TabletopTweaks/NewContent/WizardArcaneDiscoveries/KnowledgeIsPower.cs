using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.MechanicsChanges;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.WizardArcaneDiscoveries {
    static class KnowledgeIsPower {
        public static void AddKnowledgeIsPower() {
            var KnowledgeIsPower = Helpers.CreateBlueprint<BlueprintFeature>($"KnowledgeIsPower", bp => {
                bp.SetName($"Knowledge Is Power");
                bp.SetDescription("Your understanding of physical forces gives you power over them. " +
                    "You add your Intelligence modifier on combat maneuver checks and to your CMD.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] {};
                bp.AddComponent<CMDBonus>(c => {
                    c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Intelligence;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<CMBBonus>(c => {
                    c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Intelligence;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<RecalculateOnStatChange>(c => {
                    c.Stat = StatType.Intelligence;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                    c.m_Stat = StatType.Intelligence;
                });
            });
            if (ModSettings.AddedContent.WizardArcaneDiscoveries.IsDisabled("KnowledgeIsPower")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(KnowledgeIsPower);
        }
    }
}
