using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class AbilityFocusStunningFist {
        public static void AddAbilityFocusStunningFist() {
            var StunningFist = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
            var StunningFistAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("732ae7773baf15447a6737ae6547fc1e");
            var StunningFistFatigueAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("32f92fea1ab81c843a436a49f522bfa1");
            var StunningFistSickenedAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c81906c75821cbe4c897fa11bdaeee01");

            var AbilityFocusStunningFist = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AbilityFocusStunningFist", bp => {
                bp.SetName(TTTContext, "Ability Focus — Stunning Fist");
                bp.SetDescription(TTTContext, "Add +2 to the DC for the effects you deliver with your Stunning Fist.");
                bp.m_Icon = StunningFist.Icon;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistFatigueAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistSickenedAbility;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
                bp.AddPrerequisiteFeature(StunningFist);
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("AbilityFocusStunningFist")) { return; }
            FeatTools.AddAsFeat(AbilityFocusStunningFist);
        }
    }
}
