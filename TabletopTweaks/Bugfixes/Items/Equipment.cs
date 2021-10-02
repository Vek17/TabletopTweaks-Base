using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Items {
    static class Equipment {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Equipment");
                PatchMagiciansRing();
                PatchManglingFrenzy();
                PatchMetamagicRods();
                PatchHolySymbolofIomedae();

                void PatchMagiciansRing() {
                    if (ModSettings.Fixes.Items.Equipment.IsDisabled("MagiciansRing")) { return; }

                    var RingOfTheSneakyWizardFeature = Resources.GetBlueprint<BlueprintFeature>("d848f1f1b31b3e143ba4aeeecddb17f4");
                    RingOfTheSneakyWizardFeature.GetComponent<IncreaseSpellSchoolDC>().BonusDC = 2;
                    Main.LogPatch("Patched", RingOfTheSneakyWizardFeature);
                }

                void PatchHolySymbolofIomedae() {
                    if (ModSettings.Fixes.Items.Equipment.IsDisabled("HolySymbolofIomedae")) { return; }

                    var Artifact_HolySymbolOfIomedaeArea = Resources.GetBlueprint<BlueprintAbilityAreaEffect>("e6dff35442f00ab4fa2468804c15efc0");
                    var Artifact_HolySymbolOfIomedaeBuff = Resources.GetBlueprint<BlueprintBuff>("c8b1c0f5cd21f1d4e892f7440ec28e24");
                    Artifact_HolySymbolOfIomedaeArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .UnitExit = Helpers.CreateActionList(
                            Helpers.Create<ContextActionRemoveBuff>(a => a.m_Buff = Artifact_HolySymbolOfIomedaeBuff.ToReference<BlueprintBuffReference>())
                    );
                    Main.LogPatch("Patched", Artifact_HolySymbolOfIomedaeArea);
                }

                // Fix Mangling Frenzy does not apply to Bloodrager's Rage
                void PatchManglingFrenzy() {
                    if (ModSettings.Fixes.Items.Equipment.IsDisabled("ManglingFrenzy")) { return; }
                    var ManglingFrenzyFeature = Resources.GetBlueprint<BlueprintFeature>("29e2f51e6dd7427099b015de88718990");
                    var ManglingFrenzyBuff = Resources.GetBlueprint<BlueprintBuff>("1581c5ceea24418cadc9f26ce4d391a9");
                    var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");

                    ManglingFrenzyFeature.AddComponent(Helpers.Create<BuffExtraEffects>(c => {
                        c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                        c.m_ExtraEffectBuff = ManglingFrenzyBuff.ToReference<BlueprintBuffReference>();
                    }));
                }

                void PatchMetamagicRods() {
                    if (ModSettings.Fixes.Items.Equipment.IsDisabled("MetamagicRods")) { return; }

                    BlueprintActivatableAbility[] MetamagicRodAbilities = new BlueprintActivatableAbility[] {
                        Resources.GetBlueprint<BlueprintActivatableAbility>("ccffef1193d04ad1a9430a8009365e81"), //MetamagicRodGreaterBolsterToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("cc266cfb106a5a3449b383a25ab364f0"), //MetamagicRodGreaterEmpowerToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("c137a17a798334c4280e1eb811a14a70"), //MetamagicRodGreaterExtendToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("78b5971c7a0b7f94db5b4d22c2224189"), //MetamagicRodGreaterMaximizeToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("5016f110e5c742768afa08224d6cde56"), //MetamagicRodGreaterPersistentToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("fca35196b3b23c346a7d1b1ce20c6f1c"), //MetamagicRodGreaterQuickenToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("cc116b4dbb96375429107ed2d88943a1"), //MetamagicRodGreaterReachToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("f0d798f5139440a8b2e72fe445678d29"), //MetamagicRodGreaterSelectiveToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("056b9f1aa5c54a7996ca8c4a00a88f88"), //MetamagicRodLesserBolsterToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("ed10ddd385a528944bccbdc4254f8392"), //MetamagicRodLesserEmpowerToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("605e64c0b4586a34494fc3471525a2e5"), //MetamagicRodLesserExtendToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("868673cd023f96945a2ee61355740a96"), //MetamagicRodLesserKineticToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("485ffd3bd7877fb4d81409b120a41076"), //MetamagicRodLesserMaximizeToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("5a87350fcc6b46328a2b345f23bbda44"), //MetamagicRodLesserPersistentToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("b8b79d4c37981194fa91771fc5376c5e"), //MetamagicRodLesserQuickenToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("7dc276169f3edd54093bf63cec5701ff"), //MetamagicRodLesserReachToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("66e68fd0b661413790e3000ede141f16"), //MetamagicRodLesserSelectiveToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("afb2e1f96933c22469168222f7dab8fb"), //MetamagicRodMasterpieceToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("6cc31148ae2d48359c02712308cb4167"), //MetamagicRodNormalBolsterToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("077ec9f9394b8b347ba2b9ec45c74739"), //MetamagicRodNormalEmpowerToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("69de70b88ca056440b44acb029a76cd7"), //MetamagicRodNormalExtendToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("3b5184a55f98f264f8b39bddd3fe0e88"), //MetamagicRodNormalMaximizeToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("9ae2e56b24404144bd911378fe541597"), //MetamagicRodNormalPersistentToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("1f390e6f38d3d5247aacb25ab3a2a6d2"), //MetamagicRodNormalQuickenToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("f0b05e39b82c3be408009e26be40bc91"), //MetamagicRodNormalReachToggleAbility
                        Resources.GetBlueprint<BlueprintActivatableAbility>("04f768c59bb947e3948ce2e7e72feecb"), //MetamagicRodNormalSelectiveToggleAbility
                    };
                    MetamagicRodAbilities.ForEach(ability => {
                        ability.IsOnByDefault = false;
                        ability.DoNotTurnOffOnRest = false;
                        Main.LogPatch("Patched", ability);
                    });
                }
            }
        }
    }
}
