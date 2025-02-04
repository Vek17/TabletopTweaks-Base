using HarmonyLib;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent {
    class ContentAdder {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            [PatchBlueprintsCacheInitPriority(Priority.First)]
            [PatchBlueprintsCacheInitPostfix]
            static void CreateNewBlueprints() {
                var test = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c773973cd73d4cd7aa4ccf3868dfeba9");
                test.TemporaryContext(bp => {
                    bp.SetComponents();
                    TTTContext.Logger.LogPatch(bp);
                });
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Loading New Content");
                BaseAbilities.OneHandedToggleAbility.AddOneHandedToggle();

                Features.MartialWeaponProficencySelection.AddMartialWeaponProficencySelection();
                Features.AgeEffects.AddAgeEffects();
                //Features.NauseatedPoision.AddNauseatedPoision();

                Spells.LongArms.AddLongArms();
                Spells.ShadowEnchantment.AddShadowEnchantment();
                Spells.ShadowEnchantment.AddShadowEnchantmentGreater();
                Spells.MagicalTailSpells.AddNewMagicalTailSpells();
                Spells.MagesDisjunction.AddMagesDisjunction();
                Spells.StunningBarrierGreater.AddStunningBarrierGreater();
                Spells.CloakOfWinds.AddCloakOfWinds();
                Spells.WebBolt.AddWebBolt();
                Spells.InflictPain.AddInflictPain();
                Spells.AccursedGlare.AddAccursedGlare();
                Spells.AgeResistance.AddAgeResistance();
                Spells.SandsOfTime.AddSandsOfTime();
                Spells.SpellCurse.AddSpellCurse();

                //Added early as some things depend on them for selections
                Feats.MetamagicFeats.IntensifiedSpell.AddIntensifiedSpell();
                Feats.MetamagicFeats.RimeSpell.AddRimeSpell();
                Feats.MetamagicFeats.BurningSpell.AddBurningSpell();
                Feats.MetamagicFeats.FlaringSpell.AddFlaringSpell();
                Feats.MetamagicFeats.PiercingSpell.AddPiercingSpell();
                Feats.MetamagicFeats.SolidShadows.AddSolidShadows();
                Feats.MetamagicFeats.EncouragingSpell.AddEncouragingSpell();
                Feats.MetamagicFeats.ElementalSpell.AddElementalSpell();

                Templates.AlignmentTemplates.AddCelestialTemplate();
                Templates.AlignmentTemplates.AddEntropicTemplate();
                Templates.AlignmentTemplates.AddFiendishTemplate();
                Templates.AlignmentTemplates.AddResoluteTemplate();

                WeaponEnchantments.NonStackingTempEnchantments.AddWeaponEnhancements();
                WeaponEnchantments.NonStackingTempEnchantments.AddArmorEnhancements();
                WeaponEnchantments.TwoHandedDamageMultiplier.AddTwoHandedDamageMultiplierEnchantment();
                WeaponEnchantments.TerrifyingTremble.AddTerrifyingTrembleEnchant();

                Races.Dwarf.AddDwarfHeritage();
                Races.Elf.AddElfHeritage();
                Races.Gnome.AddGnomeHeritage();
                Races.Halfling.AddHalflingHeritage();

                Backgrounds.Lecturer.AddLecturer();
                Backgrounds.Researcher.AddResearcher();

                RagePowers.GreaterAnimalFury.AddGreaterAnimalFury();

                RogueTalents.EmboldeningStrike.AddEmboldeningStrike();
                RogueTalents.BleedingAttack.AddBleedingAttack();

                ArcanistExploits.QuickStudy.AddQuickStudy();
                ArcanistExploits.ItemCrafting.AddItemCrafting();
                ArcanistExploits.MetamagicKnowledge.AddMetamagicKnowledge();
                ArcanistExploits.Familiar.AddFamiliar();

                MagusArcana.SpellBlending.AddSpellBlending();
                MagusArcana.BroadStudy.AddBroadStudy();

                WizardArcaneDiscoveries.ArcaneDiscoverySelection.AddArcaneDiscoverySelection();
                WizardArcaneDiscoveries.AlchemicalAffinity.AddAlchemicalAffinity();
                WizardArcaneDiscoveries.Idealize.AddIdealize();
                WizardArcaneDiscoveries.KnowledgeIsPower.AddKnowledgeIsPower();
                WizardArcaneDiscoveries.OppositionResearch.AddOppositionResearch();
                WizardArcaneDiscoveries.YuelralsBlessing.AddYuelralsBlessing();

                Hexes.Cauldron.AddCauldron();
                Hexes.IceTomb.AddIceTomb();
                Hexes.Retribution.AddRetribution();
                Hexes.Withering.AddWithering();
                Hexes.DireProphecy.AddDireProphecy();

                Features.PrimalistRagePowerSelection.AddPrimalistRagePowerSelection();
                Features.SpearChargeBuff.AddSpearChargeBuff();
                Features.PerfectStrikeZenArcherBuff.AddPerfectStrikeZenArcherBuff();
                Features.DragonDiscipleSpellbooks.AddDragonDiscipleSpellbooks();
                Features.FighterTrainingFakeLevel.AddFighterTrainingFakeLevel();

                FighterAdvancedWeaponTrainings.AdvancedWeapontrainingSelection.AddAdvancedWeaponTrainingSelection();
                FighterAdvancedWeaponTrainings.DefensiveWeaponTraining.AddDefensiveWeaponTraining();
                FighterAdvancedWeaponTrainings.FocusedWeapon.AddFocusedWeapon();
                FighterAdvancedWeaponTrainings.TrainedThrow.AddTrainedThrow();
                FighterAdvancedWeaponTrainings.TrainedGrace.AddTrainedGrace();
                FighterAdvancedWeaponTrainings.WarriorSpirit.AddWarriorSpirit();

                FighterAdvancedArmorTrainings.AdvancedArmorTraining.AddAdvancedArmorTraining();
                FighterAdvancedArmorTrainings.ArmoredConfidence.AddArmoredConfidence();
                FighterAdvancedArmorTrainings.ArmoredJuggernaut.AddArmoredJuggernaut();
                FighterAdvancedArmorTrainings.ArmorSpecialization.AddArmorSpecialization();
                FighterAdvancedArmorTrainings.CriticalDeflection.AddCriticalDeflection();
                FighterAdvancedArmorTrainings.SteelHeadbutt.AddSteelHeadbutt();

                Feats.ArmorMastery.ArmorMastery.AddArmorMasterySelection();
                Feats.ArmorMastery.SprightlyArmor.AddSprightlyArmor();
                Feats.ArmorMastery.IntenseBlows.AddIntenseBlows();
                Feats.ArmorMastery.KnockingBlows.AddKnockingBlows();
                Feats.ArmorMastery.SecuredArmor.AddSecuredArmor();

                Feats.ShieldMastery.ShieldMastery.AddShieldMasterySelection();
                Feats.ShieldMastery.DefendedMovement.AddDefendedMovement();
                Feats.ShieldMastery.StumblingBash.AddStumblingBash();
                Feats.ShieldMastery.TopplingBash.AddTopplingBash();
                Feats.ShieldMastery.TowerShieldSpecialist.AddTowerShieldSpecialist();

                Bloodlines.BloodlineRequisiteFeature.AddBloodlineRequisiteFeature();
                Bloodlines.AberrantBloodline.AddBloodragerAberrantBloodline();
                Bloodlines.AberrantBloodline.AddSorcererAberrantBloodline();
                Bloodlines.DestinedBloodline.AddBloodragerDestinedBloodline();
                Bloodlines.DestinedBloodline.AddSorcererDestinedBloodline();
                Bloodlines.AbyssalBloodline.AddBloodragerAbyssalDemonicBulkEnlargeBuff();
                Bloodlines.BloodragerArcaneBloodline.AddArcaneBloodrageReworkToggles();

                //Features to support existing clases
                Classes.Cavalier.AddCavalierFeatures();
                Classes.Oracle.AddOracleFeatures();
                Classes.Magus.AddMagusFeatures();
                Classes.Skald.AddSkaldFeatures();
                Classes.Monk.AddMonkFeatures();
                Classes.WinterWitch.AddWinterWitchFeatures();
                Classes.Barbarian.AddBarbarianFeatures();
                Classes.Aeon.AddAeonFeatures();
                //Features to support existing archetypes
                Archetypes.MadDog.AddMadDogFeatures();
                //New archetypes
                Archetypes.BladeBound.AddBlackBlade(); //Comes before all archetypes that use black blade
                Archetypes.BladeBound.AddBladeBound();
                Archetypes.BladeAdept.AddBladeAdept();
                Archetypes.CauldronWitch.AddCauldrenWitch();
                Archetypes.ElementalMaster.AddElementalMaster();
                Archetypes.MetamagicRager.AddMetamagicRager();
                Archetypes.DivineCommander.AddDivineCommander();
                Archetypes.NatureFang.AddNatureFang();
                Archetypes.ChannelerOfTheUnknown.AddChannelerOfTheUnknown();
                Archetypes.Myrmidarch.AddMyrmidarch();
                Archetypes.HolyBeast.AddHolyBeast();
                //Features to support existing prestige clases
                Classes.Loremaster.AddLoremasterFeatures();

                MythicAbilities.ImpossibleSpeed.AddImpossibleSpeed();
                MythicAbilities.ArmorMaster.AddArmorMaster();
                MythicAbilities.ArmoredMight.AddArmoredMight();
                MythicAbilities.MountedManiac.AddMountedManiac();
                MythicAbilities.MythicSpellCombat.AddMythicSpellCombat();
                MythicAbilities.PrecisionCritical.AddPrecisionCritical();
                MythicAbilities.AbundantBlessing.AddAbundantBlessing();
                MythicAbilities.AbundantBombs.AddAbundantBombs();
                MythicAbilities.AbundantChallenge.AddAbundantChallenge();
                MythicAbilities.AbundantFervor.AddAbundantFervor();
                MythicAbilities.AbundantIncense.AddAbundantIncense();
                MythicAbilities.AbundantLayOnHands.AddAbundantLayOnHands();
                MythicAbilities.HarmoniousMage.AddHarmoniousMage();
                MythicAbilities.SecondPatron.AddSecondPatron();
                MythicAbilities.EnhancedBlessings.AddEnhancedBlessings();
                MythicAbilities.ImpossibleBlessing.AddImpossibleBlessing();
                MythicAbilities.AbundantSpellKenning.AddAbundantSpellKenning();
                MythicAbilities.SecondOrder.AddSecondOrder();
                MythicAbilities.MythicBanner.AddMythicBanner();
                MythicAbilities.MaximizedCritical.AddMaximizedCritical();
                MythicAbilities.MythicBond.AddMythicBond();
                MythicAbilities.EldritchBreach.AddEldritchBreach();
                MythicAbilities.ElementalBond.AddElementalBond();
                MythicAbilities.EnergyConversion.AddEnergyConversion();
                MythicAbilities.AdamantineMind.AddAdamantineMind();
                MythicAbilities.ArcaneMetamastery.AddArcaneMetamastery();

                MythicAbilities.FavoriteMetamagicPersistent.AddFavoriteMetamagicPersistent();
                MythicAbilities.FavoriteMetamagicSelective.AddFavoriteMetamagicSelective();

                Feats.ShatterDefenses.AddNewShatterDefenseBlueprints();
                Feats.MagicalAptitude.AddMagicalAptitude();
                Feats.Scholar.AddScholar();
                Feats.SelfSufficient.AddSelfSufficient();
                Feats.ShingleRunner.AddShingleRunner();
                Feats.StreetSmarts.AddStreetSmarts();
                Feats.GracefulAthlete.AddGracefulAthlete();
                Feats.DervishDance.AddDervishDance();
                Feats.NatureSoul.AddNatureSoul();
                Feats.AnimalAlly.AddAnimalAlly();
                Feats.SpellSpecializationGreater.AddSpellSpecializationGreater();
                Feats.Stalwart.AddStalwart();
                Feats.CelestialServant.AddCelestialServant();
                Feats.ImprovedChannel.AddImprovedChannel();
                Feats.QuickChannel.AddQuickChannel();
                Feats.ErastilsBlessing.AddErastilsBlessing();
                Feats.QuickDraw.AddQuickDraw();
                Feats.UndersizedMount.AddUndersizedMount();
                Feats.TrickRiding.AddTrickRiding();
                Feats.MountedSkirmisher.AddMountedSkirmisher();
                Feats.LungingSpellTouch.AddLungingSpellTouch();
                Feats.HorseMaster.AddHorseMaster();
                Feats.DispelFocus.AddDispelFocus();
                Feats.TwoWeaponDefense.AddTwoWeaponDefense();
                Feats.VarisianTattoo.AddVarisianTattoo();
                Feats.QuickenBlessing.AddQuickenBlessing();
                Feats.RivingStrike.AddRivingStrike();
                Feats.ExpandedSpellKenning.AddExpandedSpellKenning();
                Feats.MantisStyle.AddMantisStyle();
                Feats.AbilityFocusStunningFist.AddAbilityFocusStunningFist();
                Feats.ChainChallenge.AddChainChallenge();
                Feats.MutatedShape.AddMutatedShape();
                Feats.AccursedHex.AddAccursedHex();
                Feats.SplitHex.AddSplitHex();
                Feats.ImprovedNaturalArmor.AddImprovedNaturalArmor();
                Feats.ImprovedNaturalAttack.AddImprovedNaturalAttack();
                Feats.MagicTrick.AddMagicTrick();

                Feats.ExtraReservoir.AddExtraReservoir();
                Feats.ExtraHex.AddExtraHex();
                Feats.ExtraArcanistExploit.AddExtraArcanistExploit();
                Feats.ExtraArcana.AddExtraArcana();
                Feats.ExtraKi.AddExtraKi();
                Feats.ExtraRogueTalent.AddExtraRogueTalent();
                Feats.ExtraSlayerTalent.AddExtraSlayerTalent();
                Feats.ExtraRevelation.AddExtraRevelation();
                Feats.ExtraDiscovery.AddExtraDiscovery();
                Feats.ExtraMercy.AddExtraMercy();

                MythicFeats.MythicShatterDefenses.AddMythicShatterDefenses();
                MythicFeats.MythicCombatReflexes.AddMythicCombatReflexes();
                MythicFeats.MythicWarriorPriest.AddMythicWarriorPriest();
                MythicFeats.MythicIntimidatingProwess.AddMythicIntimidatingProwess();
                MythicFeats.TitanStrike.AddTitanStrike();
                MythicFeats.MythicTwoWeaponDefense.AddMythicTwoWeaponDefense();
                MythicFeats.MythicManyshot.AddMythicManyshot();
                MythicFeats.MythicCombatExpertise.AddMythicCombatExpertise();
                MythicFeats.MythicCriticalFocus.AddMythicCriticalFocus();
                MythicFeats.MythicCleave.AddMythicCleave();
                MythicFeats.BewitchingReflex.AddBewitchingReflex();
                MythicAbilities.ExtraMythicFeat.AddExtraMythicFeat();

                AlternateCapstones.Generic.AddAlternateCapstones();
                AlternateCapstones.Alchemist.AddAlternateCapstones();
                AlternateCapstones.Arcanist.AddAlternateCapstones();
                AlternateCapstones.Barbarian.AddAlternateCapstones();
                AlternateCapstones.Bard.AddAlternateCapstones();
                AlternateCapstones.Bloodrager.AddAlternateCapstones();
                AlternateCapstones.Cavalier.AddAlternateCapstones();
                AlternateCapstones.Cleric.AddAlternateCapstones();
                AlternateCapstones.Druid.AddAlternateCapstones();
                AlternateCapstones.Fighter.AddAlternateCapstones();
                AlternateCapstones.Hunter.AddAlternateCapstones();
                AlternateCapstones.Inquisitor.AddAlternateCapstones();
                AlternateCapstones.Kineticist.AddAlternateCapstones();
                AlternateCapstones.Magus.AddAlternateCapstones();
                AlternateCapstones.Monk.AddAlternateCapstones();
                AlternateCapstones.Oracle.AddAlternateCapstones();
                AlternateCapstones.Paladin.AddAlternateCapstones();
                AlternateCapstones.Ranger.AddAlternateCapstones();
                AlternateCapstones.Rogue.AddAlternateCapstones();
                AlternateCapstones.Shaman.AddAlternateCapstones();
                AlternateCapstones.Shifter.AddAlternateCapstones();
                AlternateCapstones.Skald.AddAlternateCapstones();
                AlternateCapstones.Slayer.AddAlternateCapstones();
                AlternateCapstones.Sorcerer.AddAlternateCapstones();
                AlternateCapstones.Warpriest.AddAlternateCapstones();
                AlternateCapstones.Witch.AddAlternateCapstones();
                AlternateCapstones.Wizard.AddAlternateCapstones();
            }
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            [PatchBlueprintsCacheInitPostfix]
            static void ApplyNewMetamagics() {
                Feats.MetamagicFeats.BurningSpell.UpdateSpells();
                Feats.MetamagicFeats.EncouragingSpell.UpdateSpells();
                Feats.MetamagicFeats.FlaringSpell.UpdateSpells();
                Feats.MetamagicFeats.IntensifiedSpell.UpdateSpells();
                Feats.MetamagicFeats.PiercingSpell.UpdateSpells();
                Feats.MetamagicFeats.RimeSpell.UpdateSpells();
                Feats.MetamagicFeats.SolidShadows.UpdateSpells();
                Feats.MetamagicFeats.ElementalSpell.UpdateSpells();
            }
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            [PatchBlueprintsCacheInitPostfix]
            static void ApplyLateSelections() {
                AlternateCapstones.Inquisitor.UpdateTeamworkFeats();
            }
        }
    }
}
