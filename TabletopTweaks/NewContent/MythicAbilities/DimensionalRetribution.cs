using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class DimensionalRetribution {
        public static void AddDimensionalRetribution() {
            var DimensionalRetribution = Resources.GetBlueprint<BlueprintFeature>("939f49ad995ee8d4fad03ad0c7f655d1");
            var DweomercatDweomerleap = Resources.GetBlueprint<BlueprintAbility>("cde8c0c172c9fa34cba7703ba4824d32");
            var MidnightFane_DimensionLock_Buff = Resources.GetBlueprint<BlueprintBuff>("4b0cd08a3cea2844dba9889c1d34d667");

            var DimensionalRetributionTTTAbility = Helpers.CreateBlueprint<BlueprintAbility>("DimensionalRetributionTTTAbility", bp => {
                bp.SetName("Dimensional Retribution");
                bp.SetDescription("Every time you are targeted by an enemy spell, you may teleport to " +
                    "the spellcaster as an immediate action and make an attack of opportunity.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration","");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.SavingThrow", "");
                bp.m_Icon = DimensionalRetribution.Icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Unlimited;
                bp.CanTargetPoint = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.ResourceAssetIds = DweomercatDweomerleap.ResourceAssetIds;
                var DweomerleapComponent = DweomercatDweomerleap.GetComponent<AbilityCustomDweomerLeap>();
                bp.AddComponent<AbilityCustomDimensionalRetribution>(c => {
                    c.m_CasterDisappearProjectile = DweomerleapComponent.m_CasterDisappearProjectile;
                    c.m_CasterAppearProjectile = DweomerleapComponent.m_CasterAppearProjectile;
                    c.m_SideDisappearProjectile = DweomerleapComponent.m_SideDisappearProjectile;
                    c.m_SideAppearProjectile = DweomerleapComponent.m_SideAppearProjectile;
                    c.PortalFromPrefab = DweomerleapComponent.PortalFromPrefab;
                    c.PortalToPrefab = DweomerleapComponent.PortalToPrefab;
                    c.PortalBone = DweomerleapComponent.PortalBone;
                    c.CasterDisappearFx = DweomerleapComponent.CasterDisappearFx;
                    c.CasterAppearFx = DweomerleapComponent.CasterAppearFx;
                    c.SideDisappearFx = DweomerleapComponent.SideDisappearFx;
                    c.SideAppearFx = DweomerleapComponent.SideAppearFx;
                });
            });
            var DimensionalRetributionTTTBuff = Helpers.CreateBuff("DimensionalRetributionTTTBuff", bp => {
                bp.m_Icon = DimensionalRetributionTTTAbility.Icon;
                bp.SetName("Dimensional Retribution");
                bp.SetDescription(DimensionalRetributionTTTAbility.m_Description);
                bp.AddComponent<DimensionalRetributionLogic>(c => {
                    c.m_Ability = DimensionalRetributionTTTAbility.ToReference<BlueprintAbilityReference>();
                });                
            });
            var DimensionalRetributionTTTToggleAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>("DimensionalRetributionTTTToggleAbility", bp => {
                bp.m_Icon = DimensionalRetributionTTTAbility.Icon;
                bp.SetName("Dimensional Retribution");
                bp.SetDescription(DimensionalRetributionTTTAbility.m_Description);
                bp.m_Buff = DimensionalRetributionTTTBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = true;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
            });
            //FeatTools.AddAsMythicAbility(AbundantBlessingFeature);
            var forbiddenSpells = MidnightFane_DimensionLock_Buff.GetComponent<ForbidSpecificSpellsCast>();
            forbiddenSpells.m_Spells = forbiddenSpells.m_Spells
                .Append(DimensionalRetributionTTTAbility.ToReference<BlueprintAbilityReference>())
                .ToArray();
        }
    }
}
