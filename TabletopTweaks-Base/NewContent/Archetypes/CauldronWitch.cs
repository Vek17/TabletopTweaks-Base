using HarmonyLib;
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
            var WitchClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("1b9873f1e7bfe5449bc84d03e9c8e3cc");

            CauldronWitchMixtureAbility.AbilityAndVariants().ForEach(ability => {
                ability.GetComponents<ContextRankConfig>().ForEach(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.Classes.WitchClass.ToReference<BlueprintCharacterClassReference>() };
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
                        c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.Classes.WitchClass.ToReference<BlueprintCharacterClassReference>() };
                    });
                });

            WitchClass.m_Archetypes = WitchClass.m_Archetypes.AddItem(CauldronWitchArchetype.ToReference<BlueprintArchetypeReference>()).ToArray();

            TTTContext.Logger.LogPatch("Added", CauldronWitchArchetype);
        }
    }
}
