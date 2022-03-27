using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    class MasterfulTalent {
        public static void AddMasterfulTalent() {
            var MasterfulTalent = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MasterfulTalent", bp => {
                bp.SetName(TTTContext, "Masterful Talent");
                bp.SetDescription(TTTContext, "At 20th level, the rogue has been a thief, an actor, a merchant, a scout, a confessor, a friend, an assassin, " +
                    "and a dozen more things besides. The rogue gains a +4 bonus on all of her skills.");
                bp.AddComponent(Helpers.Create<BuffAllSkillsBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 4;
                    c.Multiplier = new ContextValue {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                }));
            });
        }
    }
}
