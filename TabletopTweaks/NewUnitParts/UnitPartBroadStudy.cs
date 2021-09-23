using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartBroadStudy : OldStyleUnitPart {

        public void AddEntry(BlueprintCharacterClassReference characterClass, EntityFact source) {
            BroadStudyEntry item = new BroadStudyEntry {
                CharacterClass = characterClass,
                Source = source
            };
            Classes.Add(item);
        }

        public void AddMythicSource(EntityFact source) {
            Mythic.Add(source);
        }

        public void RemoveEntry(EntityFact source) {
            Classes.RemoveAll((BroadStudyEntry p) => p.Source == source);
            TryRemove();
        }

        public void RemoveMythicSource(EntityFact source) {
            Mythic.RemoveAll(s => s == source);
            TryRemove();
        }

        private void TryRemove() {
            if (!Mythic.Any() && !Classes.Any()) { this.RemoveSelf(); }
        }

        public bool IsBroadStudy(AbilityData spell) {
            var Spellbook = spell?.Spellbook?.Blueprint;
            if (Spellbook == null) { return false; }
            return Classes.Any(c => {
                Spellbook book = Owner?.DemandSpellbook(c.CharacterClass);
                return book?.Blueprint?.AssetGuid == Spellbook.AssetGuid
                || spell.IsInSpellList(book.Blueprint.SpellList);
            });
        }

        public bool IsMythicBroadStudy(AbilityData spell) {
            return Mythic.Any() && (spell?.Spellbook?.IsMythic ?? false);
        }

        public List<EntityFact> Mythic = new List<EntityFact>();
        public List<BroadStudyEntry> Classes = new List<BroadStudyEntry>();
        public class BroadStudyEntry {
            public BlueprintCharacterClassReference CharacterClass;
            public EntityFact Source;
        }

        [HarmonyPatch(typeof(UnitPartMagus), "IsSpellFromMagusSpellList", new Type[] { typeof(AbilityData) })]
        class UnitDescriptor_IsSpellFromMagusSpellList_BroadStudy_Patch {
            static void Postfix(UnitPartMagus __instance, ref bool __result, AbilityData spell) {
                if (ModSettings.AddedContent.MagusArcana.IsDisabled("BroadStudy")) { return; }
                __result |= spell.Caster?.Get<UnitPartBroadStudy>()?.IsBroadStudy(spell) ?? false;
            }
        }

        [HarmonyPatch(typeof(UnitPartMagus), "IsSpellFromMagusSpellList", new Type[] { typeof(AbilityData) })]
        class UnitDescriptor_IsSpellFromMagusSpellList_MythicBroadStudy_Patch {
            static void Postfix(UnitPartMagus __instance, ref bool __result, AbilityData spell) {
                if (ModSettings.AddedContent.MythicAbilities.IsDisabled("MythicSpellCombat")) { return; }
                __result |= spell.Caster?.Get<UnitPartBroadStudy>()?.IsMythicBroadStudy(spell) ?? false;
            }
        }
    }
}
