using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    class PerfectStrikeZenArcherBuff {
        public static void AddPerfectStrikeZenArcherBuff() {
            var PerfectStrikeOwnerBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("9a41e6d073b42564b9f00ad83b7d3b52");
            var PerfectStrikeZenArcherBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "PerfectStrikeZenArcherBuff", bp => {
                bp.SetName(PerfectStrikeOwnerBuff.m_DisplayName);
                bp.SetDescription(TTTContext, $"{PerfectStrikeOwnerBuff.Description}\n" +
                    $"At 10th level, the monk can roll his attack roll three times and take the highest result.");
                bp.m_Flags = PerfectStrikeOwnerBuff.m_Flags;
                bp.IsClassFeature = true;
                bp.m_Icon = PerfectStrikeOwnerBuff.Icon;
                bp.AddComponent(Helpers.Create<ModifyD20>(c => {
                    c.Rule = RuleType.AttackRoll;
                    c.RollsAmount = 2;
                    c.TakeBest = true;
                    c.Bonus = new ContextValue();
                    c.Chance = new ContextValue();
                    c.DispellOnRerollFinished = true;
                    c.Skill = new StatType[0];
                    c.Value = new ContextValue();
                }));
            });
            var PerfectStrikeZenArcherUpgrade = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "PerfectStrikeZenArcherUpgrade", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = PerfectStrikeOwnerBuff.Icon;
                bp.SetName(TTTContext, "Perfect Strike Upgrade");
                bp.SetDescription(TTTContext, "At 10th level, the zen archer can roll his attack roll three times and take the" +
                    " highest result when making a perfect strike.");
            });
        }
    }
}
