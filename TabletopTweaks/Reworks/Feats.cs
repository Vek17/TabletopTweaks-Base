using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewContent.MechanicsChanges;

namespace TabletopTweaks.Reworks {
    static class Feats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Reworking Feats");
                PatchBolsteredSpell();
            }
            static void PatchBolsteredSpell() {
                if (ModSettings.Homebrew.Feats.IsDisabled("BolsterSpell")) { return; }

                var BolsteredSpellFeat = Resources.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");

                BolsteredSpellFeat.RemoveComponents<AddStatBonus>();
                BolsteredSpellFeat.AddComponent<MythicSneakAttack>();
                BolsteredSpellFeat.SetDescription("You make your spells deal additional area of effect damage, while making them stronger.\n" +
                    "Benefit: Spell now deals 2 more points of damage per die rolled to all its targets. " +
                    "Additionally, all enemies in 5 feet of the spell targets are dealt 2 damage " +
                    "per die rolled of the original spell. The spell can no longer apply precision damage.\n" +
                    "Level Increase: +2 (A bolstered spell uses up a spell slot two levels higher than the spell's actual level.)");

                MetamagicExtention.RegisterMetamagic(
                    metamagic: Metamagic.Bolstered,
                    name: "",
                    icon: null,
                    defaultCost: 2,
                    favoriteMetamagic: null
                );
                Main.LogPatch("Patched", BolsteredSpellFeat);
            }
        }
    }
}
