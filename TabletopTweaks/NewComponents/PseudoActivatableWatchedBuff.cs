using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintBuff))]
    [TypeId("1ab7dd64fc4b4018bbc16bb484dd81a4")]
    public class PseudoActivatableWatchedBuff :
        UnitFactComponentDelegate {

        public override void OnActivate() {
            if (!(Fact.Blueprint is BlueprintBuff buff)) {
                Main.Log($"WARNING: PseudoActivatableWatchedBuff.OnActivate triggered on fact {Fact.Name}, but this fact is not a Buff");
                return;
            }
            Main.LogDebug($"PseudoActivatableWatchedBuff.OnActivate: {buff.Name}");
            this.Owner.Ensure<UnitPartPseudoActivatableAbilities>().BuffActivated(buff);
        }

        public override void OnDeactivate() {
            if (!(Fact.Blueprint is BlueprintBuff buff)) {
                Main.Log($"WARNING: PseudoActivatableWatchedBuff.OnDeactivate triggered on fact {Fact.Name}, but this fact is not a Buff");
                return;
            }
            Main.LogDebug($"PseudoActivatableWatchedBuff.OnDeactivate: {buff.Name}");
            this.Owner.Get<UnitPartPseudoActivatableAbilities>()?.BuffDeactivated(buff);
        }

        public override void OnPostLoad() {
            if (!(Fact.Blueprint is BlueprintBuff buff)) {
                Main.Log($"WARNING: PseudoActivatableWatchedBuff.OnPostLoad triggered on fact {Fact.Name}, but this fact is not a Buff");
                return;
            }
            Main.LogDebug($"PseudoActivatableWatchedBuff.OnPostLoad: {buff.Name}");
            this.Owner.Ensure<UnitPartPseudoActivatableAbilities>().RegisterWatchedBuff(buff);
        }
    }
}
