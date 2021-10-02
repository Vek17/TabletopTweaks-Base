using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [TypeId("f97c2c9ad47743b49972f7c8c026d416")]
    class SuppressBuffsPersistant : UnitFactComponentDelegate {

        public ReferenceArrayProxy<BlueprintBuff, BlueprintBuffReference> Buffs {
            get {
                return m_Buffs;
            }
        }

        public override void OnPostLoad() {
            OnActivate();
        }

        public override void OnActivate() {
            UnitPartBuffSuppress unitPartBuffSuppress = Owner.Ensure<UnitPartBuffSuppress>();
            IEnumerable<BlueprintBuff> suppressedBuffs = GetSuppressedBuffs();
            foreach (BlueprintBuff buff in suppressedBuffs) {
                try {
                    unitPartBuffSuppress.Suppress(buff);
                } catch { }
            }
        }

        public override void OnDeactivate() {
            IEnumerable<BlueprintBuff> suppressedBuffs = GetSuppressedBuffs();
            base.OnTurnOff();
            UnitPartBuffSuppress unitPartBuffSuppress = Owner.Get<UnitPartBuffSuppress>();
            if (!unitPartBuffSuppress) {
                return;
            }
            foreach (BlueprintBuff buff in suppressedBuffs) {
                try {
                    unitPartBuffSuppress.Release(buff);
                } catch { }
            }
        }

        private IEnumerable<BlueprintBuff> GetSuppressedBuffs() {
            List<BlueprintBuff> suppressedBuffs = new List<BlueprintBuff>();
            foreach (Buff buff in Owner.Buffs) {
                if (IsSuppressed(buff)) {
                    suppressedBuffs.Add(buff.Blueprint);
                }
            }
            return suppressedBuffs;
        }

        private bool IsSuppressed(Buff buff) {
            bool test1 = Buffs.Contains(buff.Blueprint);
            bool test2 = (buff.Context.SpellDescriptor & Descriptor) > SpellDescriptor.None;
            bool test3 = Schools.Contains(buff.Context.SpellSchool);
            bool result = test1 || test2 || test3;
            //Main.LogDebug($"{result} - {test1}/{test2}/{test3} - {buff.Blueprint.name}");
            return result;
        }

#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField]
        [FormerlySerializedAs("Buffs")]
        private BlueprintBuffReference[] m_Buffs = new BlueprintBuffReference[0];
        [SerializeField]
        public SpellSchool[] Schools = new SpellSchool[0];
        [SerializeField]
        public SpellDescriptorWrapper Descriptor = SpellDescriptor.None;
#pragma warning restore IDE0044 // Add readonly modifier
    }
}
