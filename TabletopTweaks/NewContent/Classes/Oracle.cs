using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Classes {
    class Oracle {
        public static void AddOracleFeatures() {
            var NaturesWhispersACConversion = Helpers.CreateBlueprint<BlueprintFeature>("NaturesWhispersACConversion", bp => {
                bp.SetName("Natures Whispers AC Conversion");
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent(Helpers.Create<ReplaceStatBaseAttribute>(c => {
                    c.TargetStat = StatType.AC;
                    c.BaseAttributeReplacement = StatType.Charisma;
                }));
                bp.AddComponent(Helpers.Create<ReplaceCMDDexterityStat>(c => {
                    c.NewStat = StatType.Charisma;
                }));
            });
        }
    }
}
