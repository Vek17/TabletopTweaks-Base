using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System;
using System.Linq;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class ArcaneMetamastery {
        public static void AddArcaneMetamastery() {
            var Icon_ArcaneMetamastery = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_ArcaneMetamastery.png");

            var EmpowerSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
            var MaximizeSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
            var QuickenSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
            var ExtendSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
            var ReachSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78");
            var PersistentSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
            var SelectiveSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
            var BolsteredSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");

            var IntensifiedSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "IntensifiedSpellFeat");
            var RimeSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "RimeSpellFeat");
            var BurningSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "BurningSpellFeat");
            var FlaringSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "FlaringSpellFeat");
            var PiercingSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "PiercingSpellFeat");
            var SolidShadowsSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "SolidShadowsSpellFeat");
            var EncouragingSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "EncouragingSpellFeat");
            var ElementalSpellFeatAcid = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatAcid");
            var ElementalSpellFeatCold = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatCold");
            var ElementalSpellFeatElectricity = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatElectricity");
            var ElementalSpellFeatFire = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatFire");

            var ArcaneMetamasteryResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "ArcaneMetamasteryResource", bp => {
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount() {
                    m_Class = ClassTools.ClassReferences.AllMythicClasses,
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    BaseValue = 1,
                    IncreasedByLevel = true,
                    LevelIncrease = 1
                };
            });
            var ArcaneMetamasteryBaseAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "ArcaneMetamasteryBaseAbility", bp => {
                bp.SetName(TTTContext, "Arcane Metamastery");
                bp.SetDescription(TTTContext, "As a swift action, " +
                    "you can pick any one metamagic feat you know that increases the slot level of the spell by up to 2 levels. " +
                    "For the next 10 rounds, you can apply this metamagic feat to any arcane spell you cast without " +
                    "increasing the spell slot used or casting time. You can’t have more than one use of this ability active at a time. " +
                    "If you use this ability again, any previous use immediately ends.\n" +
                    "You can use this ability a number of times per day equal to 1 plus your mythic rank.");
                bp.SetLocalizedDuration(TTTContext, "10 rounds");
                bp.m_Icon = Icon_ArcaneMetamastery;
                bp.LocalizedSavingThrow = new LocalizedString();
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.ActionType = CommandType.Swift;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = ArcaneMetamasteryResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
            });
            var ArcaneMetamastery = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArcaneMetamastery", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = Icon_ArcaneMetamastery;
                bp.SetName(TTTContext, "Arcane Metamastery");
                bp.SetDescription(TTTContext, "As a swift action, " +
                    "you can pick any one metamagic feat you know that increases the slot level of the spell by up to 2 levels. " +
                    "For the next 10 rounds, you can apply this metamagic feat to any arcane spell you cast without " +
                    "increasing the spell slot used or casting time. You can’t have more than one use of this ability active at a time. " +
                    "If you use this ability again, any previous use immediately ends.\n" +
                    "You can use this ability a number of times per day equal to 1 plus your mythic rank.");
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = ArcaneMetamasteryResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        ArcaneMetamasteryBaseAbility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            var ArcaneMetamasteryGreater = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArcaneMetamasteryGreater", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = Icon_ArcaneMetamastery;
                bp.SetName(TTTContext, "Greater Arcane Metamastery");
                bp.SetDescription(TTTContext, "You can apply a metamagic feat with Arcane Metamastery that increases the slot level of the spell by up to 4 levels instead of 2.");
                bp.AddPrerequisiteFeature(ArcaneMetamastery);
            });
            ArcaneMetamasteryBaseAbility.TemporaryContext(bp => {
                bp.AddComponent<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        CreateArcaneMetamasteryAbility("Empower", EmpowerSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Maximize", MaximizeSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Quicken", QuickenSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Extend", ExtendSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Reach", ReachSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Persistent", PersistentSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Selective", SelectiveSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Bolstered", BolsteredSpellFeat).ToReference<BlueprintAbilityReference>(),

                        CreateArcaneMetamasteryAbility("Intensified", IntensifiedSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Rime", RimeSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Burning", BurningSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Flaring", FlaringSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Piercing", PiercingSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Solid Shadows", SolidShadowsSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Encouraging", EncouragingSpellFeat).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Elemental Spell (Acid)", ElementalSpellFeatAcid).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Elemental Spell (Cold)", ElementalSpellFeatCold).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Elemental Spell (Electricity)", ElementalSpellFeatElectricity).ToReference<BlueprintAbilityReference>(),
                        CreateArcaneMetamasteryAbility("Elemental Spell (Fire)", ElementalSpellFeatFire).ToReference<BlueprintAbilityReference>(),
                    };
                });
                var allBuffs = bp.AbilityVariants.Variants
                    .SelectMany(variants => variants.FlattenAllActions())
                    .OfType<ContextActionApplyBuff>()
                    .Select(action => action.Buff)
                    .OfType<BlueprintBuff>()
                    .Where(buff => buff != null)
                    .ToArray();
                allBuffs.ForEach(buff => {
                    AddBuffRemoval(buff, allBuffs);
                });
            });
            BlueprintBuff CreateArcaneMetamasteryBuff(string name, BlueprintFeature metamagicFeat, BlueprintAbility metamasteryAbility, Action<BlueprintBuff> init = null) {
                var result = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, $"ArcaneMetamastery{name.Replace(" ", "").Replace("(", "").Replace(")", "")}Buff", bp => {
                    bp.SetName(metamasteryAbility.m_DisplayName);
                    bp.SetDescription(metamasteryAbility.m_Description);
                    bp.m_Icon = metamagicFeat.Icon;
                    bp.AddComponent<AutoMetamagic>(c => {
                        c.m_Spellbook = new BlueprintSpellbookReference();
                        c.Metamagic = metamagicFeat.GetComponent<AddMetamagicFeat>().Metamagic;
                        c.School = SpellSchool.None;
                        c.MaxSpellLevel = 10;
                        c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
                    });
                });
                init?.Invoke(result);
                return result;
            }
            BlueprintAbility CreateArcaneMetamasteryAbility(string name, BlueprintFeature metamagicFeat, Action<BlueprintAbility> init = null) {
                var result = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, $"ArcaneMetamastery{name.Replace(" ", "").Replace("(", "").Replace(")", "")}Ability", bp => {
                    bp.SetName(TTTContext, $"Arcane Metamastery — {name}");
                    bp.SetDescription(ArcaneMetamasteryBaseAbility.m_Description);
                    bp.SetLocalizedDuration(TTTContext, "10 rounds");
                    bp.m_Icon = metamagicFeat.Icon;
                    bp.Type = AbilityType.Supernatural;
                    bp.Range = AbilityRange.Personal;
                    bp.CanTargetSelf = true;
                    bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                    bp.ActionType = CommandType.Swift;
                    bp.LocalizedSavingThrow = new LocalizedString();
                    bp.AddComponent<AbilityEffectRunAction>(c => {
                        c.Actions = new ActionList() {
                            Actions = new GameAction[] {
                                Helpers.Create<ContextActionApplyBuff>(a => {
                                    a.m_Buff = CreateArcaneMetamasteryBuff(name, metamagicFeat, bp).ToReference<BlueprintBuffReference>();
                                    a.DurationValue = new ContextDurationValue() {
                                        m_IsExtendable = true,
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = 10
                                    };
                                    a.AsChild = true;
                                })
                            }
                        };
                    });
                    bp.AddComponent<AbilityResourceLogic>(c => {
                        c.m_RequiredResource = ArcaneMetamasteryResource.ToReference<BlueprintAbilityResourceReference>();
                        c.m_IsSpendResource = true;
                        c.Amount = 1;
                    });
                    bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                        c.m_UnitFact = metamagicFeat.ToReference<BlueprintUnitFactReference>();
                    });
                    bp.AddComponent<AbiliyShowIfArcaneMetamastery>(c => {
                        c.m_ArcaneMetamasteryGreater = ArcaneMetamasteryGreater.ToReference<BlueprintUnitFactReference>();
                        c.m_Metamagic = metamagicFeat.GetComponent<AddMetamagicFeat>().Metamagic;
                    });
                });
                init?.Invoke(result);
                return result;
            }
            void AddBuffRemoval(BlueprintBuff targetBuff, BlueprintBuff[] allBuffs) {
                targetBuff.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
                var addFactContextActions = targetBuff.GetComponent<AddFactContextActions>();
                foreach (var buff in allBuffs.Where(b => b != targetBuff)) {
                    var removeBuff = new ContextActionRemoveBuff() {
                        m_Buff = buff.ToReference<BlueprintBuffReference>(),
                    };
                    var conditional = new Conditional() {
                        ConditionsChecker = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                        new ContextConditionHasBuff() {
                                            m_Buff = buff.ToReference<BlueprintBuffReference>(),
                                        }
                                    }
                        },
                        IfTrue = Helpers.CreateActionList(removeBuff),
                        IfFalse = Helpers.CreateActionList()
                    };
                    addFactContextActions.Activated.Actions = addFactContextActions.Activated.Actions.AppendToArray(conditional);
                }
            }

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("ArcaneMetamastery")) { return; }
            FeatTools.AddAsMythicAbility(ArcaneMetamastery);
            FeatTools.AddAsMythicAbility(ArcaneMetamasteryGreater);

            
        }
    }
}
