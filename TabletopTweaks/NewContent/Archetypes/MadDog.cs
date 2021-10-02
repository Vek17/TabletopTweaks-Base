using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    static class MadDog {
        public static void AddMadDogFeatures() {
            var madDogPetDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("MadDogPetDRProperty", bp => {
                bp.AddComponent<MadDogPetDRProperty>();
            });
        }
    }
}
