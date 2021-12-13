using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewEvents;

namespace TabletopTweaks.NewComponents {
    [TypeId("3d768a2d4890495c8a76167a1c411c60")]
    class AbilityRestoreArcanistSpell : AbilityApplyEffect, IAbilityRestriction, IAbilityRequiredParameters {
        public AbilityParameter RequiredParameters {
            get {
                return AbilityParameter.Spellbook | AbilityParameter.SpellLevel | AbilityParameter.SpellSlot;
            }
        }
        // NOT IMPLEMENTED YET
        public override void Apply(AbilityExecutionContext context, TargetWrapper target) {
            if (context.Ability.ParamSpellbook == null) {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spellbook is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (context.Ability.ParamSpellLevel == null) {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spell level is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (context.Ability.ParamSpellLevel.Value > this.SpellLevel && !this.AnySpellLevel) {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Invalid target spell level {0}: {1}", context.Ability.ParamSpellLevel.Value, context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            context.Ability.ParamSpellbook.RestoreSpontaneousSlots(context.Ability.ParamSpellLevel.Value, 1);
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability) {
            if (ability.ParamSpellbook == null || ability.ParamSpellLevel == null || (ability.ParamSpellLevel.Value > this.SpellLevel && !this.AnySpellLevel)) {
                return false;
            }
            int spontaneousSlots = ability.ParamSpellbook.GetSpontaneousSlots(ability.ParamSpellLevel.Value);
            int spellsPerDay = ability.ParamSpellbook.GetSpellsPerDay(ability.ParamSpellLevel.Value);
            return spontaneousSlots < spellsPerDay;
        }

        public string GetAbilityRestrictionUIText() {
            return "";
        }

        public bool AnySpellLevel;

        [HideIf("AnySpellLevel")]
        public int SpellLevel;
    }
}
