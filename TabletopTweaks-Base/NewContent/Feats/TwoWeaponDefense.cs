using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class TwoWeaponDefense {
        public static void AddTwoWeaponDefense() {
            var Icon_TwoWeaponDefense = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_TwoWeaponDefense.png");
            var TwoWeaponFighting = BlueprintTools.GetBlueprint<BlueprintFeature>("ac8aaf29054f5b74eb18f2af950e752d");
            var TwoWeaponDefenseMythicFeature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "TwoWeaponDefenseMythicFeature");
            var FightDefensivelyBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6ffd93355fb3bcf4592a5d976b1d32a9");

            var TwoWeaponDefenseFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TwoWeaponDefenseFeature", bp => {
                bp.SetName(TTTContext, "Two-Weapon Defense");
                bp.SetDescription(TTTContext, "You are skilled at defending yourself while dual-wielding.\n" +
                    "When wielding a double weapon or two weapons (not including natural weapons or unarmed strikes), " +
                    "you gain a +1 shield bonus to your AC. When you are fighting defensively this shield bonus increases to +2.");
                bp.m_Icon = Icon_TwoWeaponDefense;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<TwoWeaponDefenseComponent>(c => {
                    c.m_FightDefensivelyBuff = FightDefensivelyBuff;
                    c.m_MythicBlueprint = TwoWeaponDefenseMythicFeature;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Defense;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;
                    c.Value = 15;
                });
                bp.AddPrerequisiteFeature(TwoWeaponFighting);
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("TwoWeaponDefense")) { return; }
            FeatTools.AddAsFeat(TwoWeaponDefenseFeature);
        }
    }
}
