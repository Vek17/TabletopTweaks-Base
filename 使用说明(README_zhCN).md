## 使用此MOD时请勿向Owlcat官方提交BUG反馈。如果你必须这么做，请注明你使用了这个mod。

TabletopTweaks的设计目的是进行内容性更新，以更好地还原桌面Pathfinder游戏的规则，其最主要的功能是修复被错误应用的战斗系统规则。
此外，此MOD新增了一部分桌游中有、原游戏中未加入、且非常适合电子游戏环境的资源。
注意：此MOD可以在存档中途加入，或在存档中途更新版本。但一旦加入后，不能移除，否则存档会损坏。如果你决定不再想要一部分功能，可以在UMM设置中关闭。由于此MOD绝大部分的功能都可以在设置中关闭，无法移除的特性应该不至于带来太大困扰。
修改此MOD的设置后需要完全重启游戏，更改才会生效。

此MOD由1onepower翻译，你可以在[B站](https://space.bilibili.com/383719917?spm_id_from=333.1007.0.0)关注我。
欢迎所有同好加入最大的中文CRPG社区之一：[拥王者吧](https://tieba.baidu.com/f?kw=%E6%8B%A5%E7%8E%8B%E8%80%85&fr=index)，或我们的中文[discord](https://discord.gg/VHEMVrvgaw)。
如果你可以用英语交流，也欢迎加入Owlcat官方的[discord](https://discord.gg/bQVwsP7cky)
再次特别感谢此MOD的作者@Vek17，制作了如此优秀的内容

**如何安装**

1. 下载 [Unity Mod Manager](https://github.com/newman55/unity-mod-manager), 确保其版本至少为0.23.0。注意，任何非官方的盗版、克隆版MOD加载器(如"DUMM")，以及任何汉化过的UMM加载器都是不受支持的，造成对存档的任何破坏其责任自负。
2. 运行UMM，找到Pathfinder: Wrath of the Righteous，如果UMM没能自动找到游戏路径，请手动设置路径。
3. 在release里下载此MOD的发行包(一个zip压缩文件)
4. 将zip文件拖到UMM内安装。

**功能列表:(汉化暂时未完)**

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
