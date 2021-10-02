using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Validation;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [ComponentName("Buffs/AddEffect/DR")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("84824fcead6d41bf8d8a683b817a4e6b")]
    public class TTAddDamageResistancePhysical : TTAddDamageResistanceBase {
        public bool Or;
        [Header("Material")]
        public bool BypassedByMaterial;
        [ShowIf("BypassedByMaterial")]
        public PhysicalDamageMaterial Material = PhysicalDamageMaterial.Adamantite;
        [Header("Form")]
        public bool BypassedByForm;
        [EnumFlagsAsButtons(ColumnCount = 3)]
        public PhysicalDamageForm Form;
        [Header("Magic")]
        public bool BypassedByMagic;
        [ShowIf("BypassedByMagic")]
        public int MinEnhancementBonus = 1;
        [Header("Alignment")]
        public bool BypassedByAlignment;
        [EnumFlagsAsButtons(ColumnCount = 4)]
        public DamageAlignment Alignment = DamageAlignment.Good;
        [Header("Reality")]
        public bool BypassedByReality;
        [ShowIf("BypassedByReality")]
        public DamageRealityType Reality = DamageRealityType.Ghost;
        [Header("Weapon")]
        public bool BypassedByWeaponType;
        [ShowIf("BypassedByWeaponType")]
        [SerializeField]
        [FormerlySerializedAs("WeaponType")]
        private BlueprintWeaponTypeReference m_WeaponType;
        public bool BypassedByMeleeWeapon;
        [Header("Epic")]
        public bool BypassedByEpic;
        [ShowIf("BypassedByEpic")]
        public BlueprintUnitFactReference m_CheckedFactMythic;
        private const PhysicalDamageMaterial MaterialMaskPlus3 = PhysicalDamageMaterial.ColdIron | PhysicalDamageMaterial.Silver;
        private const PhysicalDamageMaterial MaterialMaskPlus4 = PhysicalDamageMaterial.ColdIron | PhysicalDamageMaterial.Silver | PhysicalDamageMaterial.Adamantite;

        public BlueprintUnitFact CheckedFactMythic => m_CheckedFactMythic?.Get();

        public BlueprintWeaponType WeaponType => m_WeaponType?.Get();

        public bool IsAbsolute => !BypassedByAlignment && !BypassedByForm && !BypassedByMagic && !BypassedByMaterial && !BypassedByReality && !BypassedByMeleeWeapon && !BypassedByWeaponType && !BypassedByEpic;

        protected override bool Bypassed(
            ComponentRuntime runtime,
            BaseDamage d,
            ItemEntityWeapon weapon) {
            if (!(d is PhysicalDamage damage))
                return true;
            return Or ? BypassedByForm && damage.Form.Intersects(Form) || BypassedByMagic && damage.EnchantmentTotal >= MinEnhancementBonus || BypassedByMaterial && CheckBypassedByMaterial(damage) || BypassedByReality && (damage.Reality & Reality) != 0 || BypassedByAlignment && CheckBypassedByAlignment(damage) || BypassedByWeaponType && weapon?.Blueprint.Type == WeaponType || BypassedByMeleeWeapon && weapon != null && weapon.Blueprint.IsMelee || BypassedByEpic && CheckBypassedByEpic(weapon) : (BypassedByForm || BypassedByMagic || BypassedByMaterial || BypassedByReality || BypassedByAlignment || BypassedByWeaponType || BypassedByMeleeWeapon) && (!BypassedByForm || damage.Form.Intersects(Form)) && (!BypassedByMagic || damage.EnchantmentTotal >= MinEnhancementBonus) && (!BypassedByMaterial || CheckBypassedByMaterial(damage)) && (!BypassedByReality || (damage.Reality & Reality) != 0) && (!BypassedByAlignment || CheckBypassedByAlignment(damage)) && (!BypassedByWeaponType || weapon?.Blueprint.Type == WeaponType) && (!BypassedByMeleeWeapon || (weapon != null ? weapon.Blueprint.IsMelee ? 1 : 0 : 1) != 0) && (!BypassedByEpic || !CheckBypassedByEpic(weapon));
        }

        private bool CheckBypassedByMaterial(PhysicalDamage damage) {
            if ((damage.MaterialsMask & Material) != 0 || damage.Enchantment >= 3 && ((PhysicalDamageMaterial.ColdIron | PhysicalDamageMaterial.Silver) & Material) != 0)
                return true;
            return damage.Enchantment >= 4 && (uint)((PhysicalDamageMaterial.ColdIron | PhysicalDamageMaterial.Silver | PhysicalDamageMaterial.Adamantite) & Material) > 0U;
        }

        private bool CheckBypassedByAlignment(PhysicalDamage damage) => (damage.AlignmentsMask & Alignment) != 0 || damage.EnchantmentTotal >= 5;

        private bool CheckBypassedByEpic(ItemEntityWeapon weapon) => weapon.IsEpic || weapon.Owner.HasFact(CheckedFactMythic);

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other) {
            return other is TTAddDamageResistancePhysical otherDR
                && Or == otherDR.Or
                && BypassedByMaterial == otherDR.BypassedByMaterial
                && (!BypassedByMaterial || Material == otherDR.Material)
                && BypassedByForm == otherDR.BypassedByForm
                && (!BypassedByForm || Form == otherDR.Form)
                && BypassedByMagic == otherDR.BypassedByMagic
                && (!BypassedByMagic || MinEnhancementBonus == otherDR.MinEnhancementBonus)
                && BypassedByAlignment == otherDR.BypassedByAlignment
                && (!BypassedByAlignment || Alignment == otherDR.Alignment)
                && BypassedByReality == otherDR.BypassedByReality
                && (!BypassedByReality || Reality == otherDR.Reality)
                && BypassedByWeaponType == otherDR.BypassedByWeaponType
                && (!BypassedByWeaponType || m_WeaponType.Equals(otherDR.m_WeaponType))
                && BypassedByMeleeWeapon == otherDR.BypassedByMeleeWeapon
                && BypassedByEpic == otherDR.BypassedByEpic
                && (m_CheckedFactMythic == null && otherDR.m_CheckedFactMythic == null || m_CheckedFactMythic != null && otherDR.m_CheckedFactMythic != null && m_CheckedFactMythic.Equals(otherDR.m_CheckedFactMythic));
        }

        public override void ApplyValidation(ValidationContext context) {
            base.ApplyValidation(context);
            if (BypassedByWeaponType && !(bool)(SimpleBlueprint)WeaponType)
                context.AddError("WeaponType is missing!");
            if (!BypassedByForm || Form != 0)
                return;
            context.AddError("Physical damage Form is missing");
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance) {
            if (vanillaResistance is Kingmaker.UnitLogic.FactLogic.AddDamageResistancePhysical vanillaPhysicalResistance) {
                Or = vanillaPhysicalResistance.Or;
                BypassedByMaterial = vanillaPhysicalResistance.BypassedByMaterial;
                Material = vanillaPhysicalResistance.Material;
                BypassedByForm = vanillaPhysicalResistance.BypassedByForm;
                Form = vanillaPhysicalResistance.Form;
                BypassedByMagic = vanillaPhysicalResistance.BypassedByMagic;
                MinEnhancementBonus = vanillaPhysicalResistance.MinEnhancementBonus;
                BypassedByAlignment = vanillaPhysicalResistance.BypassedByAlignment;
                Alignment = vanillaPhysicalResistance.Alignment;
                BypassedByReality = vanillaPhysicalResistance.BypassedByReality;
                Reality = vanillaPhysicalResistance.Reality;
                BypassedByWeaponType = vanillaPhysicalResistance.BypassedByWeaponType;
                m_WeaponType = vanillaPhysicalResistance.m_WeaponType;
                BypassedByMeleeWeapon = vanillaPhysicalResistance.BypassedByMeleeWeapon;
                BypassedByEpic = vanillaPhysicalResistance.BypassedByEpic;
                m_CheckedFactMythic = vanillaPhysicalResistance.m_CheckedFactMythic;
            }
        }
    }
}
