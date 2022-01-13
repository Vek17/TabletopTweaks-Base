using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class NauseatedPoision {
        public static void AddNauseatedPoision() {
            var Nauseated = Resources.GetBlueprintReference<BlueprintBuffReference>("956331dba5125ef48afe41875a00ca0e");
            /*
            var NauseatedPoisionTTT = Helpers.CreateBuff("NauseatedPoisionTTT", bp => {
                bp.SetName("");
                bp.SetDescription("");
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
