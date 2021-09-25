using Kingmaker.Blueprints.Classes.Prerequisites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents
{
    public abstract class PrerequisiteAbstractLogical : Prerequisite
    {
        

        public List<Prerequisite> componentPrequisites = new List<Prerequisite>();


        public void AddPrequisiteToList(Prerequisite p)
        {
            componentPrequisites.Add(p);
            p.OwnerBlueprint = this.OwnerBlueprint;
        }

    }
}
