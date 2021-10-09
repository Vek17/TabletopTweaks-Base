using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.UnitSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TabletopTweaks.NewComponents.AbilitySpecific.QuickStudyComponent;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotQuickStudy : MechanicActionBarSlotSpontaneusConvertedSpell {

        public override string GetTitle() {
            return $"{Spell.Name} - {Spell.m_ConvertedFrom.Name}";
        }

        public override TooltipBaseTemplate GetTooltipTemplate() {
            return new TooltipTemplateQuickStudy(Spell);
        }

        public override Sprite GetIcon() {
            return Spell.m_ConvertedFrom.Icon;
        }

        public override Sprite GetForeIcon() {
            return null;
        }

        public override Sprite GetDecorationSprite() {
            return UIUtility.GetDecorationBorderByIndex(Spell.m_ConvertedFrom.DecorationBorderNumber);
        }

        public override Color GetDecorationColor() {
            return UIUtility.GetDecorationColorByIndex(Spell.m_ConvertedFrom.DecorationColorNumber);
        }
    }
}
