using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class CelestialServant {
        public static void AddCelestialServant() {
            var TemplateCelestial = Resources.GetModBlueprint<BlueprintFeature>("TemplateCelestial");
            var AasimarRace = Resources.GetBlueprint<BlueprintRace>("b7f02ba92b363064fb873963bec275ee");

            var CelestialServant = Helpers.CreateBlueprint<BlueprintFeature>("CelestialServant", bp => {
                bp.SetName("Celestial Servant");
                bp.SetDescription("Your animal companion, familiar, or mount gains the celestial template and becomes a magical beast, " +
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
            if (ModSettings.AddedContent.Feats.IsDisabled("CelestialServant")) { return; }
            FeatTools.AddAsFeat(CelestialServant);
        }
    }
}
