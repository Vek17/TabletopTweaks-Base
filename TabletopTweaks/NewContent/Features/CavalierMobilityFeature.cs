using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class CavalierMobilityFeature {
        public static void AddCavalierMobilityFeature() {
            var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");

            var CavalierMobilityBuff = Helpers.CreateBuff("CavalierMobilityBuff", bp => {
                bp.SetName("Cavalier Mobility");
                bp.SetDescription("A cavalier does not take an armor check penalty on Mobility checks while riding his mount.");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<NegateArmorCheckSkillPenalty>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.SkillMobility;
                }));
            });

            var CavalierMobilityFeature = Helpers.CreateBlueprint<BlueprintFeature>("CavalierMobilityFeature", bp => {
                bp.SetName("Cavalier Mobility");
                bp.SetDescription("A cavalier does not take an armor check penalty on Mobility checks while riding his mount.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<BuffExtraEffects>(c => {
                    c.m_CheckedBuff = MountedBuff.ToReference<BlueprintBuffReference>();
                    c.m_ExtraEffectBuff = CavalierMobilityBuff.ToReference<BlueprintBuffReference>();
                }));
            });
        }
    }
}
