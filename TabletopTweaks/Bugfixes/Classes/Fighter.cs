using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.MechanicsChanges;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Fighter {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Fighter");

                PatchBase();
                PatchTwoHandedFighter();
            }
            static void PatchBase() {
                PatchTwoHandedWeaponTraining();
                PatchAdvancedWeaponTraining();
                EnableAdvancedArmorTraining();

                void PatchAdvancedWeaponTraining() {
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedWeaponTraining")) { return; }
                    var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
                    var WeaponTrainingRankUpSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("5f3cc7b9a46b880448275763fe70c0b0");
                    var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
                    var AdvancedWeaponTraining2 = Resources.GetBlueprint<BlueprintFeatureSelection>("70a139f0a4c6c534eaa34feea0d08622");
                    var AdvancedWeaponTraining3 = Resources.GetBlueprint<BlueprintFeatureSelection>("ee9ab0117ca06b84f9c66469f4428c61");
                    var AdvancedWeaponTraining4 = Resources.GetBlueprint<BlueprintFeatureSelection>("0b55d725ded1ae549bb858fba1d84114");
                    var AdvancedWeapontrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedWeaponTrainingSelection");

                    WeaponTrainingSelection.m_AllFeatures = WeaponTrainingSelection.m_AllFeatures.Where(feature => !AdvancedWeapontrainingSelection.m_AllFeatures.Contains(feature)).ToArray();
                    WeaponTrainingSelection.Mode = SelectionMode.Default;
                    WeaponTrainingSelection.AddFeatures(AdvancedWeapontrainingSelection);
                    Main.LogPatch("Patched", WeaponTrainingSelection);
                    WeaponTrainingRankUpSelection.m_AllFeatures = WeaponTrainingRankUpSelection.m_AllFeatures.Where(feature => !AdvancedWeapontrainingSelection.m_AllFeatures.Contains(feature)).ToArray();
                    WeaponTrainingRankUpSelection.m_AllFeatures.ForEach(feature => {
                        feature.Get().AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                            c.m_Feature = feature;
                            c.HideInUI = true;
                        }));
                        Main.LogPatch("Patched", feature.Get());
                    });
                    WeaponTrainingRankUpSelection.IgnorePrerequisites = true;
                    Main.LogPatch("Patched", WeaponTrainingRankUpSelection);
                    AdvancedWeapontrainingSelection.m_AllFeatures.ForEach(feature => {
                        feature.Get().RemoveComponents<PrerequisiteClassLevel>();
                        Main.LogPatch("Patched", feature.Get());
                    });
                    AdvancedWeaponTraining1.IgnorePrerequisites = false;
                    AdvancedWeaponTraining2.IgnorePrerequisites = false;
                    AdvancedWeaponTraining3.IgnorePrerequisites = false;
                    AdvancedWeaponTraining4.IgnorePrerequisites = false;
                    Main.LogPatch("Patched", AdvancedWeaponTraining1);
                    Main.LogPatch("Patched", AdvancedWeaponTraining2);
                    Main.LogPatch("Patched", AdvancedWeaponTraining3);
                    Main.LogPatch("Patched", AdvancedWeaponTraining4);
                }
                void PatchTwoHandedWeaponTraining() {
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("TwoHandedWeaponTraining")) { return; }
                    var TwoHandedFighterWeaponTraining = Resources.GetBlueprint<BlueprintFeature>("88da2a5dfc505054f933bb81014e864f");
                    var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
                    var AdvancedWeapontrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedWeaponTrainingSelection");

                    TwoHandedFighterWeaponTraining.SetComponents(
                        Helpers.Create<WeaponGroupAttackBonus>(c => {
                            c.WeaponGroup = (WeaponFighterGroup)AdditionalWeaponFighterGroups.TwoHanded;
                            c.AttackBonus = 1;
                            c.contextMultiplier = new ContextValue();
                        }),
                        Helpers.Create<WeaponGroupDamageBonus>(c => {
                            c.WeaponGroup = (WeaponFighterGroup)AdditionalWeaponFighterGroups.TwoHanded;
                            c.DamageBonus = 1;
                            c.AdditionalValue = new ContextValue();
                        }),
                        Helpers.Create<WeaponTraining>()
                    );
                    Main.LogPatch("Patched", TwoHandedFighterWeaponTraining);
                    WeaponTrainingSelection.m_AllFeatures
                        .Where(feature => !AdvancedWeapontrainingSelection.m_AllFeatures.Contains(feature))
                        .ForEach(feature => {
                            var component = feature.Get().GetComponent<WeaponGroupDamageBonus>();
                            if (component != null) {
                                component.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.WeaponTraining;
                                Main.LogPatch("Patched", feature.Get());
                            }
                        });
                }
                void EnableAdvancedArmorTraining() {
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                    var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                    var ArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                    var FighterArmorProgression = Resources.GetModBlueprint<BlueprintFeature>("FighterArmorTrainingProgression");
                    var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
                    var BaseProgression = FighterClass.Progression;
                    BaseProgression.UIGroups
                        .Where(g => g.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                        .First()
                        .m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                    BaseProgression.LevelEntries
                        .Where(entry => entry.Level > 3)
                        .Where(entry => entry.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                        .ForEach(entry => {
                            entry.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                            entry.m_Features.Remove(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                        });
                    BaseProgression.LevelEntries.First(x => x.Level == 1).m_Features.Add(FighterArmorProgression.ToReference<BlueprintFeatureBaseReference>());
                    Main.LogPatch("Patched", BaseProgression);
                    foreach (var Archetype in FighterClass.Archetypes) {
                        Archetype.RemoveFeatures
                            .Where(entry => entry.Level > 3)
                            .Where(entry => entry.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                            .ForEach(entry => {
                                entry.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                                entry.m_Features.Remove(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                            });
                        Main.LogPatch("Patched", Archetype);
                        if (Archetype.RemoveFeatures.Any(x => x.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>())))
                        {
                            LevelEntry first = Archetype.RemoveFeatures.FirstOrDefault(x => x.Level == 1);
                            if (first == null)
                            {
                                Archetype.RemoveFeatures.AddItem(new LevelEntry
                                {
                                    Level = 1,
                                    m_Features = new System.Collections.Generic.List<BlueprintFeatureBaseReference>
                                    {
                                        FighterArmorProgression.ToReference<BlueprintFeatureBaseReference>(),


                                    }
                                });
                            }
                            else
                            {
                                first.m_Features.Add(FighterArmorProgression.ToReference<BlueprintFeatureBaseReference>());
                            }
                        }
                    }

                }
            }
            static void PatchTwoHandedFighter() {
                PatchAdvancedWeaponTraining();

                void PatchAdvancedWeaponTraining() {
                    if (ModSettings.Fixes.Fighter.Archetypes["TwoHandedFighter"].IsDisabled("AdvancedWeaponTraining")) { return; }

                    var TwoHandedFighterWeaponTraining = Resources.GetBlueprint<BlueprintFeature>("88da2a5dfc505054f933bb81014e864f");
                    var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeature>("b8cecf4e5e464ad41b79d5b42b76b399");

                    var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeature>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
                    PatchPrerequisites(AdvancedWeaponTraining1);

                    void PatchPrerequisites(BlueprintFeature AdvancedWeaponTraining) {
                        AdvancedWeaponTraining.GetComponent<PrerequisiteFeature>().Group = Prerequisite.GroupType.Any;
                        AdvancedWeaponTraining.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                            c.m_Feature = TwoHandedFighterWeaponTraining.ToReference<BlueprintFeatureReference>();
                            c.Group = Prerequisite.GroupType.Any;
                        }));
                        Main.LogPatch("Patched", AdvancedWeaponTraining);
                    }
                }
            }
        }

        private enum AdditionalWeaponFighterGroups : int {
            TwoHanded = 1073741824
        }
        [HarmonyPatch(typeof(UnitPartWeaponTraining), "GetWeaponRank", new Type[] { typeof(ItemEntityWeapon) })]
        static class UnitPartWeaponTraining_GetWeaponRank_Patch {
            static bool Prefix(UnitPartWeaponTraining __instance, ref int __result, ItemEntityWeapon weapon) {
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("TwoHandedWeaponTraining")) { return true; }

                if (weapon == null) {
                    __result = 0;
                    return false;
                }
                int num = 0;
                foreach (EntityFact entityFact in __instance.WeaponTrainings) {
                    foreach (EntityFactComponent entityFactComponent in entityFact.Components) {
                        WeaponGroupAttackBonus weaponGroupAttackBonus = entityFactComponent.SourceBlueprintComponent as WeaponGroupAttackBonus;
                        WeaponFighterGroup? weaponFighterGroup = (weaponGroupAttackBonus != null) ? new WeaponFighterGroup?(weaponGroupAttackBonus.WeaponGroup) : null;
                        WeaponFighterGroupFlags fighterGroup = weapon.Blueprint.Type.FighterGroup;
                        if (weaponFighterGroup == null) {
                            continue;
                        }
                        if (fighterGroup.Contains(weaponFighterGroup.GetValueOrDefault())) {
                            num = Math.Max(num, entityFact.GetRank());
                        }
                        if (weaponFighterGroup == ((WeaponFighterGroup)AdditionalWeaponFighterGroups.TwoHanded) && weapon.Blueprint.IsTwoHanded) {
                            num = Math.Max(num, entityFact.GetRank());
                        }
                    }
                }
                __result = num;
                return false;
            }
        }

        [HarmonyPatch(typeof(WeaponGroupAttackBonus), "OnEventAboutToTrigger", new Type[] { typeof(RuleCalculateAttackBonusWithoutTarget) })]
        static class WeaponGroupAttackBonus_OnEventAboutToTrigger_Patch {
            static bool Prefix(WeaponGroupAttackBonus __instance, RuleCalculateAttackBonusWithoutTarget evt) {
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("TwoHandedWeaponTraining")) { return true; }

                if (evt.Weapon != null && evt.Weapon.Blueprint.FighterGroup.HasFlag(__instance.WeaponGroup.ToFlags())) {
                    int num = __instance.multiplyByContext ? (__instance.contextMultiplier.Calculate(__instance.Context) * __instance.AttackBonus) : __instance.AttackBonus;
                    evt.AddModifier(
                        bonus: num * __instance.Fact.GetRank(),
                        source: __instance.Fact,
                        descriptor: (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.WeaponTraining
                    );
                    return false;
                }
                if (evt.Weapon != null
                    && __instance.WeaponGroup == (WeaponFighterGroup)AdditionalWeaponFighterGroups.TwoHanded
                    && evt.Weapon.Blueprint.IsTwoHanded) {

                    int num = __instance.multiplyByContext ? (__instance.contextMultiplier.Calculate(__instance.Context) * __instance.AttackBonus) : __instance.AttackBonus;
                    evt.AddModifier(
                        bonus: num * __instance.Fact.GetRank(),
                        source: __instance.Fact,
                        descriptor: (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.WeaponTraining
                    );
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(WeaponGroupDamageBonus), "OnEventAboutToTrigger", new Type[] { typeof(RuleCalculateWeaponStats) })]
        static class WeaponGroupDamageBonus_OnEventAboutToTrigger_Patch {
            static bool Prefix(WeaponGroupDamageBonus __instance, RuleCalculateWeaponStats evt) {
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("TwoHandedWeaponTraining")) { return true; }

                int num = __instance.AdditionalValue.Calculate(__instance.Context);
                if (evt.Weapon.Blueprint.FighterGroup.HasFlag(__instance.WeaponGroup.ToFlags())) {
                    evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(
                        value: __instance.DamageBonus * __instance.Fact.GetRank() + num,
                        source: __instance.Runtime,
                        desc: __instance.Descriptor
                    ));
                    return false;
                }

                if (evt.Weapon != null
                    && __instance.WeaponGroup == (WeaponFighterGroup)AdditionalWeaponFighterGroups.TwoHanded
                    && evt.Weapon.Blueprint.IsTwoHanded) {


                    evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(
                        value: __instance.DamageBonus * __instance.Fact.GetRank() + num,
                        source: __instance.Runtime,
                        desc: __instance.Descriptor
                    ));
                }
                return false;
            }
        }
    }
}
