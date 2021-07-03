using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class InstinctualWarriorACBonusUnlock {
        public static void AddInstinctualWarriorACBonusUnlock() {
            var CunningElusionFeature = Resources.GetBlueprint<BlueprintFeature>("a71103ce28964f39b38442baa32a3031");
            var InstinctualWarriorACBonusUnlock = Helpers.Create<BlueprintFeature>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("InstinctualWarriorACBonusUnlock");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.name = "InstinctualWarriorACBonusUnlock";
                bp.m_Icon = CunningElusionFeature.Icon;
                bp.SetName(CunningElusionFeature.Name);
                bp.SetDescription(CunningElusionFeature.Description);
                bp.AddComponent(Helpers.Create<MonkNoArmorFeatureUnlock>(c => {
                    c.m_NewFact = CunningElusionFeature.ToReference<BlueprintUnitFactReference>();
                }));
            });
            Resources.AddBlueprint(InstinctualWarriorACBonusUnlock);
        }
    }
}
