using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using static Kingmaker.UI.GenericSlot.EquipSlotBase;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Demon {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Demon Resources");

                PatchBrimorakAspect();

                void PatchBrimorakAspect() {
                    if (ModSettings.Fixes.Demon.IsDisabled("BrimorakAspect")) { return; }

                    var BrimorakAspectEffectBuff = Resources.GetBlueprint<BlueprintBuff>("f154542e0b97908479a578dd7bf6d3f7");
                    var BrimorakAspectEffectProperty = Resources.GetBlueprintReference<BlueprintUnitPropertyReference>("d6a524d190f04a7ca3f920d2f96fa21b");

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

                    Main.LogPatch(BrimorakAspectEffectBuff);
                }
            }
        }
    }
}
