using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class TwoWeaponDefense {
        public static void AddTwoWeaponDefense() {
            var Icon_TwoWeaponDefense = AssetLoader.LoadInternal("Feats", "Icon_TwoWeaponDefense.png");
            var TwoWeaponFighting = Resources.GetBlueprint<BlueprintFeature>("ac8aaf29054f5b74eb18f2af950e752d");
            var TwoWeaponDefenseMythicFeature = Resources.GetModBlueprintReference<BlueprintFeatureReference>("TwoWeaponDefenseMythicFeature");
            var FightDefensivelyBuff = Resources.GetBlueprintReference<BlueprintBuffReference>("6ffd93355fb3bcf4592a5d976b1d32a9");

            var TwoWeaponDefenseFeature = Helpers.CreateBlueprint<BlueprintFeature>("TwoWeaponDefenseFeature", bp => {
                bp.SetName("Two-Weapon Defense");
                bp.SetDescription("You are skilled at defending yourself while dual-wielding.\n" +
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
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;
                    c.Value = 15;
                });
                bp.AddPrerequisiteFeature(TwoWeaponFighting);
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("TwoWeaponDefense")) { return; }
            FeatTools.AddAsFeat(TwoWeaponDefenseFeature);
        }
    }
}
