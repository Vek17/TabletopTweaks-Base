using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;

namespace TabletopTweaks.Reworks {
    class MythicFeats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Reworking Mythic Feats");
                PatchMythicSneakAttack();
            }
            static void PatchMythicSneakAttack() {
                if (ModSettings.Homebrew.MythicFeats.IsDisabled("MythicSneakAttack")) { return; }

                var SneakAttackerMythicFeat = Resources.GetBlueprint<BlueprintFeature>("d0a53bf03b978634890e5ebab4a90ecb");

                SneakAttackerMythicFeat.RemoveComponents<AddStatBonus>();
                SneakAttackerMythicFeat.AddComponent<MythicSneakAttack>();
                SneakAttackerMythicFeat.SetDescription("Your sneak attacks are especially deadly.\n" +
                    "Benifit: Your sneak attack dice are one size larger than normal. " +
                    "For example if you would normally roll d6s for sneak attacks you would roll d8s instead.");
                Main.LogPatch("Patched", SneakAttackerMythicFeat);
            }
        }
    }
}
