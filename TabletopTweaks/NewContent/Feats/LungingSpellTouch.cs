using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.CustomStatTypes;

namespace TabletopTweaks.NewContent.Feats {
    static class LungingSpellTouch {
        public static void AddLungingSpellTouch() {
            var Icon_LungingSpellTouch = AssetLoader.LoadInternal("Feats", "Icon_LungingSpellTouch.png");
            var MountedCombat = Resources.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
            var TrickRiding = Resources.GetModBlueprint<BlueprintFeature>("TrickRiding");
            var LungingSpellTouchBuff = Helpers.CreateBuff("LungingSpellTouchBuff", bp => {
                bp.SetName("Lunging Spell Touch Buff");
                bp.SetDescription("");
                bp.m_Icon = Icon_LungingSpellTouch;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.MeleeTouchReach.Stat();
                    c.Value = 5;
                    c.Descriptor = ModifierDescriptor.Feat;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Value = -2;
                    c.Descriptor = ModifierDescriptor.Feat;
                });
            });
            var LungingSpellTouchAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>("LungingSpellTouchAbility", bp => {
                bp.SetName("Lunging Spell Touch");
                bp.SetDescription("You can increase the reach of your spells’ melee touch attacks by 5 feet until the end of your turn " +
                    "by taking a –2 penalty to your AC until your next turn. You must decide to use this ability before you attempt any attacks on your turn.");
                bp.m_Icon = Icon_LungingSpellTouch;
                bp.m_Buff = LungingSpellTouchBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = true;
                bp.DoNotTurnOffOnRest = true;
            });
            var LungingSpellTouchFeature = Helpers.CreateBlueprint<BlueprintFeature>("LungingSpellTouchFeature", bp => {
                bp.SetName("Lunging Spell Touch");
                bp.SetDescription("You can increase the reach of your spells’ melee touch attacks by 5 feet until the end of your turn " +
                    "by taking a –2 penalty to your AC until your next turn. You must decide to use this ability before you attempt any attacks on your turn.");
                bp.m_Icon = Icon_LungingSpellTouch;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        LungingSpellTouchAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillKnowledgeArcana;
                    c.Value = 6;
                });
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("LungingSpellTouch")) { return; }
            FeatTools.AddAsFeat(LungingSpellTouchFeature);
        }
    }
}
