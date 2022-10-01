## Version 2.4.1
* Added Spells
    * Stunning Barrier Greater
        * This spell functions as stunning barrier, except it provides a +2 bonus to AC and on saving throws and it is not discharged until it has stunned a number of creatures equal to your caster level.

## Version 2.4.0
* Release for 2.0.0
* Fixes
    * Fixed broken charge mechanics
    * Sohei
        * Sohei should now work correctly with weapon training changes if enabled
    * Winter Witch
        * Unearly Cold's direct damage should now properly respect metamagic and added bonus damage
* Added Metamagic
	* Elemental Spell Metamagic
		* Choose one energy type: acid, cold, electricity, or fire. You may replace a spell’s normal damage with that energy type "or split the spell’s damage, so that half is of that energy type and half is of its normal type.

## Version 2.3.1
* Release for 1.4.0

## Version 2.3.0
* Fixes
    * Witch
        * Fixed Cauldren Witch incorrectly scaling with character level instead of witch level.
* Added Content
    * Rogue Talents
        * Bleeding Attack
            * A rogue with this ability can cause living opponents to bleed by hitting them with a sneak attack. This attack causes the target to take 1 additional point of damage each round for each die of the rogue’s sneak attack.
        * Emboldening Strike
            * When a rogue with this talent hits a creature with a melee attack that deals sneak attack damage, she gains a +1 circumstance bonus on saving throws for every 2 sneak attack dice rolled (minimum +1) for 1 round.
    * Spells
        * Mage's Disjunction
            * All magical effects and magic items within the radius of the spell, except for those that you carry or touch, are disjoined. That is, spells and spell-like effects are unraveled and destroyed completely (ending the effect as a dispel magic spell does), and each permanent magic item must make a successful Will save or be turned into a normal item for the duration of this spell.

## Version 2.2.3
* Fixes
    * Fixed issue where Bloodline Ascendance was not selectable when it should have been.

## Version 2.2.2
* Fixes
    * Fixed broken config options for some metamagic.
    * Improved mod compatibility with metamagics.
    * Fixed mod compatibility issue with world crawl.

## Version 2.2.1
* Fixes
    * Fixed issue where vivisectionist was unselectable with alternate capstones enabled.

## Version 2.2.0
* Fixes
    * Systems
        * Progressions no longer display features you will not get.
        * Scoll UMD DCs are now calculated correctly at 20 + Scroll CL instead of 20 + Scroll spell level.
    * Features
        * Mongrol's Blessing
            * Now properly applies negative levels.
        * Incorporeal Charm
            * Now properly updates when charisma changes.
    * Spells
        * Absolute Order
            * Now has missing compulsion descriptor.
        * Command
            * Now has missing compulsion descriptor.
        * Greater Command
            * Now has missing compulsion descriptor.
        * Legendary Proportions
            * Now only increases size by one step instead of two.
        * Power From Death
            * Now correctly lasts rounds per level instead of minutes per level.
    * Feats
        * Outflank
            * Outflank should no longer triggers on missed attacks.
        * Seize the Moment
            * Seize the moment should no longer trigger on missed attacks.
        * Cleaving Finish
            * Cleave fixes now includes a working cleaving finish that will not randomly stop after three targets.
    * Alchemist
        * Fixed Thick Fog being an illusion based concealment instead of fog based.
        * Fixed issue where Abundant Incense was not selectable if you had expanded range incense.
    * Monk
        * Scaled Fist
            * Draconic Fury now uses the correct unchained version of the progression instead of the chained.
            * Fixes stunning fist to use the same version as other monks for compatibility.
    * Weapons
        * Sound of Void
            * Now correctly removes spell resistance when hitting a flat footed target.
        * Music of Death
            * Now correctly deals bonus damage when hitting a flat footed target.
