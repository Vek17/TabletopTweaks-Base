using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    internal class BewitchingReflex {
        public static void AddBewitchingReflex() {
            var SorcerousReflexMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("f6f9f99993f9a92438bd4ffbfbb71837");

            var BewitchingReflexBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BewitchingReflexBuff", bp => {
                bp.SetName(TTTContext, "Bewitching Reflex");
                bp.SetDescription(TTTContext, "");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<BewitchingReflexComponent>();
            });
            var BewitchingReflex = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BewitchingReflex", bp => {
                bp.SetName(TTTContext, "Bewitching Reflex");
                bp.SetDescription(TTTContext, "You can cast the first hex after an initiative roll as a swift action.");
                bp.m_Icon = SorcerousReflexMythicFeat.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<CombatStateTrigger>(c => {
                    c.CombatStartActions = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = BewitchingReflexBuff.ToReference<BlueprintBuffReference>(),
                            Permanent = true,
                            DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            },
                            IsNotDispelable = true
                        }
                    );
                    c.CombatEndActions = Helpers.CreateActionList(
                        new ContextActionRemoveBuff() { 
                            m_Buff = BewitchingReflexBuff.ToReference<BlueprintBuffReference>()
                        }
                    );
                });
            });
            if (TTTContext.AddedContent.MythicFeats.IsDisabled("BewitchingReflex")) { return; }
            FeatTools.AddAsMythicFeat(BewitchingReflex);
        }
    }
}
