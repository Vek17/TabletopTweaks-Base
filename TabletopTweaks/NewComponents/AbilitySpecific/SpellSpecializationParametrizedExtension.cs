using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintParametrizedFeature), false)]
    [TypeId("b104bfacc8a3446ab149fb3241778d8e")]
    class SpellSpecializationParametrizedExtension : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            var part = base.Owner.Ensure<UnitPartSpellSpecialization>();
            part.AddEntry(base.Param.Blueprint.ToReference<BlueprintAbilityReference>(), base.Fact);
        }

        public override void OnTurnOff() {
            var part = base.Owner.Get<UnitPartSpellSpecialization>();
            if (part != null) {
                part.RemoveEntry(base.Fact);
            }
        }

        public override void OnActivate() {
            OnTurnOn();
        }

        public override void OnDeactivate() {
            OnTurnOff();
        }
    }
}
