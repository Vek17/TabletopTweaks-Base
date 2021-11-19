using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class TrickRiding {
        public static void AddTrickRiding() {
            var Icon_TrickRiding = AssetLoader.LoadInternal("Feats", "Icon_TrickRiding.png");
            var MountedCombat = Resources.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
            var MountedCombatBuff = Resources.GetBlueprint<BlueprintBuff>("5008df9965da43c593c98ed7e6cacfc6");
            var MountedCombatCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
            var TrickRiding = Helpers.CreateBlueprint<BlueprintFeature>("TrickRiding", bp => {
                bp.SetName("Trick Riding");
                bp.SetDescription("You can make a check using Mounted Combat to negate a hit on your mount twice per round instead of just once.");
                bp.m_Icon = Icon_TrickRiding;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.MountedCombatFeat };
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SkillMobility;
                    c.Value = 9;
                });
                bp.AddPrerequisiteFeature(MountedCombat);
            });
            var TrickRidingCooldownBuff = Helpers.CreateBuff("TrickRidingCooldownBuff", bp => {
                bp.SetName("Trick Riding Cooldown");
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("TrickRiding")) { return; }
            MountedCombatBuff.RemoveComponents<MountedCombat>();
            MountedCombatBuff.AddComponent<MountedCombatTTT>(c => {
                c.m_CooldownBuff = MountedCombatCooldownBuff.ToReference<BlueprintBuffReference>();
                c.m_TrickRidingCooldownBuff = TrickRidingCooldownBuff.ToReference<BlueprintBuffReference>();
                c.m_TrickRidingFeature = TrickRiding.ToReference<BlueprintFeatureReference>();
            });
            FeatTools.AddAsFeat(TrickRiding);
        }
    }
}
