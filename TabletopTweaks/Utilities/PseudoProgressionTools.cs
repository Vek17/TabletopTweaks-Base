using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TabletopTweaks.Utilities
{
    static class PseudoProgressionTools
    {
        static Dictionary<string, PseudoProgression> pseudoProgressions = new();
        public static void AddPseudoProgression(string name, string desc, Sprite icon)
        {

        }

    }

    public class PseudoProgression
    {
        public string Name { get; }
        string Desc { get; }
        Sprite Icon { get; }
        public PseudoProgression(string name, string desc, Sprite icon)
        {
            Name = name;
        }

    }
}
