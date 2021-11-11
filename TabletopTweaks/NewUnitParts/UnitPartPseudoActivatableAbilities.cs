using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewUI;

namespace TabletopTweaks.NewUnitParts {
    [TypeId("49345c288edf44e9bd9187612ed53c66")]
    public class UnitPartPseudoActivatableAbilities :
        UnitPart,
        ISubscriber,
        IUnitSubscriber,
        IUnitGainFactHandler,
        IUnitLostFactHandler {

        private static BlueprintBuffReference _nullBuffRef = BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);

        private Dictionary<BlueprintAbility, List<WeakReference<MechanicActionBarSlot>>> m_AbilitiesToMechanicSlots =
            new Dictionary<BlueprintAbility, List<WeakReference<MechanicActionBarSlot>>>();

        private Dictionary<BlueprintAbility, HashSet<BlueprintBuffReference>> m_AbilitiesToBuffs =
            new Dictionary<BlueprintAbility, HashSet<BlueprintBuffReference>>();

        private Dictionary<BlueprintBuffReference, HashSet<BlueprintAbility>> m_BuffsToAbilities =
            new Dictionary<BlueprintBuffReference, HashSet<BlueprintAbility>>();

        [JsonProperty]
        private HashSet<BlueprintBuffReference> m_ActiveWatchedBuffs = new HashSet<BlueprintBuffReference>();

        private Dictionary<string, HashSet<BlueprintBuffReference>> m_GroupsToBuffs = new Dictionary<string, HashSet<BlueprintBuffReference>>();
        private Dictionary<BlueprintBuffReference, HashSet<string>> m_BuffsToGroups = new Dictionary<BlueprintBuffReference, HashSet<string>>();

        public void RegisterPseudoActivatableAbilitySlot(MechanicActionBarSlot mechanicSlot) {
            if (!(mechanicSlot is IPseudoActivatableMechanicsBarSlot abilitySlot))
                return;

            var abilityBlueprint = abilitySlot.PseudoActivatableAbility.Blueprint;
            if (m_AbilitiesToMechanicSlots.TryGetValue(abilityBlueprint, out var slotRefs)) {
                slotRefs.Add(new WeakReference<MechanicActionBarSlot>(mechanicSlot));
            } else {
                m_AbilitiesToMechanicSlots.Add(abilityBlueprint, new List<WeakReference<MechanicActionBarSlot>>() { new WeakReference<MechanicActionBarSlot>(mechanicSlot) });
            }

            RegisterPseudoActivatableAbility(abilitySlot.PseudoActivatableAbility);
#if DEBUG
            Validate();
#endif
            UpdateStateForAbility(abilityBlueprint);
        }

        public override void OnPostLoad() {
            foreach (var buff in this.Owner.Buffs) {
                BuffGained(buff);
            }
            Validate();
        }

        public void HandleUnitGainFact(EntityFact fact) {
            if (fact.Owner != this.Owner)
                return;
            if (fact is Buff buff) {
                BuffGained(buff);
            }
        }

        private void BuffGained(Buff buff) {
            if (m_BuffsToAbilities.ContainsKey(buff.Blueprint.ToReference<BlueprintBuffReference>())) {
                BuffActivated(buff.Blueprint);
            } else {
                var pseudoActivatableComponent = buff.SourceAbility?.GetComponent<PseudoActivatable>();
                if (pseudoActivatableComponent != null
                    && pseudoActivatableComponent.Type == PseudoActivatable.PseudoActivatableType.BuffToggle
                    && pseudoActivatableComponent.Buff.Equals(buff.Blueprint.ToReference<BlueprintBuffReference>())) {
                    RegisterToggledBuffForAbility(
                        buff.SourceAbility,
                        buff.Blueprint.ToReference<BlueprintBuffReference>(),
                        buff.SourceAbility.GetComponent<PseudoActivatable>().GroupName);
                    BuffActivated(buff.Blueprint);
                }
            }
        }

