using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem;
using Kingmaker.UI.Common;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using UnityEngine;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartSpellSpecialization : OldStyleUnitPart {
        public void AddEntry(BlueprintAbilityReference spell, EntityFact source) {
            SpellSpecializationEntry item = new SpellSpecializationEntry {
                Spell = spell,
                Source = source
            };
            this.Spells.Add(item);
        }

        public void RemoveEntry(EntityFact source) {
            this.Spells.RemoveAll(entry => entry.Source == source);
            TryRemove();
        }

        public void EnableGreater(EntityFact source) {
            GreaterSpellSpecializationEntry item = new GreaterSpellSpecializationEntry {
                Source = source
            };
            this.Greater.Add(item);
        }

        public void DisableGreater(EntityFact source) {
            this.Greater.RemoveAll(entry => entry.Source == source);
            TryRemove();
        }

        private void TryRemove() {
            if (!Spells.Any() && !Greater.Any()) { this.RemoveSelf(); }
        }

        public bool IsGreater() {
            return Greater.Any();
        }

        public bool HasEntry(AbilityData spell) {
            return this.Spells.Any(entry => entry.Spell.Guid.Equals(spell.Blueprint.AssetGuid));
        }

        public List<AbilityData> GenerateConversions(AbilityData spell) {
            List<AbilityData> conversionSpells = new List<AbilityData>();
            var spellbook = spell.Spellbook;
            if (spellbook == null) {
                return conversionSpells;
            }
            int targetSpellLevel = spell.SpellLevel;
            if (targetSpellLevel <= 0) {
                return conversionSpells;
            }
            conversionSpells.AddRange(this.Spells.SelectMany(entry => {
                List<AbilityData> conversions = spellbook.GetKnownSpells(entry.Spell);
                conversions.AddRange(spellbook.GetCustomSpells(entry.Spell));
                return conversions
                    .Where(spell => spell.SpellLevel <= targetSpellLevel)
                    .ToList();
            }));
            return conversionSpells;
        }

        private readonly List<SpellSpecializationEntry> Spells = new List<SpellSpecializationEntry>();
        private readonly List<GreaterSpellSpecializationEntry> Greater = new List<GreaterSpellSpecializationEntry>();

        private class SpellSpecializationEntry {
            public BlueprintAbilityReference Spell;
            public EntityFact Source;
        }

        private class GreaterSpellSpecializationEntry {
            public EntityFact Source;
        }

        [HarmonyPatch(typeof(AbilityData), "GetConversions")]
        static class AbilityData_GetConversions_SpellSpecializationGreater_Patch {
            static void Postfix(AbilityData __instance, ref IEnumerable<AbilityData> __result) {
                if (ModSettings.AddedContent.Feats.IsDisabled("SpellSpecializationGreater")) { return; }
                List<AbilityData> list = __result.ToList();
                UnitPartSpellSpecialization spellSpecialization = __instance.Caster.Get<UnitPartSpellSpecialization>();
                if (spellSpecialization == null || !spellSpecialization.IsGreater() || spellSpecialization.HasEntry(__instance)) {
                    return;
                }
                if (__instance.Spellbook != null) {
                    var conversions = spellSpecialization.GenerateConversions(__instance);
                    foreach (var conversion in conversions) {
                        List<SpontaneousConversionAbilityData> convertedData = new List<SpontaneousConversionAbilityData>();
                        AbilityVariants variantComponent = conversion.Blueprint.GetComponent<AbilityVariants>();
                        if (variantComponent != null) {
                            foreach (var variant in variantComponent.Variants) {
                                convertedData.Add(new SpontaneousConversionAbilityData(variant, conversion.Caster, null, conversion.SpellbookBlueprint, conversion) {
                                    DecorationBorderNumber = conversion.DecorationBorderNumber,
                                    DecorationColorNumber = conversion.DecorationColorNumber,
                                    MetamagicData = conversion.MetamagicData?.Clone(),
                                    m_ConvertedFrom = __instance
                                });
                            }
                        } else {
                            convertedData.Add(new SpontaneousConversionAbilityData(conversion.Blueprint, conversion.Caster, null, conversion.SpellbookBlueprint) {
                                DecorationBorderNumber = conversion.DecorationBorderNumber,
                                DecorationColorNumber = conversion.DecorationColorNumber,
                                MetamagicData = conversion.MetamagicData?.Clone(),
                                m_ConvertedFrom = __instance
                            });
                        }

                        foreach (var ability in convertedData) {
                            AbilityData.AddAbilityUnique(ref list, ability);
                        }
                    }
                }
                __result = list;
            }
        }

        [HarmonyPatch(typeof(AbilityData), "RequireFullRoundAction", MethodType.Getter)]
        static class AbilityData_RequireFullRoundAction_QuickStudy_Patch {
            static void Postfix(AbilityData __instance, ref bool __result) {
                if (ModSettings.AddedContent.Feats.IsDisabled("SpellSpecializationGreater")) { return; }
                switch (__instance) {
                    case SpontaneousConversionAbilityData abilityData:
                        __result = abilityData.RequireFullRoundAction;
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(AbilityData), "SpellLevel", MethodType.Getter)]
        static class AbilityData_SpellLevel_QuickStudy_Patch {
            static void Postfix(AbilityData __instance, ref int __result) {
                if (ModSettings.AddedContent.Feats.IsDisabled("SpellSpecializationGreater")) { return; }
                switch (__instance) {
                    case SpontaneousConversionAbilityData abilityData:
                        __result = abilityData.SpellLevel;
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(MechanicActionBarSlotSpontaneusConvertedSpell), "GetDecorationSprite")]
        static class AbilityData_DecorationColorNumber_QuickStudy_Patch {
            static void Postfix(MechanicActionBarSlotSpontaneusConvertedSpell __instance, ref Sprite __result) {
                if (ModSettings.AddedContent.Feats.IsDisabled("SpellSpecializationGreater")) { return; }
                switch (__instance.Spell) {
                    case SpontaneousConversionAbilityData abilityData:
                        __result = UIUtility.GetDecorationBorderByIndex(abilityData.DecorationBorderNumber);
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(MechanicActionBarSlotSpontaneusConvertedSpell), "GetDecorationColor")]
        static class AbilityData_DecorationBorderNumber_QuickStudy_Patch {
            static void Postfix(MechanicActionBarSlotSpontaneusConvertedSpell __instance, ref Color __result) {
                if (ModSettings.AddedContent.Feats.IsDisabled("SpellSpecializationGreater")) { return; }
                switch (__instance.Spell) {
                    case SpontaneousConversionAbilityData abilityData:
                        __result = UIUtility.GetDecorationColorByIndex(abilityData.DecorationColorNumber);
                        break;
                }
            }
        }

        private class SpontaneousConversionAbilityData : AbilityData {
            public SpontaneousConversionAbilityData(
                BlueprintAbility blueprint,
                UnitDescriptor caster,
                [CanBeNull] Ability fact,
                [CanBeNull] BlueprintSpellbook spellbookBlueprint) : base(blueprint, caster, fact, spellbookBlueprint) {
            }

            public SpontaneousConversionAbilityData(
                BlueprintAbility blueprint,
                UnitDescriptor caster,
                [CanBeNull] Ability fact,
                [CanBeNull] BlueprintSpellbook spellbookBlueprint,
                [CanBeNull] AbilityData baseData) : this(blueprint, caster, fact, spellbookBlueprint) {
                this.baseData = baseData;
            }

            public bool IsVariant { get => baseData != null; }

            public new int SpellLevel {
                get {
                    if (this.IsVariant) {
                        return this.baseData.Spellbook?.GetSpellLevel(baseData) ?? 0;
                    }
                    return this.Spellbook?.GetSpellLevel(this) ?? 0;
                }
            }
            public new bool RequireFullRoundAction {
                get {
                    return this.MetamagicData != null
                        && this.MetamagicData.NotEmpty
                        && !this.MetamagicData.Has(Metamagic.Quicken);
                }
            }

            private readonly AbilityData baseData;
        }
    }
}
