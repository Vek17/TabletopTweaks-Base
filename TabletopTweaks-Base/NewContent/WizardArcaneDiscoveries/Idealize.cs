using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewEvents;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.WizardArcaneDiscoveries {
    static class Idealize {
        public static void AddIdealize() {
            var WizardClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");
            var ThassilonianEnchantmentFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("e1ebc61a71c55054991863a5f6f6d2c2");
            var ThassilonianIllusionFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("aa271e69902044b47a8e62c4e58a9dcb");

            var IdealizeUpgrade = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, $"IdealizeUpgrade", bp => {
                bp.SetName(TTTContext, $"Idealize Upgrade");
                bp.SetDescription(TTTContext, "In your quest for self-perfection, you have discovered a way to further enhance yourself and others.\n" +
                    "When a transmutation spell you cast grants an enhancement bonus to an ability score, that bonus increases by 2. " +
                    "At 20th level, the bonus increases by 4.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.IdealizeDiscoveryUpgrade;
                });
            });
            var Idealize = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, $"Idealize", bp => {
                bp.SetName(TTTContext, $"Idealize");
                bp.SetDescription(TTTContext, "In your quest for self-perfection, you have discovered a way to further enhance yourself and others.\n" +
                    "When a transmutation spell you cast grants an enhancement bonus to an ability score, that bonus increases by 2. " +
                    "At 20th level, the bonus increases by 4.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.IdealizeDiscovery;
                });
                bp.AddComponent<AddFeatureOnClassLevel>(c => {
                    c.m_Feature = IdealizeUpgrade.ToReference<BlueprintFeatureReference>();
                    c.m_Class = WizardClass;
                    c.Level = 20;
                    c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
                    c.m_Archetypes = new BlueprintArchetypeReference[0];
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(p => {
                    p.m_CharacterClass = WizardClass;
                    p.Level = 10;
                    p.Group = Prerequisite.GroupType.All;
                });
                bp.AddPrerequisite<PrerequisiteNoFeature>(p => {
                    p.m_Feature = ThassilonianEnchantmentFeature;
                    p.Group = Prerequisite.GroupType.All;
                });
                bp.AddPrerequisite<PrerequisiteNoFeature>(p => {
                    p.m_Feature = ThassilonianIllusionFeature;
                    p.Group = Prerequisite.GroupType.All;
                });
            });
            EventBus.Subscribe(IdealizeMechanics.Instance);
            if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("Idealize")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(Idealize);
        }
        private class IdealizeMechanics : IStatBonusCalculatedHandler, IGlobalSubscriber {

            private IdealizeMechanics() { }
            public static IdealizeMechanics Instance = new();
            public void StatBonusCalculated(ref int value, StatType stat, ModifierDescriptor descriptor, Buff buff) {
                if (descriptor != ModifierDescriptor.Enhancement) { return; }

                var context = buff?.MaybeContext;
                var owner = context?.MaybeOwner;
                var caster = context?.MaybeCaster;
                var attribute = owner?.Stats?.GetAttribute(stat);

                if (owner == null) { return; }
                if (context == null) { return; }
                if (caster == null) { return; }
                if (attribute == null || value < 0) { return; }
                if (context.SpellSchool != SpellSchool.Transmutation) { return;}
                var metadataPart = owner.Get<UnitPartBuffMetadata>();
                if (metadataPart != null && metadataPart.HasBuff(buff)) {
                    if (metadataPart.HasBuffWithFeature(buff, CustomMechanicsFeature.IdealizeDiscovery)) {
                        value += 2;
                    }
                    if (metadataPart.HasBuffWithFeature(buff, CustomMechanicsFeature.IdealizeDiscoveryUpgrade)) {
                        value += 2;
                    }
                    return;
                }
                owner.Ensure<UnitPartBuffMetadata>().AddBuffEntry(buff);
                if (context.SourceItem != null) { return; }
                if (caster.CustomMechanicsFeature(CustomMechanicsFeature.IdealizeDiscovery)) {
                    value += 2;
                    owner.Ensure<UnitPartBuffMetadata>().AddBuffEntry(buff, CustomMechanicsFeature.IdealizeDiscovery);
                }
                if (caster.CustomMechanicsFeature(CustomMechanicsFeature.IdealizeDiscoveryUpgrade)) {
                    value += 2;
                    owner.Ensure<UnitPartBuffMetadata>().AddBuffEntry(buff, CustomMechanicsFeature.IdealizeDiscoveryUpgrade);
                }
            }
        }
    }
}
