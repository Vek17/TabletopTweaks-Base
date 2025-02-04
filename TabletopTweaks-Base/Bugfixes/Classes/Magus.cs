using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.ActivatableAbilitySpendLogic;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Magus {
        [PatchBlueprintsCacheInit]
        static class Magus_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Magus")) { return; }

                var TrueMagusFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("789c7539ba659174db702e18d7c2d330");
                var MagusAlternateCapstone = NewContent.AlternateCapstones.Magus.MagusAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                TrueMagusFeature.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.MagusClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == TrueMagusFeature.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(MagusAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(MagusAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == TrueMagusFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(MagusAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Magus Resources");

                PatchBase();
                PatchEldritchScion();
                PatchHexcrafter();
                PatchSwordSaint();
            }
            static void PatchBase() {
                PatchSpellCombatDisableImmediatly();
                PatchArcaneWeaponProperties();
                PatchFighterTraining();

                void PatchSpellCombatDisableImmediatly() {
                    if (TTTContext.Fixes.Magus.Base.IsDisabled("SpellCombatDisableImmediatly")) { return; }

                    var SpellCombatAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("8898a573e8a8a184b8186dbc3a26da74");
                    var SpellStrikeAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("e958891ef90f7e142a941c06c811181e");

                    SpellCombatAbility.DeactivateImmediately = true;
                    SpellStrikeAbility.DeactivateImmediately = true;

                    TTTContext.Logger.LogPatch("Patched", SpellCombatAbility);
                    TTTContext.Logger.LogPatch("Patched", SpellStrikeAbility);
                }
                void PatchFighterTraining() {
                    if (TTTContext.Fixes.Magus.Base.IsDisabled("FighterTraining")) { return; }

                    var FighterTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("2b636b9e8dd7df94cbd372c52237eebf");
                    QuickFixTools.ReplaceClassLevelsForPrerequisites(FighterTraining, TTTContext, FeatureGroup.Feat);

                    TTTContext.Logger.LogPatch(FighterTraining);
                }
                void PatchArcaneWeaponProperties() {
                    if (TTTContext.Fixes.Magus.Base.IsDisabled("FixBurstStacking")) { return; }

                    var ArcaneWeaponFlamingBurstChoice_TTT = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(TTTContext, "ArcaneWeaponFlamingBurstChoice_TTT");
                    var ArcaneWeaponIcyBurstChoice_TTT = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(TTTContext, "ArcaneWeaponIcyBurstChoice_TTT");
                    var ArcaneWeaponShockingBurstChoice_TTT = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(TTTContext, "ArcaneWeaponShockingBurstChoice_TTT");
                    var ArcaneWeaponPlus2 = BlueprintTools.GetBlueprint<BlueprintFeature>("36b609a6946733c42930c55ac540416b");

                    var ArcaneWeaponFlamingBurstChoice = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fa45899aef2444bc8928bf658d59c016");
                    var ArcaneWeaponIcyBurstChoice = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("7ec3a153b88249978995e1a599bb1bef");
                    var ArcaneWeaponShockingBurstChoice = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("675450197b62498db71bcb677b2c1304");

                    var ArcaneWeaponFlamingBurstBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("bca914e2b1814ff886c7a91de104fd46");
                    var ArcaneWeaponIcyBurstBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("85d5bfd7c0f54adb82444877df1712b0");
                    var ArcaneWeaponShockingBurstBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0bacae88449140bdb5368354ffdd410a");

                    var ArcaneWeaponFlamingBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("32e17840df49fbd48b835d080f5673a4");
                    var ArcaneWeaponFrostBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("39f8c2ca61fa4bb419b13813001125ce");
                    var ArcaneWeaponShockBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5b76e44a1ed84704e858c38e7e97e7f2");
                    /*
                    var AddFacts = ArcaneWeaponPlus2.GetComponent<AddFacts>();
                    AddFacts.m_Facts = AddFacts.m_Facts.AppendToArray(
                        ArcaneWeaponFlamingBurstChoice_TTT.ToReference<BlueprintUnitFactReference>(),
                        ArcaneWeaponIcyBurstChoice_TTT.ToReference<BlueprintUnitFactReference>(),
                        ArcaneWeaponShockingBurstChoice_TTT.ToReference<BlueprintUnitFactReference>()
                    );
                    TTTContext.Logger.LogPatch("Patched", ArcaneWeaponPlus2);
                    */

                    AddExclusions(ArcaneWeaponFlamingBuff, ArcaneWeaponFlamingBurstBuff);
                    AddExclusions(ArcaneWeaponFrostBuff, ArcaneWeaponIcyBurstBuff);
                    AddExclusions(ArcaneWeaponShockBuff, ArcaneWeaponShockingBurstBuff);

                    void AddExclusions(BlueprintBuff normal, BlueprintBuff burst) {
                        normal.AddComponent<AddFactContextActions>(c => {
                            c.Activated = Helpers.CreateActionList(
                                new ContextActionRemoveBuff() { 
                                    m_Buff = burst.ToReference<BlueprintBuffReference>()
                                }
                            );
                            c.Deactivated = Helpers.CreateActionList();
                            c.NewRound = Helpers.CreateActionList();
                            c.Dispose = Helpers.CreateActionList();
                        });
                        burst.AddComponent<AddFactContextActions>(c => {
                            c.Activated = Helpers.CreateActionList(
                                new ContextActionRemoveBuff() {
                                    m_Buff = normal.ToReference<BlueprintBuffReference>()
                                }
                            );
                            c.Deactivated = Helpers.CreateActionList();
                            c.NewRound = Helpers.CreateActionList();
                            c.Dispose = Helpers.CreateActionList();
                        });
                        TTTContext.Logger.LogPatch(normal);
                        TTTContext.Logger.LogPatch(burst);
                    }
                }
            }
            static void PatchEldritchScion() {
                PatchFighterTraining();

                void PatchFighterTraining() {
                    if (TTTContext.Fixes.Magus.Archetypes["EldritchScion"].IsDisabled("FighterTraining")) { return; }

                    var EldritchScionFighterTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("eadc14e84c7e3c34684252b5c6459a45");
                    QuickFixTools.ReplaceClassLevelsForPrerequisites(EldritchScionFighterTraining, TTTContext, FeatureGroup.Feat);

                    TTTContext.Logger.LogPatch(EldritchScionFighterTraining);
                }
            }
            static void PatchHexcrafter() {
                PatchHexcrafterSpells();

                void PatchHexcrafterSpells() {
                    if (TTTContext.Fixes.Magus.Archetypes["Hexcrafter"].IsDisabled("Spells")) { return; }

                    var HexcrafterSpells = BlueprintTools.GetBlueprint<BlueprintFeature>("8122e8b3ddb1e184ebf6decc8b1403b5");
                    var CurseSpells = SpellTools.Spellbook.AllSpellbooks
                        .Where(book => !book.IsMythic)
                        .Select(book => book.SpellList)
                        .SelectMany(list => {
                            var AllSpells = new List<BlueprintAbility>();
                            AllSpells.AddRange(GetUnfilteredSpells(list, 1));
                            AllSpells.AddRange(GetUnfilteredSpells(list, 2));
                            AllSpells.AddRange(GetUnfilteredSpells(list, 3));
                            AllSpells.AddRange(GetUnfilteredSpells(list, 4));
                            AllSpells.AddRange(GetUnfilteredSpells(list, 5));
                            AllSpells.AddRange(GetUnfilteredSpells(list, 6));
                            return AllSpells;
                        })
                        .Distinct()
                        .Where(spell => spell.SpellDescriptor.HasFlag(SpellDescriptor.Curse))
                        .ToList();

                    HexcrafterSpells.TemporaryContext(bp => {
                        bp.SetComponents();
                        AddCurseSpells(bp, CurseSpells, 1);
                        AddCurseSpells(bp, CurseSpells, 2);
                        AddCurseSpells(bp, CurseSpells, 3);
                        AddCurseSpells(bp, CurseSpells, 4);
                        AddCurseSpells(bp, CurseSpells, 5);
                        AddCurseSpells(bp, CurseSpells, 6);
                    });

                    void AddCurseSpells(BlueprintFeature feature, IEnumerable<BlueprintAbility> spells, int spellLevel) {
                        spells
                            .Where(spell => LowestSpellLevel(spell) == spellLevel)
                            .Where(spell => SpellTools.SpellList.MagusSpellList.GetLevel(spell) == -1) //This returns -1 if the spell is not found
                            .Select(spell => spell.ToReference<BlueprintAbilityReference>())
                            .ForEach(spell => {
                                feature.AddComponent<AddKnownSpell>(c => {
                                    c.m_CharacterClass = ClassTools.ClassReferences.MagusClass;
                                    c.m_Archetype = new BlueprintArchetypeReference();
                                    c.m_Spell = spell;
                                    c.SpellLevel = spellLevel;
                                });
                            });
                    }

                    int LowestSpellLevel(BlueprintAbility spell) {
                        return spell.GetComponents<SpellListComponent>()
                            .Select(c => c.SpellLevel)
                            .Min();
                    }

                    List<BlueprintAbility> GetUnfilteredSpells(BlueprintSpellList list, int spellLevel) {
                        if (spellLevel < 0 || spellLevel >= list.SpellsByLevel.Length) {
                            return BlueprintSpellList.EmptyAbilitiesList;
                        }
                        return list.SpellsByLevel[spellLevel].Spells.ToList();
                    }
                }
            }
            static void PatchSwordSaint() {
                PatchFighterTraining();
                PatchPerfectCritical();

                void PatchFighterTraining() {
                    if (TTTContext.Fixes.Magus.Archetypes["SwordSaint"].IsDisabled("FighterTraining")) { return; }

                    var SwordSaintFighterTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("9ab2ec65977cc524a99600babc7fe3b6");
                    QuickFixTools.ReplaceClassLevelsForPrerequisites(SwordSaintFighterTraining, TTTContext, FeatureGroup.Feat);

                    TTTContext.Logger.LogPatch(SwordSaintFighterTraining);
                }
                void PatchPerfectCritical() {
                    if (TTTContext.Fixes.Magus.Archetypes["SwordSaint"].IsDisabled("PerfectCritical")) { return; }

                    var SwordSaintPerfectStrikeCritAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("c6559839738a7fc479aadc263ff9ffff");

                    SwordSaintPerfectStrikeCritAbility.SetDescription(TTTContext, "At 4th level, when a sword saint confirms a critical hit, " +
                        "he can spend 2 points from his arcane pool to increase his weapon's critical multiplier by 1.");
                    SwordSaintPerfectStrikeCritAbility
                        .GetComponent<ActivatableAbilityResourceLogic>()
                        .SpendType = ValueSpendType.Crit.Amount(2);
                    TTTContext.Logger.LogPatch("Patched", SwordSaintPerfectStrikeCritAbility);
                }
            }
        }
        [HarmonyPatch(typeof(ItemEntityWeapon), "HoldInTwoHands", MethodType.Getter)]
        static class ItemEntityWeapon_HoldInTwoHands_Patch {
            static void Postfix(ItemEntityWeapon __instance, ref bool __result) {
                if (TTTContext.Fixes.Magus.Base.IsDisabled("SpellCombatDisableImmediatly")) { return; }
                var magusPart = __instance?.Wielder?.Get<UnitPartMagus>();
                if (magusPart == null) { return; }
                if (AvailableSpellStike(magusPart)) {
                    if (__instance.Blueprint.Type.IsOneHandedWhichCanBeUsedWithTwoHands && !__instance.Blueprint.IsTwoHanded) {
                        __result = false;
                    }
                }
            }
            static bool AvailableSpellStike(UnitPartMagus magusPart) {
                return Game.Instance.TimeController.GameTime - magusPart.LastSpellCombatOpportunityTime < 1.Rounds().Seconds;
            }
        }

        // Prevents non magus classes from throwing errors during spell combat/strike
        [HarmonyPatch(typeof(UnitPartMagus), "Spellbook", MethodType.Getter)]
        class UnitPartMagus_Spellbook_Patch {
            static bool Prefix(UnitPartMagus __instance, ref Spellbook __result) {
                if (__instance.m_Spellbook == null) {
                    ClassData classData = __instance.Owner.Progression.GetClassData(__instance.Class);
                    BlueprintSpellbook blueprintSpellbook;
                    if ((blueprintSpellbook = classData?.Spellbook) == null) {
                        BlueprintCharacterClass blueprintCharacterClass = __instance.Class;
                        blueprintSpellbook = blueprintCharacterClass?.Spellbook;
                    }
                    __instance.m_Spellbook = ((blueprintSpellbook != null) ? __instance.Owner.GetSpellbook(blueprintSpellbook) : null);
                }
                __result = __instance.m_Spellbook;
                return false;
            }
        }

        // Prevents non magus books from spell combat and spell strike
        [HarmonyPatch(typeof(UnitPartMagus), "IsSpellFromMagusSpellList", new Type[] { typeof(AbilityData) })]
        class UnitPartMagus_IsSpellFromMagusSpellList_Base_Patch {
            static bool Prefix(UnitPartMagus __instance, ref bool __result, AbilityData spell) {
                bool validWandSpell = __instance.WandWielder && spell.SourceItemUsableBlueprint != null && spell.SourceItemUsableBlueprint.Type == UsableItemType.Wand;
                __result = validWandSpell || (spell.Spellbook != null && spell.Spellbook == __instance.Spellbook);
                if (TTTContext.Fixes.Magus.Base.IsDisabled("SpellCombatSpellbookRestrictions")) {
                    __result |= __instance.Spellbook != null && spell.IsInSpellList(__instance.Spellbook.Blueprint.SpellList);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(UnitPartMagus), "IsSpellFromMagusSpellList", new Type[] { typeof(AbilityData) })]
        class UnitPartMagus_IsSpellFromMagusSpellList_VarriantAbilities_Patch {
            static void Postfix(UnitPartMagus __instance, ref bool __result, AbilityData spell) {
                if (TTTContext.Fixes.Magus.Base.IsDisabled("SpellCombatAbilityVariants")) { return; }
                if (spell.ConvertedFrom != null) {
                    __result |= __instance.IsSpellFromMagusSpellList(spell.ConvertedFrom);
                }
            }
        }
    }
}