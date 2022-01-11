using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Sorcerer {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Sorcerer");

                PatchBase();
                PatchCrossblooded();
            }

            static void PatchBase() {
                PatchDraconicBloodlineDescriptions();

                void PatchDraconicBloodlineDescriptions() {
                    if (ModSettings.Fixes.Sorcerer.Base.IsDisabled("DraconicBloodlineDescriptions")) { return; }

                    var BloodlineDraconicBrassArcana = Resources.GetBlueprint<BlueprintFeature>("153e9b6b5b0f34d45ae8e815838aca80");
                    var BloodlineDraconicRedArcana = Resources.GetBlueprint<BlueprintFeature>("a8baee8eb681d53438cc17bd1d125890");
                    var BloodlineDraconicGoldArcana = Resources.GetBlueprint<BlueprintFeature>("ac04aa27a6fd8b4409b024a6544c4928");

                    var BloodlineDraconicBlackArcana = Resources.GetBlueprint<BlueprintFeature>("5515ae09c952ae2449410ab3680462ed");
                    var BloodlineDraconicCopperArcana = Resources.GetBlueprint<BlueprintFeature>("2a8ed839d57f31a4983041645f5832e2");
                    var BloodlineDraconicGreenArcana = Resources.GetBlueprint<BlueprintFeature>("caebe2fa3b5a94d4bbc19ccca86d1d6f");

                    var BloodlineDraconicSilverArcana = Resources.GetBlueprint<BlueprintFeature>("1af96d3ab792e3048b5e0ca47f3a524b");
                    var BloodlineDraconicWhiteArcana = Resources.GetBlueprint<BlueprintFeature>("456e305ebfec3204683b72a45467d87c");

                    var BloodlineDraconicBlueArcana = Resources.GetBlueprint<BlueprintFeature>("0f0cb88a2ccc0814aa64c41fd251e84e");
                    var BloodlineDraconicBronzeArcana = Resources.GetBlueprint<BlueprintFeature>("677ae97f60d26474bbc24a50520f9424");

                    PatchDescription(BloodlineDraconicBrassArcana, "fire");
                    PatchDescription(BloodlineDraconicBrassArcana, "fire");
                    PatchDescription(BloodlineDraconicBrassArcana, "fire");

                    PatchDescription(BloodlineDraconicBlackArcana, "acid");
                    PatchDescription(BloodlineDraconicCopperArcana, "acid");
                    PatchDescription(BloodlineDraconicGreenArcana, "acid");

                    PatchDescription(BloodlineDraconicSilverArcana, "cold");
                    PatchDescription(BloodlineDraconicWhiteArcana, "cold");

                    PatchDescription(BloodlineDraconicBlueArcana, "electricity");
                    PatchDescription(BloodlineDraconicBronzeArcana, "electricity");

                    void PatchDescription(BlueprintFeature arcana, string descriptor) {
                        arcana.SetDescription($"Whenever you cast a spell with the {descriptor} descriptor, that spell deals +1 point of damage per die rolled.");
                    }
                }
            }

            static void PatchCrossblooded() {
                PatchDrawbacks();

                void PatchDrawbacks() {
                    if (ModSettings.Fixes.Sorcerer.Archetypes["Crossblooded"].IsDisabled("Drawbacks")) { return; }

                    var CrossbloodedDrawbacks = Resources.GetBlueprint<BlueprintFeature>("f02fd748fecb4cc2a4d7d282c6b3de46");
                    CrossbloodedDrawbacks.SetName("Crossblooded Drawbacks");
                    CrossbloodedDrawbacks.SetDescription("A crossblooded sorcerer has one fewer spell known at each level than regular sorcerer.\n" +
                        "Furthermore, the conflicting urges created by the divergent nature of the crossblooded sorcerer’s dual heritage forces " +
                        "her to constantly take some mental effort just to remain focused on her current situation and needs. This leaves her " +
                        "with less mental resolve to deal with external threats. A crossblooded sorcerer always takes a -2 penalty on Will saves.");
                    CrossbloodedDrawbacks.AddComponent<AddStatBonus>(c => {
                        c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;
                        c.Value = -2;
                    });

                    Main.LogPatch("Patched", CrossbloodedDrawbacks);
                }
            }
        }
    }
}
