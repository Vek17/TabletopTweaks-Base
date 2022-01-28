using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [TypeId("f97c2c9ad47743b49972f7c8c026d416")]
    public class SuppressBuffsTTT : UnitFactComponentDelegate {

        public override void OnPostLoad() {
            OnActivate();
        }

        public override void OnActivate() {
            var unitPartBuffSuppress = Owner.Ensure<UnitPartBuffSupressTTT>();
            if (Continuous) {
                unitPartBuffSuppress.AddContinuousEntry(this.Fact, m_Buffs, Schools, Descriptor);
                return;
            }
            unitPartBuffSuppress.AddNormalEntry(this.Fact, m_Buffs, Schools, Descriptor);
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
        [SerializeField]
        public bool Continuous = false;
    }
}
