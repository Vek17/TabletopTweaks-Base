using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class AeonBaneIncreaseResourceFeature {
        public static void AddAeonBaneIncreaseResourceFeature() {
            var InquistorClass = Resources.GetBlueprint<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");
            var InquisitorBaneResource = Resources.GetBlueprint<BlueprintAbilityResource>("a708945b17c56fa4196e8d20f8af1b0d");
            var AeonBaneFeature = Resources.GetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
            var AeonBaneIncreaseResourceFeature = Helpers.CreateBlueprint<BlueprintFeature>("AeonBaneIncreaseResourceFeature", bp => {
                bp.HideInUI = true;
                bp.SetName("Aeon Bane Increase Resource Feature");
                bp.SetDescription("");
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.ComponentsArray = new BlueprintComponent[0];
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.m_Resource = InquisitorBaneResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 1;
                    c.m_Max = 20;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_Class = ResourcesLibrary.GetRoot().Progression.CharacterMythics.Append(InquistorClass)
                        //.Where(x => !x.Equals(InquistorClass))
                        .Select(x => x.ToReference<BlueprintCharacterClassReference>())
                        .ToArray();
                    c.m_ExceptClasses = true;
                });
            });
        }
    }
}
