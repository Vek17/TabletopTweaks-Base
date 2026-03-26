using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Loremaster {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Loremaster Resources");

                PatchPrerequisites();
                PatchSpellProgression();
                PatchSpellSecrets();
                PatchTricksterTricks();

                void PatchPrerequisites() {
                    if (TTTContext.Fixes.Loremaster.IsDisabled("Prerequisites")) { return; }

                    var LoremasterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("4a7c05adfbaf05446a6bf664d28fb103");

                    var SkillFocusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c9629ef9eebb88b479b2fbc5e836656a");
                    var SkillFocusKnowledgeArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("cad1b9175e8c0e64583432a22134d33c");
                    var SkillFocusKnowledgeWorld = BlueprintTools.GetBlueprint<BlueprintFeature>("611e863120c0f9a4cab2d099f1eb20b4");
                    var SkillFocusLoreNature = BlueprintTools.GetBlueprint<BlueprintFeature>("6507d2da389ed55448e0e1e5b871c013");
                    var SkillFocusLoreReligion = BlueprintTools.GetBlueprint<BlueprintFeature>("c541f80af8d0af4498e1abb6025780c7");
                    /*
                        var EmpowerSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
                        var ExtendSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
                        var HeightenSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("2f5d1e705c7967546b72ad8218ccf99c");
                        var MaximizeSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
                        var QuickenSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
                        var ReachSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78");
                        var PersistentSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
                        var BolsteredSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");
                        var SelectiveSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
                        var CompletelyNormalSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("094b6278f7b570f42aeaa98379f07cf2");
                    */
                    var ScribingScrolls = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a8a385bf53ee3454593ce9054375a2ec");
                    var BrewPotions = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("c0f8c4e513eb493408b8070a1de93fc0");

                    LoremasterClass.RemoveComponents<Prerequisite>();
                    LoremasterClass.AddComponent<PrerequisiteCasterTypeSpellLevel>(c => {
                        c.RequiredSpellLevel = 3;
                        c.Group = Prerequisite.GroupType.Any;
                    });
                    LoremasterClass.AddComponent<PrerequisiteCasterTypeSpellLevel>(c => {
                        c.RequiredSpellLevel = 3;
                        c.IsArcane = true;
                        c.Group = Prerequisite.GroupType.Any;
                    });
                    LoremasterClass.AddComponent<PrerequisiteStatValues>(c => {
                        c.Value = 7;
                        c.Amount = 2;
                        c.Stats = new StatType[] {
                            StatType.SkillKnowledgeArcana,
                            StatType.SkillKnowledgeWorld,
                            StatType.SkillLoreNature,
                            StatType.SkillLoreReligion
                        };
                    });

                    LoremasterClass.AddComponent<PrerequisiteFeaturesFromListFormatted>(c => {
                        c.m_Features = new BlueprintFeatureReference[] {
                            SkillFocusKnowledgeArcana.ToReference<BlueprintFeatureReference>(),
                            SkillFocusKnowledgeWorld.ToReference<BlueprintFeatureReference>(),
                            SkillFocusLoreNature.ToReference<BlueprintFeatureReference>(),
                            SkillFocusLoreReligion.ToReference<BlueprintFeatureReference>()
                        };
                        c.Amount = 1;
                    });
                    LoremasterClass.AddComponent<PrerequisiteFeaturesFromListFormatted>(c => {
                        c.m_Features = FeatTools.GetMetamagicFeats()
                        .Select(feat => feat.ToReference<BlueprintFeatureReference>())
                        .AddItem(ScribingScrolls)
                        .AddItem(BrewPotions)
                        .ToArray();
                        c.Amount = 3;
                    });
                    TTTContext.Logger.LogPatch("Patched", LoremasterClass);
                }
                void PatchSpellProgression() {
                    if (TTTContext.Fixes.Loremaster.IsDisabled("SpellProgression")) { return; }
                    var LoremasterProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("2bcd2330cc2c5a747968a8c782d4fa0a");
                    var LoremasterSecretSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("beeb25d7a7732e14f9986cdb79acecfc");
                    var LoremasterSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7a28ab4dfc010834eabc770152997e87");
                    var LoremasterSpellbookSelectionTTT = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "LoremasterSpellbookSelectionTTT");

                    LoremasterProgression.LevelEntries = LoremasterProgression.LevelEntries
                        .Where(entry => entry.Level != 1)
                        .Append(Helpers.CreateLevelEntry(1, LoremasterSpellbookSelectionTTT, LoremasterSecretSelection)).ToArray();
                    TTTContext.Logger.LogPatch("Patched", LoremasterProgression);
                }
                void PatchSpellSecrets() {
                    if (TTTContext.Fixes.Loremaster.IsDisabled("SpellSecrets")) { return; }

                    var LoremasterSecretSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("beeb25d7a7732e14f9986cdb79acecfc");
                    var LoremasterClericSpellSecret = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("904ce918c85c9f947910340b956fb877");
                    var LoremasterDruidSpellSecret = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("6b73ba9d8a718fb419a484c6e1b92c6d");
                    var LoremasterWizardSpellSecret = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("f97986f19a595e2409cfe5d92bcf697c");

                    var LoremasterClericSpellSecret_TTT = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "LoremasterClericSpellSecret_TTT");
                    var LoremasterDruidSpellSecret_TTT = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "LoremasterDruidSpellSecret_TTT");
                    var LoremasterWizardSpellSecret_TTT = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "LoremasterWizardSpellSecret_TTT");

                    LoremasterSecretSelection.RemoveFeatures(
                        LoremasterClericSpellSecret,
                        LoremasterDruidSpellSecret,
                        LoremasterWizardSpellSecret
                    );
                    LoremasterSecretSelection.AddFeatures(
                        LoremasterClericSpellSecret_TTT,
                        LoremasterDruidSpellSecret_TTT,
                        LoremasterWizardSpellSecret_TTT
                    );
                    TTTContext.Logger.LogPatch("Patched", LoremasterSecretSelection);
                }
                void PatchTricksterTricks() {
                    if (TTTContext.Fixes.Loremaster.IsDisabled("TricksterTricks")) { return; }

                    var LoremasterCombatFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("90f105c8e31a6224ea319e6a810e4af8");

                    var TricksterImprovedImprovedCritical = BlueprintTools.GetBlueprint<BlueprintFeature>("56f94badbba018b4b8277ce6e2e79e72");
                    var TricksterImprovedImprovedImprovedCritical = BlueprintTools.GetBlueprint<BlueprintFeature>("006a966007802a0478c9e21007207aac");
                    var TricksterImprovedImprovedImprovedCriticalImproved = BlueprintTools.GetBlueprint<BlueprintFeature>("319c882ab3cc51544ad2f3f43633d5b1");

                    LoremasterCombatFeatSelection.RemoveFeatures(
                        TricksterImprovedImprovedCritical,
                        TricksterImprovedImprovedImprovedCritical,
                        TricksterImprovedImprovedImprovedCriticalImproved
                    );
                    TTTContext.Logger.LogPatch("Patched", LoremasterCombatFeatSelection);
                }
            }
        }
    }
}
