using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.Base.NewContent.Feats {
    class UndersizedMount {
        public static void AddUndersizedMount() {
            var Icon_UndersizedMount = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_UndersizedMount.png");
            var UndersizedMount = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "UndersizedMount", bp => {
                bp.SetName(TTTContext, "Undersized Mount");
                bp.SetDescription(TTTContext, "You can ride creatures equal to your own size category instead of only creatures larger than you.");
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

            if (TTTContext.AddedContent.Feats.IsDisabled("UndersizedMount")) { return; }
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
