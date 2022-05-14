using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;


namespace TabletopTweaks.Base.NewContent.Feats {
    internal class ExpandedSpellKenning {
        public static void AddExpandedSpellKenning() {
            var SkaldSpellKenning = BlueprintTools.GetBlueprint<BlueprintFeature>("d385b8c302e720c43aa17b8170bc6ae2");
            var ExpandedSpellKenning = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ExpandedSpellKenning", bp => {
                bp.SetName(TTTContext, "Expanded Spell Kenning");
                bp.SetDescription(TTTContext, "You are learned in a broader range of spell traditions than most.\n" +
                    "When you use your spell kenning class feature, you can select a spell from either the druid or the witch spell list.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddPrerequisiteFeature(SkaldSpellKenning);
                bp.AddFeatures(
                    CreateExpandedSpellKenning(SpellTools.SpellList.DruidSpellList, "Druid", bp),
                    CreateExpandedSpellKenning(SpellTools.SpellList.WitchSpellList, "Witch", bp)
                );
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("ExpandedSpellKenning")) { return; }
            FeatTools.AddAsFeat(ExpandedSpellKenning);

            BlueprintFeature CreateExpandedSpellKenning(BlueprintSpellList spellList, string className, BlueprintFeatureSelection selection) {
                return Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, $"ExpandedSpellKenning{className}",
                    bp => {
                        bp.SetName(TTTContext, $"Expanded Spell Kenning — {className}");
                        bp.SetDescription(selection.m_Description);
                        bp.IsClassFeature = true;
                        bp.Groups = new FeatureGroup[] { FeatureGroup.ArcaneTricksterSpellbook };
                        bp.HideInUI = true;
                        bp.HideNotAvailibleInUI = true;
                        bp.AddComponent<AddSpellKenningSpellList>(c => {
                            c.m_SpellLists = new BlueprintSpellListReference[] { spellList.ToReference<BlueprintSpellListReference>() };
                        });
                    });
            }
        }
    }
}
