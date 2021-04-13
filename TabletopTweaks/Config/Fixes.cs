using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class Fixes {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool EnableArchetypePrerequisites = true;
        public bool FixDemonSubtypes = true;
        public FixGroup Aeon = new FixGroup();
        public FixGroup Azata = new FixGroup();
        public FixGroup Bloodrager = new FixGroup();
        public FixGroup DragonDisciple = new FixGroup();
        public FixGroup Slayer = new FixGroup();
        public FixGroup Witch = new FixGroup();
        public FixGroup Spells = new FixGroup();
        public FixGroup Bloodlines = new FixGroup();
        public FixGroup Feats = new FixGroup();
        public FixGroup MythicAbilities = new FixGroup();

        public void OverrideFixes(Fixes newFixes) {
            DisableNaturalArmorStacking = newFixes.DisableNaturalArmorStacking;
            DisablePolymorphStacking = newFixes.DisablePolymorphStacking;
            EnableArchetypePrerequisites = newFixes.EnableArchetypePrerequisites;
            FixDemonSubtypes = newFixes.FixDemonSubtypes;
            Aeon.LoadFixgroup(newFixes.Aeon);
            Azata.LoadFixgroup(newFixes.Azata);
            Bloodrager.LoadFixgroup(newFixes.Bloodrager);
            DragonDisciple.LoadFixgroup(newFixes.DragonDisciple);
            Slayer.LoadFixgroup(newFixes.Slayer);
            Witch.LoadFixgroup(newFixes.Witch);
            Spells.LoadFixgroup(newFixes.Spells);
            Bloodlines.LoadFixgroup(newFixes.Bloodlines);
            MythicAbilities.LoadFixgroup(newFixes.MythicAbilities);
        }

        public class FixGroup {
            public bool DisableAllFixes = false;
            public SortedDictionary<string, bool> Fixes = new SortedDictionary<string, bool>();
            public void LoadFixgroup(FixGroup group) {
                DisableAllFixes = group.DisableAllFixes;
                group.Fixes.ForEach(entry => { 
                    if (Fixes.ContainsKey(entry.Key)) {
                        Fixes[entry.Key] = entry.Value;
                    }
                });
            }
        }
    }
}
