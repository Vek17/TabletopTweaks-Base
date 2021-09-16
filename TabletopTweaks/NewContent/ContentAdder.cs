using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace TabletopTweaks.NewContent {
    class ContentAdder {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Loading New Content");
                BaseAbilities.OneHandedToggleAbility.AddOneHandedToggle();

                WeaponEnchantments.NonStackingTempEnchantments.AddWeaponEnhancements();

                Races.Dwarf.AddDwarfHeritage();
                Races.Elf.AddElfHeritage();
                Races.Gnome.AddGnomeHeritage();
                Races.Halfling.AddHalflingHeritage();

                ArcanistExploits.QuickStudy.AddQuickStudy();
                ArcanistExploits.ItemCrafting.AddItemCrafting();
                ArcanistExploits.MetamagicKnowledge.AddMetamagicKnowledge();
                ArcanistExploits.Familiar.AddFamiliar();

                FighterAdvancedWeaponTrainings.AdvancedWeapontrainingSelection.AddAdvancedWeaponTrainingSelection();
                FighterAdvancedWeaponTrainings.DefensiveWeaponTraining.AddDefensiveWeaponTraining();
                FighterAdvancedWeaponTrainings.FocusedWeapon.AddFocusedWeapon();
                FighterAdvancedWeaponTrainings.TrainedThrow.AddTrainedThrow();
                FighterAdvancedWeaponTrainings.TrainedGrace.AddTrainedGrace();

                FighterAdvancedArmorTrainings.AdvancedArmorTraining.AddAdvancedArmorTraining();
                FighterAdvancedArmorTrainings.ArmoredConfidence.AddArmoredConfidence();
                FighterAdvancedArmorTrainings.ArmoredJuggernaut.AddArmoredJuggernaut();
                FighterAdvancedArmorTrainings.ArmorSpecialization.AddArmorSpecialization();
                FighterAdvancedArmorTrainings.CriticalDeflection.AddCriticalDeflection();
                FighterAdvancedArmorTrainings.SteelHeadbutt.AddSteelHeadbutt();

                Features.AeonBaneIncreaseResourceFeature.AddAeonBaneIncreaseResourceFeature();
                Features.CavalierMounts.AddCavalierMountFeatureWolf();
                Features.PrimalistRagePowerSelection.AddPrimalistRagePowerSelection();
                Features.LongspearChargeBuff.AddLongspearChargeBuff();
                Features.CavalierMobilityFeature.AddCavalierMobilityFeature();
                Features.PerfectStrikeZenArcherBuff.AddPerfectStrikeZenArcherBuff();

                Bloodlines.BloodlineRequisiteFeature.AddBloodlineRequisiteFeature();
                Bloodlines.AberrantBloodline.AddBloodragerAberrantBloodline();
                Bloodlines.AberrantBloodline.AddSorcererAberrantBloodline();
                Bloodlines.DestinedBloodline.AddBloodragerDestinedBloodline();
                Bloodlines.DestinedBloodline.AddSorcererDestinedBloodline();
                Bloodlines.AbyssalBloodline.AddBloodragerAbyssalDemonicBulkEnlargeBuff();

                Archetypes.CauldronWitch.AddCauldrenWitch();
                Archetypes.ElementalMaster.AddElementalMaster();
                Archetypes.MetamagicRager.AddMetamagicRager();

                Spells.LongArms.AddLongArms();
                Spells.ShadowEnchantment.AddShadowEnchantment();
                Spells.ShadowEnchantment.AddShadowEnchantmentGreater();

                MythicAbilities.ImpossibleSpeed.AddImpossibleSpeed();
                MythicAbilities.ArmorMaster.AddArmorMaster();
                MythicAbilities.ArmoredMight.AddArmoredMight();
                MythicAbilities.MountedManiac.AddMountedManiac();

                Feats.MagicalAptitude.AddMagicalAptitude();
                Feats.Scholar.AddScholar();
                Feats.SelfSufficient.AddSelfSufficient();
                Feats.ShingleRunner.AddShingleRunner();
                Feats.StreetSmarts.AddStreetSmarts();
                Feats.GracefulAthlete.AddGracefulAthlete();
                Feats.DervishDance.AddDervishDance();

                Feats.ExtraReservoir.AddExtraReservoir();
                Feats.ExtraHex.AddExtraHex();
                Feats.ExtraArcanistExploit.AddExtraArcanistExploit();
                Feats.ExtraArcana.AddExtraArcana();
                Feats.ExtraKi.AddExtraKi();
                Feats.ExtraRogueTalent.AddExtraRogueTalent();
                Feats.ExtraSlayerTalent.AddExtraSlayerTalent();
                Feats.ExtraRevelation.AddExtraRevelation();
                Feats.ExtraDiscovery.AddExtraDiscovery();

                AlternateCapstones.MasterfulTalent.AddMasterfulTalent();
            }
        }
    }
}
