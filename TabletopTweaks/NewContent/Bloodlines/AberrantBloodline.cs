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
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Bloodlines {
    public static class AberrantBloodline {
        static BlueprintFeatureReference BloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(
            Settings.Blueprints.NewBlueprints["BloodlineRequisiteFeature"]).ToReference<BlueprintFeatureReference>();
        static BlueprintFeatureReference AberrantBloodlineRequisiteFeature = CreateBloodlineRequisiteFeature();

        static BlueprintFeatureReference CreateBloodlineRequisiteFeature() {
            var AberrantBloodlineRequisiteFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["AberrantBloodlineRequisiteFeature"];
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.name = "AberrantBloodlineRequisiteFeature";
                bp.SetName("Aberrant Bloodline");
                bp.SetDescription("Aberrant Bloodline Requisite Feature");
            });
            Resources.AddBlueprint(AberrantBloodlineRequisiteFeature);
            return AberrantBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
        }
        public static void AddBloodragerAberrantBloodline() {
            var BloodragerStandardRageBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
            var BloodragerClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499").ToReference<BlueprintCharacterClassReference>();
            var GreenragerArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("5648585af75596f4a9fa3ae385127f57").ToReference<BlueprintArchetypeReference>();
            //Bonus Spells
            var EnlargePerson = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("c60969e7f264e6d4b84a1499fdcf9039").ToReference<BlueprintAbilityReference>();
            var SeeInvisibility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("30e5dc243f937fc4b95d2f8f4e1b7ff3").ToReference<BlueprintAbilityReference>();
            var Displacement = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("903092f6488f9ce45a80943923576ab3").ToReference<BlueprintAbilityReference>();
            var SpikeStones = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d1afa8bc28c99104da7d784115552de5").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var CombatReflexes = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0f8939ae6f220984e8fb568abbdfba95").ToReference<BlueprintFeatureReference>();
            var GreatFortitude = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("79042cb55f030614ea29956177977c52").ToReference<BlueprintFeatureReference>();
            var ImprovedDisarm = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("25bc9c439ac44fd44ac3b1e58890916f").ToReference<BlueprintFeatureReference>();
            var ImprovedDirtyTrick = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("ed699d64870044b43bb5a7fbe3f29494").ToReference<BlueprintFeatureReference>(); //No Grapple
            var ImprovedInitiative = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74").ToReference<BlueprintFeatureReference>();
            var ImprovedUnarmedStrike = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167").ToReference<BlueprintFeatureReference>();
            var IronWill = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("175d1577bb6c9a04baf88eec99c66334").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var BloodragerAberrantStaggeringStrike = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantStaggeringStrike"];
                bp.name = "BloodragerAberrantStaggeringStrike";
                bp.SetName("Staggering Strike");
                bp.SetDescription("At 1st level, when you confirm a critical hit the target must succeed at a Fortitude saving "
                    + "throw or be staggered for 1 round. The DC of this save is equal to 10 + 1/2 your bloodrager level + your "
                    + "Constitution modifier. These effects stack with the Staggering Critical feat; the target must save against "
                    + "each effect individually.");
            });
            var BloodragerAberrantStaggeringStrikeBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantStaggeringStrikeBuff"];
                bp.name = "BloodragerAberrantStaggeringStrikeBuff";
                bp.SetName("Staggering Strike");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                var Staggered = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3").ToReference<BlueprintBuffReference>();

                var applyBuff = Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = Staggered;
                    c.AsChild = false;
                    c.IsFromSpell = false;
                    c.DurationValue = new ContextDurationValue();
                    c.DurationValue.m_IsExtendable = true;
                    c.DurationValue.DiceCountValue = new ContextValue();
                    c.DurationValue.BonusValue = new ContextValue();
                    c.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    c.DurationValue.BonusValue = 1;
                });
                var conditionSaved = Helpers.Create<ContextActionConditionalSaved>(c => {
                    c.Failed = new ActionList();
                    c.Failed.Actions = c.Failed.Actions.AddToArray(applyBuff);
                });
                var savingThrow = Helpers.Create<ContextActionSavingThrow>(c => {
                    c.Type = SavingThrowType.Fortitude;
                    c.Actions = new ActionList();
                    c.Actions.Actions = c.Actions.Actions.AddToArray(conditionSaved);
                });
                bp.AddComponent(Helpers.Create<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CriticalHit = true;
                    c.Action = new ActionList();
                    c.Action.Actions = c.Action.Actions.AddToArray(savingThrow);
                }));
                bp.AddComponent(Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.StatType = StatType.Constitution;
                }));
            });
            var BloodragerAberrantAbnormalReach = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantAbnormalReach"];
                bp.name = "BloodragerAberrantAbnormalReach";
                bp.SetName("Abnormal Reach");
                bp.SetDescription("At 4th level, your limbs elongate and your reach increases by 5 feet.");
            });
            var BloodragerAberrantAbnormalReachBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantAbnormalReachBuff"];
                bp.name = "BloodragerAberrantAbnormalReachBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.Reach;
                    c.Value = 5;
                }));
            });
            var BloodragerAberrantFortitude = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantFortitude"];
                bp.name = "BloodragerAberrantFortitude";
                bp.SetName("Aberrant Fortitude");
                bp.SetDescription("At 8th level, you become immune to the sickened and nauseated conditions.");
            });
            var BloodragerAberrantFortitudeBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantFortitudeBuff"];
                bp.name = "BloodragerAberrantFortitudeBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Sickened;
                }));
                bp.AddComponent(Helpers.Create<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Nauseated;
                }));
                bp.AddComponent(Helpers.Create<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.Nauseated | SpellDescriptor.Sickened;
                }));
                bp.AddComponent(Helpers.Create<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Nauseated | SpellDescriptor.Sickened;
                }));
            });
            var BloodragerAberrantUnusualAnatomy = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantUnusualAnatomy"];
                bp.name = "BloodragerAberrantUnusualAnatomy";
                bp.SetName("Unusual Anatomy");
                bp.SetDescription("At 12th level, your internal anatomy shifts and changes, giving you a 50% chance to negate "
                    + "any critical hit or sneak attack that hits you. The damage is instead rolled normally.");
            });
            var BloodragerAberrantUnusualAnatomyBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantUnusualAnatomyBuff"];
                bp.name = "BloodragerAberrantUnusualAnatomyBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddFortification>(c => {
                    c.UseContextValue = false;
                    c.Bonus = 50;
                }));
            });
            var BloodragerAberrantResistance = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantResistance"];
                bp.name = "BloodragerAberrantResistance";
                bp.SetName("Aberrant Resistance");
                bp.SetDescription("At 16th level, you are immune to disease, exhaustion, fatigue, and poison, and to the staggered condition.");
            });
            var BloodragerAberrantResistanceBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantResistanceBuff"];
                bp.name = "BloodragerAberrantResistanceBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Fatigued;
                }));
                bp.AddComponent(Helpers.Create<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Exhausted;
                }));
                bp.AddComponent(Helpers.Create<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Staggered;
                }));
                bp.AddComponent(Helpers.Create<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted | SpellDescriptor.Staggered | SpellDescriptor.Disease | SpellDescriptor.Poison;
                }));
                bp.AddComponent(Helpers.Create<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted | SpellDescriptor.Staggered | SpellDescriptor.Disease | SpellDescriptor.Poison;
                }));
            });
            var BloodragerAberrantForm = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantForm"];
                bp.name = "BloodragerAberrantForm";
                bp.SetName("Aberrant Form");
                bp.SetDescription("At 20th level, your body becomes truly unnatural. You are immune to critical hits and sneak attacks. "
                    + "In addition, you gain blindsight with a range of 60 feet and your bloodrager damage reduction increases by 1. "
                    + "You have these benefits constantly, even while not bloodraging.");
                bp.AddComponent(Helpers.Create<Blindsense>(c => {
                    c.Range.m_Value = 60;
                    c.Blindsight = true;
                }));
                bp.AddComponent(Helpers.Create<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.GazeAttack;
                }));
                bp.AddComponent(Helpers.Create<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.GazeAttack;
                }));
                bp.AddComponent(Helpers.Create<AddDamageResistancePhysical>(c => {
                    c.BypassedByAlignment = false;
                    c.BypassedByForm = false;
                    c.BypassedByMagic = false;
                    c.BypassedByMaterial = false;
                    c.BypassedByReality = false;
                    c.BypassedByMeleeWeapon = false;
                    c.BypassedByWeaponType = false;
                    c.Value.Value = 1;
                    c.Value.ValueType = ContextValueType.Simple;
                }));
                bp.AddComponent(Helpers.Create<AddFortification>(c => {
                    c.UseContextValue = false;
                    c.Bonus = 100;
                }));
            });
            Resources.AddBlueprint(BloodragerAberrantStaggeringStrike);
            Resources.AddBlueprint(BloodragerAberrantStaggeringStrikeBuff);
            Resources.AddBlueprint(BloodragerAberrantAbnormalReach);
            Resources.AddBlueprint(BloodragerAberrantAbnormalReachBuff);
            Resources.AddBlueprint(BloodragerAberrantFortitude);
            Resources.AddBlueprint(BloodragerAberrantFortitudeBuff);
            Resources.AddBlueprint(BloodragerAberrantUnusualAnatomy);
            Resources.AddBlueprint(BloodragerAberrantUnusualAnatomyBuff);
            Resources.AddBlueprint(BloodragerAberrantResistance);
            Resources.AddBlueprint(BloodragerAberrantResistanceBuff);
            Resources.AddBlueprint(BloodragerAberrantForm);
            //Bloodline Feats
            var BloodragerAberrantFeatSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantFeatSelection"];
                bp.name = "BloodragerAberrantFeatSelection";
                bp.SetName("Bonus Feats");
                bp.SetDescription("Bonus Feats: Combat Reflexes, Great Fortitude, Improved Disarm, Improved Dirty Trick, Improved Initiative, Improved Unarmed Strike, Iron Will.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = new BlueprintFeatureReference[] {
                    CombatReflexes,
                    GreatFortitude,
                    ImprovedDisarm,
                    ImprovedDirtyTrick,
                    ImprovedInitiative,
                    ImprovedUnarmedStrike,
                    IronWill
                };
                bp.m_AllFeatures = bp.m_Features;
            });
            var BloodragerAberrantFeatSelectionGreenrager = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantFeatSelectionGreenrager"];
                bp.name = "BloodragerAberrantFeatSelectionGreenrager";
                bp.SetName(BloodragerAberrantFeatSelection.m_DisplayName);
                bp.SetDescription(BloodragerAberrantFeatSelection.m_Description);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = BloodragerAberrantFeatSelection.m_Features;
                bp.m_AllFeatures = bp.m_Features;
                bp.AddComponent(Helpers.Create<PrerequisiteNoArchetype>(c => {
                    c.HideInUI = true;
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Archetype = GreenragerArchetype;
                }));
            });
            Resources.AddBlueprint(BloodragerAberrantFeatSelection);
            Resources.AddBlueprint(BloodragerAberrantFeatSelectionGreenrager);
            //Bloodline Spells
            var BloodragerAberrantSpell7 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantSpell7"];
                bp.name = "BloodragerAberrantSpell7";
                bp.SetName("Bonus Spell — Enlarge Person");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = EnlargePerson;
                    c.SpellLevel = 1;
                }));
            });
            var BloodragerAberrantSpell10 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantSpell10"];
                bp.name = "BloodragerAberrantSpell10";
                bp.SetName("Bonus Spell — See Invisibility");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = SeeInvisibility;
                    c.SpellLevel = 2;
                }));
            });
            var BloodragerAberrantSpell13 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantSpell13"];
                bp.name = "BloodragerAberrantSpell13";
                bp.SetName("Bonus Spell — Displacement");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = Displacement;
                    c.SpellLevel = 3;
                }));
            });
            var BloodragerAberrantSpell16 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantSpell16"];
                bp.name = "BloodragerAberrantSpell16";
                bp.SetName("Bonus Spell — Stoneskin");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = SpikeStones;
                    c.SpellLevel = 4;
                }));
            });
            Resources.AddBlueprint(BloodragerAberrantSpell7);
            Resources.AddBlueprint(BloodragerAberrantSpell10);
            Resources.AddBlueprint(BloodragerAberrantSpell13);
            Resources.AddBlueprint(BloodragerAberrantSpell16);
            //Bloodline Core
            var BloodragerAberrantBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantBloodline"];
                bp.name = "BloodragerAberrantBloodline";
                bp.SetName("Aberrant");
                bp.SetDescription("There is a taint in your blood that is both alien and bizarre. When you bloodrage, this manifests in peculiar and terrifying ways.");
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = BloodragerClass
                    }
                };
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.LevelEntries = new LevelEntry[] {
                    new LevelEntry(){ Level = 1, Features = { BloodragerAberrantStaggeringStrike, AberrantBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 4, Features = { BloodragerAberrantAbnormalReach }},
                    new LevelEntry(){ Level = 6, Features = { BloodragerAberrantFeatSelectionGreenrager }},
                    new LevelEntry(){ Level = 7, Features = { BloodragerAberrantSpell7 }},
                    new LevelEntry(){ Level = 8, Features = { BloodragerAberrantFortitude }},
                    new LevelEntry(){ Level = 9, Features = { BloodragerAberrantFeatSelectionGreenrager }},
                    new LevelEntry(){ Level = 10, Features = { BloodragerAberrantSpell10 }},
                    new LevelEntry(){ Level = 12, Features = { BloodragerAberrantFeatSelection, BloodragerAberrantUnusualAnatomy }},
                    new LevelEntry(){ Level = 13, Features = { BloodragerAberrantSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { BloodragerAberrantFeatSelection }},
                    new LevelEntry(){ Level = 16, Features = { BloodragerAberrantResistance, BloodragerAberrantSpell16 }},
                    new LevelEntry(){ Level = 18, Features = { BloodragerAberrantFeatSelection }},
                    new LevelEntry(){ Level = 20, Features = { BloodragerAberrantForm }},
                };
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = AberrantBloodlineRequisiteFeature;
                }));
            });
            var BloodragerAberrantBaseBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.name = "BloodragerAberrantBaseBuff";
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerAberrantBaseBuff"];
            });

            BloodragerAberrantBaseBuff.AddConditionalBuff(BloodragerAberrantStaggeringStrike, BloodragerAberrantStaggeringStrikeBuff);
            BloodragerAberrantBaseBuff.AddConditionalBuff(BloodragerAberrantAbnormalReach, BloodragerAberrantAbnormalReachBuff);
            BloodragerAberrantBaseBuff.AddConditionalBuff(BloodragerAberrantFortitude, BloodragerAberrantFortitudeBuff);
            BloodragerAberrantBaseBuff.AddConditionalBuff(BloodragerAberrantUnusualAnatomy, BloodragerAberrantUnusualAnatomyBuff);
            BloodragerAberrantBaseBuff.AddConditionalBuff(BloodragerAberrantResistance, BloodragerAberrantResistanceBuff);
            Resources.AddBlueprint(BloodragerAberrantBloodline);
            Resources.AddBlueprint(BloodragerAberrantBaseBuff);
            BloodragerStandardRageBuff.AddConditionalBuff(BloodragerAberrantBloodline, BloodragerAberrantBaseBuff);

            BloodlineTools.ApplyPrimalistException(BloodragerAberrantAbnormalReach, 4, BloodragerAberrantBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerAberrantFortitude, 8, BloodragerAberrantBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerAberrantUnusualAnatomy, 12, BloodragerAberrantBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerAberrantResistance, 16, BloodragerAberrantBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerAberrantForm, 20, BloodragerAberrantBloodline);
            if (!Settings.AddedContent.AberrantBloodline) { return; }
            BloodlineTools.RegisterBloodragerBloodline(BloodragerAberrantBloodline);
        }

        public static void AddSorcererAberrantBloodline() {
            var SorcererClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf").ToReference<BlueprintCharacterClassReference>();
            var AcidArrow = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("9a46dfd390f943647ab4395fc997936d");
            var BloodlineInfernalClassSkill = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("f07a37a5b245304429530842cb65e213");
            //Bonus Spells
            var EnlargePerson = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("c60969e7f264e6d4b84a1499fdcf9039").ToReference<BlueprintAbilityReference>();
            var SeeInvisibility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("30e5dc243f937fc4b95d2f8f4e1b7ff3").ToReference<BlueprintAbilityReference>();
            var Blink = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("045351f1421ee3f449a9143db701d192").ToReference<BlueprintAbilityReference>();
            var SpikeStones = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d1afa8bc28c99104da7d784115552de5").ToReference<BlueprintAbilityReference>();
            var Feeblemind = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("444eed6e26f773a40ab6e4d160c67faa").ToReference<BlueprintAbilityReference>();
            var Eyebite = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("3167d30dd3c622c46b0c0cb242061642").ToReference<BlueprintAbilityReference>();
            var PolymorphGreaterBase = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("a9fc28e147dbb364ea4a3c1831e7e55f").ToReference<BlueprintAbilityReference>();
            var MindBlank = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("df2a0ba6b6dcecf429cbb80a56fee5cf").ToReference<BlueprintAbilityReference>();
            var ShapeChange = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("22b9044aa229815429d57d0a30e4b739").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var CombatCasting = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("06964d468fde1dc4aa71a92ea04d930d").ToReference<BlueprintFeatureReference>();
            var ImprovedDisarm = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("25bc9c439ac44fd44ac3b1e58890916f").ToReference<BlueprintFeatureReference>();
            var ImprovedDirtyTrick = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("ed699d64870044b43bb5a7fbe3f29494").ToReference<BlueprintFeatureReference>(); //No Grapple
            var ImprovedInitiative = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74").ToReference<BlueprintFeatureReference>();
            var ImprovedUnarmedStrike = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167").ToReference<BlueprintFeatureReference>();
            var IronWill = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("175d1577bb6c9a04baf88eec99c66334").ToReference<BlueprintFeatureReference>();
            var ExtendSpell = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef").ToReference<BlueprintFeatureReference>();
            var SkillFocusKnowledgeWorld = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("611e863120c0f9a4cab2d099f1eb20b4").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var SorcererAberrantClassSkill = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantClassSkill"];
                bp.name = "SorcererAberrantClassSkill";
                bp.SetName("Class Skill — Knowledge (World)");
                bp.SetDescription("Additional class skill from the aberrant bloodline.");
                bp.AddComponent(Helpers.Create<AddClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                }));
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BloodlineInfernalClassSkill.Icon;
            });
            var SorcererAberrantBloodlineArcana = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantBloodlineArcana"];
                bp.name = "SorcererAberrantBloodlineArcana";
                bp.IsClassFeature = true;
                bp.SetName("Aberrant Bloodline Arcana");
                bp.SetDescription("Whenever you cast a spell of the polymorph subschool, increase the duration "
                    +"of the spell by 50% (minimum 1 round). This bonus does not stack with the increase granted by the Extend Spell feat.");
                bp.AddComponent(Helpers.Create<AberrantArcanaExtendComponent>());
            });
            var SorcererAberrantAcidicRayResource = Helpers.Create<BlueprintAbilityResource>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantAcidicRayResource"];
                bp.name = "SorcererAberrantAcidicRayResource";
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 3,
                    IncreasedByStat = true,
                    ResourceBonusStat = StatType.Charisma,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var SorcererAberrantAcidicRayAbility = Helpers.Create<BlueprintAbility>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantAcidicRayAbility"];
                bp.name = "SorcererAberrantAcidicRayAbility";
                bp.SetName("Acidic Ray");
                bp.SetDescription("Starting at 1st level, you can fire an acidic ray as a standard action, targeting any "
                    +"foe within 30 feet as a ranged touch attack. The acidic ray deals 1d6 points of acid damage + 1 "
                    +"for every two sorcerer levels you possess. You can use this ability a number of times per day equal to 3 + your Charisma modifier.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.CanTargetEnemies = true;
                bp.Range = AbilityRange.Close;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = AcidArrow.Icon;
                bp.ResourceAssetIds = AcidArrow.ResourceAssetIds;
                bp.AddComponent(Helpers.Create<SpellComponent>(c => {
                    c.School = SpellSchool.Conjuration;
                }));
                bp.AddComponent(Helpers.Create<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Acid;
                }));
                bp.AddComponent(Helpers.Create<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = SorcererAberrantAcidicRayResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                }));
                bp.AddComponent(Helpers.Create<AbilityDeliverProjectile>(c => {
                    c.m_Projectiles = AcidArrow.GetComponent<AbilityDeliverProjectile>().m_Projectiles;
                    c.m_LineWidth = new Kingmaker.Utility.Feet() { m_Value = 5};
                    c.m_Weapon = AcidArrow.GetComponent<AbilityDeliverProjectile>().m_Weapon;
                    c.NeedAttackRoll = true;
                }));
                var dealDamage = Helpers.Create<ContextActionDealDamage>(c => {
                    c.DamageType = new DamageTypeDescription {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Acid
                    };
                    c.Duration = new ContextDurationValue() {
                        m_IsExtendable = true,
                        DiceCountValue = new ContextValue(),
                        BonusValue = new ContextValue()
                    };
                    c.Value = new ContextDiceValue {
                        DiceType = DiceType.D6,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 1
                        },
                        BonusValue = new ContextValue {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus
                        }
                    };
                });
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] { dealDamage };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 2;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[] { SorcererClass };
                }));
            });
            var SorcererAberrantAcidicRay = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantAcidicRay"];
                bp.name = "SorcererAberrantAcidicRay";
                bp.SetName("Acidic Ray");
                bp.SetDescription("Starting at 1st level, you can fire an acidic ray as a standard action, targeting any "
                    + "foe within 30 feet as a ranged touch attack. The acidic ray deals 1d6 points of acid damage + 1 "
                    + "for every two sorcerer levels you possess. You can use this ability a number of times per day equal to 3 + your Charisma modifier.");
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SorcererAberrantAcidicRayAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                }));
                bp.AddComponent(Helpers.Create<AddAbilityResources>(c => {
                    c.m_Resource = SorcererAberrantAcidicRayResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                }));
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = SorcererAberrantAcidicRayAbility.Icon;
            });
            var SorcererAberrantLongLimbs = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantLongLimbs"];
                bp.name = "SorcererAberrantLongLimbs";
                bp.Ranks = 3;
                bp.IsClassFeature = true; ;
                bp.SetName("Long Limbs");
                bp.SetDescription("At 3rd level, your reach increases by 5 feet whenever you are making a melee touch attack. "
                    +"This ability does not otherwise increase your threatened area. At 11th level, this bonus to your reach "
                    +"increases to 10 feet. At 17th level, this bonus to your reach increases to 15 feet.");
                bp.AddComponent(Helpers.Create<AddTouchReach>(c => {
                    c.Value = 5;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                }));
            });
            var SorcererAberrantUnusualAnatomy = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantUnusualAnatomy"];
                bp.name = "SorcererAberrantUnusualAnatomy";
                bp.IsClassFeature = true;
                bp.Ranks = 2;
                bp.SetName("Unusual Anatomy");
                bp.SetDescription("At 9th level, your anatomy changes, giving you a 25% chance to ignore any critical hit or sneak attack scored against you. This chance increases to 50% at 13th level.");
                bp.AddComponent(Helpers.Create<AddFortification>(c => {
                    c.UseContextValue = false;
                    c.Bonus = 25;
                }));
            });
            var SorcererAberrantAlienResistance = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantAlienResistance"];
                bp.name = "SorcererAberrantAlienResistance";
                bp.IsClassFeature = true;
                bp.SetName("Alien Resistance");
                bp.SetDescription("At 15th level, you gain spell resistance equal to your sorcerer level + 10.");
                bp.AddComponent(Helpers.Create<AddSpellResistance>(c => {
                    c.Value = new ContextValue();
                    c.Value.ValueType = ContextValueType.Rank;
                    c.Value.ValueRank = AbilityRankType.StatBonus;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.BonusValue;
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_StepLevel = 10;
                    c.m_Class = new BlueprintCharacterClassReference[] { SorcererClass };
                }));
            });
            var SorcererAberrantForm = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantForm"];
                bp.name = "SorcererAberrantForm";
                bp.SetName("Aberrant Form");
                bp.SetDescription("At 20th level, your body becomes truly unnatural. You are immune to critical hits and sneak attacks. +"
                    +"In addition, you gain blindsight with a range of 60 feet and damage reduction 5/—");
                bp.AddComponent(Helpers.Create<Blindsense>(c => {
                    c.Range.m_Value = 60;
                    c.Blindsight = true;
                }));
                bp.AddComponent(Helpers.Create<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.GazeAttack;
                }));
                bp.AddComponent(Helpers.Create<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.GazeAttack;
                }));
                bp.AddComponent(Helpers.Create<AddDamageResistancePhysical>(c => {
                    c.BypassedByAlignment = false;
                    c.BypassedByForm = false;
                    c.BypassedByMagic = false;
                    c.BypassedByMaterial = false;
                    c.BypassedByReality = false;
                    c.BypassedByMeleeWeapon = false;
                    c.BypassedByWeaponType = false;
                    c.Value.Value = 5;
                    c.Value.ValueType = ContextValueType.Simple;
                }));
                bp.AddComponent(Helpers.Create<AddFortification>(c => {
                    c.UseContextValue = false;
                    c.Bonus = 100;
                }));
            });
            Resources.AddBlueprint(SorcererAberrantClassSkill);
            Resources.AddBlueprint(SorcererAberrantBloodlineArcana);
            Resources.AddBlueprint(SorcererAberrantAcidicRayResource);
            Resources.AddBlueprint(SorcererAberrantAcidicRayAbility);
            Resources.AddBlueprint(SorcererAberrantAcidicRay);
            Resources.AddBlueprint(SorcererAberrantLongLimbs);
            Resources.AddBlueprint(SorcererAberrantUnusualAnatomy);
            Resources.AddBlueprint(SorcererAberrantAlienResistance);
            Resources.AddBlueprint(SorcererAberrantForm);
            //Bloodline Feats
            var SorcererAberrantFeatSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantFeatSelection"];
                bp.name = "SorcererAberrantFeatSelection";
                bp.SetName("Bonus Feats");
                bp.SetDescription("Bonus Feats: Combat Casting, Improved Disarm, Improved Dirty Trick, Improved Initiative, Improved Unarmed Strike, Iron Will, Extend Spell, Skill Focus (Knowledge World).");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = new BlueprintFeatureReference[] {
                    CombatCasting,
                    ImprovedDisarm,
                    ImprovedDirtyTrick,
                    ImprovedInitiative,
                    ImprovedUnarmedStrike,
                    IronWill,
                    ExtendSpell,
                    SkillFocusKnowledgeWorld
                };
                bp.m_AllFeatures = bp.m_Features;
            });
            Resources.AddBlueprint(SorcererAberrantFeatSelection);
            //Bloodline Spells
            var SorcererAberrantSpell3 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell3"];
                bp.name = "SorcererAberrantSpell3";
                bp.IsClassFeature = true;
                var Spell = EnlargePerson;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 1;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell5 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell5"];
                bp.name = "SorcererAberrantSpell5";
                bp.IsClassFeature = true;
                var Spell = SeeInvisibility;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 2;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell7 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell7"];
                bp.name = "SorcererAberrantSpell7";
                bp.IsClassFeature = true;
                var Spell = Blink;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    +$"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 3;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell9 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell9"];
                bp.name = "SorcererAberrantSpell9";
                bp.IsClassFeature = true;
                var Spell = SpikeStones;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 4;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell11 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell11"];
                bp.name = "SorcererAberrantSpell11";
                bp.IsClassFeature = true;
                var Spell = Feeblemind;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 5;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell13 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell13"];
                bp.name = "SorcererAberrantSpell13";
                bp.IsClassFeature = true;
                var Spell = Eyebite;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 6;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell15 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell15"];
                bp.name = "SorcererAberrantSpell15";
                bp.IsClassFeature = true;
                var Spell = PolymorphGreaterBase;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 7;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell17 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell17"];
                bp.name = "SorcererAberrantSpell17";
                bp.IsClassFeature = true;
                var Spell = MindBlank;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 8;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererAberrantSpell19 = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantSpell19"];
                bp.name = "SorcererAberrantSpell19";
                bp.IsClassFeature = true;
                var Spell = ShapeChange;
                bp.SetName(Spell.Get().Name);
                bp.SetDescription("At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 9;
                }));
                bp.m_Icon = Spell.Get().Icon;
            });
            Resources.AddBlueprint(SorcererAberrantSpell3);
            Resources.AddBlueprint(SorcererAberrantSpell5);
            Resources.AddBlueprint(SorcererAberrantSpell7);
            Resources.AddBlueprint(SorcererAberrantSpell9);
            Resources.AddBlueprint(SorcererAberrantSpell11);
            Resources.AddBlueprint(SorcererAberrantSpell13);
            Resources.AddBlueprint(SorcererAberrantSpell15);
            Resources.AddBlueprint(SorcererAberrantSpell17);
            Resources.AddBlueprint(SorcererAberrantSpell19);
            //Bloodline Core
            var SorcererAberrantBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererAberrantBloodline"];
                bp.name = "SorcererAberrantBloodline";
                bp.SetName("Aberrant Bloodline");
                bp.SetDescription("There is a taint in your blood, one that is alien and bizarre. You tend to think in odd ways, approaching problems "
                    +"from an angle that most would not expect. Over time, this taint manifests itself in your physical form.\n"
                    +"Bonus Feats of the Aberrant Bloodline:: Combat Casting, Improved Disarm, Improved Dirty Trick, Improved Initiative, "
                    +"Improved Unarmed Strike, Iron Will, Extend Spell, Skill Focus (Knowledge World).");
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = SorcererClass
                    }
                };
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.LevelEntries = new LevelEntry[] {
                    new LevelEntry(){ Level = 1, Features = { SorcererAberrantAcidicRay, SorcererAberrantBloodlineArcana, SorcererAberrantClassSkill, AberrantBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 3, Features = { SorcererAberrantSpell3, SorcererAberrantLongLimbs }},
                    new LevelEntry(){ Level = 5, Features = { SorcererAberrantSpell5 }},
                    new LevelEntry(){ Level = 7, Features = { SorcererAberrantSpell7 }},
                    new LevelEntry(){ Level = 9, Features = { SorcererAberrantSpell9, SorcererAberrantUnusualAnatomy }},
                    new LevelEntry(){ Level = 11, Features = { SorcererAberrantSpell11, SorcererAberrantLongLimbs }},
                    new LevelEntry(){ Level = 13, Features = { SorcererAberrantSpell13, SorcererAberrantUnusualAnatomy }},
                    new LevelEntry(){ Level = 15, Features = { SorcererAberrantSpell15, SorcererAberrantAlienResistance }},
                    new LevelEntry(){ Level = 17, Features = { SorcererAberrantSpell17, SorcererAberrantLongLimbs }},
                    new LevelEntry(){ Level = 19, Features = { SorcererAberrantSpell19 }},
                    new LevelEntry(){ Level = 20, Features = { SorcererAberrantForm }},
                };
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = AberrantBloodlineRequisiteFeature;
                }));
            });
            var CrossbloodedAberrantBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["CrossbloodedAberrantBloodline"];
                bp.name = "CrossbloodedAberrantBloodline";
                bp.SetName("Aberrant Bloodline");
                bp.SetDescription("There is a taint in your blood, one that is alien and bizarre. You tend to think in odd ways, approaching problems "
                    + "from an angle that most would not expect. Over time, this taint manifests itself in your physical form.\n"
                    + "Bonus Feats of the Aberrant Bloodline:: Combat Casting, Improved Disarm, Improved Dirty Trick, Improved Initiative, "
                    + "Improved Unarmed Strike, Iron Will, Extend Spell, Skill Focus (Knowledge World).");
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = SorcererClass
                    }
                };
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.LevelEntries = new LevelEntry[] {
                    new LevelEntry(){ Level = 1, Features = { SorcererAberrantBloodlineArcana, SorcererAberrantClassSkill, AberrantBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 3, Features = { SorcererAberrantSpell3 }},
                    new LevelEntry(){ Level = 5, Features = { SorcererAberrantSpell5 }},
                    new LevelEntry(){ Level = 7, Features = { SorcererAberrantSpell7 }},
                    new LevelEntry(){ Level = 9, Features = { SorcererAberrantSpell9 }},
                    new LevelEntry(){ Level = 11, Features = { SorcererAberrantSpell11 }},
                    new LevelEntry(){ Level = 13, Features = { SorcererAberrantSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { SorcererAberrantSpell15 }},
                    new LevelEntry(){ Level = 17, Features = { SorcererAberrantSpell17 }},
                    new LevelEntry(){ Level = 19, Features = { SorcererAberrantSpell19 }}
                };
            });
            var SeekerAberrantBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SeekerAberrantBloodline"];
                bp.name = "SeekerAberrantBloodline";
                bp.SetName("Aberrant Bloodline");
                bp.SetDescription("There is a taint in your blood, one that is alien and bizarre. You tend to think in odd ways, approaching problems "
                    + "from an angle that most would not expect. Over time, this taint manifests itself in your physical form.\n"
                    + "Bonus Feats of the Aberrant Bloodline:: Combat Casting, Improved Disarm, Improved Dirty Trick, Improved Initiative, "
                    + "Improved Unarmed Strike, Iron Will, Extend Spell, Skill Focus (Knowledge World).");
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = SorcererClass
                    }
                };
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.LevelEntries = new LevelEntry[] {
                    new LevelEntry(){ Level = 1, Features = { SorcererAberrantAcidicRay, SorcererAberrantBloodlineArcana, SorcererAberrantClassSkill, AberrantBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 3, Features = { SorcererAberrantSpell3 }},
                    new LevelEntry(){ Level = 5, Features = { SorcererAberrantSpell5 }},
                    new LevelEntry(){ Level = 7, Features = { SorcererAberrantSpell7 }},
                    new LevelEntry(){ Level = 9, Features = { SorcererAberrantSpell9, SorcererAberrantUnusualAnatomy }},
                    new LevelEntry(){ Level = 11, Features = { SorcererAberrantSpell11 }},
                    new LevelEntry(){ Level = 13, Features = { SorcererAberrantSpell13, SorcererAberrantUnusualAnatomy }},
                    new LevelEntry(){ Level = 15, Features = { SorcererAberrantSpell15 }},
                    new LevelEntry(){ Level = 17, Features = { SorcererAberrantSpell17 }},
                    new LevelEntry(){ Level = 19, Features = { SorcererAberrantSpell19 }},
                    new LevelEntry(){ Level = 20, Features = { SorcererAberrantForm }},
                };
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = AberrantBloodlineRequisiteFeature;
                }));
            });
            BloodlineTools.RegisterSorcererFeatSelection(SorcererAberrantFeatSelection, SorcererAberrantBloodline);

            Resources.AddBlueprint(SorcererAberrantBloodline);
            Resources.AddBlueprint(CrossbloodedAberrantBloodline);
            Resources.AddBlueprint(SeekerAberrantBloodline);

            if (!Settings.AddedContent.AberrantBloodline) { return; }
            BloodlineTools.RegisterSorcererBloodline(SorcererAberrantBloodline);
            BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedAberrantBloodline);
            BloodlineTools.RegisterSeekerBloodline(SeekerAberrantBloodline);
        }
    }
}
