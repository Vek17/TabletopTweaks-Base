using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.RagePowers {
    internal class GreaterAnimalFury {
        public static void AddGreaterAnimalFury() {
            var AnimalFuryFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("25954b1652bebc2409f9cb9d5728bceb");
            var AnimalFuryRageBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a67b51a8074ae47438280be0a87b01b6");

            var GreaterAnimalFuryBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "GreaterAnimalFuryBuff", bp => {
                bp.SetName(TTTContext, "Greater Animal Fury");
                bp.SetDescription(TTTContext, "The bite attack from animal fury deals damage as if you were one size larger.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<WeaponSizeChangeTTT>(c => {
                    c.CheckWeaponCategory = true;
                    c.Categories = new WeaponCategory[] { WeaponCategory.Bite };
                    c.SizeChange = 1;
                });
            });
            var GreaterAnimalFuryFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "GreaterAnimalFuryFeature", bp => {
                bp.SetName(GreaterAnimalFuryBuff.m_DisplayName);
                bp.SetDescription(GreaterAnimalFuryBuff.m_Description);
                bp.m_Icon = AnimalFuryFeature.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.RagePower };
                bp.AddComponent<BuffExtraEffects>(c => {
                    c.m_CheckedBuff = AnimalFuryRageBuff;
                    c.m_ExtraEffectBuff = GreaterAnimalFuryBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddPrerequisiteFeature(AnimalFuryFeature);
            });
            if (TTTContext.AddedContent.RagePowers.IsDisabled("GreaterAnimalFury")) { return; }
            FeatTools.AddAsRagePower(GreaterAnimalFuryFeature);
        }
    }
}
