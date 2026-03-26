using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Sorcerer {
        [PatchBlueprintsCacheInit]
        static class Sorcerer_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Sorcerer")) { return; }

                var SorcererAlternateCapstone = NewContent.AlternateCapstones.Sorcerer.SorcererAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();
                var BloodlineAscendance = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
                var BloodlineCapstoneSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "BloodlineCapstoneSelection");
                var MythicAbilitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
                var MythicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9ee0f6745f555484299b0a1563b99d81");
                var ExtraMythicAbilityMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");

                BloodlineAscendance.TemporaryContext(bp => {
                    bp.AllFeatures.ForEach(capstone => {
                        capstone.AddComponent<PrerequisiteInPlayerParty>(c => {
                            c.CheckInProgression = true;
                            c.HideInUI = true;
                            c.Not = true;
                            c.IgnoreLevelsBelow = 20;
                            c.m_BypassSelections = new BlueprintFeatureSelectionReference[] {
                                BloodlineCapstoneSelection.ToReference<BlueprintFeatureSelectionReference>(),
                                BloodlineAscendance.ToReference<BlueprintFeatureSelectionReference>(),
                                MythicAbilitySelection.ToReference<BlueprintFeatureSelectionReference>(),
                                BloodlineAscendance.ToReference<BlueprintFeatureSelectionReference>(),
                                BloodlineAscendance.ToReference<BlueprintFeatureSelectionReference>(),
                                MythicFeatSelection.ToReference<BlueprintFeatureSelectionReference>(),
                                ExtraMythicAbilityMythicFeat.ToReference<BlueprintFeatureSelectionReference>()
                            };
                        });
                        capstone.HideNotAvailibleInUI = true;
                    });
                    bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                        c.m_Feature = NewContent.AlternateCapstones.Sorcerer.SorcererAlternateCapstone.ToReference<BlueprintFeatureReference>();
                    });
                });

                ClassTools.Classes.SorcererClass.TemporaryContext(bp => {
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(SorcererAlternateCapstone));
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Sorcerer");

                PatchBase();
                PatchCrossblooded();
            }

            static void PatchBase() {
                PatchDraconicBloodlineDescriptions();
                PatchElementalBloodlineDescriptions();

                void PatchDraconicBloodlineDescriptions() {
                    if (TTTContext.Fixes.Sorcerer.Base.IsDisabled("DraconicBloodlineDescriptions")) { return; }

                    var BloodlineDraconicBrassArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("153e9b6b5b0f34d45ae8e815838aca80");
                    var BloodlineDraconicRedArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("a8baee8eb681d53438cc17bd1d125890");
                    var BloodlineDraconicGoldArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("ac04aa27a6fd8b4409b024a6544c4928");

                    var BloodlineDraconicBlackArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("5515ae09c952ae2449410ab3680462ed");
                    var BloodlineDraconicCopperArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("2a8ed839d57f31a4983041645f5832e2");
                    var BloodlineDraconicGreenArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("caebe2fa3b5a94d4bbc19ccca86d1d6f");

                    var BloodlineDraconicSilverArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("1af96d3ab792e3048b5e0ca47f3a524b");
                    var BloodlineDraconicWhiteArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("456e305ebfec3204683b72a45467d87c");

                    var BloodlineDraconicBlueArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("0f0cb88a2ccc0814aa64c41fd251e84e");
                    var BloodlineDraconicBronzeArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("677ae97f60d26474bbc24a50520f9424");

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
                        arcana.SetDescription(TTTContext, $"Whenever you cast a spell with the {descriptor} descriptor, that spell deals +1 point of damage per die rolled.");
                    }
                }
                void PatchElementalBloodlineDescriptions() {
                    if (TTTContext.Fixes.Sorcerer.Base.IsDisabled("PatchElementalBloodlineDescriptions")) { return; }

                    var BloodlineElementalAirArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("54ae8876bb5d78242beec0752592a018");
                    var BloodlineElementalAirArcanaAbilily = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5f6315dfeb74a564f96f460d72f7206c");
                    var BloodlineElementalAirArcanaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("3f5763ac8b4e080469f9a41adf3a16c3");

                    var BloodlineElementalEarthArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("5282afee8f3dfda49a34e36c3cee9d2c");
                    var BloodlineElementalEarthArcanaAbilily = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("94ce51ed666fc8d42830aa9fe48897f9");
                    var BloodlineElementalEarthArcanaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("3d700f97e681b014e894d9ff9c972a83");

                    var BloodlineElementalFireArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("c33b319082a7edc468d3eda248a527f3");
                    var BloodlineElementalFireArcanaAbilily = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("924dfcd481c0be54c959c2846b3fb7da");
                    var BloodlineElementalFireArcanaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b3e3882ab6829e34983f31e989c00dfc");

                    var BloodlineElementalWaterArcana = BlueprintTools.GetBlueprint<BlueprintFeature>("68d7772fa2f03e247ad1676ddd5eb4e2");
                    var BloodlineElementalWaterArcanaAbilily = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("dd484f0706325de40aee5dba15fbce45");
                    var BloodlineElementalWaterArcanaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("912fbab5b3579e9409fcb0f750bb6f2b");

                    PatchArcana(BloodlineElementalAirArcana, BloodlineElementalAirArcanaAbilily, BloodlineElementalAirArcanaBuff);
                    PatchArcana(BloodlineElementalEarthArcana, BloodlineElementalEarthArcanaAbilily, BloodlineElementalEarthArcanaBuff);
                    PatchArcana(BloodlineElementalFireArcana, BloodlineElementalFireArcanaAbilily, BloodlineElementalFireArcanaBuff);
                    PatchArcana(BloodlineElementalWaterArcana, BloodlineElementalWaterArcanaAbilily, BloodlineElementalWaterArcanaBuff);

                    void PatchArcana(BlueprintFeature feature, BlueprintActivatableAbility ability, BlueprintBuff buff) {
                        feature.m_DisplayName = ability.m_DisplayName;
                        feature.SetDescription(TTTContext, "Whenever you cast a spell that deals energy damage, " +
                            "you can change the type of damage to match the type of your bloodline. " +
                            "This also changes the spell’s type to match the type of your bloodline.");
                        feature.HideInUI = false;
                        feature.m_Icon = ability.Icon;

                        buff.m_Icon = ability.Icon;

                        ability.DeactivateImmediately = true;

                        TTTContext.Logger.LogPatch(feature);
                        TTTContext.Logger.LogPatch(buff);
                    }
                }
            }

            static void PatchCrossblooded() {
                PatchDrawbacks();

                void PatchDrawbacks() {
                    if (TTTContext.Fixes.Sorcerer.Archetypes["Crossblooded"].IsDisabled("Drawbacks")) { return; }

                    var CrossbloodedDrawbacks = BlueprintTools.GetBlueprint<BlueprintFeature>("f02fd748fecb4cc2a4d7d282c6b3de46");
                    CrossbloodedDrawbacks.SetName(TTTContext, "Crossblooded Drawbacks");
                    CrossbloodedDrawbacks.SetDescription(TTTContext, "A crossblooded sorcerer has one fewer spell known at each level than regular sorcerer.\n" +
                        "Furthermore, the conflicting urges created by the divergent nature of the crossblooded sorcerer’s dual heritage forces " +
                        "her to constantly take some mental effort just to remain focused on her current situation and needs. This leaves her " +
                        "with less mental resolve to deal with external threats. A crossblooded sorcerer always takes a -2 penalty on Will saves.");
                    CrossbloodedDrawbacks.AddComponent<AddStatBonus>(c => {
                        c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;
                        c.Value = -2;
                    });

                    TTTContext.Logger.LogPatch("Patched", CrossbloodedDrawbacks);
                }
            }
        }
    }
}
