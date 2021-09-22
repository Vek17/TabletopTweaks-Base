using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents
{
    class PrerequisitePsuedoProgressionRank : Prerequisite
    {
        public BlueprintFeatureReference m_KeyRef;

        public BlueprintFeature Key => m_KeyRef.Get();

        public int Level = 1;

        public bool Not;

        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state)
        {
           
            if (this.Not)
            {
                return this.GetRank(unit) < this.Level;
            }
            return this.GetRank(unit) >= this.Level;
        }

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("{0} {1}: {2}", this.Key.Name, UIStrings.Instance.Tooltips.Level, this.Level));
            if (unit != null)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.CurrentValue, this.GetRank(unit)));
            }
            return stringBuilder.ToString();
        }

        private int GetRank(UnitDescriptor unit)
        {
            int num = 0;
            List<PseudoProgressionRankModifier> features = unit.Progression.Features.SelectFactComponents<PseudoProgressionRankModifier>().ToList();
            double rank = 0;
            foreach (PseudoProgressionRankModifier f in features)
            {
                if (f.Key.Get() == m_KeyRef.Get())
                {
                    rank += f.GetModifier(unit);
                }
            }
            return (int)rank;

            

        }
    }
}
