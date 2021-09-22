using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class CriticalDeflection {
        public static void AddCriticalDeflection() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");

            var CriticalDeflectionEffect = Helpers.CreateBlueprint<BlueprintFeature>("CriticalDeflectionEffect", bp => {
                bp.SetName("Critical Deflection");
                bp.SetDescription("Critical Deflection");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<CriticalConfirmationACBonus>(c => {
                    c.Bonus = 2;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 4;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });
            var CriticalDeflectionFeature = Helpers.CreateBlueprint<BlueprintFeature>("CriticalDeflectionFeature", bp => {
                bp.SetName("Critical Deflection");
                bp.SetDescription("While wearing armor or using a shield, the fighter gains a +2 bonus to his AC against attack rolls made to " +
                    "confirm a critical hit. This bonus increases by 1 at 7th level and every 4 fighter levels thereafter, to a maximum of +6 at 19th level.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = CriticalDeflectionEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light,
                        ArmorProficiencyGroup.Medium,
                        ArmorProficiencyGroup.Heavy,
                        ArmorProficiencyGroup.Buckler,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                }));
            });

            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.IsDisabled("CriticalDeflection")) { return; }
            AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(CriticalDeflectionFeature);
        }
    }
}
