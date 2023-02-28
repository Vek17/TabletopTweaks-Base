using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class SecondOrder {
        public static void AddSecondOrder() {
            var CavalierOrderSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("d710e30ea20240247ad87ad86bcd50f2");

            var SecondOrderSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "SecondOrderSelection", bp => {
                bp.SetName(TTTContext, "Second Order");
                bp.SetDescription(TTTContext, "You're devotion to your order has granted you additional powers of another order.\n" +
                    "You select a second order, gaining all its benifits.");
                bp.m_Icon = CavalierOrderSelection.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Mode = SelectionMode.OnlyNew;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddFeatures(CavalierOrderSelection.m_AllFeatures);
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisiteFeature(CavalierOrderSelection);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("SecondOrder")) { return; }
            FeatTools.AddAsMythicAbility(SecondOrderSelection);
            CavalierOrderSelection.m_AllFeatures
                .Select(feature => feature.Get())
                .OfType<BlueprintProgression>()
                .ForEach(order => {
                    order.GiveFeaturesForPreviousLevels = true;
                    TTTContext.Logger.LogPatch("Patched", order);
                });
        }
    }
}
