using Kingmaker.Blueprints;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
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

        private Dictionary<string, HashSet<BlueprintBuffReference>> m_BuffGroups = new Dictionary<string, HashSet<BlueprintBuffReference>>();

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

            RegisterPseudoActivatableAbility(abilitySlot.PseudoActivatableAbility);
#if DEBUG
            Validate();
#endif
            UpdateStateForAbility(abilityBlueprint);
        }

        public override void OnPostLoad() {
            foreach(var ability in this.Owner.Abilities) {
                if (ability.GetComponent<PseudoActivatable>() != null) {
                    RegisterPseudoActivatableAbility(ability.Data);
                }
            }

            foreach (var buff in this.Owner.Buffs) {
                if (m_BuffsToAbilities.ContainsKey(buff.Blueprint.ToReference<BlueprintBuffReference>())) {
                    BuffActivated(buff.Blueprint);
                }
            }
            Validate();
        }

        public void RegisterPseudoActivatableAbility(AbilityData ability) {
            var abilityBlueprint = ability.Blueprint;
            var pseudoActivatableComponent = abilityBlueprint.GetComponent<PseudoActivatable>();
            if (pseudoActivatableComponent == null) {
                Main.Log($"WARNING: UnitPartPseudoActivatableAbilities.RegisterPseudoActivatableAbility called for ability \"{abilityBlueprint.NameSafe()}\", which does not have a PseudoActivatable component");
                return;
            }
            if (!pseudoActivatableComponent.Buff.Equals(_nullBuffRef)) {
                RegisterToggledBuffForAbility(abilityBlueprint, pseudoActivatableComponent.Buff, pseudoActivatableComponent.GroupName);
            } else {
                var abilityVariants = ability.GetConversions();
                if (abilityVariants.Empty()) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities.RegisterPseudoActivatableAbility called for ability \"{abilityBlueprint.NameSafe()}\", but the PseudoActivatable component has no Buff set, and the ability does not have variants.");
                    return;
                }

                foreach (var variant in abilityVariants) {
                    var variantBlueprint = variant.Blueprint;
                    var variantPseudoActivatableComponent = variantBlueprint.GetComponent<PseudoActivatable>();
                    if (variantPseudoActivatableComponent != null && !variantPseudoActivatableComponent.Buff.Equals(_nullBuffRef)) {
                        RegisterToggledBuffForAbility(variantBlueprint, variantPseudoActivatableComponent.Buff, variantPseudoActivatableComponent.GroupName);
                        RegisterToggledBuffForAbility(abilityBlueprint, variantPseudoActivatableComponent.Buff, variantPseudoActivatableComponent.GroupName);
                    }
                }
            }
        }

        private void RegisterToggledBuffForAbility(BlueprintAbility abilityBlueprint, BlueprintBuffReference buffRef, string buffGroupName) {
            if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                m_AbilitiesToBuffs.Add(abilityBlueprint, new HashSet<BlueprintBuffReference>());
            }
            m_AbilitiesToBuffs[abilityBlueprint].Add(buffRef);
            if (m_BuffsToAbilities.TryGetValue(buffRef, out var abilities)) {
                abilities.Add(abilityBlueprint);
            } else {
                m_BuffsToAbilities.Add(buffRef, new HashSet<BlueprintAbility> { abilityBlueprint });
            }
            if (buffGroupName != null) {
                if (m_BuffGroups.TryGetValue(buffGroupName, out var buffsInGroup)) {
                    buffsInGroup.Add(buffRef);
                } else {
                    m_BuffGroups.Add(buffGroupName, new HashSet<BlueprintBuffReference> { buffRef });
                }
            }
        }

        public void ToggleBuff(BlueprintBuffReference buffRef, BlueprintAbility sourceAbility, MechanicsContext context, string buffGroupName) {
            if (!this.Owner.Descriptor.HasFact(buffRef)) {
                if (buffGroupName != null) {
                    if (!m_BuffGroups.TryGetValue(buffGroupName, out var buffsInGroup)) {
                        m_BuffGroups.Add(buffGroupName, new HashSet<BlueprintBuffReference> { buffRef });
                    } else {
                        buffsInGroup.Add(buffRef);
                        foreach (var buffInGroup in buffsInGroup) {
                            if (this.Owner.Descriptor.HasFact(buffInGroup)) {
                                this.Owner.Buffs.RemoveFact(buffInGroup);
                                BuffDeactivated(buffInGroup);
                            }
                        }
                    }
                }
                var appliedBuff = this.Owner.Descriptor.AddBuff(buffRef, context, new TimeSpan?());
                appliedBuff.IsFromSpell = false;
                appliedBuff.IsNotDispelable = true;
                appliedBuff.SourceAbility = sourceAbility;
                BuffActivated(buffRef);
            } else {
                this.Owner.Buffs.RemoveFact(buffRef);
                BuffDeactivated(buffRef);
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

        private void UpdateAbilitiesForBuff(BlueprintBuffReference buff) {
            if (!m_BuffsToAbilities.TryGetValue(buff, out var abilities))
                return;

            foreach(var abilityBlueprint in abilities) {
                UpdateStateForAbility(abilityBlueprint);
            }
        }

        private void UpdateStateForAbility(BlueprintAbility abilityBlueprint) {
            if (!m_AbilitiesToBuffs.TryGetValue(abilityBlueprint, out var watchedBuffs) || !m_AbilitiesToMechanicSlots.TryGetValue(abilityBlueprint, out var slotRefs))
                return;

            var shouldBeActive = watchedBuffs.Any(buff => m_ActiveWatchedBuffs.Contains(buff));
            BlueprintBuffReference activeBuff = null;
            if (watchedBuffs.Count > 1) {
                var activeBuffs = watchedBuffs.Where(b => m_ActiveWatchedBuffs.Contains(b)).ToList();
                if (activeBuffs.Count == 1) {
                    activeBuff = activeBuffs[0];
                }
            }
            UpdateSlotRefs(slotRefs, shouldBeActive, activeBuff);
        }


        private void UpdateSlotRefs(List<WeakReference<MechanicActionBarSlot>> slotRefs, bool shouldBeActive, BlueprintBuffReference buffForForeIcon = null) {
            List<WeakReference<MechanicActionBarSlot>> slotRefsToRemove = new List<WeakReference<MechanicActionBarSlot>>();
            foreach (var slotRef in slotRefs) {
                if (slotRef.TryGetTarget(out var slot)) {
                    if (slot is IPseudoActivatableMechanicsBarSlot pseudoActivatableSlot) {
                        pseudoActivatableSlot.ShouldBeActive = shouldBeActive;
                    }
                    if (slot is MechanicActionBarSlotPseudoActivatableAbility pseudoActivatableAbilitySlot) {
                        pseudoActivatableAbilitySlot.ForeIconOverride = buffForForeIcon?.Get()?.Icon;
                        pseudoActivatableAbilitySlot.ShouldUpdateForeIcon = true;
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

            foreach(var activeBuff in m_ActiveWatchedBuffs) {
                if (!this.Owner.HasFact(activeBuff)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": buff \"{activeBuff.NameSafe()}\" is in active watched buffs list, but owner does not actually have that buff.");
                }
            }

            foreach (var buff in this.Owner.Buffs) {
                if (buff.SourceAbility != null 
                    && buff.SourceAbility.GetComponent<PseudoActivatable>() != null 
                    && buff.SourceAbility.GetComponent<PseudoActivatable>().Buff.Equals(buff.Blueprint.ToReference<BlueprintBuffReference>())
                    && !m_ActiveWatchedBuffs.Contains(buff.Blueprint.ToReference<BlueprintBuffReference>())) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": unit has buff \"{buff.Name}\", which is toggled by pseudo activatable ability \"{buff.SourceAbility.NameSafe()}\", but this buff is not present in active watched buff list.");
                }
            }

            var abilitiesInBuffsDictionary = m_BuffsToAbilities.Values.SelectMany(x => x).ToHashSet();
            foreach(var abilityBlueprint in abilitiesInBuffsDictionary) {
                if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": ability \"{abilityBlueprint.NameSafe()}\" is in values of Buff dictionary, but is not a key in Abilities dictionary");
                }
            }

            var buffsInAbilitiesDictionary = m_AbilitiesToBuffs.Values.SelectMany(x => x).ToHashSet();
            foreach(var buff in buffsInAbilitiesDictionary) {
                if (!m_BuffsToAbilities.ContainsKey(buff)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": buff \"{buff.NameSafe()}\" is in values of Abilities dictionary, but is not a key in the Buffs dictionary");
                }
            }

            var abilitiesInMechanicsSlots = m_AbilitiesToMechanicSlots
                .Values
                .SelectMany(x => x)
                .Select(slotRef => {
                    if (!slotRef.TryGetTarget(out var slot) || !(slot is IPseudoActivatableMechanicsBarSlot pseudoActivatableSlot))
                        return null;
                    return pseudoActivatableSlot.PseudoActivatableAbility.Blueprint;
                })
                .NotNull()
                .ToList();

            foreach (var abilityBlueprint in abilitiesInMechanicsSlots) {
                if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": a MechanicSlot is registered for ability \"{abilityBlueprint.NameSafe()}\", but this ability is not a key in the Abilities dictionary");
                }
            }

            foreach (var buffGroup in m_BuffGroups) {
                var hasBuffs = new HashSet<BlueprintBuffReference>();
                foreach(var buff in buffGroup.Value) {
                    if (this.Owner.HasFact(buff)) {
                        hasBuffs.Add(buff);
                    }
                }
                if (hasBuffs.Count > 1) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": multiple buffs are active for group {buffGroup.Key}. Active buffs: ");
                    foreach(var activeBuff in hasBuffs) {
                        Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\":  - {activeBuff.NameSafe()}");
                    }
                }
            }
        }
    }
}
