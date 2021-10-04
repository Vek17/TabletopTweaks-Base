using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.Prerequisites;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    class DeadeyesBlessing {
        private static readonly BlueprintFeature FollowsErastil = Resources.GetBlueprint<BlueprintFeature>("afc775188deb7a44aa4cbde03512c671");
        private static readonly BlueprintFeature ZenArchery = Resources.GetBlueprint<BlueprintFeature>("379c0da9f384e7547a70c259445377f5");
        private static readonly BlueprintFeature WeaponFocus = Resources.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");

        public static void AddDeadeyesBlessing() {

            var deadeyesBlessing = Helpers.CreateBlueprint<BlueprintFeature>("DeadeyesBlessingFeat", bp => {
                bp.SetName("Deadeye's Blessing");
                bp.SetDescription("Your deity grants you prowess with a bow that far exceeds your own physical capabilities.\n\nYou can use your Wisdom modifier instead of your Dexterity modifier on ranged attack rolls when using a bow.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent(ZenArchery.Components[0]);
                bp.AddPrerequisiteFeature(FollowsErastil);
                bp.AddComponent(Helpers.Create<PrerequisiteParametrizedFeature>(c => {
                    c.m_Feature = WeaponFocus.ToReference<BlueprintFeatureReference>();
                    c.m_Spell = null;
                    c.ParameterType = FeatureParameterType.WeaponCategory;
                    c.WeaponCategory = WeaponCategory.Longbow;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Ranged;
                }));
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("DeadeyesBlessing")) { return; }
            FeatTools.AddAsFeat(deadeyesBlessing);
        }
    }
}
