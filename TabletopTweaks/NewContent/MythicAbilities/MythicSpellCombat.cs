using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class MythicSpellCombat {
        public static void AddMythicSpellCombat() {
            var MythicAbilitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var SpellCombatFeature = Resources.GetBlueprint<BlueprintFeature>("2464ba53317c7fc4d88f383fac2b45f9");

            var MythicSpellCombat = Helpers.CreateBlueprint<BlueprintFeature>("MythicSpellCombat", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = SpellCombatFeature.Icon;
                bp.SetName("Mythic Spell Combat");
                bp.SetDescription("The magus can use his spell combat and spellstrike abilities while casting or using spells from a mythic spellbook.");
                bp.AddComponent<BroadStudyMythicComponent>();
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = SpellTools.SpellCastingClasses.MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 1;
                });
                bp.AddPrerequisite<PrerequisiteMythicSpellbook>();
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("MythicSpellCombat")) { return; }
            FeatTools.AddAsMythicAbility(MythicSpellCombat);
        }
    }
}
