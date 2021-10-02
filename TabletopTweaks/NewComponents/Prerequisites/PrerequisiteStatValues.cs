using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using System.Text;

namespace TabletopTweaks.NewComponents.Prerequisites {
    class PrerequisiteStatValues : Prerequisite {
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state) {
            return CheckUnit(unit);
        }

        public bool CheckUnit(UnitDescriptor unit) {
            return Stats.Count(stat => GetStatValue(unit, stat) >= Value) >= Amount;
        }

        public override string GetUITextInternal(UnitDescriptor unit) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Has {Value} {(Value > 1 ? "Ranks" : "Rank")} in {GetWord(Amount)} of the following skills:");
            stringBuilder.Append("\n");
            foreach (var stat in Stats) {
                stringBuilder.Append(GetStatString(unit, stat));
            }
            return stringBuilder.ToString();
        }

        private int GetStatValue(UnitDescriptor unit, StatType stat) {
            int num = stat.IsSkill() ? unit.Stats.GetStat(stat).BaseValue : unit.Stats.GetStat(stat).PermanentValue;
            foreach (ReplaceStatForPrerequisites replaceStatForPrerequisites in unit.Progression.Features.SelectFactComponents<ReplaceStatForPrerequisites>()) {
                if (replaceStatForPrerequisites.OldStat == stat) {
                    num = ReplaceStatForPrerequisites.ResultStat(replaceStatForPrerequisites, num, unit, false);
                }
            }
            return num;
        }

        private string GetStatString(UnitDescriptor unit, StatType stat) {
            StringBuilder stringBuilder = new StringBuilder();
            string statString = LocalizedTexts.Instance.Stats.GetText(stat);
            stringBuilder.Append(statString);
            if (unit != null) {
                stringBuilder.Append(": ");
                stringBuilder.Append($"Current value: {GetStatValue(unit, stat)}");
                stringBuilder.Append("\n");
            }
            if (GetStatValue(unit, stat) >= Value) {
                return $"<color=#323545>{stringBuilder}</color>";
            }
            return stringBuilder.ToString();
        }

        private string GetWord(int i) {
            return i switch {
                1 => "one",
                2 => "two",
                3 => "three",
                4 => "four",
                5 => "five",
                6 => "six",
                7 => "seven",
                8 => "eight",
                9 => "nine",
                _ => i.ToString(),
            };
        }

        public StatType[] Stats;
        public int Value;
        public int Amount;
    }
}
