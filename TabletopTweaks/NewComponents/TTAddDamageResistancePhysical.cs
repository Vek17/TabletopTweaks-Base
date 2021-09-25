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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents
{
    [ComponentName("Buffs/AddEffect/DR")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [AllowMultipleComponents]
    [TypeId("84824fcead6d41bf8d8a683b817a4e6b")]
    public class TTAddDamageResistancePhysical : TTAddDamageResistanceBase
    {
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

        public BlueprintUnitFact CheckedFactMythic => this.m_CheckedFactMythic?.Get();

        public BlueprintWeaponType WeaponType => this.m_WeaponType?.Get();

        public bool IsAbsolute => !this.BypassedByAlignment && !this.BypassedByForm && !this.BypassedByMagic && !this.BypassedByMaterial && !this.BypassedByReality && !this.BypassedByMeleeWeapon && !this.BypassedByWeaponType && !this.BypassedByEpic;

        protected override bool Bypassed(
            TTAddDamageResistanceBase.ComponentRuntime runtime,
            BaseDamage d,
            ItemEntityWeapon weapon)
        {
            if (!(d is PhysicalDamage damage))
                return true;
            return this.Or ? this.BypassedByForm && damage.Form.Intersects(this.Form) || this.BypassedByMagic && damage.EnchantmentTotal >= this.MinEnhancementBonus || this.BypassedByMaterial && this.CheckBypassedByMaterial(damage) || this.BypassedByReality && (damage.Reality & this.Reality) != (DamageRealityType)0 || this.BypassedByAlignment && this.CheckBypassedByAlignment(damage) || this.BypassedByWeaponType && weapon?.Blueprint.Type == this.WeaponType || this.BypassedByMeleeWeapon && weapon != null && weapon.Blueprint.IsMelee || this.BypassedByEpic && this.CheckBypassedByEpic(weapon) : (this.BypassedByForm || this.BypassedByMagic || this.BypassedByMaterial || this.BypassedByReality || this.BypassedByAlignment || this.BypassedByWeaponType || this.BypassedByMeleeWeapon) && (!this.BypassedByForm || damage.Form.Intersects(this.Form)) && (!this.BypassedByMagic || damage.EnchantmentTotal >= this.MinEnhancementBonus) && (!this.BypassedByMaterial || this.CheckBypassedByMaterial(damage)) && (!this.BypassedByReality || (damage.Reality & this.Reality) != (DamageRealityType)0) && (!this.BypassedByAlignment || this.CheckBypassedByAlignment(damage)) && (!this.BypassedByWeaponType || weapon?.Blueprint.Type == this.WeaponType) && (!this.BypassedByMeleeWeapon || (weapon != null ? (weapon.Blueprint.IsMelee ? 1 : 0) : 1) != 0) && (!this.BypassedByEpic || !this.CheckBypassedByEpic(weapon));
        }

        private bool CheckBypassedByMaterial(PhysicalDamage damage)
        {
            if ((damage.MaterialsMask & this.Material) != (PhysicalDamageMaterial)0 || damage.Enchantment >= 3 && ((PhysicalDamageMaterial.ColdIron | PhysicalDamageMaterial.Silver) & this.Material) != (PhysicalDamageMaterial)0)
                return true;
            return damage.Enchantment >= 4 && (uint)((PhysicalDamageMaterial.ColdIron | PhysicalDamageMaterial.Silver | PhysicalDamageMaterial.Adamantite) & this.Material) > 0U;
        }

        private bool CheckBypassedByAlignment(PhysicalDamage damage) => (damage.AlignmentsMask & this.Alignment) != (DamageAlignment)0 || damage.EnchantmentTotal >= 5;

        private bool CheckBypassedByEpic(ItemEntityWeapon weapon) => weapon.IsEpic || weapon.Owner.HasFact((BlueprintFact)this.CheckedFactMythic);

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other)
        {
            return other is TTAddDamageResistancePhysical otherDR
                && this.Or == otherDR.Or
                && this.BypassedByMaterial == otherDR.BypassedByMaterial
                && (!this.BypassedByMaterial || this.Material == otherDR.Material)
                && this.BypassedByForm == otherDR.BypassedByForm
                && (!this.BypassedByForm || this.Form == otherDR.Form)
                && this.BypassedByMagic == otherDR.BypassedByMagic
                && (!this.BypassedByMagic || this.MinEnhancementBonus == otherDR.MinEnhancementBonus)
                && this.BypassedByAlignment == otherDR.BypassedByAlignment
                && (!this.BypassedByAlignment || this.Alignment == otherDR.Alignment)
                && this.BypassedByReality == otherDR.BypassedByReality
                && (!this.BypassedByReality || this.Reality == otherDR.Reality)
                && this.BypassedByWeaponType == otherDR.BypassedByWeaponType
                && (!this.BypassedByWeaponType || this.m_WeaponType.Equals(otherDR.m_WeaponType))
                && this.BypassedByMeleeWeapon == otherDR.BypassedByMeleeWeapon
                && this.BypassedByEpic == otherDR.BypassedByEpic
                && ((this.m_CheckedFactMythic == null && otherDR.m_CheckedFactMythic == null) || (this.m_CheckedFactMythic != null && otherDR.m_CheckedFactMythic != null && this.m_CheckedFactMythic.Equals(otherDR.m_CheckedFactMythic)));
        }

        public override void ApplyValidation(ValidationContext context)
        {
            base.ApplyValidation(context);
            if (this.BypassedByWeaponType && !(bool)(SimpleBlueprint)this.WeaponType)
                context.AddError("WeaponType is missing!");
            if (!this.BypassedByForm || this.Form != (PhysicalDamageForm)0)
                return;
            context.AddError("Physical damage Form is missing");
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            if (vanillaResistance is Kingmaker.UnitLogic.FactLogic.AddDamageResistancePhysical vanillaPhysicalResistance)
            {
                this.Or = vanillaPhysicalResistance.Or;
                this.BypassedByMaterial = vanillaPhysicalResistance.BypassedByMaterial;
                this.Material = vanillaPhysicalResistance.Material;
                this.BypassedByForm = vanillaPhysicalResistance.BypassedByForm;
                this.Form = vanillaPhysicalResistance.Form;
                this.BypassedByMagic = vanillaPhysicalResistance.BypassedByMagic;
                this.MinEnhancementBonus = vanillaPhysicalResistance.MinEnhancementBonus;
                this.BypassedByAlignment = vanillaPhysicalResistance.BypassedByAlignment;
                this.Alignment = vanillaPhysicalResistance.Alignment;
                this.BypassedByReality = vanillaPhysicalResistance.BypassedByReality;
                this.Reality = vanillaPhysicalResistance.Reality;
                this.BypassedByWeaponType = vanillaPhysicalResistance.BypassedByWeaponType;
                this.m_WeaponType = vanillaPhysicalResistance.m_WeaponType;
                this.BypassedByMeleeWeapon = vanillaPhysicalResistance.BypassedByMeleeWeapon;
                this.BypassedByEpic = vanillaPhysicalResistance.BypassedByEpic;
                this.m_CheckedFactMythic = vanillaPhysicalResistance.m_CheckedFactMythic;
            }
        }
    }
}
