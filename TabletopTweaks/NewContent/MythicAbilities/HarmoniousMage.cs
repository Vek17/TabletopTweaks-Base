using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class HarmoniousMage {
        public static void AddHarmoniousMage() {
            var SpecialisationSchoolUniversalistProgression = Resources.GetBlueprint<BlueprintProgression>("0933849149cfc9244ac05d6a5b57fd80");
            var OppositionSchoolSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("6c29030e9fea36949877c43a6f94ff31");

            var HarmoniousMageFeature = Helpers.CreateBlueprint<BlueprintFeature>("HarmoniousMageFeature", bp => {
                bp.SetName("Harmonious Mage");
                bp.SetDescription("Your wizardly studies have moved beyond the concept of opposition schools. " +
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

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("HarmoniousMage")) { return; }
            FeatTools.AddAsMythicAbility(HarmoniousMageFeature);
        }
    }
}
