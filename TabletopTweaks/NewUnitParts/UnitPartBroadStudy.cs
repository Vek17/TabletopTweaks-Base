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

        public void RemoveEntry(EntityFact source) {
            Classes.RemoveAll((BroadStudyEntry p) => p.Source == source);
        }

        public bool IsBroadStudy(AbilityData spell) {
            var Spellbook = spell?.Spellbook.Blueprint;
            return Classes.Any(c => {
                Spellbook book = Owner?.DemandSpellbook(c.CharacterClass);
                return book?.Blueprint?.AssetGuid == Spellbook.AssetGuid || spell.IsInSpellList(book.Blueprint.SpellList);
            });
        }

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
    }
}
