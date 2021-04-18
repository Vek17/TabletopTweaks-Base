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
        public ClassGroup Bloodrager = new ClassGroup();
        public ClassGroup Rogue = new ClassGroup();
        public ClassGroup Slayer = new ClassGroup();
        public ClassGroup Witch = new ClassGroup();
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

            Bloodrager.LoadClassGroup(newFixes.Bloodrager);
            Rogue.LoadClassGroup(newFixes.Rogue);
            Slayer.LoadClassGroup(newFixes.Slayer);
            Witch.LoadClassGroup(newFixes.Witch);

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

        public class ClassGroup {
            public bool DisableAllFixes = false;
            public FixGroup Base = new FixGroup();
            public SortedDictionary<string, FixGroup> Archetypes = new SortedDictionary<string, FixGroup>();
            public void LoadClassGroup(ClassGroup group) {
                DisableAllFixes = group.DisableAllFixes;
                Base.LoadFixgroup(group.Base);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key)) {
                        Archetypes[entry.Key].LoadFixgroup(entry.Value);
                    }
                });
            }
        }
    }
}
