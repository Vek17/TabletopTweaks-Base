using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Items;
using Kingmaker.UI.UnitSettings.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.BaseAbilities {
    class OneHandedToggleAbility {
        public static void AddOneHandedToggle() {

            var FightDefensivelyFeature = Resources.GetBlueprint<BlueprintFeature>("ca22afeb94442b64fb8536e7a9f7dc11");
            var FightDefensivelyToggleAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("09d742e8b50b0214fb71acfc99cc00b3");
            var icon = AssetLoader.LoadInternal("Abilities", "Icon_OneHandedToggle.png");

            var OneHandedBuff = Helpers.CreateBuff("OneHandedBuff", bp => {
                bp.m_Icon = icon;
                bp.SetName("Use Weapon One Handed");
                bp.SetDescription("");
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.UseWeaponOneHanded;
                });
            });
            var OneHandedToggleAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>("OneHandedToggleAbility", bp => {
                bp.m_Icon = icon;
                bp.SetName("Use Weapon One Handed");
                bp.SetDescription("You can choose to wield your weapon in one hand instead of two if possible.");
                bp.m_Buff = OneHandedBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = false;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
                bp.AddComponent(Helpers.CreateCopy(FightDefensivelyToggleAbility.GetComponent<ActionPanelLogic>()));
            });
            var OneHandedToggleFeature = Helpers.CreateBlueprint<BlueprintFeature>("OneHandedToggleFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("OneHanded Toggle Feature");
                bp.SetDescription("You can choose to wield your weapon in one hand instead of two if possible.");
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        OneHandedToggleAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
            });

            if (ModSettings.AddedContent.BaseAbilities.IsDisabled("OneHandedToggle")) { return; }
            var AddFacts = FightDefensivelyFeature.GetComponent<AddFacts>();
            AddFacts.m_Facts = AddFacts.m_Facts.AppendToArray(OneHandedToggleFeature.ToReference<BlueprintUnitFactReference>());
        }
        [HarmonyPatch(typeof(ItemEntityWeapon), "HoldInTwoHands", MethodType.Getter)]
        static class ItemEntityWeapon_HoldInTwoHands_Patch {
            static void Postfix(ItemEntityWeapon __instance, ref bool __result) {
                if (ModSettings.AddedContent.BaseAbilities.IsDisabled("OneHandedToggle")) { return; }
                if (__instance.Wielder != null && __instance.Wielder.CustomMechanicsFeature(CustomMechanicsFeature.UseWeaponOneHanded)) {
                    if (__instance.Blueprint.IsOneHandedWhichCanBeUsedWithTwoHands && !__instance.Blueprint.IsTwoHanded) {
                        __result = false;
                    }
                }
            }
        }
    }
}
