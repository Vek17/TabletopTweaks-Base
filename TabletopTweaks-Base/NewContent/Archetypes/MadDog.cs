using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    static class MadDog {
        public static void AddMadDogFeatures() {
            var madDogPetDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "MadDogPetDRProperty", bp => {
                bp.AddComponent<MadDogPetDRProperty>();
            });
        }
    }
}
