using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Cleric {
        [PatchBlueprintsCacheInit]
        static class Cleric_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Cleric")) { return; }

                var ClericAlternateCapstone = NewContent.AlternateCapstones.Cleric.ClericAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                ClassTools.Classes.ClericClass.TemporaryContext(bp => {
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(ClericAlternateCapstone));
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Domain_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.First)]
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
