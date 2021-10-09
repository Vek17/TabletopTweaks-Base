using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    static class ChannelerOfTheUnknown {
        private static readonly BlueprintFeatureSelection DeitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("59e7a76987fe3b547b9cce045f4db3e4");
        private static readonly BlueprintFeatureSelection MartialWeaponProficencySelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("MartialWeaponProficencySelection");
        private static readonly BlueprintFeatureSelection ExoticWeaponProficiencySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("9a01b6815d6c3684cb25f30b8bf20932");
        private static readonly BlueprintFeature LightArmorProficiency = Resources.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
        private static readonly BlueprintFeature MediumArmorProficiency = Resources.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");
        private static readonly BlueprintFeature SimpleWeaponProficiency = Resources.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd");
        private static readonly BlueprintFeature ShieldsProficiency = Resources.GetBlueprint<BlueprintFeature>("cb8686e7357a68c42bdd9d4e65334633");

        private static readonly BlueprintCharacterClass ClericClass = Resources.GetBlueprint<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0");
        private static readonly BlueprintSpellsTable CrusaderSpellLevels = Resources.GetBlueprint<BlueprintSpellsTable>("799265ebe0ed27641b6d415251943d03");
        private static readonly BlueprintFeature ClericProficiencies = Resources.GetBlueprint<BlueprintFeature>("8c971173613282844888dc20d572cfc9");
        private static readonly BlueprintFeatureSelection ChannelEnergySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("d332c1748445e8f4f9e92763123e31bd");
        private static readonly BlueprintFeatureSelection DomainsSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9");
        private static readonly BlueprintFeatureSelection SecondDomainsSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e");
        private static readonly BlueprintFeature ClericOrisonsFeature = Resources.GetBlueprint<BlueprintFeature>("e62f392949c24eb4b8fb2bc9db4345e3");
        private static readonly BlueprintFeature RayCalculateFeature = Resources.GetBlueprint<BlueprintFeature>("d3e6275cfa6e7a04b9213b7b292a011c");
        private static readonly BlueprintFeature TouchCalculateFeature = Resources.GetBlueprint<BlueprintFeature>("62ef1cdb90f1d654d996556669caf7fa");
        private static readonly BlueprintFeature FullCasterFeature = Resources.GetBlueprint<BlueprintFeature>("9fc9813f569e2e5448ddc435abf774b3");
        private static readonly BlueprintFeature DetectMagic = Resources.GetBlueprint<BlueprintFeature>("ee0b69e90bac14446a4cf9a050f87f2e");

        private static readonly BlueprintFeature DarknessDomainProgression = Resources.GetBlueprint<BlueprintFeature>("1e1b4128290b11a41ba55280ede90d7d");
        private static readonly BlueprintFeature DestructionDomainProgression = Resources.GetBlueprint<BlueprintFeature>("269ff0bf4596f5248864bc2653a2f0e0");
        private static readonly BlueprintFeature LuckDomainProgression = Resources.GetBlueprint<BlueprintFeature>("8bd8cfad69085654b9118534e4aa215e");
        private static readonly BlueprintFeature MadnessDomainProgression = Resources.GetBlueprint<BlueprintFeature>("9ebe166b9b901c746b1858029f13a2c5");

        public static void AddChannelerOfTheUnknown() {
            var ChannelerOfTheUnknownSpellLevels = Helpers.CreateBlueprint<BlueprintSpellsTable>("ChannelerOfTheUnknownSpellLevels", bp => {
                bp.Levels = CrusaderSpellLevels.Levels.Select(level => SpellTools.CreateSpellLevelEntry(level.Count)).ToArray();
            });
            var ChannelerOfTheUnknownSpellbook = Helpers.CreateBlueprint<BlueprintSpellbook>("ChannelerOfTheUnknownSpellbook", bp => {
                bp.Name = ClericClass.Spellbook.Name;
                bp.CastingAttribute = ClericClass.Spellbook.CastingAttribute;
                bp.AllSpellsKnown = ClericClass.Spellbook.AllSpellsKnown;
                bp.CantripsType = ClericClass.Spellbook.CantripsType;
                bp.HasSpecialSpellList = ClericClass.Spellbook.HasSpecialSpellList;
                bp.SpecialSpellListName = ClericClass.Spellbook.SpecialSpellListName;
                bp.m_SpellsPerDay = ChannelerOfTheUnknownSpellLevels.ToReference<BlueprintSpellsTableReference>();
                bp.m_SpellsKnown = ClericClass.Spellbook.m_SpellsKnown;
                bp.m_SpellSlots = ClericClass.Spellbook.m_SpellSlots;
                bp.m_SpellList = ClericClass.Spellbook.m_SpellList;
                bp.m_MythicSpellList = ClericClass.Spellbook.m_MythicSpellList;
                bp.m_CharacterClass = ClericClass.Spellbook.m_CharacterClass;
                bp.m_Overrides = ClericClass.Spellbook.m_Overrides;
                bp.AddComponent<CustomSpecialSlotAmount>(c => c.Amount = 2);
            });
            var ChannelerOfTheUnknownProficiencies = Helpers.CreateBlueprint<BlueprintFeature>("ChannelerOfTheUnknownProficiencies", bp => {
                bp.SetName("Channeler Of The Unknown Proficiencies");
                bp.SetDescription("Channelers Of The Unknown are proficient with all {g|Encyclopedia:Weapon_Proficiency}simple weapons{/g}, light armor, medium armor, and shields (except tower shields).");
                bp.m_Icon = ClericProficiencies.Icon;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
                        MediumArmorProficiency.ToReference<BlueprintUnitFactReference>(),
                        SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
                        ShieldsProficiency.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            var ChannelerOfTheUnknownWeaponProficiency = Helpers.CreateBlueprint<BlueprintFeatureSelection>("ChannelerOfTheUnknownWeaponProficiency", bp => {
                bp.SetName("Channeler Of The Unknown Weapon Proficiency");
                bp.SetDescription("A channeler of the unknown loses proficiency with her deity’s favored weapon. She instead gains proficiency with one martial or exotic weapon, " +
                    "chosen when she first takes this archetype, which thereafter effectively functions as her holy or unholy symbol for the purposes of class abilities and spellcasting. " +
                    "Once she makes this choice, she can’t later change it.");
                bp.m_Icon = ClericProficiencies.Icon;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddFeatures(MartialWeaponProficencySelection.AllFeatures.Select(feature => feature).ToArray());
                bp.AddFeatures(ExoticWeaponProficiencySelection.AllFeatures.Select(feature => feature).ToArray());
            });
            var ChannelerOfTheUnknownPowerOfTheUnknown = Helpers.CreateBlueprint<BlueprintFeatureSelection>("ChannelerOfTheUnknownPowerOfTheUnknown", bp => {
                bp.SetName("Power of the Unknown");
                bp.SetDescription("A channeler of the unknown has lost the benefit of the domains granted by her deity, but the unknown entity that answers her " +
                    "supplications instead grants her the benefits of one domain from the following list: Darkness, Destruction, Luck, Madness, or Void. Instead " +
                    "of a single domain spell slot, the channeler of the unknown gains two domain spell slots per spell level she can cast. A channeler of the " +
                    "unknown cannot select a subdomain in place of the domain available to her.");
                bp.m_Icon = DomainsSelection.Icon;
                bp.Group = FeatureGroup.Domain;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.AddFeatures(DarknessDomainProgression, DestructionDomainProgression, LuckDomainProgression, MadnessDomainProgression);
                bp.AddComponent<AddFacts>(c => {
                    // To support all features that check for domains this way
                    c.m_Facts = new BlueprintUnitFactReference[] { DomainsSelection.ToReference<BlueprintUnitFactReference>() };
                });
            });
            var ChannelerOfTheUnknownArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("ChannelerOfTheUnknownArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString("ChannelerOfTheUnknownArchetype.Name", "Channeler of the Unknown");
                bp.LocalizedDescription = Helpers.CreateString("ChannelerOfTheUnknownArchetype.Description", "While most clerics who fall out of favor with their deities " +
                    "simply lose their divine connection and the powers it granted, a few continue to go through the motions of prayer and obedience, persisting " +
                    "in the habits of faith even when their faith itself has faded. Among these, an even smaller number find that while their original deity no " +
                    "longer answers their prayers, something else does: an unknown entity or force of the universe channeling its power through a trained and " +
                    "practicing vessel.");
                bp.m_ReplaceSpellbook = ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        ClericProficiencies,
                        ChannelEnergySelection,
                        DomainsSelection,
                        SecondDomainsSelection
                    ),
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, 
                        ChannelerOfTheUnknownProficiencies,
                        ChannelerOfTheUnknownWeaponProficiency,
                        ChannelerOfTheUnknownPowerOfTheUnknown
                    ),
                };
            });
            if (ModSettings.AddedContent.Archetypes.IsDisabled("ChannelerOfTheUnknown")) { return; }
            ClericClass.m_Archetypes = ClericClass.m_Archetypes.AppendToArray(ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>());
            DeitySelection.AllFeatures.ForEach(deity => {
                var addFeature = deity.GetComponent<AddFeatureOnClassLevel>();
                if (addFeature != null) {
                    deity.AddComponent<AddFeatureOnClassLevelExclude>(c => {
                        c.m_Class = addFeature.m_Class;
                        c.m_AdditionalClasses = addFeature.m_AdditionalClasses;
                        c.m_Archetypes = addFeature.m_Archetypes;
                        c.m_ExcludeArchetypes = new BlueprintArchetypeReference[] { ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>() };
                        c.m_Feature = addFeature.m_Feature;
                        c.Level = addFeature.Level;
                        c.BeforeThisLevel = addFeature.BeforeThisLevel;
                    });
                    deity.RemoveComponent(addFeature);
                    Main.LogPatch("Patched", deity);
                }
            });
        }
    }
}
