using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class AgeResistance {
        public static void AddAgeResistance() {
            var Icon_AgeResistanceLesser = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_AgeResistanceLesser.png");
            var Icon_AgeResistance = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_AgeResistance.png");
            var Icon_AgeResistanceGreater = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_AgeResistanceGreater.png");
            var Icon_ScrollOfAgeResistanceLesser = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfAgeResistanceLesser.png");
            var Icon_ScrollOfAgeResistance = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfAgeResistance.png");
            var Icon_ScrollOfAgeResistanceGreater = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfAgeResistanceGreater.png");

            var resistenergy00 = new PrefabLink() {
                AssetId = "e23fec8d2024a8c48a8b4a57693e31a7"
            };

            var AgeResistanceLesserBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "AgeResistanceLesserBuff", bp => {
                bp.SetName(TTTContext, "Age Resistance, Lesser");
                bp.SetDescription(TTTContext, "You ignore the physical detriments of being middle-aged. " +
                    "This spell does not cause you to look younger, nor does it prevent you from dying of old age, " +
                    "but as long as the spell is in effect, you ignore the –1 penalties to " +
                    "Strength, Dexterity, and Constitution that accrue once you become middle-aged. " +
                    "You retain the age-related bonuses to Intelligence, Wisdom, and Charisma while under the effects of this spell. " +
                    "Additional penalties that you accrue upon becoming old or venerable apply in full.");
                bp.m_Icon = Icon_AgeResistanceLesser;
                bp.FxOnStart = resistenergy00;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
            });
            var AgeResistanceLesserAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "AgeResistanceLesserAbility", bp => {
                bp.SetName(AgeResistanceLesserBuff.m_DisplayName);
                bp.SetDescription(AgeResistanceLesserBuff.m_Description);
                bp.m_Icon = Icon_AgeResistanceLesser;
                bp.SetLocalizedDuration(TTTContext, "24 hours");
                bp.SetLocalizedSavingThrow(TTTContext, "");
                bp.AvailableMetamagic = Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = true;
                bp.SpellResistance = false;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;

                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            m_Buff = AgeResistanceLesserBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Days,
                                BonusValue = 1,
                                DiceCountValue = 0
                            }
                        }
                    );
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Transmutation;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Buff;
                    c.SavingThrow = CraftSavingThrow.None;
                    c.AOEType = CraftAOE.None;
                });
            });
            var AgeResistanceBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "AgeResistanceBuff", bp => {
                bp.SetName(TTTContext, "Age Resistance");
                bp.SetDescription(TTTContext, "You ignore the physical detriments of being middle-aged or old-aged. " +
                    "This spell does not cause you to look younger, nor does it prevent you from dying of old age, " +
                    "but as long as the spell is in effect, you ignore the –3 penalties to " +
                    "Strength, Dexterity, and Constitution that accrue once you become old-aged. " +
                    "You retain the age-related bonuses to Intelligence, Wisdom, and Charisma while under the effects of this spell. " +
                    "Additional penalties that you accrue upon becoming venerable apply in full.");
                bp.m_Icon = Icon_AgeResistance;
                bp.FxOnStart = resistenergy00;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
            });
            var AgeResistanceAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "AgeResistanceAbility", bp => {
                bp.SetName(AgeResistanceBuff.m_DisplayName);
                bp.SetDescription(AgeResistanceBuff.m_Description);
                bp.m_Icon = Icon_AgeResistance;
                bp.SetLocalizedDuration(TTTContext, "24 hours");
                bp.SetLocalizedSavingThrow(TTTContext, "");
                bp.AvailableMetamagic = Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = true;
                bp.SpellResistance = false;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;

                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionRemoveBuff() {
                            m_Buff = AgeResistanceLesserBuff.ToReference<BlueprintBuffReference>()
                        },
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            m_Buff = AgeResistanceBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Days,
                                BonusValue = 1,
                                DiceCountValue = 0
                            }
                        }
                    );
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Transmutation;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Buff;
                    c.SavingThrow = CraftSavingThrow.None;
                    c.AOEType = CraftAOE.None;
                });
            });
            var AgeResistanceGreaterBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "AgeResistanceGreaterBuff", bp => {
                bp.SetName(TTTContext, "Age Resistance, Greater");
                bp.SetDescription(TTTContext, "You ignore the physical detriments of age. " +
                    "This spell does not cause you to look younger, nor does it prevent you from dying of old age, " +
                    "but as long as the spell is in effect, you ignore the –6 penalties to " +
                    "Strength, Dexterity, and Constitution that accrue once you become venerable. " +
                    "You retain the age-related bonuses to Intelligence, Wisdom, and Charisma while under the effects of this spell.");
                bp.m_Icon = Icon_AgeResistanceGreater;
                bp.FxOnStart = resistenergy00;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.Venerable;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
            });
            var AgeResistanceGreaterAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "AgeResistanceGreaterAbility", bp => {
                bp.SetName(AgeResistanceGreaterBuff.m_DisplayName);
                bp.SetDescription(AgeResistanceGreaterBuff.m_Description);
                bp.m_Icon = Icon_AgeResistanceGreater;
                bp.SetLocalizedDuration(TTTContext, "24 hours");
                bp.SetLocalizedSavingThrow(TTTContext, "Will negates");
                bp.AvailableMetamagic = Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = true;
                bp.SpellResistance = false;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionRemoveBuff() {
                            m_Buff = AgeResistanceLesserBuff.ToReference<BlueprintBuffReference>()
                        },
                        new ContextActionRemoveBuff() {
                            m_Buff = AgeResistanceBuff.ToReference<BlueprintBuffReference>()
                        },
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            m_Buff = AgeResistanceGreaterBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Days,
                                BonusValue = 1,
                                DiceCountValue = 0
                            }
                        }
                    );
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Transmutation;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Buff;
                    c.SavingThrow = CraftSavingThrow.None;
                    c.AOEType = CraftAOE.None;
                });
            });

            var ScrollOfAgeResistanceLesser = ItemTools.CreateScroll(TTTContext, "ScrollOfAgeResistanceLesser", Icon_ScrollOfAgeResistanceLesser, AgeResistanceLesserAbility, 4, 7);
            var ScrollOfAgeResistance = ItemTools.CreateScroll(TTTContext, "ScrollOfAgeResistance", Icon_ScrollOfAgeResistance, AgeResistanceAbility, 6, 11);
            var ScrollOfAgeResistanceGreater = ItemTools.CreateScroll(TTTContext, "ScrollOfAgeResistanceGreater", Icon_ScrollOfAgeResistanceGreater, AgeResistanceGreaterAbility, 7, 13);

            if (TTTContext.AddedContent.Spells.IsDisabled("AgeResistance")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfAgeResistanceLesser);
            VenderTools.AddScrollToLeveledVenders(ScrollOfAgeResistance);
            VenderTools.AddScrollToLeveledVenders(ScrollOfAgeResistanceGreater);

            AgeResistanceLesserAbility.AddToSpellList(SpellTools.SpellList.AlchemistSpellList, 3);
            AgeResistanceLesserAbility.AddToSpellList(SpellTools.SpellList.DruidSpellList, 4);
            AgeResistanceLesserAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 4);
            AgeResistanceLesserAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 4);

            AgeResistanceAbility.AddToSpellList(SpellTools.SpellList.AlchemistSpellList, 4);
            AgeResistanceAbility.AddToSpellList(SpellTools.SpellList.DruidSpellList, 6);
            AgeResistanceAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 6);
            AgeResistanceAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 6);

            AgeResistanceGreaterAbility.AddToSpellList(SpellTools.SpellList.AlchemistSpellList, 5);
            AgeResistanceGreaterAbility.AddToSpellList(SpellTools.SpellList.DruidSpellList, 7);
            AgeResistanceGreaterAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 7);
            AgeResistanceGreaterAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 7);
        }
    }
}
