using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Text;
using TabletopTweaks.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.Prerequisites {
    [TypeId("33c66b9aaaa348cfa8efd39841cf255b")]
    public class PrerequisiteNoFeaturesFromList : Prerequisite {
        [InitializeStaticString]
        private static readonly LocalizedString DoesntHaveFeature = Helpers.CreateString("PrerequisiteNoFeaturesFromList.UI", "Doesn't have any of the following features");
        [InitializeStaticString]
        private static readonly LocalizedString DoesntHaveMoreThan = Helpers.CreateString("PrerequisiteNoFeaturesFromList.UI", "Doesn't have more than");
        [InitializeStaticString]
        private static readonly LocalizedString OfTheFollowingFeatures = Helpers.CreateString("PrerequisiteNoFeaturesFromList.UI", "of the following features");
        public ReferenceArrayProxy<BlueprintFeature, BlueprintFeatureReference> Features {
            get {
                return m_Features;
            }
            set {
                m_Features = value;
            }
        }
        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            if (Amount == 0) {
                stringBuilder.Append(DoesntHaveFeature);
            } else {
                stringBuilder.Append(DoesntHaveMoreThan);
                stringBuilder.Append(" ");
                stringBuilder.Append(Amount);
                stringBuilder.Append(" ");
                stringBuilder.Append(OfTheFollowingFeatures);
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

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state) {
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

        [SerializeField]
        [FormerlySerializedAs("Features")]
        public BlueprintFeatureReference[] m_Features;
        public int Amount = 0;
    }
}
