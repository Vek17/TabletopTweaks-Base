using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Craft;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    static class MagesDisjunction {
        public static void AddMagesDisjunction() {
            //var icon = AssetLoader.Image2Sprite.Create($"{Context.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_LongArm.png");
            var Icon_MagesDisjunctionAbility = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_MagesDisjunctionAbility.png");
            var Icon_MagesDisjunctionScroll = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_MagesDisjunctionScroll.png");
            var dispelmagic00fx = new PrefabLink() { 
                AssetId = "3eda0e7f710821045a35ebe432af667c"
            };
            var AdamantineWeaponEnchantment = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("ab39e7d59dd12f4429ffef5dca88dc7b");
            var AdamantineArmorLightEnchant = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("5faa3aaee432ac444b101de2b7b0faf7");
            var AdamantineArmorMediumEnchant = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("aa25531ab5bb58941945662aa47b73e7");
            var AdamantineArmorHeavyEnchant = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("933456ff83c454146a8bf434e39b1f93");
            var ColdIronWeaponEnchantment = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("e5990dc76d2a613409916071c898eee8");
            var MithralWeaponEnchantment = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("0ae8fc9f2e255584faf4d14835224875");
            var MithralArmorEnchant = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("7b95a819181574a4799d93939aa99aff");
            var Masterwork = BlueprintTools.GetBlueprintReference<BlueprintEquipmentEnchantmentReference>("6b38844e2bffbac48b63036b66e735be");

            var SummonedCreatureVisual = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("706c182e86d9be848b59ddccca73d13e");

            var MagesDisjunctionEnchantment = Helpers.CreateBlueprint<BlueprintEquipmentEnchantment>(TTTContext, $"MagesDisjunctionEnchantment", bp => {
                bp.SetName(TTTContext, "Disjoined");
                bp.SetDescription(TTTContext, "Magical effects of this item are suppressed.");
                bp.SetPrefix(TTTContext, "Disjoined");
                bp.SetSuffix(TTTContext, "");
                bp.AddComponent<DisjointedEffect>(c => {
                    c.m_IgnoreEnchantments = new BlueprintEquipmentEnchantmentReference[] {
                        AdamantineWeaponEnchantment,
                        AdamantineArmorLightEnchant,
                        AdamantineArmorMediumEnchant,
                        AdamantineArmorHeavyEnchant,
                        ColdIronWeaponEnchantment,
                        MithralWeaponEnchantment,
                        MithralArmorEnchant,
                        Masterwork
                    };
                });
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
            });
            var MagesDisjunctionAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "MagesDisjunctionAbility", bp => {
                bp.SetName(TTTContext, "Mage's Disjunction");
                bp.SetDescription(TTTContext, "All magical effects and magic items within the radius of the spell, except for those that you carry or touch, are disjoined. " +
                    "That is, spells and spell-like effects are unraveled and destroyed completely (ending the effect as a dispel magic spell does), " +
                    "and each permanent magic item must make a successful Will save or be turned into a normal item for the duration of this spell. " +
                    "An item in a creature’s possession uses its possessor’s Will save bonus.");
                bp.SetLocalizedDuration(TTTContext, "1 minute/level");
                bp.SetLocalizedSavingThrow(TTTContext, "Will partial");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Reach | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Persistent;
                bp.Range = AbilityRange.Medium;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetPoint = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_MagesDisjunctionAbility;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() { 
                            ConditionsChecker = new ConditionsChecker() { 
                                Conditions = new Condition[] { 
                                    new ContextConditionHasFact(){ 
                                        m_Fact = SummonedCreatureVisual
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionKill()
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionDispelMagic() {
                                    m_StopAfterCountRemoved = false,
                                    m_CountToRemove = new ContextValue(),
                                    m_BuffType = ContextActionDispelMagic.BuffType.FromSpells,
                                    m_MaxSpellLevel = new ContextValue(),
                                    m_MaxCasterLevel = new ContextValue(),
                                    m_CheckType = Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.None,
                                    ContextBonus = new ContextValue(),
                                    Schools = new SpellSchool[0],
                                    OnSuccess = Helpers.CreateActionList(),
                                    OnFail = Helpers.CreateActionList()
                                },
                                new ContextActionDisjointEnchantments() {
                                    m_DisjointEnchantment = MagesDisjunctionEnchantment.ToReference<BlueprintEquipmentEnchantmentReference>(),
                                    Duration = new ContextDurationValue() {
                                        Rate = DurationRate.Minutes,
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        }
                                    }
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = Kingmaker.UnitLogic.Mechanics.Components.ContextRankBaseValueType.CasterLevel;
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = dispelmagic00fx;
                    c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
                    c.PositionAnchor = AbilitySpawnFxAnchor.None;
                    c.OrientationAnchor = AbilitySpawnFxAnchor.None;
                });
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_Radius = 40.Feet();
                    c.m_TargetType = TargetType.Any;
                    c.m_Condition = new ConditionsChecker() {
                        Conditions = new Condition[] { 
                            new ContextConditionIsCaster(){ 
                                Not = true
                            }
                        }
                    };
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Abjuration;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Other;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.AOE;
                });
            });
            var MagesDisjunctionScroll = ItemTools.CreateScroll(TTTContext, "ScrollOfMagesDisjunction", Icon_MagesDisjunctionScroll, MagesDisjunctionAbility, 9, 17);
            if (TTTContext.AddedContent.Spells.IsDisabled("MagesDisjunction")) { return; }
            MagesDisjunctionAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 9);
            SpellTools.SpellList.MagicDomainSpellList.SpellsByLevel
                .Where(level => level.SpellLevel == 9)
                .ForEach(level => level.Spells.Clear());
            MagesDisjunctionAbility.AddToSpellList(SpellTools.SpellList.MagicDomainSpellList, 9);
        }
    }
}
