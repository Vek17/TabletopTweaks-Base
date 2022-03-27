using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    class Oracle {
        public static void AddOracleFeatures() {
            var NaturesWhispersACConversion = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "NaturesWhispersACConversion", bp => {
                bp.SetName(TTTContext, "Natures Whispers AC Conversion");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent<ReplaceStatBaseAttribute>(c => {
                    c.TargetStat = StatType.AC;
                    c.BaseAttributeReplacement = StatType.Charisma;
                });
                bp.AddComponent<ReplaceCMDDexterityStat>(c => {
                    c.NewStat = StatType.Charisma;
                });
                bp.AddComponent<ForceACUpdate>();
            });
        }
    }
}
