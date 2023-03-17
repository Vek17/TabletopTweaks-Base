using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class WebBolt {
        public static void AddWebBolt() {
            var Icon_WebBolt = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WebBolt.png");
            var Icon_ScrollOfWebBolt = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfWebBolt.png");
            var WebGrappled = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a719abac0ea0ce346b401060754cc1c0");
            var Web00_Projectile = BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("a5e1248fd4894a43b5b01845638bebfc");
            var WebBoltAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "WebBoltAbility", bp => {
                bp.SetName(TTTContext, "Web Bolt");
                bp.SetDescription(TTTContext, "You launch a ball of webbing at a target, which must make a save or be affected as if by a web spell occupying only the creature’s space.\n\n" +
                    "Web creates a many-layered mass of strong, sticky strands. These strands trap those caught in them. " +
                    "The strands are similar to spiderwebs but far larger and tougher. Creatures caught within a web become grappled by the sticky fibers.\n" +
                    "Anyone in the effect's area when the spell is cast must make a Reflex save. If this save succeeds, " +
                    "the creature is inside the web but is otherwise unaffected. " +
                    "If the save fails, the creature gains the grappled condition, but can break free by making a combat maneuver check, " +
                    "Athletics check, or Mobility check as a standard action against the DC of this spell. " +
                    "The entire area of the web is considered difficult terrain. " +
                    "Anyone moving through the webs must make a Reflex save each round. " +
                    "Creatures that fail lose their movement and become grappled in the first square of webbing that they enter.");
                bp.SetLocalizedDuration(TTTContext, "1 minute/level");
                bp.SetLocalizedSavingThrow(TTTContext, "Reflex negates");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Persistent | Metamagic.Reach;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.CanTargetEnemies = true;
                bp.SpellResistance = false;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_WebBolt;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Reflex;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionConditionalSaved() {
                            Succeed = Helpers.CreateActionList(),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = WebGrappled,
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Minutes,
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
                bp.AddComponent<AbilityDeliverProjectile>(c => {
                    c.m_Projectiles = new BlueprintProjectileReference[] { Web00_Projectile };
                    c.m_LineWidth = 5.Feet();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Conjuration;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Debuff;
                    c.SavingThrow = CraftSavingThrow.Reflex;
                    c.AOEType = CraftAOE.None;
                });
            });
            var ScrollOfWebBolt = ItemTools.CreateScroll(TTTContext, "ScrollOfWebBolt", Icon_ScrollOfWebBolt, WebBoltAbility, 1, 1);

            if (TTTContext.AddedContent.Spells.IsDisabled("WebBolt")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfWebBolt);

            WebBoltAbility.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 1);
            WebBoltAbility.AddToSpellList(SpellTools.SpellList.MagusSpellList, 1);
            WebBoltAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 1);
            WebBoltAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 1);
        }
    }
}