        public void HandleUnitLostFact(EntityFact fact) {
            if (fact.Owner != this.Owner)
                return;

            if (fact is Buff buff) {
                BuffDeactivated(buff.Blueprint);
            }
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
                    Main.LogDebug($"UnitPartPseudoActivatableAbilities.RegisterPseudoActivatableAbility called for ability \"{abilityBlueprint.NameSafe()}\", but the PseudoActivatable component has no Buff set, and the ability does not have variants.");
                    if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                        m_AbilitiesToBuffs.Add(abilityBlueprint, new HashSet<BlueprintBuffReference>());
                    }
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
                if (m_GroupsToBuffs.TryGetValue(buffGroupName, out var buffsInGroup)) {
                    buffsInGroup.Add(buffRef);
                } else {
                    m_GroupsToBuffs.Add(buffGroupName, new HashSet<BlueprintBuffReference> { buffRef });
                }
                if (m_BuffsToGroups.TryGetValue(buffRef, out var groupsForBuff)) {
                    groupsForBuff.Add(buffGroupName);
                } else {
                    m_BuffsToGroups.Add(buffRef, new HashSet<string> { buffGroupName });
                }
            }
        }

        public void BuffActivated(BlueprintBuff buff) {
            var buffRef = buff.ToReference<BlueprintBuffReference>();
            if (m_ActiveWatchedBuffs.Add(buffRef)) {
                if (m_BuffsToGroups.TryGetValue(buffRef, out var groupsForBuff)) {
                    // add the to be removed buffs to a collection first, otherwise we'll be modifying m_ActiveWatchedBuffs while using it in the calculation
                    var buffsToRemove = new HashSet<BlueprintBuffReference>();
                    // double nested loop, but usually buffs are only in one group, and there should be in the order of 10 buffs in a group so this should be fine
                    foreach (var groupName in groupsForBuff) {
                        foreach (var buffToRemove in m_GroupsToBuffs[groupName].Where(b => !b.Equals(buffRef) && m_ActiveWatchedBuffs.Contains(b))) {
                            buffsToRemove.Add(buffToRemove);
                        }
                    }
                    foreach (var buffToRemove in buffsToRemove) {
                        this.Owner.Buffs.RemoveFact(buffToRemove);
                    }
                }
                UpdateAbilitiesForBuff(buffRef);
            }
        }

        public void BuffDeactivated(BlueprintBuff buff) {
            var buffRef = buff.ToReference<BlueprintBuffReference>();
            if (m_ActiveWatchedBuffs.Remove(buffRef)) {
                UpdateAbilitiesForBuff(buffRef);
            }
        }

        private void UpdateAbilitiesForBuff(BlueprintBuffReference buff) {
            if (!m_BuffsToAbilities.TryGetValue(buff, out var abilities))
                return;

            foreach (var abilityBlueprint in abilities) {
                UpdateStateForAbility(abilityBlueprint);
            }
        }

        private void UpdateStateForAbility(BlueprintAbility abilityBlueprint) {
            if (!m_AbilitiesToBuffs.TryGetValue(abilityBlueprint, out var watchedBuffs) || !m_AbilitiesToMechanicSlots.TryGetValue(abilityBlueprint, out var slotRefs))
                return;

            var shouldBeActive = watchedBuffs.Any(buff => m_ActiveWatchedBuffs.Contains(buff));
            BlueprintBuffReference buffForForeIcon = null;

            var pseudoActivatableComponent = abilityBlueprint.GetComponent<PseudoActivatable>();
            if (pseudoActivatableComponent.m_Type == PseudoActivatable.PseudoActivatableType.VariantsBase) {
                if (!pseudoActivatableComponent.ActiveWhenVariantActive) {
                    shouldBeActive = false;
                }
                if (pseudoActivatableComponent.UseActiveVariantForeIcon) {
                    if (watchedBuffs.Count > 1) {
                        var activeBuffs = watchedBuffs.Where(b => m_ActiveWatchedBuffs.Contains(b)).ToList();
                        if (activeBuffs.Count == 1) {
                            buffForForeIcon = activeBuffs[0];
                        }
                    }
                }
            }
            UpdateSlotRefs(slotRefs, shouldBeActive, buffForForeIcon);
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

            foreach (var activeBuff in m_ActiveWatchedBuffs) {
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
            foreach (var abilityBlueprint in abilitiesInBuffsDictionary) {
                if (!m_AbilitiesToBuffs.ContainsKey(abilityBlueprint)) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": ability \"{abilityBlueprint.NameSafe()}\" is in values of Buff dictionary, but is not a key in Abilities dictionary");
                }
            }

            var buffsInAbilitiesDictionary = m_AbilitiesToBuffs.Values.SelectMany(x => x).ToHashSet();
            foreach (var buff in buffsInAbilitiesDictionary) {
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

            foreach (var buffGroup in m_GroupsToBuffs) {
                foreach (var buffRef in buffGroup.Value) {
                    if (!m_BuffsToGroups.ContainsKey(buffRef)) {
                        Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": buff \"{buffRef.NameSafe()}\" is in group named \"{buffGroup.Key}\" in m_GroupsToBuffs, but this buff is not present as a key in m_BuffsToGroups");
                    } else if (!m_BuffsToGroups[buffRef].Contains(buffGroup.Key)) {
                        Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": buff \"{buffRef.NameSafe()}\" is in group named \"{buffGroup.Key}\" in m_GroupsToBuffs, but the groups for this buff in m_BuffsToGroups do not contain \"{buffGroup.Key}\"");
                    }
                }
            }

            foreach (var groupedBuff in m_BuffsToGroups) {
                foreach (var groupName in groupedBuff.Value) {
                    if (!m_GroupsToBuffs.ContainsKey(groupName)) {
                        Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": buff group \"{groupName}\" is linked to buff \"{groupedBuff.Key.NameSafe()}\" in m_BuffsToGroups, but this group is not present as a key in m_GroupsToBuffs");
                    } else if (!m_GroupsToBuffs[groupName].Contains(groupedBuff.Key)) {
                        Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": buff group \"{groupName}\" is linked to buff \"{groupedBuff.Key.NameSafe()}\" in m_BuffsToGroups, but the buffs for this group in m_GroupsToBuffs do not contain \"{groupedBuff.Key.NameSafe()}\"");
                    }
                }
            }

            foreach (var buffGroup in m_GroupsToBuffs) {
                var hasBuffs = new HashSet<BlueprintBuffReference>();
                foreach (var buff in buffGroup.Value) {
                    if (this.Owner.HasFact(buff)) {
                        hasBuffs.Add(buff);
                    }
                }
                if (hasBuffs.Count > 1) {
                    Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\": multiple buffs are active for group {buffGroup.Key}. Active buffs: ");
                    foreach (var activeBuff in hasBuffs) {
                        Main.Log($"WARNING: UnitPartPseudoActivatableAbilities Validation Error on unit \"{Owner.CharacterName}\":  - {activeBuff.NameSafe()}");
                    }
                }
            }
        }
    }
}
