using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.BaseAbilities {
    class OneHandedToggleAbility {
        public static void AddOneHandedToggle() {

            var FightDefensivelyFeature = Resources.GetBlueprint<BlueprintFeature>("ca22afeb94442b64fb8536e7a9f7dc11");
            var icon = AssetLoader.LoadInternal("Abilities", "Icon_OneHandedToggle.png");

            var OneHandedBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["OneHandedBuff"];
                bp.name = "MountedManiacBuff";
                bp.m_Icon = icon;
                bp.SetName("Use Weapon One Handed");
                bp.SetDescription("");
            });
            var OneHandedToggleAbility = Helpers.Create<BlueprintActivatableAbility>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["OneHandedToggleAbility"];
                bp.name = "MountedManiacActivatableAbility";
                bp.m_Icon = icon;
                bp.SetName("Use Weapon One Handed");
                bp.SetDescription("You can choose to wield your weapon in one hand instead of two if possible.");
                bp.m_Buff = OneHandedBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = false;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
            });
            var OneHandedToggleFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["OneHandedToggleFeature"];
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.name = "MountedManiacFeature";
                bp.m_Icon = icon;
                bp.SetName("Mounted Maniac");
                bp.SetDescription("You can choose to wield your weapon in one hand instead of two if possible.");
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        OneHandedToggleAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                }));
            });
            Resources.AddBlueprint(OneHandedBuff);
            Resources.AddBlueprint(OneHandedToggleAbility);
            Resources.AddBlueprint(OneHandedToggleFeature);

            if (ModSettings.AddedContent.MythicAbilities.DisableAll || !ModSettings.AddedContent.BaseAbilities.Enabled["OneHandedToggle"]) { return; }
            var AddFacts = FightDefensivelyFeature.GetComponent<AddFacts>();
            AddFacts.m_Facts = AddFacts.m_Facts.AppendToArray(OneHandedToggleFeature.ToReference<BlueprintUnitFactReference>());
        }
        [HarmonyPatch(typeof(ItemEntityWeapon), "HoldInTwoHands", MethodType.Getter)]
        static class ItemEntityWeapon_HoldInTwoHands_Patch {
            static void Postfix(ItemEntityWeapon __instance, ref bool __result) {
                var OneHandedBuff = Resources.GetBlueprint<BlueprintBuff>(ModSettings.Blueprints.NewBlueprints["OneHandedBuff"]);
                if (__instance.Wielder != null && __instance.Wielder.HasFact(OneHandedBuff)) {
                    if (__instance.Blueprint.IsOneHandedWhichCanBeUsedWithTwoHands && !__instance.Blueprint.IsTwoHanded) {
                        __result = false;
                    }
                }
            }
        }
    }
}
