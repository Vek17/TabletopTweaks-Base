using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    class ContextConditionHasCondition : ContextCondition {

        public override string GetConditionCaption() {
            return $"Check if target has condition: {string.Join(", ", Conditions.Select(c => c.ToString()))}";
        }

        public override bool CheckCondition() {
            return Conditions.Any(condition => base.Target.Unit.Descriptor.State.HasCondition(condition));
        }

        [SerializeField]
        [FormerlySerializedAs("Buff")]
        public UnitCondition[] Conditions;
    }
}
