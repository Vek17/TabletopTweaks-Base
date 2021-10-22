using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : BlueprintComponent {
        public BlueprintBuffReference m_BuffToWatch;

        public BlueprintBuffReference BuffToWatch => m_BuffToWatch ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);

        [HarmonyPatch(typeof(AddAbilityUseTrigger), nameof(AddAbilityUseTrigger.RunAction))]
        static class AddAbilityUseTrigger_PseudoActivatable_Patch {
            static bool Prefix(AbilityData spell) {
                return !spell.Blueprint.GetComponent<PseudoActivatable>();
            }
        }
    }
}
