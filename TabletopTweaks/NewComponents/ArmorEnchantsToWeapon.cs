using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [TypeId("1c41e810627b489bbdc47138903dbc86")]
    public class ArmorEnchantsToWeapon : WeaponEnchantmentLogic,
        ISubscriber, IInitiatorRulebookSubscriber,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage> {

        static readonly BlueprintArmorEnchantment AdamantineHeavy = Resources.GetBlueprint<BlueprintArmorEnchantment>("933456ff83c454146a8bf434e39b1f93");
        static readonly BlueprintArmorEnchantment AdamantineMedium = Resources.GetBlueprint<BlueprintArmorEnchantment>("aa25531ab5bb58941945662aa47b73e7");
        static readonly BlueprintArmorEnchantment Mithril = Resources.GetBlueprint<BlueprintArmorEnchantment>("7b95a819181574a4799d93939aa99aff");

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon == base.Owner) {
                var bonus = evt.Initiator?.Body?.Armor?.MaybeArmor?.EnchantmentValue ?? 0;
                if (bonus > 0) {
                    evt.Enhancement = bonus > evt.Enhancement ? bonus : evt.Enhancement;
                }
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
            if (evt.Weapon == base.Owner) {
                var bonus = evt.Initiator?.Body?.Armor?.MaybeArmor?.EnchantmentValue ?? 0;
                if (bonus > 0) {
                    evt.AddModifier(bonus, base.Fact, ModifierDescriptor.Enhancement);
                }
            }
        }

        public void OnEventAboutToTrigger(RulePrepareDamage evt) {
            var armorEnchants = evt.Initiator?.Body.Armor?.MaybeArmor?.Enchantments;
            PhysicalDamage physicalDamage;
            if (evt.DamageBundle.Weapon == base.Owner && (physicalDamage = (evt.DamageBundle.WeaponDamage as PhysicalDamage)) != null) {
                if (armorEnchants.Any(e => e.Blueprint == AdamantineHeavy || e.Blueprint == AdamantineMedium)) {
                    physicalDamage.AddMaterial(PhysicalDamageMaterial.Adamantite);
                }
                if (armorEnchants.Any(e => e.Blueprint == Mithril)) {
                    physicalDamage.AddMaterial(PhysicalDamageMaterial.ColdIron);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
        }
    }
}
