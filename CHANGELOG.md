## Version 1.10.4
* Fix for trick riding not working properly in all cases
* 

## Version 1.10.3
* Haramaki now counts as light armor for effects that depend on it (can still be equiped without proficency)
* Selective metamagic now requires 10 ranks of knowledge arcana
* Spells that are not valid for selective metamagic can no longer be made selective
* You can no longer full attack after moving while mounted
* Lunge is now selectable as a feat
    * You can increase the reach of your melee attacks by 5 feet until the end of your turn by taking a –2 penalty to your AC until your next turn. You must decide to use this ability before any attacks are made.
* Added Lunging Spell Touch Feat
    * You can increase the reach of your spells’ melee touch attacks by 5 feet until the end of your turn by taking a –2 penalty to your AC until your next turn. You must decide to use this ability before you attempt any attacks on your turn.
* Added Mounted Skirmisher Feat
    * If your mount moves its speed or less, you can still take a full-attack action.
* Added Horse Master Feat
    * Use your character level to determine your effective druid level for determining the powers and abilities of your mount.

## Version 1.10.2
* Fixed some incorrect ability types for Black Blade
* Added Undersized Mount
* Added Trick Riding
* Cleanups for dynamic item naming
* Magus now gets its missing "Burst" enchantments
* Activatabilies should now correctly turn off when they run out of charges
* Half of the Pair should now more accurately update with range changes
* Phantasmal Mage metamagic should now work correctly with shadow spells
* Minor fixes to Zippy magic tweaks
* Crossblooded sorcerer now takes the missing -2 will save penalty from tabletop

## Version 1.10.1
* Chinese localization from @1onepower
* Fixes to Warrior Spirit save loading issues
* Halfing alternate racial trait blessed now removes the correct trait
* Added Blade Bound Magus archetype
    * A Magus archetype which sacrifices some of thier arcane pool for a unique weapon that scales along side them

## Version 1.10.0
* Added homebrew flags in the settings
* Fixed minor issues with AddFacts
* Added Quick Draw
* Celestial Bloodline now works correctly with Metamagic Rager
* Adjusted Magical Tail Feats @Balkoth
* Kitsune polymorph is now correctly considered a polymorph @Balkoth
* Updated string handling to support general localization (will need help from speakers to do actual localization work)
* Shadow Enchantment should now work correctly
* Added support for dynamic item naming

## Version 1.9.6b
* Aspect of Omox DR now stacks globaly
* TabletopTweaks should now work again on non English langagues

## Version 1.9.6a
* Fixed Arcane Bloodrager spell effects CL for mixed blood bloodrager

## Version 1.9.6
* Rebuilt Metamagic rager to be less janky
* Fixed Arcane Bloodrager polymorph effects breaking turn based mode

## Version 1.9.5
* Loremaster now supports Hunter, Skald, and Warpriest
* Terrifying Tremble's on kill effect now works

## Version 1.9.4
* Updated handling of nested activatable abilities
* Fixed issue where additional spell selections from spell blending/loremaster were not actually applying
* Fixed unarmed strikes not being valid for weapon training
* Close to the Abyss now correctly deals 1.5 strength damage instead of 0.5
* Steel Headbutt should now more correctly use armor enchantment values
* Aeon Bane is now a swift action instead of a free
* Aeon Gaze is now a free action instead of a swift
* Updated Deity favored weapon mechanics to fix proficiency not being granted properly in some cases

## Version 1.9.3a
* Fixed issue with Oracle curse progression moving at the wrong rate
* Fixed issue with alternate racial features breaking with Destinaty Beyond Birth
* Fixed CL scaling on Burning Magic Revelation

## Version 1.9.3
* Added new modified version of Armor Master as a homebrew option
* Updated icon for Aeon Bane
* Minor fixes to nested activatable abilities
* Aberrant bloodline should now work correctly with Mixed Blood Bloodrager
* Destined bloodline should now work correctly with Mixed Blood Bloodrager

