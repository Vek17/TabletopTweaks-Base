
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintFeature))]
    [AllowedOn(typeof(BlueprintBuff))]
    [TypeId("bbd018ab3de94718bbcdd0a737629873")]
    class BuffExtraEffectsRequirements : UnitFactComponentDelegate<BuffExtraEffectsRequirements.BuffExtraEffectsData>, IUnitBuffHandler, IGlobalSubscriber, ISubscriber {

        public override void OnActivate() {
            if (Owner.HasFact(CheckedBuff)) {
                base.Data.AppliedBuff = Owner.AddBuff(ExtraEffectBuff, base.Context, null);
            }
        }

        public override void OnDeactivate() {
            Owner.RemoveFact(ExtraEffectBuff);
        }

        public void HandleBuffDidAdded(Buff buff) {
            if (buff.Blueprint == CheckedBuff.Get() && buff.Owner == Owner && base.Data.AppliedBuff == null) {
                if (CheckWeaponCategory) {
                    var MainWeapon = Owner?.Body?.PrimaryHand?.MaybeWeapon?.Blueprint?.Category;
                    var SecondaryWeapon = Owner?.Body?.SecondaryHand?.MaybeWeapon?.Blueprint?.Category;
                    if (MainWeapon != WeaponCategory && SecondaryWeapon != WeaponCategory) {
                        return;
                    }
                }
                if (CheckFacts) {
                    foreach (var fact in CheckedFacts) {
                        if (!Owner.HasFact(fact)) { return; }
                    }
                }
                base.Data.AppliedBuff = Owner.AddBuff(ExtraEffectBuff, Context, null);
            }
        }

        public void HandleBuffDidRemoved(Buff buff) {
            if (buff.Blueprint == CheckedBuff.Get() && buff.Owner == Owner && !Owner.HasFact(CheckedBuff)) {
                Buff appliedBuff = base.Data.AppliedBuff;
                if (appliedBuff != null) {
                    appliedBuff.Remove();
                }
                base.Data.AppliedBuff = null;
            }
        }

        [SerializeField]
        [FormerlySerializedAs("CheckedBuff")]
        public BlueprintBuffReference CheckedBuff;

        [SerializeField]
        [FormerlySerializedAs("ExtraEffectBuff")]
        public BlueprintBuffReference ExtraEffectBuff;

        [SerializeField]
        public bool CheckFacts = false;

        [SerializeField]
        public BlueprintUnitFactReference[] CheckedFacts = new BlueprintUnitFactReference[0];

        [SerializeField]
        public bool CheckWeaponCategory = false;

        [SerializeField]
        public WeaponCategory WeaponCategory;

        [AllowMultipleComponents]
        public class BuffExtraEffectsData {
            [JsonProperty]
            public Buff AppliedBuff;
        }
    }
}
