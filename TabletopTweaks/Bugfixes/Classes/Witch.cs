using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

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
                PatchAmelioratingHex();
                PatchMajorAmelioratingHex();

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

                void PatchAmelioratingHex() {
                    if (ModSettings.Fixes.Witch.Base.IsDisabled("AmelioratingHex")) { return; }

                    var WitchHexAmelioratingDazzleSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("c1a5e2bb65fbc6d479f957fd54b2f313");
                    var WitchHexAmelioratingFatuguedSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("32b27dd734277464c954f22b35338a62");
                    var WitchHexAmelioratingShakenSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("d98e8e525c0f5c746b4d8f1ea24b865a");
                    var WitchHexAmelioratingSickenedSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("ddee5f689ef42274ca5d5731cb96b075");

                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingDazzleSuppressBuff);
                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingFatuguedSuppressBuff);
                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingShakenSuppressBuff);
                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingSickenedSuppressBuff);
                }

                void PatchMajorAmelioratingHex() {
                    if (ModSettings.Fixes.Witch.Base.IsDisabled("MajorAmelioratingHex")) { return; }

                    var WitchHexMajorAmelioratingBlindedSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("66700a810f380c5419dc13f03bb76f45");
                    var WitchHexMajorAmelioratingCurseSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("ee471eead8a069c449710ac5073322c0");
                    var WitchHexMajorAmelioratingDiseaseSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("0870624a0fa21cf418d4c922da4205d4");
                    var WitchHexMajorAmelioratingPoisonSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("2cf1962ef4cd4b5468c09bf4959d1bf7");

                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingBlindedSuppressBuff);
                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingCurseSuppressBuff);
                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingDiseaseSuppressBuff);
                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingPoisonSuppressBuff);
                }
            }
        }
    }
}
