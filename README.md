## PLEASE DO NOT SUBMIT BUG REPORTS WHILE USING THIS MOD AS THERE ARE SIGNIFICANT CHANGES IF YOU ABSOLUTELY MUST SUBMIT A BUG REPORT PLEASE NOTIFY OWLCAT THAT IT IS IN USE

This mod seeks to adjust the game to be closer to the tabletop Pathfinder ruleset. This is primarily focused on rebuilding spells and fixing improperly implemented rules.

Additionally this mod seeks to add in content that was not included in the base game from the source books.
Once a game is saved with this mod is enabled it will likely require this mod to be present to load so do not remove or disable the mod once enabled. You can however disable any feature of the mod at will without breaking saves.

All fixes and added content are configurable and can be disabled by editing the various settings json files present in the UserSettings folder of the mod. If this folder is no present start your game with the mod enabled and it will generate.

**How to install**

1. Download and install [Unity Mod Manager](https://github.com/newman55/unity-mod-manager), make sure it is at least version 0.23.0
2. Run Unity Mod Manger and set it up to find Wrath of the Righteous
3. Download the Tabletop Tweaks mod
4. Install the mod by dragging the zip file from step 5 into the Unity Mod Manager window under the Mods tab

**FEATURES:**

    Adds One Handed Weapon toggle
    
    Adds The following bloodlines:
        Aberrant Sorcerer Bloodline
        Destined Sorcerer Bloodline
        Aberrant Bloodrager Bloodline
        Destined Bloodrager Bloodline
    
    Adds the following spells:
        Long Arm
        Shadow Enchantment
        Shadow Enchantment Greater
    
    Adds the following mythic abilities:
        Impossible Speed
        Armor Master
        Armored Might
        Mounted Maniac
        Mythic Spell Combat
        Precision Critical

    Adds the following mythic feats:
        Shatter Defenses (Mythic)
    
    Adds the following arcanist exploits:
        Quick Study
        Familiar
        Metamagic Knowledge
        Item Crafting
    
    Adds the following magus arcana:
        Broad Study
        Spell Blending

    Adds the following advanced weapon trainings:
        Trained Grace
        Trained Throw
        Defensive Weapon Training
        Focused Weapon
        Warrior Spirit

    Adds the following advanced armor trainings:
        Armored Confidence
        Armored Juggernaut
        Armor Specialization
        Critical Deflection
        Steel Headbut
        
    Adds the following archetypes:
        Bloodrager - Metamagic Rager
        Warpriest - Divine Commander
        Druid - Nature Fang
        Cleric - Channeler of the Unknown
        Magus - Bladebound

    Adds the following feats:
        Dervish Dance
        Graceful Athlete
        Magical Aptitude
        Scholar
        Self-Sufficent
        Shingle Runner
        Street Smarts
        Extra Reservoir
        Extra Ki
        Extra Hex
        Extra Arcanist Exploit
        Extra Arcana
        Extra Rogue Talent
        Extra Slayer Talent
        Extra Revelation
        Extra Discovery
        Extra Mercy
        Nature Soul
        Animal Ally
        Greater Spell Specialization
        Erastil's Blessing
        Stalwart
        Improved Stalwart
        Celestial Servant
        Quick Draw
        Undersized Mount
        Trick Riding

    Adds the following rogue talents:
        Graceful Athlete

    Adds the following racial traits:
        added dwarf stoutheart trait
        added dwarf stoic negotiator trait
        added elf arcane focus trait
        added elf long limbed trait
        added elf moon kissed trait
        added elf vigilance trait
        added gnome keen trait
        added gnome fell magic trait
        added gnome utilitarian magic trait
        added gnome inquisitive trait
        added gnome nosophobia trait
        added halfling blessed trait
        added halfling secretive survivor trait
        added halfling underfoot trait

    Adds the following backgrounds:
        Lecturer

    Enables the following data mined content:
        Cauldron Witch Archetype
        Elemental Master Archetype
    
    Contains the following changes/fixes:
        Disables Natural Armor Stacking
        Disables Polymorph Stacking
        Disables Canny Defense Stacking
        Disables Monk AC Stacking with other untyped of same source
        Enables activatable abilities like rage to persist after combat if the limitless feature is present
        fixes temporary hp not updating properly when damage is taken
        Natural 20s now are always a success when rolled during critical confirmation
        Limited fixes to shadow spells
        Longspears now fill the roll of lances for mounted bonus damage
        Vital Strike no longer crits incorrectly
        Empower and Maximize no longer stack incorrectly
        Skills points properly increase from permanant bonuses
        Selective now only works on instaneous effects
        Coup De Grace now scales properly (Thanks @Perunq)
        Class specific feat selections should now have the correct feats
        The Holy Symbol of Iomedae no longer turns itself off
        Profane ascention now works corectly for dexterity and strength based characters
        DR Rules now more closely follow the tabletop rules
        Favorite Metamagic Bolster can now be picked

    Reworks the following Mythic Paths:
        Aeon
            Aeon Bane usages now scale at 2x Mythic level + Character level
            Aeon Bane no longer grants disadvantage on spell resistance and instead correctly adds mythic rank to spell resistance checks
            Aeon Improved Bane now uses greater dispel magic rules to remove 1/4 CL buffs (where CL is defined as Character Level + Mythic Rank) instead of dispeling all buffs
            Aeon Greater Bane damage is now rolled into the main weapon attack instead of a separate instance
            Aeon Greater Bane now auto dispels one buff on the first hit
            Aeon Greater Bane now allows you to cast swift action spells as a move action
            Aeon Gaze selection is no longer limited on the first selection and all selections are available
            Aeon Gaze now functions like Inquisitor Judgments where multiple can be activated for the same resouce usage
            Aeon Gaze DC has been adjusted from 15 + 2x Mythic Level to 10 + 1/2 Character Level + 2x Mythic level
                This takes the DC range from DC 21-35 to DC 21-40 (+2 vs Chaotic)
        Azata
            Performance usages now scale at Mythic Level + Character Level
            Favorable Magic now works on reoccurring saves
            Azata songs can now be started outside of combat

    Alchemist
        Grenadier
            No longer gets brew potions
            No longer gets poision resistance
    Arcansit
        Fixed consume spells minimum resources
    Bloodrager
        Fixes abyssal bulk blood rage feature
        Fixes spells per day to prevent premature qualification of prerequisites
        Fixed limitless rage not granting extra temp hp
        Rebuilt Arcane Bloodrage to work better
        Primalist
            Prevents qualification of Extra Rage power feat
            All Rage powers should now work
        Reformed Fiend
            Fixes Hatred Against Evil
    Cavalier
        Adds cavalier mobility feature
        Fixes base mount selection
        Fixes supreme charge
        Gendarme
            Fixes transfixing charge
    Dragon Disciple
        Spellbooks now work:
            Stigmitized Witch
            Sage Sorcerer
            Empyreal Sorcerer
            Unlettered Arcansist
            Bature Mage
    Fighter
        Advanced Weapon Training Feat now properly repects prerequisites
        Two Handed Weapon Training now works with all Weapon Training effects
        Two Handed Fighter
            Can now take Advanced Weapon Training feats
    Hellknight
        Pentamic Faith now requires the Godclaw Order instead of the Diety
    Kineticist
        Elemental Engine
            Fixes bug preventing this from being selectable
    Lich
        Enabled merging for exploiter wizard and nature mage
    Loremaster
        Spell Secrets now work
        Prerequisites have been fixed
        Trickster feats removed from selection
        No longer causes you to lose a caster level
        The following classes can now progress thier spellbook with loremaster
            Hunter
            Warpriest
            Crossblooded Sorcerer
            Espionage Expert
            Nature Mage
    Magus
        Spell combat now works with abilities that have variants
        Burst enchantments are now available
        Sword Saint
            Perfect Critical now correctly costs 2 points of pool instead of 1
    Monk
        Zen Archer
            Perfect Strike now upgrades at level 10
    Oracle
        Natures Whispers no longer stacks with Scaled Fist
        Curses should no progress at the correct rate
    Ranger
        Reverts favorable enemy outsider to work on all outsiders
        Espionage Expert
            Trapfinding now addes 1/2 class level to trickery as well as perception
    Rogue
        Fixes rogue talents being able to be picked more than once
        Trapfinding now addes 1/2 class level to trickery as well as perception
        Slippery mind now properly updates on stat change
        Eldritch Scoundrel
            Fixes sneak attack progression
    Slayer
        Fixes studied target bonus
        Trapfinding now addes 1/2 class level to trickery as well as perception
    Sorcerer
        Crossblooded
            Now takes the missing -2 will save penalty
    Witch
        Fixes hex casting stat for:
            Evil eye
        Fixes hex descriptors for
            Death Curse
            Delicious Fright
            Evil Eye
            Hoarfrost
            Restless Slumber
    Spells
        Updates to better match tabletop versions:
            Angelic Aspect Greater
            Magical Vestment
        Fixes:
            Believe in Yourself
            Bestow Curse Greater
            Break Enchantment
            Ode to Miraculous Magic
            Crusader's Edge
            Second Breath
    Bloodlines
        Fixes bloodline prerequisites to prevent impossible stacking
        Fixed dragon disciple bloodline rules to prevent impossible stacking
        Enabled dragonhier scion progression with dragon disciple
    Feats
        Crane wing now requires a free hand
        Slashing grace now always requires a free hand
        Fencing grace now always requires a free hand
        Mounted Combat now works properly
        Indomitable Mount now works properly
        Spirited Charage now works properly
        Persistant metatmagic can now be applied to spells
        Empower metatmagic can now be applied to touch spells
        Maximize metatmagic can now be applied to touch spells
        Bolster metatmagic can now be applied to touch spells
        Bolster metatmagic splash damage should not longer hit you (unless you targeted a friendly)
    Mythic Abilities
        Bloodline Ascendance now works with mutated bloodlines
        Enduring spells now works with temporary item enhancement spells
        Second bloodline is now available to mutated bloodlines
        Second bloodrager bloodline now works with reformed fiend

Acknowledgments:  

-   Pathfinder Wrath of The Righteous Discord channel members
-   @Balkoth, @Narria, @edoipi, @SpaceHamster and the rest of our great Discord modding community - help.
-   PS: Spacehamster's [Modding Wiki](https://github.com/spacehamster/OwlcatModdingWiki/wiki/Beginner-Guide) is an excellent place to start if you want to start modding on your own.
-   Join our [Discord](https://discord.gg/bQVwsP7cky)