* Added Content
    * Skald
        * Spell Kenning has been implemented.
    * Monk
        * Stunning Fist: Stagger
            * This ability works as Stunning Fist, but it makes the target staggered for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.
        * Stunning Fist: Blind
            * This ability works as Stunning Fist, but it permanently blinds the target on a failed save instead of stunning for 1 round.
        * Stunning Fist: Paralyze
            * This ability works as Stunning Fist, but it paralyzes the target for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.
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
    * Feats
        * Ability Focus - Stunning Fist
            * Add +2 to the DC for all saving throws against your stunning fist.
        * Expanded Spell Kenning
            * When you use your spell kenning class feature, you can select a spell from either the druid or the witch spell list.
        * Mantis Style
            * You gain one additional Stunning Fist attempt per day. While using this style, you gain a +2 bonus to the DC of effects you deliver with your Stunning Fist.
        * Mantis Wisdom
            * Treat half your levels in classes other than monk as monk levels for determining effects you can apply to a target of your Stunning Fist per the Stunning Fist monk class feature. While using Mantis Style, you gain a +2 bonus on unarmed attack rolls with which you are using Stunning Fist attempts.
        * Mantis Torment
            * While using Mantis Style, you make an unarmed attack that expends two daily attempts of your Stunning Fist. If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.
    * Mythic Abilities
        * Abundant Spell Kenning.
            * You can use Spell Kenning a number of additional times per day equal to one third your mythic rank.

## Version 2.1.1
* Fixes
    * Hotfix for immortal mythic enemies.

## Version 2.1.0
* Fixes
    * Systems
        * Fixed Concealment checks for invisibility.
        * Size shifts are now supported from Fine to Colossal.
        * Charge damage multipliers should now work correctly again.
        * Prebuffs will be applied at a more correct level instead of nearly always CL 20.
        * Saves will now properly remember who cast buffs which should fix several bugs including idealize discovery not working on save load.
    * Feats
        * Cleave
            * Cleave now checks for adjacency to the last target (Adjacent = within 5ft) instead of just reach.
        * Cleaving Finish
            * Cleaving Finish now checks for adjacency to the last target (Adjacent = within 5ft) instead of just reach.
        * Toppling Bash
            * Toppling Bash's action cost has been removed for now due to bugs breaking action econemy when toggled. This is still limited to once per round.
    * Spells
        * Animal Growth
            * Animal Growth should now work correctly with animal companions.
        * Corrupt Magic
            * Corrupt Magic now uses a single dispel roll for all buffs like other dispels.
        * Death Ward
            * Death Ward now Supresses existing negative levels.
        * Mind Blank
            * Mind blank now makes you immune to detection with divination effects like see invisibility and true seeing.
        * Zero State
            * Zero State now uses a single dispel roll for all buffs like other dispels.
    * Armor
        * Singing Steel
            * Singing Steel now works.
    * Equipment
        * Flawless Belt Of Physical Perfection 8
            * Now increases critical hit range by 1 in all cases.
            * Now appears as a DLC1 reward.
    * Alchemsist
        * Fixed Incense Fog scaling incorrectly if you took expanded area.
    * Cavalier
        * Order of the Star's Calling ability should now properly apply bonuses.
    * Hunter
        * Fixed Divine Hunter's animal companion getting unlimited smites.
    * Rogue
        * Dispelling Attack now uses the correct CL and no longer removes debuffs.
* Added Mythic Feats
	* Cleave (Mythic)
		* Whenever you use Cleave or Cleaving Finish, your attacks can be made against a foe that is within your reach.


## Version 2.0.0a
* Fixed issue preventing settings from being saved

## Version 2.0.0
This has been split and now depends on [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core/releases).

* Fixes
    * Mechanics
        * Fixed issue with some effects being considered polymorphs incorrectly.
    * Sorcerer
        * Fixed issues with Destined Bloodline's Touch of Destiny applying bonuses incorrectly.
	* Bloodrager
        * Fixed disruptive bloodraget not triggering on arcane bloodrage.
        * Fixed caster's bane not triggering for arcane bloodrage.
* Added Metamagic
	* Encouraging Metamagic
		* Any morale bonus granted by an encouraging spell is increased by 1.
