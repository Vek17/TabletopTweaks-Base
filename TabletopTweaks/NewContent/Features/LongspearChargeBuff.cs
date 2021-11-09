using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class LongspearChargeBuff {
        public static void AddLongspearChargeBuff() {
            var LongspearChargeBuff = Helpers.CreateBuff("LongspearChargeBuff", bp => {
                bp.SetName("75390bf5d4f14875bdd892e2cc95ed31", "Longspear Charge");
                bp.SetDescription("596d14a699a845809553331365596d1a", "");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddOutgoingWeaponDamageBonus>(c => {
                    c.BonusDamageMultiplier = 1;
                }));
                bp.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
            });
        }
    }
}
