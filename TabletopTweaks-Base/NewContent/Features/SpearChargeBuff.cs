using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    class SpearChargeBuff {
        public static void AddSpearChargeBuff() {
            var SpearChargeBuffTTT = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SpearChargeBuffTTT", bp => {
                bp.SetName(TTTContext, "Spear Charge");
                bp.SetDescription(TTTContext, "");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddOutgoingWeaponDamageBonus>(c => {
                    c.BonusDamageMultiplier = 1;
                    c.RemoveAfterTrigger = true;
                });
            });
        }
    }
}
