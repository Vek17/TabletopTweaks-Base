using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    static class LongArms {
        public static void AddLongArms() {
            //var icon = AssetLoader.Image2Sprite.Create($"{Context.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_LongArm.png");
            var icon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_LongArm.png");
            var LongArmBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "LongArmBuff", bp => {
                bp.SetName(TTTContext, "Long Arm");
                bp.SetDescription(TTTContext, "Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.");
                bp.m_Icon = icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Stat = StatType.Reach;
                    c.Descriptor = ModifierDescriptor.Enhancement;
                    c.Value = 5;
                }));
            });
            var LongArmAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "LongArmAbility", bp => {
                bp.SetName(TTTContext, "Long Arm");
                bp.SetDescription(TTTContext, "Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.");
                bp.SetLocalizedDuration(TTTContext, "1 minute/level");
                bp.SetLocalizedSavingThrow(TTTContext, "");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal;
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = icon;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            m_Buff = LongArmBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                },
                                DiceCountValue = new ContextValue(),
                                DiceType = DiceType.One
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

            if (TTTContext.AddedContent.Spells.IsDisabled("LongArm")) { return; }

            LongArmAbility.AddToSpellList(SpellTools.SpellList.AlchemistSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.MagusSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 1);
        }
    }
}
