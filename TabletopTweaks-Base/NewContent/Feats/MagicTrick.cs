using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class MagicTrick {
        public static void AddMagicTrick() {
            var Fireball = BlueprintTools.GetBlueprint<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
            var Fireball00 = BlueprintTools.GetBlueprint<BlueprintProjectile>("8927afa172e0fc54484a29fa0c4c40c4");
            var MoltenOrb00 = BlueprintTools.GetBlueprint<BlueprintProjectile>("49c812020338e90479b54cfc5b1f6305");
            var AlchemistExplosiveBomb00 = BlueprintTools.GetBlueprint<BlueprintProjectile>("78fa47590c24f0b47acdec286e171e81");
            var Icon_FireballClusterBomb = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_FireballClusterBomb.png");

            var FireballCluster10ft = MoltenOrb00.CreateCopy(TTTContext, "FireballCluster10ft", bp => {
                bp.ProjectileHit = new Kingmaker.Visual.HitSystem.ProjectileHitSettings();
                bp.ProjectileHit.TemporaryContext(c => {
                    c.HitFx = AlchemistExplosiveBomb00.ProjectileHit.HitFx;
                    c.HitSnapFx = new PrefabLink();
                    c.MissFx = AlchemistExplosiveBomb00.ProjectileHit.HitFx;
                    c.MissDecalFx = new PrefabLink();
                });
                bp.FallsOnMiss = true;
                bp.MissMinRadius = 0.3f;
                bp.MissMaxRadius = 1.3f;
            });
            var FireballCluster8ft = MoltenOrb00.CreateCopy(TTTContext, "FireballCluster8ft", bp => {
                bp.ProjectileHit = new Kingmaker.Visual.HitSystem.ProjectileHitSettings();
                bp.ProjectileHit.TemporaryContext(c => {
                    c.HitFx = MoltenOrb00.ProjectileHit.HitFx;
                    c.HitSnapFx = new PrefabLink();
                    c.MissFx = MoltenOrb00.ProjectileHit.HitFx;
                    c.MissDecalFx = new PrefabLink();
                });
                bp.FallsOnMiss = true;
                bp.MissMinRadius = 0.3f;
                bp.MissMaxRadius = 1.3f;
            });
            var FireballClusterBomb = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "FireballClusterBomb", bp => {
                bp.SetName(TTTContext, "Fireball — Cluster Bomb");
                bp.SetDescription(TTTContext, "You are able to throw multiple small explosions with a single spell instead of the normal effect. " +
                    "For every 2 caster levels, you toss a miniature fireball with a 10-foot radius that deals 2d6 points of fire damage. " +
                    "A creature attempts a single Reflex save against the combined damage.");
                bp.SetLocalizedDuration(TTTContext, "");
                bp.SetLocalizedSavingThrow(TTTContext, "Reflex half");
                bp.AvailableMetamagic = Fireball.AvailableMetamagic
                    | (Metamagic)CustomMetamagic.ElementalFire
                    | (Metamagic)CustomMetamagic.ElementalAcid
                    | (Metamagic)CustomMetamagic.ElementalElectricity
                    | (Metamagic)CustomMetamagic.ElementalCold
                    | (Metamagic)CustomMetamagic.Burning
                    | (Metamagic)CustomMetamagic.Flaring
                    | (Metamagic)CustomMetamagic.Intensified
                    | (Metamagic)CustomMetamagic.Piercing;
                bp.m_Parent = Fireball.ToReference<BlueprintAbilityReference>();
                bp.Range = AbilityRange.Medium;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.SpellResistance = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetPoint = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_FireballClusterBomb;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Reflex;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionDealDamage() {
                            DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Fire
                            },
                            Duration = new ContextDurationValue() {
                                DiceCountValue = 0,
                                BonusValue = 0
                            },
                            Value = new ContextDiceValue() {
                                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                DiceCountValue = new ContextValue() {
                                    ValueType = ContextValueType.Shared,
                                    ValueShared = AbilitySharedValue.Damage
                                },
                                BonusValue = 0
                            },
                            HalfIfSaved = true
                        }
                    );
                });
                bp.AddComponent<AbilityDeliverCluster>(c => {
                    c.m_Projectiles = new BlueprintProjectileReference[] {
                        FireballCluster10ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                        FireballCluster8ft.ToReference<BlueprintProjectileReference>(),
                    };
                    c.DelayBetweenProjectiles = 0.03f;
                    c.UseMaxProjectilesCount = true;
                    c.MaxProjectilesCountRank = AbilityRankType.ProjectilesCount;
                });
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_TargetType = TargetType.Any;
                    c.m_Condition = new ConditionsChecker() {
                        Conditions = new Condition[0]
                    };
                    c.m_Radius = 10.Feet();
                    c.m_SpreadSpeed = 25.Feet();
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.ProjectilesCount;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.DivStep;
                    c.m_StepLevel = 2;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.DivStep;
                    c.m_StepLevel = 2;
                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.Damage;
                    c.Value = new ContextDiceValue() {
                        DiceCountValue = 0,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageDice
                        }
                    };
                    c.Modifier = 2;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Evocation;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Fire;
                });
            });
            var MagicTrickFireball = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MagicTrickFireball", bp => {
                bp.SetName(TTTContext, "Magic Trick (Fireball)");
                bp.SetDescription(TTTContext, "Cluster Bomb: You are able to throw multiple small explosions with a single spell instead of the normal effect. " +
                    "For every 2 caster levels, you toss a miniature fireball with a 10-foot radius that deals 2d6 points of fire damage. " +
                    "A creature attempts a single Reflex save against the combined damage.");
                //bp.m_Icon = DispelMagicGreater.Icon;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<AddSpecificSpellConversion>(c => {
                    c.m_targetSpell = Fireball.ToReference<BlueprintAbilityReference>();
                    c.m_convertSpell = FireballClusterBomb.ToReference<BlueprintAbilityReference>();
                });
                bp.AddPrerequisite<PrerequisiteSpellKnown>(c => {
                    c.m_Spell = Fireball.ToReference<BlueprintAbilityReference>();
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillKnowledgeArcana;
                    c.Value = 6;
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("MagicTrickFireball")) { return; }
            FeatTools.AddAsFeat(MagicTrickFireball);
        }
    }
}
