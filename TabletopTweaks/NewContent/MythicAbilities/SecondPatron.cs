using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class SecondPatron {
        public static void AddSecondPatron() {
            var ElementalWitchPatronSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("3172b6960c774e19ad029c5e4a96d3e4");
            var HagboundWitchPatronSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0b9af221d99a91842b3a2afbc6a68a1e");
            var WitchPatronSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("381cf4c890815d049a4420c6f31d063f");

            var WitchHexAmelioratingFeature = Resources.GetBlueprint<BlueprintFeature>("3cdd3660fb69f3e4db0160fa97dfa85d");

            var SecondPatronRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>("SecondPatronRequisiteFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("Witch Patron");
                bp.SetDescription("Patron Requisite Feature");
            });
            var SecondPatronFeature = Helpers.CreateBlueprint<BlueprintFeatureSelection>("SecondPatronFeature", bp => {
                bp.SetName("Second Patron");
                bp.SetDescription("You've attracted the favor of a second patron.\n" +
                    "You select a second patron, gaining all its benifits.");
                bp.m_Icon = WitchHexAmelioratingFeature.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Mode = SelectionMode.OnlyNew;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddFeatures(WitchPatronSelection.m_AllFeatures);
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisiteFeature(WitchPatronSelection, GroupType.Any);
                bp.AddPrerequisiteFeature(ElementalWitchPatronSelection, GroupType.Any);
                bp.AddPrerequisiteFeature(HagboundWitchPatronSelection, GroupType.Any);
                bp.AddPrerequisiteFeature(SecondPatronRequisiteFeature, GroupType.Any);
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("SecondPatron")) { return; }
            FeatTools.AddAsMythicAbility(SecondPatronFeature);
            WitchPatronSelection.m_AllFeatures
                .Select(feature => feature.Get())
                .OfType<BlueprintProgression>()
                .ForEach(patron => {
                    patron.GiveFeaturesForPreviousLevels = true;
                    patron.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { SecondPatronRequisiteFeature.ToReference<BlueprintUnitFactReference>() };
                    });
                    Main.LogPatch("Patched", patron);
                });
        }
    }
}
