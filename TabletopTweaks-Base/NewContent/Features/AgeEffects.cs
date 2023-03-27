using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    internal class AgeEffects {
        public static void AddAgeEffects() {
            var DLC3_HasteIslandAge1 = BlueprintTools.GetBlueprint<BlueprintBuff>("0aca74909b144441a31866b977572f91");
            var DLC3_HasteIslandAge2 = BlueprintTools.GetBlueprint<BlueprintBuff>("b26c4f0f18ed41db9f78cfda0c7b874b");
            var DLC3_HasteIslandAge3 = BlueprintTools.GetBlueprint<BlueprintBuff>("9cab5a802dfd4e3e86a0623046bf88aa");

            var ConstructType = BlueprintTools.GetBlueprint<BlueprintFeature>("fd389783027d63343b4a5634bd81645f");
            var UndeadType = BlueprintTools.GetBlueprint<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33");
            var SubtypeElemental = BlueprintTools.GetBlueprint<BlueprintFeature>("198fd8924dabcb5478d0f78bd453c586");

            var AgelessFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AgelessFeature", bp => {
                bp.SetName(TTTContext, "Ageless");
                bp.SetDescription(TTTContext, "Anything ageless is immune to the effects of aging.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Mental;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                    c.Type = UnitPartAgeTTT.NegateType.Mental;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.Venerable;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.Venerable;
                    c.Type = UnitPartAgeTTT.NegateType.Mental;
                });
            });

            DLC3_HasteIslandAge1.TemporaryContext(bp => {
                bp.SetDescription(TTTContext, "You gain a +1 bonus to Intelligence, Wisdom, and Charisma and suffer a -1 penalty to Strength, Dexterity, and Constitution.");
                bp.SetComponents();
                bp.AddComponent<AddAgeStatChanges>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                });
            });
            DLC3_HasteIslandAge2.TemporaryContext(bp => {
                bp.SetDescription(TTTContext, "You gain a +2 bonus to Intelligence, Wisdom, and Charisma and suffer a -3 penalty to Strength, Dexterity, and Constitution.");
                bp.SetComponents();
                bp.AddComponent<AddAgeStatChanges>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                });
            });
            DLC3_HasteIslandAge3.TemporaryContext(bp => {
                bp.SetDescription(TTTContext, "You gain a +3 bonus to Intelligence, Wisdom, and Charisma and suffer a -6 penalty to Strength, Dexterity, and Constitution.");
                bp.SetComponents();
                bp.AddComponent<AddAgeStatChanges>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.Venerable;
                });
            });

            MakeAgeless(ConstructType);
            MakeAgeless(SubtypeElemental);
            MakeAgeless(UndeadType);

            void MakeAgeless(BlueprintFeature feature) {
                feature.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        AgelessFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            }
        }
    }
}
