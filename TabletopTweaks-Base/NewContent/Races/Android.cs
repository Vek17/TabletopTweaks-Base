using Kingmaker;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Base.NewContent.Races {
    internal static  class Android {
        public static void AddAndroid() {
            var DLC5_HumanRace_Android = BlueprintTools.GetBlueprintReference<BlueprintRaceReference>("d1d114f539b74468b157ac69c275f266");

            Game.Instance.BlueprintRoot.Progression.m_CharacterRaces = Game.Instance.BlueprintRoot.Progression.m_CharacterRaces.AppendToArray(DLC5_HumanRace_Android);
        }
    }
}
