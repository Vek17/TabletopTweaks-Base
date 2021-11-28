using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.Prerequisites;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Loremaster {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Loremaster Resources");

                PatchPrerequisites();
                PatchSpellProgression();
                PatchSpellSecrets();
                PatchTricksterTricks();

                void PatchPrerequisites() {
                    if (ModSettings.Fixes.Loremaster.IsDisabled("Prerequisites")) { return; }

                    var LoremasterClass = Resources.GetBlueprint<BlueprintCharacterClass>("4a7c05adfbaf05446a6bf664d28fb103");

                    var SkillFocusSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c9629ef9eebb88b479b2fbc5e836656a");
                    var SkillFocusKnowledgeArcana = Resources.GetBlueprint<BlueprintFeature>("cad1b9175e8c0e64583432a22134d33c");
                    var SkillFocusKnowledgeWorld = Resources.GetBlueprint<BlueprintFeature>("611e863120c0f9a4cab2d099f1eb20b4");
                    var SkillFocusLoreNature = Resources.GetBlueprint<BlueprintFeature>("6507d2da389ed55448e0e1e5b871c013");
                    var SkillFocusLoreReligion = Resources.GetBlueprint<BlueprintFeature>("c541f80af8d0af4498e1abb6025780c7");

                    var EmpowerSpellFeat = Resources.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
                    var ExtendSpellFeat = Resources.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
                    var HeightenSpellFeat = Resources.GetBlueprint<BlueprintFeature>("2f5d1e705c7967546b72ad8218ccf99c");
                    var MaximizeSpellFeat = Resources.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
                    var QuickenSpellFeat = Resources.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
                    var ReachSpellFeat = Resources.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78");
                    var PersistentSpellFeat = Resources.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
                    var BolsteredSpellFeat = Resources.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");
                    var SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
                    var CompletelyNormalSpellFeat = Resources.GetBlueprint<BlueprintFeature>("094b6278f7b570f42aeaa98379f07cf2");
                    var ScribingScrolls = Resources.GetBlueprint<BlueprintFeature>("a8a385bf53ee3454593ce9054375a2ec");
                    var BrewPotions = Resources.GetBlueprint<BlueprintFeature>("c0f8c4e513eb493408b8070a1de93fc0");

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
                        c.m_Features = new BlueprintFeatureReference[] {
                            ScribingScrolls.ToReference<BlueprintFeatureReference>(),
                            BrewPotions.ToReference<BlueprintFeatureReference>(),
                            EmpowerSpellFeat.ToReference<BlueprintFeatureReference>(),
                            ExtendSpellFeat.ToReference<BlueprintFeatureReference>(),
                            HeightenSpellFeat.ToReference<BlueprintFeatureReference>(),
                            MaximizeSpellFeat.ToReference<BlueprintFeatureReference>(),
                            QuickenSpellFeat.ToReference<BlueprintFeatureReference>(),
                            ReachSpellFeat.ToReference<BlueprintFeatureReference>(),
                            PersistentSpellFeat.ToReference<BlueprintFeatureReference>(),
                            BolsteredSpellFeat.ToReference<BlueprintFeatureReference>(),
                            SelectiveSpellFeat.ToReference<BlueprintFeatureReference>(),
                            CompletelyNormalSpellFeat.ToReference<BlueprintFeatureReference>()
                        };
                        c.Amount = 3;
                    });
                    Main.LogPatch("Patched", LoremasterClass);
                }

                void PatchSpellProgression() {
                    if (ModSettings.Fixes.Loremaster.IsDisabled("SpellProgression")) { return; }
                    var LoremasterProgression = Resources.GetBlueprint<BlueprintProgression>("2bcd2330cc2c5a747968a8c782d4fa0a");
                    var LoremasterSecretSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("beeb25d7a7732e14f9986cdb79acecfc");
                    var LoremasterSpellbookSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7a28ab4dfc010834eabc770152997e87");
                    var LoremasterSpellbookSelectionTTT = Resources.GetModBlueprint<BlueprintFeatureSelection>("LoremasterSpellbookSelectionTTT");

                    LoremasterProgression.LevelEntries = LoremasterProgression.LevelEntries
                        .Where(entry => entry.Level != 1)
                        .Append(Helpers.CreateLevelEntry(1, LoremasterSpellbookSelectionTTT, LoremasterSecretSelection)).ToArray();
                    Main.LogPatch("Patched", LoremasterProgression);
                }

                void PatchSpellSecrets() {
                    if (ModSettings.Fixes.Loremaster.IsDisabled("SpellSecrets")) { return; }

                    var LoremasterSecretSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("beeb25d7a7732e14f9986cdb79acecfc");
                    var LoremasterClericSpellSecret = Resources.GetBlueprint<BlueprintFeatureSelection>("904ce918c85c9f947910340b956fb877");
                    var LoremasterDruidSpellSecret = Resources.GetBlueprint<BlueprintFeatureSelection>("6b73ba9d8a718fb419a484c6e1b92c6d");
                    var LoremasterWizardSpellSecret = Resources.GetBlueprint<BlueprintFeatureSelection>("f97986f19a595e2409cfe5d92bcf697c");

                    var LoremasterClericSpellSecret_TTT = Resources.GetModBlueprint<BlueprintFeatureSelection>("LoremasterClericSpellSecret_TTT");
                    var LoremasterDruidSpellSecret_TTT = Resources.GetModBlueprint<BlueprintFeatureSelection>("LoremasterDruidSpellSecret_TTT");
                    var LoremasterWizardSpellSecret_TTT = Resources.GetModBlueprint<BlueprintFeatureSelection>("LoremasterWizardSpellSecret_TTT");

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
                    Main.LogPatch("Patched", LoremasterSecretSelection);
                }

                void PatchTricksterTricks() {
                    if (ModSettings.Fixes.Loremaster.IsDisabled("TricksterTricks")) { return; }

                    var LoremasterCombatFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("90f105c8e31a6224ea319e6a810e4af8");

                    var TricksterImprovedImprovedCritical = Resources.GetBlueprint<BlueprintFeature>("56f94badbba018b4b8277ce6e2e79e72");
                    var TricksterImprovedImprovedImprovedCritical = Resources.GetBlueprint<BlueprintFeature>("006a966007802a0478c9e21007207aac");
                    var TricksterImprovedImprovedImprovedCriticalImproved = Resources.GetBlueprint<BlueprintFeature>("319c882ab3cc51544ad2f3f43633d5b1");

                    LoremasterCombatFeatSelection.RemoveFeatures(
                        TricksterImprovedImprovedCritical,
                        TricksterImprovedImprovedImprovedCritical,
                        TricksterImprovedImprovedImprovedCriticalImproved
                    );
                    Main.LogPatch("Patched", LoremasterCombatFeatSelection);
                }
            }
        }
    }
}
