using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Witch {
        [PatchBlueprintsCacheInit]
        static class Witch_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Witch")) { return; }

                var WitchAlternateCapstone = NewContent.AlternateCapstones.Witch.WitchAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                ClassTools.Classes.WitchClass.TemporaryContext(bp => {
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(WitchAlternateCapstone));
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Witch");

                PatchBase();
            }
            static void PatchBase() {
                PatchAgilityPatron();
                PatchAmelioratingHex();
                PatchMajorAmelioratingHex();

                void PatchAgilityPatron() {
                    if (TTTContext.Fixes.Witch.Base.IsDisabled("AgilityPatron")) { return; }

                    var WitchAgilityPatronProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("08518b2a62446c74b9ae08ee73664047");
                    var WitchAgilityPatronSpellLevel8 = BlueprintTools.GetBlueprint<BlueprintFeature>("6164c2aa71247bd4d91274ebe93a6c0f");
                    var WitchAgilityPatronSpellLevel9 = BlueprintTools.GetBlueprint<BlueprintFeature>("1e418c5f030347542ad47dd752cdea05");

                    var AnimalShapes = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("cf689244b2c7e904eb85f26fd6e81552");
                    var Shapechange = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("22b9044aa229815429d57d0a30e4b739");

                    PatchPatronSpell(WitchAgilityPatronProgression, WitchAgilityPatronSpellLevel8, AnimalShapes);
                    PatchPatronSpell(WitchAgilityPatronProgression, WitchAgilityPatronSpellLevel9, Shapechange);
                    TTTContext.Logger.LogPatch("Patched", WitchAgilityPatronProgression);
                }

                void PatchPatronSpell(BlueprintProgression patron, BlueprintFeature patronSpellfeature, BlueprintAbilityReference spell) {
                    patronSpellfeature.GetComponent<AddKnownSpell>().m_Spell = spell;
                    var AddSpells = patron.GetComponent<AddSpellsToDescription>();
                    AddSpells.m_Spells = AddSpells.m_Spells.AppendToArray(spell);

                    TTTContext.Logger.LogPatch("Patched", patronSpellfeature);
                }

                void PatchAmelioratingHex() {
                    if (TTTContext.Fixes.Witch.Base.IsDisabled("AmelioratingHex")) { return; }

                    var WitchHexAmelioratingDazzleSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c1a5e2bb65fbc6d479f957fd54b2f313");
                    var WitchHexAmelioratingFatuguedSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("32b27dd734277464c954f22b35338a62");
                    var WitchHexAmelioratingShakenSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d98e8e525c0f5c746b4d8f1ea24b865a");
                    var WitchHexAmelioratingSickenedSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ddee5f689ef42274ca5d5731cb96b075");

                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingDazzleSuppressBuff, TTTContext);
                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingFatuguedSuppressBuff, TTTContext);
                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingShakenSuppressBuff, TTTContext);
                    QuickFixTools.ReplaceSuppression(WitchHexAmelioratingSickenedSuppressBuff, TTTContext);
                }

                void PatchMajorAmelioratingHex() {
                    if (TTTContext.Fixes.Witch.Base.IsDisabled("MajorAmelioratingHex")) { return; }

                    var WitchHexMajorAmelioratingBlindedSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("66700a810f380c5419dc13f03bb76f45");
                    var WitchHexMajorAmelioratingCurseSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ee471eead8a069c449710ac5073322c0");
                    var WitchHexMajorAmelioratingDiseaseSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0870624a0fa21cf418d4c922da4205d4");
                    var WitchHexMajorAmelioratingPoisonSuppressBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2cf1962ef4cd4b5468c09bf4959d1bf7");

                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingBlindedSuppressBuff, TTTContext);
                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingCurseSuppressBuff, TTTContext);
                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingDiseaseSuppressBuff, TTTContext);
                    QuickFixTools.ReplaceSuppression(WitchHexMajorAmelioratingPoisonSuppressBuff, TTTContext);
                }
            }
        }
    }
}
