using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    class AbundantSpellKenning {
        public static void AddAbundantSpellKenning() {
            var SkaldSpellKenning = BlueprintTools.GetBlueprint<BlueprintFeature>("d385b8c302e720c43aa17b8170bc6ae2");
            var SkaldSpellKenningResource = BlueprintTools.GetModBlueprintReference<BlueprintAbilityResourceReference>(TTTContext, "SkaldSpellKenningResource");

            var AbundantSpellKenning = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AbundantSpellKenning", bp => {
                bp.SetName(TTTContext, "Abundant Spell Kenning");
                bp.SetDescription(TTTContext, "You've mastery of magic allows you to more redily duplicate spells of other classes.\n" +
                    "Benefit: You can use Spell Kenning a number of additional times per day equal to one thrid your mythic rank.");
                bp.m_Icon = SkaldSpellKenning.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_Resource = SkaldSpellKenningResource;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.DivStep;
                    c.m_StepLevel = 3;
                });
                bp.AddPrerequisiteFeature(SkaldSpellKenning);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AbundantSpellKenning")) { return; }
            FeatTools.AddAsMythicAbility(AbundantSpellKenning);
        }
    }
}
