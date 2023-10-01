using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalActivatableAbilityGroups;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class EnergyConversion {
        public static void AddEnergyConversion() {
            var Icon_EnergyConversion = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_EnergyConversion.png");
            var Icon_EnergyConversionAcid = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_EnergyConversionAcid.png");
            var Icon_EnergyConversionCold = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_EnergyConversionCold.png");
            var Icon_EnergyConversionElectricity = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_EnergyConversionElectricity.png");
            var Icon_EnergyConversionFire = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_EnergyConversionFire.png");

            var EnergyConversionBuffAcid = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "EnergyConversionBuffAcid", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Acid");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the cold, electricity, or fire descriptor " +
                    "you can switch the energy type to acid. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionAcid;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<ChangeSpellElementalDamage>(c => {
                    c.Element = DamageEnergyType.Acid;
                });
            });
            var EnergyConversionBuffCold = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "EnergyConversionBuffCold", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Cold");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, electricity, or fire descriptor " +
                    "you can switch the energy type to cold. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionCold;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<ChangeSpellElementalDamage>(c => {
                    c.Element = DamageEnergyType.Cold;
                });
            });
            var EnergyConversionBuffElectricity = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "EnergyConversionBuffElectricity", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Electricity");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, cold, electricity, or fire descriptor " +
                    "you can switch the energy type to a different one of those energy types. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionElectricity;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<ChangeSpellElementalDamage>(c => {
                    c.Element = DamageEnergyType.Electricity;
                });
            });
            var EnergyConversionBuffFire = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "EnergyConversionBuffFire", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Fire");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, cold, or electricity descriptor " +
                    "you can switch the energy type to fire. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionFire;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<ChangeSpellElementalDamage>(c => {
                    c.Element = DamageEnergyType.Fire;
                });
            });
            var EnergyConversionToggleAcid = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "EnergyConversionToggleAcid", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Acid");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the cold, electricity, or fire descriptor " +
                    "you can switch the energy type to acid. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionAcid;
                bp.m_Buff = EnergyConversionBuffAcid.ToReference<BlueprintBuffReference>();
                bp.Group = (ActivatableAbilityGroup)ExtentedActivatableAbilityGroup.EnergyConversion;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.HiddenInUI = true;
            });
            var EnergyConversionToggleCold = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "EnergyConversionToggleCold", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Cold");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, electricity, or fire descriptor " +
                    "you can switch the energy type to cold. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionCold;
                bp.m_Buff = EnergyConversionBuffCold.ToReference<BlueprintBuffReference>();
                bp.Group = (ActivatableAbilityGroup)ExtentedActivatableAbilityGroup.EnergyConversion;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.HiddenInUI = true;
            });
            var EnergyConversionToggleElectricity = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "EnergyConversionToggleElectricity", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Electricity");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, cold, or fire descriptor " +
                    "you can switch the energy type to electricity. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionElectricity;
                bp.m_Buff = EnergyConversionBuffElectricity.ToReference<BlueprintBuffReference>();
                bp.Group = (ActivatableAbilityGroup)ExtentedActivatableAbilityGroup.EnergyConversion;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.HiddenInUI = true;
            });
            var EnergyConversionToggleFire = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "EnergyConversionToggleFire", bp => {
                bp.SetName(TTTContext, "Energy Conversion — Fire");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, cold, or electricity descriptor " +
                    "you can switch the energy type to fire. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversionFire;
                bp.m_Buff = EnergyConversionBuffFire.ToReference<BlueprintBuffReference>();
                bp.Group = (ActivatableAbilityGroup)ExtentedActivatableAbilityGroup.EnergyConversion;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.HiddenInUI = true;
            });
            var EnergyConversionToggleVariants = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "EnergyConversionToggleVariants", bp => {
                bp.SetName(TTTContext, "Energy Conversion");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, cold, electricity, or fire descriptor " +
                    "you can switch the energy type to a different one of those energy types. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversion;
                bp.m_Buff = new BlueprintBuffReference();
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.AddComponent<ActivationDisable>();
                bp.AddComponent<ActivatableAbilityVariants>(c => {
                    c.m_Variants = new BlueprintActivatableAbilityReference[] {
                        EnergyConversionToggleAcid.ToReference<BlueprintActivatableAbilityReference>(),
                        EnergyConversionToggleCold.ToReference<BlueprintActivatableAbilityReference>(),
                        EnergyConversionToggleElectricity.ToReference<BlueprintActivatableAbilityReference>(),
                        EnergyConversionToggleFire.ToReference<BlueprintActivatableAbilityReference>()
                    };
                });
            });
            var EnergyConversion = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "EnergyConversion", bp => {
                bp.SetName(TTTContext, "Energy Conversion");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with the acid, cold, electricity, or fire descriptor " +
                    "you can switch the energy type to a different one of those energy types. " +
                    "If the spell normally has its original energy type as a descriptor, " +
                    "it loses that descriptor and gains the new type as a descriptor. " +
                    "All other effects of the spell remain unchanged.");
                bp.m_Icon = Icon_EnergyConversion;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        EnergyConversionToggleVariants.ToReference<BlueprintUnitFactReference>(),
                        EnergyConversionToggleAcid.ToReference<BlueprintUnitFactReference>(),
                        EnergyConversionToggleCold.ToReference<BlueprintUnitFactReference>(),
                        EnergyConversionToggleElectricity.ToReference<BlueprintUnitFactReference>(),
                        EnergyConversionToggleFire.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("EnergyConversion")) { return; }
            FeatTools.AddAsMythicAbility(EnergyConversion);
        }
    }
}
