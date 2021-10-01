using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [TypeId("febcdf299ca242a7be5bacfda8e4254f")]
    class ContextRestoreResourcesFixed : ContextAction {
        public BlueprintAbilityResource Resource {
            get {
                BlueprintAbilityResourceReference resource = this.m_Resource;
                if (resource == null) {
                    return null;
                }
                return resource.Get();
            }
        }

        public override string GetCaption() {
            return "Restore resource";
        }

        public override void RunAction() {
            TargetWrapper target = base.Target;
            UnitEntityData unitEntityData = target?.Unit;
            if (unitEntityData == null) {
                PFLog.Default.Error("Target is missing", Array.Empty<object>());
                return;
            }
            if (m_IsFullRestoreAllResources) {
                unitEntityData.Descriptor.Resources.FullRestoreAll();
                return;
            }
            if (!ContextValueRestoration) {
                unitEntityData.Descriptor.Resources.Restore(Resource, 1);
                return;
            }
            unitEntityData.Descriptor.Resources.Restore(Resource, Value.Calculate(base.Context));
        }

        public bool m_IsFullRestoreAllResources;

        [SerializeField]
        [FormerlySerializedAs("Resource")]
        [HideIf("m_IsFullRestoreAllResources")]
        private BlueprintAbilityResourceReference m_Resource;

        [HideIf("m_IsFullRestoreAllResources")]
        public bool ContextValueRestoration;

        [ShowIf("ContextValueRestoration")]
        public ContextValue Value;
    }
}
