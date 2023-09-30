using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class EldritchBreach {
        public static void AddEldritchBreach() {
            var DispelMagicGreater = BlueprintTools.GetBlueprint<BlueprintAbility>("f0f761b808dc4b149b08eaf44b99f633");

            var EldritchBreach = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "EldritchBreach", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = DispelMagicGreater.Icon;
                bp.SetName(TTTContext, "Eldritch Breach");
                bp.SetDescription(TTTContext, "You are adept at breaching magical defenses and overcoming resistance to your magic.\n" +
                    "When attempting a caster level check to dispel an effect or overcome spell resistance roll twice and take the higher result.");
                bp.AddComponent<ModifyD20>(c => {
                    c.Rule = RuleType.DispelMagic | RuleType.SpellResistance;
                    c.RollsAmount = 1;
                    c.TakeBest = true;
                });
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("EldritchBreach")) { return; }
            FeatTools.AddAsMythicAbility(EldritchBreach);
        }
    }
}
