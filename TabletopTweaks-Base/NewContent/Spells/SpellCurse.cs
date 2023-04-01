using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class SpellCurse {
        public static void AddSpellCurse() {
            var Icon_Spellcurse = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_Spellcurse.png");
            var Icon_ScrollOfSpellcurse = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfSpellcurse.png");
            var boneshaker00 = new PrefabLink() {
                AssetId = "b18f28e70b2b46f49acae7eeedd01c9d"
            };
            var SpellCurseProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "SpellCurseProperty", bp => {
                bp.AddComponent<BuffCountGetter>();
            });
            var SpellCurseAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "SpellCurseAbility", bp => {
                bp.SetName(TTTContext, "Spellcurse");
                bp.SetDescription(TTTContext, "You disrupt any spell energy affecting your target, " +
                    "causing that energy to crackle with power and harm the target. " +
                    "The target takes 1d6 points of damage for each spell with a duration of 1 round or greater currently affecting it. " +
                    "The spells themselves are not dispelled or modified.");
                bp.SetLocalizedDuration(TTTContext, "");
                bp.SetLocalizedSavingThrow(TTTContext, "Will half");
                bp.AvailableMetamagic = Metamagic.Extend
                    | Metamagic.Heighten
                    | Metamagic.Empower
                    | Metamagic.Maximize
                    | Metamagic.Bolstered
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Long;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_Spellcurse;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionDealDamage() {
                            DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Magic
                            },
                            Duration = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            },
                            Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = new ContextValue() {
                                    ValueType = ContextValueType.TargetCustomProperty,
                                    m_CustomProperty = SpellCurseProperty.ToReference<BlueprintUnitPropertyReference>(),
                                },
                                BonusValue = 0
                            },
                            HalfIfSaved = true
                        }
                    );
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = boneshaker00;
                    c.Anchor = AbilitySpawnFxAnchor.ClickedTarget;
                    c.PositionAnchor = AbilitySpawnFxAnchor.None;
                    c.OrientationAnchor = AbilitySpawnFxAnchor.None;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Necromancy;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Curse;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Damage;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.None;
                });
            });
            var ScrollOfSpellcurse = ItemTools.CreateScroll(TTTContext, "ScrollOfSpellcurse", Icon_ScrollOfSpellcurse, SpellCurseAbility, 3, 5);

            if (TTTContext.AddedContent.Spells.IsDisabled("SpellCurse")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfSpellcurse);

            SpellCurseAbility.AddToSpellList(SpellTools.SpellList.ClericSpellList, 3);
            SpellCurseAbility.AddToSpellList(SpellTools.SpellList.InquisitorSpellList, 2);
            SpellCurseAbility.AddToSpellList(SpellTools.SpellList.ShamanSpelllist, 4);
            SpellCurseAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 3);
        }
    }
}
