using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Abilities {
    static class BaseAbilities {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Base Abilities");
                FixCoupDeGrace();

                static void FixCoupDeGrace() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("CoupDeGrace")) { return; }

                    var CoupDeGraceAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("32280b137ca642c45be17e2d92898758");
                    CoupDeGraceAbility.SetDescription(TTTContext, "As a full-round action, you can use a melee weapon to deliver a coup de grace " +
                        "to a helpless opponent.\nYou automatically hit and score a critical hit. If the defender survives the damage, he " +
                        "must make a Fortitude save (DC 10 + damage dealt) or die. A rogue also gets her extra sneak attack damage against " +
                        "a helpless opponent when delivering a coup de grace.\nDelivering a coup de grace provokes attacks of opportunity " +
                        "from threatening opponents.\nYou can’t deliver a coup de grace against a creature that is immune to critical hits.");
                    CoupDeGraceAbility.ReplaceComponents<AbilityEffectRunAction>(
                        Helpers.Create<CoupDeGraceTTT>(c => {
                            c.Actions = Helpers.CreateActionList(new GameAction[] {
                                Helpers.Create<ContextActionProvokeAttackOfOpportunity>(a => a.ApplyToCaster = true)
                            });
                        })
                    );
                    TTTContext.Logger.LogPatch("Patched", CoupDeGraceAbility);
                }
            }
        }
    }
}
