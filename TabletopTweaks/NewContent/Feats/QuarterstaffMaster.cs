using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.UI.UnitSettings.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;
using System;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Cheats;
using Kingmaker.Controllers.MapObjects;
using Kingmaker.Controllers.Rest;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Globalmap;
using Kingmaker.Items;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Tasks;
using Kingmaker.Kingdom.UI;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.Tutorial;
using Kingmaker.UI.FullScreenUITypes;
using Kingmaker.UI.Group;
using Kingmaker.UI.IngameMenu;
using Kingmaker.UI.Kingdom;
using Kingmaker.UI.MainMenuUI;
using Kingmaker.UI.MVVM._VM.MainMenu;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Class.Kineticist.ActivatableAbility;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.View.MapObjects;
using ModKit;
using Owlcat.Runtime.Core.Utils;
using UnityEngine;


namespace TabletopTweaks.NewContent.Feats {
    class QuarterstaffMaster {
        public static void AddQuarterstaffMaster() {
            if (ModSettings.AddedContent.Feats.IsDisabled("QuarterstaffMasterFeat")) ;
            BlueprintParametrizedFeature WeaponFocus = Resources.GetBlueprint<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");

            var QuarterstaffMasterFeat = Helpers.CreateBlueprint<BlueprintFeature>("QuarterstaffMasterFeat", bp => {
                bp.SetName("Quarterstaff Master");
                bp.SetDescription("By employing a number of different stances and techniques, you can wield a quarterstaff as a one-handed weapon. At the start of your turn, you decide whether or not you are going to wield the quarterstaff as a one-handed or two-handed weapon. When you wield it as a one-handed weapon, your other hand is free, and you cannot use the staff as a double weapon.");
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.AddComponent<PrerequisiteParametrizedFeature>(c => {
                    c.m_Feature = WeaponFocus.ToReference<BlueprintFeatureReference>();
                    c.ParameterType = FeatureParameterType.WeaponCategory;
                    c.WeaponCategory = WeaponCategory.Quarterstaff;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
                });
            bp.AddComponent<PrerequisiteStatValue>(c => { 
                c.Stat = StatType.BaseAttackBonus;
                c.Value = 5;
            });
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
            });
            FeatTools.AddAsFeat(QuarterstaffMasterFeat);
        }
    }

    //Allow qstaff 1h
    [HarmonyPatch(typeof(ItemEntityWeapon), "HoldInTwoHands", MethodType.Getter)]
    static class QuarterstaffOneHanded {
        static void Postfix(ItemEntityWeapon __instance, ref bool __result) {
            if (ModSettings.AddedContent.BaseAbilities.IsDisabled("OneHandedToggle")) { return; }
            var quarterstaff = Resources.GetBlueprint<BlueprintWeaponType>("629736dabac7f9f4a819dc854eaed2d6");
            var qstaffmaster = Resources.GetModBlueprint<BlueprintFeature>("QuarterstaffMasterFeat");
            if (__instance.Wielder != null && __instance.Wielder.CustomMechanicsFeature(CustomMechanicsFeature.UseWeaponOneHanded)) {
                if (__instance.Wielder.HasFact(qstaffmaster) && __instance.Blueprint.Type == quarterstaff) {
                    __result = false;
                }
            }
        }
    }
    //Allow spellcombat when wielding qstaff 1h
    [HarmonyPatch(typeof(UnitPartMagus), "IsSpellCombatThisRoundAllowed")]
    static class QuarterstaffSpellCombat {
        static void Postfix(ref bool __result, UnitPart __instance) {
            if (ModSettings.AddedContent.BaseAbilities.IsDisabled("OneHandedToggle")) { return; }
            if (ModSettings.AddedContent.Feats.IsDisabled("QuarterstaffMasterFeat")) { return; }
            var quarterstaff = Resources.GetBlueprint<BlueprintWeaponType>("629736dabac7f9f4a819dc854eaed2d6");
            var qstaffmaster = Resources.GetModBlueprint<BlueprintFeature>("QuarterstaffMasterFeat");
            if (__instance.Owner != null && __instance.Owner.CustomMechanicsFeature(CustomMechanicsFeature.UseWeaponOneHanded)) {
                if (__instance.Owner.HasFact(qstaffmaster) && __instance.Owner.Body.CurrentHandsEquipmentSet.PrimaryHand.Weapon.Blueprint.Type == quarterstaff) {
                    __result = true;
                }
            }
        }
    }
}
