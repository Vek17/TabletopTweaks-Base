using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class FavoriteMetamagicPersistent {
        public static void AddFavoriteMetamagicPersistent() {
            var PersistentSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
            var FavoriteMetamagicSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");

            var FavoriteMetamagicPersistent = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "FavoriteMetamagicPersistent", bp => {
                bp.SetName(TTTContext, "Favorite Metamagic — Persistent");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicPersistent;
                });
                bp.AddPrerequisiteFeature(PersistentSpellFeat);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("FavoriteMetamagicPersistent")) { return; }
            MetamagicExtention.RegisterMetamagic(
                context: TTTContext,
                metamagic: Metamagic.Persistent,
                name: "",
                icon: null,
                defaultCost: 2,
                CustomMechanicsFeature.FavoriteMetamagicPersistent
            );
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicPersistent);
        }
    }
}
