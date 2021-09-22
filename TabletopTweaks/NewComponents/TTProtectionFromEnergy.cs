using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents
{
    [ComponentName("Protection From Energy")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("0820cc56c9fa41e2a98cdb49256df47c")]
    public class TTProtectionFromEnergy : TTAddDamageResistanceEnergy
    {
        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            base.AdditionalInitFromVanillaDamageResistance(vanillaResistance);
            if (vanillaResistance is Kingmaker.Designers.Mechanics.Buffs.ProtectionFromEnergy vanillaProtection)
            {
                this.Immunity = true;
                this.Priority = DRPriority.High;
            }
        }

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other)
        {
            return other is TTProtectionFromEnergy && base.IsSameDRTypeAs(other);
        }
    }
}
