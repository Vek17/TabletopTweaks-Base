using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;
namespace TabletopTweaks.Base.NewContent.Feats.ArmorMastery {
    static class IntenseBlows {
        internal static void AddIntenseBlows() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorFocusLight = BlueprintTools.GetBlueprint<BlueprintFeature>("3bc6e1d2b44b5bb4d92e6ba59577cf62");
            var ArmorFocusMedium = BlueprintTools.GetBlueprint<BlueprintFeature>("7dc004879037638489b64d5016997d12");
            var ArmorFocusHeavy = BlueprintTools.GetBlueprint<BlueprintFeature>("c27e6d2b0d33d42439f512c6d9a6a601");

            var LightArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
            var MediumArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");
            var HeavyArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("1b0f68188dcc435429fb87a022239681");

            var PowerAttackFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9972f33f977fc724c838e59641b2fca5");
            var PowerAttackBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5898bcf75a0942449a5dc16adc97b279");

            var IntenseBlowsEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "IntenseBlowsEffect", bp => {
                bp.SetName(TTTContext, "Intense Blows");
                bp.SetDescription(TTTContext, "Your forceful blows and your armor’s weight make you more difficult to hinder in combat.\n" +
                    "Benefit: When wearing heavy armor and using Power Attack, you gain a +1 bonus to your CMD until the beginning of your next turn. " +
                    "When your base attack bonus reaches +4, and every 4 points thereafter, this bonus increases by another 1.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddStatBonusIfHasFact>(c => {
                    c.Stat = StatType.AdditionalCMD;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { PowerAttackBuff };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = StatType.BaseAttackBonus;
                    c.m_Progression = ContextRankProgression.OnePlusDivStep;
                    c.m_StepLevel = 4;
                });
                bp.AddComponent<RecalculateOnLevelUp>();
            });
            var IntenseBlowsFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "IntenseBlowsFeature", bp => {
                bp.SetName(IntenseBlowsEffect.m_DisplayName);
                bp.SetDescription(IntenseBlowsEffect.m_Description);
                bp.m_Icon = ArmorFocusLight.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = IntenseBlowsEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Strength;
                    c.Value = 13;
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 4;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 6;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeature(PowerAttackFeature);
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ArmorFocusHeavy);
            });

            if (TTTContext.AddedContent.ArmorMasteryFeats.IsDisabled("IntenseBlows")) { return; }
            ArmorMastery.AddToArmorMasterySelection(IntenseBlowsFeature);
        }
    }
}
