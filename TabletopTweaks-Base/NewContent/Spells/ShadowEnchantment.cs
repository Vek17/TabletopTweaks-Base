using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    class ShadowEnchantment {
        public static void AddShadowEnchantment() {
            //var icon = AssetLoader.Image2Sprite.Create($"{Context.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_ShadowEnchantment.png");
            var icon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_ShadowEnchantment.png");
            var PowerfulShadows = BlueprintTools.GetBlueprint<BlueprintFeature>("6a9448ec047c642408af6debb8536c38");
            var ShadowEnchantmentProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "ShadowEnchantmentProperty", bp => {
                bp.BaseValue = 20;
                bp.AddComponent<FactRankGetter>(c => {
                    c.m_Fact = PowerfulShadows.ToReference<BlueprintUnitFactReference>();
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.MultiplyByModifier,
                        m_StepLevel = 20,
                        m_CustomProgression = new PropertySettings.CustomProgressionItem[0]
                    };
                });
            });
            var ShadowEnchantment = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "ShadowEnchantment", bp => {
                bp.SetName(TTTContext, "Shadow Enchantment");
                bp.SetDescription(TTTContext, "You use material from the Shadow Plane to cast a quasi-real, illusory version of a psychic, sorcerer, or wizard enchantment spell of 2nd level "
                    + "or lower. Spells that deal damage or have other effects work as normal unless the affected creature succeeds at a Will save. If the disbelieved enchantment "
                    + "spell has a damaging effect, that effect is one-fifth as strong (if applicable) or only 20% likely to occur.\n"
                    + "If recognized as a shadow enchantment, a damaging spell deals only one - fifth(20 %) the normal amount of damage.\n"
                    + "If the disbelieved attack has a special effect other than damage, that effect is one-fifth as strong (if applicable) or only 20% likely to occur. "
                    + "Regardless of the result of the save to disbelieve, an affected creature is also allowed any save (or spell resistance) that the spell being simulated allows, "
                    + "but the save DC is set according to shadow enchantment’s level (3rd) rather than the spell’s normal level. Objects, mindless creatures, and creatures immune "
                    + "to mind-affecting effects automatically succeed at their Will saves against this spell.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AvailableMetamagic = Metamagic.Empower
                    | Metamagic.Quicken
                    | Metamagic.Heighten
                    | Metamagic.Reach
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent;
                bp.Range = AbilityRange.Close;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetPoint = true;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = icon;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Illusion;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    //No Descriptor?
                });
                bp.AddComponent<AbilityShadowSpell>(c => {
                    c.School = SpellSchool.Enchantment;
                    c.m_Factor = ShadowEnchantmentProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.MaxSpellLevel = 2;
                    c.SpellList = SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>();
                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.One,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        },
                        BonusValue = new ContextValue() {
                            Value = 20
                        }
                    };
                    c.Modifier = 1;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Feature = PowerfulShadows.ToReference<BlueprintFeatureReference>();
                    c.m_FeatureList = new BlueprintFeatureReference[0];
                    c.m_Progression = ContextRankProgression.MultiplyByModifier;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[0];
                    c.m_StepLevel = 20;
                    c.m_Max = 20;
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[0];
                    c.m_Class = new BlueprintCharacterClassReference[0];
                    c.m_CustomPropertyList = new BlueprintUnitPropertyReference[0];
                });
            });
            if (TTTContext.AddedContent.Spells.IsDisabled("ShadowEnchantment")) { return; }
            ShadowEnchantment.AddToSpellList(SpellTools.SpellList.BardSpellList, 3);
            ShadowEnchantment.AddToSpellList(SpellTools.SpellList.TricksterSpelllist, 3);
            ShadowEnchantment.AddToSpellList(SpellTools.SpellList.WizardSpellList, 3);
        }
        public static void AddShadowEnchantmentGreater() {
            //var icon = AssetLoader.Image2Sprite.Create($"{Context.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_ShadowEnchantmentGreater.png");
            var icon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_ShadowEnchantmentGreater.png");
            var PowerfulShadows = BlueprintTools.GetBlueprint<BlueprintFeature>("6a9448ec047c642408af6debb8536c38");
            var ShadowEnchantmentGreaterProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "ShadowEnchantmentGreaterProperty", bp => {
                bp.BaseValue = 60;
                bp.AddComponent<FactRankGetter>(c => {
                    c.m_Fact = PowerfulShadows.ToReference<BlueprintUnitFactReference>();
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.MultiplyByModifier,
                        m_StepLevel = 20,
                        m_CustomProgression = new PropertySettings.CustomProgressionItem[0]
                    };
                });
            });
            var ShadowEnchantmentGreater = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "ShadowEnchantmentGreater", bp => {
                bp.SetName(TTTContext, "Shadow Enchantment, Greater");
                bp.SetDescription(TTTContext, "This spell functions like shadow enchantment, except that it enables you to create partially real, illusory versions of psychic, sorcerer, "
                    + "or wizard enchantment spells of 5th level or lower. If the spell is recognized as a greater shadow enchantment, it’s only three-fifths (60%) as effective.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AvailableMetamagic = Metamagic.Empower
                    | Metamagic.Quicken
                    | Metamagic.Heighten
                    | Metamagic.Reach
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent;
                bp.Range = AbilityRange.Close;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetPoint = true;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = icon;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Illusion;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    //No Descriptor?
                });
                bp.AddComponent<AbilityShadowSpell>(c => {
                    c.School = SpellSchool.Enchantment;
                    c.m_Factor = ShadowEnchantmentGreaterProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.MaxSpellLevel = 6;
                    c.SpellList = SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>();
                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.One,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        },
                        BonusValue = new ContextValue() {
                            Value = 20
                        }
                    };
                    c.Modifier = 1;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Feature = PowerfulShadows.ToReference<BlueprintFeatureReference>();
                    c.m_FeatureList = new BlueprintFeatureReference[0];
                    c.m_Progression = ContextRankProgression.MultiplyByModifier;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[0];
                    c.m_StepLevel = 20;
                    c.m_Max = 20;
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[0];
                    c.m_Class = new BlueprintCharacterClassReference[0];
                    c.m_CustomPropertyList = new BlueprintUnitPropertyReference[0];
                });
            });
            if (TTTContext.AddedContent.Spells.IsDisabled("ShadowEnchantmentGreater")) { return; }
            ShadowEnchantmentGreater.AddToSpellList(SpellTools.SpellList.BardSpellList, 6);
            ShadowEnchantmentGreater.AddToSpellList(SpellTools.SpellList.TricksterSpelllist, 6);
            ShadowEnchantmentGreater.AddToSpellList(SpellTools.SpellList.WizardSpellList, 6);
        }
    }
}
