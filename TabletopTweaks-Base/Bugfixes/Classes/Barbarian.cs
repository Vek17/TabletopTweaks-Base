using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Barbarian {
        [PatchBlueprintsCacheInit]
        static class Barbarian_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Barbarian")) { return; }

                var MightyRage = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("06a7e5b60020ad947aed107d82d1f897");
                var BarbarianAlternateCapstone = NewContent.AlternateCapstones.Barbarian.BarbarianAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MightyRage.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.BarbarianClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MightyRage.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(BarbarianAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(BarbarianAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MightyRage.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(BarbarianAlternateCapstone));
                    });
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
                TTTContext.Logger.LogHeader("Patching Barbarian");

                PatchBase();
                PatchAnimalFury();
                PatchBeastTotem();
                PatchWreckingBlows();
                PatchCripplingBlows();
            }

            static void PatchBase() {
            }

            static void PatchAnimalFury() {
                if (TTTContext.Fixes.Barbarian.Base.IsDisabled("AnimalFury")) { return; }

                var AnimalFuryBite = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("8c01e7fccbb829947bc5894f63fb389a");
                AnimalFuryBite.TemporaryContext(bp => {
                    bp.KeepInPolymorph = true;
                });

                TTTContext.Logger.LogPatch(AnimalFuryBite);
            }
            static void PatchBeastTotem() {
                if (TTTContext.Fixes.Barbarian.Base.IsDisabled("BeastTotem")) { return; }

                var BeastTotemFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c085888db293f2741b881cc989a2ab14");
                var BeastTotemRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ec7db4946877f73439c4ee661f645452");
                BeastTotemRageBuff.TemporaryContext(bp => {
                    bp.m_DisplayName = BeastTotemFeature.m_DisplayName;
                });

                TTTContext.Logger.LogPatch(BeastTotemRageBuff);
            }
            static void PatchWreckingBlows() {
                if (TTTContext.Fixes.Barbarian.Base.IsDisabled("WreckingBlows")) { return; }
                var WreckingBlowsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("5bccc86dd1f187a4a99f092dc054c755");
                var PowerfulStanceEffectBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");
                WreckingBlowsFeature.GetComponent<BuffExtraEffects>().m_CheckedBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                TTTContext.Logger.LogPatch("Patched", WreckingBlowsFeature);
            }

            static void PatchCripplingBlows() {
                if (TTTContext.Fixes.Barbarian.Base.IsDisabled("CripplingBlows")) { return; }
                var CripplingBlowsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0eec6efbb7f66e148817c9f51b804f08");
                var PowerfulStanceEffectBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");
                CripplingBlowsFeature.GetComponent<BuffExtraEffects>().m_CheckedBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                TTTContext.Logger.LogPatch("Patched", CripplingBlowsFeature);
            }
        }
    }
}
