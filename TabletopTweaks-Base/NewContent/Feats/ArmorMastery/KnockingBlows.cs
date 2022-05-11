using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ArmorMastery {
    static class KnockingBlows {
        internal static void AddKnockingBlows() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorFocusLight = BlueprintTools.GetBlueprint<BlueprintFeature>("3bc6e1d2b44b5bb4d92e6ba59577cf62");
            var ArmorFocusMedium = BlueprintTools.GetBlueprint<BlueprintFeature>("7dc004879037638489b64d5016997d12");
            var ArmorFocusHeavy = BlueprintTools.GetBlueprint<BlueprintFeature>("c27e6d2b0d33d42439f512c6d9a6a601");

            var LightArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
            var MediumArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");
            var HeavyArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("1b0f68188dcc435429fb87a022239681");

            var IntenseBlowsFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "IntenseBlowsFeature");
            var PowerAttackFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9972f33f977fc724c838e59641b2fca5");
            var PowerAttackBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5898bcf75a0942449a5dc16adc97b279");

            var KnockingBlowsBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "KnockingBlowsBuff", bp => {
                bp.SetName(TTTContext, "Knocking Blows");
                bp.SetDescription(TTTContext, "The weight of your blows overwhelms your opponents.\n" +
                    "Benefit: While wearing heavy armor if you hit a creature that is no more than one size category larger than you with a Power Attack, " +
                    "the creature you attacked is also knocked off balance. Until the beginning of your next turn, " +
                    "it takes a –4 penalty to its CMD against combat maneuvers that move it or knock it prone.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<CMDBonusAgainstManeuvers>(c => {
                    c.Value = -4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Maneuvers = new CombatManeuver[] { CombatManeuver.BullRush, CombatManeuver.Overrun, CombatManeuver.Pull, CombatManeuver.Trip };
                });
            });
            var KnockingBlowsEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "KnockingBlowsEffect", bp => {
                bp.SetName(KnockingBlowsBuff.m_DisplayName);
                bp.SetDescription(KnockingBlowsBuff.m_Description);
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponRangeType = true;
                    c.RangeType = WeaponRangeType.Melee;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<Conditional>(condition => {
                            condition.ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionSizeDifferneceFromCaster(){
                                        delta = 1
                                    }
                                }
                            };
                            condition.IfFalse = Helpers.CreateActionList();
                            condition.IfTrue = Helpers.CreateActionList(
                                Helpers.Create<ContextActionApplyBuff>(a => {
                                    a.m_Buff = KnockingBlowsBuff.ToReference<BlueprintBuffReference>();
                                    a.IsNotDispelable = true;
                                    a.DurationValue = new ContextDurationValue() {
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = 1
                                    };
                                })
                            );
                        })
                    );
                });
            });
            var KnockingBlowsFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "KnockingBlowsFeature", bp => {
                bp.SetName(KnockingBlowsBuff.m_DisplayName);
                bp.SetDescription(KnockingBlowsBuff.m_Description);
                bp.m_Icon = ArmorFocusLight.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = KnockingBlowsEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Strength;
                    c.Value = 13;
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 8;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 11;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeature(PowerAttackFeature);
                bp.AddPrerequisiteFeature(IntenseBlowsFeature);
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ArmorFocusHeavy);
            });

            if (TTTContext.AddedContent.ArmorMasteryFeats.IsDisabled("KnockingBlows")) { return; }
            ArmorMastery.AddToArmorMasterySelection(KnockingBlowsFeature);
        }
    }
}
