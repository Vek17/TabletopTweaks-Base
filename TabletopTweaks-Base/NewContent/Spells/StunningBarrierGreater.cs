using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Craft;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class StunningBarrierGreater {
        public static void AddStunningBarrierGreater() {
            //var icon = AssetLoader.Image2Sprite.Create($"{Context.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_LongArm.png");
            var Stunned = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("09d39b38bb7c6014394b6daced9bacd3");
            var StunningBarrierStun = BlueprintTools.GetBlueprint<BlueprintAbility>("a08c6ca24141e9e4ea7a679dfecf8007");
            var ScrollOfStunningBarrier = BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("e029ec259c9a37249b113060df32a01d");
            var Icon_StunningBarrierGreater = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_StunningBarrierGreater.png");
            var Icon_ScrollOfStunningBarrierGreater = ScrollOfStunningBarrier.Icon;

            var StunningBarrierName = Helpers.CreateString(TTTContext, "StunningBarrierGreater.Name", 
                "Greater Stunning Barrier", 
                shouldProcess:false
            );
            var StunningBarrierDescription = Helpers.CreateString(TTTContext, "StunningBarrierGreater.Description", 
                "You are closely surrounded by a barely visible magical field. " +
                "The field provides a +2 deflection bonus to AC and a +2 resistance bonus on saves. " +
                "Any creature that strikes you with a melee attack is stunned for 1 round (Will negates). " +
                "Once the field has stunned a number of creatures equal to your caster level, the spell is discharged.", 
                shouldProcess: true
            );
            var StunningBarrierLocalizedDuration = Helpers.CreateString(TTTContext, "StunningBarrierGreater.Name", 
                "1 round/level or until discharged", 
                shouldProcess: false
            );
            var StunningBarrierLocalizedSavingThrow = Helpers.CreateString(TTTContext, "StunningBarrierGreater.Name", 
                "Will negates stun", 
                shouldProcess: false
            );
            //Stub for reference creation
            var StunningBarrierGreaterBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "StunningBarrierGreaterBuff");
            var StunningBarrierGreaterStun = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "StunningBarrierGreaterStun", bp => {
                bp.SetName(StunningBarrierName);
                bp.SetDescription(StunningBarrierDescription);
                bp.SetLocalizedDuration(StunningBarrierLocalizedDuration);
                bp.SetLocalizedSavingThrow(StunningBarrierLocalizedSavingThrow);
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.Persistent | Metamagic.CompletelyNormal;
                bp.Type = AbilityType.SpellLike;
                bp.Range = AbilityRange.Long;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.SpellResistance = true;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_StunningBarrierGreater;
                bp.ResourceAssetIds = StunningBarrierStun.ResourceAssetIds;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionConditionalSaved() { 
                            Succeed = Helpers.CreateActionList(),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() { 
                                    m_Buff = Stunned,
                                    DurationValue = new ContextDurationValue() { 
                                        DiceCountValue = 0,
                                        BonusValue = 1
                                    }
                                },
                                new ContextActionOnContextCaster() {
                                    Actions = Helpers.CreateActionList(
                                          new ContextActionRemoveBuff() {
                                              m_Buff = StunningBarrierGreaterBuff.ToReference<BlueprintBuffReference>(),
                                              RemoveRank = true
                                          }
                                    )
                                }
                            )
                        }
                    );
                });
                var effects = StunningBarrierStun.GetComponent<AbilitySpawnFx>();
                if (effects is not null) {
                    bp.AddComponent<AbilitySpawnFx>(c => {
                        c.PrefabLink = effects.PrefabLink;
                        c.Anchor = effects.Anchor;
                        c.PositionAnchor = effects.PositionAnchor;
                        c.OrientationAnchor = effects.OrientationAnchor;
                    });
                }
                bp.AddComponent<BlockSpellDuplicationComponent>();
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Abjuration;
                });
            });
            StunningBarrierGreaterBuff.TemporaryContext(bp => {
                bp.SetName(StunningBarrierName);
                bp.SetDescription(StunningBarrierDescription);
                bp.m_Icon = Icon_StunningBarrierGreater;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.Ranks = 100;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = ModifierDescriptor.Deflection;
                    c.Value = 2;
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SaveFortitude;
                    c.Descriptor = ModifierDescriptor.Resistance;
                    c.Value = 2;
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SaveReflex;
                    c.Descriptor = ModifierDescriptor.Resistance;
                    c.Value = 2;
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SaveWill;
                    c.Descriptor = ModifierDescriptor.Resistance;
                    c.Value = 2;
                });
                bp.AddComponent<AddTargetAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.OnlyMelee = true;
                    c.ActionOnSelf = Helpers.CreateActionList();
                    c.ActionsOnAttacker = Helpers.CreateActionList(
                        new ContextActionCastSpell() { 
                            m_Spell = StunningBarrierGreaterStun.ToReference<BlueprintAbilityReference>(),
                            DC = new ContextValue(),
                            SpellLevel = new ContextValue()
                        }
                    );
                });
                bp.AddComponent<ReplaceAbilityParamsWithContext>(c => {
                    c.m_Ability = StunningBarrierGreaterStun.ToReference<BlueprintAbilityReference>();
                });
            });
            var StunningBarrierGreaterAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "StunningBarrierGreaterAbility", bp => {
                bp.SetName(StunningBarrierName);
                bp.SetDescription(StunningBarrierDescription);
                bp.SetLocalizedDuration(StunningBarrierLocalizedDuration);
                bp.SetLocalizedSavingThrow(StunningBarrierLocalizedSavingThrow);
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.Persistent | Metamagic.CompletelyNormal;
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_StunningBarrierGreater;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuffRanks>(a => {
                            a.IsFromSpell = true;
                            a.m_Buff = StunningBarrierGreaterBuff.ToReference<BlueprintBuffReference>();
                            a.Rank = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            };
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                },
                                DiceCountValue = 0
                            };
                        })
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Abjuration;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Buff;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.None;
                });
            });
            var MagesDisjunctionScroll = ItemTools.CreateScroll(TTTContext, "ScrollOfStunningBarrierGreater", Icon_ScrollOfStunningBarrierGreater, StunningBarrierGreaterAbility, 3, 5);

            if (TTTContext.AddedContent.Spells.IsDisabled("StunningBarrierGreater")) { return; }

            StunningBarrierGreaterAbility.AddToSpellList(SpellTools.SpellList.ClericSpellList, 3);
            StunningBarrierGreaterAbility.AddToSpellList(SpellTools.SpellList.InquisitorSpellList, 3);
            StunningBarrierGreaterAbility.AddToSpellList(SpellTools.SpellList.PaladinSpellList, 3);
            StunningBarrierGreaterAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 3);
        }
    }
}
