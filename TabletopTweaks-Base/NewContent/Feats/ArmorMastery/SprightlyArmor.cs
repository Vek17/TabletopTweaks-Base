using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ArmorMastery {
    static class SprightlyArmor {
        internal static void AddSprightlyArmor() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorFocusLight = BlueprintTools.GetBlueprint<BlueprintFeature>("3bc6e1d2b44b5bb4d92e6ba59577cf62");
            var ArmorFocusMedium = BlueprintTools.GetBlueprint<BlueprintFeature>("7dc004879037638489b64d5016997d12");
            var ArmorFocusHeavy = BlueprintTools.GetBlueprint<BlueprintFeature>("c27e6d2b0d33d42439f512c6d9a6a601");

            var LightArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
            var MediumArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");
            var HeavyArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("1b0f68188dcc435429fb87a022239681");

            var SprightlyArmorEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SprightlyArmorEffect", bp => {
                bp.SetName(TTTContext, "Sprightly Armor");
                bp.SetDescription(TTTContext, "You make the most of your armor’s magical capabilities.\n" +
                    "Benefit: While wearing light armor you add your armor’s enhancement bonus as a bonus on your initiative checks.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddArmorEnhancementBonusToStat>(c => {
                    c.Stat = StatType.Initiative;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var SprightlyArmorFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SprightlyArmorFeature", bp => {
                bp.SetName(SprightlyArmorEffect.m_DisplayName);
                bp.SetDescription(SprightlyArmorEffect.m_Description);
                bp.m_Icon = ArmorFocusLight.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = SprightlyArmorEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Value = 13;
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 8;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 11;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ArmorFocusLight);
            });

            if (TTTContext.AddedContent.ArmorMasteryFeats.IsDisabled("SprightlyArmor")) { return; }
            ArmorMastery.AddToArmorMasterySelection(SprightlyArmorFeature);
        }
    }
}
