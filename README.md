This mod seeks to adjust the game to be closer to the tabletop Pathfinder ruleset.

Additionally this mod seeks to add in content that was not included in the base game from the source books. There is some minor homebew that is all marked in the mod settings.

Once a game is saved with this mod is enabled it will require this mod to be present to load so do not remove or disable the mod once enabled. You can however disable any feature of the mod at will without breaking saves.

All fixes and added content are configurable and can be disabled via the unity mod manager menu.

**How to install**

1. Download and install [Unity Mod Manager](https://github.com/newman55/unity-mod-manager), make sure it is at least version 0.23.0
2. Run Unity Mod Manger and set it up to find Wrath of the Righteous
3. Download the Tabletop Tweaks mod
4. Install the mod by dragging the zip file from step 5 into the Unity Mod Manager window under the Mods tab

## Fixes
* UI Fixes
    * Dynamic Item Naming
        * Allows Weapon/Armor names to be modified by any additional enchants they gain outside of thier base enchants.
    * Saving Throw Overtips
        * Overhead saving throw rolls now display the correct values
    * Spell Tooltips
        * Updates spell tooltips to include what spellbook the spell is in.
* Bases Fixes
    * Progressions
        * Progressions will no longer incorrectly include mythic levels, or double count some classes levels.
    * Coup De Grace
        * Coup De Grace saving throw DC is now based on damage dealt.
    * Damage Reduction
        * Prevents most forms of damage reduction from stacking unless specifically specified.
    * Unlimited Abilities Combat Behavior
        * Allows abilities that have become unlimited to remain on after combat ends.
    * Natural Armor
        * Prevents Natural Armor bonuses from stacking unless they are specficed to stack. These use normal descriptor rules still so a natural armor bonus and a natural armor enhancment bonus will still stack.
    * Polymorph Effects
        * Prevents multiple polymorph effects from being applied to a character at a time.
        * Active Polymorph effects will correctly suppress size effects from non polymorph spells.
    * Size Effects
        * Prevents multiple size changing buffs granting benifits at the same time. Old buffs are suppressed.
    * Feat Selections
        * Cleans up limited feat selections (like fighter combat feats) to include all feats of the specified type.
    * Background Modifiers
        * Changes the skill and attack bonuses granted from backgrounds into trait bonuses.
    * Critical Confirmation
        * Allows a natural 20 to always confirm a critical.
    * Inherent Stat Bonuses
        * Allows Inhernt bonuses to intelligence to now grant bonus skill points.
    * Mounted Longspear
        * Spears now grant additional damage during a mounted charge in the same mannor a lance would.
    * Enemy Buff CL
        * Enemy buffs now have the correct CL for applied buffs as defined on thier blueprints.
    * Shadow Spells
        * Shadow spells now are correctly treated as being from the Illusion school for all effects.
    * Mounted Movement
        * Prevents full round actions after moving while mounted.
    * Nauseated Condition
        * Removes the poison descripotor from nauseated.
    * Staggered Condition
        * Removes the movement impairing descriptor from staggered.

* Spells
    * Spell flags
        * Retags buffs from spells as coming from spells to allow them to be dispeled correctly.
    * Acid Maw 
        * Acid Maw no longer causes excessive damage instances to trigger when attacking.
    * Believe In Yourself
        * Believe in yourself now grants the correct bonus amount.
    * Bestow Greater Curse
        * Bestow Greater Curse now actually bestows a greater curse not a normal curse.
    * Break Enchantment
        * Break Enchantment no longer affects friendly buffs.
    * Chain Lightning
        * Chain Lightning now respect the 20 CL max for damage dice.
    * Crusader's Edge
        * Crusaders Edge's nauseate effect now only procs on critical hits.
    * Dispel Magic Greater
        * Greater Dispel Magic now only removes 1/4 CL buffs instead of all buffs.
    * Firebrand
        * Firebrand no longer causes excessive damage instances to trigger when attacking.
    * Geniekind
        * Geniekind no longer causes excessive damage instances to trigger when attacking.
    * Greater Magic Weapon
        * Greater Magic Weapon no longer stacks with existing enhancement bonuses.
    * Hellfire Ray
        * Hellfire Ray no longer has the Fire descriptor.
    * Magical Vestment
        * Magical Vestment now enhances your armor instead of granting a floating modifier.
    * Microscopic Proportions 
        * Microscopic Proportions now correctly grants a size bonus instead of an untyped bonus.
    * Remove Fear
        * Remove Fear no longer grants immunity to shaken and fear.
    * Remove Sickness
        * Remove Sickness no longer grants immunity to sickness and nausea.
    * Shadow Conjuration
        * Shadow Conjuration has been added to the Wizard spell list.
    * Shadow Conjuration Greater 
        * Now has the correct shadow factor of 60 instead of 40.
    * Shadow Evocation
        * Shadow Evocation can now have the correct metamagics applied.
    * Greater Shadow Evocation
        * Greater Shadow Evocation can now have the correct metamagic applied.
        * Now has the correct shadow factor of 60 instead of 40.
    * Starlight
        * Starlight no longer is affected by true sight.
    * Unbreakable Heart
        * Unbreakable Heart no longer grants complete immunity to confusion and emotion effects and instead supresses correctly.
    * WrackingRay
        * Wracking Ray now deals the correct amount of ability damage.

* Feats
    * AlliedSpellcaster
        * Allied Spellcaster no longer applies globally.
    * Arcane Strike 
        * Arcane Strike no longer causes too many damage instances when used by a dragonheir scion.
    * Brew Potions
        * Brew Potions is no longer tagged as a combat feat.
    * Bolstered Metamagic
        * Sticky touch spells can now be bolstered.
    * EmpowerMetamagic
        * Sticky touch spells can now be empowered.
        * Prevents extra dice from empowered metamagic from being maximized by maximize metamagic.
    * Maximize Metamagic
        * Sticky touch spells can now be maximized.
    * PersistantMetamagic
        * Allows any spell with a saving throw to be made persistant.
    * Selective Metamagic
        * Retags selective spells to exclude non instantaneous spells.
        * Now requires 10 ranks of knowledge arcana.
    * Crane Wing
        * Now requires a free hand to recieve the bonuses.
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
    * ShatterDefenses
        * Now requires you to hit a shaken target once before they become flat footed.
    * SlashingGrace
        * Fixed an edge case that sometimes allowed slashing grace from applying to two handed weapons.
    * Spell Specialization
        * Enables spell sepecialization selection on all levelups.
    * Spirited Charge
        * Bonus damage no longer can crit.
    * Vital Strike
        * Bonus damage no longer can crit.
    * Weapon Finesse
        * No longer treats any weapon with Fencing/Slashing grace into a finesse weapon.

* Mythic Feats
    * Expanded Arsenal
        * No longer allows stacking multiple spell focuses on the same school to increase DC, you can only benifit from spell focus once.
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
    * Mythic Charge
        * Prevents Mythic charge from triggering multiple fake attacks and procing weapon effects more often than it should.
    * Second Bloodline
        * All bloodlines now qualify for second bloodline including mutated ones.
    * Second Bloodrager Bloodline
        * Reformed Fiend now qualifies for all bloodlines

* Bloodlines
    * Rebuilt all prerequisites to fix multiple issues with prestige class interactions when multiclassing

* Aeon
    * Aeon Demythication
        * Aeon Demythication should now actually suppress mythic effects.

* Lich
    * Death Rush
        * Prevents Death Rush from triggering multiple fake attacks and procing weapon effects more often than it should.
    * Spellbook Merging
        * Allows Nature mage to merge with the Lich spellbook.

* Trickster
    * Use Magic Device 2
        * Allows trickster to ignore class/alignment restricitons of equipment with UMD2 trick.

* Alchemist
    * Mutagen
        * Prevents the stacking of mutagens. Only one may be active at a time.
    * Grenadier
        * Removed brew potions from the archetype
        * Removed posion resistance from the archetype

* Arcanist
    * Prepared Spell UI
        * Makes arcanist spellbook behave like a prepared caster not a spontaneous one.

* Barbarian
    * Crippling Blows
        * Allows crippling blows to work when raging.
    * Wrecking Blows
        * Allows crippling blows to work when raging.

* Bloodrager
    * Abysal Bulk
        * Enables abysal bulk to not dispel existing enlarge person when rage ends.
    * Arcane Bloodrage
        * Completly rebuilds arcane bloodrage with new UI
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
        * Allows Cavalier to ignore thier armor check penalty while mounted for mobility skill checks.
    * Mount Selection
        * Allows the Cavalier to select a wolf for a mount if they are of small size.
    * Supreme Charge
        * Prevents Supreme Charge damage from criting and moves it into the new charage damage system.
    * Gendarme
        * Prevents Transfixing Charge damage from criting and moves it into the new charage damage system.

* Cleric
    * Glory Domain
        * Glory domain no longer grants an untyped bonus the the raw Charsima stat.

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

* Magus
    * Arcane Weapon
        * Adds: Flaming Burst, Icy Burst, Shocking Burst enchant options.
    * Spell COmbat
        * Lets spell combat work with spells that have variants like dimention door.
        * Disables spell combat immediatly when toggled off instead of having to wait until the next round.
        * Prevents spells that are not in the magus spellbook from working with spell combat.
    * Sword Saint
        * Updates perfect critical's cost to 2 points of arcane pool instead of 1.

* Monk
    * ZenArcher
        * At level 10 a zen archer will roll 3 dice instead of 2 with perfect strike.

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
        * Favored enemy outsider now applies to ALL demons. The Demons of X are disabled unless you already have ranks, but otherwise function identicly to favored enemy outsider and you can keep picking them for compatability with existing characters.
    * Espionage Expert
        * Trapfinding now grants bonuses to perception and trickery.

* Rogue
    * Rogue Talents
        * Prevents you from selecting the same talent more than once.
    * Slippery Mind
        * Slippery mind is now an advanced talent and correctly updates its save bonuses when your dexterity changes.
    * Trapfinding
        * Trapfinding now grants bonuses to perception and trickery.
    * Eldritch Scoundrel
        * Removes the level 2 rogue talent and adds in a level 4 talent.
        * Removes the sneak attack dice granted at level 1.
    * Sylvan Trickster
        * Fey Tricks now includes all rogue talents

* Shaman
    * Ameliorating Hex 
        * Ameliorating Hex  no longer grants complete immunity to effects and instead supresses correctly.

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
    * Weather Blessinggit
        * Weather Minor blessing no longer causes excessive damage instances.
    * Luck Blessing
        * Luck Blessing now grants the correct major ability.

* Witch
    * Agility Patron
        * Agility Patron now gets Animal Shapes at 16th level and Shapechange at 18th.
    * Ameliorating Hex 
        * Ameliorating Hex  no longer grants complete immunity to effects and instead supresses correctly.
    * Major Ameliorating Hex 
        * Major Ameliorating Hex no longer grants complete immunity to effects and instead supresses correctly.

* Hellknight
    * Pentamic Faith
        * Pentamic Faith now requires the Godclaw hellknight order not the diety.

* Loremaster
    * Prerequisites
        * Updates the prerequisites to be more strict and better match tabletop.
    * Spell Progression
        * Grants spell progression at level 1. This is not retroactive.
    * Spell Secrets
        * Spell Secrets now work.
    * Trickster Tricks
        * Remvoes trickster tricks from the combat feat selection.

* Crusade
    * TrainingGrounds
        * Now grants the correct damage bonus.

* Armor
    * Haramaki
        * Haramaki are now counted as light armor properly.

* Equipment
    * Half Of The Pair
        * Will more accuratly update the bonus with range.
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
    * Terrifying Tremble
        * Implements the missing on kill effect. Whenever the wielder of this weapon lands a killing blow, he deals sonic damage equal to his ranks in the Athletics skill to all enemies within 10 feet. Successful Reflex save (DC 30) halves the damage.
    * Thundering Burst
        * Fixes thundering burst to deal D10s like the description says instead of D8s.

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
    
* Spells
    * Long Arm
        * Your arms temporarily grow in length, increasing your reach with those limbs by 5 feet.
    * Shadow Enchantment
        * You use material from the Shadow Plane to cast a quasi-real, illusory version of a psychic, sorcerer, or wizard enchantment spell of 2nd level or lower. Spells that deal damage or have other effects work as normal unless the affected creature succeeds at a Will save. If the disbelieved enchantment spell has a damaging effect, that effect is one-fifth as strong (if applicable) or only 20% likely to occur. If recognized as a shadow enchantment, a damaging spell deals only one-fifth (20%) the normal amount of damage. If the disbelieved attack has a special effect other than damage, that effect is one-fifth as strong (if applicable) or only 20% likely to occur. Regardless of the result of the save to disbelieve, an affected creature is also allowed any save (or spell resistance) that the spell being simulated allows, but the save DC is set according to shadow enchantment's level (3rd) rather than the spell's normal level.
    * Shadow Enchantment Greater
        * This spell functions like shadow enchantment, except that it enables you to create partially real, illusory versions of psychic, sorcerer, or wizard enchantment spells of 5th level or lower. If the spell is recognized as a greater shadow enchantment, it's only three-fifths (60%) as effective.

* Metamagic
    * Burning Spell (Metamagic)
        * The acid or fire effects of the affected spell adhere to the creature, causing more damage the next round. When a creature takes acid or fire damage from the affected spell, that creature takes damage equal to 2x the spell’s actual level at the start of its next turn. The damage is acid or fire, as determined by the spell’s descriptor.
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

* Feats
    * AnimalAlly
        * You gain an animal companion as if you were a druid of your character level -3. Unlike normal animals of its kind, an animal companion's Hit Dice, abilities, skills, and feats advance as you advance in level.
    * CelestialServant
        * Your animal companion, familiar, or mount gains the celestial template and becomes a magical beast, though you may still treat it as an animal when using Handle Animal, wild empathy, or any other spells or class abilities that specifically affect animals.
    * DervishDance
        * When wielding a scimitar with one hand, you can use your Dexterity modifier instead of your Strength modifier on melee attack and damage rolls.
    * Dispel Focus
        * Whenever you attempt a dispel check based on your caster level, you gain a +2 bonus to the check.
    * Greater Dispel Focus
        * Whenever you attempt a dispel check based on your caster level, you gain a +2 bonus to the check. This stacks with the bonus from Dispel Focus.
    * ErastilsBlessing
        * You can use your Wisdom modifier instead of your Dexterity modifier on ranged attack rolls when using a bow.
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
    * VarisianTattoo
        * Select a school of magic in which you have Spell Focus. Spells from this school are cast at +1 caster level.

* Mythic Feats
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
    * Armored Might
        * You treat the armor bonus from your armor as 50% higher than normal, to a maximum increase of half your mythic rank plus one.
    * Armor Master
        * While wearing armor, you reduce the armor check penalty by 1 per mythic rank and increase the maximum Dexterity bonus allowed by by 1 per mythic rank. Additionally you reduce your arcane spell failure chance from armor and sheilds by 5% per mythic rank.
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
        * You select a second patron, gaining all its benifits.

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

* MagusArcana
    * Broad Study
        * The magus selects another one of his spellcasting classes. The magus can use his spellstrike and spell combat abilities while casting or using spells from the spell list of that class. This does not allow him to cast arcane spells from that class's spell list without suffering the normal chances of arcane spell failure, unless the spell lacks somatic components.
    * Spell Blending
        * When a magus selects this arcana, he must select one spell from the wizard spell list that is of a magus spell level he can cast. He adds this spell to his spellbook and list of magus spells known as a magus spell of its wizard spell level. He can instead select two spells to add in this way, but both must be at least one level lower than the highest-level magus spell he can cast.

* Rogue Talents
    * Graceful Athlete
        * Add your Dexterity modifier instead of your Strength bonus to Athletics checks.

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

## Reworks
* Feats
    * Bolstered Spell Metamagic
        * Level increase has been increased from +1 to +2. (This is defaulted to off)

* Mythic Feats
    * Mythic Sneak Attack
        * Your sneak attack dice are one size larger than normal. For example if you would normally roll d6s for sneak attacks you would roll d8s instead.

 * Mythic Feats
    * Elemental Barrage
        * Every time you deal elemental damage to a creature with a spell, you apply an elemental mark to it. If during the next three rounds the marked target takes elemental damage from any source with a different element, the target is dealt additional Divine damage. The damage is 1d6 per mythic rank of your character.

* Aeon
    * Aeon Bane
        * Updates Aeon Bane's Icon.
        * Aeon Bane adds mythic rank to spell resistance checks.
        * Aeon Bane usages now scale at 2x Mythic level + Character level.
    * Aeon Improved Bane
        * Aeon Improved Bane now uses greater dispel magic rules to remove 1/4 CL buffs where CL is defined as Character Level + Mythic Rank.
    * Aeon Greater Bane
        * Aeon Greater Bane now allows you to cast swift action spells as a move action.
        * Aeon Greater Bane damage is now rolled into the main weapon attack instead of a separate instance.
        * Aeon Greater Bane now has the garentee'd auto dispel on first hit.
    * Aeon Gaze
        * Aeon Gaze now functions like Inquisitor Judgments where multiple can be activated for the same resouce usage.
        * Aeon Gaze DC has been adjusted from 15 + 2x Mythic Level to 10 + 1/2 Character Level + 2x Mythic level.
        * Aeon Gaze selection is no longer limited on the first selection and all selections are available.

* Azata
    * Performances
        * Azata perforamnce usages now scale at Mythic level + Character level.
    * FavorableMagic
        * Favorable magic now works on reoccuring saves.
    * Zippy Magic
        * Zippy magic will now ignore some harmful effects.

Acknowledgments:  

-   Pathfinder Wrath of The Righteous Discord channel members
-   @Balkoth, @Narria, @edoipi, @SpaceHamster and the rest of our great Discord modding community - help.
-   PS: Wolfie's [Modding Wiki](https://github.com/WittleWolfie/OwlcatModdingWiki/wiki) is an excellent place to start if you want to start modding on your own.
-   Join our [Discord](https://discord.com/invite/wotr)
