using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.Bugfixes.Classes {
    class Warpriest {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Warpriest");
                PatchBase();
            }
            static void PatchBase() {
                PatchAirBlessing();
                PatchEarthBlessing();
                PatchFireBlessing();
                PatchWaterBlessing();
                PatchWeatherBlessing();
                PatchLuckBlessing();

                void PatchAirBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("AirBlessing")) { return; }

                    var AirBlessingMajorBuff = Resources.GetBlueprint<BlueprintBuff>("ac410725d8fc6fe4b81d47269f4f3ea1");

                    var DealDamage = AirBlessingMajorBuff.FlattenAllActions().OfType<ContextActionDealDamage>().First();
                    AirBlessingMajorBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    AirBlessingMajorBuff.AddComponent<AdditionalDiceOnAttack>(c => {
                        c.OnHit = true;
                        c.OnCharge = true;
                        c.InitiatorConditions = new ConditionsChecker();
                        c.TargetConditions = new ConditionsChecker();
                        c.Value = DealDamage.Value;
                        c.DamageType = DealDamage.DamageType;
                    });
                    Main.LogPatch("Patched", AirBlessingMajorBuff);
                }
                void PatchEarthBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("EarthBlessing")) { return; }

                    var EarthBlessingMinorBuff = Resources.GetBlueprint<BlueprintBuff>("8237e758e7520fe46894bd00ebc9fac7");
                    PatchElementalDamageOnAttack(EarthBlessingMinorBuff);
                }
                void PatchFireBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("FireBlessing")) { return; }

                    var FireBlessingMinorBuff = Resources.GetBlueprint<BlueprintBuff>("35d99b00e5a28ff42ae609be9d621fdb");
                    PatchElementalDamageOnAttack(FireBlessingMinorBuff);
                }
                void PatchWaterBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("WaterBlessing")) { return; }

                    var WaterBlessingMinorBuff = Resources.GetBlueprint<BlueprintBuff>("21cdbf11919e0eb4db1ed46ce488f206");
                    PatchElementalDamageOnAttack(WaterBlessingMinorBuff);
                }
                void PatchWeatherBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("WeatherBlessing")) { return; }

                    var WeatherBlessingMinorBuff = Resources.GetBlueprint<BlueprintBuff>("05a33a6177bf7f54695443fdf3faa701");
                    PatchElementalDamageOnAttack(WeatherBlessingMinorBuff);
                }
                void PatchLuckBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("LuckBlessing")) { return; }

                    var LuckBlessingMajorFeature = Resources.GetBlueprint<BlueprintFeature>("0b59acd4d1fffa34da9fc91da05dd398");
                    var LuckBlessingMajorAbility = Resources.GetBlueprintReference<BlueprintUnitFactReference>("49fa2b54589c34a42b8f06b8de1a6639");
                    LuckBlessingMajorFeature.GetComponent<AddFacts>().m_Facts = new BlueprintUnitFactReference[] { LuckBlessingMajorAbility };
                    Main.LogPatch("Patched", LuckBlessingMajorFeature);
                }

                void PatchElementalDamageOnAttack(BlueprintBuff buff) {
                    var DealDamage = buff.FlattenAllActions().OfType<ContextActionDealDamage>().First();
                    buff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    buff.AddComponent<AddAdditionalWeaponDamage>(c => {
                        c.Value = DealDamage.Value;
                        c.DamageType = DealDamage.DamageType;
                    });
                    Main.LogPatch("Patched", buff);
                }
            }

            static void PatchArchetypes() {
            }
        }
    }
}
