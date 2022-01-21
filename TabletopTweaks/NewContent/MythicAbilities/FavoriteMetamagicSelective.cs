using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewContent.MechanicsChanges;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class FavoriteMetamagicSelective {
        public static void AddFavoriteMetamagicSelective() {
            var SelectiveSpellFeat = Resources.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");

            var FavoriteMetamagicPersistent = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicSelective", bp => {
                bp.SetName("Favorite Metamagic — Selective");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicIntensified;
                });
                bp.AddPrerequisiteFeature(SelectiveSpellFeat);
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("FavoriteMetamagicSelective")) { return; }
            MetamagicExtention.RegisterMetamagic(
                metamagic: Metamagic.Selective,
                name: "",
                icon: null,
                defaultCost: 1,
                CustomMechanicsFeature.FavoriteMetamagicIntensified
            );
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicPersistent);
        }
    }
}
