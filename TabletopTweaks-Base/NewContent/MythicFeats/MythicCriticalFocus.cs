using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    static class MythicCriticalFocus {
        public static void AddMythicCriticalFocus() {
            var MonsterMythicClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("2a624b417a801be49b8b97b6355e2f4c");
            var CriticalFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("8ac59959b1b23c347a0361dc97cc786d");
            var CriticalFocusMythic = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "CriticalFocusMythicFeature", bp => {
                bp.m_Icon = CriticalFocus.m_Icon;
                bp.SetName(TTTContext, "Critical Focus (Mythic)");
                bp.SetDescription(TTTContext, "Your blows unerringly find your target’s vital spots.\n" +
                    "You automatically confirm critical threats against non-mythic opponents. " +
                    "In addition, when you threaten a critical hit against a creature wearing armor with " +
                    "the fortification special ability or similar effect, that creature must roll twice and " +
                    "take the worse result when determining critical hit negation.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<RerollFortification>(c => {
                    c.ConfirmedCriticals = true;
                    c.SneakAttacks = false;
                    c.PreciseStrike = false;
                    c.RerollCount = 1;
                    c.TakeBest = true;
                });
                bp.AddComponent<CritAutoconfirmAgainstClass>(c => {
                    c.ExceptClasses = true;
                    c.m_Classes = new BlueprintCharacterClassReference[] {
                        MonsterMythicClass
                    };
                });
                bp.AddPrerequisiteFeature(CriticalFocus);
            });
            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicCombatReflexes")) { return; }
            FeatTools.AddAsMythicFeat(CriticalFocusMythic);
        }
    }
}
