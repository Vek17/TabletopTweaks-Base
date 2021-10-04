using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class Stalwart {
        public static void AddStalwart() {
            var Diehard = Resources.GetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad");

            var CombatExpertiseBuff = Resources.GetBlueprint<BlueprintBuff>("e81cd772a7311554090e413ea28ceea1");
            var FightDefensivelyBuff = Resources.GetBlueprint<BlueprintBuff>("6ffd93355fb3bcf4592a5d976b1d32a9");
            var CraneStyleBuff = Resources.GetBlueprint<BlueprintBuff>("e8ea7bd10136195478d8a5fc5a44c7da");
            var CautiousFighter = Resources.GetBlueprint<BlueprintFeature>("4a6fbe77a4a2ce24db0cd0b1e4d93db1");
            var SwordLordSteelNetFeature = Resources.GetBlueprint<BlueprintFeature>("b4202533d1748f84484658491d2ff766");


            var StalwartImprovedFeature = Helpers.CreateBlueprint<BlueprintFeature>("StalwartImprovedFeature", bp => {
                bp.SetName("Improved Stalwart");
                bp.SetDescription("Double the DR you gain from Stalwart, to a maximum of DR 10/—.");
            });

            var StalwartDRPpropertyBlueprint = Helpers.CreateBlueprint<BlueprintUnitProperty>("StalwartDRProperty", bp => {
                bp.AddComponent(new StalwartDRProperty(
                    CombatExpertiseBuff.ToReference<BlueprintUnitFactReference>(), 
                    FightDefensivelyBuff.ToReference<BlueprintUnitFactReference>(), 
                    CraneStyleBuff.ToReference<BlueprintUnitFactReference>(), 
                    CautiousFighter.ToReference<BlueprintUnitFactReference>(), 
                    SwordLordSteelNetFeature.ToReference<BlueprintUnitFactReference>(),
                    StalwartImprovedFeature.ToReference<BlueprintUnitFactReference>()));
            });

            var StalwartFeature = Helpers.CreateBlueprint<BlueprintFeature>("StalwartFeature", bp => {
                bp.SetName("Stalwart");
                bp.SetDescription("While fighting defensively or using Combat Expertise, " +
                    "you can forgo the dodge bonus to AC you would normally gain to instead gain an equivalent amount of DR, " +
                    "to a maximum of DR 5/—, until the start of your next turn. This damage reduction stacks with DR you gain from class features, " +
                    "such as the barbarian’s, but not with DR from any other source.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                // TODO: move these to the buff granted by the toggle granted by this feat
                bp.AddComponent<TTAddDamageResistancePhysical>(c => {
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_CheckedFactMythic = BlueprintReferenceBase.CreateTyped<BlueprintUnitFactReference>(null);
                    c.m_WeaponType = BlueprintReferenceBase.CreateTyped<BlueprintWeaponTypeReference>(null);
                    c.Pool = new ContextValue { };
                    c.IsStacksWithClassFeatures = true;
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_Feature = BlueprintReferenceBase.CreateTyped<BlueprintFeatureReference>(null);
                    c.m_FeatureList = Array.Empty<BlueprintFeatureReference>();
                    c.m_Buff = BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
                    c.m_CustomProgression = Array.Empty<ContextRankConfig.CustomProgressionItem>();
                    c.Archetype = BlueprintReferenceBase.CreateTyped<BlueprintArchetypeReference>(null);
                    c.m_AdditionalArchetypes = Array.Empty<BlueprintArchetypeReference>();
                    c.m_Class = Array.Empty<BlueprintCharacterClassReference>();
                    c.m_CustomPropertyList = Array.Empty<BlueprintUnitPropertyReference>();
                    c.m_CustomProperty = StalwartDRPpropertyBlueprint.ToReference<BlueprintUnitPropertyReference>();
                });
                bp.AddPrerequisite<PrerequisiteFeature>(p => {
                    p.m_Feature = Diehard.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(p => {
                    p.Stat = StatType.BaseAttackBonus;
                    p.Value = 4;
                });
            });

            StalwartImprovedFeature.AddPrerequisite<PrerequisiteFeature>(p => {
                p.m_Feature = Diehard.ToReference<BlueprintFeatureReference>();
            });
            StalwartImprovedFeature.AddPrerequisite<PrerequisiteFeature>(p => {
                p.m_Feature = StalwartFeature.ToReference<BlueprintFeatureReference>();
            });
            StalwartImprovedFeature.AddPrerequisite<PrerequisiteStatValue>(p => {
                p.Stat = StatType.BaseAttackBonus;
                p.Value = 11;
            });
        }
    }
}
