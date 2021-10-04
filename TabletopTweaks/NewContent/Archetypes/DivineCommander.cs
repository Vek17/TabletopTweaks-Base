using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Collections.Generic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    static class DivineCommander {

        private static readonly BlueprintCharacterClass WarpriestClass = Resources.GetBlueprint<BlueprintCharacterClass>("30b5e47d47a0e37438cc5a80c96cfb99");

        private static readonly BlueprintArchetype WarpriestCultLeaderArchetype = Resources.GetBlueprint<BlueprintArchetype>("cd2d419961bb019438b77d4aecc2fc04");
        private static readonly BlueprintFeature BlessingSelection1 = Resources.GetBlueprint<BlueprintFeature>("6d9dcc2a59210a14891aeedb09d406aa");
        private static readonly BlueprintFeature BlessingSelection2 = Resources.GetBlueprint<BlueprintFeature>("b7ce4a67287cda746a59b31c042305cf");

        private static readonly BlueprintFeature BonusFeatSelection = Resources.GetBlueprint<BlueprintFeature>("303fd456ddb14437946e344bad9a893b");

        private static readonly BlueprintFeature PetTemplateSelection = Resources.GetBlueprint<BlueprintFeature>("b1c0bcb356a496e4487b7dd8e7521043");

        private static readonly BlueprintProgression DruidAnimalCompanionProgression = Resources.GetBlueprint<BlueprintProgression>("3853d5405ebfc0f4a86930bb7082b43b");
        private static readonly BlueprintFeatureSelection DruidAnimalCompanionSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("571f8434d98560c43935e132df65fe76");

        private static readonly BlueprintFeature CavalierTacticianFeature = Resources.GetBlueprint<BlueprintFeature>("404f6e3da48b87f4e9fca21150d47f71");
        private static readonly BlueprintFeature CavalierTacticianFeatureGreater = Resources.GetBlueprint<BlueprintFeature>("c7913a6c300cb994b8e800e1096f9280");
        private static readonly BlueprintAbility CavalierTacticianAbility = Resources.GetBlueprint<BlueprintAbility>("3ff8ef7ba7b5be0429cf32cd4ddf637c");
        private static readonly BlueprintAbility CavalierTacticianAbilitySwift = Resources.GetBlueprint<BlueprintAbility>("78b8d3fd0999f964f82d1c5ec30900e8");
        private static readonly BlueprintFeature CavalierTacticianSupportFeature = Resources.GetBlueprint<BlueprintFeature>("37c496c0c2f04544b83a8d013409fd47");
        private static readonly BlueprintAbilityResource CavalierTacticianResource = Resources.GetBlueprint<BlueprintAbilityResource>("7a5fd780d1a866444ad893382d41ec22");
        private static readonly BlueprintFeature CavalierTacticianFeatSelection = Resources.GetBlueprint<BlueprintFeature>("7bc55b5e381358c45b42153b8b2603a6");

        private static System.Collections.Generic.List<BlueprintFeatureBaseReference> MakeFeatureReferences(params BlueprintFeature [] features) {
            var list = new System.Collections.Generic.List<BlueprintFeatureBaseReference>(features.Length);
            foreach (var feature in features) {
                list.Add(feature.ToReference<BlueprintFeatureBaseReference>());
            }
            return list;
        }

        public static void AddFeatureModifierAtLevel(this List<LevelEntry> entries, int level, params BlueprintFeature[] features) {
            entries.Add(new LevelEntry() {
                Level = level,
                m_Features = MakeFeatureReferences(features)
            });
        }


        public static void AddDivineCommander() {

            var WarpriestAnimalCompanionProgression = Helpers.CreateCopy(DruidAnimalCompanionProgression, acProgressionBp => {
                acProgressionBp.name = "WarpriestAnimalCompanionProgression";
                acProgressionBp.AssetGuid = ModSettings.Blueprints.GetGUID(acProgressionBp.name);
                acProgressionBp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                        new BlueprintProgression.ClassWithLevel() {
                            m_Class = WarpriestClass.ToReference<BlueprintCharacterClassReference>()
                        }
                    };
            });
            Resources.AddBlueprint(WarpriestAnimalCompanionProgression);

            var warpriestAnimalCompanionSelection = Helpers.CreateCopy(DruidAnimalCompanionSelection, acSelectionBp => {
                acSelectionBp.name = "WarpriestAnimalCompanionSelection";
                acSelectionBp.AssetGuid = ModSettings.Blueprints.GetGUID(acSelectionBp.name);
                var progression = acSelectionBp.Components[0] as AddFeatureOnApply;
                progression.m_Feature = WarpriestAnimalCompanionProgression.ToReference<BlueprintFeatureReference>();
            });
            Resources.AddBlueprint(warpriestAnimalCompanionSelection);


            BlueprintFeature tacticianFeatSelection = Helpers.CreateCopy(CavalierTacticianFeatSelection, resource => {
                resource.name = "WarpriestDivineCommanderTacticianFeatSelection";
                resource.AssetGuid = ModSettings.Blueprints.GetGUID(resource.name);

            });
            Resources.AddBlueprint(tacticianFeatSelection);

            BlueprintAbilityResource tacticianAbilityResource = Helpers.CreateCopy(CavalierTacticianResource, resource => {
                resource.name = "WarpriestDivineCommanderTacticianResource";
                resource.AssetGuid = ModSettings.Blueprints.GetGUID(resource.name);

                resource.m_MaxAmount.IncreasedByLevelStartPlusDivStep = true;
                resource.m_MaxAmount.StartingLevel = 9;
                resource.m_MaxAmount.LevelStep = 6;
            });
            Resources.AddBlueprint(tacticianAbilityResource);

            Dictionary<System.Guid, BlueprintBuff> buffMapper = new Dictionary<System.Guid, BlueprintBuff>();

            var warpriestTacticianSupportFeature = Helpers.CreateCopy(CavalierTacticianSupportFeature, bp => {
                bp.name = "DivineCommanderTacticianSupportFeature";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);

                foreach (var component in bp.Components) {
                    var addFeature = component as AddFeatureIfHasFact;
                    var buff = Helpers.CreateCopy(Resources.GetBlueprint<BlueprintBuff>(addFeature.m_Feature.Guid));
                    buffMapper.Add(buff.AssetGuid.m_Guid, buff);
                    buff.name = buff.name.Replace("Cavalier", "DivineCommander");
                    buff.AssetGuid = ModSettings.Blueprints.GetGUID(buff.name);
                    Resources.AddBlueprint(buff);
                    addFeature.m_Feature = buff.ToReference<BlueprintUnitFactReference>();                    
                }
            });
            Resources.AddBlueprint(warpriestTacticianSupportFeature);

            var tacticianDescription = Helpers.CreateString("DivineCommanderArchetype.Tactician.Description", "At 3rd level, a divine commander gains a teamwork feat as a bonus feat." +
                    " She must meet the prerequisites for this feat." +
                    " As a standard action, the divine commander can grant this feat to all allies within 30 feet who can see and hear her." +
                    " Allies retain the use of this bonus feat for 4 rounds, plus 1 round for every 2 levels beyond 3rd that the divine commander possesses." +
                    " Allies do not need to meet the prerequisites of this bonus feat." +
                    " The divine commander can use this ability once per day at 3rd level, plus one additional time per day at 9th and 15th levels.");


            var tacticianDescriptionGreater = Helpers.CreateString("DivineCommanderArchetype.TacticianGreater.Description", "At 12th level, the divine commander gains an additional teamwork feat as a bonus feat." +
                    " She must meet the prerequisites for this feat." +
                    " The divine commander can grant this feat to her allies using the battle tactician ability." +
                    " Additionally, using the battle tactician ability is now a swift action.");

            var warpriestTacticianAbility = Helpers.CreateCopy(CavalierTacticianAbility, bp => {
                bp.name = "DivineCommanderTacticianAbility";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_Description = tacticianDescription;

                bp.GetComponent<ContextRankConfig>().m_Class = new BlueprintCharacterClassReference[] {
                    WarpriestClass.ToReference<BlueprintCharacterClassReference>()
                };

                var applyFact = bp.GetComponent<AbilityApplyFact>();
                for (int i = 0; i < applyFact.m_Facts.Length; i++) {
                    var factRef = applyFact.m_Facts[i];
                    applyFact.m_Facts[i] = buffMapper[factRef.Guid.m_Guid].ToReference<BlueprintUnitFactReference>();
                }

                var resourceLogic = bp.GetComponent<AbilityResourceLogic>();
                resourceLogic.m_RequiredResource = tacticianAbilityResource.ToReference<BlueprintAbilityResourceReference>();
            });
            Resources.AddBlueprint(warpriestTacticianAbility);


            var warpriestTacticianAbilitySwift = Helpers.CreateCopy(CavalierTacticianAbilitySwift, bp => {
                bp.name = "DivineCommanderTacticianAbilitySwift";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_Description = tacticianDescription;


                bp.GetComponent<ContextRankConfig>().m_Class = new BlueprintCharacterClassReference[] {
                    WarpriestClass.ToReference<BlueprintCharacterClassReference>()
                };

                var applyFact = bp.GetComponent<AbilityApplyFact>();
                for (int i = 0; i < applyFact.m_Facts.Length; i++) {
                    var factRef = applyFact.m_Facts[i];
                    applyFact.m_Facts[i] = buffMapper[factRef.Guid.m_Guid].ToReference<BlueprintUnitFactReference>();
                }

                var resourceLogic = bp.GetComponent<AbilityResourceLogic>();
                resourceLogic.m_RequiredResource = tacticianAbilityResource.ToReference<BlueprintAbilityResourceReference>();
            });
            Resources.AddBlueprint(warpriestTacticianAbilitySwift);

            var warpriestTacticianFeature = Helpers.CreateCopy(CavalierTacticianFeature, bp => {
                bp.name = "WarpriestDivineCommanderTacticianFeature";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);

                bp.m_Description = tacticianDescription;

                var addAbilityComponent = bp.GetComponent<AddAbilityResources>();
                addAbilityComponent.m_Resource = tacticianAbilityResource.ToReference<BlueprintAbilityResourceReference>();

                var addFacts = bp.GetComponent<AddFacts>();
                addFacts.m_Facts[0] = warpriestTacticianAbility.ToReference<BlueprintUnitFactReference>();
                addFacts.m_Facts[1] = warpriestTacticianSupportFeature.ToReference<BlueprintUnitFactReference>();
            });
            Resources.AddBlueprint(warpriestTacticianFeature);


            var warpriestTacticianFeatureGreater = Helpers.CreateCopy(CavalierTacticianFeatureGreater, bp => {
                bp.name = "WarpriestDivineCommanderTacticianFeatureGreater";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);

                bp.m_Description = tacticianDescriptionGreater;

                var addFacts = bp.GetComponent<AddFeatureIfHasFact>();
                addFacts.m_CheckedFact = warpriestTacticianAbilitySwift.ToReference<BlueprintUnitFactReference>();
                addFacts.m_Feature = warpriestTacticianAbilitySwift.ToReference<BlueprintUnitFactReference>();
            });
            Resources.AddBlueprint(warpriestTacticianFeatureGreater);


            var archetype = Helpers.CreateBlueprint<BlueprintArchetype>("DivineCommanderArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString("DivineCommanderArchetype.Name", "Divine Commander");
                bp.LocalizedDescription = Helpers.CreateString("DivineCommanderArchetype.Description", "Some warpriests are called to lead great armies and face legions of foes. These divine commanders live for war and fight for glory." +
                    " Their hearts quicken at battle cries, and they charge forth with their deity’s symbol held high." +
                    " These leaders of armies do so to promote the agenda of their faith, and lead armies of devoted followers willing to give their lives for the cause.");


                var removeFeatures = new List<LevelEntry>();
                var addFeatures = new List<LevelEntry>();

                removeFeatures.AddFeatureModifierAtLevel(1, BlessingSelection1, BlessingSelection2);
                foreach (var level in new int[] { 3, 6, 12 }) {
                    removeFeatures.AddFeatureModifierAtLevel(level, BonusFeatSelection);
                }

                addFeatures.AddFeatureModifierAtLevel(1, warpriestAnimalCompanionSelection);
                addFeatures.AddFeatureModifierAtLevel(3, warpriestTacticianFeature, tacticianFeatSelection);
                addFeatures.AddFeatureModifierAtLevel(6, PetTemplateSelection);
                addFeatures.AddFeatureModifierAtLevel(12, warpriestTacticianFeatureGreater, tacticianFeatSelection);

                bp.RemoveFeatures = removeFeatures.ToArray();
                bp.AddFeatures = addFeatures.ToArray();

            });

            if (ModSettings.AddedContent.Archetypes.IsDisabled("DivineCommander")) { return; }
            WarpriestClass.m_Archetypes = WarpriestClass.m_Archetypes.AppendToArray(archetype.ToReference<BlueprintArchetypeReference>());
            Main.LogPatch("Added", archetype);
        }
    }
}
