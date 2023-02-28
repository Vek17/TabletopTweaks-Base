using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    static class NatureFang {
        private static readonly BlueprintFeature NatureSense = BlueprintTools.GetBlueprint<BlueprintFeature>("3a859e435fdd6d343b80d4970a7664c1");
        private static readonly BlueprintFeature WildShapeExtraUse = BlueprintTools.GetBlueprint<BlueprintFeature>("f78260b9a089ccc44b55f0fed08b1752");
        private static readonly BlueprintFeature WildShapeWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
        private static readonly BlueprintFeature WildShapeLeopardFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c4d651bc0d4eabd41b08ee81bfe701d8");
        private static readonly BlueprintFeature WildShapeElementalSmallFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("bddd46a6f6a3e6e4b99008dcf5271c3b");
        private static readonly BlueprintFeature WildShapeBearFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1368c7ce69702444893af5ffd3226e19");
        private static readonly BlueprintFeature WildShapeElementalMediumFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e4b88e2a044c67469c038ac2f09d061");
        private static readonly BlueprintFeature WildShapeSmilodonFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("253c0c0d00e50a24797445f20af52dc8");
        private static readonly BlueprintFeature WildShapeElementalLargeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e66154511a6f9fc49a9de644bd8922db");
        private static readonly BlueprintFeature WildShapeShamblingMoundFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0f31b23c2ab39354bbde4e33e8151495");
        private static readonly BlueprintFeature WildShapeElementalHugeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("fe58dd496a36e274b86958f4677071b2");

        private static readonly BlueprintFeature ResistNaturesLure = BlueprintTools.GetBlueprint<BlueprintFeature>("ad6a5b0e1a65c3540986cf9a7b006388");
        private static readonly BlueprintFeature VenomImmunity = BlueprintTools.GetBlueprint<BlueprintFeature>("5078622eb5cecaf4683fa16a9b948c2c");

        private static readonly BlueprintFeature SneakAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");

        private static readonly BlueprintFeature SlayerTalent2 = BlueprintTools.GetBlueprint<BlueprintFeature>("04430ad24988baa4daa0bcd4f1c7d118");
        private static readonly BlueprintFeature SlayerTalent6 = BlueprintTools.GetBlueprint<BlueprintFeature>("43d1b15873e926848be2abf0ea3ad9a8");
        private static readonly BlueprintFeature SlayerTalent10 = BlueprintTools.GetBlueprint<BlueprintFeature>("913b9cf25c9536949b43a2651b7ffb66");
        private static readonly BlueprintFeature SlayerAdvancedTalents = BlueprintTools.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");

        private static readonly BlueprintFeature SlayerStudyTargetFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("09bdd9445ac38044389476689ae8d5a1");
        private static readonly BlueprintFeature SlayerSwiftStudyTargetFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("40d4f55a5ac0e4f469d67d36c0dfc40b");

        private static readonly BlueprintFeature UncannyDodgeTalent = BlueprintTools.GetBlueprint<BlueprintFeature>("ca5274d057152fa45b7527cad0927840");
        private static readonly BlueprintFeature ImprovedUncannyDodgeTalent = BlueprintTools.GetBlueprint<BlueprintFeature>("e821c61b2711cea4cb993725b910e7e8");

        private static readonly BlueprintBuff SlayerStudyTargetBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");

        private static readonly BlueprintCharacterClass DruidClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("610d836f3a3a9ed42a4349b62f002e96");

        public static void AddNatureFang() {

            var NatureFangStudiedTargetFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "NatureFangStudiedTargetFeature", bp => {
                bp.SetName(TTTContext, "Studied Target");
                bp.SetDescription(TTTContext, "At 1st level, a nature fang gains the slayer’s studied target class feature." +
                    " At 5th level and every 5 levels thereafter, the nature fang’s bonus against her studied target increases by 1.");
                bp.m_Icon = SlayerStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { SlayerStudyTargetFeature.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var NatureFangStudiedTargetSwiftFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "NatureFangStudiedTargetSwiftFeature", bp => {
                bp.SetName(TTTContext, "Studied Target");
                bp.SetDescription(TTTContext, "At 7th level, a character can study an opponent as a move or swift action.");
                bp.m_Icon = SlayerSwiftStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { SlayerSwiftStudyTargetFeature.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var NatureFangAdvancedSlayerTalent = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "NatureFangAdvancedSlayerTalent", bp => {
                bp.SetName(TTTContext, "Advanced Talent");
                bp.SetDescription(TTTContext, "Starting at 12th level, a Nature Fang can select an advanced slayer talent in place of a slayer talent.");
                bp.m_Icon = SlayerAdvancedTalents.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { SlayerAdvancedTalents.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var NatureFangArcehtype = Helpers.CreateBlueprint<BlueprintArchetype>(TTTContext, "NatureFangArcehtype", bp => {
                bp.SetName(TTTContext, "Nature Fang");
                bp.SetDescription(TTTContext, "A nature fang is a druid who stalks and slays those who despoil nature, kill scarce animals, or introduce diseases to unprotected habitats." +
                    " She gives up a close empathic connection with the natural world to become its deadly champion and avenger.");
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, NatureSense),
                    Helpers.CreateLevelEntry(4, WildShapeWolfFeature, ResistNaturesLure),
                    Helpers.CreateLevelEntry(6, WildShapeLeopardFeature, WildShapeExtraUse, WildShapeElementalSmallFeature),
                    Helpers.CreateLevelEntry(8, WildShapeBearFeature, WildShapeExtraUse, WildShapeElementalMediumFeature),
                    Helpers.CreateLevelEntry(9, VenomImmunity),
                    Helpers.CreateLevelEntry(10, WildShapeSmilodonFeature, WildShapeExtraUse, WildShapeElementalLargeFeature, WildShapeShamblingMoundFeature),
                    Helpers.CreateLevelEntry(12, WildShapeElementalHugeFeature, WildShapeExtraUse),
                    Helpers.CreateLevelEntry(14, WildShapeExtraUse),
                    Helpers.CreateLevelEntry(16, WildShapeExtraUse),
                    Helpers.CreateLevelEntry(18, WildShapeExtraUse)
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, NatureFangStudiedTargetFeature),
                    Helpers.CreateLevelEntry(4, SlayerTalent2, SneakAttack),
                    Helpers.CreateLevelEntry(6, SlayerTalent6),
                    Helpers.CreateLevelEntry(7, NatureFangStudiedTargetSwiftFeature),
                    Helpers.CreateLevelEntry(8, SlayerTalent6),
                    Helpers.CreateLevelEntry(10, SlayerTalent10),
                    Helpers.CreateLevelEntry(12, NatureFangAdvancedSlayerTalent, SlayerTalent10),
                    Helpers.CreateLevelEntry(14, SlayerTalent10),
                    Helpers.CreateLevelEntry(16, SlayerTalent10),
                    Helpers.CreateLevelEntry(18, SlayerTalent10),
                    Helpers.CreateLevelEntry(20, SlayerTalent10)
                };
            });

            var studyTargetRankConfig = SlayerStudyTargetBuff.GetComponent<ContextRankConfig>();
            studyTargetRankConfig.m_Class = studyTargetRankConfig.m_Class.AppendToArray(DruidClass.ToReference<BlueprintCharacterClassReference>());
            studyTargetRankConfig.m_AdditionalArchetypes = studyTargetRankConfig.m_AdditionalArchetypes.AppendToArray(NatureFangArcehtype.ToReference<BlueprintArchetypeReference>());

            UncannyDodgeTalent.AddPrerequisite<PrerequisiteArchetypeLevel>(p => {
                p.m_CharacterClass = DruidClass.ToReference<BlueprintCharacterClassReference>();
                p.m_Archetype = NatureFangArcehtype.ToReference<BlueprintArchetypeReference>();
                p.Level = 4;
                p.Group = Prerequisite.GroupType.Any;
            });
            ImprovedUncannyDodgeTalent.AddPrerequisite<PrerequisiteArchetypeLevel>(p => {
                p.m_CharacterClass = DruidClass.ToReference<BlueprintCharacterClassReference>();
                p.m_Archetype = NatureFangArcehtype.ToReference<BlueprintArchetypeReference>();
                p.Level = 12;
                p.Group = Prerequisite.GroupType.Any;
            });

            if (TTTContext.AddedContent.Archetypes.IsDisabled("NatureFang")) { return; }
            DruidClass.m_Archetypes = DruidClass.m_Archetypes.AppendToArray(NatureFangArcehtype.ToReference<BlueprintArchetypeReference>());

            DruidClass.Progression.UIGroups = DruidClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(
                    SlayerTalent2,
                    SlayerTalent6,
                    SlayerTalent10
                ),
                Helpers.CreateUIGroup(
                    NatureFangStudiedTargetFeature,
                    NatureFangStudiedTargetSwiftFeature
                )
            );
            TTTContext.Logger.LogPatch("Added", NatureFangArcehtype);
        }
    }
}
