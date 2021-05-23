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
                ArcanistExploits.QuickStudy.AddQuickStudy();
                ArcanistExploits.ItemCrafting.AddItemCrafting();
                ArcanistExploits.MetamagicKnowledge.AddMetamagicKnowledge();
                ArcanistExploits.Familiar.AddFamiliar();
                Features.AeonBaneIncreaseResourceFeature.AddAeonBaneIncreaseResourceFeature();
                Features.InstinctualWarriorACBonusUnlock.AddInstinctualWarriorACBonusUnlock();
                Features.CavalierMounts.AddCavalierMountFeatureWolf();
                Features.PrimalistRagePowerSelection.AddPrimalistRagePowerSelection();
                Bloodlines.BloodlineRequisiteFeature.AddBloodlineRequisiteFeature();
                Bloodlines.AberrantBloodline.AddBloodragerAberrantBloodline();
                Bloodlines.AberrantBloodline.AddSorcererAberrantBloodline();
                Bloodlines.DestinedBloodline.AddBloodragerDestinedBloodline();
                Bloodlines.DestinedBloodline.AddSorcererDestinedBloodline();
                Bloodlines.AbyssalBloodline.AddBloodragerAbyssalDemonicBulkEnlargeBuff();
                Archetypes.CauldronWitch.AddCauldrenWitch();
                Archetypes.ElementalMaster.AddElementalMaster();
                Races.Dwarf.AddDwarfHeritage();
                Races.Elf.AddElfHeritage();
                Races.Gnome.AddGnomeHeritage();
                Races.Halfling.AddHalflingHeritage();
                Spells.LongArms.AddLongArms();
                Spells.ShadowEnchantment.AddShadowEnchantment();
                Spells.ShadowEnchantment.AddShadowEnchantmentGreater();
                MythicAbilities.ImpossibleSpeed.AddImpossibleSpeed();
                MythicAbilities.ArmorMaster.AddArmorMaster();
                MythicAbilities.ArmoredMight.AddArmoredMight();
            }
        }
    }
}
