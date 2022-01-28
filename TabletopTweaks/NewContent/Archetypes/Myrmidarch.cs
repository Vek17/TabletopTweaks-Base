using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    static class Myrmidarch {

        public static void AddMyrmidarch() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var SwordSaintSpellLevels = Resources.GetBlueprint<BlueprintSpellsTable>("b9fdc0b2d37eb9e4298f9163edf5ca82");
            var MagusArcanaSelection = Resources.GetBlueprint<BlueprintFeature>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
            var MagusSpellRecallFeature = Resources.GetBlueprint<BlueprintFeature>("61fc0521e9992624e9c518060bf89c0f");
            var FighterTraining = Resources.GetBlueprint<BlueprintFeature>("2b636b9e8dd7df94cbd372c52237eebf");
            var MagusImprovedSpellRecallFeature = Resources.GetBlueprint<BlueprintFeature>("0ef6ec1c2fdfc204fbd3bff9f1609490");
            var ImprovedSpellCombat = Resources.GetBlueprint<BlueprintFeature>("836879fcd5b29754eb664a090bd6c22f");
            var GreaterSpellCombat = Resources.GetBlueprint<BlueprintFeature>("379887a82a7248946bbf6d0158663b5e");
            var TrueMagusFeature = Resources.GetBlueprint<BlueprintFeature>("789c7539ba659174db702e18d7c2d330");

            var EldritchArcherRangedSpellStrike = Resources.GetBlueprint<BlueprintFeature>("6aa84ca8918ac604685a3d39a13faecc");
            var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
            var WeaponTrainingRankUpSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("5f3cc7b9a46b880448275763fe70c0b0");
            var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorMastery = Resources.GetBlueprint<BlueprintFeature>("ae177f17cfb45264291d4d7c2cb64671");
            var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
            var FighterTrainingFakeLevel = Resources.GetModBlueprintReference<BlueprintFeatureBaseReference>("FighterTrainingFakeLevel");

            var ArcaneMediumArmor = Resources.GetBlueprintReference<BlueprintFeatureBaseReference>("b24897e082896654c8dd64c8fb677363");
            var ArcaneHeavyArmor = Resources.GetBlueprintReference<BlueprintFeatureBaseReference>("447ca91389e5c9246acb2c640d63f4da");

            var MyrmidarchSpellLevels = Helpers.CreateBlueprint<BlueprintSpellsTable>("MyrmidarchSpellLevels", bp => {
                bp.Levels = SwordSaintSpellLevels.Levels.Select(level => SpellTools.CreateSpellLevelEntry(level.Count)).ToArray();
            });

            var MyrmidarchSpellbook = Helpers.CreateCopy<BlueprintSpellbook>(MagusClass.Spellbook, bp => {
                bp.name = "MyrmidarchSpellbook";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_SpellsPerDay = MyrmidarchSpellLevels.ToReference<BlueprintSpellsTableReference>();
                SpellTools.Spellbook.AllSpellbooks.Add(bp);
                Resources.AddBlueprint(bp);
            });
            /*
            var MyrmidarchSpellbook = Helpers.CreateBlueprint<BlueprintSpellbook>("MyrmidarchSpellbook", bp => {
                bp.Name = MagusClass.Spellbook.Name;
                bp.CastingAttribute = MagusClass.Spellbook.CastingAttribute;
                bp.AllSpellsKnown = MagusClass.Spellbook.AllSpellsKnown;
                bp.CantripsType = MagusClass.Spellbook.CantripsType;
                bp.HasSpecialSpellList = MagusClass.Spellbook.HasSpecialSpellList;
                bp.SpecialSpellListName = MagusClass.Spellbook.SpecialSpellListName;
                bp.m_SpellsPerDay = MyrmidarchSpellLevels.ToReference<BlueprintSpellsTableReference>();
                bp.m_SpellsKnown = MagusClass.Spellbook.m_SpellsKnown;
                bp.m_SpellSlots = MagusClass.Spellbook.m_SpellSlots;
                bp.m_SpellList = MagusClass.Spellbook.m_SpellList;
                bp.m_MythicSpellList = MagusClass.Spellbook.m_MythicSpellList;
                bp.m_CharacterClass = MagusClass.Spellbook.m_CharacterClass;
                bp.m_Overrides = MagusClass.Spellbook.m_Overrides;
                bp.IsArcane = true;
                SpellTools.Spellbook.AllSpellbooks.Add(bp);
            });
            */
            var MyrmidarchFighterTraining = Helpers.CreateBlueprint<BlueprintFeature>("MyrmidarchFighterTraining", bp => {
                bp.SetName("Fighter Training");
                bp.SetDescription("At 7th level, a myrmidarch counts his magus level –3 as his fighter level for " +
                    "the purpose of qualifying for feats (if he has levels in fighter, these levels stack).");
                bp.m_Icon = FighterTraining.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<ClassLevelsForPrerequisites>(c => {
                    c.m_FakeClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_ActualClass = MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.Modifier = 1;
                    c.Summand = -3;
                });
            });

            var MyrmidarchFighterTrainingUpgrade = Helpers.CreateBlueprint<BlueprintProgression>("MyrmidarchFighterTrainingUpgrade", bp => {
                bp.SetName("Fighter Training Upgrade");
                bp.SetDescription("At 10th level, the myrmidarch treats his magus levels as fighter levels for the purposes of fighter training.");
                bp.m_Icon = FighterTraining.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            FighterTrainingFakeLevel
                        },
                    })
                    .ToArray();
                bp.AddClass(MagusClass);
            });

            var MyrmidarchArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("MyrmidarchArchetype", bp => {
                bp.SetName("Myrmidarch");
                bp.SetDescription("The myrmidarch is a skilled specialist, using magic to supplement and augment his martial mastery. " +
                    "Less inclined to mix the two than a typical magus, the myrmidarch seeks supremacy with blade, bow, and armor.");
                bp.m_ReplaceSpellbook = MyrmidarchSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(4, MagusSpellRecallFeature),
                    Helpers.CreateLevelEntry(6, MagusArcanaSelection),
                    Helpers.CreateLevelEntry(8, ImprovedSpellCombat),
                    Helpers.CreateLevelEntry(10, FighterTraining),
                    Helpers.CreateLevelEntry(11, MagusImprovedSpellRecallFeature),
                    Helpers.CreateLevelEntry(12, MagusArcanaSelection),
                    Helpers.CreateLevelEntry(14, GreaterSpellCombat),
                    Helpers.CreateLevelEntry(18, MagusArcanaSelection),
                    Helpers.CreateLevelEntry(20, TrueMagusFeature),
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(4, EldritchArcherRangedSpellStrike),
                    Helpers.CreateLevelEntry(6, WeaponTrainingSelection),
                    Helpers.CreateLevelEntry(7, MyrmidarchFighterTraining),
                    Helpers.CreateLevelEntry(8, ArmorTraining),
                    Helpers.CreateLevelEntry(10, MyrmidarchFighterTrainingUpgrade),
                    Helpers.CreateLevelEntry(12, WeaponTrainingSelection, WeaponTrainingRankUpSelection),
                    Helpers.CreateLevelEntry(14, ArmorTrainingSelection),
                    Helpers.CreateLevelEntry(18, WeaponTrainingSelection, WeaponTrainingRankUpSelection),
                    Helpers.CreateLevelEntry(20, ArmorMastery),
                };
            });
            /*
            var MysticTheurgeChannelerOfTheUnknownLevelUp = Helpers.CreateBlueprint<BlueprintFeature>("MysticTheurgeChannelerOfTheUnknownLevelUp", bp => {
                bp.SetName("Channeler Of The Unknown");
                bp.SetDescription("At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 10;
                bp.HideInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.AddComponent<AddSpellbookLevel>(c => {
                    c.m_Spellbook = ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>();
                });
            });
            var MysticTheurgeChannelerOfTheUnknownProgression = Helpers.CreateBlueprint<BlueprintProgression>("MysticTheurgeChannelerOfTheUnknownProgression", bp => {
                bp.SetName("Channeler Of The Unknown");
                bp.SetDescription("At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>()
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(2, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(3, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(4, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(5, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(6, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(7, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(8, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(9, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(10, MysticTheurgeChannelerOfTheUnknownLevelUp)
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                });
                bp.AddComponent<MysticTheurgeSpellbook>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_MysticTheurge = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>();
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });

            var HellknightSigniferChannelerOfTheUnknownLevelUp = Helpers.CreateBlueprint<BlueprintFeature>("HellknightSigniferChannelerOfTheUnknownLevelUp", bp => {
                bp.SetName("Channeler Of The Unknown");
                bp.SetDescription("At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 10;
                bp.HideInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.AddComponent<AddSpellbookLevel>(c => {
                    c.m_Spellbook = ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>();
                });
            });
            var HellknightSigniferChannelerOfTheUnknownProgression = Helpers.CreateBlueprint<BlueprintProgression>("HellknightSigniferChannelerOfTheUnknownProgression", bp => {
                bp.SetName("Channeler Of The Unknown");
                bp.SetDescription("At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = HellknightSigniferClass.ToReference<BlueprintCharacterClassReference>()
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(2, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(3, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(4, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(5, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(6, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(7, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(8, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(9, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(10, HellknightSigniferChannelerOfTheUnknownLevelUp)
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                });
                bp.AddComponent<MysticTheurgeSpellbook>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_MysticTheurge = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>();
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
            */

            //if (ModSettings.AddedContent.Archetypes.IsDisabled("ChannelerOfTheUnknown")) { return; }
            //MagusClass.m_Archetypes = MagusClass.m_Archetypes.AppendToArray(MyrmidarchArchetype.ToReference<BlueprintArchetypeReference>());

            /*
            MagusClass.Progression.UIGroups.Where(group => group.m_Features.Contains(ArcaneMediumArmor)).ForEach(group => {
                group.m_Features.Add(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                group.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                group.m_Features.Add(ArmorMastery.ToReference<BlueprintFeatureBaseReference>());
            });
            */
            MagusClass.Progression.UIGroups = MagusClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(WeaponTrainingSelection),
                //Helpers.CreateUIGroup(WeaponTrainingRankUpSelection),
                Helpers.CreateUIGroup(ArmorTraining, ArmorTrainingSelection, ArmorMastery),
                Helpers.CreateUIGroup(MyrmidarchFighterTraining, MyrmidarchFighterTrainingUpgrade)
            );

            //Main.LogPatch("Added", MyrmidarchArchetype);

            /*
            // Add to Mystic Theurge
            MysticTheurgeClericProgression.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
            });
            MysticTheurgeDivineSpellbookSelection.AddFeatures(MysticTheurgeChannelerOfTheUnknownProgression);
            Main.LogPatch("Patched", MysticTheurgeClericProgression);
            Main.LogPatch("Patched", MysticTheurgeDivineSpellbookSelection);
            // Add to Hellknight Signifier
            HellknightSigniferClericProgression.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
            });
            HellknightSigniferSpellbook.AddFeatures(HellknightSigniferChannelerOfTheUnknownProgression);
            Main.LogPatch("Patched", HellknightSigniferClericProgression);
            Main.LogPatch("Patched", HellknightSigniferSpellbook);
            // Enable Angel Merge
            AngelIncorporateSpellbook.m_AllowedSpellbooks = AngelIncorporateSpellbook.m_AllowedSpellbooks.AppendToArray(ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>());
            Main.LogPatch("Patched", AngelIncorporateSpellbook);
            */
        }
    }
}
