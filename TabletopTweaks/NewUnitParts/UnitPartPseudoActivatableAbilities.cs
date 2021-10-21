using Kingmaker.Blueprints;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUI;

namespace TabletopTweaks.NewUnitParts {
    public class UnitPartPseudoActivatableAbilities : UnitPart {

        private static BlueprintBuffReference _nullBuffRef = BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);

        private Dictionary<BlueprintAbility, List<WeakReference<MechanicActionBarSlot>>> m_AbilitiesToMechanicSlots =
            new Dictionary<BlueprintAbility, List<WeakReference<MechanicActionBarSlot>>>();

        private Dictionary<BlueprintAbility, HashSet<BlueprintBuffReference>> m_AbilitiesToBuffs =
            new Dictionary<BlueprintAbility, HashSet<BlueprintBuffReference>>();

        private Dictionary<BlueprintBuffReference, HashSet<BlueprintAbility>> m_BuffsToAbilities =
            new Dictionary<BlueprintBuffReference, HashSet<BlueprintAbility>>();

        private HashSet<BlueprintBuffReference> m_ActiveWatchedBuffs = new HashSet<BlueprintBuffReference>();


        public void RegisterPseudoActivatableAbilitySlot(MechanicActionBarSlot mechanicSlot) {
            if (!(mechanicSlot is IPseudoActivatableMechanicsBarSlot abilitySlot))
                return;

            var abilityBlueprint = abilitySlot.PseudoActivatableAbility.Blueprint;
            if (m_AbilitiesToMechanicSlots.TryGetValue(abilityBlueprint, out var slotRefs)) {
                slotRefs.Add(new WeakReference<MechanicActionBarSlot>(mechanicSlot));
            }
            else {
                m_AbilitiesToMechanicSlots.Add(abilityBlueprint, new List<WeakReference<MechanicActionBarSlot>>() { new WeakReference<MechanicActionBarSlot>(mechanicSlot) });
            }

            if (m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                UpdateStateForAbility(abilityBlueprint);
                return;
            }

            m_AbilitiesToBuffs.Add(abilityBlueprint, new HashSet<BlueprintBuffReference>());
            if (!abilitySlot.BuffToWatch.Equals(_nullBuffRef)) {
                m_AbilitiesToBuffs[abilityBlueprint].Add(abilitySlot.BuffToWatch);
                if (m_BuffsToAbilities.TryGetValue(abilitySlot.BuffToWatch, out var abilities)) {
                    abilities.Add(abilityBlueprint);
                } else {
                    m_BuffsToAbilities.Add(abilitySlot.BuffToWatch, new HashSet<BlueprintAbility> { abilityBlueprint });
                }
            } else {
                var abilityVariants = abilityBlueprint.GetComponent<AbilityVariants>();
                if (abilityVariants != null) {
                    HashSet<BlueprintBuffReference> variantBlueprintBuffsToWatch = new HashSet<BlueprintBuffReference>();
                    foreach (var variant in abilityVariants.Variants) {
                        var pseudoActivatableComponent = variant.GetComponent<PseudoActivatable>();
                        if (pseudoActivatableComponent != null && !pseudoActivatableComponent.BuffToWatch.Equals(_nullBuffRef)) {
                            variantBlueprintBuffsToWatch.Add(pseudoActivatableComponent.BuffToWatch);
                        }
                    }
                    if (variantBlueprintBuffsToWatch.Any()) {
                        foreach(var buffRef in variantBlueprintBuffsToWatch) {
                            m_AbilitiesToBuffs[abilityBlueprint].Add(buffRef);
                            if (m_BuffsToAbilities.TryGetValue(buffRef, out var abilities)) {
                                abilities.Add(abilityBlueprint);
                            } else {
                                m_BuffsToAbilities.Add(buffRef, new HashSet<BlueprintAbility> { abilityBlueprint });
                            }
                        }
                    }
                }
            }
#if DEBUG
            Validate();
#endif
            UpdateStateForAbility(abilityBlueprint);
        }

        public void RegisterWatchedBuff(BlueprintBuff buff) {
            var buffRef = buff.ToReference<BlueprintBuffReference>();
            if (m_BuffsToAbilities.TryGetValue(buffRef, out var abilities)) {
                foreach(var abilityBlueprint in abilities) {
                    if (m_AbilitiesToBuffs.TryGetValue(abilityBlueprint, out var buffsForAbility)) {
                        buffsForAbility.Add(buffRef);
                    } else {
                        m_AbilitiesToBuffs.Add(abilityBlueprint, new HashSet<BlueprintBuffReference> { buffRef });
                    }
                }
            } else {
                m_BuffsToAbilities.Add(buffRef, new HashSet<BlueprintAbility>());
            }

            if (this.Owner.Descriptor.HasFact(buffRef)) {
                BuffActivated(buff);
            } else {
                BuffDeactivated(buff);
            }
        }

