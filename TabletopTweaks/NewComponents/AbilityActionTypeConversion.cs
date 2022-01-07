using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewEvents;

namespace TabletopTweaks.NewComponents {
    [TypeId("d2cf98b4e2854b90b4d451059cc581c0")]
    class AbilityActionTypeConversion : UnitFactComponentDelegate, ISpontaneousConversionHandler {

        private ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> Abilities {
            get {
                return this.m_Abilities;
            }
        }

        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions) {
            var conversionList = conversions.ToList();
            var MatchesDescriptors = CheckDescriptors && Descriptors.HasAnyFlag(ability.Blueprint.SpellDescriptor);
            if (Abilities.Contains(ability.Blueprint) || (MatchesDescriptors && (RequireAoE ? ability.IsAOE : !ability.IsAOE))) {
                CustomSpeedAbilityData swiftAbility = new CustomSpeedAbilityData(ability, null) {
                    MetamagicData = ability.MetamagicData ?? new MetamagicData(),
                    OverridenResourceLogic = new AbilityResourceLogic() {
                        m_RequiredResource = ability.ResourceLogic.RequiredResource.ToReference<BlueprintAbilityResourceReference>(),
                        m_IsSpendResource = ability.ResourceLogic.IsSpendResource,
                        Amount = ability.ResourceLogic.CalculateCost(ability) * ResourceMultiplier,
                    },
                    CustomActionType = ActionType
                };
                AbilityData.AddAbilityUnique(ref conversionList, swiftAbility);
                conversions = conversionList;
            }
        }

        public UnitCommand.CommandType ActionType = UnitCommand.CommandType.Swift;
        public int ResourceMultiplier = 2;
        public BlueprintAbilityReference[] m_Abilities = new BlueprintAbilityReference[0];
        public SpellDescriptorWrapper Descriptors;
        public bool CheckDescriptors;
        public bool RequireAoE;

        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.RuntimeActionType), MethodType.Getter)]
        static class AbilityData_RuntimeActionType_QuickChannel_Patch {
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                switch (__instance) {
                    case CustomSpeedAbilityData abilityData:
                        __result = abilityData.RuntimeActionType;
                        break;
                }
            }
        }
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.ActionType), MethodType.Getter)]
        static class AbilityData_ActionType_QuickChannel_Patch {
            static void Postfix(AbilityData __instance, ref UnitCommand.CommandType __result) {
                switch (__instance) {
                    case CustomSpeedAbilityData abilityData:
                        __result = abilityData.ActionType;
                        break;
                }
            }
        }

        private class CustomSpeedAbilityData : AbilityData {

            public CustomSpeedAbilityData() : base() { }
            public CustomSpeedAbilityData(
                BlueprintAbility blueprint,
                UnitDescriptor caster,
                [CanBeNull] Ability fact,
                [CanBeNull] BlueprintSpellbook spellbookBlueprint) : base(blueprint, caster, fact, spellbookBlueprint) {
            }

            public CustomSpeedAbilityData(AbilityData other, BlueprintAbility replaceBlueprint) : this(replaceBlueprint ?? other.Blueprint, other.Caster, other.Fact, other.SpellbookBlueprint) {
                MetamagicData metamagicData = other.MetamagicData;
                this.MetamagicData = (metamagicData != null) ? metamagicData.Clone() : null;
                this.m_ConvertedFrom = other;
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
