using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    static class MetamagicRager {
        private static readonly BlueprintCharacterClass BloodragerClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
        private static readonly BlueprintSpellbook BloodragerSpellbook = BlueprintTools.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
        private static readonly BlueprintAbilityResource BloodragerRageResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("4aec9ec9d9cd5e24a95da90e56c72e37");
        private static readonly BlueprintFeature ImprovedUncannyDodge = BlueprintTools.GetBlueprint<BlueprintFeature>("485a18c05792521459c7d06c63128c79");

        private static readonly BlueprintFeature EmpowerSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
        private static readonly BlueprintFeature ExtendSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
        private static readonly BlueprintFeature HeightenSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("2f5d1e705c7967546b72ad8218ccf99c");
        private static readonly BlueprintFeature MaximizeSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
        private static readonly BlueprintFeature PersistentSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
        private static readonly BlueprintFeature QuickenSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e");
        private static readonly BlueprintFeature ReachSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78");
        private static readonly BlueprintFeature SelectiveSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
        private static readonly BlueprintFeature BolsteredSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");
        private static readonly BlueprintFeature CompletelyNormalSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("094b6278f7b570f42aeaa98379f07cf2");
        private static readonly BlueprintFeature IntensifiedSpell = BlueprintTools.GetBlueprint<BlueprintFeature>("8ad7fd39abea4722b39eb5a67d606a41");
        private static readonly BlueprintFeature PiercingSpell = BlueprintTools.GetBlueprint<BlueprintFeature>("c101ad6879a94204a780506f7a554865");

        private static readonly BlueprintFeature IntensifiedSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "IntensifiedSpellFeat");
        private static readonly BlueprintFeature RimeSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "RimeSpellFeat");
        private static readonly BlueprintFeature BurningSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "BurningSpellFeat");
        private static readonly BlueprintFeature FlaringSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "FlaringSpellFeat");
        private static readonly BlueprintFeature PiercingSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "PiercingSpellFeat");
        private static readonly BlueprintFeature SolidShadowsSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "SolidShadowsSpellFeat");
        private static readonly BlueprintFeature EncouragingSpellFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "EncouragingSpellFeat");
        private static readonly BlueprintFeature ElementalSpellFeatAcid = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatAcid");
        private static readonly BlueprintFeature ElementalSpellFeatCold = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatCold");
        private static readonly BlueprintFeature ElementalSpellFeatElectricity = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatElectricity");
        private static readonly BlueprintFeature ElementalSpellFeatFire = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ElementalSpellFeatFire");

        public static void AddMetamagicRager() {

            var MetaRageFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MetaRageFeature", bp => {
                bp.SetName(TTTContext, "Meta-Rage");
                bp.SetDescription(TTTContext, "At 5th level, a metamagic rager can sacrifice additional rounds of " +
                    "bloodrage to apply a metamagic feat he knows to a bloodrager spell. This costs a number of rounds of bloodrage equal to twice what the spell’s " +
                    "adjusted level would normally be with the metamagic feat applied (minimum 2 rounds). The metamagic rager does not have to be bloodraging " +
                    "to use this ability. The metamagic effect is applied without increasing the level of the spell slot expended, though the casting time is " +
                    "increased as normal. The metamagic rager can apply only one metamagic feat he knows in this manner with each casting.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Icon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_MetaRage.png");
                bp.AddComponent<MetaRageComponent>(c => {
                    c.ConvertSpellbook = BloodragerSpellbook.ToReference<BlueprintSpellbookReference>();
                    c.RequiredResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();
                });
            });
            var MetamagicRagerArchetype = Helpers.CreateBlueprint<BlueprintArchetype>(TTTContext, "MetamagicRagerArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString(TTTContext, "MetamagicRagerArchetype.Name", "Metamagic Rager");
                bp.LocalizedDescription = Helpers.CreateString(TTTContext, "MetamagicRagerArchetype.Description", "While metamagic is difficult for many bloodragers to utilize, " +
                    "a talented few are able to channel their bloodrage in ways that push their spells to impressive ends.");
                bp.AddFeatures = new LevelEntry[] {
                    new LevelEntry() {
                        Level = 5,
                        m_Features = new System.Collections.Generic.List<BlueprintFeatureBaseReference>() {
                            MetaRageFeature.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }
                };
                bp.RemoveFeatures = new LevelEntry[] {
                    new LevelEntry() {
                        Level = 5,
                        m_Features = new System.Collections.Generic.List<BlueprintFeatureBaseReference>() {
                            ImprovedUncannyDodge.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }
                };
            });
            PatchBloodlines(MetamagicRagerArchetype);
            // These abilities are deprecated but are still around for save compatability
            var MetaRageBaseAbility1 = CreateMetaRageLevel(1);
            var MetaRageBaseAbility2 = CreateMetaRageLevel(2);
            var MetaRageBaseAbility3 = CreateMetaRageLevel(3);
            var MetaRageBaseAbility4 = CreateMetaRageLevel(4);
            if (TTTContext.AddedContent.Archetypes.IsDisabled("MetamagicRager")) { return; }
            BloodragerClass.m_Archetypes = BloodragerClass.m_Archetypes.AppendToArray(MetamagicRagerArchetype.ToReference<BlueprintArchetypeReference>());
            TTTContext.Logger.LogPatch("Added", MetamagicRagerArchetype);
        }
        private static BlueprintBuff CreateMetamagicBuff(string name, BlueprintFeature metamagicFeat, int level, Action<BlueprintBuff> init = null) {
            var result = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, name, bp => {
                bp.m_Icon = metamagicFeat.Icon;
                bp.AddComponent<AddAbilityUseTrigger>(c => {
                    c.m_Spellbooks = new BlueprintSpellbookReference[] { BloodragerSpellbook.ToReference<BlueprintSpellbookReference>() };
                    c.m_Ability = new BlueprintAbilityReference();
                    c.Action = new ActionList() {
                        Actions = new GameAction[] {
                            Helpers.Create<ContextActionRemoveSelf>()
                        }
                    };
                    c.AfterCast = true;
                    c.FromSpellbook = true;
                });
                bp.AddComponent<AutoMetamagic>(c => {
                    c.m_Spellbook = BloodragerSpellbook.ToReference<BlueprintSpellbookReference>();
                    c.Metamagic = metamagicFeat.GetComponent<AddMetamagicFeat>().Metamagic;
                    c.School = SpellSchool.None;
                    c.MaxSpellLevel = level;
                    c.CheckSpellbook = true;
                });
            });
            init?.Invoke(result);
            return result;
        }
        private static BlueprintAbility CreateMetamagicAbility(string name, BlueprintBuff buff, int cost, BlueprintFeature metamagicFeat, BlueprintUnitFactReference[] blockedBuffs, Action<BlueprintAbility> init = null) {
            var result = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, name, bp => {
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = CommandType.Free;
                bp.LocalizedDuration = new LocalizedString();
                bp.LocalizedSavingThrow = new LocalizedString();
                bp.m_Icon = metamagicFeat.Icon;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList() {
                        Actions = new GameAction[] {
                            Helpers.Create<ContextActionApplyBuff>(a => {
                                a.m_Buff = buff.ToReference<BlueprintBuffReference>();
                                a.DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue(){
                                        Value = 2
                                    }
                                };
                                a.AsChild = true;
                            })
                        }
                    };

                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = buff.GetComponent<AutoMetamagic>().MaxSpellLevel * 2 + cost;
                });
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = metamagicFeat.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = blockedBuffs;
                });
            });
            init?.Invoke(result);
            return result;
        }
        private static BlueprintAbility CreateMetaRageLevel(int level) {
            var MetaRageEmpowerBuff = CreateMetamagicBuff($"MetaRageEmpowerBuff{level}", EmpowerSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Empower)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 4} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Empowered as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageExtendBuff = CreateMetamagicBuff($"MetaRageExtendBuff{level}", ExtendSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Extend)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Extended as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageMaximizeBuff = CreateMetamagicBuff($"MetaRageMaximizeBuff{level}", MaximizeSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Maximize)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 6} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Maximized as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRagePersistentBuff = CreateMetamagicBuff($"MetaRagePersistentBuff{level}", PersistentSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Persistent)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 4} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Persistent as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageQuickenBuff = CreateMetamagicBuff($"MetaRageQuickenBuff{level}", QuickenSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Quicken)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 8} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Quickened as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageReachBuff = CreateMetamagicBuff($"MetaRageReachBuff{level}", ReachSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Reach)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Reach as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageSelectiveBuff = CreateMetamagicBuff($"MetaRageSelectiveBuff{level}", SelectiveSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Selective)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Selective as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageBolsteredBuff = CreateMetamagicBuff($"MetaRageBolsteredBuff{level}", BolsteredSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Bolstered)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Bolstered as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageIntensifiedBuff = CreateMetamagicBuff($"MetaRageIntensifiedBuff{level}", IntensifiedSpell, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Intensified)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Intensified as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRagePiercingBuff = CreateMetamagicBuff($"MetaRagePiercingBuff{level}", PiercingSpell, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Piercing)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Piercing as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });

            var MetaRageIntensifiedTTTBuff = CreateMetamagicBuff($"MetaRageIntensifiedTTTBuff{level}", IntensifiedSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Intensified)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Intensified as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageRimeTTTBuff = CreateMetamagicBuff($"MetaRageRimeTTTBuff{level}", RimeSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Rime)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Rime as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageBurningTTTBuff = CreateMetamagicBuff($"MetaRageBurningTTTBuff{level}", BurningSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Burning)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 4} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Burning as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageFlaringTTTBuff = CreateMetamagicBuff($"MetaRageFlaringTTTBuff{level}", FlaringSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Flaring)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Flaring as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRagePiercingTTTBuff = CreateMetamagicBuff($"MetaRagePiercingTTTBuff{level}", PiercingSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Piercing)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Piercing as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageSolidShadowsTTTBuff = CreateMetamagicBuff($"MetaRageSolidShadowsTTTBuff{level}", SolidShadowsSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Solid Shadows)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Solid Shadows as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageEncouragingTTTBuff = CreateMetamagicBuff($"MetaRageEncouragingTTTBuff{level}", EncouragingSpellFeat, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Encouraging)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Encouraging as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageElementalSpellFireTTTBuff = CreateMetamagicBuff($"MetaRageElementalSpellFireTTTBuff{level}", ElementalSpellFeatFire, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Fire)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Elemental — Fire as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageElementalSpellColdTTTBuff = CreateMetamagicBuff($"MetaRageElementalSpellColdTTTBuff{level}", ElementalSpellFeatCold, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Cold)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Elemental — Cold as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageElementalSpellElectricityTTTBuff = CreateMetamagicBuff($"MetaRageElementalSpellElectricityTTTBuff{level}", ElementalSpellFeatElectricity, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Electricity)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Elemental — Electricity as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });
            var MetaRageElementalSpellAcidTTTBuff = CreateMetamagicBuff($"MetaRageElementalSpellAcidTTTBuff{level}", ElementalSpellFeatAcid, level, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Acid)");
                bp.SetDescription(TTTContext, $"The metamagic rager can spend {level * 2 + 2} rounds of bloodrage as a " +
                    "{g|Encyclopedia:Free_Action}free action{/g} to make next bloodrager {g|Encyclopedia:Spell}spell{/g} " + $"of level {level} or lower " +
                    "he casts in 2 {g|Encyclopedia:Combat_Round}rounds{/g} Elemental — Acid as per using the corresponding metamagic {g|Encyclopedia:Feat}feat{/g}.");
            });

            var MetaRageBuffs = new BlueprintUnitFactReference[] {
                MetaRageEmpowerBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageExtendBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageMaximizeBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRagePersistentBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageQuickenBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageReachBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageSelectiveBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageBolsteredBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageIntensifiedBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRagePiercingBuff.ToReference<BlueprintUnitFactReference>(),

                MetaRageIntensifiedTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageRimeTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageBurningTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageFlaringTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRagePiercingTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageSolidShadowsTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageEncouragingTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageElementalSpellFireTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageElementalSpellColdTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageElementalSpellElectricityTTTBuff.ToReference<BlueprintUnitFactReference>(),
                MetaRageElementalSpellAcidTTTBuff.ToReference<BlueprintUnitFactReference>()
            };

            var MetaRageEmpowerAbility = CreateMetamagicAbility($"MetaRageEmpowerAbility{level}", MetaRageEmpowerBuff, 4, EmpowerSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Empower)");
                bp.SetDescription(MetaRageEmpowerBuff.m_Description);
            });
            var MetaRageExtendAbility = CreateMetamagicAbility($"MetaRageExtendAbility{level}", MetaRageExtendBuff, 2, ExtendSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Extend)");
                bp.SetDescription(MetaRageExtendBuff.m_Description);
            });
            var MetaRageMaximizeAbility = CreateMetamagicAbility($"MetaRageMaximizeAbility{level}", MetaRageMaximizeBuff, 6, MaximizeSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Maximize)");
                bp.SetDescription(MetaRageMaximizeBuff.m_Description);
            });
            var MetaRagePersistentAbility = CreateMetamagicAbility($"MetaRagePersistentAbility{level}", MetaRagePersistentBuff, 4, PersistentSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Persistent)");
                bp.SetDescription(MetaRagePersistentBuff.m_Description);
            });
            var MetaRageQuickenAbility = CreateMetamagicAbility($"MetaRageQuickenAbility{level}", MetaRageQuickenBuff, 8, QuickenSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Quicken)");
                bp.SetDescription(MetaRageQuickenBuff.m_Description);
            });
            var MetaRageReachAbility = CreateMetamagicAbility($"MetaRageReachAbility{level}", MetaRageReachBuff, 2, ReachSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Reach)");
                bp.SetDescription(MetaRageReachBuff.m_Description);
            });
            var MetaRageSelectiveAbility = CreateMetamagicAbility($"MetaRageSelectiveAbility{level}", MetaRageSelectiveBuff, 2, SelectiveSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Selective)");
                bp.SetDescription(MetaRageSelectiveBuff.m_Description);
            });
            var MetaRageBolsteredAbility = CreateMetamagicAbility($"MetaRageBolsteredAbility{level}", MetaRageBolsteredBuff, 2, BolsteredSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Bolstered)");
                bp.SetDescription(MetaRageBolsteredBuff.m_Description);
            });
            var MetaRageIntensifiedAbility = CreateMetamagicAbility($"MetaRageIntensifiedAbility{level}", MetaRageIntensifiedBuff, 2, IntensifiedSpell, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Intensified)");
                bp.SetDescription(MetaRageIntensifiedBuff.m_Description);
            });
            var MetaRagePiercingAbility = CreateMetamagicAbility($"MetaRageBolsteredAbility{level}", MetaRagePiercingBuff, 2, PiercingSpell, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Piercing)");
                bp.SetDescription(MetaRagePiercingBuff.m_Description);
            });

            var MetaRageIntensifiedTTTAbility = CreateMetamagicAbility($"MetaRageIntensifiedTTTAbility{level}", MetaRageIntensifiedTTTBuff, 2, IntensifiedSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Intensified)");
                bp.SetDescription(MetaRageIntensifiedTTTBuff.m_Description);
            });
            var MetaRageRimeTTTAbility = CreateMetamagicAbility($"MetaRageRimeTTTAbility{level}", MetaRageRimeTTTBuff, 2, RimeSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Rime)");
                bp.SetDescription(MetaRageRimeTTTBuff.m_Description);
            });
            var MetaRageBurningTTTAbility = CreateMetamagicAbility($"MetaRageBurningTTTAbility{level}", MetaRageBurningTTTBuff, 4, BurningSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Burning)");
                bp.SetDescription(MetaRageBurningTTTBuff.m_Description);
            });
            var MetaRageFlaringTTTAbility = CreateMetamagicAbility($"MetaRageFlaringTTTAbility{level}", MetaRageFlaringTTTBuff, 2, FlaringSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Flaring)");
                bp.SetDescription(MetaRageFlaringTTTBuff.m_Description);
            });
            var MetaRagePiercingTTTAbility = CreateMetamagicAbility($"MetaRagePiercingTTTAbility{level}", MetaRagePiercingTTTBuff, 2, PiercingSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Piercing)");
                bp.SetDescription(MetaRagePiercingTTTBuff.m_Description);
            });
            var MetaRageSolidShadowsTTTAbility = CreateMetamagicAbility($"MetaRageSolidShadowsTTTAbility{level}", MetaRageSolidShadowsTTTBuff, 2, SolidShadowsSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Solid Shadows)");
                bp.SetDescription(MetaRageSolidShadowsTTTBuff.m_Description);
            });
            var MetaRageEncouragingTTTAbility = CreateMetamagicAbility($"MetaRageEncouragingTTTAbility{level}", MetaRageEncouragingTTTBuff, 2, EncouragingSpellFeat, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Encouraging)");
                bp.SetDescription(MetaRageEncouragingTTTBuff.m_Description);
            });
            var MetaRageElementalSpellFireTTTAbility = CreateMetamagicAbility($"MetaRageElementalSpellFireTTTAbility{level}", MetaRageElementalSpellFireTTTBuff, 2, ElementalSpellFeatFire, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Fire)");
                bp.SetDescription(MetaRageElementalSpellFireTTTBuff.m_Description);
            });
            var MetaRageElementalSpellColdTTTAbility = CreateMetamagicAbility($"MetaRageElementalSpellColdTTTAbility{level}", MetaRageElementalSpellColdTTTBuff, 2, ElementalSpellFeatCold, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Cold)");
                bp.SetDescription(MetaRageElementalSpellColdTTTBuff.m_Description);
            });
            var MetaRageElementalSpellElectricityTTTAbility = CreateMetamagicAbility($"MetaRageElementalSpellElectricityTTTAbility{level}", MetaRageElementalSpellElectricityTTTBuff, 2, ElementalSpellFeatElectricity, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Electricity)");
                bp.SetDescription(MetaRageElementalSpellElectricityTTTBuff.m_Description);
            });
            var MetaRageElementalSpellAcidTTT = CreateMetamagicAbility($"MetaRageElementalSpellAcidTTT{level}", MetaRageElementalSpellAcidTTTBuff, 2, ElementalSpellFeatAcid, MetaRageBuffs, bp => {
                bp.SetName(TTTContext, "Meta-Rage (Elemental — Acid)");
                bp.SetDescription(MetaRageElementalSpellAcidTTTBuff.m_Description);
            });

            var MetaRageBaseAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, $"MetaRageBaseAbility{level}", bp => {
                bp.SetName(TTTContext, "Meta-Rage");
                bp.SetDescription(TTTContext, "At 5th level, a metamagic rager can sacrifice additional rounds of " +
                    "bloodrage to apply a metamagic feat he knows to a bloodrager spell. This costs a number of rounds of bloodrage equal to twice what the spell’s " +
                    "adjusted level would normally be with the metamagic feat applied (minimum 2 rounds). The metamagic rager does not have to be bloodraging " +
                    "to use this ability. The metamagic effect is applied without increasing the level of the spell slot expended, though the casting time is " +
                    "increased as normal. The metamagic rager can apply only one metamagic feat he knows in this manner with each casting.");
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.LocalizedDuration = new LocalizedString();
                bp.LocalizedSavingThrow = new LocalizedString();
                bp.m_Icon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: $"Icon_MetaRage{level}.png");
                bp.AddComponent<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        MetaRageEmpowerAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageExtendAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageMaximizeAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRagePersistentAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageQuickenAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageReachAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageSelectiveAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageBolsteredAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageIntensifiedAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRagePiercingAbility.ToReference<BlueprintAbilityReference>(),

                        MetaRageIntensifiedTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageRimeTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageBurningTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageFlaringTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRagePiercingTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageSolidShadowsTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageEncouragingTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageElementalSpellFireTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageElementalSpellColdTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageElementalSpellElectricityTTTAbility.ToReference<BlueprintAbilityReference>(),
                        MetaRageElementalSpellAcidTTT.ToReference<BlueprintAbilityReference>()
                    };
                });
                bp.AddComponent<AbilityShowIfCasterCanCastSpells>(c => {
                    c.Class = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = level;
                });
            });

            return MetaRageBaseAbility;
        }
        private static void PatchBloodlines(BlueprintArchetype archetype) {
            var basicBloodlines = new BlueprintProgression[] {
                BlueprintTools.GetModBlueprint<BlueprintProgression>(TTTContext, "BloodragerAberrantBloodline"),
                BloodlineTools.Bloodline.BloodragerAbyssalBloodline,
                BloodlineTools.Bloodline.BloodragerArcaneBloodline,
                BloodlineTools.Bloodline.BloodragerCelestialBloodline,
                BlueprintTools.GetModBlueprint<BlueprintProgression>(TTTContext, "BloodragerDestinedBloodline"),
                BloodlineTools.Bloodline.BloodragerFeyBloodline,
                BloodlineTools.Bloodline.BloodragerInfernalBloodline,
                BloodlineTools.Bloodline.BloodragerSerpentineBloodline,
                BloodlineTools.Bloodline.BloodragerUndeadBloodline,
                //Second Bloodlines
                BlueprintTools.GetModBlueprint<BlueprintProgression>(TTTContext, "BloodragerAberrantSecondBloodline"),
                BloodlineTools.Bloodline.BloodragerAbyssalSecondBloodline,
                BloodlineTools.Bloodline.BloodragerArcaneSecondBloodline,
                BloodlineTools.Bloodline.BloodragerCelestialSecondBloodline,
                BlueprintTools.GetModBlueprint<BlueprintProgression>(TTTContext, "BloodragerDestinedSecondBloodline"),
                BloodlineTools.Bloodline.BloodragerFeySecondBloodline,
                BloodlineTools.Bloodline.BloodragerInfernalSecondBloodline,
                BloodlineTools.Bloodline.BloodragerSerpentineSecondBloodline,
                BloodlineTools.Bloodline.BloodragerUndeadSecondBloodline,
            };
            var draconicBloodlines = new BlueprintProgression[] {
                BloodlineTools.Bloodline.BloodragerDragonBlackBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBlueBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBrassBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBronzeBloodline,
                BloodlineTools.Bloodline.BloodragerDragonCopperBloodline,
                BloodlineTools.Bloodline.BloodragerDragonGoldBloodline,
                BloodlineTools.Bloodline.BloodragerDragonGreenBloodline,
                BloodlineTools.Bloodline.BloodragerDragonRedBloodline,
                BloodlineTools.Bloodline.BloodragerDragonSilverBloodline,
                BloodlineTools.Bloodline.BloodragerDragonWhiteBloodline,
                //Second Bloodlines
                BloodlineTools.Bloodline.BloodragerDragonBlackSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBlueSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBrassSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonBronzeSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonCopperSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonGoldSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonGreenSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonRedSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonSilverSecondBloodline,
                BloodlineTools.Bloodline.BloodragerDragonWhiteSecondBloodline,
            };
            var elementalBloodlines = new BlueprintProgression[] {
                BloodlineTools.Bloodline.BloodragerElementalAcidBloodline,
                BloodlineTools.Bloodline.BloodragerElementalColdBloodline,
                BloodlineTools.Bloodline.BloodragerElementalElectricityBloodline,
                BloodlineTools.Bloodline.BloodragerElementalFireBloodline,
                //Second Bloodlines
                BloodlineTools.Bloodline.BloodragerElementalAcidSecondBloodline,
                BloodlineTools.Bloodline.BloodragerElementalColdSecondBloodline,
                BloodlineTools.Bloodline.BloodragerElementalElectricitySecondBloodline,
                BloodlineTools.Bloodline.BloodragerElementalFireSecondBloodline
            };
            int[] featLevels = { 6, 9, 12, 15, 18 };
            var metamagicFeats = FeatTools.GetMetamagicFeats();
            foreach (var bloodline in basicBloodlines) {
                BlueprintFeatureSelection MetamagicRagerFeatSelection = null;
                foreach (var levelEntry in bloodline.LevelEntries.Where(entry => featLevels.Contains(entry.Level))) {
                    foreach (var selection in levelEntry.Features.OfType<BlueprintFeatureSelection>()) {
                        if (MetamagicRagerFeatSelection == null) {
                            MetamagicRagerFeatSelection = selection.CreateCopy(TTTContext, GenerateName(bloodline), bp => {
                                bp.HideNotAvailibleInUI = true;
                                bp.AddFeatures(metamagicFeats);
                                bp.AddComponent<PrerequisiteArchetypeLevel>(c => {
                                    c.HideInUI = true;
                                    c.CheckInProgression = true;
                                    c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                                    c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                                });
                                bp.RemoveComponents<PrerequisiteNoArchetype>(c => {
                                    return c.HideInUI == true
                                        && c.m_CharacterClass.Guid == BloodragerClass.AssetGuid
                                        && c.m_Archetype.Guid == archetype.AssetGuid
                                        && c.CheckInProgression == true;
                                });
                            });
                        }
                        if (selection.GetComponents<PrerequisiteNoArchetype>().Any(c => c.m_Archetype.Get().AssetGuid == archetype.AssetGuid)) { continue; }
                        selection.AddComponent<PrerequisiteNoArchetype>(c => {
                            c.HideInUI = true;
                            c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                            c.CheckInProgression = true;
                        });
                    }
                    levelEntry.m_Features.Add(MetamagicRagerFeatSelection.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            BlueprintFeatureSelection DraconicMetamagicRagerFeatSelection = null;
            foreach (var bloodline in draconicBloodlines) {
                foreach (var levelEntry in bloodline.LevelEntries.Where(entry => featLevels.Contains(entry.Level))) {
                    foreach (var selection in levelEntry.Features.OfType<BlueprintFeatureSelection>()) {
                        if (selection.GetComponents<PrerequisiteNoArchetype>().Any(c => c.m_Archetype.Get().AssetGuid == archetype.AssetGuid)) { continue; }
                        if (DraconicMetamagicRagerFeatSelection == null) {
                            DraconicMetamagicRagerFeatSelection = selection.CreateCopy(TTTContext, GenerateName(bloodline), bp => {
                                bp.AddFeatures(metamagicFeats);
                                bp.AddComponent<PrerequisiteArchetypeLevel>(c => {
                                    c.HideInUI = true;
                                    c.CheckInProgression = true;
                                    c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                                    c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                                });
                            });
                        }
                        selection.AddComponent<PrerequisiteNoArchetype>(c => {
                            c.HideInUI = true;
                            c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                            c.CheckInProgression = true;
                        });
                    }
                    levelEntry.m_Features.Add(DraconicMetamagicRagerFeatSelection.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            BlueprintFeatureSelection ElementalMetamagicRagerFeatSelection = null;
            foreach (var bloodline in elementalBloodlines) {
                foreach (var levelEntry in bloodline.LevelEntries.Where(entry => featLevels.Contains(entry.Level))) {
                    foreach (var selection in levelEntry.Features.OfType<BlueprintFeatureSelection>()) {
                        if (selection.GetComponents<PrerequisiteNoArchetype>().Any(c => c.m_Archetype.Get().AssetGuid == archetype.AssetGuid)) { continue; }
                        if (ElementalMetamagicRagerFeatSelection == null) {
                            ElementalMetamagicRagerFeatSelection = selection.CreateCopy(TTTContext, GenerateName(bloodline), bp => {
                                bp.AddFeatures(metamagicFeats);
                                bp.AddComponent<PrerequisiteArchetypeLevel>(c => {
                                    c.HideInUI = true;
                                    c.CheckInProgression = true;
                                    c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                                    c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                                });
                            });
                        }
                        selection.AddComponent<PrerequisiteNoArchetype>(c => {
                            c.HideInUI = true;
                            c.m_CharacterClass = BloodragerClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                            c.CheckInProgression = true;
                        });
                    }
                    levelEntry.m_Features.Add(ElementalMetamagicRagerFeatSelection.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            string GenerateName(BlueprintFeature bloodline) {
                string[] split = Regex.Split(bloodline.name, @"(?<!^)(?=[A-Z])");
                string second = Regex.IsMatch(bloodline.name, "Second") ? "Second" : "";
                return $"{split[0]}{second}{split[1]}MetamagicRagerFeatSelection";
            }
        }
    }
}
