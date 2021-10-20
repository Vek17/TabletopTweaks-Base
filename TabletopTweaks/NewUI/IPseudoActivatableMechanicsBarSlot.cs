using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewUI {
    public interface IPseudoActivatableMechanicsBarSlot {
        public abstract BlueprintBuffReference BuffToWatch { get; set; }
        public abstract bool ShouldBeActive { get; set; }
        public abstract AbilityData PseudoActivatableAbility { get; }
    }
}
