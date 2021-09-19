using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents
{
    class PseudoProgressionRankClassModifier : PseudoProgressionRankModifier
    {
        public override double GetModifier(UnitDescriptor d)
        {
            ClassData classData = d.Progression.Classes.Find(x => x.CharacterClass == m_ActualClass.Get());
            if (classData == null)
                return 0;
            else
            {
                double modifer = ((classData.Level * multiplier) + scalar);
                if (floored)
                    return Math.Max(modifer, 0);
                else
                    return modifer;
            }
            
          

            
        }

        public BlueprintCharacterClassReference m_ActualClass;

        public int scalar = 0;

        public double multiplier = 1;

        public bool floored = true;
    }
}
