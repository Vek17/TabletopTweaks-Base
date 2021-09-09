using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;


namespace TabletopTweaks.NewContent.Custom
{
    static class VitalStrikeFix
    {
        static readonly BlueprintAbility VitalStrikeAbility = Resources.GetBlueprint<BlueprintAbility>("efc60c91b8e64f244b95c66b270dbd7c");
        static readonly BlueprintAbility VitalStrikeAbilityImproved = Resources.GetBlueprint<BlueprintAbility>("c714cd636700ac24a91ca3df43326b00");
        static readonly BlueprintAbility VitalStrikeAbilityGreater = Resources.GetBlueprint<BlueprintAbility>("11f971b6453f74d4594c538e3c88d499");

        public static void FixVitalStrike()
        {
            AttachAbilityDeliver(VitalStrikeAbility);
            AttachAbilityDeliver(VitalStrikeAbilityImproved);
            AttachAbilityDeliver(VitalStrikeAbilityGreater);
            
            void AttachAbilityDeliver(BlueprintAbility vitalStrike)
            {
                vitalStrike.RemoveComponents<AbilityCustomMeleeAttack>();

                vitalStrike.AddComponent(Helpers.Create<AbilityDeliverVitalStrikeWithWeapon>(c => {
                    c.VitalStrikeMod = 2;
                }));
                vitalStrike.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
            }
        }
    }
}
