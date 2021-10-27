using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : UnitFactComponentDelegate {

        public PseudoActivatableType m_Type;

        // type BuffToggle
        public BlueprintBuffReference m_Buff;
        public string m_GroupName;

        // type VariantsBase
        public bool m_ActiveWhenVariantActive = true;
        public bool m_UseActiveVariantForeIcon = true;

        public PseudoActivatableType Type => m_Type;
        public BlueprintBuffReference Buff => m_Buff ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
        public string GroupName => m_GroupName;

        public bool ActiveWhenVariantActive => m_ActiveWhenVariantActive;
        public bool UseActiveVariantForeIcon => m_UseActiveVariantForeIcon;

        public override void OnTurnOn() {
            if (this.Fact is not Ability ability) {
                Main.Log($"WARNING: PseudoActivatable component is present on Fact \"{this.Fact?.Name}\", but this Fact is not an Ability.");
                return;
            }
            this.Owner.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbility(ability.Data);
        }

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
