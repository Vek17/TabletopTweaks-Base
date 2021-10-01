using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class MythicSpellCombat {
        public static void AddMythicSpellCombat() {
            var TricksterWizardSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("bbe483b903854104a11606412803f214");
            var SpellCombatFeature = Resources.GetBlueprint<BlueprintFeature>("2464ba53317c7fc4d88f383fac2b45f9");

            var MythicSpellCombat = Helpers.CreateBlueprint<BlueprintFeature>("MythicSpellCombat", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = SpellCombatFeature.Icon;
                bp.SetName("Mythic Spell Combat");
                bp.SetDescription("The magus can use his spell combat and spellstrike abilities while casting or " +
                    "using spells from a spellbook granted by a mythic class.");
                bp.AddComponent<BroadStudyMythicComponent>(c => {
                    c.Spellbooks = new BlueprintSpellbookReference[] { TricksterWizardSpellbook.ToReference<BlueprintSpellbookReference>() };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = SpellTools.SpellCastingClasses.MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 6;
                });
                bp.AddPrerequisite<PrerequisiteMythicSpellbook>();
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("MythicSpellCombat")) { return; }
            FeatTools.AddAsMythicAbility(MythicSpellCombat);
        }
    }
}
