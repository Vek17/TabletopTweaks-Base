using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.IO;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Spells {
    static class LongArms {
        public static void CreateLongArms() {
            var icon = AssetLoader.Image2Sprite.Create($"{ModSettings.ModEntry.Path}Assets{Path.DirectorySeparatorChar}Abilities{Path.DirectorySeparatorChar}Icon_LongArm.png");
            var LongArmBuff = Helpers.Create<BlueprintBuff>(bp => {
                bp.m_AssetGuid = ModSettings.Blueprints.NewBlueprints["LongArmBuff"];
                bp.name = "LongArmBuff";
                bp.SetName("Long Arm");
                bp.SetDescription("Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.");
                bp.m_Icon = icon;
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Stat = StatType.Reach;
                    c.Descriptor = ModifierDescriptor.Enhancement;
                    c.Value = 5;
                }));
            });
            var applyBuff = Helpers.Create<Kingmaker.UnitLogic.Mechanics.Actions.ContextActionApplyBuff>(bp => {
                bp.IsFromSpell = true;
                bp.m_Buff = LongArmBuff.ToReference<BlueprintBuffReference>();
                bp.Permanent = false;
                bp.IsNotDispelable = false;
                bp.DurationValue = new ContextDurationValue() {
                    Rate = DurationRate.Minutes,
                    BonusValue = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    },
                    DiceCountValue = new ContextValue(),
                    DiceType = DiceType.One
                };
            });
            var LongArmAbility = Helpers.Create<BlueprintAbility>(bp => {
                bp.m_AssetGuid = ModSettings.Blueprints.NewBlueprints["LongArmAbility"];
                bp.name = "LongArmAbility";
                bp.SetName("Long Arm");
                bp.SetDescription("Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.");
                bp.LocalizedDuration = Helpers.CreateString("LongArmAbility.Duration", "1 minute/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = icon;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList {
                        Actions = new GameAction[] { applyBuff }
                    };
                }));
                bp.AddComponent(Helpers.Create<SpellComponent>(c => {
                    c.School = SpellSchool.Transmutation;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_Max = 20;
                    c.m_Min = 1;
                    c.m_UseMin = false;
                }));
                //bp.AddComponent(Helpers.CreateContextRankConfig());
                bp.AddComponent(Helpers.Create<ContextCalculateAbilityParams>());
            });
            Resources.AddBlueprint(LongArmBuff);
            Resources.AddBlueprint(LongArmAbility);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.AlchemistSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.MagusSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 1);
            LongArmAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 1);
        }
    }
}
