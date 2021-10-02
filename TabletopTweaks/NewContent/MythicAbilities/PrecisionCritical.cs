using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class PrecisionCritical {
        public static void AddPrecisionCritical() {
            var ImpromptuSneakAttackFeature = Resources.GetBlueprint<BlueprintFeature>("8ec618121de114845981933a3d5c4b02");

            var PrecisionCritical = Helpers.CreateBlueprint<BlueprintFeature>("PrecisionCritical", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = ImpromptuSneakAttackFeature.Icon;
                bp.SetName("Precision Critical");
                bp.SetDescription("Whenever you score a critical hit, double any extra precision damage dice, such as sneak attack damage. These dice are only doubled, not multiplied by the weapon’s critical modifier.");
                bp.AddComponent<PrecisionCriticalComponent>(c => {
                    c.CriticalMultiplier = 2;
                });
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("PrecisionCritical")) { return; }
            FeatTools.AddAsMythicAbility(PrecisionCritical);
        }
    }
}
