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
        public static void AddBloodragerDestinedBloodline() {
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

        public static void AddSorcererDestinedBloodline() {
            var SorcererClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf").ToReference<BlueprintCharacterClassReference>();
            var AcidArrow = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("9a46dfd390f943647ab4395fc997936d");
            var BloodlineInfernalClassSkill = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("f07a37a5b245304429530842cb65e213");

            //Bonus Spells
            var MageShield = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("ef768022b0785eb43a18969903c537c4").ToReference<BlueprintAbilityReference>();
            var Blur = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1").ToReference<BlueprintAbilityReference>();
            var ProtectionFromEnergy = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f").ToReference<BlueprintAbilityReference>();
            var FreedomOfMovement = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").ToReference<BlueprintAbilityReference>();
            var BreakEnchantment = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("7792da00c85b9e042a0fdfc2b66ec9a8").ToReference<BlueprintAbilityReference>();
            var HeroismGreater = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e15e5e7045fda2244b98c8f010adfe31").ToReference<BlueprintAbilityReference>();
            var CircleOfClarity = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("f333185ae986b2a45823cce86535a122").ToReference<BlueprintAbilityReference>();
            var ProtectionFromSpells = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("42aa71adc7343714fa92e471baa98d42").ToReference<BlueprintAbilityReference>();
            var Foresight = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1f01a098d737ec6419aedc4e7ad61fdd").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var ArcaneStrike = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0ab2f21a922feee4dab116238e3150b4").ToReference<BlueprintFeatureReference>();
            var Diehard = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad").ToReference<BlueprintFeatureReference>();
            var Endurance = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174").ToReference<BlueprintFeatureReference>();
            var MaximizeSpell = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b").ToReference<BlueprintFeatureReference>();
            var SiezeTheMoment = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760").ToReference<BlueprintFeatureReference>(); //No Grapple
            var LightningReflexes = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e").ToReference<BlueprintFeatureReference>();
            var WeaponFocus = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").ToReference<BlueprintFeatureReference>();
            var SkillFocusKnowledgeWorld = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("611e863120c0f9a4cab2d099f1eb20b4").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var SorcererDestinedClassSkill = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedClassSkill"];
                bp.name = "SorcererDestinedClassSkill";
                bp.SetName("Class Skill — Knowledge (World)");
                bp.SetDescription("Additional class skill from the destined bloodline.");
                bp.AddComponent(Helpers.Create<AddClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                }));
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BloodlineInfernalClassSkill.Icon;
            });
            var SorcererDestinedBloodlineArcana = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedBloodlineArcana"];
                bp.name = "SorcererDestinedBloodlineArcana";
                bp.IsClassFeature = true;
                bp.SetName("Destined Bloodline Arcana");
                bp.SetDescription("Whenever you cast a spell with a range of “personal,” you gain a luck bonus equal to the spell’s level on all your saving throws for 1 round.");
                bp.AddComponent(Helpers.Create<AberrantArcanaExtendComponent>());
            });
            var SorcererDestinedTouchOfDestinyResource = Helpers.Create<BlueprintAbilityResource>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedTouchOfDestinyResource"];
                bp.name = "SorcererDestinedTouchOfDestinyResource";
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
            var SorcererDestinedTouchOfDestinyAbility = Helpers.Create<BlueprintAbility>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedTouchOfDestinyAbility"];
                bp.name = "SorcererDestinedTouchOfDestinyAbility";
                bp.SetName("Touch of Destiny");
                bp.SetDescription("At 1st level, you can touch a creature as a standard action, giving it an insight bonus on attack rolls, skill checks, "
                    +"ability checks, and saving throws equal to 1/2 your sorcerer level (minimum 1) for 1 round. You can use this ability a number of "
                    +"times per day equal to 3 + your Charisma modifier.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.CanTargetEnemies = true;
                bp.Range = AbilityRange.Touch;
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
                    c.m_RequiredResource = SorcererDestinedTouchOfDestinyResource.ToReference<BlueprintAbilityResourceReference>();
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
            var SorcererDestinedTouchOfDestiny = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedTouchOfDestiny"];
                bp.name = "SorcererDestinedTouchOfDestiny";
                bp.SetName(SorcererDestinedTouchOfDestinyAbility.Name);
                bp.SetDescription(SorcererDestinedTouchOfDestinyAbility.Description);
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SorcererDestinedTouchOfDestinyAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                }));
                bp.AddComponent(Helpers.Create<AddAbilityResources>(c => {
                    c.m_Resource = SorcererDestinedTouchOfDestinyResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                }));
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = SorcererDestinedTouchOfDestinyAbility.Icon;
            });
            var SorcererDestinedFated = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedFated"];
                bp.name = "SorcererDestinedFated";
                bp.Ranks = 3;
                bp.IsClassFeature = true; ;
                bp.SetName("Fated");
                bp.SetDescription("Starting at 3rd level, you gain a +1 luck bonus on all of your saving throws and to your AC during surprise rounds"
                    +"(see Combat) and when you are otherwise unaware of an attack. At 7th level and every four levels thereafter, this bonus increases "
                    +"by +1, to a maximum of +5 at 19th level.");
                bp.AddComponent(Helpers.Create<AddTouchReach>(c => {
                    c.Value = 5;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                }));
            });
            var SorcererDestinedItWasMeantToBeResource = Helpers.Create<BlueprintAbilityResource>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedItWasMeantToBeResource"];
                bp.name = "SorcererDestinedItWasMeantToBeResource";
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
            var SorcererDestinedItWasMeantToBeAbility = Helpers.Create<BlueprintAbility>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedItWasMeantToBeAbility"];
                bp.name = "SorcererDestinedItWasMeantToBeAbility";
                bp.SetName("It Was Meant To Be");
                bp.SetDescription("At 9th level, you may reroll any one attack roll, critical hit confirmation roll, or level check made to overcome spell resistance. "
                    +"At 17th level, you can use this ability twice per day.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.CanTargetEnemies = true;
                bp.Range = AbilityRange.Touch;
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
                    c.m_RequiredResource = SorcererDestinedItWasMeantToBeResource.ToReference<BlueprintAbilityResourceReference>();
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
            var SorcererDestinedItWasMeantToBe = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedItWasMeantToBe"];
                bp.name = "SorcererDestinedItWasMeantToBe";
                bp.IsClassFeature = true;
                bp.Ranks = 2;
                bp.SetName("It Was Meant To Be");
                bp.SetDescription(" At 9th level, you may reroll any one attack roll, critical hit confirmation roll, or level check made to overcome spell resistance. "
                    +"At 9th level, you can use this ability once per day. At 17th level, you can use this ability twice per day.");
                bp.AddComponent(Helpers.Create<AddFortification>(c => {
                    c.UseContextValue = false;
                    c.Bonus = 25;
                }));
            });
            var SorcererDestinedWithinReachResource = Helpers.Create<BlueprintAbilityResource>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedWithinReachResource"];
                bp.name = "SorcererDestinedWithinReachResource";
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
            var SorcererDestinedWithinReach = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedWithinReach"];
                bp.name = "SorcererDestinedWithinReach";
                bp.IsClassFeature = true;
                bp.SetName("Within Reach");
                bp.SetDescription("At 15th level, your ultimate destiny is drawing near. Once per day, when an attack or spell that causes "
                    +"damage would result in your death, you may attempt a DC 20 Will save. If successful, you are instead reduced to –1 hit "
                    +"points and are automatically stabilized. The bonus from your fated ability applies to this save.");
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
            var SorcererDestinedDestinyRealizedResource = Helpers.Create<BlueprintAbilityResource>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedDestinyRealizedResource"];
                bp.name = "SorcererDestinedDestinyRealizedResource";
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
            var SorcererDestinedDestinyRealizedAbility = Helpers.Create<BlueprintAbility>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedDestinyRealizedAbility"];
                bp.name = "SorcererDestinedDestinyRealizedAbility";
                bp.SetName("Destiny Realized");
                bp.SetDescription("Once per day, you can automatically succeed at one caster level check made to overcome spell resistance. You must use this ability before making the roll.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.CanTargetEnemies = true;
                bp.Range = AbilityRange.Touch;
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
                    c.m_RequiredResource = SorcererDestinedDestinyRealizedResource.ToReference<BlueprintAbilityResourceReference>();
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
            var SorcererDestinedDestinyRealized = Helpers.Create<BlueprintFeature>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedDestinyRealized"];
                bp.name = "SorcererDestinedDestinyRealized";
                bp.SetName("Destiny Realized");
                bp.SetDescription("At 20th level, your moment of destiny is at hand. Any critical threats made against you only confirm if the second "
                    +"roll results in a natural 20 on the die. Any critical threats you score with a spell are automatically confirmed. Once per day, you "
                    +"can automatically succeed at one caster level check made to overcome spell resistance. You must use this ability before making the roll.");
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
            Resources.AddBlueprint(SorcererDestinedClassSkill);
            Resources.AddBlueprint(SorcererDestinedBloodlineArcana);
            Resources.AddBlueprint(SorcererDestinedTouchOfDestinyResource);
            Resources.AddBlueprint(SorcererDestinedTouchOfDestinyAbility);
            Resources.AddBlueprint(SorcererDestinedTouchOfDestiny);
            Resources.AddBlueprint(SorcererDestinedFated);
            Resources.AddBlueprint(SorcererDestinedItWasMeantToBeResource);
            Resources.AddBlueprint(SorcererDestinedItWasMeantToBeAbility);
            Resources.AddBlueprint(SorcererDestinedItWasMeantToBe);
            Resources.AddBlueprint(SorcererDestinedWithinReachResource);
            Resources.AddBlueprint(SorcererDestinedWithinReach);
            Resources.AddBlueprint(SorcererDestinedDestinyRealizedResource);
            Resources.AddBlueprint(SorcererDestinedDestinyRealizedAbility);
            Resources.AddBlueprint(SorcererDestinedDestinyRealized);
            //Bloodline Feats
            var SorcererDestinedFeatSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedFeatSelection"];
                bp.name = "SorcererDestinedFeatSelection";
                bp.SetName("Bonus Feats");
                bp.SetDescription("Bonus Feats: Arcane Strike, Diehard, Endurance, Sieze The Moment, Lightning Reflexes, Maximize Spell, Skill Focus (Knowledge World), Weapon Focus.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = new BlueprintFeatureReference[] {
                    ArcaneStrike,
                    Diehard,
                    Endurance,
                    SiezeTheMoment,
                    LightningReflexes,
                    WeaponFocus,
                    SkillFocusKnowledgeWorld,
                    MaximizeSpell
                };
                bp.m_AllFeatures = bp.m_Features;
            });
            Resources.AddBlueprint(SorcererDestinedFeatSelection);
            //Bloodline Spells
            var SorcererDestinedSpell3 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = MageShield;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell3"];
                bp.name = "SorcererDestinedSpell3";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell5 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = Blur;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell5"];
                bp.name = "SorcererDestinedSpell5";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell7 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = ProtectionFromEnergy;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell7"];
                bp.name = "SorcererDestinedSpell7";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell9 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = FreedomOfMovement;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell9"];
                bp.name = "SorcererDestinedSpell9";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell11 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = BreakEnchantment;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell11"];
                bp.name = "SorcererDestinedSpell11";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell13 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = HeroismGreater;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell13"];
                bp.name = "SorcererDestinedSpell13";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell15 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = CircleOfClarity;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell15"];
                bp.name = "SorcererDestinedSpell15";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell17 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = ProtectionFromSpells;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell17"];
                bp.name = "SorcererDestinedSpell17";
                bp.IsClassFeature = true;
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
            var SorcererDestinedSpell19 = Helpers.Create<BlueprintFeature>(bp => {
                var Spell = Foresight;
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedSpell19"];
                bp.name = "SorcererDestinedSpell19";
                bp.IsClassFeature = true;
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
            Resources.AddBlueprint(SorcererDestinedSpell3);
            Resources.AddBlueprint(SorcererDestinedSpell5);
            Resources.AddBlueprint(SorcererDestinedSpell7);
            Resources.AddBlueprint(SorcererDestinedSpell9);
            Resources.AddBlueprint(SorcererDestinedSpell11);
            Resources.AddBlueprint(SorcererDestinedSpell13);
            Resources.AddBlueprint(SorcererDestinedSpell15);
            Resources.AddBlueprint(SorcererDestinedSpell17);
            Resources.AddBlueprint(SorcererDestinedSpell19);
            //Bloodline Core
            var SorcererDestinedBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SorcererDestinedBloodline"];
                bp.name = "SorcererDestinedBloodline";
                bp.SetName("Destined Bloodline");
                bp.SetDescription("Your family is destined for greatness in some way. Your birth could have been foretold in prophecy, or perhaps "
                    + "it occurred during an especially auspicious event, such as a solar eclipse. Regardless of your bloodline’s origin, you have a great future ahead.\n"
                    + "Bonus Feats of the Aberrant Bloodline: Arcane Strike, Diehard, Endurance, Sieze The Moment, Lightning Reflexes, Maximize Spell, Skill Focus (Knowledge World), Weapon Focus.");
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
                    new LevelEntry(){ Level = 1, Features = { SorcererDestinedTouchOfDestiny, SorcererDestinedBloodlineArcana, SorcererDestinedClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 3, Features = { SorcererDestinedSpell3, SorcererDestinedFated }},
                    new LevelEntry(){ Level = 5, Features = { SorcererDestinedSpell5 }},
                    new LevelEntry(){ Level = 7, Features = { SorcererDestinedSpell7 }},
                    new LevelEntry(){ Level = 9, Features = { SorcererDestinedSpell9, SorcererDestinedItWasMeantToBe }},
                    new LevelEntry(){ Level = 11, Features = { SorcererDestinedSpell11 }},
                    new LevelEntry(){ Level = 13, Features = { SorcererDestinedSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { SorcererDestinedSpell15, SorcererDestinedWithinReach }},
                    new LevelEntry(){ Level = 17, Features = { SorcererDestinedSpell17 }},
                    new LevelEntry(){ Level = 19, Features = { SorcererDestinedSpell19 }},
                    new LevelEntry(){ Level = 20, Features = { SorcererDestinedDestinyRealized }},
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
            var CrossbloodedDestinedBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["CrossbloodedDestinedBloodline"];
                bp.name = "CrossbloodedDestinedBloodline";
                bp.SetName(SorcererDestinedBloodline.Name);
                bp.SetDescription(SorcererDestinedBloodline.Description);
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
                    new LevelEntry(){ Level = 1, Features = { SorcererDestinedBloodlineArcana, SorcererDestinedClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 3, Features = { SorcererDestinedSpell3 }},
                    new LevelEntry(){ Level = 5, Features = { SorcererDestinedSpell5 }},
                    new LevelEntry(){ Level = 7, Features = { SorcererDestinedSpell7 }},
                    new LevelEntry(){ Level = 9, Features = { SorcererDestinedSpell9 }},
                    new LevelEntry(){ Level = 11, Features = { SorcererDestinedSpell11 }},
                    new LevelEntry(){ Level = 13, Features = { SorcererDestinedSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { SorcererDestinedSpell15 }},
                    new LevelEntry(){ Level = 17, Features = { SorcererDestinedSpell17 }},
                    new LevelEntry(){ Level = 19, Features = { SorcererDestinedSpell19 }}
                };
            });
            var SeekerDestinedBloodline = Helpers.Create<BlueprintProgression>(bp => {
                bp.m_AssetGuid = Settings.Blueprints.NewBlueprints["SeekerDestinedBloodline"];
                bp.name = "SeekerDestinedBloodline";
                bp.SetName(SorcererDestinedBloodline.Name);
                bp.SetDescription(SorcererDestinedBloodline.Description);
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
                    new LevelEntry(){ Level = 1, Features = { SorcererDestinedTouchOfDestiny, SorcererDestinedBloodlineArcana, SorcererDestinedClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 3, Features = { SorcererDestinedSpell3 }},
                    new LevelEntry(){ Level = 5, Features = { SorcererDestinedSpell5 }},
                    new LevelEntry(){ Level = 7, Features = { SorcererDestinedSpell7 }},
                    new LevelEntry(){ Level = 9, Features = { SorcererDestinedSpell9 }},
                    new LevelEntry(){ Level = 11, Features = { SorcererDestinedSpell11 }},
                    new LevelEntry(){ Level = 13, Features = { SorcererDestinedSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { SorcererDestinedSpell15 }},
                    new LevelEntry(){ Level = 17, Features = { SorcererDestinedSpell17 }},
                    new LevelEntry(){ Level = 19, Features = { SorcererDestinedSpell19 }},
                    new LevelEntry(){ Level = 20, Features = { SorcererDestinedDestinyRealized }},
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
            BloodlineTools.RegisterSorcererFeatSelection(SorcererDestinedFeatSelection, SorcererDestinedBloodline);

            Resources.AddBlueprint(SorcererDestinedBloodline);
            Resources.AddBlueprint(CrossbloodedDestinedBloodline);
            Resources.AddBlueprint(SeekerDestinedBloodline);

            if (!Settings.AddedContent.AberrantBloodline || false) { return; }
            BloodlineTools.RegisterSorcererBloodline(SorcererDestinedBloodline);
            BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedDestinedBloodline);
            BloodlineTools.RegisterSeekerBloodline(SeekerDestinedBloodline);
        }
    }
}