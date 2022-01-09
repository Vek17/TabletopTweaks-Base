using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.NewUnitParts;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [TypeId("f97c2c9ad47743b49972f7c8c026d416")]
    class SuppressBuffsTTT : UnitFactComponentDelegate {

        public override void OnPostLoad() {
            OnActivate();
        }

        public override void OnActivate() {
            var unitPartBuffSuppress = Owner.Ensure<UnitPartBuffSupressTTT>();
            if (!Schools.Empty()) {
                unitPartBuffSuppress.AddEntry(this.Fact, Schools);
            }
            if (Descriptor != SpellDescriptor.None) {
                unitPartBuffSuppress.AddEntry(this.Fact, Descriptor);
            }
            if (!m_Buffs.Empty()) {
                unitPartBuffSuppress.AddEntry(this.Fact, m_Buffs);
            }
        }

        public override void OnDeactivate() {
            var unitPartBuffSuppress = Owner.Ensure<UnitPartBuffSupressTTT>();
            unitPartBuffSuppress.RemoveEntry(this.Fact);
        }

        [SerializeField]
        [FormerlySerializedAs("Buffs")]
        public BlueprintBuffReference[] m_Buffs = new BlueprintBuffReference[0];
        [SerializeField]
        public SpellSchool[] Schools = new SpellSchool[0];
        [SerializeField]
        public SpellDescriptorWrapper Descriptor = SpellDescriptor.None;
    }
}
