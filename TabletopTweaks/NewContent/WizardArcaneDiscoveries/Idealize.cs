using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.WizardArcaneDiscoveries {
    static class Idealize {
        public static void AddIdealize() {
            var WizardClass = Resources.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");
            var ThassilonianEnchantmentFeature = Resources.GetBlueprintReference<BlueprintFeatureReference>("e1ebc61a71c55054991863a5f6f6d2c2");
            var ThassilonianIllusionFeature = Resources.GetBlueprintReference<BlueprintFeatureReference>("aa271e69902044b47a8e62c4e58a9dcb");

            var IdealizeUpgrade = Helpers.CreateBlueprint<BlueprintFeature>($"IdealizeUpgrade", bp => {
                bp.SetName($"Idealize Upgrade");
                bp.SetDescription("In your quest for self-perfection, you have discovered a way to further enhance yourself and others.\n" +
                    "When a transmutation spell you cast grants an enhancement bonus to an ability score, that bonus increases by 2. " +
                    "At 20th level, the bonus increases by 4.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.IdealizeDiscoveryUpgrade;
                });
            });
            var Idealize = Helpers.CreateBlueprint<BlueprintFeature>($"Idealize", bp => {
                bp.SetName($"Idealize");
                bp.SetDescription("In your quest for self-perfection, you have discovered a way to further enhance yourself and others.\n" +
                    "When a transmutation spell you cast grants an enhancement bonus to an ability score, that bonus increases by 2. " +
                    "At 20th level, the bonus increases by 4.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.IdealizeDiscovery;
                });
                bp.AddComponent<AddFeatureOnClassLevel>(c => {
                    c.m_Feature = IdealizeUpgrade.ToReference<BlueprintFeatureReference>();
                    c.m_Class = WizardClass;
                    c.Level = 20;
                    c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
                    c.m_Archetypes = new BlueprintArchetypeReference[0];
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(p => {
                    p.m_CharacterClass = WizardClass;
                    p.Level = 10;
                    p.Group = Prerequisite.GroupType.All;
                });
                bp.AddPrerequisite<PrerequisiteNoFeature>(p => {
                    p.m_Feature = ThassilonianEnchantmentFeature;
                    p.Group = Prerequisite.GroupType.All;
                });
                bp.AddPrerequisite<PrerequisiteNoFeature>(p => {
                    p.m_Feature = ThassilonianIllusionFeature;
                    p.Group = Prerequisite.GroupType.All;
                });
            });
            if (ModSettings.AddedContent.WizardArcaneDiscoveries.IsDisabled("Idealize")) { return; }
            ArcaneDiscoverySelection.AddToArcaneDiscoverySelection(Idealize);
        }
        [HarmonyPatch(typeof(AddStatBonus), nameof(AddStatBonus.OnTurnOn))]
        static class AddStatBonus_Idealize_Patch {
            static readonly MethodInfo Modifier_AddModifierUnique = AccessTools.Method(typeof(ModifiableValue), "AddModifierUnique", new Type[] {
                typeof(int),
                typeof(EntityFactComponent),
                typeof(ModifierDescriptor)
            });
            static readonly FieldInfo AddStatBonus_Descriptor = AccessTools.Field(typeof(AddStatBonus), nameof(AddStatBonus.Descriptor));
            static readonly FieldInfo AddStatBonus_Stat = AccessTools.Field(typeof(AddStatBonus), nameof(AddStatBonus.Stat));
            static readonly MethodInfo AddStatBonus_Context = AccessTools.PropertyGetter(typeof(AddStatBonus), nameof(AddStatBonus.Context));
            static readonly MethodInfo AddStatBonus_Idealize_Patch_AddIdealizeBonus = AccessTools.Method(
                typeof(AddStatBonus_Idealize_Patch), 
                nameof(AddStatBonus_Idealize_Patch.AddIdealizeBonus)
            );
            //Add Idealize calculations
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (ModSettings.AddedContent.WizardArcaneDiscoveries.IsDisabled("Idealize")) { return instructions; }

                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AddStatBonus_Idealize_Patch_AddIdealizeBonus)
                });
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                int target = 0;
                for (int i = 0; i < codes.Count; i++) {
                    //Find where the modifier is added and grab the load of the value varriable
                    if (codes[i].opcode == OpCodes.Ldloc_0) { target = i + 1; }
                    if (codes[i].Calls(Modifier_AddModifierUnique)) {
                        return target;
                    }
                }
                Main.Log("ADD STAT IDEALIZE PATCH: COULD NOT FIND TARGET");
                return -1;
            }

            static int AddIdealizeBonus(int value, AddStatBonus component) {
                var context = component.Context;
                var owner = context?.MaybeOwner;
                var caster = context?.MaybeCaster;
                var attribute = owner?.Stats?.GetAttribute(component.Stat);

                if (caster == null) { return value; }
                if (owner == null) { return value; }
                if (attribute == null || value < 0) { return value; }
                if (!context.SourceAbility?.IsSpell ?? true
                    || context.SpellSchool != SpellSchool.Transmutation
                    || component.Descriptor != ModifierDescriptor.Enhancement) {
                    return value; 
                }

                if (caster.CustomMechanicsFeature(CustomMechanicsFeature.IdealizeDiscovery)) {
                    value += 2; 
                }
                if (caster.CustomMechanicsFeature(CustomMechanicsFeature.IdealizeDiscoveryUpgrade)) { 
                    value += 2; 
                }
                return value;
            }
        }
    }
}
