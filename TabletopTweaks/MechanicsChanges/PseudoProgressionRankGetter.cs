using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.MechanicsChanges
{
    public class PseudoProgressionRankGetter : PropertyValueGetter
    {
       
        
        public override int GetBaseValue(UnitEntityData unit)
        {

            List<PseudoProgressionRankModifier> features = unit.Progression.Features.SelectFactComponents<PseudoProgressionRankModifier>().ToList();
            double rank = 0;
            foreach(PseudoProgressionRankModifier f in features)
            {
                if (f.Key.Get() == Key.Get())
                {
                    rank += f.GetModifier(unit);
                }
            }
            return (int)rank;
        }

        public BlueprintFeatureReference Key;
    }
}
