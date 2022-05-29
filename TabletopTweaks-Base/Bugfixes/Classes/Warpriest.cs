using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Warpriest {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Warpriest_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Warpriest")) { return; }

                var WarpriestAspectOfWar = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("65cc7abc21826a344aa156e2a40dcecc");
                var WarpriestAlternateCapstone = NewContent.AlternateCapstones.Warpriest.WarpriestAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                WarpriestAspectOfWar.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.WarpriestClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == WarpriestAspectOfWar.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(WarpriestAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(WarpriestAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == WarpriestAspectOfWar.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(WarpriestAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Warpriest");
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
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("AirBlessing")) { return; }

                    var AirBlessingMajorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ac410725d8fc6fe4b81d47269f4f3ea1");

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
                    TTTContext.Logger.LogPatch("Patched", AirBlessingMajorBuff);
                }
                void PatchEarthBlessing() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("EarthBlessing")) { return; }

                    var EarthBlessingMinorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("8237e758e7520fe46894bd00ebc9fac7");
                    PatchElementalDamageOnAttack(EarthBlessingMinorBuff);
                }
                void PatchFireBlessing() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("FireBlessing")) { return; }

                    var FireBlessingMinorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("35d99b00e5a28ff42ae609be9d621fdb");
                    PatchElementalDamageOnAttack(FireBlessingMinorBuff);
                }
                void PatchWaterBlessing() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("WaterBlessing")) { return; }

                    var WaterBlessingMinorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("21cdbf11919e0eb4db1ed46ce488f206");
                    PatchElementalDamageOnAttack(WaterBlessingMinorBuff);
                }
                void PatchWeatherBlessing() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("WeatherBlessing")) { return; }

                    var WeatherBlessingMinorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("05a33a6177bf7f54695443fdf3faa701");
                    PatchElementalDamageOnAttack(WeatherBlessingMinorBuff);
                }
                void PatchLuckBlessing() {
                    if (TTTContext.Fixes.Warpriest.Base.IsDisabled("LuckBlessing")) { return; }

                    var LuckBlessingMajorFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0b59acd4d1fffa34da9fc91da05dd398");
                    var LuckBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("49fa2b54589c34a42b8f06b8de1a6639");
                    LuckBlessingMajorFeature.GetComponent<AddFacts>().m_Facts = new BlueprintUnitFactReference[] { LuckBlessingMajorAbility };
                    TTTContext.Logger.LogPatch("Patched", LuckBlessingMajorFeature);
                }

                void PatchElementalDamageOnAttack(BlueprintBuff buff) {
                    var DealDamage = buff.FlattenAllActions().OfType<ContextActionDealDamage>().First();
                    buff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    buff.AddComponent<AddAdditionalWeaponDamage>(c => {
                        c.Value = DealDamage.Value;
                        c.DamageType = DealDamage.DamageType;
                    });
                    TTTContext.Logger.LogPatch("Patched", buff);
                }
            }

            static void PatchArchetypes() {
            }
        }
    }
}