        public void BuffActivated(BlueprintBuff buff) {
            var buffRef = buff.ToReference<BlueprintBuffReference>();
            m_ActiveWatchedBuffs.Add(buffRef);
            UpdateAbilitiesForBuff(buffRef);
        }

        public void BuffDeactivated(BlueprintBuff buff) {
            var buffRef = buff.ToReference<BlueprintBuffReference>();
            m_ActiveWatchedBuffs.Remove(buffRef);
            UpdateAbilitiesForBuff(buffRef);
        }

        private void UpdateStateForAbility(BlueprintAbility abilityBlueprint) {
            if (!m_AbilitiesToBuffs.TryGetValue(abilityBlueprint, out var watchedBuffs) || !m_AbilitiesToMechanicSlots.TryGetValue(abilityBlueprint, out var slotRefs))
                return;

            var shouldBeActive = watchedBuffs.Any(buff => m_ActiveWatchedBuffs.Contains(buff));
            UpdateSlotRefs(slotRefs, shouldBeActive);
        }

        private void UpdateAbilitiesForBuff(BlueprintBuffReference buff) {
            if (!m_BuffsToAbilities.TryGetValue(buff, out var abilities))
                return;

            Dictionary<BlueprintAbility, bool> abilitiesToggleStatus = new Dictionary<BlueprintAbility, bool>();
            foreach(var abilityBlueprint in abilities) {
                if (m_AbilitiesToBuffs.TryGetValue(abilityBlueprint, out var watchedBuffs)) {
                    abilitiesToggleStatus.Add(abilityBlueprint, watchedBuffs.Any(buff => m_ActiveWatchedBuffs.Contains(buff)));
                }
            }
            foreach(var abilityToggleStatus in abilitiesToggleStatus) {
                if (m_AbilitiesToMechanicSlots.TryGetValue(abilityToggleStatus.Key, out var slotRefs)) {
                    UpdateSlotRefs(slotRefs, abilityToggleStatus.Value);
                }
            }
        }

        private void UpdateSlotRefs(List<WeakReference<MechanicActionBarSlot>> slotRefs, bool shouldBeActive) {
            List<WeakReference<MechanicActionBarSlot>> slotRefsToRemove = new List<WeakReference<MechanicActionBarSlot>>();
            foreach (var slotRef in slotRefs) {
                if (slotRef.TryGetTarget(out var slot)) {
                    if (slot is IPseudoActivatableMechanicsBarSlot pseudoActivatableSlot) {
                        pseudoActivatableSlot.ShouldBeActive = shouldBeActive;
                    }
                } else {
                    slotRefsToRemove.Add(slotRef);
                }
            }
            foreach (var slotRef in slotRefsToRemove) {
                slotRefs.Remove(slotRef);
            }
        }

        private void Validate() {
            var abilitiesInBuffsDictionary = m_BuffsToAbilities.Values.SelectMany(x => x).ToHashSet();
            foreach(var abilityBlueprint in abilitiesInBuffsDictionary) {
                if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error: ability \"{abilityBlueprint.NameSafe()}\" is in values of Buff dictionary, but is not a key in Abilities dictionary");
                }
            }

            var buffsInAbilitiesDictionary = m_AbilitiesToBuffs.Values.SelectMany(x => x).ToHashSet();
            foreach(var buff in buffsInAbilitiesDictionary) {
                if (!m_BuffsToAbilities.ContainsKey(buff)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error: buff \"{buff.NameSafe()}\" is in values of Abilities dictionary, but is not a key in the Buffs dictionary");
                }
            }

            var abilitiesInMechanicsSlots = m_AbilitiesToMechanicSlots
                .Values
                .SelectMany(x => x)
                .Select(slotRef => {
                    if (!slotRef.TryGetTarget(out var slot) || !(slot is IPseudoActivatableMechanicsBarSlot pseudoActivatableSlot))
                        return null;
                    return pseudoActivatableSlot.PseudoActivatableAbility.Blueprint;
                }).ToList();

            foreach (var abilityBlueprint in abilitiesInMechanicsSlots) {
                if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error: a MechanicSlot is registered for ability \"{abilityBlueprint.NameSafe()}\", but this ability is not a key in the Abilities dictionary");
                }
            }
        }
    }
}
