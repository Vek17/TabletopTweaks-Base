using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class AccursedHex {
        public static void AddAccursedHex() {
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var ShamanHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4223fe18c75d4d14787af196a04e14e7");
            var HexcrafterMagusHexMagusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");

            var Icon_AccursedHex = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_AccursedHex.png");

            var AccursedHexBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "AccursedHexBuff", bp => {
                bp.SetName(TTTContext, "Accursed Hex");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AccursedHexBuffComponent>();
            });
            var AccursedHexMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AccursedHexMythicFeature", bp => {
                bp.SetName(TTTContext, "Accursed Hex (Mythic)");
                bp.SetDescription(TTTContext, "When you use Accursed Hex to target a creature with one of your hexes a second time, " +
                    "that creature must roll its saving throw twice and take the lower result.");
                bp.m_Icon = Icon_AccursedHex;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
            });
            var AccursedHexFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AccursedHexFeature", bp => {
                bp.SetName(TTTContext, "Accursed Hex");
                bp.SetDescription(TTTContext, "When you target a creature with a hex that cannot target the same creature more than once per day, " +
                    "and that creature succeeds at its saving throw against the hex’s effect, " +
                    "you can target the creature with the same hex a second time before the end of your next turn. " +
                    "If the second attempt fails, you can make no further attempts to target that creature with the same hex for 1 day. ");
                bp.m_Icon = Icon_AccursedHex;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<AccursedHexComponent>(c => {
                    c.m_MythicFeature = AccursedHexMythicFeature.ToReference<BlueprintFeatureReference>();
                    c.m_AccursedBuff = AccursedHexBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddPrerequisiteFeaturesFromList(1, 
                    WitchHexSelection,
                    ShamanHexSelection,
                    HexcrafterMagusHexMagusSelection,
                    SylvanTricksterTalentSelection
                );
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
            });
            AccursedHexMythicFeature.AddPrerequisiteFeature(AccursedHexFeature);

            if (TTTContext.AddedContent.Feats.IsDisabled("AccursedHex")) { return; }
            FeatTools.AddAsFeat(AccursedHexFeature);
            UpdateAbilityRestrictions(WitchHexSelection);
            UpdateAbilityRestrictions(ShamanHexSelection);

            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicAccursedHex")) { return; }
            FeatTools.AddAsMythicFeat(AccursedHexMythicFeature);

            void UpdateAbilityRestrictions(BlueprintFeatureSelection selection) {
                selection.AllFeatures
                    .SelectMany(f => f.GetComponents<AddFacts>())
                    .OfType<AddFacts>()
                    .SelectMany(c => c.Facts)
                    .OfType<BlueprintAbility>()
                    .ForEach(bp => {
                        bp.GetComponents<AbilityTargetHasFact>()
                            .Where(c => c.Inverted)
                            .ForEach(c => {
                                var checkedFacts = c.m_CheckedFacts;
                                bp.RemoveComponent(c);
                                bp.AddComponent<AbilityTargetHasNoFactUnlessAccursedHex>(n => {
                                    n.m_CheckedFacts = checkedFacts;
                                });
                            });
                    });
            }
        }
    }
}