* Added Mythic Feats
	* Critical Focus (Mythic)
		* You automatically confirm critical threats against non-mythic opponents. In addition, when you threaten a critical hit against a creature wearing armor with the fortification special ability or similar effect, that creature must roll twice and take the worse result when determining critical hit negation.
* Added Feats
    * Riving Strike
        * If you have a weapon that is augmented by your Arcane Strike feat, when you damage a creature with an attack made with that weapon, that creature takes a –2 penalty on saving throws against spells and spell-like abilities. This effect lasts for 1 round.
* Added Armor Mastery Feats
	* Intense Blows
		* When wearing heavy armor and using Power Attack, you gain a +1 bonus to your CMD until the beginning of your next turn. When your base attack bonus reaches +4, and every 4 points thereafter, this bonus increases by another 1.
	* Knocking Blows
		* While wearing heavy armor if you hit a creature that is no more than one size category larger than you with a Power Attack, the creature you attacked is also knocked off balance. Until the beginning of your next turn, it takes a –4 penalty to its CMD against combat maneuvers that move it or knock it prone.
	* Secured Armor
		* When you are hit by a confirmed critical hit or a sneak attack while wearing medium or heavy armor, there is a 25% chance that the critical hit or sneak attack is negated and damage is instead rolled normally. Special: This chance stacks with the light fortification and moderate fortification armor special abilities.
	* Sprightly Armor
		* While wearing light armor you add your armor’s enhancement bonus as a bonus on your initiative checks.
* Added Shield Mastery Feats
	* Defended Movement
		* You gain a +2 bonus to your AC against attacks of opportunity.
	* Stumbling Bash
		* Creatures struck by your shield bash take a –2 penalty to their AC until the end of your next turn.
	* Toppling Bash
		* As a swift action when you hit a creature with a shield bash, you can attempt a trip combat maneuver against that creature at a –5 penalty. This does not provoke an attack of opportunity.
	* Tower Shield Specialist
		* You reduce the armor check penalty for tower shields by 3, and if you have the armor training class feature, you modify the armor check penalty and maximum Dexterity bonus of tower shields as if they were armor.

## Version 1.11.2
* Fixes
    * Feats
        * Destructive Dispel's prerequisites have been updated.
        * Dispel Synergy's prerequisites have been updated.
    * Spells
        * Frightful Aspect now correctly applies fear when hit in melee.
        * Winds Of Fall now works.
        * Perfect Form can no longer be stacked multiple time for multiple stat boosts.
    * Mythic Abilities
        * Expose Vulnerability
            * Now no longer critically hits and procs additional weapons effects.
    * Mythic Feats
        * Expaned Arsenal now interacts correctly with School Mastery and Varisian Tatto.
    * Units
        * Staunton Vane (Boss) now has the correct feats instead of using random default feats.
    * Magus
        * Eldritch Schon can now take the new bloodlines.
    * Aeon
        * Power of Law now works correctly with Reroll effects.
    * Demon
        * Brimorak Aspect now properly applies to all spells.
    * Items
        * Stormlord's Resolve can now be deactivated immediatly instead of taking 1 turn to turn off.
    * Homebrew Reworks
        * Dimensional Retribution
            * Rework of Dimensional Retribution to make it work more like dweomercat leap. This means it is an automatic action that teleports you and does an attack of opportunity.
* New Arcane Discoveries
    * Alchemical Affinity
        * Whenever you cast a spell that appears on both the wizard and alchemist spell lists, you treat your caster level as 1 higher than normal and the save DC of such spells increases by 1.
    * Idealize
        * When a transmutation spell you cast grants an enhancement bonus to an ability score, that bonus increases by 2. At 20th level, the bonus increases by 4.
    * Knowledge Is Power
        * You add your Intelligence modifier on combat maneuver checks and to your CMD.
    * Opposition Research
        * Select one Wizard opposition school; preparing spells of this school now only requires one spell slot of the appropriate level instead of two.
    * Yuelral's Blessing
        * Whenever you cast a spell that appears on both the wizard and druid spell lists, you treat your caster level as 1 higher than normal and the save DC of such spells increases by 1.
