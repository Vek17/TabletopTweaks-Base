using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class SplitHex {
        public static void AddSplitHex() {
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var ShamanHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4223fe18c75d4d14787af196a04e14e7");

            var WitchMajorHex = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8ac781b33e380c84aa578f1b006dd6c5");
            var WitchGrandHex = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d24c2467804ce0e4497d9978bafec1f9");

            var Icon_SplitHex = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_SplitHex.png");
            var Icon_SplitHexMajor = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_SplitHexMajor.png");

            var BaseDescription = Helpers.CreateString(TTTContext, "SplitHex.description",
                "You can split the effect of one of your targeted hexes, affecting another creature you can see.\n" +
                "When you use one of your hexes (not a major hex or a grand hex) that targets a single creature, you can cast the hex again as a free action.");
            var AutomaticDescription = Helpers.CreateString(TTTContext, "SplitHexAutomatic.description",
                "You can split the effect of one of your targeted hexes, affecting another creature you can see.\n" +
                "When you use one of your hexes (not a major hex or a grand hex) that targets a single creature it also affects the closest other valid target within 30 feet.");
            var MajorBaseDescription = Helpers.CreateString(TTTContext, "SplitHexMajor.description",
                "You can split the effect of one of your targeted hexes, affecting another creature you can see.\n" +
                "When you use one of your major hexes (not a grand hex) that targets a single creature, you can cast the hex again as a free action.");
            var MajorAutomaticDescription = Helpers.CreateString(TTTContext, "SplitHexMajorAutomatic.description",
                "You can split the effect of one of your targeted major hexes, affecting another creature you can see.\n" +
                "When you use one of your hexes (not a grand hex) that targets a single creature it also affects the closest other valid target within 30 feet.");
            var SplitHexMajor = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SplitHexMajor", bp => {
                bp.SetName(TTTContext, "Split Major Hex");
                bp.m_Icon = Icon_SplitHexMajor;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                if (TTTContext.AddedContent.Feats.IsEnabled("SplitHexAutomatic")) {
                    bp.SetDescription(MajorAutomaticDescription);
                } else {
                    bp.SetDescription(MajorBaseDescription);
                }
            });
            var SplitHex = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SplitHex", bp => {
                bp.SetName(TTTContext, "Split Hex");
                bp.m_Icon = Icon_SplitHex;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                if (TTTContext.AddedContent.Feats.IsEnabled("SplitHexAutomatic")) {
                    bp.SetDescription(AutomaticDescription);
                    bp.AddComponent<SplitHexComponentAutomatic>(c => {
                        c.m_MajorHex = WitchMajorHex;
                        c.m_GrandHex = WitchGrandHex;
                        c.m_SplitMajorHex = SplitHexMajor.ToReference<BlueprintFeatureReference>();
                    });
                } else {
                    bp.SetDescription(BaseDescription);
                    bp.AddComponent<SplitHexComponent>(c => {
                        c.m_MajorHex = WitchMajorHex;
                        c.m_GrandHex = WitchGrandHex;
                        c.m_SplitMajorHex = SplitHexMajor.ToReference<BlueprintFeatureReference>();
                    });
                }
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = ClassTools.ClassReferences.WitchClass;
                    c.Level = 10;
                });
            });
            SplitHexMajor.TemporaryContext(bp => {
                bp.AddPrerequisiteFeature(SplitHex);
                bp.AddPrerequisite<PrerequisiteCasterLevel>(c => {
                    c.RequiredCasterLevel = 18;
                });
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("SplitHex")) { return; }
            FeatTools.AddAsFeat(SplitHex);
            FeatTools.AddAsFeat(SplitHexMajor);
            UpdateAbilityRestrictions(WitchHexSelection);
            UpdateAbilityRestrictions(ShamanHexSelection);

            void UpdateAbilityRestrictions(BlueprintFeatureSelection selection) {
                selection.AllFeatures
                    .SelectMany(f => f.GetComponents<AddFacts>())
                    .OfType<AddFacts>()
                    .SelectMany(c => c.Facts)
                    .OfType<BlueprintAbility>()
                    .ForEach(bp => {
                        bp.AbilityAndVariants().ForEach(ability => {
                            if (!ability.GetComponent<AbilityTargetNoSplitHexRepeat>()) {
                                ability.AddComponent<AbilityTargetNoSplitHexRepeat>();
                            }
                        });
                    });
            }
        }
    }
}
