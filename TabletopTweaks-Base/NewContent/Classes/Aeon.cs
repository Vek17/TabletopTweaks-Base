using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    internal class Aeon {
        public static void AddAeonFeatures() {

            var Artifact_AeonCloakDCProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "Artifact_AeonCloakDCProperty", bp => {
                bp.BaseValue = 10;
                bp.AddComponent<SimplePropertyGetter>(c => {
                    c.Property = UnitProperty.MythicLevel;
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.AsIs
                    };
                });
                bp.AddComponent<SimplePropertyGetter>(c => {
                    c.Property = UnitProperty.StatBonusWisdom;
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.AsIs
                    };
                });
            });
        }
    }
}
