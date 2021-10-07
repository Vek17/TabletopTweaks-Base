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
