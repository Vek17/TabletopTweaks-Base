using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class SplitHex {
        public static void AddSplitHex() {
            var TemplateCelestial = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "TemplateCelestial");
            var AasimarRace = BlueprintTools.GetBlueprint<BlueprintRace>("b7f02ba92b363064fb873963bec275ee");
            var BaseDescription = Helpers.CreateString(TTTContext, "SplitHex.description", 
                "You can split the effect of one of your targeted hexes, affecting another creature you can see.\n" +
                "When you use one of your hexes that targets a single creature, you can cast the hex again as a free action.");
            var AutomaticDescription = Helpers.CreateString(TTTContext, "SplitHexAutomatic.description",
                "You can split the effect of one of your targeted hexes, affecting another creature you can see.\n" +
                "When you use one of your hexes that targets a single creature it also affects the closest other valid target within 30 feet.");
            var SplitHex = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SplitHex", bp => {
                bp.SetName(TTTContext, "Split Hex");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                if (TTTContext.AddedContent.Feats.IsEnabled("SplitHexAutomatic")) {
                    bp.SetDescription(AutomaticDescription);
                    bp.AddComponent<SplitHexComponentAutomatic>();
                } else {
                    bp.SetDescription(BaseDescription);
                    bp.AddComponent<SplitHexComponent>();
                }
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = ClassTools.ClassReferences.WitchClass;
                    c.Level = 10;
                });
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("SplitHex")) { return; }
            FeatTools.AddAsFeat(SplitHex);
        }
    }
}
