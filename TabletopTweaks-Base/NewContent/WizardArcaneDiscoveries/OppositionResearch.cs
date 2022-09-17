using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.WizardArcaneDiscoveries {
    static class OppositionResearch {
        public static void AddOppositionResearch() {
            var WizardClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");

            var OppositionSchoolAbjuration = BlueprintTools.GetBlueprint<BlueprintFeature>("7f8c1b838ff2d2e4f971b42ccdfa0bfd");
            var OppositionSchoolConjuration = BlueprintTools.GetBlueprint<BlueprintFeature>("ca4a0d68c0408d74bb83ade784ebeb0d");
            var OppositionSchoolDivination = BlueprintTools.GetBlueprint<BlueprintFeature>("09595544116fe5349953f939aeba7611");
            var OppositionSchoolEnchantment = BlueprintTools.GetBlueprint<BlueprintFeature>("875fff6feb84f5240bf4375cb497e395");
            var OppositionSchoolEvocation = BlueprintTools.GetBlueprint<BlueprintFeature>("c3724cfbe98875f4a9f6d1aabd4011a6");
            var OppositionSchoolIllusion = BlueprintTools.GetBlueprint<BlueprintFeature>("6750ead44c0c034428c6509c68110375");
            var OppositionSchoolNecromancy = BlueprintTools.GetBlueprint<BlueprintFeature>("a9bb3dcb2e8d44a49ac36c393c114bd9");
            var OppositionSchoolTransmutation = BlueprintTools.GetBlueprint<BlueprintFeature>("fc519612a3c604446888bb345bca5234");

            var OppositionResearchSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "OppositionResearchSelection", bp => {
                bp.SetName(TTTContext, "Opposition Research");
                bp.SetDescription(TTTContext, "Select one Wizard opposition school; preparing spells of this school " +
                    "now only requires one spell slot of the appropriate level instead of two.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteNoFeature>(p => {
                    p.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "HarmoniousMageFeature");
                    p.Group = Prerequisite.GroupType.All;
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(p => {
                    p.m_CharacterClass = WizardClass;
                    p.Level = 9;
                    p.Group = Prerequisite.GroupType.All;
                });
                bp.AddFeatures(
                    CreateOppositionResearch(OppositionSchoolAbjuration),
                    CreateOppositionResearch(OppositionSchoolConjuration),
                    CreateOppositionResearch(OppositionSchoolDivination),
                    CreateOppositionResearch(OppositionSchoolEnchantment),
                    CreateOppositionResearch(OppositionSchoolEvocation),
                    CreateOppositionResearch(OppositionSchoolIllusion),
                    CreateOppositionResearch(OppositionSchoolNecromancy),
                    CreateOppositionResearch(OppositionSchoolTransmutation)
                );
            });

            if (TTTContext.AddedContent.WizardArcaneDiscoveries.IsDisabled("OppositionResearch")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(OppositionResearchSelection);
        }

        private static BlueprintFeature CreateOppositionResearch(BlueprintFeature OppositionSchool) {
            var School = OppositionSchool.GetComponent<AddOppositionSchool>().School;
            var OppositionResearch = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, $"OppositionResearch{School}", bp => {
                bp.SetName(TTTContext, $"Opposition Research — {School}");
                bp.SetDescription(TTTContext, $"Preparing spells from the {School} school now only requires one spell slot of the appropriate spell level instead of two.");
                bp.m_Icon = OppositionSchool.Icon;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<OppositionResearchComponent>(c => {
                    c.School = School;
                });
                bp.AddPrerequisiteFeature(OppositionSchool);
            });
            return OppositionResearch;
        }
    }
}
