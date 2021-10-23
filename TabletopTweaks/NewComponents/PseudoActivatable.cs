using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : AbilityApplyEffect {

        public BlueprintBuffReference m_Buff;
        public string m_GroupName;

        public BlueprintBuffReference Buff => m_Buff ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
        public string GroupName => m_GroupName;

        public override void Apply(AbilityExecutionContext context, TargetWrapper target) {
            if (m_Buff == null || !target.IsUnit || context.MaybeCaster == null || target.Unit != context.MaybeCaster)
                return;

            var owner = context.MaybeCaster;
            if (this.OwnerBlueprint is not BlueprintAbility sourceAbility) {
                Main.Log("WARNING: PseudoActivatable component is present on a blueprint that is not a BlueprintAbility. This will not work.");
                return;
            }
            owner.Ensure<UnitPartPseudoActivatableAbilities>().ToggleBuff(m_Buff, sourceAbility, context, m_GroupName);
        }

        [HarmonyPatch(typeof(AddAbilityUseTrigger), nameof(AddAbilityUseTrigger.RunAction))]
        static class AddAbilityUseTrigger_PseudoActivatable_Patch {
            static bool Prefix(AbilityData spell) {
                return !spell.Blueprint.GetComponent<PseudoActivatable>();
            }
        }
    }
}
