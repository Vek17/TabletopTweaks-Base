using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Validation;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : BlueprintComponent {

        public PseudoActivatableType m_Type;
        public BlueprintBuffReference m_Buff;
        public string m_GroupName;

        public PseudoActivatableType Type => m_Type;
        public BlueprintBuffReference Buff => m_Buff ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
        public string GroupName => m_GroupName;

        public enum PseudoActivatableType {
            BuffToggle,
            VariantsBase
        }

        [HarmonyPatch(typeof(AddAbilityUseTrigger), nameof(AddAbilityUseTrigger.RunAction))]
        static class AddAbilityUseTrigger_PseudoActivatable_Patch {
            static bool Prefix(AbilityData spell) {
                return !spell.Blueprint.GetComponent<PseudoActivatable>();
            }
        }
    }
}
