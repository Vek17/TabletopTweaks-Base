using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents
{
    // Token: 0x02001CEB RID: 7403
    [ComponentName("Add ability resources")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]

    public class FeatureSpecificClassLevelsForPrerequisites : UnitFactComponentDelegate
    {

        public BlueprintCharacterClass FakeClass
        {
            get
            {
                BlueprintCharacterClassReference fakeClass = this.m_FakeClass;
                if (fakeClass == null)
                {
                    return null;
                }
                return fakeClass.Get();
            }
        }

        public Func<BlueprintFeature, bool> _Applicable;

        public bool IsApplicable(BlueprintFeature blueprintFeature)
        {
            if (_Applicable == null)
            {
                return false;
            }
            else
            {
                return _Applicable.Invoke(blueprintFeature);
            }
        }

        public BlueprintCharacterClass ActualClass
        {
            get
            {
                BlueprintCharacterClassReference actualClass = this.m_ActualClass;
                if (actualClass == null)
                {
                    return null;
                }
                return actualClass.Get();
            }
        }

        // Token: 0x04007B77 RID: 31607
        [SerializeField]
        [FormerlySerializedAs("FakeClass")]
        public BlueprintCharacterClassReference m_FakeClass;

        // Token: 0x04007B78 RID: 31608
        [SerializeField]
        [FormerlySerializedAs("ActualClass")]
        public BlueprintCharacterClassReference m_ActualClass;

        public FeatureSpecificClassLevelsForPrerequisites() { }

        public FeatureSpecificClassLevelsForPrerequisites(BlueprintCharacterClassReference fake, BlueprintCharacterClassReference real, Func<BlueprintFeature, bool> func, double modifier, int summand)
        {
            m_ActualClass = real;
            m_FakeClass = fake;
            _Applicable = func;
            Modifier = modifier;
            Summand = summand;
        }

        [SerializeField]
        private BlueprintFeatureReference SpecificFeature;

        // Token: 0x04007B79 RID: 31609
        public double Modifier;

        // Token: 0x04007B7A RID: 31610
        public int Summand;
    }
}
