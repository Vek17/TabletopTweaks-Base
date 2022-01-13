using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewContent.MechanicsChanges;
using TabletopTweaks.NewEvents;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("b522a7a4b3a44772bc5cfbdd55b1e0f9")]
    class MetaRageComponent : UnitFactComponentDelegate, ISpontaneousConversionHandler {

        private BlueprintSpellbook Spellbook { get => ConvertSpellbook.Get(); }
        static readonly Metamagic ExcludeMask = Metamagic.Heighten | Metamagic.CompletelyNormal;

        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions) {
            if (ability.Spellbook?.Blueprint != Spellbook) { return; }

            var conversionList = conversions.ToList();
            RuleCollectMetamagic collectMetamagic = new RuleCollectMetamagic(ability.Spellbook, ability.Blueprint, ability.SpellLevel);
            Rulebook.Trigger(collectMetamagic);

            foreach (var metamagicFeature in collectMetamagic.KnownMetamagics) {
                AddMetamagicFeat component = metamagicFeature.GetComponent<AddMetamagicFeat>();
                Metamagic metamagic = component.Metamagic;
                if ((metamagic & ExcludeMask) > 0) { continue; }
                if (!ability.Blueprint.AvailableMetamagic.HasMetamagic(metamagic)) { continue; }
                if (ability.MetamagicData?.Has(metamagic) ?? false) { continue; }

                var newMetamagicData = ability.MetamagicData?.Clone() ?? new MetamagicData();
                newMetamagicData.Add(metamagic);

                AbilityVariants variantComponent = ability.Blueprint.GetComponent<AbilityVariants>();
                if (variantComponent != null) {
                    foreach (var variant in variantComponent.Variants) {
                        MetaRageAbilityData metaAbility = new MetaRageAbilityData(variant, ability.Caster, null, ability.SpellbookBlueprint) {
                            MetamagicData = newMetamagicData,
                            OverridenResourceLogic = new AbilityResourceLogic() {
                                m_RequiredResource = RequiredResource,
                                m_IsSpendResource = true,
                                Amount = CalculateCost(ability, metamagic),
                            },
                            m_MetamagicFeature = metamagicFeature.Blueprint.ToReference<BlueprintFeatureReference>(),
                            m_ConvertedFrom = ability,
                            addedMetamagic = metamagic
                        };
                        AbilityData.AddAbilityUnique(ref conversionList, metaAbility);
                    }
                } else {
                    MetaRageAbilityData metaAbility = new MetaRageAbilityData(ability, null) {
                        MetamagicData = newMetamagicData,
                        OverridenResourceLogic = new AbilityResourceLogic() {
                            m_RequiredResource = RequiredResource,
                            m_IsSpendResource = true,
                            Amount = CalculateCost(ability, metamagic),
                        },
                        m_MetamagicFeature = metamagicFeature.Blueprint.ToReference<BlueprintFeatureReference>(),
                        addedMetamagic = metamagic
                    };
                    AbilityData.AddAbilityUnique(ref conversionList, metaAbility);
                }
            }
            conversions = conversionList;
        }

        private static int CalculateCost(AbilityData ability, Metamagic metamagic) {
            return (ability.SpellLevel + AdjustedCost(metamagic, ability.Caster)) * 2;
        }

        private static int AdjustedCost(Metamagic metamagic, UnitDescriptor unit) {
            UnitMechanicFeatures features = unit.State.Features;
            switch (metamagic) {
                case Metamagic.Empower:
                    if (features.FavoriteMetamagicEmpower) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Extend:
                    if (features.FavoriteMetamagicExtend) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Bolstered:
                    if (features.FavoriteMetamagicBolstered) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Maximize:
                    if (features.FavoriteMetamagicMaximize) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Quicken:
                    if (features.FavoriteMetamagicQuicken) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Reach:
                    if (features.FavoriteMetamagicReach) { return metamagic.DefaultCost() - 1; }
                    break;
                case Metamagic.Selective:
                    if (features.FavoriteMetamagicSelective) { return metamagic.DefaultCost() - 1; }
                    break;
            }
            if (MetamagicExtention.HasFavoriteMetamagic(unit, metamagic)) {
                return metamagic.DefaultCost() - 1;
            }
            return metamagic.DefaultCost();
        }

        public BlueprintSpellbookReference ConvertSpellbook;
        public BlueprintAbilityResourceReference RequiredResource;

        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.Name), MethodType.Getter)]
        static class AbilityData_Name_MetaRage_Patch {
            static void Postfix(AbilityData __instance, ref string __result) {
                switch (__instance) {
                    case MetaRageAbilityData abilityData:
                        __result = abilityData.Name;
                        break;
                }
            }
        }
        // UI Slot defined in NewUI
        public class MetaRageAbilityData : AbilityData {

            public MetaRageAbilityData() : base() { }
            public MetaRageAbilityData(
                BlueprintAbility blueprint,
                UnitDescriptor caster,
                [CanBeNull] Ability fact,
                [CanBeNull] BlueprintSpellbook spellbookBlueprint) : base(blueprint, caster, fact, spellbookBlueprint) {
            }

            public MetaRageAbilityData(AbilityData other, BlueprintAbility replaceBlueprint) : this(replaceBlueprint ?? other.Blueprint, other.Caster, other.Fact, other.SpellbookBlueprint) {
                MetamagicData metamagicData = other.MetamagicData;
                this.MetamagicData = (metamagicData != null) ? metamagicData.Clone() : null;
                this.m_ConvertedFrom = other;
            }

            public new string Name { get => $"{Blueprint.Name} — {addedMetamagic}"; }
            public BlueprintFeature MetamagicFeature { get => m_MetamagicFeature.Get(); }

            public BlueprintFeatureReference m_MetamagicFeature;
            public Metamagic addedMetamagic;
        }
    }
}
