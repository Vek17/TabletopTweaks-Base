using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.ActivatableAbilities;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class MaximizedCritical {
        public static void AddMaximizedCritical() {
            var SwordSaintPerfectStrikeAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5a169c57935dc3343836c027e35d65b3");

            var MaximizedCritical = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MaximizedCritical", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = SwordSaintPerfectStrikeAbility.Icon;
                bp.SetName(TTTContext, "Maximized Critical");
                bp.SetDescription(TTTContext, "Whenever you score a critical hit, the weapon’s damage result is always the maximum possible amount you could roll. " +
                    "This doesn’t affect other dice added to the damage, such as from sneak attack or the flaming weapon special ability. " +
                    "For example, if you score a critical hit with a longsword (1d8/×2), " +
                    "treat the sword’s damage dice as if you had rolled 8 both times, " +
                    "then add any other damage bonuses that you would normally apply to a critical hit.");
                bp.AddComponent<MaximizedCriticalComponent>();
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("MaximizedCritical")) { return; }
            FeatTools.AddAsMythicAbility(MaximizedCritical);
        }
    }
}
