using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Cavalier {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Cavalier.DisableAllFixes) { return; }
                Main.LogHeader("Cavalier Resources");
                CavalierMountSelection();
                Main.LogHeader("Cavalier Complete");
            }
            static void CavalierMountSelection() {
                if (!ModSettings.Fixes.Cavalier.Base.Fixes["CavalierMountSelection"]) { return; }
                var CavalierMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
                var AnimalCompanionEmptyCompanion = Resources.GetBlueprint<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
                var AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
                var AnimalCompanionFeatureElk = Resources.GetBlueprint<BlueprintFeature>("aa92fea676be33d4dafd176d699d7996");

                var CavalierMountFeatureWolf = Helpers.CreateCopy(Resources.GetBlueprint<BlueprintFeature>("67a9dc42b15d0954ca4689b13e8dedea"), bp => {
                    bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["CavalierMountFeatureWolf"];
                    bp.AddComponent(Helpers.Create<PrerequisiteSize>(c => {
                        c.Size = Kingmaker.Enums.Size.Small;
                    }));
                });

                var CavalierMountFeatureDog = Helpers.CreateCopy(Resources.GetBlueprint<BlueprintFeature>("f894e003d31461f48a02f5caec4e3359"), bp => {
                    bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["CavalierMountFeatureDog"];
                    bp.AddComponent(Helpers.Create<PrerequisiteCharacterLevel>(c => {
                        c.Level = 4;
                    }));
                    bp.AddComponent(Helpers.Create<PrerequisiteSize>(c => {
                        c.Size = Kingmaker.Enums.Size.Small;
                    }));
                });
                
                Resources.AddBlueprint(CavalierMountFeatureWolf);
                Resources.AddBlueprint(CavalierMountFeatureDog);

                CavalierMountSelection.m_AllFeatures = new BlueprintFeatureReference[] {
                    AnimalCompanionEmptyCompanion.ToReference<BlueprintFeatureReference>(),
                    AnimalCompanionFeatureHorse.ToReference<BlueprintFeatureReference>(),
                    AnimalCompanionFeatureElk.ToReference<BlueprintFeatureReference>(),
                    CavalierMountFeatureWolf.ToReference<BlueprintFeatureReference>(),
                    CavalierMountFeatureDog.ToReference<BlueprintFeatureReference>(),
                };
                CavalierMountSelection.m_Features = CavalierMountSelection.m_AllFeatures;
                Main.LogPatch("Patched", CavalierMountSelection);
            }
            static void PatchArchetypes() {
            }
        }
    }
}