## Version 1.9.2
* Removed the movement impairing descriptor from staggered
* Fixed issue with DR config not working correctly
* Fixed issue where loremaster wouldn't always give you spell from the correct list
* Inherent stat bonuses (like those from stat tomes) now count for feat prerequisites

## Version 1.9.1b
* Fixed issue with Aeon Limitless gaze crashing
* Fixed issue mythic rework setting missing the correct interface

## Version 1.9.1a
* Settings UI fixes

## Version 1.9.1
* Updated for 1.1.0

## Version 1.9.0
* Energy Resistance should now show up when inspecting units
* Settings should no longer break when some other mods are present
* Loremaster should now work properly with spontaneous casters in all cases
* Major update to settings, this is unfortunatly a breaking change
* Settings now have a UMM UI

## Version 1.8.2a
* Fixed broken quick study tooltips
* Nature's Whispers MAY work correctly this time
* Warrior Spirit should now correctly remember which buff you have selected

## Version 1.8.2
* Cleaned up icon rendering
* Added Warrior Spirit advanced weapon training with the following options:
    * Anarchic
    * Axiomatic
    * Holy
    * Unholy
    * Corrosive
    * CorrosiveBurst
    * Flaming
    * FlamingBurst
    * Frost
    * IcyBurst
    * Shock
    * ShockingBurst
    * Thundering
    * Thundering Burst
    * Cruel
    * Keen
    * Bane
    * Ghost Touch
    * Brilliant Energy
    * Speed

## Version 1.8.1
* Aeon Bane no longer grants disadvantage on spell resistance and instead correctly adds mythic rank to spell resistance checks
* Aeon Improved Bane now uses greater dispel magic rules to remove 1/4 CL buffs where CL is defined as Character Level + Mythic Rank
* Aeon Greater Bane now has the garentee'd auto dispel on first hit
* Break Enchantment no longer removes friendly buffs
* Cleaned up fighter armor training speed increase to work more correctly with archetypes

## Version 1.8.0
* Aeon Mythic Rework
    * Aeon Bane is now a free action to activate instead of a swift
    * Aeon Bane usages now scale at 2x Mythic level + Character level
    * Aeon Greater Bane damage is now rolled into the main weapon attack instead of a separate instance
    * Aeon Greater Bane now allows you to cast swift action spells as a move action
    * Aeon Gaze selection is no longer limited on the first selection and all selections are available
    * Aeon Gaze now functions like Inquisitor Judgments where multiple can be activated for the same swift action and resouce usage
    * Aeon Gaze DC has been adjusted from 15 + 2x Mythic Level to 10 + 1/2 Character Level + 2x Mythic level
        * This takes the DC range from DC 21-35 to DC 21-40 (+2 vs Chaotic)
* fixed armor specilization cap being lower than it should have been
* made nature oracle AC conversion more consistant
* advanced armor training should now increase heavy armor movement speed again

## Version 1.7.0
* arcane bloodrage rework (Thanks to @bguns)
* added lecturer backgorund

## Version 1.6.1
* fixed some buffs being dispellable that should not have been
* fixed paladin divine mount template
* fixed rare serailization bug that could prevent saves from loading

## Version 1.6.0a
* actual release version

## Version 1.6.0
* dispel magic greater now correctly has a cap on how many buffs it can remove (1/4 CL)
* added new cleric archetype
    * a fallen cleric archetype that has a unique domain selection, can convert all spells into domain spells, and gets a unique form of channeling that damages ALL things
* added improved channel feat
* added quick channel feat

## Version 1.5.2
* preapplied buffs will now be added at the correct CL, this will result in many buffs being easier to dispell
* tagged more buffs applied by spells as being applied by spells, this means they will now be correctly dispellable
* added celestial servant feat

## Version 1.5.1
* fixed issue with uncanny dodge prerequisites
* fixed utilitarian magic tooltip
* primailist rage powers should all work now with bloodrage (Thanks to @bguns)
* oracle curses should now work correctly (Thanks to @pheonix99)
* nature oracle and scaled fist AC bonus no longer stack

