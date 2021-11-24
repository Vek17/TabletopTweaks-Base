using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;


namespace TabletopTweaks.NewContent.Feats {
    class QuarterstaffMaster {
        public static void AddQuarterstaffMaster() {
            if (ModSettings.AddedContent.Feats.IsDisabled("QuarterstaffMasterFeat")) return;
            BlueprintParametrizedFeature WeaponFocus = Resources.GetBlueprint<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");

            var QuarterstaffMasterFeat = Helpers.CreateBlueprint<BlueprintFeature>("QuarterstaffMasterFeat", bp => {
                bp.SetName("Quarterstaff Master");
                bp.SetDescription("By employing a number of different stances and techniques, you can wield a quarterstaff as a one-handed weapon. At the start of your turn, you decide whether or not you are going to wield the quarterstaff as a one-handed or two-handed weapon. When you wield it as a one-handed weapon, your other hand is free, and you cannot use the staff as a double weapon.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.AddComponent<PrerequisiteParametrizedFeature>(c => {
                    c.m_Feature = WeaponFocus.ToReference<BlueprintFeatureReference>();
                    c.ParameterType = FeatureParameterType.WeaponCategory;
                    c.WeaponCategory = WeaponCategory.Quarterstaff;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
                });
                bp.AddComponent<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 5;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
            });
            FeatTools.AddAsFeat(QuarterstaffMasterFeat);
        }
    }

    //Allow qstaff 1h
    [HarmonyPatch(typeof(ItemEntityWeapon), "HoldInTwoHands", MethodType.Getter)]
    static class QuarterstaffOneHanded {
        static void Postfix(ItemEntityWeapon __instance, ref bool __result) {
            if (ModSettings.AddedContent.BaseAbilities.IsDisabled("OneHandedToggle")) { return; }
            if (ModSettings.AddedContent.Feats.IsDisabled("QuarterstaffMasterFeat")) { return; }
            var quarterstaff = Resources.GetBlueprint<BlueprintWeaponType>("629736dabac7f9f4a819dc854eaed2d6");
            var qstaffmaster = Resources.GetModBlueprint<BlueprintFeature>("QuarterstaffMasterFeat");
            if (__instance.Wielder != null && __instance.Wielder.CustomMechanicsFeature(CustomMechanicsFeature.UseWeaponOneHanded)) {
                if (__instance.Wielder.HasFact(qstaffmaster) && __instance.Blueprint.Type == quarterstaff) {
                    __result = false;
                }
            }
        }
    }
    //Allow spellcombat when wielding qstaff 1h
    [HarmonyPatch(typeof(UnitPartMagus), "IsSpellCombatThisRoundAllowed")]
    static class QuarterstaffSpellCombat {
        static void Postfix(ref bool __result, UnitPart __instance) {
            if (ModSettings.AddedContent.BaseAbilities.IsDisabled("OneHandedToggle")) { return; }
            if (ModSettings.AddedContent.Feats.IsDisabled("QuarterstaffMasterFeat")) { return; }
            var quarterstaff = Resources.GetBlueprint<BlueprintWeaponType>("629736dabac7f9f4a819dc854eaed2d6");
            var qstaffmaster = Resources.GetModBlueprint<BlueprintFeature>("QuarterstaffMasterFeat");
            if (__instance.Owner != null && __instance.Owner.CustomMechanicsFeature(CustomMechanicsFeature.UseWeaponOneHanded)) {
                if (__instance.Owner.HasFact(qstaffmaster) && __instance.Owner.Body.CurrentHandsEquipmentSet.PrimaryHand.Weapon.Blueprint.Type == quarterstaff) {
                    __result = true;
                }
            }
        }
    }
}
