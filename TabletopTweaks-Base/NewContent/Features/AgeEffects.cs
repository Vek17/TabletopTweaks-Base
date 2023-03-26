using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    internal class AgeEffects {
        public static void AddAgeEffects() {
            var DLC3_HasteIslandAge1 = BlueprintTools.GetBlueprint<BlueprintBuff>("0aca74909b144441a31866b977572f91");
            var DLC3_HasteIslandAge2 = BlueprintTools.GetBlueprint<BlueprintBuff>("b26c4f0f18ed41db9f78cfda0c7b874b");
            var DLC3_HasteIslandAge3 = BlueprintTools.GetBlueprint<BlueprintBuff>("9cab5a802dfd4e3e86a0623046bf88aa");

            var ConstructType = BlueprintTools.GetBlueprint<BlueprintFeature>("fd389783027d63343b4a5634bd81645f");
            var UndeadType = BlueprintTools.GetBlueprint<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33");
            var SubtypeElemental = BlueprintTools.GetBlueprint<BlueprintFeature>("198fd8924dabcb5478d0f78bd453c586");

            var MiddleAgeNegatePhysicalFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MiddleAgeNegatePhysicalFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Middle Aged Negate Physical");
                bp.SetDescription(TTTContext, "");
            });
            var MiddleAgeNegateMentalFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MiddleAgeNegateMentalFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Middle Aged Negate Mental");
                bp.SetDescription(TTTContext, "");
            });
            var OldAgeNegatePhysicalFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldAgeNegatePhysicalFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Old Aged Negate Physical");
                bp.SetDescription(TTTContext, "");
            });
            var OldAgeNegateMentalFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldAgeNegateMentalFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Old Aged Aged Negate Mental");
                bp.SetDescription(TTTContext, "");
            });
            var VeneratedAgeNegatePhysicalFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "VeneratedAgeNegatePhysicalFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Venerated Negate Physical");
                bp.SetDescription(TTTContext, "");
            });
            var VeneratedAgeNegateMentalFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "VeneratedAgeNegateMentalFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Venerated Negate Mental");
                bp.SetDescription(TTTContext, "");
            });

            var MiddleAgeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MiddleAgeFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(DLC3_HasteIslandAge1.m_DisplayName);
                bp.SetDescription(DLC3_HasteIslandAge1.m_Description);
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Strength;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Constitution;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Charisma;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
            });
            var OldAgeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "OldAgeFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(DLC3_HasteIslandAge2.m_DisplayName);
                bp.SetDescription(DLC3_HasteIslandAge2.m_Description);
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Strength;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -2;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OldAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -2;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OldAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Constitution;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -2;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OldAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OldAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OldAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Charisma;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OldAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
            });
            var VeneratedAgeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "VeneratedAgeFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(DLC3_HasteIslandAge3.m_DisplayName);
                bp.SetDescription(DLC3_HasteIslandAge3.m_Description);
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Strength;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -3;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        VeneratedAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -3;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        VeneratedAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Constitution;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -3;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        VeneratedAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        VeneratedAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        VeneratedAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.Charisma;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 1;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        VeneratedAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                    c.InvertCondition = true;
                });
            });

            var AgelessFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AgelessFeature", bp => {
                bp.SetName(TTTContext, "Ageless");
                bp.SetDescription(TTTContext, "Anything ageless is immune to the effects of aging.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MiddleAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>(),
                        MiddleAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>(),
                        OldAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>(),
                        OldAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>(),
                        VeneratedAgeNegatePhysicalFeature.ToReference<BlueprintUnitFactReference>(),
                        VeneratedAgeNegateMentalFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            DLC3_HasteIslandAge1.TemporaryContext(bp => {
                bp.SetDescription(TTTContext, "You gain a +1 bonus to Intelligence, Wisdom, and Charisma and suffer a -1 penalty to Strength, Dexterity, and Constitution.");
                bp.SetComponents();
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MiddleAgeFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            DLC3_HasteIslandAge2.TemporaryContext(bp => {
                bp.SetDescription(TTTContext, "You gain a +2 bonus to Intelligence, Wisdom, and Charisma and suffer a -3 penalty to Strength, Dexterity, and Constitution.");
                bp.SetComponents();
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new ContextActionRemoveBuff() {
                            m_Buff = DLC3_HasteIslandAge1.ToReference<BlueprintBuffReference>()
                        }
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MiddleAgeFeature.ToReference<BlueprintUnitFactReference>(),
                        OldAgeFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            DLC3_HasteIslandAge3.TemporaryContext(bp => {
                bp.SetDescription(TTTContext, "You gain a +3 bonus to Intelligence, Wisdom, and Charisma and suffer a -6 penalty to Strength, Dexterity, and Constitution.");
                bp.SetComponents();
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new ContextActionRemoveBuff() {
                            m_Buff = DLC3_HasteIslandAge2.ToReference<BlueprintBuffReference>()
                        }
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MiddleAgeFeature.ToReference<BlueprintUnitFactReference>(),
                        OldAgeFeature.ToReference<BlueprintUnitFactReference>(),
                        VeneratedAgeFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            MakeAgeless(ConstructType);
            MakeAgeless(SubtypeElemental);
            MakeAgeless(UndeadType);

            void MakeAgeless(BlueprintFeature feature) {
                feature.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        AgelessFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            }
        }
    }
}
