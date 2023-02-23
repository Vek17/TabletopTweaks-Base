using Kingmaker.UnitLogic.FactLogic;
using System;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    internal class AnimalCompanions {
        private static class LevelAdjustments {
            [PostPatchInitialize]
            static void Update_AddPet_RankToLevelAnimalCompanion() {
                if (TTTContext.Fixes.AnimalCompanions.IsDisabled("AnimalCompanionProgression")) { return; }
                AddPet.RankToLevelAnimalCompanion[0] = 0;
                AddPet.RankToLevelAnimalCompanion[1] = 2;
                AddPet.RankToLevelAnimalCompanion[2] = 3;
                AddPet.RankToLevelAnimalCompanion[3] = 3;
                AddPet.RankToLevelAnimalCompanion[4] = 4;
                AddPet.RankToLevelAnimalCompanion[5] = 5;
                AddPet.RankToLevelAnimalCompanion[6] = 6;
                AddPet.RankToLevelAnimalCompanion[7] = 6;
                AddPet.RankToLevelAnimalCompanion[8] = 7;
                AddPet.RankToLevelAnimalCompanion[9] = 8;
                AddPet.RankToLevelAnimalCompanion[10] = 9;
                AddPet.RankToLevelAnimalCompanion[11] = 9;
                AddPet.RankToLevelAnimalCompanion[12] = 10;
                AddPet.RankToLevelAnimalCompanion[13] = 11;
                AddPet.RankToLevelAnimalCompanion[14] = 12;
                AddPet.RankToLevelAnimalCompanion[15] = 12;
                AddPet.RankToLevelAnimalCompanion[16] = 13;
                AddPet.RankToLevelAnimalCompanion[17] = 14;
                AddPet.RankToLevelAnimalCompanion[18] = 15;
                AddPet.RankToLevelAnimalCompanion[19] = 15;
                AddPet.RankToLevelAnimalCompanion[20] = 16;

                Main.TTTContext.Logger.Log(String.Join(",", AddPet.RankToLevelAnimalCompanion));
            }
        }
    }
}
