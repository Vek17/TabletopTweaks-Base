using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class HarmoniousMage {
        public static void AddHarmoniousMage() {
            var SpecialisationSchoolUniversalistProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("0933849149cfc9244ac05d6a5b57fd80");
            var OppositionSchoolSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("6c29030e9fea36949877c43a6f94ff31");

            var HarmoniousMageFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "HarmoniousMageFeature", bp => {
                bp.SetName(TTTContext, "Harmonious Mage");
                bp.SetDescription(TTTContext, "Your wizardly studies have moved beyond the concept of opposition schools. " +
                    "Preparing spells from one of your opposition schools now only requires one spell slot " +
                    "of the appropriate level instead of two.");
                bp.m_Icon = SpecialisationSchoolUniversalistProgression.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<Kingmaker.Designers.Mechanics.Facts.HarmoniousMage>();
                bp.AddPrerequisiteFeature(OppositionSchoolSelection);
            });

            //if (TTTContext.AddedContent.MythicAbilities.IsDisabled("HarmoniousMage")) { return; }
            //FeatTools.AddAsMythicAbility(HarmoniousMageFeature);
        }
    }
}
