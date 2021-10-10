using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : BlueprintComponent {
        public BlueprintBuffReference m_BuffToWatch;

        public BlueprintBuffReference BuffToWatch => m_BuffToWatch ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
    }
}
