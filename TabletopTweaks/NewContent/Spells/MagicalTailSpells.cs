using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Spells {
    class MagicalTailSpells {
        public static void AddNewMagicalTailSpells() {

            BlueprintAbility sleepKitsune = Resources.GetBlueprint<BlueprintAbility>("f8a32c60ae1f878408b525bb967ef48c");
            BlueprintAbility hideousLaughter = Resources.GetBlueprint<BlueprintAbility>("fd4d9fd7f87575d47aafe2a64a6e2d8d");

            BlueprintAbility deepSlumberKitsune = Resources.GetBlueprint<BlueprintAbility>("2bc8d4bb8baa23a4b84ef34945d13733");
            BlueprintAbility heroism = Resources.GetBlueprint<BlueprintAbility>("5ab0d42fb68c9e34abae4921822b9d63");

            var hideousLaughterKitsune = Helpers.CreateCopy(hideousLaughter, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("HideousLaughterKitsune");
            });

            hideousLaughterKitsune.RemoveComponents<AbilityResourceLogic>();
            hideousLaughterKitsune.AddComponent<AbilityResourceLogic>(c => {
                c.Amount = sleepKitsune.GetComponent<AbilityResourceLogic>().Amount;
                c.CostIsCustom = sleepKitsune.GetComponent<AbilityResourceLogic>().CostIsCustom;
                c.m_IsSpendResource = sleepKitsune.GetComponent<AbilityResourceLogic>().m_IsSpendResource;
                c.m_RequiredResource = sleepKitsune.GetComponent<AbilityResourceLogic>().m_RequiredResource;
                c.name = sleepKitsune.GetComponent<AbilityResourceLogic>().name;
                c.ResourceCostDecreasingFacts = sleepKitsune.GetComponent<AbilityResourceLogic>().ResourceCostDecreasingFacts;
                c.ResourceCostIncreasingFacts = sleepKitsune.GetComponent<AbilityResourceLogic>().ResourceCostIncreasingFacts;
            });

            Resources.AddBlueprint(hideousLaughterKitsune);

            var heroismKitsune = Helpers.CreateCopy(heroism, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("HeroismKitsune");
            });

            heroismKitsune.RemoveComponents<AbilityResourceLogic>();
            heroismKitsune.AddComponent<AbilityResourceLogic>(c => {
                c.Amount = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().Amount;
                c.CostIsCustom = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().CostIsCustom;
                c.m_IsSpendResource = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().m_IsSpendResource;
                c.m_RequiredResource = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().m_RequiredResource;
                c.name = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().name;
                c.ResourceCostDecreasingFacts = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().ResourceCostDecreasingFacts;
                c.ResourceCostIncreasingFacts = deepSlumberKitsune.GetComponent<AbilityResourceLogic>().ResourceCostIncreasingFacts;
            });

            Resources.AddBlueprint(heroismKitsune);
        }
    }
}
