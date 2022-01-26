using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Cavalier {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Cavalier");

                PatchBase();
                PatchGendarme();
                PatchOrders();
            }

            static void PatchBase() {
                PatchCavalierMobility();
                PatchCavalierMountSelection();
                PatchSupremeCharge();

                void PatchCavalierMountSelection() {
                    if (ModSettings.Fixes.Cavalier.Base.IsDisabled("CavalierMountSelection")) { return; }

                    var CavalierMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
                    var AnimalCompanionEmptyCompanion = Resources.GetBlueprint<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
                    var AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
                    var AnimalCompanionFeatureHorse_PreorderBonus = Resources.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");
                    var CavalierMountFeatureWolf = Resources.GetModBlueprint<BlueprintFeature>("CavalierMountFeatureWolf");

                    CavalierMountSelection.SetFeatures(
                        AnimalCompanionEmptyCompanion,
                        AnimalCompanionFeatureHorse,
                        AnimalCompanionFeatureHorse_PreorderBonus,
                        CavalierMountFeatureWolf
                    );
                    CavalierMountSelection.m_Features = CavalierMountSelection.m_AllFeatures;
                    Main.LogPatch("Patched", CavalierMountSelection);
                }
                void PatchSupremeCharge() {
                    if (ModSettings.Fixes.Cavalier.Base.IsDisabled("SupremeCharge")) { return; }

                    var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                    var ChargeBuff = Resources.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                    var CavalierSupremeCharge = Resources.GetBlueprint<BlueprintFeature>("77af3c58e71118d4481c50694bd99e77");
                    var CavalierSupremeChargeBuff = Resources.GetBlueprint<BlueprintBuff>("7e9c5be79cfb3d44586dd650c7c7d198");

                    CavalierSupremeCharge.RemoveComponents<BuffExtraEffects>();
                    CavalierSupremeCharge.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff.ToReference<BlueprintUnitFactReference>() };
                        c.ExtraEffectBuff = CavalierSupremeChargeBuff.ToReference<BlueprintBuffReference>();
                    }));
                    CavalierSupremeChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    CavalierSupremeChargeBuff.AddComponent(Helpers.Create<AddOutgoingWeaponDamageBonus>(c => {
                        c.BonusDamageMultiplier = 1;
                    }));
                    CavalierSupremeChargeBuff.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
                    Main.LogPatch("Patched", CavalierSupremeCharge);
                    Main.LogPatch("Patched", CavalierSupremeChargeBuff);
                }
                void PatchCavalierMobility() {
                    if (ModSettings.Fixes.Cavalier.Base.IsDisabled("CavalierMobility")) { return; }

                    var CavalierProgression = Resources.GetBlueprint<BlueprintProgression>("aa70326bdaa7015438df585cf2ab93b9");
                    var CavalierMobilityFeature = Resources.GetModBlueprint<BlueprintFeature>("CavalierMobilityFeature");
                    var DiscipleOfThePikeArchetype = Resources.GetBlueprint<BlueprintArchetype>("4c4c3f9df00a5e04680d172a290111c4");

                    CavalierProgression.LevelEntries.Where(l => l.Level == 1).First().m_Features.Add(CavalierMobilityFeature.ToReference<BlueprintFeatureBaseReference>());
                    DiscipleOfThePikeArchetype.RemoveFeatures.Where(l => l.Level == 1).First().m_Features.Add(CavalierMobilityFeature.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            static void PatchGendarme() {
                PatchTransfixingCharge();

                void PatchTransfixingCharge() {
                    if (ModSettings.Fixes.Cavalier.Archetypes["Gendarme"].IsDisabled("TransfixingCharge")) { return; }

                    var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                    var ChargeBuff = Resources.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                    var GendarmeTransfixingCharge = Resources.GetBlueprint<BlueprintFeature>("72a0bde01943f824faa98bd55f04c06d");
                    var GendarmeTransfixingChargeBuff = Resources.GetBlueprint<BlueprintBuff>("6334e70d212add149909a36340ef5300");

                    GendarmeTransfixingCharge.RemoveComponents<BuffExtraEffects>();
                    GendarmeTransfixingCharge.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff.ToReference<BlueprintUnitFactReference>() };
                        c.ExtraEffectBuff = GendarmeTransfixingChargeBuff.ToReference<BlueprintBuffReference>();
                    }));
                    GendarmeTransfixingChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    GendarmeTransfixingChargeBuff.AddComponent(Helpers.Create<AddOutgoingWeaponDamageBonus>(c => {
                        c.BonusDamageMultiplier = 2;
                    }));
                    GendarmeTransfixingChargeBuff.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
                    Main.LogPatch("Patched", GendarmeTransfixingCharge);
                    Main.LogPatch("Patched", GendarmeTransfixingChargeBuff);
                }
            }

            static void PatchOrders() {

                PatchVisibility();
                PatchCalling();
                void PatchVisibility() {
                    if (ModSettings.Fixes.Cavalier.Base.IsDisabled("OrderAbilityVisibility")) { return; }
                    //Order Of The Cockatrice Level 15
                    Resources.GetBlueprint<BlueprintFeature>("1ee7bb75e8d29b641b39294ad4d9afca").HideInCharacterSheetAndLevelUp = false;

                    //Order Of The Lion Level 2
                    Resources.GetBlueprint<BlueprintFeature>("66ed10b9ff262734ca90a7b7167db764").HideInCharacterSheetAndLevelUp = false;

                    //Order Of The Lion Level 8
                    Resources.GetBlueprint<BlueprintFeature>("14c5ae26f6c962047be3fda7f865f519").HideInCharacterSheetAndLevelUp = false;

                    //Order Of The Star Level 2
                    Resources.GetBlueprint<BlueprintFeature>("271af8eb5d6ccd04899f548380bff006").HideInCharacterSheetAndLevelUp = false;

                    //Order Of The Star Level 8
                    Resources.GetBlueprint<BlueprintFeature>("2cc3041fdc8693640a0b18c8d14e77e0").HideInCharacterSheetAndLevelUp = false;

                    //Order Of The Sword Level 15
                    Resources.GetBlueprint<BlueprintFeature>("485ffa7a62af8064fa76d6d0de13c253").HideInCharacterSheetAndLevelUp = false;



                }

                void PatchCalling()
                {
                    if (ModSettings.Fixes.Cavalier.Base.IsDisabled("FixOrderOfTheStarsChannelling")) { return; }
                    var callingChannelSupport = Resources.GetBlueprint<BlueprintFeature>("eff49ecc28a0ce54caf416bdacedf4f3");
                    callingChannelSupport.AddComponent(Helpers.Create<IncreaseSpellDescriptorCasterLevel>(x => {
                        x.Descriptor = new Kingmaker.Blueprints.Classes.Spells.SpellDescriptorWrapper(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.ChannelPositiveHeal);
                        x.BonusCasterLevel = 1;
                        x.ModifierDescriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;


                    }));
                    callingChannelSupport.AddComponent(Helpers.Create<IncreaseSpellDescriptorCasterLevel>(x => {
                        x.Descriptor = new Kingmaker.Blueprints.Classes.Spells.SpellDescriptorWrapper(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.ChannelNegativeHeal);
                        x.BonusCasterLevel = 1;
                        x.ModifierDescriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;


                    }));
                    callingChannelSupport.AddComponent(Helpers.Create<IncreaseSpellDescriptorCasterLevel>(x => {
                        x.Descriptor = new Kingmaker.Blueprints.Classes.Spells.SpellDescriptorWrapper(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.ChannelNegativeHarm);
                        x.BonusCasterLevel = 1;
                        x.ModifierDescriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;


                    }));
                    callingChannelSupport.AddComponent(Helpers.Create<IncreaseSpellDescriptorCasterLevel>(x => {
                        x.Descriptor = new Kingmaker.Blueprints.Classes.Spells.SpellDescriptorWrapper(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.ChannelPositiveHarm);
                        x.BonusCasterLevel = 1;
                        x.ModifierDescriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;


                    }));
                    callingChannelSupport.AddComponent(Helpers.Create<AddCasterLevelForAbility>(x => {
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                        x.m_Spell = Resources.GetBlueprintReference<BlueprintAbilityReference>("8d6073201e5395d458b8251386d72df1");
                        x.Bonus = 1;

                    }));
                    callingChannelSupport.AddComponent(Helpers.Create<AddCasterLevelForAbility>(x => {
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                        x.m_Spell = Resources.GetBlueprintReference<BlueprintAbilityReference>("caae1dc6fcf7b37408686971ee27db13");
                        x.Bonus = 1;

                    }));
                    callingChannelSupport.AddComponent(Helpers.Create<AddCasterLevelForAbility>(x => {
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                        x.m_Spell = Resources.GetBlueprintReference<BlueprintAbilityReference>("8337cea04c8afd1428aad69defbfc365");
                        x.Bonus = 1;

                    }));

                    

                }
            }

         
        }
    }
}
