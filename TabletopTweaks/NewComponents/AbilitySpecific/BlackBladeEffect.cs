using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("aac8d3adce2c4251ba6531b47d099186")]
    public class BlackBladeEffect : UnitFactComponentDelegate<BlackBladeEffect.AppliedEnchantData> {

        public override void OnTurnOn() {
            var part = base.Owner.Get<UnitPartBlackBlade>();
            if (part == null) { return; }
            Data.EnchantID = part.ApplyEnchantment(Enchantment, base.Context).UniqueId;
        }
        public override void OnTurnOff() {
            var part = base.Owner.Get<UnitPartBlackBlade>();
            if (part == null) { return; }
            if (!string.IsNullOrEmpty(Data.EnchantID)) {
                part.RemoveEnchantment(Data.EnchantID);
            }
            Data.EnchantID = null;
        }
        public bool BlackBladeStrike;
        public bool EnergyAttunement;
        public bool LifeDrinker;
        public BlueprintWeaponEnchantmentReference Enchantment;

        public class AppliedEnchantData {
            [JsonProperty]
            public string EnchantID;
        }
    }
}
