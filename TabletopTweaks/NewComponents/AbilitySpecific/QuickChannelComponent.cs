using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewEvents;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    class QuickChannelComponent : UnitFactComponentDelegate, ISpontaneousConversionHandler {
        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions) {
            var conversionList = conversions.ToList();
            var isChannel = ability.Blueprint.SpellDescriptor
                .HasAnyFlag(SpellDescriptor.ChannelNegativeHarm | SpellDescriptor.ChannelNegativeHeal | SpellDescriptor.ChannelPositiveHarm | SpellDescriptor.ChannelPositiveHeal)
                && ability.IsAOE;
            if (isChannel) {
                QuickChannelAbilityData moveAbility = new QuickChannelAbilityData(ability, null) {
                    MetamagicData = (ability.MetamagicData ?? new MetamagicData()),
                    ExtraSpellSlotCost = 1,
                    OverridenResourceLogic = new AbilityResourceLogic() {
                        m_RequiredResource = ability.ResourceLogic.RequiredResource.ToReference<BlueprintAbilityResourceReference>(),
                        m_IsSpendResource = ability.ResourceLogic.IsSpendResource,
                        Amount = ability.ResourceLogic.CalculateCost(ability) * 2,
                    },
                    CustomActionType = UnitCommand.CommandType.Move
                };
                conversionList.Add(moveAbility);
                conversions = conversionList;
            }
        }
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.RuntimeActionType), MethodType.Getter)]
        static class AbilityData_RuntimeActionType_QuickChannel_Patch {
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                switch (__instance) {
                    case QuickChannelAbilityData abilityData:
                        __result = abilityData.RuntimeActionType;
                        break;
                }
            }
        }
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.ActionType), MethodType.Getter)]
        static class AbilityData_ActionType_QuickChannel_Patch {
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                switch (__instance) {
                    case QuickChannelAbilityData abilityData:
                        __result = abilityData.ActionType;
                        break;
                }
            }
        }
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.Name), MethodType.Getter)]
        static class AbilityData_Name_QuickChannel_Patch {
            static void Postfix(AbilityData __instance, ref string __result) {
                switch (__instance) {
                    case QuickChannelAbilityData abilityData:
                        __result = abilityData.Name;
                        break;
                }
            }
        }
        private class QuickChannelAbilityData : AbilityData {

            public QuickChannelAbilityData() : base() { }
            public QuickChannelAbilityData(
                BlueprintAbility blueprint,
                UnitDescriptor caster,
                [CanBeNull] Ability fact,
                [CanBeNull] BlueprintSpellbook spellbookBlueprint) : base(blueprint, caster, fact, spellbookBlueprint) {
            }

            public QuickChannelAbilityData(AbilityData other, BlueprintAbility replaceBlueprint) : this(replaceBlueprint ?? other.Blueprint, other.Caster, other.Fact, other.SpellbookBlueprint) {
                MetamagicData metamagicData = other.MetamagicData;
                this.MetamagicData = (metamagicData != null) ? metamagicData.Clone() : null;
                this.m_ConvertedFrom = other;
            }

            public new string Name {
                get {
                    return $"{Blueprint.Name} — Quick";
                }
            }

            public new UnitCommand.CommandType RuntimeActionType {
                get {
                    return CustomActionType;
                }
            }

            public new UnitCommand.CommandType ActionType {
                get {
                    return CustomActionType;
                }
            }

            public UnitCommand.CommandType CustomActionType;
        }
    }
}
