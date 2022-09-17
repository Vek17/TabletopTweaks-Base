using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    class LongspearChargeBuff {
        public static void AddLongspearChargeBuff() {
            var LongspearChargeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "LongspearChargeBuff", bp => {
                bp.SetName(TTTContext, "Longspear Charge");
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
