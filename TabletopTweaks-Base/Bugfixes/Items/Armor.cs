using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Items {
    static class Armor {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Armor");
                PatchHaramaki();
                PatchSingingSteel();
                PatchTempEnchantments();

                void PatchHaramaki() {
                    if (TTTContext.Fixes.Items.Armor.IsDisabled("Haramaki")) { return; }

                    var HaramakiType = BlueprintTools.GetBlueprint<BlueprintArmorType>("9511d62bcfc57c245bf64350a5933470");
                    HaramakiType.m_ProficiencyGroup = ArmorProficiencyGroup.Light;
                    TTTContext.Logger.LogPatch("Patched", HaramakiType);
                }
                void PatchTempEnchantments() {
                    var TemporaryArmorEnhancementBonus1 = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1d9b60d57afb45c4f9bb0a3c21bb3b98");
                    var TemporaryArmorEnhancementBonus2 = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("d45bfd838c541bb40bde7b0bf0e1b684");
                    var TemporaryArmorEnhancementBonus3 = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("51c51d841e9f16046a169729c13c4d4f");
                    var TemporaryArmorEnhancementBonus4 = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("a23bcee56c9fcf64d863dafedb369387");
                    var TemporaryArmorEnhancementBonus5 = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("15d7d6cbbf56bd744b37bbf9225ea83b");

                    TemporaryArmorEnhancementBonus1.TemporaryContext(bp => {
                        bp.RemoveComponents<AddStatBonusEquipment>();
                        bp.AddComponent<ArmorEnhancementBonus>(c => {
                            c.EnhancementValue = 1;
                        });
                    });
                    TemporaryArmorEnhancementBonus2.TemporaryContext(bp => {
                        bp.RemoveComponents<AddStatBonusEquipment>();
                        bp.AddComponent<ArmorEnhancementBonus>(c => {
                            c.EnhancementValue = 2;
                        });
                    });
                    TemporaryArmorEnhancementBonus3.TemporaryContext(bp => {
                        bp.RemoveComponents<AddStatBonusEquipment>();
                        bp.AddComponent<ArmorEnhancementBonus>(c => {
                            c.EnhancementValue = 3;
                        });
                    });
                    TemporaryArmorEnhancementBonus4.TemporaryContext(bp => {
                        bp.RemoveComponents<AddStatBonusEquipment>();
                        bp.AddComponent<ArmorEnhancementBonus>(c => {
                            c.EnhancementValue = 4;
                        });
                    });
                    TemporaryArmorEnhancementBonus5.TemporaryContext(bp => {
                        bp.RemoveComponents<AddStatBonusEquipment>();
                        bp.AddComponent<ArmorEnhancementBonus>(c => {
                            c.EnhancementValue = 5;
                        });
                    });
                    TTTContext.Logger.LogPatch(TemporaryArmorEnhancementBonus1);
                    TTTContext.Logger.LogPatch(TemporaryArmorEnhancementBonus2);
                    TTTContext.Logger.LogPatch(TemporaryArmorEnhancementBonus3);
                    TTTContext.Logger.LogPatch(TemporaryArmorEnhancementBonus4);
                    TTTContext.Logger.LogPatch(TemporaryArmorEnhancementBonus5);
                }
                void PatchSingingSteel() {
                    if (TTTContext.Fixes.Items.Armor.IsDisabled("SingingSteel")) { return; }

                    var SingingSteelFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("793af594fa45e6a47b89719dda7d5f7a");
                    var BreastplateType = BlueprintTools.GetBlueprintReference<BlueprintArmorTypeReference>("d326c3c61a84c6f40977c84fab41503d");
                    var SingingSteelBreastplatePlus5 = BlueprintTools.GetBlueprint<BlueprintItemArmor>("f8318f96333f4065baa2f04b1ba537eb");

                    SingingSteelFeature.TemporaryContext(bp => {
                        bp.AddComponent<AddMechanicsFeature>(c => {
                            c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.SingingSteel;
                        });
                    });
                    SingingSteelBreastplatePlus5.TemporaryContext(bp => {
                        bp.m_Type = BreastplateType;
                    });
                    TTTContext.Logger.LogPatch(SingingSteelFeature);
                    TTTContext.Logger.LogPatch(SingingSteelBreastplatePlus5);
                }
            }
        }
        [HarmonyPatch(typeof(ItemEntityArmor), nameof(ItemEntityArmor.CanBeEquippedInternal), new Type[] { typeof(UnitDescriptor) })]
        static class ItemEntityArmor_CanBeEquippedInternal_Haramaki_Patch {
            static BlueprintArmorType HaramakiType = BlueprintTools.GetBlueprint<BlueprintArmorType>("9511d62bcfc57c245bf64350a5933470");
            static void Postfix(ItemEntityArmor __instance, UnitDescriptor owner, ref bool __result) {
                if (TTTContext.Fixes.Items.Armor.IsDisabled("Haramaki")) { return; }
                if (__instance.Blueprint.Type == HaramakiType) { __result = true; }
            }
        }
    }
}
