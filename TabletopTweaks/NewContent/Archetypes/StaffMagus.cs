using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    class StaffMagus {

        private static readonly BlueprintCharacterClass MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
        private static readonly BlueprintFeature SimpleWeaponProficiency = Resources.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd");
        private static readonly BlueprintFeature LightArmorProficiency = Resources.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
        private static readonly BlueprintFeature MagusProficiencies = Resources.GetBlueprint<BlueprintFeature>("8f8c2640ffad89349883fc2e5ff2091e");
        private static readonly BlueprintFeature MagusMediumArmor = Resources.GetBlueprint<BlueprintFeature>("b24897e082896654c8dd64c8fb677363");
        private static readonly BlueprintFeature MagusHeavyArmor = Resources.GetBlueprint<BlueprintFeature>("447ca91389e5c9246acb2c640d63f4da");

        public static void AddStaffMagus() {
            if (ModSettings.AddedContent.Feats.IsDisabled("QuarterstaffMasterFeat")) return;
            if (ModSettings.AddedContent.Archetypes.IsDisabled("StaffMagus")) return;

            var StaffMagusProficiencies = Helpers.CreateBlueprint<BlueprintFeature>("StaffMagusProficiencies", bp => {
                bp.SetName("Staff Magus Proficiencies");
                bp.SetDescription("A staff magus is proficient with simple weapons only. He can cast magus spells while wearing light armor without incurring the normal arcane spell failure chance. Like any other arcane spellcaster, a magus wearing medium armor or heavy armor or using a shield incurs a chance of arcane spell failure if the spell in question has a somatic component. A multiclass magus still incurs the normal arcane spell failure chance for arcane spells received from other classes. This replaces the normal magus weapon and armor proficiency feature.");
                bp.m_Icon = MagusProficiencies.m_Icon;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
                        SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>()
                    };
                });
                bp.AddComponent<ArcaneArmorProficiency>(c => {
                    c.Armor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light };
                });
            });
            var QuarterstaffDefense = Helpers.CreateBlueprint<BlueprintFeature>("QuarterstaffDefense", bp => {
                bp.SetName("Quarterstaff Defense");
                bp.SetDescription("At 7th level, while wielding a quarterstaff, the staff magus gains a shield bonus to his Armor Class equal to the enhancement bonus of the quarterstaff, including any enhancement bonus on that staff from his arcane pool class feature. At 13th level, this bonus increases by +3. This ability replaces the medium armor and heavy armor class abilities.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Icon = MagusMediumArmor.m_Icon;
            });
            var QuarterstaffDefenseImproved = Helpers.CreateBlueprint<BlueprintFeature>("QuarterstaffDefenseImproved", bp => {
                bp.SetName("Quarterstaff Defense (Upgrade)");
                bp.SetDescription(QuarterstaffDefense.Description);
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Icon = MagusHeavyArmor.m_Icon;
            });

            var StaffMagusArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("StaffMagus", bp => {
                bp.SetName("Staff Magus");
                bp.SetDescription("While most magi use a one-handed weapon as their melee implement of choice, one group of magi uses the quarterstaff instead. These lightly armored magi use staves for both defense and inflicting their spells upon enemies. Skilled in manipulating these weapons with one hand or two, they eventually learn how to use arcane staves as well, and are just as formidable in combat as their sword-swinging brethren. ");
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, MagusProficiencies),
                    Helpers.CreateLevelEntry(7, MagusMediumArmor),
                    Helpers.CreateLevelEntry(13, MagusHeavyArmor)
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, StaffMagusProficiencies),
                    Helpers.CreateLevelEntry(7, QuarterstaffDefense),
                    Helpers.CreateLevelEntry(13, QuarterstaffDefenseImproved)
                };
            });
            MagusClass.m_Archetypes = MagusClass.m_Archetypes.AppendToArray(StaffMagusArchetype.ToReference<BlueprintArchetypeReference>());
            Main.LogPatch("Added", StaffMagusArchetype);
        }
    }
}
