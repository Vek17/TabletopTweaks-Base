using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.FactLogic;
using Newtonsoft.Json;
using System;
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
                            OverridenResourceLogic = new MetamRageResourceOverride() {
                                m_RequiredResource = RequiredResource,
                                addedMetamagic = metamagic
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
                        OverridenResourceLogic = new MetamRageResourceOverride() {
                            m_RequiredResource = RequiredResource,
                            addedMetamagic = metamagic
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

            public new string Name { get => $"{Blueprint.Name} — {UIUtilityTexts.GetMetamagicName(addedMetamagic)}"; }
            public BlueprintFeature MetamagicFeature { get => m_MetamagicFeature.Get(); }
            [JsonProperty]
            public BlueprintFeatureReference m_MetamagicFeature;
            [JsonProperty]
            public Metamagic addedMetamagic;
        }
        private class MetamRageResourceOverride : IAbilityResourceLogic {
            public MetamRageResourceOverride() : base() { }

            public BlueprintAbilityResource RequiredResource => m_RequiredResource.Get();

            public bool IsSpendResource => true;

            public int CalculateCost(AbilityData ability) {
                return (ability.SpellLevel + AdjustedCost(addedMetamagic, ability.Caster)) * 2;
            }

            public void Spend(AbilityData ability) {
                UnitEntityData unit = ability.Caster.Unit;
                if (unit == null) {
                    PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                    return;
                }
                if (unit.Blueprint.IsCheater) {
                    return;
                }
                unit.Descriptor.Resources.Spend(this.RequiredResource, CalculateCost(ability));
            }

            [JsonProperty]
            public BlueprintAbilityResourceReference m_RequiredResource;
            [JsonProperty]
            public Metamagic addedMetamagic;
        }
    }
}
