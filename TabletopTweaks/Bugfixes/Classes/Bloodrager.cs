using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Bloodrager {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Bloodrager");

                PatchBaseClass();
                PatchPrimalist();
                PatchReformedFiend();
                PatchSteelblood();
            }
            static void PatchBaseClass() {
                PatchSpellbook();
                PatchAbysalBulk();
                PatchLimitlessRage();

                void PatchAbysalBulk() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("AbysalBulk")) { return; }
                    var BloodragerAbyssalBloodlineBaseBuff = Resources.GetBlueprint<BlueprintBuff>("2ba7b4b3b87156543b43d0686404655a");
                    var BloodragerAbyssalDemonicBulkBuff = Resources.GetBlueprint<BlueprintBuff>("031a8053a7c02ab42ad53f50dd2e9437");
                    var BloodragerAbyssalDemonicBulkEnlargeBuff = Resources.GetModBlueprint<BlueprintBuff>("BloodragerAbyssalDemonicBulkEnlargeBuff");

                    var ApplyBuff = new ContextActionApplyBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>(),
                        AsChild = true,
                        Permanent = true
                    };
                    var RemoveBuff = new ContextActionRemoveBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>()
                    };
                    var AddFactContext = BloodragerAbyssalBloodlineBaseBuff.GetComponent<AddFactContextActions>();

                    AddFactContext.Activated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(ApplyBuff);
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().IfTrue = null;
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(RemoveBuff);
                }
                void PatchSpellbook() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("Spellbook")) { return; }
                    BlueprintSpellbook BloodragerSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
                    var BloodragerSpellKnownTable = BloodragerSpellbook.SpellsKnown;
                    var BloodragerSpellPerDayTable = BloodragerSpellbook.SpellsPerDay;
                    BloodragerSpellbook.CasterLevelModifier = 0;
                    BloodragerSpellKnownTable.Levels = new SpellsLevelEntry[] {
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0,2),
                        CreateSpellLevelEntry(0,3),
                        CreateSpellLevelEntry(0,4),
                        CreateSpellLevelEntry(0,4,2),
                        CreateSpellLevelEntry(0,4,3),
                        CreateSpellLevelEntry(0,5,4),
                        CreateSpellLevelEntry(0,5,4,2),
                        CreateSpellLevelEntry(0,5,4,3),
                        CreateSpellLevelEntry(0,6,5,4),
                        CreateSpellLevelEntry(0,6,5,4,2),
                        CreateSpellLevelEntry(0,6,5,4,3),
                        CreateSpellLevelEntry(0,6,6,5,4),
                        CreateSpellLevelEntry(0,6,6,5,4),
                        CreateSpellLevelEntry(0,6,6,5,4),
                        CreateSpellLevelEntry(0,6,6,6,5),
                        CreateSpellLevelEntry(0,6,6,6,5),
                        CreateSpellLevelEntry(0,6,6,6,5)
                    };
                    BloodragerSpellPerDayTable.Levels = new SpellsLevelEntry[] {
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1,1),
                        CreateSpellLevelEntry(0,1,1),
                        CreateSpellLevelEntry(0,2,1),
                        CreateSpellLevelEntry(0,2,1,1),
                        CreateSpellLevelEntry(0,2,1,1),
                        CreateSpellLevelEntry(0,2,2,1),
                        CreateSpellLevelEntry(0,3,2,1,1),
                        CreateSpellLevelEntry(0,3,2,1,1),
                        CreateSpellLevelEntry(0,3,2,2,1),
                        CreateSpellLevelEntry(0,3,3,2,1),
                        CreateSpellLevelEntry(0,4,3,2,1),
                        CreateSpellLevelEntry(0,4,3,2,2),
                        CreateSpellLevelEntry(0,4,3,3,2),
                        CreateSpellLevelEntry(0,4,4,3,2)
                    };
                    Main.LogPatch("Patched", BloodragerSpellPerDayTable);
                    SpellsLevelEntry CreateSpellLevelEntry(params int[] count) {
                        var entry = new SpellsLevelEntry();
                        entry.Count = count;
                        return entry;
                    }
                }
                void PatchLimitlessRage() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("LimitlessRage")) { return; }
                    var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                    var BloodragerRageResource = Resources.GetBlueprint<BlueprintAbilityResource>("4aec9ec9d9cd5e24a95da90e56c72e37");
                    BloodragerStandartRageBuff
                        .GetComponent<TemporaryHitPointsPerLevel>()
                        .m_LimitlessRageResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();
                    Main.LogPatch("Patched", BloodragerRageResource);
                }
            }
            static void PatchPrimalist() {
                PatchRagePowerFeatQualifications();

                void PatchRagePowerFeatQualifications() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("RagePowerFeatQualifications")) { return; }
                    var PrimalistTakeRagePowers4 = Resources.GetBlueprint<BlueprintProgression>("8eb5c34bb8471a0438e7eb3994de3b92");
                    var PrimalistTakeRagePowers8 = Resources.GetBlueprint<BlueprintProgression>("db2710cd915bbcf4193fa54083e56b27");
                    var PrimalistTakeRagePowers12 = Resources.GetBlueprint<BlueprintProgression>("e43a7bfd5c90a514cab1c11b41c550b1");
                    var PrimalistTakeRagePowers16 = Resources.GetBlueprint<BlueprintProgression>("b6412ff44f3a82f499d0dd6748a123bc");
                    var PrimalistTakeRagePowers20 = Resources.GetBlueprint<BlueprintProgression>("5905a80d5934248439e83612d9101b4b");

                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers4, 4);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers8, 8);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers12, 12);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers16, 16);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers20, 20);

                    void PatchPrimalistTakeRagePowers(BlueprintProgression PrimalistTakeRagePowers, int level) {
                        var PrimalistRagePowerSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("PrimalistRagePowerSelection");
                        PrimalistTakeRagePowers.LevelEntries = new LevelEntry[] {
                            new LevelEntry {
                                Level = level,
                                Features = {
                                    PrimalistRagePowerSelection,
                                    PrimalistRagePowerSelection
                                }
                            }
                        };
                        Main.LogPatch("Patched", PrimalistTakeRagePowers);
                    }
                }
            }
            static void PatchReformedFiend() {
                PatchHatredAgainstEvil();
                PatchDamageReduction();

                void PatchHatredAgainstEvil() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("HatredAgainstEvil")) { return; }
                    var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
                    var ReformedFiendBloodrageBuff = Resources.GetBlueprint<BlueprintBuff>("72a679f712bd4f69a07bf03d5800900b");
                    var rankConfig = ReformedFiendBloodrageBuff.GetComponent<ContextRankConfig>();

                    rankConfig.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    rankConfig.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass.ToReference<BlueprintCharacterClassReference>() };
                    rankConfig.m_UseMin = true;
                }
                void PatchDamageReduction() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("DamageReduction")) { return; }
                    var ReformedFiendDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("2a3243ad1ccf43d5a5d69de3f9d0420e");
                    ReformedFiendDamageReductionFeature.GetComponent<AddDamageResistancePhysical>().BypassedByAlignment = true;
                }
            }

            static void PatchSteelblood()
            {
                PatchArmorTraining();

                void PatchArmorTraining()
                {
                    var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                    var Steelblood = Resources.GetBlueprint<BlueprintArchetype>("32a5dff92373a9641b43e97d453b9369");
                    Steelblood.AddFeatures.First(x => x.Level == 1).m_Features.Add(Resources.GetModBlueprint<BlueprintFeature>("SteelbloodArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());

                    var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                    foreach (LevelEntry i in Steelblood.AddFeatures.Where(x => x.Features.Contains(ArmorTraining) && x.Level > 5))
                    {
                        i.Features.Remove(ArmorTraining);
                        i.Features.Add(ArmorTrainingSelection);
                    }
                }
            }
        }
    }
}
