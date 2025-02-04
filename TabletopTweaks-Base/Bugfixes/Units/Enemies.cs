using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UI.GenericSlot.EquipSlotBase;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Units {
    static class Enemies {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Bosses");
                PatchAnomaly();
                PatchBalors();
            }
        }
        static void PatchAnomaly() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("Anomaly")) { return; }

            PatchAnomalyChaoticMind();

            void PatchAnomalyChaoticMind() {
                var AnomalyTemplateDefensive_ChaoticMindBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2159f35f1dfb4ee78da818f443a086ee");
                var AnomalyDistortionNormalDCProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("0cc67f363c944539bb09217f2ba3e149");

                AnomalyTemplateDefensive_ChaoticMindBuff.TemporaryContext(bp => {
                    var OriginalTrigger = bp.GetComponent<AddAbilityUseTargetTrigger>();
                    bp.RemoveComponents<AddAbilityUseTargetTrigger>();
                    bp.AddComponent<AddAbilityUseTargetTriggerTTT>(c => {
                        c.ToCaster = true;
                        c.DontCheckType = true;
                        c.CheckDescriptor = true;
                        c.SpellDescriptor = SpellDescriptor.MindAffecting;
                        c.TriggerOnEffectApply = true;
                        c.TriggerEvenIfNoEffect = true;
                        c.Action = OriginalTrigger.Action;
                    });
                    bp.AddComponent<AddSpellImmunity>(c => {
                        c.Type = SpellImmunityType.SpellDescriptor;
                        c.SpellDescriptor = SpellDescriptor.MindAffecting;
                    });
                    bp.FlattenAllActions().OfType<ContextActionSavingThrow>()?.ForEach(a => {
                        a.HasCustomDC = true;
                        a.CustomDC = new ContextValue() {
                            ValueType = ContextValueType.CasterCustomProperty,
                            m_CustomProperty = AnomalyDistortionNormalDCProperty
                        };
                    });
                });

            }
        }
        static void PatchBalors() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("Balors")) { return; }

            var BalorVorpalStrikeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("acc4a16c4088f2546b4237dcbb774f14");
            var BalorVorpalStrikeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5220bc4386bf3e147b1beb93b0b8b5e7");
            var Vorpal = BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("2f60bfcba52e48a479e4a69868e24ebc");

            BalorVorpalStrikeBuff.SetComponents();
            BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                c.m_EnchantmentBlueprint = Vorpal;
                c.Slot = SlotType.PrimaryHand;
            });
            BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                c.m_EnchantmentBlueprint = Vorpal;
                c.Slot = SlotType.SecondaryHand;
            });
            BalorVorpalStrikeFeature.AddComponent<RecalculateOnEquipmentChange>();

            TTTContext.Logger.LogPatch(BalorVorpalStrikeFeature);
            TTTContext.Logger.LogPatch(BalorVorpalStrikeBuff);
        }
    }
}