* New Items
    * Lesser Burning Metamagic Rod
        * Avaiable from skeletal merchant in act 2.
    * Lesser Intensified Metamagic Rod
        * Avaiable from skeletal merchant in act 2.
    * Lesser Piercing Metamagic Rod
        * Avaiable from Defender's Heart scroll vender.
    * Burning Metamagic Rod
        * Avaiable from skeletal merchant in act 3.
    * Flaring Metamagic Rod
        * Avaiable from scroll merchant in Drezen during act 3.
    * Intensified Metamagic Rod
        * Avaiable from skeletal merchant in act 3.
    * Piercing Metamagic Rod
        * Avaiable from scroll vender in act 2 warcamp.
    * Rime Metamagic Rod
        * Avaiable from skeletal merchant in act 3.
    * Greater Burning Metamagic Rod
        * Avaiable from magic merchant in the fleshmarkets.
    * Greater Flaring Metamagic Rod
        * Avaiable from scroll merchant in Drezen during act 5.
    * Greater Intensified Metamagic Rod
        * Avaiable from skeletal merchant in act 5.
    * Greater Rime Metamagic Rod
        * Avaiable from magic merchant in the fleshmarkets.

## Version 1.11.1
* Fixes
    * General
        * Elemental Bloodline spells now work more correctly with metamagics.
        * Addative critical range increases are no longer multiplied by keen/improved critical. This includes trickster improved imporved critical.
        * Fixed issue where bolstered metamagic was granting an unrelated stat boost.
    * Mythic Abiltiies
        * Ascendant Element bypasses elemental immunities again.
    * Spells
        * Vamperic Blade no longer triggers extra fake attacks.
    * Aeon
        * Aeon 10th level now correctly ignores energy immunities of chaotic characters.
        * Aeon Gaze buffs now all have proper icons.
    * Sorcerer
        * Elemental Bloodline arcanas now correctly show up in character creation.
* Reworks
    * Aeon 
        * Aeon no longer gets all gaze selections at all times. With the new gazes this no longer feels needed.
    * Azata
        * Incredible Might now grants a mythic bonus isntead of a morale bonus.
    * Lich
        * Deadly Magic is now usable 3 + half mythic rank rounds per day.
        * Decaying Touch has been rebuilt to prevent abuse cases but should work exactly as described now.
        * Eclipse Chill is now usable 3 + half mythic rank rounds per day.
        * Eclipse Chill DC is now 10 + 1/2 character level + twice your mythic rank.
        * Tainted Sneak Attack DC is now 10 + 1/2 character level + twice your mythic rank.
        * Tainted Sneak Attack now works on spells.

## Version 1.11.0a
* Chinese Localization has been updated for 1.11.0a
* Fixes
    * Metamagic
        * Burning Spell Metamagic now applies reworked elemental barrage when the burn damage ticks.
    * Feats
        * Destructive Dispel DC now benefits from global DC increases
    * Spells
        * Supernova now respects the saving throw for the blind and always deals the correct amount of damage.
    * Aeon
        * Aeon 10th level immunities now correctly grant automatic natural 20s for saving throws from chaotic enemies.
    * Mythic Feats
        * Combat Expertise (Mythic) now interacts as expected with Stalwart

