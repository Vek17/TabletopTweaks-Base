using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class AdamantineMind {
        public static void AddAdamantineMind() {
            var Stunned = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("09d39b38bb7c6014394b6daced9bacd3");
            var Icon_AdamantineMind = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_AdamantineMind.png");

            var AdamantineMind = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AdamantineMind", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = Icon_AdamantineMind;
                bp.SetName(TTTContext, "Adamantine Mind");
                bp.SetDescription(TTTContext, "Your mind is as hard as any armor, and is dangerous to engage.\n" +
                    "You gain a bonus equal to your tier on saving throws against mind-affecting effects. " +
                    "Whenever you succeed at a save against a mind-affecting effect, " +
                    "the creature attacking you with that effect must succeed at a Will save (at the same DC) or be stunned for 1 round.");
                bp.AddComponent<AdamantineMindTrigger>(c => {
                    c.Action = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = Stunned,
                            DurationValue = new ContextDurationValue() {
                                DiceCountValue = 0,
                                BonusValue = 1
                            }
                        }
                    );
                });
                bp.AddComponent<SavingThrowBonusAgainstDescriptor>(c => {
                    c.SpellDescriptor = SpellDescriptor.MindAffecting;
                    c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                    c.Bonus = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AdamantineMind")) { return; }
            FeatTools.AddAsMythicAbility(AdamantineMind);
        }
    }
}
