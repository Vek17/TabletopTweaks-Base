using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Classes {
    class Oracle {
        public static void AddOracleFeatures() {
            var NaturesWhispersACConversion = Helpers.CreateBlueprint<BlueprintFeature>("NaturesWhispersACConversion", bp => {
                bp.SetName("fcaaf44a85c14bb092b23e75b299db5d", "Natures Whispers AC Conversion");
                bp.SetDescription("fb09ce56707e4ad191a0d5c6470e7620", "");
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
