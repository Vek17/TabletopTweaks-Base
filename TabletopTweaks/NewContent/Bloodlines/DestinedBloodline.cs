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
    class DestinedBloodline {

        static BlueprintFeatureReference BloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(
            Settings.Blueprints.NewBlueprints["BloodlineRequisiteFeature"]).ToReference<BlueprintFeatureReference>();
        static BlueprintFeatureReference DestinedBloodlineRequisiteFeature = CreateBloodlineRequisiteFeature();

        static BlueprintFeatureReference CreateBloodlineRequisiteFeature() {
            var AberrantBloodlineRequisiteFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["DestinedBloodlineRequisiteFeature"];
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.name = "DestinedBloodlineRequisiteFeature";
                bp.SetName("Destined Bloodline");
                bp.SetDescription("Destined Bloodline Requisite Feature");
            });
            Resources.AddBlueprint(AberrantBloodlineRequisiteFeature);
            return AberrantBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
        }
        public static void AddBloodragerAberrantBloodline() {
            var BloodragerStandardRageBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
            var BloodragerClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499").ToReference<BlueprintCharacterClassReference>();
            var GreenragerArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("5648585af75596f4a9fa3ae385127f57").ToReference<BlueprintArchetypeReference>();
            //Bonus Spells
            var MageShield = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("ef768022b0785eb43a18969903c537c4").ToReference<BlueprintAbilityReference>();
            var Blur = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1").ToReference<BlueprintAbilityReference>();
            var ProtectionFromEnergy = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f").ToReference<BlueprintAbilityReference>();
            var FreedomOfMovement = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var Diehard = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad").ToReference<BlueprintFeatureReference>();
            var Endurance = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174").ToReference<BlueprintFeatureReference>();
            var ImprovedInitiative = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74").ToReference<BlueprintFeatureReference>();
            var IntimidatingProwess = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("d76497bfc48516e45a0831628f767a0f").ToReference<BlueprintFeatureReference>();
            var SiezeTheMoment = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760").ToReference<BlueprintFeatureReference>(); //No Grapple
            var LightningReflexes = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e").ToReference<BlueprintFeatureReference>();
            var WeaponFocus = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var BloodragerDestinedStrikeResource = Helpers.Create<BlueprintAbilityResource>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedStrikeResource"];
                bp.name = "BloodragerDestinedStrikeResource";
            });
            var BloodragerDestinedAbility = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedAbility"];
                bp.name = "BloodragerDestinedAbility";
                bp.SetName("Destined Strike");
                bp.SetDescription("At 1st level, as a free action up to three times per day you can grant yourself an insight bonus equal to 1/2 your "
                    + "bloodrager level (minimum 1) on one melee attack. At 12th level, you can use this ability up to five times per day.");
            });
            var BloodragerDestinedStrike = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedStrike"];
                bp.name = "BloodragerDestinedStrike";
                bp.SetName("Destined Strike");
                bp.SetDescription("At 1st level, as a free action up to three times per day you can grant yourself an insight bonus equal to 1/2 your "
                    +"bloodrager level (minimum 1) on one melee attack. At 12th level, you can use this ability up to five times per day.");
            });
            var BloodragerDestinedStrikeBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedStrikeBuff"];
                bp.name = "BloodragerDestinedStrikeBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName("Destined Strike");
                bp.SetDescription("At 1st level, as a free action up to three times per day you can grant yourself an insight bonus equal to 1/2 your "
                    + "bloodrager level (minimum 1) on one melee attack. At 12th level, you can use this ability up to five times per day.");
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
            var BloodragerDestinedFatedBloodrager = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedFatedBloodrager"];
                bp.name = "BloodragerDestinedFatedBloodrager";
                bp.SetName("Fated Bloodrager");
                bp.SetDescription("At 4th level, you gain a +1 luck bonus to AC and on saving throws. At 8th level and every"
                    +"4 levels thereafter, this bonus increases by 1 (to a maximum of +5 at 20th level).");
            });
            var BloodragerDestinedFatedBloodragerBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedFatedBloodragerBuff"];
                bp.name = "BloodragerDestinedFatedBloodragerBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(BloodragerDestinedFatedBloodrager.Name);
                bp.SetDescription(BloodragerDestinedFatedBloodrager.Description);
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.Reach;
                    c.Value = 5;
                }));
            });
            var BloodragerDestinedCertainStrike = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedCertainStrike"];
                bp.name = "BloodragerDestinedCertainStrike";
                bp.SetName("Certain Strike");
                bp.SetDescription("At 8th level, you may reroll an attack roll once during a bloodrage. You must decide to use this ability after "
                    +"the die is rolled, but before the GM reveals the results. You must take the second result, even if it’s worse.");
            });
            var BloodragerDestinedCertainStrikeBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedCertainStrikeBuff"];
                bp.name = "BloodragerDestinedCertainStrikeBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(BloodragerDestinedCertainStrike.Name);
                bp.SetDescription(BloodragerDestinedCertainStrike.Description);
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
            var BloodragerDestinedDefyDeath = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedDefyDeath"];
                bp.name = "BloodragerDestinedDefyDeath";
                bp.SetName("Defy Death");
                bp.SetDescription("At 12th level, once per day when an attack or spell that deals damage would result in your death"
                    +", you can attempt a DC 20 Fortitude save. If you succeed, you are instead reduced to 1 hit point; if you "
                    +"succeed and already have less than 1 hit point, you instead take no damage.");
            });
            var BloodragerDestinedDefyDeathBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedDefyDeathBuff"];
                bp.name = "BloodragerDestinedDefyDeathBuff";
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(BloodragerDestinedDefyDeath.Name);
                bp.SetDescription(BloodragerDestinedDefyDeath.Description);
                bp.AddComponent(Helpers.Create<AddFortification>(c => {
                    c.UseContextValue = false;
                    c.Bonus = 50;
                }));
            });
            var BloodragerDestinedUnstoppable = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedUnstoppable"];
                bp.name = "BloodragerDestinedUnstoppable";
                bp.SetName("Unstoppable ");
                bp.SetDescription("At 16th level, any critical threats you score are automatically confirmed. Any critical "
                    +"threats made against you confirm only if the second roll results in a natural 20 (or is automatically confirmed).");
            });
            var BloodragerDestinedUnstoppableBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedUnstoppableBuff"];
                bp.name = "BloodragerDestinedUnstoppableBuff";
                bp.SetName(BloodragerDestinedUnstoppable.Name);
                bp.SetDescription(BloodragerDestinedUnstoppable.Description);
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
            var BloodragerDestinedVictoryOrDeath = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedVictoryOrDeath"];
                bp.name = "BloodragerDestinedVictoryOrDeath";
                bp.SetName("Victory or Death");
                bp.SetDescription("At 20th level, you are immune to paralysis and petrification, as well as to the stunned, dazed, "
                    +"and staggered conditions. You have these benefits constantly, even while not bloodraging.");
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
            Resources.AddBlueprint(BloodragerDestinedStrikeResource);
            Resources.AddBlueprint(BloodragerDestinedStrike);
            Resources.AddBlueprint(BloodragerDestinedStrikeBuff);
            Resources.AddBlueprint(BloodragerDestinedFatedBloodrager);
            Resources.AddBlueprint(BloodragerDestinedFatedBloodragerBuff);
            Resources.AddBlueprint(BloodragerDestinedCertainStrike);
            Resources.AddBlueprint(BloodragerDestinedCertainStrikeBuff);
            Resources.AddBlueprint(BloodragerDestinedDefyDeath);
            Resources.AddBlueprint(BloodragerDestinedDefyDeathBuff);
            Resources.AddBlueprint(BloodragerDestinedUnstoppable);
            Resources.AddBlueprint(BloodragerDestinedUnstoppableBuff);
            Resources.AddBlueprint(BloodragerDestinedVictoryOrDeath);
            //Bloodline Feats
            var BloodragerDestinedFeatSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedFeatSelection"];
                bp.name = "BloodragerDestinedFeatSelection";
                bp.SetName("Bonus Feats");
                bp.SetDescription("Bonus Feats: Combat Reflexes, Great Fortitude, Improved Disarm, Improved Dirty Trick, Improved Initiative, Improved Unarmed Strike, Iron Will.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = new BlueprintFeatureReference[] {
                    Diehard,
                    Endurance,
                    ImprovedInitiative,
                    IntimidatingProwess,
                    SiezeTheMoment,
                    LightningReflexes,
                    WeaponFocus
                };
                bp.m_AllFeatures = bp.m_Features;
            });
            var BloodragerDestinedFeatSelectionGreenrager = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedFeatSelectionGreenrager"];
                bp.name = "BloodragerDestinedFeatSelectionGreenrager";
                bp.SetName(BloodragerDestinedFeatSelection.m_DisplayName);
                bp.SetDescription(BloodragerDestinedFeatSelection.m_Description);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = BloodragerDestinedFeatSelection.m_Features;
                bp.m_AllFeatures = bp.m_Features;
                bp.AddComponent(Helpers.Create<PrerequisiteNoArchetype>(c => {
                    c.HideInUI = true;
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Archetype = GreenragerArchetype;
                }));
            });
            Resources.AddBlueprint(BloodragerDestinedFeatSelection);
            Resources.AddBlueprint(BloodragerDestinedFeatSelectionGreenrager);
            //Bloodline Spells
            var BloodragerDestinedSpell7 = Helpers.Create<BlueprintFeature>(bp => {
                var spell = MageShield;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedSpell7"];
                bp.name = "BloodragerDestinedSpell7";
                bp.SetName($"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 1;
                }));
            });
            var BloodragerDestinedSpell10 = Helpers.Create<BlueprintFeature>(bp => {
                var spell = Blur;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedSpell10"];
                bp.name = "BloodragerDestinedSpell10";
                bp.SetName($"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 2;
                }));
            });
            var BloodragerDestinedSpell13 = Helpers.Create<BlueprintFeature>(bp => {
                var spell = ProtectionFromEnergy;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedSpell13"];
                bp.name = "BloodragerDestinedSpell13";
                bp.SetName($"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 3;
                }));
            });
            var BloodragerDestinedSpell16 = Helpers.Create<BlueprintFeature>(bp => {
                var spell = FreedomOfMovement;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedSpell16"];
                bp.SetName($"Bonus Spell — {spell.Get().Name}");
                bp.SetName("Bonus Spell — Freedom Of Movement");
                bp.SetDescription("At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.AddComponent(Helpers.Create<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 4;
                }));
            });
            Resources.AddBlueprint(BloodragerDestinedSpell7);
            Resources.AddBlueprint(BloodragerDestinedSpell10);
            Resources.AddBlueprint(BloodragerDestinedSpell13);
            Resources.AddBlueprint(BloodragerDestinedSpell16);
            //Bloodline Core
            var BloodragerDestinedBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedBloodline"];
                bp.name = "BloodragerDestinedBloodline";
                bp.SetName("Destined");
                bp.SetDescription("Your bloodline is destined for great things. When you bloodrage, you exude a greatness that makes all but the most legendary creatures seem lesser.");
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
                    new LevelEntry(){ Level = 1, Features = { BloodragerDestinedStrike, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 4, Features = { BloodragerDestinedFatedBloodrager }},
                    new LevelEntry(){ Level = 6, Features = { BloodragerDestinedFeatSelectionGreenrager }},
                    new LevelEntry(){ Level = 7, Features = { BloodragerDestinedSpell7 }},
                    new LevelEntry(){ Level = 8, Features = { BloodragerDestinedCertainStrike }},
                    new LevelEntry(){ Level = 9, Features = { BloodragerDestinedFeatSelectionGreenrager }},
                    new LevelEntry(){ Level = 10, Features = { BloodragerDestinedSpell10 }},
                    new LevelEntry(){ Level = 12, Features = { BloodragerDestinedFeatSelection, BloodragerDestinedDefyDeath }},
                    new LevelEntry(){ Level = 13, Features = { BloodragerDestinedSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { BloodragerDestinedFeatSelection }},
                    new LevelEntry(){ Level = 16, Features = { BloodragerDestinedUnstoppable, BloodragerDestinedSpell16 }},
                    new LevelEntry(){ Level = 18, Features = { BloodragerDestinedFeatSelection }},
                    new LevelEntry(){ Level = 20, Features = { BloodragerDestinedVictoryOrDeath }},
                };
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = DestinedBloodlineRequisiteFeature;
                }));
            });
            var BloodragerDestinedBaseBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.name = "BloodragerDestinedBaseBuff";
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["BloodragerDestinedBaseBuff"];
            });

            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedFatedBloodrager, BloodragerDestinedFatedBloodragerBuff);
            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedCertainStrike, BloodragerDestinedCertainStrikeBuff);
            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedDefyDeath, BloodragerDestinedDefyDeathBuff);
            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedUnstoppable, BloodragerDestinedUnstoppableBuff);
            Resources.AddBlueprint(BloodragerDestinedBloodline);
            Resources.AddBlueprint(BloodragerDestinedBaseBuff);
            BloodragerStandardRageBuff.AddConditionalBuff(BloodragerDestinedBloodline, BloodragerDestinedBaseBuff);

            BloodlineTools.ApplyPrimalistException(BloodragerDestinedFatedBloodrager, 4, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedCertainStrike, 8, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedDefyDeath, 12, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedUnstoppable, 16, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedVictoryOrDeath, 20, BloodragerDestinedBloodline);
            if (!Settings.AddedContent.AberrantBloodline || false) { return; }
            BloodlineTools.RegisterBloodragerBloodline(BloodragerDestinedBloodline);
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
                    + "of the spell by 50% (minimum 1 round). This bonus does not stack with the increase granted by the Extend Spell feat.");
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
                    + "foe within 30 feet as a ranged touch attack. The acidic ray deals 1d6 points of acid damage + 1 "
                    + "for every two sorcerer levels you possess. You can use this ability a number of times per day equal to 3 + your Charisma modifier.");
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
                    c.m_LineWidth = new Kingmaker.Utility.Feet() { m_Value = 5 };
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
                    + "This ability does not otherwise increase your threatened area. At 11th level, this bonus to your reach "
                    + "increases to 10 feet. At 17th level, this bonus to your reach increases to 15 feet.");
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
                    + "In addition, you gain blindsight with a range of 60 feet and damage reduction 5/—");
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
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
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
                    new LevelEntry(){ Level = 1, Features = { SorcererAberrantAcidicRay, SorcererAberrantBloodlineArcana, SorcererAberrantClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
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
                    c.m_Feature = DestinedBloodlineRequisiteFeature;
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
                    new LevelEntry(){ Level = 1, Features = { SorcererAberrantBloodlineArcana, SorcererAberrantClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
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
                    new LevelEntry(){ Level = 1, Features = { SorcererAberrantAcidicRay, SorcererAberrantBloodlineArcana, SorcererAberrantClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
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
                    c.m_Feature = DestinedBloodlineRequisiteFeature;
                }));
            });
            BloodlineTools.RegisterSorcererFeatSelection(SorcererAberrantFeatSelection, SorcererAberrantBloodline);

            Resources.AddBlueprint(SorcererAberrantBloodline);
            Resources.AddBlueprint(CrossbloodedAberrantBloodline);
            Resources.AddBlueprint(SeekerAberrantBloodline);

            if (!Settings.AddedContent.AberrantBloodline || false) { return; }
            BloodlineTools.RegisterSorcererBloodline(SorcererAberrantBloodline);
            BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedAberrantBloodline);
            BloodlineTools.RegisterSeekerBloodline(SeekerAberrantBloodline);
        }
    }
}