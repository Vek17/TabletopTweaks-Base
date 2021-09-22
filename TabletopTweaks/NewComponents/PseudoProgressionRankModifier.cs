using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents
{
    public abstract class PseudoProgressionRankModifier : UnitFactComponentDelegate
    {
        
        public BlueprintFeatureReference Key;

        public abstract double GetModifier(UnitDescriptor d);

        
    }
}
