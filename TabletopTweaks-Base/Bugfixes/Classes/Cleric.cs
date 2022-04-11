using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Cleric {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        [HarmonyPriority(Priority.First)]
        static class BlueprintsCache_Init_Domain_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Cleric");

                PatchBaseClass();
            }
            static void PatchBaseClass() {
                PatchGloryDomain();

                void PatchGloryDomain() {
                    if (TTTContext.Fixes.Cleric.Base.IsDisabled("GloryDomain")) { return; }

                    var GloryDomainBaseBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("55edcfff497a1e04a963f72c485da5cb");
                    GloryDomainBaseBuff.RemoveComponents<AddContextStatBonus>(component => component.Stat == StatType.Charisma);
                    GloryDomainBaseBuff.AddComponent<AbilityScoreCheckBonus>(c => {
                        c.Bonus = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus
                        };
                        c.Descriptor = ModifierDescriptor.UntypedStackable;
                        c.Stat = StatType.Charisma;
                    });
                    TTTContext.Logger.LogPatch("Patched", GloryDomainBaseBuff);
                }
            }
        }
    }
}
