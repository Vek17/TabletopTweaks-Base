using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Utilities
{
    public static class ClassTools
    {
        /// <summary>
        /// Edit the level entry for target level if it exists, create if it isn't 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="level"></param>
        /// <param name="payload"></param>
        public static void CreateOrEditLevel(this LevelEntry[] target, int level, Action<LevelEntry> payload)
        {
            LevelEntry targetEntry = target.FirstOrDefault(x => x.Level == level);
            if (targetEntry == null)
            {
                targetEntry = new LevelEntry { Level = level };
                payload.Invoke(targetEntry);
                target.AddItem(targetEntry);
            }
            else
            {
                payload.Invoke(targetEntry);
            }
        }

    }
}
