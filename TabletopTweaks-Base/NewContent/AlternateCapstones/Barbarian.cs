using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Barbarian {
        public static BlueprintFeatureSelection BarbarianAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var MightyRage = BlueprintTools.GetBlueprint<BlueprintFeature>("06a7e5b60020ad947aed107d82d1f897");

            var Unstoppable = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "Unstoppable", bp => {
                bp.SetName(TTTContext, "Unstoppable");
                bp.SetDescription(TTTContext, "At 20th level, nothing can kill the barbarian, though not for lack of trying.\n" +
                    "The barbarian gains DR 3/— or increases the value of any existing damage reduction by 3. In addition, she gains 20 energy resistance to acid, cold, electricity, and fire.");
                bp.IsClassFeature = true;
                if (TTTContext.Fixes.BaseFixes.IsDisabled("DamageReductionRework")) {
                    bp.AddComponent<AddDamageResistancePhysical>(c => {
                        c.Value = 3;
                        c.m_CheckedFactMythic = BlueprintReferenceBase.CreateTyped<BlueprintUnitFactReference>(null);
                        c.m_WeaponType = BlueprintReferenceBase.CreateTyped<BlueprintWeaponTypeReference>(null);
                        c.Pool = new ContextValue();
                    });
                } else {
                    bp.AddComponent<TTAddDamageResistancePhysical>(c => {
                        c.Value = 3;
                        c.m_CheckedFactMythic = BlueprintReferenceBase.CreateTyped<BlueprintUnitFactReference>(null);
                        c.m_WeaponType = BlueprintReferenceBase.CreateTyped<BlueprintWeaponTypeReference>(null);
                        c.Pool = new ContextValue();
                        c.AddToAllStacks = true;
                    });
                }
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Fire;
                    c.Value = 20;
                    c.ValueMultiplier = 1;
                    c.Pool = new ContextValue();
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Cold;
                    c.Value = 20;
                    c.ValueMultiplier = 1;
                    c.Pool = new ContextValue();
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Acid;
                    c.Value = 20;
                    c.ValueMultiplier = 1;
                    c.Pool = new ContextValue();
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Electricity;
                    c.Value = 20;
                    c.ValueMultiplier = 1;
                    c.Pool = new ContextValue();
                });
            });
            BarbarianAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BarbarianAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = MightyRage.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    MightyRage,
                    Unstoppable,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
