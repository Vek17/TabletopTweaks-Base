using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.Feats {
    class UndersizedMount {
        public static void AddUndersizedMount() {
            var Icon_UndersizedMount = AssetLoader.LoadInternal("Feats", "Icon_UndersizedMount.png");
            var UndersizedMount = Helpers.CreateBlueprint<BlueprintFeature>("UndersizedMount", bp => {
                bp.SetName("Undersized Mount");
                bp.SetDescription("You can ride creatures equal to your own size category instead of only creatures larger than you.");
                bp.m_Icon = Icon_UndersizedMount;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.MountedCombatFeat };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.UndersizedMount;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SkillMobility;
                    c.Value = 1;
                });
            });

            if (ModSettings.AddedContent.Feats.IsDisabled("UndersizedMount")) { return; }
            FeatTools.AddAsFeat(UndersizedMount);
        }
        [HarmonyPatch(typeof(AbilityTargetIsSuitableMountSize), nameof(AbilityTargetIsSuitableMountSize.CanMount))]
        static class AbilityTargetIsSuitableMountSize_CanMount_UndersizedMount_Patch {
            static bool Prefix(UnitEntityData master, UnitEntityData pet, ref bool __result) {
                if (!master.CustomMechanicsFeature(CustomMechanicsFeature.UndersizedMount)) { return true; }

                __result = master != null && pet != null && pet.State.Size >= master.State.Size;
                return false;
            }
        }
    }
}
