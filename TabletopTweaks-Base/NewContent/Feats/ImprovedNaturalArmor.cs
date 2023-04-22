using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class ImprovedNaturalArmor {
        public static void AddImprovedNaturalArmor() {
            var ImprovedNaturalArmor = BlueprintTools.GetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");

            ImprovedNaturalArmor.TemporaryContext(bp => {
                bp.SetName(TTTContext, "Improved Natural Armor");
                bp.SetDescription(TTTContext, "This creature’s hide is tougher than most.\n" +
                    "The creature’s natural armor bonus increases by +1.\n" +
                    "A creature can gain this feat multiple times. Each time the creature takes the feat, its natural armor bonus increases by another point.");
                bp.Ranks = 20;
                bp.HideNotAvailibleInUI = true;
                bp.SetComponents();
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = (ModifierDescriptor)NaturalArmor.Stackable;
                    c.Value = 1;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Constitution;
                    c.Value = 13;
                });
                bp.AddComponent<PrerequisiteStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = ModifierDescriptor.NaturalArmor;
                });
                bp.AddPrerequisite<PrerequisiteIsPet>(c => {
                    c.HideInUI = true;
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("ImprovedNaturalArmor")) { return; }
            FeatTools.AddAsFeat(ImprovedNaturalArmor);
        }
    }
}