## Version 1.5.0b
* fixed issue where ContextRankConfigs would sometimes calculate incorrectly for classes without an archetype

## Version 1.5.0a
* fixed issue where ContextRankConfigs would sometimes calculate incorrectly if only one archetype was setup

## Version 1.5.0
* added new archetypes (big thanks to @factubsio for their work on these)
    * nature fang
        * A druid/slayer hybrid archetype who trades wild shape for studied target and slayer talents
    * divine commander
        * A warpriest/cavalier hybrid archetype who trades blessings and some bonus feats for a divine mount and the tactician ability
* added erastil's blessing feat (Thanks @bguns)
* added stalwart and improved stalwart feats (Thanks @factubsio)
* fixed issues with ContextRankConfigs giving incorrect progressions in some cases. Unsure of the scope of the bug but things should work more correctly now.


## Version 1.4.1
* favorite metamagic bolstered is now pickable
* fixed cases where it was sometimes impossible to complete levelup due to the DR rework

## Version 1.4.0
* weapon training no longer stacks with itself incorrectly
* greater spell specialization now works more correctly with variant spells
* Damage Reduction (HUGE Thanks to @bguns):
    * Overview: This provides a comprehensive rework of the damage reduction / resistance mechanics in the game. This rework allows for (more) correct stacking of DR, and fixes issues with the interaction between Protection From Energy, energy resistance, and the Abjuration school's Elemental Absorption class feature.
    * Fixed: DR X/- now increases and stacks as per the tabletop rules
        * Changed: Mangling Frenzy item extra rage DR allowed to stack with barbarian(-like) damage reduction
        * Changed: Lich's Indestructible Bones mythic ability allowed to stack with all other sources of DR/-
        * Changed: Azata's The Bound of Possibility item now grants stacking DR/Lawful to match Aivu
        * Changed: Armored Juggernaut (added by TTT) only stacks with adamantine armors, and is increased by Armor Mastery
    * Fixed: interaction between protection from energy, energy resistance, and the abjuration school's energy absorption feature
    * Fixed: Clustered Shots was able to (in theory) overcome force resistance
    * Fixed: Abilities that reduced DR (such as Aeon's Enforcing Gaze with the DR option) also reduced energy resistance(s)
    * Fixed: Skald Damage Reduction was not increased by the Increased Damage Reduction rage power
    * Fixed: Mad Dog's pet's DR did not increase if the Mad Dog took the Increased Damage Reduction rage power
    * Fixed: Bloodrager (Primalist) DR was not increased by the Increased Damage Reduction rage power
    * Fixed: Winter Oracle Ice Armor revelation gave DR 5/- instead of DR 5/piercing
    * Fixed: Bruiser's Chainshirt gave DR 3/- instead of DR 3/piercing
    * Fixed: Warden of Darkness (Tower Shield) gave DR 5/- instead of DR 5/good
    * Fixed: Aivu was gaining DR/- instead of DR/Lawful
    * Fixed: The rage buff from the Mangling Frenzy belt did not apply to Bloodrager's rage


## Version 1.3.7
* fixed issue where settings were not always properly preserved in version updates
* shatter defenses should now work correctly for anything with an attack rolls instead of just weapon attacks
* added greater spell specialization

## Version 1.3.6
* animal ally should now always progress correctly
* fixed for 1.0.7

## Version 1.3.5
* shatter defenses now works like in tabletop
* extra feat mythic feat can now only be taken once
* extra mythic ability mythic feat can now only be taken once
* improvements to natual armor stacking rules
* added mythic shatter defenses
* added nature soul @Aegonek 
* added animal ally @Aegonek 

## Version 1.3.4a
* fixed issue where weapon materials didn't apply correctly
* fixed issues where activiatable abilities with resources spent incorrecty
* fixed issue where vital force was not properly applied to melee attacks

## Version 1.3.4
* added precision critical mythic ability
* rowdy's vital force now deals precision damage
* profane ascension now works correctly for dexterity and strength based characters
* sword saint's perfect critical now correctly costs 2 points of pool instead of 1

## Version 1.3.3
* fixed mounted maniac not triggering
* fixed coup de grace not causing sneak attack damage
* trickster UMD2 now allows you to ignore equipment restrictions as the description states
* bolster should now be allowed on sticky touch spells like shocking grasp
* empower should now be allowed on sticky touch spells like shocking grasp
* maximize should now be allowed on sticky touch spells like shocking grasp
* bolster splash damage should not longer hit you (unless you targeted a friendly)

## Version 1.3.2
* extra slayer talents should now allow all slayers
* coup de grace is now working again (opps)
* The holy symbol of Iomedae no longer turns itself off

## Version 1.3.1
* fixed an edge case with spell blending preventing multiple from being taken in the same level
* divine caster classes should now work as expected with the loremaster class
    * this is NOT retroactive
* the following classes can now progress thier spellbook with loremaster
    * Hunter
    * Warpriest
    * Crossblooded Sorcerer
    * Espionage Expert
    * Nature Mage

## Version 1.3.0
* mythic spell combat now works with UMD3s spellbook
* fixed feat selections missing relevent feats
* fix coup de grace DC scaling (HUGE thanks to @Perunq)
* additional spell selections from feats now happen before normal spell selections

## Version 1.2.4
* fixed issue with broken cantrips with spell combat
* added mythic spell combat

## Version 1.2.3
* spell combat now works with spells that have variants

## Version 1.2.2
* fixed issue where broad study's settings were inverted

## Version 1.2.1
* disabled selective metamagic on non instantaneous effects
* added broad study magus arcana
* eldritch scion can now take bloodline ascendancy 

## Version 1.2.0
* loremaster spell secrets now work
* loremaster prerequisites have been updated
* loremaster no longer allows you to take trickster feats
* loremaster no longer causes you to lose a caster level
* fixed bug where trained throw was not granting damage

## Version 1.1.2
* dragon disciple can now progress with:
    * stigmitized Witch
    * sage sorcerer
    * empyreal sorcerer
    * unlettered arcansist
    * nature mage

## Version 1.1.1
* added spell blending (magus arcana)

## Version 1.1.0
* added extra reservoir
* added extra ki
* added extra hex
* added extra arcanist exploit
* added extra arcana
* added extra rogue talent
* added extra slayer talent
* added extra revelation
* added extra discovery
* added extra mercy

## Version 1.0.9
* added focused weapon advanced weapon training
* eldritch scion can now properly take a second bloodline
* fixed broken fighter weapon training from 1.0.3

## Version 1.0.8
* added zen archer perfect strike upgrade

## Version 1.0.7
* added config options for crusade fixes
* added defensive weapon training
* fixed lich spellbook merging
* fixed mod not loading due to vital strike changes

## Version 1.0.6
* metamagic rods no longer are on by default
* greater magic weapon no longer applies a stacking bonus
* fixed a bug where favorable magic would sometimes not deal 75% damage instead of 50%
* removed arcanist fix
* training grounds no longer sets your crusade units damage to 10% instead of 110%

## Version 1.0.5
* fixed race config not presisting on mod load

## Version 1.0.4
* azata songs can now be activated outside of combat
* fixed feats config not presisting on mod load
* fixed dragon kind 1 and 2 not working

## Version 1.0.3
* enabled persistant metamagic on spells

## Version 1.0.2
* updated arcane reservoir fix to be self healing
* fixed issue where new Arcanist exploits were not available to exploiter wizard

## Version 1.0.1
* fixed arcane reservoir sometimes removing too many points on rest
* fixed second breath

## Version 1.0.0
* updated for release
* skill points now properly increase with permanant bonuses
* improved Description tagging
* improved quick study UI
* fixed grenadier archetype features
* fixed overtip UI rolls
* fixed maxinimiz/empower stacking rules
* fixed slippry mind not updating properly
* fixed magicians ring DC bonus
* fixed some damage stacking rules to reduce multiple damage procs
* fixed pentamic faith prerequisities
* reworked alternate features to allow multiple selections
* added dervish dance feat
* added graceful athlete feat/rogue talent
* added dwarf stoutheart trait
* added dwarf stoic negotiator trait
* added elf arcane focus trait
* added elf long limbed trait
* added elf moon kissed trait
* added elf vigilance trait
* added gnome keen trait
* added gnome fell magic trait
* added gnome utilitarian magic trait
* added gnome inquisitive trait
* added gnome nosophobia trait
* added halfling blessed trait
* added halfling secretive survivor trait
* added halfling underfoot trait

## Version 0.5.1
* fixed weapon finesse being applied to all weapons with DamageGrace

## Version 0.5.0
* updated for beta3

## Version 0.4.1
* fixed issue where auto metamagic was broken in some cases

## Version 0.4.0
* fixed vital strike crits
* fixed bug where charge bonus damage could sometimes apply too often
* added magical aptitude feat
* added scholar feat
* added self-sufficent feat
* added shingle runner feat
* added street smarts feat
* fixed bloodrager caster level
* added metamagic rager

## Version 0.3.0

* fixed mounted combat
* fixed indomitable mount
* fixed spirited  charge
* fixed cavalier supreme charge 
* fixed cavalier transfixing charge 
* added cavalier mobility
* longspears are now treated as lances for the purposes of mounted bonus damage

## Version 0.2.0

* added one handed weapon toggle
* fixed slashing grace
* fixed fencing grace
* two handed fighter can now take advanced weapon training feats
* two handed fighter training now properly works with fighter training effects
* fighter advanced weapon training now properly respect prerequisites
* added trained grace advanced weapon training
* added trained throw advanced weapon training
* fixed issue with buffs that could sometimes break lighting

## Version 0.1.3

* fixed armor check penalty for armor master mythic abilities

## Version 0.1.2

* Changes to icons for new mythic abilities

## Version 0.1.1

* added mounted maniac mythic ability

## Version 0.1.0

* made mobility skill persist after combat
* trapfinding now adds 1/2 class level to trickery as well as perception for Rogue, Slayer, and Espionage Expert
* fixed bug where things would be selectable illegally in some cases
* fixed rogue talents being able to be picked more than once
* added impossible speed mythic ability
* added armor master mythic abilities
* added armored might mythic ability

## Version 0.0.7

* fixed Consume Spells resource to have a minimum of 1
* limited Shadow Spells Fixes
* enabled Trickster T3 Tricks
* added Shadow Enchantment

## Version 0.0.6

* natural 20s now are always a success when rolled during critical confirmation
* fixed everlasting judgement
* more auto metamagic fixes
* added Homebrew Heritage for Dwarf
* added Homebrew Heritage for Elf
* added Homebrew Heritage for Gnome
* added Homebrew Heritage for Halfling

## Version 0.0.5

* fixed reformed fiend damage reduction to be DR/Evil instead of DR/-
* added arcanist exploit quick study
* added arcanist exploit familiar
* added arcanist exploit metamagic knowledge
* added arcanist exploit item crafting

## Version 0.0.4

* fixed limitless rage not graniting extra temp HP to bloodragers
* fixed boundless healing not applying reach
* fixed temporary hp not updating properly when damage is taken
* enabled activatable abilities like rage to persist after combat if the limitless feature is present
* fixed bug with long arm not properly calculating duration for some classes
* fixed bug with destined and aberrant bloodrage preventing buffs from properly expiring
    

## Version 0.0.3

* fixed rogue - rowdy vital force

## Version 0.0.2

* fixed domain zealot fix
* fixed reformed fiend - hatred against evil
* fixed monk ac bonus stacking with instinctual warrior
* fixed instinctual warrior cunning elusion ac bonus applying while wearing armor
* fixed elemental engine to be selectable
