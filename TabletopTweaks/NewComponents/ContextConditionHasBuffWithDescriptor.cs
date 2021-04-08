using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Linq;

namespace TabletopTweaks.NewComponents {
	public class ContextConditionHasBuffWithDescriptor : ContextCondition {
		public override string GetConditionCaption() {
			return "Check if target has buffs with Descriptor";
		}

		public override bool CheckCondition() {
			var buffList = Target.Unit.Buffs;
			//bool containsDescriptor = buffList.Enumerable.SelectMany(buff => buff.Blueprint.GetComponents<SpellDescriptorComponent>()).Any(c => Descriptor.HasAnyFlag(c.Descriptor));
			bool containsDescriptor = buffList.Enumerable.Any(buff => (buff.MaybeContext.SpellDescriptor & Descriptor) == Descriptor);
			return Not ? !containsDescriptor : containsDescriptor;
		}
		public SpellDescriptorWrapper Descriptor;
	}
}
