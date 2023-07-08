## [![Download zip](https://custom-icon-badges.herokuapp.com/badge/-Download-blue?style=for-the-badge&logo=download&logoColor=white "Download zip")](https://github.com/Vek17/TabletopTweaks-Base/releases/latest/download/TabletopTweaks-Base.zip) Latest Release 

## This is a TabletopTweaks mod and requires [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core/releases) [![Download zip](https://custom-icon-badges.herokuapp.com/badge/-Download-blue?style=for-the-badge&logo=download&logoColor=white "Download zip")](https://github.com/Vek17/TabletopTweaks-Core/releases/latest/download/TabletopTweaks-Core.zip)

This module provides tabletop based fixes and new content to Wrath of the Righteous. 

Once a game is saved with this mod is enabled it will require this mod to be present to load, so do not remove or disable the mod once enabled. You can however disable any feature of the mod at will without breaking saves.

All changes are configurable and can be disabled via the unity mod manager menu.

**How to install**

1. Download and install [Unity Mod Manager](https://github.com/newman55/unity-mod-manager), make sure it is at least version.
2. Run Unity Mod Manager and set it up to find Wrath of the Righteous.
3. Download the [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core/releases/latest) mod from the releases page.
4. Download the [TabletopTweaks-Base](https://github.com/Vek17/TabletopTweaks-Base/releases/latest) mod from the releases page.
5. Install the mods by dragging the zip file from step 3 & 4 into the Unity Mod Manager window under the Mods tab.

## Fixes
* UI Fixes
    * Dynamic Item Naming
        * Allows Weapon/Armor names to be modified by any additional enchants they gain outside of their base enchants.
    * Saving Throw Overtips
        * Overhead saving throw rolls now display the correct values
    * Spell Tooltips
        * Updates spell tooltips to include what spellbook the spell is in.
    * Suppressed Buffs
        * Suppressed buffs now have custom UI rules to better indicate them.
* Bases Fixes
    * Area of Effects
        * Area of Effects should no longer double trigger on cast as if an extra round had elapsed.
    * Coup De Grace
        * Coup De Grace saving throw DC is now based on damage dealt.
    * Damage Reduction
        * Prevents most forms of damage reduction from stacking unless specifically specified.
    * Damage Vulnerability
        * Now works properly on combined damage types (IE: Slashing/Bludgeoning)
    * Unlimited Abilities Combat Behavior
        * Allows abilities that have become unlimited to remain on after combat ends.
    * Natural Armor
        * Prevents Natural Armor bonuses from stacking unless they are specified to stack. These use normal descriptor rules still so a natural armor bonus and a natural armor enhancement bonus will still stack.
    * Polymorph Effects
        * Prevents multiple polymorph effects from being applied to a character at a time.
        * Active Polymorph effects will correctly suppress size effects from non polymorph spells.
    * Size Effects
        * Prevents multiple size changing buffs granting benefits at the same time. Old buffs are suppressed.
    * Size Limits
        * Size shifts are now supported from Fine to Colossal.
    * Feat Selections
        * Cleans up limited feat selections (like fighter combat feats) to include all feats of the specified type.
    * Background Modifiers
        * Changes the skill and attack bonuses granted from backgrounds into trait bonuses.
    * Critical Confirmation
        * Allows a natural 20 to always confirm a critical.
    * Inherent Stat Bonuses
        * Allows Inherent bonuses to intelligence to now grant bonus skill points.
    * Mounted Longspear
        * Spears now grant additional damage during a mounted charge in the same manor a lance would.
    * Enemy Buff CL
        * Enemy buffs now have the correct CL for applied buffs as defined on their blueprints.
        * Prebuffs will be applied at a more correct level instead of nearly always CL 20.
    * Shadow Spells
        * Shadow spells now are correctly treated as being from the Illusion school for all effects.
    * Mounted Movement
        * Prevents full round actions after moving while mounted.
    * Nauseated Condition
        * Removes the poison descriptor from nauseated.
    * Staggered Condition
        * Removes the movement impairing descriptor from staggered.
    * Invisibility
        * Invisibility now properly grants concealment.
    * Loading
        * Saves will now properly remember who cast buffs which should fix several bugs including idealize discovery not working on save load.
    * Scrolls
        * Scoll UMD DCs are now calculated correctly at 20 + Scroll CL instead of 20 + Scroll spell level.
    * Progressions
        * Progressions no longer display features you will not get.

* Spells
    * Spell flags
        * Retags buffs from spells as coming from spells to allow them to be dispelled correctly.
    * Absolute Order
        * Now has missing compulsion descriptor.
    * Abyssal Storm 
        * Abyssal Storm no longer saves for half and no longer kills the caster.
    * Acid Maw 
        * Acid Maw no longer causes excessive damage instances to trigger when attacking.
    * Animal Growth
        * Animal Growth should now work correctly with animal companions.
    * Believe In Yourself
        * Believe in yourself now grants the correct bonus amount. Now correctly has CL scale buff duration.
    * Bestow Greater Curse
        * Bestow Greater Curse now actually bestows a greater curse not a normal curse.
    * Break Enchantment
        * Break Enchantment no longer affects friendly buffs.
    * Burst of Sonic Energy
        * Now applies the correct amount of damage instead of triggering chaotic healing.
    * Chain Lightning
        * Chain Lightning now respect the 20 CL max for damage dice.
    * Command
        * Now has missing compulsion descriptor.
    * Greater Command
        * Now has missing compulsion descriptor.
    * Corrupt Magic
        * Corrupt Magic now uses a single dispel roll for all buffs like other dispels.
    * Crusader's Edge
        * Crusaders Edge's nauseate effect now only procs on critical hits.
    * Death Ward
        * Death Ward now suppresses existing negative levels.
    * Dispel Magic Greater
        * Greater Dispel Magic now only removes 1/4 CL buffs instead of all buffs.
    * Eye Of The Sun
        * Eye Of The Sun now deals the correct amount of damage.
    * Field of Flowers
        * Now applies the correct debuffs instead of using Repulsive Nature's debuffs.
    * Firebrand
        * Firebrand no longer causes excessive damage instances to trigger when attacking.
    * Friendly Hug
        * Now has fully working immunities.
    * Geniekind
        * Geniekind no longer causes excessive damage instances to trigger when attacking.
    * Greater Magic Weapon
        * Greater Magic Weapon no longer stacks with existing enhancement bonuses.
    * Hellfire Ray
        * Hellfire Ray no longer has the Fire descriptor.
    * Joy of Life
        * Now converts outgoing damage to Holy.
    * Legendary Proportions
        * Now only increases size by one step instead of two.
    * Magical Vestment
        * Magical Vestment now enhances your armor instead of granting a floating modifier.
    * Microscopic Proportions 
        * Microscopic Proportions now correctly grants a size bonus instead of an untyped bonus.
    * Mind Blank
        * Mind blank now makes you immune to detection with divination effects like see invisibility and true seeing.
    * Nature's Grasp
        * Now deals the correct amount of damage.
    * Ode to Miraculous Magic
        * Now correctly has CL scale buff duration.
    * Power From Death
        * Now correctly lasts rounds per level instead of minutes per level.
    * Protection of Nature
        * Concealment no longer is bypassed by true seeing.
    * Remove Fear
        * Remove Fear no longer grants immunity to shaken and fear.
    * Remove Sickness
        * Remove Sickness no longer grants immunity to sickness and nausea.
    * Repulsive Nature
        * Now uses the correct spell DC and actually applies its debuffs on fail.
    * Shadow Conjuration Greater 
        * Now has the correct shadow factor of 60 instead of 40.
    * Shadow Evocation
        * Shadow Evocation can now have the correct metamagics applied.
    * Songs of Steel
        * No longer procs extra spell effects from the spell damage, deals the bonus damage based on CL, and applies only on first hit.
    * Greater Shadow Evocation
        * Greater Shadow Evocation can now have the correct metamagic applied.
        * Now has the correct shadow factor of 60 instead of 40.
    * Starlight
        * Starlight no longer is affected by true sight.
    * Sudden Squall
        * Now has working targeting and debuff.
    * Sun Form
        * Sun Form now deals the correct amount of damage.
    * Unbreakable Bond
        * Now has fully working immunities.
    * Unbreakable Heart
        * Unbreakable Heart no longer grants complete immunity to confusion and emotion effects and instead suppresses correctly.
    * Water Push
        * Now has working targeting and effects.
    * Water Torrent
        * Now has working targeting and debuff.
    * Wracking Ray
        * Wracking Ray now deals the correct amount of ability damage.
    * Vampiric Blade
        * Vampiric Blade no longer triggers extra fake attacks.
    * Zero State
        * Zero State now uses a single dispel roll for all buffs like other dispels.

* Feats
    * Allied Spellcaster
        * Allied Spellcaster no longer applies globally.
    * Arcane Strike 
        * Arcane Strike no longer causes too many damage instances when used by a dragonheir scion.
    * Brew Potions
        * Brew Potions is no longer tagged as a combat feat.
    * Bolstered Metamagic
        * Sticky touch spells can now be bolstered.
    * Cleave
        * Cleave now checks for adjacency to the last target (Adjacent = within 5ft) instead of just reach.
    * Cleaving Finish
        * Cleaving Finish will no longer randomly stop after three targets.
    * Cleaving Finish
        * Cleaving Finish now checks for adjacency to the last target (Adjacent = within 5ft) instead of just reach.
    * Empower Metamagic
        * Sticky touch spells can now be empowered.
        * Prevents extra dice from empowered metamagic from being maximized by maximize metamagic.
    * Bolster Metamagic
        * No longer applies twice on spells with pre rolled values.
    * Maximize Metamagic
        * Sticky touch spells can now be maximized.
    * Persistent Metamagic
        * Allows any spell with a saving throw to be made persistent.
    * Selective Metamagic
        * Retags selective spells to exclude non instantaneous spells.
        * Now requires 10 ranks of knowledge arcana.
    * Crane Wing
        * Now requires a free hand to receive the bonuses.
    * Destructive Dispel
        * Now calculates the DC based on the effective CL of the dispel and the highest mental stat to better support edge cases. Formula is 10 + 1/2 CL + Highest Mental Stat.
    * Endurance
        * Endurance now grants +4 Athletics if you have 10 ranks in Athletics like similar feats.
    * Fencing Grace
        * Fixed an edge case that sometimes allowed fencing grace from applying to two handed weapons.
    * IndomitableMount
        * Now works correctly
    * MagicalTail
        * Magical Tail gives Hideous Laughter at 2 and Heroism at 5 instead of sleep spells.
    * Mounted Combat
        * Now works correctly
    * Outflank
        * Outflank should no longer triggers on missed attacks.
    * ShatterDefenses
        * Now requires you to hit a shaken target once before they become flat footed.
    * Sieze the Moment
        * Sieze the moment should no longer trigger on missed attacks.
    * SlashingGrace
        * Fixed an edge case that sometimes allowed slashing grace from applying to two handed weapons.
    * Spell Specialization
        * Enables spell specialization selection on all levelups.
    * Spirited Charge
        * Bonus damage no longer can crit.
    * Weapon Finesse
        * No longer treats any weapon with Fencing/Slashing grace into a finesse weapon.

* Mythic Feats
    * Expanded Arsenal
        * No longer allows stacking multiple spell focuses on the same school to increase DC, you can only benefit from spell focus once.
    * Extra Feat
        * Can no longer be picked more than once.
    * Extra Mythic Ability
        * Can no longer be picked more than once.

* Mythic Abilities
    * Bloodline Ascendance
        * All bloodline should now qualify including mutated ones.
    * Close To The Abyss
        * Fixes the magic gore's damage multiplier to be 1.5 instead of 0.5
    * Enduring Spells
        * Now works on equipment enhancing effects like crusader's edge.
    * Second Bloodline
        * All bloodlines now qualify for second bloodline including mutated ones.
    * Second Bloodrager Bloodline
        * Reformed Fiend now qualifies for all bloodlines

* Bloodlines
    * Rebuilt all prerequisites to fix multiple issues with prestige class interactions when multiclassing

* Aeon
    * Aeon Demythication
        * Aeon Demythication should now actually suppress mythic effects.

* Demon
    * Demonic Form Balor
        * Balor transformation now properly gets a vorpal weapon.

* Lich
    * Death Rush
        * Prevents Death Rush from triggering multiple fake attacks and procing weapon effects more often than it should.
    * Spellbook Merging
        * Allows Nature mage to merge with the Lich spellbook.

* Trickster
    * Use Magic Device 2
        * Allows trickster to ignore class/alignment restrictions of equipment with UMD2 trick.

* Alchemist
    * Mutagen
        * Prevents the stacking of mutagens. Only one may be active at a time.
    * Grenadier
        * Removed brew potions from the archetype
        * Removed poison resistance from the archetype
    * Incense Synthesizer
        * Incense Fog now scales correctly if you take expanded area.
        * Thick Fog is no longer illusion based concealment.
        * Sacred Incense no longer bypass all immunities. 

* Arcanist
    * Prepared Spell UI
        * Makes arcanist spellbook behave like a prepared caster not a spontaneous one.

* Barbarian
    * Crippling Blows
        * Allows crippling blows to work when raging.
    * Wrecking Blows
        * Allows crippling blows to work when raging.

* Bloodrager
    * Abyssal Bulk
        * Enables abysal bulk to not dispel existing enlarge person when rage ends.
    * Arcane Bloodrage
        * Completely rebuilds arcane bloodrage with new UI
    * Disruptive Bloodrage
        * Now properly triggers for arcane bloodragers.
    * Caster's Bane
        * Now properly triggers for arcane bloodragers.
    * Spellbook
        * Fixes spell progression to not incorrectly qualify for some features that require casting early.
    * Primalist
        * Enables all rage powers to work with bloodrage.
        * Prevents primalist from qualifying for extra rage powers feat.
    * Reformed Fiend
        * Fixes DR to be the correct DR/Good.
        * Hatred against evil now grants the proper bonus.

* Cavalier
    * Cavalier Mobility
        * Allows Cavalier to ignore their armor check penalty while mounted for mobility skill checks.
    * Mighty Charge
        Allows Cavalier to activate both bull rush and trip at the same time.
    * Mount Selection
        * Allows the Cavalier to select a wolf for a mount if they are of small size.
    * Order of the Star
        * Order of the Star's Calling ability should now properly apply bonuses.
    * Supreme Charge
        * Prevents Supreme Charge damage from criting and moves it into the new charge damage system.
    * Gendarme
        * Prevents Transfixing Charge damage from criting and moves it into the new charge damage system.

* Cleric
    * Glory Domain
        * Glory domain no longer grants an untyped bonus the the raw Charisma stat.

* Fighter
    * Advanced Weapon Training
        * Enforces the weapon training prerequisites properly.
    * Unarmed Weapon Training
        * Makes unarmed correctly count as the close weapon group.
    * Weapon Training
        * Prevents bonuses from multiple weapon training groups from stacking.
    * Two Weapon Fighter
        * Allows two handed fighter to pick advanced weapon training feats.
        * Treats two handed fighter's weapon training as proper weapon training.

* Hunter
    * Divine Hunter
        * Divine Hunter's animal companion no longer gets unlimited smites.

* Magus
    * Arcane Weapon
        * Adds: Flaming Burst, Icy Burst, Shocking Burst enchant options.
    * Spell COmbat
        * Lets spell combat work with spells that have variants like dimension door.
        * Disables spell combat immediately when toggled off instead of having to wait until the next round.
        * Prevents spells that are not in the magus spellbook from working with spell combat.
    * Sword Saint
        * Updates perfect critical's cost to 2 points of arcane pool instead of 1.

* Monk
    * Stunning Fist: Stagger
        * This ability works as Stunning Fist, but it makes the target staggered for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.
    * Stunning Fist: Blind
        * This ability works as Stunning Fist, but it permantly blinds the target on a failed save instead of stunning for 1 round.
    * Stunning Fist: Paralyze
        * This ability works as Stunning Fist, but it paralyzes the target for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.
    * ZenArcher
        * At level 10 a zen archer will roll 3 dice instead of 2 with perfect strike.
    * Scaled Fist
        * Draconic Fury now uses the correct unchained version of the progression isntead of the chained.
        * Fixes stunning fist to use the same version as other monks for compatability.

* Oracle
    * Nature's Oracle
        * Prevents Natures Whispers from stacking with Scaled Fist AC bonus. If scaled fist is present dexterity will be used for AC not charisma.
    * Burning Magic
        * Fixes the CL scaling to be equal to oracle level.

* Paladin
    * Divine Mount
        * Updates the template gained by the paladin mount at level 9 to have all of its features.
    * Smite
        * The attack bonus from Smite Evil/Smite Chaos/Mark of Justice will no longer stack.

* Ranger
    * Favored Enemy
        * Favored enemy outsider now applies to ALL demons. The Demons of X are disabled unless you already have ranks, but otherwise function identically to favored enemy outsider and you can keep picking them for compatibility with existing characters.
    * Espionage Expert
        * Trapfinding now grants bonuses to perception and trickery.

* Rogue
    * Dispelling Attack
        * Dispelling Attack now uses the correct CL and no longer removes debuffs.
    * Rogue Talents
        * Prevents you from selecting the same talent more than once.
    * Trapfinding
        * Trapfinding now grants bonuses to perception and trickery.
    * Eldritch Scoundrel
        * Removes the level 2 rogue talent and adds in a level 4 talent.
        * Removes the sneak attack dice granted at level 1.
    * Sylvan Trickster
        * Fey Tricks now includes all rogue talents

* Shaman
    * Ameliorating Hex 
        * Ameliorating Hex no longer grants complete immunity to effects and instead suppresses correctly.

* Skald
    * Spell Kenning has been added.

* Slayer
    * Trapfinding
        * Trapfinding now grants bonuses to perception and trickery.

* Sorcerer
    * Crossblooded
        * Crossblooded sorcerers now take the -2 will saving throw penalty that they were missing.

* Warpriest
    * Air Blessing
        * Air Major blessing no longer causes excessive damage instances.
    * Earth Blessing
        * Earth Minor blessing no longer causes excessive damage instances.
    * Fire Blessing
        * Fire Minor blessing no longer causes excessive damage instances.
    * Water Blessing
        * Water Minor blessing no longer causes excessive damage instances.
    * Weather Blessing
        * Weather Minor blessing no longer causes excessive damage instances.
    * Luck Blessing
        * Luck Blessing now grants the correct major ability.

* Witch
    * Agility Patron
        * Agility Patron now gets Animal Shapes at 16th level and Shapechange at 18th.
    * Ameliorating Hex 
        * Ameliorating Hex  no longer grants complete immunity to effects and instead suppresses correctly.
    * Major Ameliorating Hex 
        * Major Ameliorating Hex no longer grants complete immunity to effects and instead suppresses correctly.

* Hellknight
    * Pentamic Faith
        * Pentamic Faith now requires the Godclaw hellknight order not the deity.

* Loremaster
    * Prerequisites
        * Updates the prerequisites to be more strict and better match tabletop.
    * Spell Progression
        * Grants spell progression at level 1. This is not retroactive.
    * Spell Secrets
        * Spell Secrets now work.
    * Trickster Tricks
        * Removes trickster tricks from the combat feat selection.

* Winter Witch
    * Unearthly Cold
        * Direct damage should now properly respect metamagic and added bonus damage.

* Crusade
    * TrainingGrounds
        * Now grants the correct damage bonus.

* Armor
    * Haramaki
        * Haramaki are now counted as light armor properly.
    * Singing Steel
        * Singing Steel now works properly.

* Equipment
    * Amulet of the Asp
        * Now correctly works with Ray attacks.
    * Amulet of Quick Draw
        * No longer creates excessive damage instance and now functions correctly with critical hits.
    * Flawless Belt Of Physical Perfection 8
        * Now increases critical hit range by 1 in all cases.
        * Now appears as a DLC1 reward.
    * Half Of The Pair
        * Will more accurately update the bonus with range.
    * Holy Symbol of Iomedae
        * Will now stay on after saving/loading or changing areas.
    * Magicians Ring
        * Now grants +2 DC instead of +1 DC.
    * Mangling Frenzy
        * Now works with bloodrage.
    * Metamagic Rods
        * Now default to off.

* Weapons
    * Blade Of The Merciful
        * Prevents from triggering multiple fake attacks and procing weapon effects more often than it should.
    * Energy Burst
        * Fixes the critical multiplier calculation of energy burst (like flaming burst) effects to get the correct value.
    * Honorable Judgement
        * Prevents from triggering multiple fake attacks and procing weapon effects more often than it should.
    * Finnean
        * Finnean now always deals the correct amount of damage.
    * Radiance 
        * Now correctly grants spell resistance instead of spell penetrations.
    * Terrifying Tremble
        * Implements the missing on kill effect. Whenever the wielder of this weapon lands a killing blow, he deals sonic damage equal to his ranks in the Athletics skill to all enemies within 10 feet. Successful Reflex save (DC 30) halves the damage.
    * Thundering Burst
        * Fixes thundering burst to deal D10s like the description says instead of D8s.
    * Sound of Void
        * Now correctly removes spell resistance when hitting a flat footed target.
    * Music of Death
        * Now correctly deals bonus damage when hitting a flat footed target.
    * Vorpal
        * Vorpal now works correctly.

## Added Content
* Base Abilities
    * One Handed Weapon toggle

* Archetypes
    * Arcanist - Elemental Master
        * Enables an incomplete archetype from Owlcat.
    * Bloodrager - Metamagic Rager
        * A bloodrager archetype that can spend rage rounds to apply metamagic effects to thier spells.
    * Cleric - Channeler Of The Unknown
        * A fallen cleric archetype that has a unique domain selection, can convert all spells into domain spells, and gets a unique form of channeling that damages ALL things.
    * Magus - Blade Bound
        * A select group of magi are called to carry a black blade-a sentient weapon of often unknown and possibly unknowable purpose. These weapons become valuable tools and allies, as both the magus and weapon typically crave arcane power, but as a black blade becomes more aware, its true motivations manifest, and as does its ability to influence its wielder with its ever-increasing ego.
    * Druid - NatureFang
        * A druid/slayer hybrid archetype who trades wild shape for studied target and slayer talents.
    * Warpriest - Divine Commander
        * A warpriest/cavalier hybrid archetype who trades blessings and some bonus feats for a divine mount and the tactician ability.
    * Witch - Cauldron Witch
        * Enables a complete archetype from Owlcat.
    
* Alternate Capstones
    * Generic Capstones
        * Perfect Body, Flawless Mind
            * The character increases her ability scores by a collective total of 8.
        * Great Beast
            * The animal companion’s Strength, Dexterity, Constitution, and Wisdom scores each increase by 4. This capstone is available to any class with an animal companion.
        * Old Dog, New Tricks
            * The character gains four combat feats. This capstone is available to characters of any class that gains at least four bonus combat feats.
    * Alchemist
        * Vast Explosions
            * The alchemist’s bomb damage increases by 3d6.
    * Arcanist
        * Deep Reservoir
            * Her arcane reservoir increases by 10.
    * Barbarian
        * Unstoppable
            * The barbarian gains DR 3/— or increases the value of any existing damage reduction by 3. In addition, she gains 20 energy resistance to acid, cold, electricity, and fire.
    * Cleric
        * Proxy
            * She can select an additional domain from the list offered by her deity.
    * Fighter
        * Veteran of Endless War
            * The bonuses granted by his armor training and weapon training increase by 2 each.
    * Inquisitor
        * Team Leader
            * As a standard action the inquisitor can spend a standard action granting characters up to three of the inquisitor’s teamwork feats (the inquisitor’s choice) as bonus feats for the next 24 hours.
    * Kineticist
        * Unbridled Power
            * Her damage with her blasts increases by 2d6+2 (for physical blasts) or by 2d6 (for energy blasts).
    * Monk
        * Old Master
            * The monk gains one additional attack at his highest base attack bonus when using flurry of blows, and he gains a dodge bonus to AC of 2.
    * Oracle
        * Diverse Mysteries
            * The oracle can select two revelations from another mystery. She must meet the prerequisites for these revelations.
    * Ranger
        * Seen It Before
            * The ranger adds his favored enemy bonus as an insight bonus on saves against spells and abilities used by his favored enemies.
    * Rogue
        * Masterful Talent
            * The rogue gains a +4 bonus on all of her skills.
    * Skald
        * Great Kenning
            * The skald can use spell kenning three additional times per day and can select one additional spell list from which he can cast spells with spell kenning.
    * Slayer
        * Against the Odds
            * When the slayer uses studied target, he can study up to two additional foes within 30 feet in the same action.
    * Warpriest
        * Hammer of God
            * The warpriest gains two additional blessings from the list offered by his deity. He can also call upon his blessings two more times each day.
    * Wizard
        * Well-Prepared
            * The wizard gains six additional 1st- and 2nd-level spell slots, four additional 3rd- and 4th-level spell slots, two additional 5th-level spell slots, and one additional 6th-level spell slot.


* Spells
    * Cloak of Winds
        * You shroud a creature in a whirling screen of strong, howling wind. Ranged attack rolls against the subject take a -4 penalty.
    * Long Arm
        * Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.
    * Mage's Disjunction
        * All magical effects and magic items within the radius of the spell, except for those that you carry or touch, are disjoined. That is, spells and spell-like effects are unraveled and destroyed completely (ending the effect as a dispel magic spell does), and each permanent magic item must make a successful Will save or be turned into a normal item for the duration of this spell.
    * Shadow Enchantment
        * You use material from the Shadow Plane to cast a quasi-real, illusory version of a psychic, sorcerer, or wizard enchantment spell of 2nd level or lower. Spells that deal damage or have other effects work as normal unless the affected creature succeeds at a Will save. If the disbelieved enchantment spell has a damaging effect, that effect is one-fifth as strong (if applicable) or only 20% likely to occur. If recognized as a shadow enchantment, a damaging spell deals only one-fifth (20%) the normal amount of damage. If the disbelieved attack has a special effect other than damage, that effect is one-fifth as strong (if applicable) or only 20% likely to occur. Regardless of the result of the save to disbelieve, an affected creature is also allowed any save (or spell resistance) that the spell being simulated allows, but the save DC is set according to shadow enchantment's level (3rd) rather than the spell's normal level.
    * Shadow Enchantment Greater
        * This spell functions like shadow enchantment, except that it enables you to create partially real, illusory versions of psychic, sorcerer, or wizard enchantment spells of 5th level or lower. If the spell is recognized as a greater shadow enchantment, it's only three-fifths (60%) as effective.
    * Stunning Barrier Greater
        * This spell functions as stunning barrier, except it provides a +2 bonus to AC and on saving throws and it is not discharged until it has stunned a number of creatures equal to your caster level.

* Metamagic
    * Burning Spell (Metamagic)
        * The acid or fire effects of the affected spell adhere to the creature, causing more damage the next round. When a creature takes acid or fire damage from the affected spell, that creature takes damage equal to 2x the spell’s actual level at the start of its next turn. The damage is acid or fire, as determined by the spell’s descriptor.
    * Encouraging Spell (Metamagic)
		* Any morale bonus granted by an encouraging spell is increased by 1.
    * Flaring Spell (Metamagic)
        * The electricity, fire, or light effects of the affected spell create a flaring that dazzles creatures that take damage from the spell. A flare spell causes a creature that takes fire or electricity damage from the affected spell to become dazzled for a number of rounds equal to the actual level of the spell. A flaring spell only affects spells with a fire, light, or electricity descriptor.
    * Intensified Spell (Metamagic)
        * An intensified spell increases the maximum number of damage dice by 5 levels. You must actually have sufficient caster levels to surpass the maximum in order to benefit from this feat. No other variables of the spell are affected, and spells that inflict damage that is not modified by caster level are not affected by this feat.
    * Piercing Spell (Metamagic)
        * When you cast a piercing spell against a target with spell resistance, it treats the spell resistance of the target as 5 lower than its actual SR.
    * Rime Spell (Metamagic)
        * The frost of your cold spell clings to the target, impeding it for a short time. A rime spell causes creatures that takes cold damage from the spell to become entangled for a number of rounds equal to the original level of the spell.
    * Solid Shadows (Metamagic)
        * When casting a shadow spell, that spell is 20% more real than normal.
    * Elemental Spell (Metamagic)
        * Choose one energy type: acid, cold, electricity, or fire. You may replace a spell’s normal damage with that energy type "or split the spell’s damage, so that half is of that energy type and half is of its normal type.

* Feats
    * Ability Focus - Stunning Fist
        * Add +2 to the DC for all saving throws against your stunning fist.
    * Animal Ally
        * You gain an animal companion as if you were a druid of your character level -3. Unlike normal animals of its kind, an animal companion's Hit Dice, abilities, skills, and feats advance as you advance in level.
    * Celestial Servant
        * Your animal companion, familiar, or mount gains the celestial template and becomes a magical beast, though you may still treat it as an animal when using Handle Animal, wild empathy, or any other spells or class abilities that specifically affect animals.
    * Dervish Dance
        * When wielding a scimitar with one hand, you can use your Dexterity modifier instead of your Strength modifier on melee attack and damage rolls.
    * Dispel Focus
        * Whenever you attempt a dispel check based on your caster level, you gain a +2 bonus to the check.
    * Greater Dispel Focus
        * Whenever you attempt a dispel check based on your caster level, you gain a +2 bonus to the check. This stacks with the bonus from Dispel Focus.
    * Erastil's Blessing
        * You can use your Wisdom modifier instead of your Dexterity modifier on ranged attack rolls when using a bow.
    * Expanded Spell Kenning
        * When you use your spell kenning class feature, you can select a spell from either the druid or the witch spell list.
    * Extra Arcana
        * You gain one additional magus arcana. You must meet all the prerequisites for this magus arcana.
    * Extra Arcanist Exploit
        * You gain one additional arcanist exploit. You must meet the prerequisites for this arcanist exploit.
    * Extra Discovery
        * You gain one additional discovery. You must meet all of the prerequisites for this discovery.
    * Extra Hex
        * You gain one additional hex. You must meet the prerequisites for this hex.
    * Extra Ki
        * Your ki pool increases by 2.
    * Extra Mercy
        * Select one additional mercy for which you qualify. When you use lay on hands to heal damage to one target, it also receives the additional effects of this mercy.
    * Extra Reservoir
        * You gain 3 more points in your arcane reservoir, and the maximum number of points in your arcane reservoir increases by that amount.
    * Extra Revelation
        * You gain one additional revelation. You must meet all of the prerequisites for this revelation.
    * Extra Rogue Talent
        * You gain one additional rogue talent. You must meet all of the prerequisites for this rogue talent.
    * Extra Slayer Talent
        * You gain one additional slayer talent. You must meet the prerequisites for this slayer talent.
    * Graceful Athlete
        * Add your Dexterity modifier instead of your Strength bonus to Athletics checks.
    * Improved Channel
        * Add 2 to the DC of saving throws made to resist the effects of your channel energy ability.
    * Lunge
        * You can increase the reach of your melee attacks by 5 feet until the end of your turn by taking a –2 penalty to your AC until your next turn.
    * Lunging Spell Touch
        * You can increase the reach of your spells’ melee touch attacks by 5 feet until the end of your turn by taking a –2 penalty to your AC until your next turn. You must decide to use this ability before you attempt any attacks on your turn.
    * Magical Aptitude
        * You get a +2 bonus on Knowledge (Arcana) and Use Magic Device skill checks. If you have 10 or more ranks in one of these skills, the bonus increases to +4 for that skill.
    * Mantis Style
        * You gain one additional Stunning Fist attempt per day. While using this style, you gain a +2 bonus to the DC of effects you deliver with your Stunning Fist.
    * Mantis Wisdom
        * Treat half your levels in classes other than monk as monk levels for determining effects you can apply to a target of your Stunning Fist per the Stunning Fist monk class feature. While using Mantis Style, you gain a +2 bonus on unarmed attack rolls with which you are using Stunning Fist attempts.
    * Mantis Torment
        * While using Mantis Style, you make an unarmed attack that expends two daily attempts of your Stunning Fist. If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.
    * Mounted Skirmisher
        * If your mount moves its speed or less, you can still take a full-attack action.
    * Nature Soul
        * You get a +2 bonus on all Lore (Nature) checks and Perception checks. If you have 10 or more ranks in one of these skills, the bonus increases to +4 for that skill.
    * Two-Weapon Defense
        * When wielding a double weapon or two weapons (not including natural weapons or unarmed strikes), you gain a +1 shield bonus to your AC. When you are fighting defensively this shield bonus increases to +2.
    * Quick Channel
        * You may channel energy as a move action by spending 2 daily uses of that ability.
    * Quick Draw
        * You can draw a weapon as a free action instead of as a move action.
    * Quicken Blessing
        * Choose one of your blessings that normally requires a standard action to use. You can expend two of your daily uses of blessings to deliver that blessing (regardless of whether it’s a minor or major effect) as a swift action instead.
    * Riving Strike
        * If you have a weapon that is augmented by your Arcane Strike feat, when you damage a creature with an attack made with that weapon, that creature takes a –2 penalty on saving throws against spells and spell-like abilities. This effect lasts for 1 round.
    * Scholar
        * You get a +2 bonus on Knowledge (Arcana) and Knowledge (World) skill checks. If you have 10 or more ranks in one of these skills, the bonus increases to +4 for that skill.
    * Self-Sufficient
        * You get a +2 bonus on Lore (Nature) and Lore (Religion) skill checks. If you have 10 or more ranks in one of these skills, the bonus increases to +4 for that skill.
    * Shingle Runner
        * You get a +2 bonus on Athletics and Mobility skill checks. If you have 10 or more ranks in one of these skills, the bonus increases to +4 for that skill.
    * Greater Spell Specialization
        * By sacrificing a prepared spell of the same or higher level than your specialized spell, you may spontaneously cast your specialized spell. The specialized spell is treated as its normal level, regardless of the spell slot used to cast it.
    * Stalwart
        * While fighting defensively or using Combat Expertise, you can forgo the dodge bonus to AC you would normally gain to instead gain an equivalent amount of DR, to a maximum of DR 5/', until the start of your next turn.
    * Improved Stalwart
        * Double the DR you gain from Stalwart, to a maximum of DR 10/-.
    * Trick Riding
        * You can make a check using Mounted Combat to negate a hit on your mount twice per round instead of just once.
    * Undersized Mount
        * You can ride creatures equal to your own size category instead of only creatures larger than you.
    * Varisian Tattoo
        * Select a school of magic in which you have Spell Focus. Spells from this school are cast at +1 caster level.

* Armor Mastery Feats
    * Intense Blows
		* When wearing heavy armor and using Power Attack, you gain a +1 bonus to your CMD until the beginning of your next turn. When your base attack bonus reaches +4, and every 4 points thereafter, this bonus increases by another 1.
	* Knocking Blows
		* While wearing heavy armor if you hit a creature that is no more than one size category larger than you with a Power Attack, the creature you attacked is also knocked off balance. Until the beginning of your next turn, it takes a –4 penalty to its CMD against combat maneuvers that move it or knock it prone.
	* Secured Armor
		* When you are hit by a confirmed critical hit or a sneak attack while wearing medium or heavy armor, there is a 25% chance that the critical hit or sneak attack is negated and damage is instead rolled normally. Special: This chance stacks with the light fortification and moderate fortification armor special abilities.
	* Sprightly Armor
		* While wearing light armor you add your armor’s enhancement bonus as a bonus on your initiative checks.

* Shield Mastery Feats
    * Defended Movement
		* You gain a +2 bonus to your AC against attacks of opportunity.
	* Stumbling Bash
		* Creatures struck by your shield bash take a –2 penalty to their AC until the end of your next turn.
	* Toppling Bash
		* As a swift action when you hit a creature with a shield bash, you can attempt a trip combat maneuver against that creature at a –5 penalty. This does not provoke an attack of opportunity.
	* Tower Shield Specialist
		* You reduce the armor check penalty for tower shields by 3, and if you have the armor training class feature, you modify the armor check penalty and maximum Dexterity bonus of tower shields as if they were armor.

* Mythic Feats
    * Cleave (Mythic)
		* Whenever you use Cleave or Cleaving Finish, your attacks can be made against a foe that is within your reach.
    * Critical Focus (Mythic)
		* You automatically confirm critical threats against non-mythic opponents. In addition, when you threaten a critical hit against a creature wearing armor with the fortification special ability or similar effect, that creature must roll twice and take the worse result when determining critical hit negation.
    * Combat Expertise (Mythic)
        * Whenever you use Combat Expertise, you gain an additional +2 dodge bonus to your Armor Class.
    * Combat Reflexes (Mythic)
        * You can make any number of additional attacks of opportunity per round.
    * Intimidating Prowess (Mythic)
        * You gain a bonus on Intimidate checks equal to your mythic rank.
    * Manyshot (Mythic)
        * When making a full-attack action with a bow and using Manyshot, you fire two arrows with both your first and second attacks, instead of just your first attack.
    * Shatter Defenses (Mythic)
        * An opponent you affect with Shatter Defenses is flat-footed to all attacks, not just yours.
    * Two-Weapon Defense (Mythic)
        * When using Two-Weapon Defense, you apply the highest enhancement bonus from your two weapons to the shield bonus granted by that feat.
    * Warrior Priest (Mythic)
        * You gain a bonus equal to half your tier both on initiative checks and on concentration checks.
    * Titan Strike
        * Your unarmed strike deals damage as if you were one size category larger. You also gain a +1 bonus for each size category that your target is larger than you on the following: bull rush, drag, grapple, overrun, sunder, and trip combat maneuver checks and the DC of your Stunning Fist.

* Mythic Abilities
    * Abundant Blessing
        * You can use Blessings a number of additional times per day equal to your mythic rank.
    * Abundant Bombs
        * You can throw additional bombs per day equal to twice your mythic rank.
    * Abundant Lay On Hands
        * You can use Lay on Hands a number of additional times per day equal to your mythic rank.
    * Abundant Incense
        * You can use incense fog for an additional number of rounds equal to your mythic rank.
    * Abundant Fervor
        * You can use Fervor a number of additional times per day equal to your mythic rank.
    * Abundant Spell Kenning.
        * You can use Spell Kenning a number of additional times per day equal to one thrid your mythic rank.
    * Armored Might
        * You treat the armor bonus from your armor as 50% higher than normal, to a maximum increase of half your mythic rank plus one.
    * Armor Master
        * While wearing armor, you reduce the armor check penalty by 1 per mythic rank and increase the maximum Dexterity bonus allowed by by 1 per mythic rank. Additionally you reduce your arcane spell failure chance from armor and shields by 5% per mythic rank.
    * Enhanced Blessings
        * The effects from your blessings now last twice as long.
    * Favorite Metamagic Persistent
        * Select one kind of metamagic. The spell level cost for its use decreases by one (to a minimum of 0).
    * Favorite Metamagic Selective
        * Select one kind of metamagic. The spell level cost for its use decreases by one (to a minimum of 0).
    * Harmonious Mage
        * Your wizardly studies have moved beyond the concept of opposition schools. Preparing spells from one of your opposition schools now only requires one spell slot of the appropriate level instead of two.
    * Impossible Blessing
        * You gain one more blessing, ignoring all blessing prerequisites.
    * Impossible Speed
        * Your base land speed increases by 30 feet plus an additional 5 feet for every mythic rank.
    * Mounted Maniac
        * Your unstoppable momentum while mounted is terrifying. Whenever you charge a creature while mounted, you can attempt an Intimidate check to demoralize all enemies within 30 feet of your target, adding your mythic rank to the result of the check.
    * Mythic Spell Combat
        * The magus can use his spell combat and spellstrike abilities while casting or using spells from a spellbook granted by a mythic class.
    * Precision Critical
        * Whenever you score a critical hit, double any extra precision damage dice, such as sneak attack damage. These dice are only doubled, not multiplied by the weapon's critical modifier.
    * Second Patron
        * You select a second patron, gaining all its benefits.

* Bloodlines
    * Aberrant Sorcerer Bloodline
    * Destined Sorcerer Bloodline
    * Aberrant Bloodrager Bloodline
    * Destined Bloodrager Bloodline

* Arcanist Exploits
    * Familiar
        * An arcanist with this exploit can acquire a familiar as the arcane bond wizard class feature. A familiar is a magical pet that enhances the wizard's skills and senses.
    * Item Crafting
        * The arcanist can select one item creation feat as a bonus feat. She must meet the prerequisites of this feat.
    * Metamagic Knowledge
        * The arcanist can select one metamagic feat as a bonus feat. She must meet the prerequisites of this feat.
    * Quick Study
        * The arcanist can prepare a spell in place of an existing spell by expending 1 point from her arcane reservoir. Using this ability is a full-round action that provokes an attack of opportunity. The spell prepared must be of the same level as the spell being replaced.

* Fighter Advanced Armor Training
    * Armored Confidence
        * While wearing armor, the fighter gains a bonus on Intimidate checks based upon the type of armor he is wearing: +1 for light armor, +2 for medium armor, or +3 for heavy armor. This bonus increases by 1 at 7th level and every 4 fighter levels thereafter, to a maximum of +4 at 19th level.
    * Armored Juggernaut
        * When wearing heavy armor, the fighter gains DR 1/-. At 7th level, the fighter gains DR 1/- when wearing medium armor, and DR 2/- when wearing heavy armor. At 11th level, the fighter gains DR 1/- when wearing light armor, DR 2/- when wearing medium armor, and DR 3/- when wearing heavy armor.
    * Armor Specialization
        * The fighter selects one specific type of armor with which he is proficient, such as light or heavy. While wearing the selected type of armor, the fighter adds one-quarter of his fighter level to the armor''s armor bonus, up to a maximum bonus of +3 for light armor, +4 for medium armor, or +5 for heavy armor. This increase to the armor bonus doesn't increase the benefit that the fighter gains from feats, class abilities, or other effects that are determined by his armor's base armor bonus, including other advanced armor training options.
    * Critical Deflection
        * While wearing armor or using a shield, the fighter gains a +2 bonus to his AC against attack rolls made to confirm a critical hit. This bonus increases by 1 at 7th level and every 4 fighter levels thereafter, to a maximum of +6 at 19th level.
    * Steel Headbutt
        * While wearing medium or heavy armor, a fighter can deliver a headbutt with his helm as part of a full attack action. This headbutt is in addition to his normal attacks, and is made using the fighter's base attack bonus - 5. A helmet headbutt deals 1d3 points of damage if the fighter is wearing medium armor, or 1d4 points of damage if he is wearing heavy armor (1d2 and 1d3, respectively, for Small creatures), plus an amount of damage equal to 1/2 the fighter's Strength modifier. Treat this attack as a weapon attack made using the same special material and echantment bonus (if any) as the armor.

* Fighter Advanced Weapon Training
    * Defensive Weapon Training
        * The fighter gains a +1 shield bonus to his Armor Class. The fighter adds half his weapon's enhancement bonus (if any) to this shield bonus. When his weapon training bonus for weapons from the associated fighter weapon group reaches +4, this shield bonus increases to +2. This shield bonus is lost if the fighter is immobilized or helpless.
    * Focused Weapon
        * The fighter selects one weapon for which he has Weapon Focus and that belongs to the associated fighter weapon group. The fighter can deal damage with this weapon based on the damage of the warpriest's sacred weapon class feature, treating his fighter level as his warpriest level. The fighter must have Weapon Focus and Weapon Training with the selected weapon in order to choose this option. A focused weapon deals 1d6 damage, this increases to 1d8 at level 6, 1d10 at level 10, 2d6 at level 15, and 2d8 at level 20
    * Trained Grace
        * When the fighter uses Weapon Finesse to make a melee attack with a weapon, using his Dexterity modifier on attack rolls and his Strength modifier on damage rolls, he doubles his weapon training bonus on damage rolls. The fighter must have Weapon Finesse in order to choose this option.
    * Trained Throw
        * When the fighter makes a ranged attack with a thrown weapon and applies his Dexterity modifier on attack rolls and his Strength modifier on damage rolls, he doubles his weapon training bonus on damage rolls.
    * Warrior Spirit
        * The fighter can forge a spiritual bond with a weapon that belongs to the associated weapon group, allowing him to unlock the weapon's potential. Each day he gains a number of points of spiritual energy equal to 1 + his maximum weapon training bonus. While wielding a weapon he has weapon traiing with, he can spend 1 point of spiritual energy to grant the weapon an enhancement bonus equal to his weapon training bonus. Enhancement bonuses gained by this advanced weapon training option stack with those of the weapon, to a maximum of +5. The fighter can also imbue the weapon with any one weapon special ability with an equivalent enhancement bonus less than or equal to his maximum bonus by reducing the granted enhancement bonus by the amount of the equivalent enhancement bonus. These bonuses last for 1 minute.

* Magus Arcana
    * Broad Study
        * The magus selects another one of his spellcasting classes. The magus can use his spellstrike and spell combat abilities while casting or using spells from the spell list of that class. This does not allow him to cast arcane spells from that class's spell list without suffering the normal chances of arcane spell failure, unless the spell lacks somatic components.
    * Spell Blending
        * When a magus selects this arcana, he must select one spell from the wizard spell list that is of a magus spell level he can cast. He adds this spell to his spellbook and list of magus spells known as a magus spell of its wizard spell level. He can instead select two spells to add in this way, but both must be at least one level lower than the highest-level magus spell he can cast.

* Rogue Talents
    * Graceful Athlete
        * Add your Dexterity modifier instead of your Strength bonus to Athletics checks.
    * Bleeding Attack
        * A rogue with this ability can cause living opponents to bleed by hitting them with a sneak attack. This attack causes the target to take 1 additional point of damage each round for each die of the rogue’s sneak attack.
    * Emboldening Strike
        * When a rogue with this talent hits a creature with a melee attack that deals sneak attack damage, she gains a +1 circumstance bonus on saving throws for every 2 sneak attack dice rolled (minimum +1) for 1 round.

* Alternate Racial Traits
    * Dwarf - Stoutheart
        * Not all dwarves are as standoffish and distrusting as their peers, though they can be seen as foolhardy and brash by their kin. Dwarves with this racial trait gain +2 Constitution, +2 Charisma, and -2 Intelligence. This racial trait alters the dwarves’ ability score modifiers.
    * Dwarf - Stoic Negotiator
        * Some dwarves use their unwavering stubbornness to get what they want in negotiations and other business matters. They gain a +2 racial bonus on Persuasion checks and Persuasion is a class skill for them. This racial trait replaces defensive training, hatred.
    * Elf - Fierani Elf
        * Having returned to Golarion to reclaim their ancestral homeland, some elves of the Fierani Forest have a closer bond to nature than most of their kin. Elves with this racial trait gain +2 Dexterity, +2 Wisdom, and -2 Constitution. This racial trait alters the elves’ ability score modifiers.
    * Elf - Arcane Focus
        * Some elven families have such long traditions of producing wizards (and other arcane spellcasters) that they raise their children with the assumption each is destined to be a powerful magic-user, with little need for mundane concerns such as skill with weapons. Elves with this racial trait gain a +2 racial bonus on concentration checks. This racial trait replaces weapon familiarity.
    * Elf - Long Limbed
        * Elves with this racial trait have a base move speed of 35 feet. This racial trait replaces weapon familiarity.
    * Elf - Moon Kissed
        * Elves with this alternate racial trait gain a +1 racial bonus on saving throws. This replaces elven immunities and keen senses.
    * Elf - Vigilance
        * You gain a +2 dodge bonus to AC against attacks by chaotic creatures. This trait replaces elven magic.
    * Gnome - Artisan
        * Some gnomes lack their race’s iconic humor and propensity for pranks, instead devoting nearly all of their time and energy to their crafts. Such gnomes gain +2 Constitution, +2 Intelligence, and -2 Strength. This racial trait alters the gnomes’ ability score modifiers.
    * Gnome - Keen
        * Some gnomes are far more cleaver than they seem, and have devoted all of their time in the pursit of knowledge. Such gnomes gain +2 Charisma, +2 Intelligence, and -2 Strength.\nThis racial trait alters the gnomes’ ability score modifiers. This racial trait replaces defensive training.
    * Gnome - Fell Magic
        * Gnomes add +1 to the DC of any saving throws against necromancy spells that they cast. This racial trait replaces gnome magic
    * Gnome - Utilitarian Magic
        * Some gnomes develop practical magic to assist them with their obsessive projects. These gnomes add 1 to the DC of any saving throws against transmutation spells they cast. This racial trait replaces gnome magic
    * Gnome - Inquisitive
        * Gnomes have a knack for being in places they shouldn’t be. Gnomes with this trait gain a +2 racial bonus on Trickery and Mobility checks. This racial trait replaces keen senses and obsessive.
    * Gnome - Nosophobia
        * You gain a +4 bonus on saves against disease and poison, including magical diseases. This racial trait replaces obsessive.
    * Halfling - Bruiser
        * A lifetime of brutal survival, either under the heavy burdens of slavery or on the streets, has made some halflings more adept at taking blows than dodging them. Halflings with this racial trait gain +2 Constitution, +2 Charisma, and -2 Dexterity. This racial trait alters the halflings’ ability score modifiers.
    * Halfling - Blessed
        * Halflings with this trait receive a +2 racial bonus on saving throws against curse effects and hexes. This bonus stacks with the bonus granted by halfling luck. This racial trait replaces fearless.
    * Halfling - Secretive Survivor
        * Halflings from poor and desperate communities, most often in big cities, must take what they need without getting caught in order to survive. They gain a +2 racial bonus on Persuasion and Stealth checks. This racial trait replaces sure-footed.
    * Halfling - Underfoot
        * Halflings must train hard to effectively fight bigger opponents. Halflings with this racial trait gain a +1 dodge bonus to AC against foes larger than themselves. This racial trait replaces halfling luck.

* Backgrounds
    * Lecturer
        * Lecturer adds Knowledge (World) and Persuasion to the list of her class skills. She can also use her Intelligence instead of Charisma while attempting Persuasion checks.

Acknowledgments:  

-   Pathfinder Wrath of The Righteous Discord channel members
-   @Balkoth, @Narria, @edoipi, @SpaceHamster and the rest of our great Discord modding community - help.
-   PS: Wolfie's [Modding Wiki](https://github.com/WittleWolfie/OwlcatModdingWiki/wiki) is an excellent place to start if you want to start modding on your own.
-   Join our [Discord](https://discord.gg/owlcat)
