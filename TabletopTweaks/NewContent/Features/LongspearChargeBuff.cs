using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class LongspearChargeBuff {
        public static void AddLongspearChargeBuff() {
            var LongspearChargeBuff = Helpers.CreateBuff(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["LongspearChargeBuff"];
                bp.name = "LongspearChargeBuff";
                bp.SetName("Longspear Charge");
                bp.SetDescription("");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<AddOutgoingWeaponDamageBonus>(c => {
                    c.BonusDamageMultiplier = 1;
                }));
                bp.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
            });
            Resources.AddBlueprint(LongspearChargeBuff);
        }
    }
}
