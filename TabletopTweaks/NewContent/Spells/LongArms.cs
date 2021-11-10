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
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Spells {
    static class LongArms {
        public static void AddLongArms() {
            //var icon = AssetLoader.Image2Sprite.Create($"{ModSettings.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_LongArm.png");
            var icon = AssetLoader.LoadInternal("Abilities", "Icon_LongArm.png");
            var LongArmBuff = Helpers.CreateBuff("LongArmBuff", bp => {
                bp.SetName("Long Arm");
                bp.SetDescription("Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.");
                bp.m_Icon = icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Stat = StatType.Reach;
                    c.Descriptor = ModifierDescriptor.Enhancement;
                    c.Value = 5;
                }));
            });
            var applyBuff = Helpers.Create<Kingmaker.UnitLogic.Mechanics.Actions.ContextActionApplyBuff>(bp => {
                bp.IsFromSpell = true;
                bp.m_Buff = LongArmBuff.ToReference<BlueprintBuffReference>();
                bp.DurationValue = new ContextDurationValue() {
                    Rate = DurationRate.Minutes,
                    BonusValue = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    },
                    DiceCountValue = new ContextValue(),
                    DiceType = DiceType.One
                };
            });
            var LongArmAbility = Helpers.CreateBlueprint<BlueprintAbility>("LongArmAbility", bp => {
                bp.SetName("Long Arm");
                bp.SetDescription("Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.");
                bp.LocalizedDuration = Helpers.CreateString("LongArmAbility.Duration", "1 minute/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken;
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = icon;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList {
                        Actions = new GameAction[] { applyBuff }
                    };
                }));
                bp.AddComponent(Helpers.Create<SpellComponent>(c => {
                    c.School = SpellSchool.Transmutation;
                }));
                bp.AddComponent(Helpers.Create<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Buff;
                    c.SavingThrow = CraftSavingThrow.None;
                    c.AOEType = CraftAOE.None;
                }));
            });
            if (ModSettings.AddedContent.Spells.IsDisabled("LongArm")) { return; }
            LongArmAbility.AddToSpellList(SpellTools.SpellList.AlchemistSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.MagusSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 1);
        }
    }
}
