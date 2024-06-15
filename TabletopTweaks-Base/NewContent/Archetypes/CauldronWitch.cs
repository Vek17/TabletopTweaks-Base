using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    static class CauldronWitch {
        public static void AddCauldrenWitch() {
            if (TTTContext.AddedContent.Archetypes.IsDisabled("CauldronWitchArchetype")) { return; }

            var CauldronWitchArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("e0012a7015774e140be217f4a1480b6f");
            var CauldronWitchMixtureAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("bde7657b0338dab4e835673725abd385");
            var WitchClass = ClassTools.ClassReferences.WitchClass;
            var WitchPatronSelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("381cf4c890815d049a4420c6f31d063f");
            var WitchHexCauldronFeature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureBaseReference>(TTTContext, "WitchHexCauldronFeature");
            var WitchHexSelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("9846043cf51251a4897728ed6e24e76f");

            CauldronWitchMixtureAbility.AbilityAndVariants().ForEach(ability => {
                ability.GetComponents<ContextRankConfig>().ForEach(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.WitchClass, ClassTools.ClassReferences.WinterWitchClass };
                });
            });
            CauldronWitchMixtureAbility.AbilityAndVariants()
                .SelectMany(a => a.FlattenAllActions())
                .OfType<ContextActionApplyBuff>()
                .Select(a => a.Buff)
                .OfType<BlueprintBuff>()
                .Distinct()
                .ForEach(buff => {
                    buff.GetComponents<ContextRankConfig>().ForEach(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                        c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.WitchClass, ClassTools.ClassReferences.WinterWitchClass };
                    });
                });
            CauldronWitchArchetype.RemoveFeatures
                .Where(entry => entry.Level == 1)
                .ForEach(entry => {
                    entry.m_Features.Remove(WitchPatronSelection);
                });
            CauldronWitchArchetype.AddFeatures
                .Where(entry => entry.Level == 1)
                .First().m_Features.Add(WitchHexCauldronFeature);

            ClassTools.Classes.WitchClass.m_Archetypes = ClassTools.Classes.WitchClass.m_Archetypes.AppendToArray(CauldronWitchArchetype.ToReference<BlueprintArchetypeReference>());

            ClassTools.Classes.WitchClass.Progression.UIGroups = ClassTools.Classes.WitchClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(WitchHexSelection, WitchHexCauldronFeature)
            );

            TTTContext.Logger.LogPatch("Added", CauldronWitchArchetype);
        }
    }
}
