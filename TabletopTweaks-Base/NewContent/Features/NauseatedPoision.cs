using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Base.NewContent.Features {
    class NauseatedPoision {
        public static void AddNauseatedPoision() {
            var Nauseated = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("956331dba5125ef48afe41875a00ca0e");
            /*
            var NauseatedPoisionTTT = Helpers.CreateBlueprint<BlueprintBuff>("NauseatedPoisionTTT", bp => {
                bp.SetName(TTTContext, "");
                bp.SetDescription(TTTContext, "");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Nauseated | SpellDescriptor.Poison;
                });
                bp.AddComponent<RemoveWhenCombatEnded>();
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new ContextActionApplyBuff() { 
                            m_Buff = Nauseated,
                            Permanent = true,
                            DurationValue = new ContextDurationValue() { 
                                BonusValue = 0,
                                DiceCountValue = 0,
                                m_IsExtendable = true
                            },
                            AsChild = true
                        }    
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
            });
            */
        }
    }
}
