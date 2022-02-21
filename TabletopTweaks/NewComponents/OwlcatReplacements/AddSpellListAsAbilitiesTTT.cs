using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using UnityEngine;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    public class AddSpellListAsAbilitiesTTT : UnitFactComponentDelegate<AddSpellListAsAbilitiesData> {
        private ReferenceArrayProxy<BlueprintAbilityResource, BlueprintAbilityResourceReference> ResourcePerSpellLevel {
            get {
                return this.m_ResourcePerSpellLevel;
            }
        }

        private BlueprintSpellList SpellList {
            get {
                if (m_SpellList == null) {
                    return null;
                }
                return m_SpellList.Get();
            }
        }
        public override void OnActivate() {
            if (SpellList == null) {
                return;
            }
            UnitEntityData owner = base.Owner;
            foreach (SpellLevelList spellLevelList in SpellList.SpellsByLevel) {
                if (owner.Progression.MythicLevel >= spellLevelList.SpellLevel) {
                    foreach (BlueprintAbility spell in spellLevelList.Spells) {
                        if (!base.Data.Abilities.Any((string id) => owner.Facts.FindById(id)?.Blueprint == spell)) {
                            Ability ability = owner.AddFact<Ability>(spell, null, null);
                            if (ability != null) {
                                BlueprintAbilityResource blueprintAbilityResource = ability.UsagesPerDayResource = this.ResourcePerSpellLevel.Get(spellLevelList.SpellLevel - 1);
                                base.Data.Abilities.Add(ability.UniqueId);
                                if (!base.Data.Resources.Contains(blueprintAbilityResource)) {
                                    owner.Resources.Add(blueprintAbilityResource, true);
                                    base.Data.Resources.Add(blueprintAbilityResource);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void OnDeactivate() {
            if (base.IsReapplying) {
                return;
            }
            EntityFactsManager facts = base.Owner.Facts;
            foreach (string factId in base.Data.Abilities) {
                facts.Remove(facts.FindById(factId), true);
            }
            foreach (BlueprintAbilityResource blueprint in base.Data.Resources) {
                base.Owner.Resources.Remove(blueprint);
            }
            base.Data.Abilities.Clear();
            base.Data.Resources.Clear();
        }

        [SerializeField]
        [ValidateNotNull]
        public BlueprintSpellListReference m_SpellList;

        [SerializeField]
        public BlueprintAbilityResourceReference[] m_ResourcePerSpellLevel;
    }
}
