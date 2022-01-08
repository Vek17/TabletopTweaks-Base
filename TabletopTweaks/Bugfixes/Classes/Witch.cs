using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Witch {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Witch");

                PatchBaseClass();
            }
            static void PatchBaseClass() {
                PatchAgilityPatron();

                void PatchAgilityPatron() {
                    if (ModSettings.Fixes.Witch.Base.IsDisabled("AgilityPatron")) { return; }
                    var WitchAgilityPatronProgression = Resources.GetBlueprint<BlueprintProgression>("08518b2a62446c74b9ae08ee73664047");
                    var WitchAgilityPatronSpellLevel8 = Resources.GetBlueprint<BlueprintFeature>("6164c2aa71247bd4d91274ebe93a6c0f");
                    var WitchAgilityPatronSpellLevel9 = Resources.GetBlueprint<BlueprintFeature>("1e418c5f030347542ad47dd752cdea05");

                    var AnimalShapes = Resources.GetBlueprintReference<BlueprintAbilityReference>("cf689244b2c7e904eb85f26fd6e81552");
                    var Shapechange = Resources.GetBlueprintReference<BlueprintAbilityReference>("22b9044aa229815429d57d0a30e4b739");

                    PatchPatronSpell(WitchAgilityPatronProgression, WitchAgilityPatronSpellLevel8, AnimalShapes);
                    PatchPatronSpell(WitchAgilityPatronProgression, WitchAgilityPatronSpellLevel9, Shapechange);
                    Main.LogPatch("Patched", WitchAgilityPatronProgression);
                }

                void PatchPatronSpell(BlueprintProgression patron, BlueprintFeature patronSpellfeature, BlueprintAbilityReference spell) {
                    patronSpellfeature.GetComponent<AddKnownSpell>().m_Spell = spell;
                    var AddSpells = patron.GetComponent<AddSpellsToDescription>();
                    AddSpells.m_Spells = AddSpells.m_Spells.AppendToArray(spell);

                    Main.LogPatch("Patched", patronSpellfeature);
                }
            }
        }
    }
}
