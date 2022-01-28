using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.Properties;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Classes {
    static class Lich {
        public static void AddLichFeatures() {
            var LichDCProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("LichDCProperty", bp => {
                bp.AddComponent<CompositePropertyGetter>(c => {
                    c.CalculationMode = CompositePropertyGetter.Mode.Sum;
                    c.Properties = new CompositePropertyGetter.ComplexProperty[] {
                        new CompositePropertyGetter.ComplexProperty {
                            Property = UnitProperty.Level,
                            Numerator = 1,
                            Denominator = 2
                        },
                        new CompositePropertyGetter.ComplexProperty {
                            Property = UnitProperty.MythicLevel,
                            Numerator = 2,
                            Denominator = 1
                        }
                    };
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.AsIs,
                        m_CustomProgression = new PropertySettings.CustomProgressionItem[0]
                    };
                });
                bp.BaseValue = 10;
            });
        }
    }
}
