using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Units;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    [TypeId("44703c29059c48f8bd7d398b874c708e")]
    class SuppressBuffsEveryRound : UnitFactComponentDelegate, ITickEachRound {

        public ReferenceArrayProxy<BlueprintBuff, BlueprintBuffReference> Buffs {
            get {
                return m_Buffs;
            }
        }

        public void OnNewRound() {
            if (conditionsChecker.Check()) {
                UnitPartBuffSuppress unitPartBuffSuppress = base.Owner.Ensure<UnitPartBuffSuppress>();
                foreach (BlueprintBuff buff in m_SuppressedBuffs) {
                    unitPartBuffSuppress.Suppress(buff);
                }
            }
        }

        public override void OnActivate() {
            UnitPartBuffSuppress unitPartBuffSuppress = base.Owner.Ensure<UnitPartBuffSuppress>();
            List<BlueprintBuff> suppressedBuffs = new List<BlueprintBuff>();

            foreach (Buff buff in Owner.Buffs) {
                if (IsSuppressed(buff)) {
                    unitPartBuffSuppress.Suppress(buff.Blueprint);
                    suppressedBuffs.Add(buff.Blueprint);
                }
            }


            if (Descriptor != SpellDescriptor.None) {
                unitPartBuffSuppress.Suppress(Descriptor);
            }
            if (Schools != null && Schools.Length > 0) {
                unitPartBuffSuppress.Suppress(Schools);
            }
            foreach (BlueprintBuff buff in Buffs) {
                unitPartBuffSuppress.Suppress(buff);
            }
            m_SuppressedBuffs = suppressedBuffs.Select(bp => bp.ToReference<BlueprintBuffReference>()).ToArray();
        }

        public override void OnDeactivate() {
            base.OnTurnOff();
            UnitPartBuffSuppress unitPartBuffSuppress = base.Owner.Get<UnitPartBuffSuppress>();
            if (!unitPartBuffSuppress) {
                return;
            }
            if (Descriptor != SpellDescriptor.None) {
                unitPartBuffSuppress.Release(Descriptor);
            }
            if (Schools != null && Schools.Length > 0) {
                unitPartBuffSuppress.Release(Schools);
            }
            foreach (BlueprintBuff buff in Buffs) {
                unitPartBuffSuppress.Release(buff);
            }
        }

        private static IEnumerable<SpellDescriptor> GetValues(SpellDescriptor spellDescriptor) {


            return from v in EnumUtils.GetValues<SpellDescriptor>()
                   where v != SpellDescriptor.None && (spellDescriptor & v) > SpellDescriptor.None
                   select v;
        }

        public bool IsSuppressed(Buff buff) {
            return Buffs.Contains(buff.Blueprint) ||
                (Descriptor != SpellDescriptor.None &&
                UnitPartBuffSuppress.GetValues(buff.Context.SpellDescriptor).Any((SpellDescriptor d) => GetValues(Descriptor).Any(c => (c & d) > SpellDescriptor.None))) ||
                (Schools != null && Schools.Length > 0 &&
                Schools.Contains(buff.Context.SpellSchool));
        }
#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField]
        [FormerlySerializedAs("Buffs")]
        private BlueprintBuffReference[] m_Buffs = new BlueprintBuffReference[0];

        [SerializeField]
        private BlueprintBuffReference[] m_SuppressedBuffs = new BlueprintBuffReference[0];

        public SpellSchool[] Schools = new SpellSchool[0];

        public SpellDescriptorWrapper Descriptor = SpellDescriptor.None;


        public ConditionsChecker conditionsChecker = new ConditionsChecker();
#pragma warning restore IDE0044 // Add readonly modifier
    }
}
