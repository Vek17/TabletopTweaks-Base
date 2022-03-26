using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ShieldMastery {
    static class DefendedMovement {
        internal static void AddDefendedMovement() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ShieldFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("ac57069b6bf8c904086171683992a92a");

            var DefendedMovementEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DefendedMovementEffect", bp => {
                bp.SetName(TTTContext, "Defended Movement");
                bp.SetDescription(TTTContext, "You deftly block attacks with your shield while moving through battle.\n" +
                    "Benefit: You gain a +2 bonus to your AC against attacks of opportunity.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<ACBonusAgainstAttacks>(c => {
                    c.OnlyAttacksOfOpportunity = true;
                    c.Value = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var DefendedMovementFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DefendedMovementFeature", bp => {
                bp.SetName(DefendedMovementEffect.m_DisplayName);
                bp.SetDescription(DefendedMovementEffect.m_Description);
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = DefendedMovementEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Buckler,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield,
                    };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 4;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 6;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ShieldFocus);
            });

            if (TTTContext.AddedContent.ShieldMasteryFeats.IsDisabled("DefendedMovement")) { return; }
            ShieldMastery.AddToShieldMasterySelection(DefendedMovementFeature);
        }
    }
}