## Version 1.11.0
* Fixes
    * General
        * More of the unique metamagic rods will no longer default to active
        * Nauseated is no longer considered a poison effect globally.
        * Fixed bug that was causing split damage spells to calculate damage incorrectly in some cases.
        * Active Polymorph effects will correctly suppress size effects from non polymorph spells.
        * Only one size changing buff can grant benifits at the same times.
    * Feats
        * Arcane Strike no longer causes too many damage instances when used by a dragonheir scion.
        * Brew Potions is no longer tagged as a combat feat
        * Destructive Dispel now calculates the DC based on the effective CL of the dispel and the highest mental stat to better support edge cases. Formula is 10 + 1/2 CL + Highest Mental Stat.
        * Horsemaster prerequisites now restrict it from some cavalier archetypes
        * Persistent Metamagic now can be applied to a few more spells
    * Mythic Feats
        * Expanded Arsenal can no longer be used to stack multiple spell focus feats on the same school to increase DC
    * Spells
        * Abyssal Storm no longer saves for half and no longer kills the caster.
        * Acid Maw no longer causes excessive damage instances to trigger when attacking.
        * Chain Lightning now respects the CL 20 cap for its damage dice.
        * Eye Of The Sun now deals the correct amount of damage.
        * Firebrand no longer causes excessive damage instances to trigger when attacking.
        * Flamestrike now properly respects the reflex saving throw with the divine portion of its damage.
        * Geniekind no longer causes excessive damage instances to trigger when attacking.
        * Hellfire Ray no longer has the Fire descriptor and correctly splits damage half and half.
        * Magical Vestment now works correctly when used by enemies in prebuffs.
        * Magical Vestment can now be correctly dispeled.
        * Magical Vestment now grants a non stacking armor bonus if no armor is equiped.
        * Microscopic Proportions now correctly grants a size bonus instead of an untyped bonus.
        * Remove Fear no longer grants immunity to shaken and fear.
        * Remove Sickness no longer grants immunity to sickness and nausea.
        * Shadow Conjuration Greater now has the correct shadow factor of 60 instead of 40.
        * Shadow Evocation Greater now has the correct shadow factor of 60 instead of 40.
        * Starlight is no longer affected by true sight as it is not an illusion effect.
        * Sun Form now deals the correct amount of damage.
        * Unbreakable Heart no longer grants complete immunity to confusion and emotion effects and instead supresses correctly.
    * Enemies
        * Balors now correctly get their vorpal weapons. Be afraid.
    * Items
        * Aspect of the Asp will now actually deal bonus damage on ray spells.
        * The Vorpal weapon enchant now works.
        * Radiance now correctly grants spell resistance instead of spell penetrations.
        * Finnean now always deals the correct amount of damage.
    * Aeon
        * Aeon Demythication should now actually suppress mythic effects.
    * Demon
        * Balor transformation now properly gets a vorpal weapon.
    * Angel
        * Angel Unbroken DR should now stack with all other DR.
    * Cleric
        * Glory domain no longer grants an untyped bonus the the raw Charsima stat.
    * Magus
        * Spell combat/strike is now properly restricted to the magus spellbook instead of the magus spell list.
    * Paladin
        * Smite Evil/Smite Chaos/Mark of Justice attack bonus no longer stacks.
    * Rogue
        * Slippery Mind is now an advanced talent like PnP
        * Sylvan Trickster Fey Tricks now includes all rogue talents
    * Shaman
        * Ameliorating Hex no longer grants complete immunity to effects and instead supresses correctly.
    * Sorcerer
        * Updated Draconic Bloodline arcana descriptions to better match their effects.
    * Warpriest
        * Air Major blessing no longer causes excessive damage instances.
        * Earth Minor blessing no longer causes excessive damage instances.
        * Fire Minor blessing no longer causes excessive damage instances.
        * Water Minor blessing no longer causes excessive damage instances.
        * Weather Minor blessing no longer causes excessive damage instances.
        * Luck Blessing now grants the correct major blessing
    * Witch
        * Agility Patron now gets Animal Shapes at 16th level and Shapechange at 18th.
        * Ameliorating Hex no longer grants complete immunity to effects and instead supresses correctly.
        * Major Ameliorating Hex no longer grants complete immunity to effects and instead supresses correctly.
        * Removed unneeded witch patches
* UI Tweaks
    * Dynamic item naming of armor no longer includes the enhancement bonus of the armor when it does not deviate from the original value.
    * Spell tooltips now display what spellbook they are from.
    * Suppressed buffs now have custom UI rules to better indicate them.
