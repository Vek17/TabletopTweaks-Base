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
    static class FavoriteMetamagicPersistent {
        public static void AddFavoriteMetamagicPersistent() {
            var PersistentSpellFeat = Resources.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");

            var FavoriteMetamagicPersistent = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicPersistent", bp => {
                bp.SetName("Favorite Metamagic — Persistent");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicIntensified;
                });
                bp.AddPrerequisiteFeature(PersistentSpellFeat);
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("FavoriteMetamagicPersistent")) { return; }
            MetamagicExtention.RegisterMetamagic(
                metamagic: Metamagic.Persistent,
                name: "",
                icon: null,
                defaultCost: 2,
                CustomMechanicsFeature.FavoriteMetamagicIntensified
            );
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicPersistent);
        }
    }
}
