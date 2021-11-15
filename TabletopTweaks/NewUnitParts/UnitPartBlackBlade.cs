using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartBlackBlade : UnitPart {
        public UnitPartBlackBlade() {
            Ego = new ModifiableValue(Owner.Stats, CustomStatTypes.CustomStatType.BlackBladeEgo.Stat()) { 
                BaseValue = 5
            };
            Intelligence = new ModifiableValueAttributeStat(Owner.Stats, StatType.Intelligence) {
                BaseValue = 11
            };
            Wisdom = new ModifiableValueAttributeStat(Owner.Stats, StatType.Wisdom) {
                BaseValue = 7
            };
            Charisma = new ModifiableValueAttributeStat(Owner.Stats, StatType.Charisma) {
                BaseValue = 7
            };
        }

        public readonly ModifiableValue Ego;
        public readonly ModifiableValueAttributeStat Intelligence;
        public readonly ModifiableValueAttributeStat Wisdom;
        public readonly ModifiableValueAttributeStat Charisma;
    }
}
