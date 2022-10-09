using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class CloakOfWinds {
        public static void AddCloakOfWinds() {
            //var icon = AssetLoader.Image2Sprite.Create($"{Context.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_LongArm.png");
            var Icon_CloakOfWinds = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_CloakOfWinds.png");
            var Icon_ScrollOfCloakOfWinds = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfCloakOfWinds.png");
            var CloakOfWindsBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "CloakOfWindsBuff", bp => {
                bp.SetName(TTTContext, "Cloak Of Winds");
                bp.SetDescription(TTTContext, "You shroud a creature in a whirling screen of strong, howling wind. Ranged attack rolls against the subject take a -4 penalty.");
                bp.m_Icon = Icon_CloakOfWinds;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<AttackBonusAgainstTargetTTT>(c => {
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                    c.CheckRangeType = true;
                    c.RangeType = WeaponRangeType.Ranged;
                });
            });
            var CloakOfWindsAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "CloakOfWindsAbility", bp => {
                bp.SetName(TTTContext, "Cloak Of Winds");
                bp.SetDescription(TTTContext, "You shroud a creature in a whirling screen of strong, howling wind. Ranged attack rolls against the subject take a -4 penalty.");
                bp.SetLocalizedDuration(TTTContext, "1 minute/level");
                bp.SetLocalizedSavingThrow(TTTContext, "");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_CloakOfWinds;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            IsFromSpell = true,
                            m_Buff = CloakOfWindsBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                },
                                DiceCountValue = 0
                            }
                        }
                    );
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Abjuration;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Buff;
                    c.SavingThrow = CraftSavingThrow.None;
                    c.AOEType = CraftAOE.None;
                });
            });
            var ScrollOfCloakOfWinds = ItemTools.CreateScroll(TTTContext, "ScrollOfCloakOfWinds", Icon_ScrollOfCloakOfWinds, CloakOfWindsAbility, 3, 5);
            var PotionOfCloakOfWinds = ItemTools.CreatePotion(TTTContext, "PotionOfCloakOfWinds", ItemTools.PotionColor.Blue, CloakOfWindsAbility, 3, 5);

            if (TTTContext.AddedContent.Spells.IsDisabled("LongArm")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfCloakOfWinds);
            VenderTools.AddPotionToLeveledVenders(PotionOfCloakOfWinds);

            CloakOfWindsAbility.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 3);
            CloakOfWindsAbility.AddToSpellList(SpellTools.SpellList.DruidSpellList, 3);
            CloakOfWindsAbility.AddToSpellList(SpellTools.SpellList.MagusSpellList, 3);
            CloakOfWindsAbility.AddToSpellList(SpellTools.SpellList.RangerSpellList, 3);
            CloakOfWindsAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 3);
        }
    }
}
