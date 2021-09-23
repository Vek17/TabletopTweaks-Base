using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Abilities {
    static class BaseAbilities {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Base Abilities");
                FixCoupDeGrace();

                static void FixCoupDeGrace() {
                    if (ModSettings.Fixes.BaseFixes.IsDisabled("CoupDeGrace")) { return; }

                    var CoupDeGraceAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("32280b137ca642c45be17e2d92898758");
                    CoupDeGraceAbility.SetDescription("As a full-round action, you can use a melee weapon to deliver a coup de grace " +
                        "to a helpless opponent.\nYou automatically hit and score a critical hit. If the defender survives the damage, he " +
                        "must make a Fortitude save DC 10 + damage dealt) or die. A rogue also gets her extra sneak attack damage against " +
                        "a helpless opponent when delivering a coup de grace.\nDelivering a coup de grace provokes attacks of opportunity " +
                        "from threatening opponents.\nYou can’t deliver a coup de grace against a creature that is immune to critical hits.");
                    CoupDeGraceAbility.ReplaceComponents<AbilityEffectRunAction>(
                        Helpers.Create<CoupDeGraceComponent>(c => {
                            c.Actions = Helpers.CreateActionList(new GameAction[] {
                                Helpers.Create<ContextActionProvokeAttackOfOpportunity>(a => a.ApplyToCaster = true)
                            });
                        })
                    );
                    Main.LogPatch("Patched", CoupDeGraceAbility);
                }
            }
        }
    }
}
