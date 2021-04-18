using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    class PrerequisiteNoFeaturesFromList : Prerequisite {
        public ReferenceArrayProxy<BlueprintFeature, BlueprintFeatureReference> Features {
            get {
                return m_Features;
            }
            set {
                m_Features = value;
            }
        }

        public override bool Check([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
            foreach (BlueprintFeature blueprintFeature in Features) {
                int count = 0;
                if ((!(selectionState != null) || !selectionState.IsSelectedInChildren(blueprintFeature)) && unit.HasFact(blueprintFeature)) {
                    count++;
                    if (count > Amount) {
                        return false;
                    }
                }
            }
            return true;
        }

        public override string GetUIText() {
            StringBuilder stringBuilder = new StringBuilder();
            if (Amount == 0) {
                stringBuilder.Append("Doesn't have any of the following features");
            }
            else {
                stringBuilder.Append($"Doesn't have more than {Amount} of the following features");
            }
            stringBuilder.Append(":\n");
            for (int i = 0; i < Features.Length; i++) {
                stringBuilder.Append(Features[i].Name);
                if (i < Features.Length - 1) {
                    stringBuilder.Append("\n");
                }
            }
            return stringBuilder.ToString();
        }

        [SerializeField]
        [FormerlySerializedAs("Features")]
        public BlueprintFeatureReference[] m_Features;
        public int Amount = 0;
    }
}
