using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class AccursedGlare {
        public static void AddAccursedGlare() {
            var Icon_AccursedGlare = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_AccursedGlare.png");
            var Icon_ScrollOfAccursedGlare = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfAccursedGlare.png");
            var witch_evileye00 = new PrefabLink() {
                AssetId = "0843a2479096a2f48baccb8a6408f111"
            };

            var AccursedGlareBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "AccursedGlareBuff", bp => {
                bp.SetName(TTTContext, "Accursed Glare");
                bp.SetDescription(TTTContext, "You channel a fell curse through your glare. If the target fails its saving throw, " +
                    "it begins to obsessively second guess its actions and attract bad luck. " +
                    "Whenever the target attempts an attack roll or saving throw while the curse lasts, " +
                    "it must roll twice and take the lower result.");
                bp.m_Icon = Icon_AccursedGlare;
                bp.FxOnStart = witch_evileye00;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<ModifyD20>(c => {
                    c.RollsAmount = 1;
                    c.m_SavingThrowType = FlaggedSavingThrowType.All;
                    c.Rule = RuleType.AttackRoll | RuleType.SavingThrow;
                    c.RollResult = new ContextValue();
                    c.Bonus = new ContextValue();
                    c.Chance = new ContextValue();
                    c.ValueToCompareRoll = new ContextValue();
                    c.Skill = new StatType[0];
                    c.Value = new ContextValue();
                });
            });
            var AccursedGlareAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "AccursedGlareAbility", bp => {
                bp.SetName(TTTContext, "Accursed Glare");
                bp.SetDescription(TTTContext, "You channel a fell curse through your glare. If the target fails its saving throw, " +
                    "it begins to obsessively second guess its actions and attract bad luck. " +
                    "Whenever the target attempts an attack roll or saving throw while the curse lasts, " +
                    "it must roll twice and take the lower result.");
                bp.SetLocalizedDuration(TTTContext, "1 day/level");
                bp.SetLocalizedSavingThrow(TTTContext, "Will negates");
                bp.AvailableMetamagic = Metamagic.Extend
                    | Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = false;
                bp.CanTargetSelf = false;
                bp.CanTargetEnemies = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_AccursedGlare;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionConditionalSaved() {
                            Succeed = Helpers.CreateActionList(),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = AccursedGlareBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Days,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        },
                                        DiceCountValue = 0
                                    }
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Necromancy;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Curse;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Debuff;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.None;
                });
            });
            var ScrollOfAccursedGlare = ItemTools.CreateScroll(TTTContext, "ScrollOfAccursedGlare", Icon_ScrollOfAccursedGlare, AccursedGlareAbility, 3, 5);

            if (TTTContext.AddedContent.Spells.IsDisabled("AccursedGlare")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfAccursedGlare);

            AccursedGlareAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 3);
            AccursedGlareAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 3);
        }
    }
}
