using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    internal class WinterWitch {
        public static void AddWinterWitchFeatures() {
            var WinterWitchUnnaturalCold = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WinterWitchUnnaturalCold", bp => {
                bp.SetName(TTTContext, "Unnatural Cold");
                bp.SetDescription(TTTContext, "At 3rd level, whenever a winter witch’s spell, spell-like ability, or supernatural ability deals cold damage, treat affected creatures as having half their normal cold resistance when determining the damage dealt.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<PartialEnergyResistanceIgnore>(c => {
                    c.EnergyType = DamageEnergyType.Cold;
                    c.CheckAbilityType = true;
                    c.ValidAbilityTypes = new AbilityType[] { AbilityType.Spell, AbilityType.SpellLike, AbilityType.Supernatural };
                    c.ByHalf = true;
                });
            });
            var WinterWitchColdFlesh5 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WinterWitchColdFlesh5", bp => {
                bp.SetName(TTTContext, "Cold Flesh");
                bp.SetDescription(TTTContext, "At 1st level, a winter witch gains cold resistance 5, making her comfortable in near-freezing temperatures.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Cold;
                    c.Value = 5;
                    c.Pool = new ContextValue();
                    c.ValueMultiplier = new ContextValue();
                });
            });
            var WinterWitchColdFlesh10 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WinterWitchColdFlesh10", bp => {
                bp.SetName(TTTContext, "Cold Flesh");
                bp.SetDescription(TTTContext, "At 5th level, a winter witch's cold resistance increaes to 10.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Cold;
                    c.Value = 10;
                    c.Pool = new ContextValue();
                    c.ValueMultiplier = new ContextValue();
                });
            });
            var WinterWitchColdFleshImmunity = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WinterWitchColdFleshImmunity", bp => {
                bp.SetName(TTTContext, "Cold Flesh");
                bp.SetDescription(TTTContext, "At 10th level, a winter witch becomes immune to cold damage.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddEnergyImmunity>(c => {
                    c.Type = DamageEnergyType.Cold;
                });
            });
        }
    }
}
