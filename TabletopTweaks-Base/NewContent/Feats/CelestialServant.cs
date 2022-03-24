using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class CelestialServant {
        public static void AddCelestialServant() {
            var TemplateCelestial = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "TemplateCelestial");
            var AasimarRace = BlueprintTools.GetBlueprint<BlueprintRace>("b7f02ba92b363064fb873963bec275ee");

            var CelestialServant = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "CelestialServant", bp => {
                bp.SetName(TTTContext, "Celestial Servant");
                bp.SetDescription(TTTContext, "Your animal companion, familiar, or mount gains the celestial template and becomes a magical beast, " +
                    "though you may still treat it as an animal when using Handle Animal, wild empathy, or any other spells or class abilities " +
                    "that specifically affect animals.\n" +
                    "Creature gains spell resistance equal to its level + 5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to cold, acid, and electricity.\n" +
                    "5 — 10 HD: resistance 10 to cold, acid, and electricity, DR 5/evil\n" +
                    "11+ HD: resistance 15 to cold, acid, and electricity, DR 10/evil\n" +
                    "Smite Evil (Su): Once per day, the celestial creature may smite a evil-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is evil, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateCelestial.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisiteFeature(AasimarRace);
                bp.AddPrerequisite<PrerequisitePet>();
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("CelestialServant")) { return; }
            FeatTools.AddAsFeat(CelestialServant);
        }
    }
}
