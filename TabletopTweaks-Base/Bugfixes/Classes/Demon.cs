using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Demon {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Demon Resources");

                PatchBrimorakAspect();

                void PatchBrimorakAspect() {
                    if (TTTContext.Fixes.Demon.IsDisabled("BrimorakAspect")) { return; }

                    var BrimorakAspectEffectBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f154542e0b97908479a578dd7bf6d3f7");
                    var BrimorakAspectEffectProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("d6a524d190f04a7ca3f920d2f96fa21b");

                    BrimorakAspectEffectBuff.TemporaryContext(bp => {
                        bp.RemoveComponents<DraconicBloodlineArcana>();
                        bp.AddComponent<BonusDamagePerDie>(c => {
                            c.CheckDescriptor = false;
                            c.SpellsOnly = true;
                            c.UseContextBonus = true;
                            c.Value = new ContextValue() {
                                ValueType = ContextValueType.CasterCustomProperty,
                                m_CustomProperty = BrimorakAspectEffectProperty,
                                ValueRank = Kingmaker.Enums.AbilityRankType.StatBonus
                            };
                        });
                    });

                    TTTContext.Logger.LogPatch(BrimorakAspectEffectBuff);
                }
            }
        }
    }
}
