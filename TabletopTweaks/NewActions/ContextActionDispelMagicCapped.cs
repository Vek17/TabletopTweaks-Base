using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewActions {
    [TypeId("a4bd44f0c42744db82e5ffaedb9b956a")]
    class ContextActionDispelMagicCapped : ContextActionDispelMagic {

        public override void RunAction() {
            if (base.Target.Unit == null) {
                using (EntityPoolEnumerator<AreaEffectEntityData> enumerator = Game.Instance.State.AreaEffects.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        AreaEffectEntityData areaEffectEntityData = enumerator.Current;
                        if (areaEffectEntityData.Blueprint.Shape != AreaEffectShape.AllArea
                            && areaEffectEntityData.View.Contains(base.Target.Point)
                            && TryDispelAreaEffect(areaEffectEntityData)
                            && m_StopAfterFirstRemoved) {
                            break;
                        }
                    }
                    return;
                }
            }
            List<Buff> buffList = base.Target.Unit.Buffs.Enumerable.ToList();
            buffList.Sort((Buff b1, Buff b2) => -b1.Context.Params.CasterLevel.CompareTo(b2.Context.Params.CasterLevel));
            int maxDispels = m_StopAfterFirstRemoved ? 1 : (DispelLimitDividend.Calculate(base.Context) / DispelLimitDivisor.Calculate(base.Context));
            int dispelledBuffs = 0;

            foreach (var buff in buffList) {
                if (TryDispelBuff(buff)) {
                    dispelledBuffs++;
                }
                if (maxDispels > 0 && dispelledBuffs >= maxDispels) {
                    break;
                }
            }
        }
        public ContextValue DispelLimitDividend;
        public ContextValue DispelLimitDivisor = 1;
    }
}
