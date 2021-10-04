using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    static class NatureFang {
        private static readonly BlueprintFeature NatureSense = Resources.GetBlueprint<BlueprintFeature>("3a859e435fdd6d343b80d4970a7664c1");
        private static readonly BlueprintFeature WildShapeExtraUse = Resources.GetBlueprint<BlueprintFeature>("f78260b9a089ccc44b55f0fed08b1752");
        private static readonly BlueprintFeature WildShapeWolfFeature = Resources.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
        private static readonly BlueprintFeature WildShapeLeopardFeature = Resources.GetBlueprint<BlueprintFeature>("c4d651bc0d4eabd41b08ee81bfe701d8");
        private static readonly BlueprintFeature WildShapeElementalSmallFeature = Resources.GetBlueprint<BlueprintFeature>("bddd46a6f6a3e6e4b99008dcf5271c3b");
        private static readonly BlueprintFeature WildShapeBearFeature = Resources.GetBlueprint<BlueprintFeature>("1368c7ce69702444893af5ffd3226e19");
        private static readonly BlueprintFeature WildShapeElementalMediumFeature = Resources.GetBlueprint<BlueprintFeature>("6e4b88e2a044c67469c038ac2f09d061");
        private static readonly BlueprintFeature WildShapeSmilodonFeature = Resources.GetBlueprint<BlueprintFeature>("253c0c0d00e50a24797445f20af52dc8");
        private static readonly BlueprintFeature WildShapeElementalLargeFeature = Resources.GetBlueprint<BlueprintFeature>("e66154511a6f9fc49a9de644bd8922db");
        private static readonly BlueprintFeature WildShapeShamblingMoundFeature = Resources.GetBlueprint<BlueprintFeature>("0f31b23c2ab39354bbde4e33e8151495");
        private static readonly BlueprintFeature WildShapeElementalHugeFeature = Resources.GetBlueprint<BlueprintFeature>("fe58dd496a36e274b86958f4677071b2");

        private static readonly BlueprintFeature ResistNaturesLure = Resources.GetBlueprint<BlueprintFeature>("ad6a5b0e1a65c3540986cf9a7b006388");
        private static readonly BlueprintFeature VenomImmunity = Resources.GetBlueprint<BlueprintFeature>("5078622eb5cecaf4683fa16a9b948c2c");

        
        //private static readonly BlueprintFeature StudyTarget = Resources.GetBlueprint<BlueprintFeature>("09bdd9445ac38044389476689ae8d5a1");
        private static readonly BlueprintFeature SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");

        private static readonly BlueprintFeature SlayerTalent2 = Resources.GetBlueprint<BlueprintFeature>("04430ad24988baa4daa0bcd4f1c7d118");
        private static readonly BlueprintFeature SlayerTalent6 = Resources.GetBlueprint<BlueprintFeature>("43d1b15873e926848be2abf0ea3ad9a8");
        private static readonly BlueprintFeature SlayerTalent10 = Resources.GetBlueprint<BlueprintFeature>("913b9cf25c9536949b43a2651b7ffb66");
        private static readonly BlueprintFeature SlayerAdvancedTalents = Resources.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");

        private static readonly BlueprintFeature SanctifiedSlayerStudiedTargetFeature = Resources.GetBlueprint<BlueprintFeature>("6d9d3fbc30564e64d966dba27cd6357a");
        private static readonly BlueprintFeature SlayerSwiftStudyTarget = Resources.GetBlueprint<BlueprintFeature>("40d4f55a5ac0e4f469d67d36c0dfc40b");

        private static readonly BlueprintBuff SlayerStudyTargetBuff = Resources.GetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");


        private static readonly BlueprintCharacterClass DruidClass = Resources.GetBlueprint<BlueprintCharacterClass>("610d836f3a3a9ed42a4349b62f002e96");

        private static LevelEntry MakeLevelEntry(int level, params BlueprintFeature[] features) {
            var entry = new LevelEntry() {
                Level = level,
                m_Features = new List<BlueprintFeatureBaseReference>(features.Length)
            };
            for (int i = 0; i < features.Length; i++) {
                entry.m_Features.Add(features[i].ToReference<BlueprintFeatureBaseReference>());
            }
            return entry;
        }

        private static readonly List<LevelEntry> FeaturesToRemove = new List<LevelEntry> {
            MakeLevelEntry(1, NatureSense),
            MakeLevelEntry(4, WildShapeWolfFeature, ResistNaturesLure),
            MakeLevelEntry(6, WildShapeLeopardFeature, WildShapeExtraUse, WildShapeElementalSmallFeature),
            MakeLevelEntry(8, WildShapeBearFeature, WildShapeExtraUse, WildShapeElementalMediumFeature),
            MakeLevelEntry(9, VenomImmunity),
            MakeLevelEntry(10, WildShapeSmilodonFeature, WildShapeExtraUse, WildShapeElementalLargeFeature, WildShapeShamblingMoundFeature),
            MakeLevelEntry(12, WildShapeElementalHugeFeature, WildShapeExtraUse),
            MakeLevelEntry(14, WildShapeExtraUse),
            MakeLevelEntry(16, WildShapeExtraUse),
            MakeLevelEntry(18, WildShapeExtraUse),
        };
        
        private static readonly List<LevelEntry> FeaturesToAdd = new List<LevelEntry> {
            MakeLevelEntry(4, SlayerTalent2, SneakAttack),
            MakeLevelEntry(6, SlayerTalent6),
            MakeLevelEntry(8, SlayerTalent6),
            MakeLevelEntry(10, SlayerTalent10),
            MakeLevelEntry(12, SlayerAdvancedTalents, SlayerTalent10),
            MakeLevelEntry(14, SlayerTalent10),
            MakeLevelEntry(16, SlayerTalent10),
            MakeLevelEntry(18, SlayerTalent10),
            MakeLevelEntry(20, SlayerTalent10),
        };

        public static void AddNatureFang() {
            var studiedTargetFeature = Helpers.CreateCopy(SanctifiedSlayerStudiedTargetFeature, bp => {
                bp.name = "NatureFangStudiedTargetFeature.Name";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_Description = Helpers.CreateString("NatureFangStudiedTargetFeature.Description", "At 1st level, a nature fang gains the slayer’s studied target class feature." +
                    " At 5th level and every 5 levels thereafter, the nature fang’s bonus against her studied target increases by 1." +
                    " Unlike a slayer, a nature fang does not gain the ability to maintain more than one studied target at the same time.");
            });
            Resources.AddBlueprint(studiedTargetFeature);

            var swiftStudiedTargetFeature = Helpers.CreateCopy(SanctifiedSlayerStudiedTargetFeature, bp => {
                bp.name = "NatureFangStudiedTargetSwiftFeature.Name";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_Description = Helpers.CreateString("NatureFangStudiedTargetSwiftFeature.Description", "At 7th level, a character can study an opponent as a move or {g|Encyclopedia:Swift_Action}swift action{/g}.");
                bp.GetComponent<AddFacts>().m_Facts[0] = SlayerSwiftStudyTarget.ToReference<BlueprintUnitFactReference>();
            });
            Resources.AddBlueprint(swiftStudiedTargetFeature);


            FeaturesToAdd.Add(MakeLevelEntry(1, studiedTargetFeature));
            FeaturesToAdd.Add(MakeLevelEntry(7, swiftStudiedTargetFeature));

            var archetype = Helpers.CreateBlueprint<BlueprintArchetype>("NatureFangArcehtype", bp => {
                bp.LocalizedName = Helpers.CreateString("NatureFangArchetype.Name", "Nature Fang");
                bp.LocalizedDescription = Helpers.CreateString("NatureFangArchetype.Description", "A nature fang is a druid who stalks and slays those who despoil nature, kill scarce animals, or introduce diseases to unprotected habitats." +
                    " She gives up a close empathic connection with the natural world to become its deadly champion and avenger.");

                bp.RemoveFeatures = FeaturesToRemove.ToArray();
                bp.AddFeatures = FeaturesToAdd.ToArray();
            });

            var studyTargetRankConfig = SlayerStudyTargetBuff.GetComponent<ContextRankConfig>();
            studyTargetRankConfig.m_Class = studyTargetRankConfig.m_Class.AppendToArray(DruidClass.ToReference<BlueprintCharacterClassReference>());

            if (ModSettings.AddedContent.Archetypes.IsDisabled("NatureFang")) { return; }
            DruidClass.m_Archetypes = DruidClass.m_Archetypes.AppendToArray(archetype.ToReference<BlueprintArchetypeReference>());

            DruidClass.Progression.UIGroups = DruidClass.Progression.UIGroups.AppendToArray(
                new UIGroup() {
                    m_Features = new List<BlueprintFeatureBaseReference> {
                        SlayerTalent2.ToReference<BlueprintFeatureBaseReference>(),
                        SlayerTalent6.ToReference<BlueprintFeatureBaseReference>(),
                        SlayerTalent10.ToReference<BlueprintFeatureBaseReference>(),
                    }
                },
                new UIGroup() {
                    m_Features = new List<BlueprintFeatureBaseReference>() {
                        studiedTargetFeature.ToReference<BlueprintFeatureBaseReference>(),
                        swiftStudiedTargetFeature.ToReference<BlueprintFeatureBaseReference>(),
                    }
                }
            );

            Main.LogPatch("Added", archetype);
        }
    }
}
