using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : BlueprintComponent {
        public BlueprintBuffReference m_BuffToWatch;

        public BlueprintBuffReference BuffToWatch => m_BuffToWatch ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
    }
}
