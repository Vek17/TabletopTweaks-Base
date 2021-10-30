using Kingmaker.UI.Common;
using Kingmaker.UI.UnitSettings;
using UnityEngine;
using static TabletopTweaks.NewComponents.AbilitySpecific.MetaRageComponent;

namespace TabletopTweaks.NewUI {
    class MechanicActionBarSlotMetaRage : MechanicActionBarSlotSpontaneusConvertedSpell {

        /*
        public override Sprite GetIcon() {
            return Spell.m_ConvertedFrom.Icon;
        }*/

        public override Sprite GetForeIcon() {
            var metarageData = Spell as MetaRageAbilityData;
            return metarageData?.MetamagicFeature?.Icon ?? base.GetForeIcon();
        }

        public override Sprite GetDecorationSprite() {
            return UIUtility.GetDecorationBorderByIndex(Spell.m_ConvertedFrom.DecorationBorderNumber);
        }

        public override Color GetDecorationColor() {
            return UIUtility.GetDecorationColorByIndex(Spell.m_ConvertedFrom.DecorationColorNumber);
        }
    }
}
