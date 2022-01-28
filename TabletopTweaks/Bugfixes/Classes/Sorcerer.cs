using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
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
                PatchElementalBloodlineDescriptions();

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
                    PatchDescription(BloodlineDraconicRedArcana, "fire");
                    PatchDescription(BloodlineDraconicGoldArcana, "fire");

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
                void PatchElementalBloodlineDescriptions() {
                    if (ModSettings.Fixes.Sorcerer.Base.IsDisabled("PatchElementalBloodlineDescriptions")) { return; }

                    var BloodlineElementalAirArcana = Resources.GetBlueprint<BlueprintFeature>("54ae8876bb5d78242beec0752592a018");
                    var BloodlineElementalAirArcanaAbilily = Resources.GetBlueprint<BlueprintActivatableAbility>("5f6315dfeb74a564f96f460d72f7206c");
                    var BloodlineElementalAirArcanaBuff = Resources.GetBlueprint<BlueprintBuff>("3f5763ac8b4e080469f9a41adf3a16c3");

                    var BloodlineElementalEarthArcana = Resources.GetBlueprint<BlueprintFeature>("5282afee8f3dfda49a34e36c3cee9d2c");
                    var BloodlineElementalEarthArcanaAbilily = Resources.GetBlueprint<BlueprintActivatableAbility>("94ce51ed666fc8d42830aa9fe48897f9");
                    var BloodlineElementalEarthArcanaBuff = Resources.GetBlueprint<BlueprintBuff>("3d700f97e681b014e894d9ff9c972a83");

                    var BloodlineElementalFireArcana = Resources.GetBlueprint<BlueprintFeature>("c33b319082a7edc468d3eda248a527f3");
                    var BloodlineElementalFireArcanaAbilily = Resources.GetBlueprint<BlueprintActivatableAbility>("924dfcd481c0be54c959c2846b3fb7da");
                    var BloodlineElementalFireArcanaBuff = Resources.GetBlueprint<BlueprintBuff>("b3e3882ab6829e34983f31e989c00dfc");

                    var BloodlineElementalWaterArcana = Resources.GetBlueprint<BlueprintFeature>("68d7772fa2f03e247ad1676ddd5eb4e2");
                    var BloodlineElementalWaterArcanaAbilily = Resources.GetBlueprint<BlueprintActivatableAbility>("dd484f0706325de40aee5dba15fbce45");
                    var BloodlineElementalWaterArcanaBuff = Resources.GetBlueprint<BlueprintBuff>("912fbab5b3579e9409fcb0f750bb6f2b");

                    PatchArcana(BloodlineElementalAirArcana, BloodlineElementalAirArcanaAbilily, BloodlineElementalAirArcanaBuff);
                    PatchArcana(BloodlineElementalEarthArcana, BloodlineElementalEarthArcanaAbilily, BloodlineElementalEarthArcanaBuff);
                    PatchArcana(BloodlineElementalFireArcana, BloodlineElementalFireArcanaAbilily, BloodlineElementalFireArcanaBuff);
                    PatchArcana(BloodlineElementalWaterArcana, BloodlineElementalWaterArcanaAbilily, BloodlineElementalWaterArcanaBuff);

                    void PatchArcana(BlueprintFeature feature, BlueprintActivatableAbility ability, BlueprintBuff buff) {
                        feature.m_DisplayName = ability.m_DisplayName;
                        feature.SetDescription("Whenever you cast a spell that deals energy damage, " +
                            "you can change the type of damage to match the type of your bloodline. " +
                            "This also changes the spell’s type to match the type of your bloodline.");
                        feature.HideInUI = false;
                        feature.m_Icon = ability.Icon;

                        buff.m_Icon = ability.Icon;

                        ability.DeactivateImmediately = true;

                        Main.LogPatch(feature);
                        Main.LogPatch(buff);
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
