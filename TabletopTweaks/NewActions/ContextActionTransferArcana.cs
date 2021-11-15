using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace TabletopTweaks.NewActions {
    [TypeId("c932d7de9d3b4c558933292395aeda38")]
    class ContextActionTransferArcana : ContextAction {

        public override string GetCaption() {
            return string.Format("Transfers points between arcane pools");
        }

        public override void RunAction() {
            var Caster = base.Context.MaybeCaster;
            if (Caster == null) { return; }
            if (Caster.Resources.ContainsResource(m_sourceResource) 
                && Caster.Resources.ContainsResource(m_destinationResource) 
                && !Caster.Resources.HasEnoughResource(m_sourceResource, m_sourceAmount)) { 
                return; 
            }
            RuleSavingThrow TransferSave = new RuleSavingThrow(Caster, SavingThrowType.Will, SaveDC.Calculate(base.Context));
            base.Context.TriggerRule(TransferSave);
            if (!TransferSave.IsPassed) {
                using (base.AbilityContext.GetDataScope(base.Target)) {
                    this.FailedActions.Run();
                }
            } else {
                Caster.Resources.Spend(m_sourceResource, m_sourceAmount);
                Caster.Resources.Restore(m_destinationResource, m_destinationAmount);
            }
        }

        public BlueprintAbilityResourceReference m_sourceResource;
        public BlueprintAbilityResourceReference m_destinationResource;
        public int m_sourceAmount;
        public int m_destinationAmount;
        public ActionList FailedActions;
        public ContextValue SaveDC;
    }
}
