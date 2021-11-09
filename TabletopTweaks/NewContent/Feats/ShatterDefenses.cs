using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ShatterDefenses {
        public static void AddNewShatterDefenseBlueprints() {
            var ShatterDefenses = Resources.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");

            var ShatterDefensesDisplayBuff = Helpers.CreateBuff("ShatterDefensesDisplayBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.Stacking = StackingType.Prolong;
                bp.SetName("56668b2c5f96451fb57b458d35902072", "Shattered Defenses");
                bp.SetDescription("20806d2952534a0d8d2b82eb5fbcabcc", "An opponent you affect with Shatter Defenses is flat-footed to your attacks.");
            });
            var ShatterDefensesBuff = Helpers.CreateBuff("ShatterDefensesBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Stack;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = ShatterDefensesDisplayBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = false,
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = 2
                            },
                            AsChild = true
                        }
                    );
                    c.NewRound = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                });
                bp.AddComponent<ForceFlatFooted>(c => {
                    c.AgainstCaster = true;
                });
                bp.SetName("47e6903401314707aaae3d41615905a8", "Shattered Defenses");
                bp.SetDescription("ac61fae2e3e44490945f8f21f6edcde8", "An opponent you affect with Shatter Defenses is flat-footed to your attacks.");
            });
        }
    }
}
