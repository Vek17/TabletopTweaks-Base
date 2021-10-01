using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.Prerequisites;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class CavalierMounts {
        public static void AddCavalierMountFeatureWolf() {
            var AnimalCompanionFeatureWolf = Resources.GetBlueprint<BlueprintFeature>("67a9dc42b15d0954ca4689b13e8dedea");
            var CavalierMountFeatureWolf = Helpers.CreateCopy(AnimalCompanionFeatureWolf, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("CavalierMountFeatureWolf");
                bp.name = "CavalierMountFeatureWolf";
                bp.AddComponent(Helpers.Create<PrerequisiteSize>(c => {
                    c.Size = Kingmaker.Enums.Size.Small;
                }));
            });
            Resources.AddBlueprint(CavalierMountFeatureWolf);
        }
    }
}