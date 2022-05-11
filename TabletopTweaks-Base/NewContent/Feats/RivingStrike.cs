using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class RivingStrike {
        public static void AddRivingStrike() {
            var ArcaneStrikeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0ab2f21a922feee4dab116238e3150b4");
            var ArcaneStrikeBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("98ac795afd1b2014eb9fdf2b9820808f");

            var RivingStrikeEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "RivingStrikeEffectBuff", bp => {
                bp.m_Icon = ArcaneStrikeFeature.m_Icon;
                bp.SetName(TTTContext, "Riving Strike");
                bp.SetDescription(TTTContext, "Creature takes a -2 penalty on saving throws against spells and spell-like abilities.");
                bp.AddComponent<SavingThrowBonusAgainstAbilityType>(c => {
                    c.AbilityType = AbilityType.Spell;
                    c.Bonus = -2;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<SavingThrowBonusAgainstAbilityType>(c => {
                    c.AbilityType = AbilityType.SpellLike;
                    c.Bonus = -2;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var RivingStrikeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "RivingStrikeBuff", bp => {
                bp.SetName(TTTContext, "Riving Strike");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = ArcaneStrikeFeature.m_Icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = RivingStrikeEffectBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            };
                        }));
                });
            });
            var RivingStrikeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "RivingStrikeFeature", bp => {
                bp.SetName(TTTContext, "Riving Strike");
                bp.SetDescription(TTTContext, "When you infuse your weapon with arcane might, your attacks make foes more susceptible to magic.\n" +
                    "Benefit: If you have a weapon that is augmented by your Arcane Strike feat, " +
                    "when you damage a creature with an attack made with that weapon, " +
                    "that creature takes a –2 penalty on saving throws against spells and spell-like abilities. " +
                    "This effect lasts for 1 round.");
                bp.m_Icon = RivingStrikeBuff.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<BuffExtraEffects>(c => {
                    c.m_CheckedBuff = ArcaneStrikeBuff;
                    c.m_ExtraEffectBuff = RivingStrikeBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddPrerequisiteFeature(ArcaneStrikeFeature);
                bp.AddPrerequisite<PrerequisiteCasterTypeSpellLevel>(c => {
                    c.IsArcane = true;
                    c.RequiredSpellLevel = 1;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Magic;
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("RivingStrike")) { return; }
            FeatTools.AddAsFeat(RivingStrikeFeature);
        }
    }
}
