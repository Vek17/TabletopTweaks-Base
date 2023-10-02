using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Monk {
        public static BlueprintFeatureSelection MonkAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var KiPerfectSelfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("3854f693180168a4980646aee9494c72");
            var SoheiArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("fad7c56737ed12e42aacc330acc86428");
            var ZenArcherArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("2b1a58a7917084f49b097e86271df21c");
            var MonkFlurryOfBlowstUnlock = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd99770e6bd240a4aab70f7af103e56a");
            var SoheiFlurryOfBlowsUnlock = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("cd4381b73b6709146bbcc0a528a6f471");
            var ZenArcherFlurryOfBlowsUnlock = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("3e470edc8a733b641bcbbbb5b9527ff6");
            var QuarterstaffMasterFlurryUnlock = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("44b0f313ec56481eb447019fbe714330");
            var Longbow = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("7a1211c05ec2c46428f41e3c0db9423f");
            var CompositeLongbow = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("1ac79088a7e5dde46966636a3ac71c35");
            var Shortbow = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("99ce02fb54639b5439d07c99c55b8542");
            var CompositeShortbow = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("011f6f86a0b16df4bbf7f40878c3e80b");
            var Quarterstaff = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("629736dabac7f9f4a819dc854eaed2d6");

            var OldMasterFeatureFlurryFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterFeatureFlurryFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.AddComponent<BuffExtraAttack>(c => {
                    c.Number = 1;
                });
            });
            var OldMasterFeatureACFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterFeatureACFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = ModifierDescriptor.Dodge;
                    c.Value = 2;
                });
            });
            var OldMasterUnlockMonkFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterUnlockMonkFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.HideInUI = true;
                bp.IsClassFeature = true;
                bp.AddComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>(c => {
                    c.m_NewFact = OldMasterFeatureFlurryFeature.ToReference<BlueprintUnitFactReference>();
                    c.m_BowWeaponTypes = new BlueprintWeaponTypeReference[0];
                });
            });
            var OldMasterUnlockZenArcherFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterUnlockZenArcherFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.HideInUI = true;
                bp.IsClassFeature = true;
                bp.AddComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>(c => {
                    c.m_NewFact = OldMasterFeatureFlurryFeature.ToReference<BlueprintUnitFactReference>();
                    c.m_BowWeaponTypes = new BlueprintWeaponTypeReference[] {
                        Longbow,
                        CompositeLongbow,
                        Shortbow,
                        CompositeShortbow
                    };
                    c.IsZenArcher = true;
                });
            });
            var OldMasterUnlockSoheiFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterUnlockSoheiFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.HideInUI = true;
                bp.IsClassFeature = true;
                bp.AddComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>(c => {
                    c.m_NewFact = OldMasterFeatureFlurryFeature.ToReference<BlueprintUnitFactReference>();
                    c.m_BowWeaponTypes = new BlueprintWeaponTypeReference[0];
                    c.IsSohei = true;
                });
            });
            var OldMasterUnlockQuarterstaffFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterUnlockQuarterstaffFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.HideInUI = true;
                bp.IsClassFeature = true;
                bp.AddComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>(c => {
                    c.m_NewFact = OldMasterFeatureFlurryFeature.ToReference<BlueprintUnitFactReference>();
                    c.m_BowWeaponTypes = new BlueprintWeaponTypeReference[] {
                        Quarterstaff
                    };
                    c.IsZenArcher = true;
                });
            });
            var OldMasterUnlockBaseFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldMasterUnlockBaseFeature", bp => {
                bp.SetName(TTTContext, "Old Master");
                bp.SetDescription(TTTContext, "At 20th level, the monk has reached the highest levels of his martial arts school.\n" +
                    "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.");
                bp.IsClassFeature = true;
                bp.AddComponent<MonkNoArmorFeatureUnlock>(c => {
                    c.m_NewFact = OldMasterFeatureACFeature.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = MonkFlurryOfBlowstUnlock;
                    c.m_Feature = OldMasterUnlockMonkFeature.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = ZenArcherFlurryOfBlowsUnlock;
                    c.m_Feature = OldMasterUnlockZenArcherFeature.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = SoheiFlurryOfBlowsUnlock;
                    c.m_Feature = OldMasterUnlockSoheiFeature.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = QuarterstaffMasterFlurryUnlock;
                    c.m_Feature = OldMasterUnlockQuarterstaffFeature.ToReference<BlueprintUnitFactReference>();
                });
            });
            MonkAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "MonkAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = KiPerfectSelfFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    KiPerfectSelfFeature,
                    OldMasterUnlockBaseFeature,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature,
                    Generic.OldDogNewTricksProgression
                );
            });
        }
    }
}
