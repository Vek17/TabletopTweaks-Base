using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.UnitLogic;
using System.Text;

namespace TabletopTweaks.NewComponents.Prerequisites {
    class PrerequisiteFeaturesFromListFormatted : PrerequisiteFeaturesFromList {
        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            if (Amount <= 1) {
                stringBuilder.Append(string.Format("{0}:\n", UIStrings.Instance.Tooltips.OneFrom));
            } else {
                stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.FeaturesFrom, Amount));
                stringBuilder.Append(":\n");
            }
            for (int i = 0; i < Features.Length; i++) {
                stringBuilder.Append(GetFeatureString(unit, Features[i]));
                if (i < Features.Length - 1) {
                    stringBuilder.Append("\n");
                }
            }
            return stringBuilder.ToString();
        }
        private string GetFeatureString(UnitDescriptor unit, BlueprintFeature feature) {
            string featureString = feature.Name;
            if (HasFeature(unit, feature)) {
                return $"<color=#323545>{featureString}</color>";
            }
            return featureString;
        }
        private bool HasFeature(UnitDescriptor unit, BlueprintFeature feature) {
            return unit?.Progression?.Features?.HasFact(feature) ?? false;
        }
    }
}
