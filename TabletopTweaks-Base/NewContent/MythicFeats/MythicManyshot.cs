using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.RuleSystem.Rules;
using System;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    static class MythicManyshot {
        public static void AddMythicManyshot() {
            var Manyshot = BlueprintTools.GetBlueprint<BlueprintFeature>("adf54af2a681792489826f7fd1b62889");
            var ManyshotMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ManyshotMythicFeature", bp => {
                bp.m_Icon = Manyshot.m_Icon;
                bp.SetName(TTTContext, "Manyshot (Mythic)");
                bp.SetDescription(TTTContext, "You can fire a barrage of arrows at your target with very little effort.\n" +
                    "When making a full-attack action with a bow and using Manyshot, you fire two arrows with both your first and second attacks, instead of just your first attack.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.ManyshotMythic;
                });
                bp.AddPrerequisiteFeature(Manyshot);
            });
            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicManyshot")) { return; }
            FeatTools.AddAsMythicFeat(ManyshotMythicFeature);
        }
        [HarmonyPatch(typeof(RuleAttackWithWeapon), nameof(RuleAttackWithWeapon.LaunchProjectiles), new Type[] { typeof(BlueprintProjectileReference[]) })]
        static class MythicManyShot_RuleAttackWithWeapon_LaunchProjectiles_Patch {
            static bool Prefix(RuleAttackWithWeapon __instance, BlueprintProjectileReference[] projectiles) {
                if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicManyshot")) { return true; }
                foreach (BlueprintProjectileReference blueprintProjectileReference in projectiles) {
                    if (blueprintProjectileReference.Get() != null) {
                        __instance.LaunchProjectile(blueprintProjectileReference.Get(), true);

                        if (__instance.IsFirstAttack && CanManyshot(__instance)) {
                            __instance.LaunchProjectile(blueprintProjectileReference.Get(), false);
                        } else if (__instance.AttackNumber == 1 && CanManyshot(__instance) && __instance.Initiator.CustomMechanicsFeature(CustomMechanicsFeature.ManyshotMythic)) {
                            __instance.LaunchProjectile(blueprintProjectileReference.Get(), false);
                        }
                    }
                }
                return false;
            }
            private static bool CanManyshot(RuleAttackWithWeapon rule) {
                return rule.Weapon.Blueprint.FighterGroup.Contains(WeaponFighterGroup.Bows)
                    && rule.Initiator.Descriptor.State.Features.Manyshot
                    && rule.IsFullAttack
                    && !rule.Initiator.Descriptor.State.Features.SuppressedManyshot;
            }
        }
    }
}
