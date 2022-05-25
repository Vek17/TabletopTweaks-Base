using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Oracle {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Oracle_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Oracle")) { return; }

                var OracleFinalRevelation = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("0336dc22538ba5f42b73da4fb3f50849");
                var OracleAlternateCapstone = NewContent.AlternateCapstones.Oracle.OracleAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();
                var DiverseMysteries = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DiverseMysteries");
                var OracleRevelationSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("60008a10ad7ad6543b1f63016741a5d2");
                var OracleMysterySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("5531b975dcdf0e24c98f1ff7e017e741");

                OracleFinalRevelation.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                foreach (var mystery in OracleMysterySelection.AllFeatures) {
                    var capstone = mystery.GetComponent<AddFeatureOnClassLevel>(c => c.Level == 20).m_Feature;
                    mystery.RemoveComponents<AddFeatureOnClassLevel>(c => c.Level == 20);
                    mystery.AddComponent<AddFeatureOnClassLevelToPlayers>(c => {
                        c.Not = true;
                        c.Level = 20;
                        c.m_Class = ClassTools.Classes.OracleClass.ToReference<BlueprintCharacterClassReference>();
                        c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
                        c.m_Archetypes = new BlueprintArchetypeReference[0];
                        c.m_Feature = capstone;
                    });
                    capstone.Get().AddPrerequisiteFeature(mystery);
                    OracleRevelationSelection.AllFeatures.ForEach(revelation => {
                        var prerequisite = revelation.GetComponent<PrerequisiteFeaturesFromList>(c => c.m_Features.Any(f => f.deserializedGuid == mystery.AssetGuid));
                        if (prerequisite == null) { return; }
                        revelation.AddComponent<PrerequisiteOracleMystery>(c => {
                            c.m_BypassFeature = DiverseMysteries.ToReference<BlueprintFeatureReference>();
                            c.m_Features = prerequisite.m_Features.ToArray();
                            c.Amount = 1;
                        });
                        revelation.RemoveComponent(prerequisite);
                    });
                }
                ClassTools.Classes.OracleClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == OracleFinalRevelation.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(OracleAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(OracleAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == OracleFinalRevelation.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(OracleAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Oracle");
                PatchBase();
            }
            static void PatchBase() {
                PatchNaturesWhisper();
                PatchFlameMystery();

                void PatchNaturesWhisper() {
                    if (TTTContext.Fixes.Oracle.Base.IsDisabled("NaturesWhisperMonkStacking")) { return; }

                    var OracleRevelationNatureWhispers = BlueprintTools.GetBlueprint<BlueprintFeature>("3d2cd23869f0d98458169b88738f3c32");
                    var NaturesWhispersACConversion = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "NaturesWhispersACConversion");
                    var ScaledFistACBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("3929bfd1beeeed243970c9fc0cf333f8");

                    OracleRevelationNatureWhispers.RemoveComponents<ReplaceStatBaseAttribute>();
                    OracleRevelationNatureWhispers.RemoveComponents<ReplaceCMDDexterityStat>();
                    OracleRevelationNatureWhispers.AddComponent<HasFactFeatureUnlock>(c => {
                        c.m_CheckedFact = ScaledFistACBonus.ToReference<BlueprintUnitFactReference>();
                        c.m_Feature = NaturesWhispersACConversion.ToReference<BlueprintUnitFactReference>();
                        c.Not = true;
                    });
                    TTTContext.Logger.LogPatch("Patched", OracleRevelationNatureWhispers);
                }

                void PatchFlameMystery() {
                    PatchRevelationBurningMagic();

                    void PatchRevelationBurningMagic() {
                        if (TTTContext.Fixes.Oracle.Base.IsDisabled("RevelationBurningMagic")) { return; }

                        var OracleRevelationBurningMagicBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4ae27ae7c3d758041b25e9a3aff73592");
                        OracleRevelationBurningMagicBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.AsIs;
                        TTTContext.Logger.LogPatch("Patched", OracleRevelationBurningMagicBuff);
                    }
                }
            }
            static void PatchArchetypes() {
            }
        }
    }
}
