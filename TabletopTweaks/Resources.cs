
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Extensions;

namespace TabletopTweaks {
    class Resources {
        private static IEnumerable<BlueprintScriptableObject> blueprints;
        static private IEnumerable<BlueprintBuff> polymorphBuffs;
        static public IEnumerable<BlueprintBuff> PolymorphBuffs {
            get {
                if (polymorphBuffs == null) {
                    Main.LogHeader($"Identifying Polymorph Buffs");
                    IEnumerable<BlueprintBuff> taggedPolyBuffs = Resources.GetBlueprints<BlueprintBuff>()
                        .Where(bp => bp.GetComponents<SpellDescriptorComponent>()
                            .Where(c => (c.Descriptor & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0);
                    polymorphBuffs = Resources.GetBlueprints<BlueprintAbility>()
                        .Where(bp =>
                            (bp.GetComponents<SpellDescriptorComponent>()
                                .Where(c => c != null)
                                .Where(d => d.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0)
                            || (bp.GetComponents<AbilityExecuteActionOnCast>()
                                .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>())
                                .Where(c => c.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0)
                            || (bp.GetComponents<AbilityEffectRunAction>()
                                .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>()
                                    .Concat(c.Actions.Actions.OfType<ContextActionConditionalSaved>()
                                        .SelectMany(a => a.Failed.Actions
                                        .OfType<ContextActionRemoveBuffsByDescriptor>())))
                                .Where(c => c.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0))
                        .SelectMany(a => a.FlattenAllActions())
                        .OfType<ContextActionApplyBuff>()
                        .Where(c => c.Buff != null)
                        .Select(c => c.Buff)
                        .Concat(taggedPolyBuffs)
                        .Where(bp => bp.AssetGuid != "e6f2fc5d73d88064583cb828801212f4") // Fatigued
                        .Where(bp => !bp.HasFlag(BlueprintBuff.Flags.HiddenInUi))
                        .Distinct();

                    polymorphBuffs
                        .OrderBy(c => c.name)
                        .ForEach(c => Main.LogPatch("PolymorphBuff Found", c));
                    Main.LogHeader($"Identified: {polymorphBuffs.Count()} Polymorph Buffs");
                }
                return polymorphBuffs;
            }
        }

        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
            }
            return blueprints.Concat(ResourcesLibrary.s_LoadedBlueprints.Values).OfType<T>().Distinct();
        }
        public static void AddBlueprint([NotNull]BlueprintScriptableObject blueprint, string assetId) {
            blueprint.m_AssetGuid = assetId;
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(assetId);
            if (loadedBlueprint == null) {
                ResourcesLibrary.s_LoadedBlueprints[assetId] = blueprint;
                Main.LogPatch("Added", blueprint);
                if (blueprint != null) {
                    blueprint.OnEnableWithLibrary();
                }
            }
            else {
                Main.LogDebug($"Asset ID: {assetId} already in use by: {loadedBlueprint.name}");
            }
        }
    }
}