* Homebrew Reworks
    * Bolstered Spell
        * Increased the spell level increase from bolstered from +1 to +2.
    * Elemental Barrage
        * Elemental Barrage has been reworked to move it away from attack enhancment stacking with weapon enchants, to instead act more like a caster ability.
            * Every time you deal elemental damage to a creature with a spell, you apply an elemental mark to it. If during the next three rounds the marked target takes elemental damage from any source with a different element, the target is dealt additional Divine damage. The damage is 1d6 per mythic rank of your character.
    * Mythic Sneak Attack
        * Mythic Sneak Attack now increases the size of your sneak attack dice (ex: d6 -> d8) instead of addding an additional sneak attack dice.
* Added Metamagic
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
* Added Feats
    * Dispel Focus
        * Whenever you attempt a dispel check based on your caster level, you gain a +2 bonus on the check.
    * Greater Dispel Focus
        * Whenever you attempt a dispel check based on your caster level, you gain a +2 bonus to the check. This stacks with the bonus from Dispel Focus.
    * Quicken Blessing
        * Choose one of your blessings that normally requires a standard action to use. You can expend two of your daily uses of blessings to deliver that blessing (regardless of whether it’s a minor or major effect) as a swift action instead.
    * Two-Weapon Defense
        * When wielding a double weapon or two weapons (not including natural weapons or unarmed strikes), you gain a +1 shield bonus to your AC. When you are fighting defensively or using the total defense action, this shield bonus increases to +2.
    * Varisian Tattoo
        * Select a school of magic in which you have Spell Focus. Spells from this school are cast at +1 caster level.
* Added Mythic Abilities
    * Abundant Blessing
        * You can use Blessings a number of additional times per day equal to your mythic rank.
    * Abundant Bombs
        * You can throw a number of additional bombs per day equal to twice your mythic rank.
    * Abundant Fervor
        * You can use Fervor a number of additional times per day equal to your mythic rank.
    * Abundant Incense
        * The number of rounds per day you can use Incense Fog increases by a number of rounds equal to your mythic rank.
    * Abundant Lay On Hands
        * You can use Lay On Hands a number of additional times per day equal to your mythic rank.
    * Enhanced Blessings
        * The effects from your blessings now last twice as long.
    * Favorite Metamagic Persistent
        * Select one kind of metamagic. The spell level cost for its use decreases by one (to a minimum of 0).
    * Favorite Metamagic Selective
        * Select one kind of metamagic. The spell level cost for its use decreases by one (to a minimum of 0).
    * Harmonious Mage
        * Preparing spells from one of your opposition schools now only requires one spell slot of the appropriate level instead of two.
    * Impossible Blessing
        * You gain one more blessing, ignoring all blessing prerequisites.
    * Second Patron
        * You select a second patron, gaining all its benifits.
* Added Mythic Feats
    * Combat Expertise (Mythic)
        * Whenever you use Combat Expertise, you gain an additional +2 dodge bonus to your Armor Class.
    * Combat Reflexes (Mythic)
        * You can make any number of additional attacks of opportunity per round.
    * Intimidating Prowess (Mythic)
        * You gain a bonus on Intimidate checks equal to your mythic rank.
    * Manyshot (Mythic)
        * When making a full-attack action with a bow and using Manyshot, you fire two arrows with both your first and second attacks, instead of just your first attack.
    * Titan Strike
        * Your unarmed strike deals damage as if you were one size category larger. You also gain a +1 bonus for each size category that your target is larger than you on the following: bull rush, drag, grapple, overrun, sunder, and trip combat maneuver checks and the DC of your Stunning Fist.
    * Two-Weapon Defense (Mythic)
        * When using Two-Weapon Defense, you apply the highest enhancement bonus from your two weapons to the shield bonus granted by that feat.
    * Warrior Priest (Mythic)
        * You gain a bonus equal to half your mythic rank both on initiative checks and on concentration checks.

## Version 1.10.4
* Now works with 1.1.6e
* Fix for trick riding not working properly in all cases
* Allied Spellcaster no longer applies globally
* Fixed Aeon Greater Bane applying twice
* Removed unneeded Second Breath patch

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
