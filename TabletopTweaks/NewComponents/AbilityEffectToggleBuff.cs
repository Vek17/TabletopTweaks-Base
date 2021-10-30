using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("6dc16c033e0d4f94827db76f1fbab421")]
    public class AbilityEffectToggleBuff : AbilityApplyEffect {

        public BlueprintBuffReference m_Buff;

        public BlueprintBuffReference Buff => m_Buff ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);

        public override void Apply(AbilityExecutionContext context, TargetWrapper target) {
            if (m_Buff == null || !target.IsUnit || context.MaybeCaster == null || target.Unit != context.MaybeCaster)
                return;

            if (this.OwnerBlueprint is not BlueprintAbility sourceAbility) {
                Main.Log("WARNING: AbilityEffectToggleBuff component is present on a blueprint that is not a BlueprintAbility. This will not work.");
                return;
            }

            var owner = context.MaybeCaster;
            if (!owner.Descriptor.HasFact(m_Buff)) {
                var appliedBuff = owner.Descriptor.AddBuff(m_Buff, context, new TimeSpan?());
                appliedBuff.IsFromSpell = false;
                appliedBuff.IsNotDispelable = true;
                appliedBuff.SourceAbility = sourceAbility;
            } else {
                owner.Buffs.RemoveFact(m_Buff);
            }
        }
    }
}
