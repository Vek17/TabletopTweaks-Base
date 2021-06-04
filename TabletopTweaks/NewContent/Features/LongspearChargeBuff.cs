using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
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
                bp.AddComponent(Helpers.Create<OutcomingAdditionalDamageAndHealingModifier>(c => {
                    c.Type = OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage;
                    c.ModifierPercents = new ContextValue() {
                        Value = 100
                    };
                }));
                bp.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
            });
            Resources.AddBlueprint(LongspearChargeBuff);
        }
    }
}
