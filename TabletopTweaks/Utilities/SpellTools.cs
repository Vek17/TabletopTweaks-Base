using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Linq;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Utilities {
    static class SpellTools {
        public static void AddToSpellList(this BlueprintAbility spell, BlueprintSpellList spellList, int level) {
            AddComponentIfMissing(spellList);
            AddToListIfMissing(spellList);
            if (spellList == SpellList.WizardSpellList) {
                var school = spell.School;
                var specialistList = specialistSchoolList.Value[(int)school];
                specialistList?.SpellsByLevel[level].Spells.Add(spell);
                AddComponentIfMissing(spellList);
                AddToListIfMissing(spellList);

                for (int i = 0; i < thassilonianSchoolList.Value.Length; i++) {
                    if (thassilonianOpposedSchools.Value[i] != null && !thassilonianOpposedSchools.Value[i].Contains(school)) {
                        AddComponentIfMissing(thassilonianSchoolList.Value[i]);
                        AddToListIfMissing(thassilonianSchoolList.Value[i]);
                    }
                }

                if (school == SpellSchool.Enchantment || school == SpellSchool.Illusion) {
                    AddComponentIfMissing(SpellList.FeyspeakerSpelllist);
                    AddToListIfMissing(SpellList.FeyspeakerSpelllist);
                }
            }
            void AddComponentIfMissing(BlueprintSpellList list) {
                if (list == null) { return; }
                if (!spell.GetComponents<SpellListComponent>().Any(c => c.m_SpellList.Get() == list && c.SpellLevel == level)) {
                    var comp = new SpellListComponent {
                        SpellLevel = level,
                        m_SpellList = list.ToReference<BlueprintSpellListReference>()
                    };
                    spell.AddComponent(comp);
                }
            }
            void AddToListIfMissing(BlueprintSpellList list) {
                if (list == null) { return; }
                if (!list.SpellsByLevel[level].Spells.Contains(spell)) {
                    list.SpellsByLevel[level].Spells.Add(spell);
                }
            }
        }
        static readonly Lazy<BlueprintSpellList[]> specialistSchoolList = new Lazy<BlueprintSpellList[]>(() => {
            var result = new BlueprintSpellList[(int)SpellSchool.Universalist + 1];
            result[(int)SpellSchool.Abjuration] = SpellList.WizardAbjurationSpellList;
            result[(int)SpellSchool.Conjuration] = SpellList.WizardConjurationSpellList;
            result[(int)SpellSchool.Divination] = SpellList.WizardDivinationSpellList;
            result[(int)SpellSchool.Enchantment] = SpellList.WizardEnchantmentSpellList;
            result[(int)SpellSchool.Evocation] = SpellList.WizardEvocationSpellList;
            result[(int)SpellSchool.Illusion] = SpellList.WizardIllusionSpellList;
            result[(int)SpellSchool.Necromancy] = SpellList.WizardNecromancySpellList;
            result[(int)SpellSchool.Transmutation] = SpellList.WizardTransmutationSpellList;
            return result;
        });
        static readonly Lazy<BlueprintSpellList[]> thassilonianSchoolList = new Lazy<BlueprintSpellList[]>(() => {
            var result = new BlueprintSpellList[(int)SpellSchool.Universalist + 1];
            result[(int)SpellSchool.Abjuration] = SpellList.ThassilonianAbjurationSpellList;
            result[(int)SpellSchool.Conjuration] = SpellList.ThassilonianConjurationSpellList;
            result[(int)SpellSchool.Enchantment] = SpellList.ThassilonianEnchantmentSpellList;
            result[(int)SpellSchool.Evocation] = SpellList.ThassilonianEvocationSpellList;
            result[(int)SpellSchool.Illusion] = SpellList.ThassilonianIllusionSpellList;
            result[(int)SpellSchool.Necromancy] = SpellList.ThassilonianNecromancySpellList;
            result[(int)SpellSchool.Transmutation] = SpellList.ThassilonianTransmutationSpellList;
            return result;
        });
        static readonly Lazy<SpellSchool[][]> thassilonianOpposedSchools = new Lazy<SpellSchool[][]>(() => {
            var result = new SpellSchool[(int)SpellSchool.Universalist + 1][];

            result[(int)SpellSchool.Abjuration] = new SpellSchool[] { SpellSchool.Evocation, SpellSchool.Necromancy };
            result[(int)SpellSchool.Conjuration] = new SpellSchool[] { SpellSchool.Evocation, SpellSchool.Illusion };
            result[(int)SpellSchool.Enchantment] = new SpellSchool[] { SpellSchool.Necromancy, SpellSchool.Transmutation };
            result[(int)SpellSchool.Evocation] = new SpellSchool[] { SpellSchool.Abjuration, SpellSchool.Conjuration };
            result[(int)SpellSchool.Illusion] = new SpellSchool[] { SpellSchool.Conjuration, SpellSchool.Transmutation };
            result[(int)SpellSchool.Necromancy] = new SpellSchool[] { SpellSchool.Abjuration, SpellSchool.Enchantment };
            result[(int)SpellSchool.Transmutation] = new SpellSchool[] { SpellSchool.Enchantment, SpellSchool.Illusion };
            return result;
        });
        public static class SpellList {
            public static BlueprintSpellList AeonSpellList => Resources.GetBlueprint<BlueprintSpellList>("24b0c796f723a144e9891b6c4794c595");
            public static BlueprintSpellList AeonSpellMythicList => Resources.GetBlueprint<BlueprintSpellList>("ca8c6024bd2519f4b97162a3ad286920");
            public static BlueprintSpellList AirDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("9678d121f669f864d9da5dabf1ca1ce0");
            public static BlueprintSpellList AlchemistSpellList => Resources.GetBlueprint<BlueprintSpellList>("f60d0cd93edc65c42ad31e34a905fb2f");
            public static BlueprintSpellList AngelClericSpelllist => Resources.GetBlueprint<BlueprintSpellList>("c074062863fbc1e4bab02f9e6e4eb78d");
            public static BlueprintSpellList AngelMythicSpelllist => Resources.GetBlueprint<BlueprintSpellList>("deaffb4218ccf2f419ffd6e41603131a");
            public static BlueprintSpellList AnimalDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("d0ccda70fddc0e346a227068502771c1");
            public static BlueprintSpellList ArmagsBladeSpellList => Resources.GetBlueprint<BlueprintSpellList>("3ea72a95cce88b4449a917ad0a0f36da");
            public static BlueprintSpellList ArtificeDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("5ef652f325d21414d8565fcfb19d8177");
            public static BlueprintSpellList AzataMythicSpelllist => Resources.GetBlueprint<BlueprintSpellList>("db216faa0ff2b984399e7495755c7111");
            public static BlueprintSpellList AzataMythicSpellsSpelllist => Resources.GetBlueprint<BlueprintSpellList>("10c634d2b386d8d41b18a889adb8cd49");
            public static BlueprintSpellList BardSpellList => Resources.GetBlueprint<BlueprintSpellList>("25a5013493bdcf74bb2424532214d0c8");
            public static BlueprintSpellList BattleSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("e788846aaf449404db11e51182174be8");
            public static BlueprintSpellList BloodragerSpellList => Resources.GetBlueprint<BlueprintSpellList>("98c05aeff6e3d384f8aec6d584973642");
            public static BlueprintSpellList BonesSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("24dc5eb552a9e2c4cb9a17c355a80d2e");
            public static BlueprintSpellList ChaosDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("2ff9831eb262758449287f820108428d");
            public static BlueprintSpellList CharmDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("31c742d02fc33204cad4e02dddf028dd");
            public static BlueprintSpellList ClericSpellList => Resources.GetBlueprint<BlueprintSpellList>("8443ce803d2d31347897a3d85cc32f53");
            public static BlueprintSpellList CommunityDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("75576ed8cab010644a11f9ecd512a7f9");
            public static BlueprintSpellList DarknessDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("fa79e8d1fe20b0e43bf3ebca4cef93b9");
            public static BlueprintSpellList DeathDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("436986e90d1e81b45a1accb6fa7261f0");
            public static BlueprintSpellList DemonSpelllist => Resources.GetBlueprint<BlueprintSpellList>("abb1991bf6e996348bb743471ee7e1c1");
            public static BlueprintSpellList DemonUsualSpelllist => Resources.GetBlueprint<BlueprintSpellList>("78721d556676f264da947bfe263b1da0");
            public static BlueprintSpellList DestructionDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("6f9fe425726026d4f9c28c32d5d03481");
            public static BlueprintSpellList DruidSpellList => Resources.GetBlueprint<BlueprintSpellList>("bad8638d40639d04fa2f80a1cac67d6b");
            public static BlueprintSpellList EarthDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("df3bc5bda7deb9d46b0f177db3bb7876");
            public static BlueprintSpellList EvilDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("81bff1165d9468a44b2f815f7c26a373");
            public static BlueprintSpellList FeyspeakerSpelllist => Resources.GetBlueprint<BlueprintSpellList>("640b4c89527334e45b19d884dd82e500");
            public static BlueprintSpellList FireDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("d8f30625d1b1f9d41a24446cbf7ac52e");
            public static BlueprintSpellList FlamesSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("659fbc54fc519b44dacacc78e7d46dec");
            public static BlueprintSpellList FrostSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("bbae401660bbad94c865d71029d8439e");
            public static BlueprintSpellList GloryDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("7b3506924ed8354419b7829736ab2c7e");
            public static BlueprintSpellList GoodDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("dc242eb60eed94a4eb0640d773780090");
            public static BlueprintSpellList HealingDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("033b2b6a8899be844ae8aa91d4dab477");
            public static BlueprintSpellList HunterSpelllist => Resources.GetBlueprint<BlueprintSpellList>("d090b791bfe381740b98ed4ff909b1cf");
            public static BlueprintSpellList InquisitorSpellList => Resources.GetBlueprint<BlueprintSpellList>("57c894665b7895c499b3dce058c284b3");
            public static BlueprintSpellList KnowledgeDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("384627980c2a60a43800f14029fbb8a7");
            public static BlueprintSpellList LawDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("57b0bbdc1114ee846945f1808b13cff7");
            public static BlueprintSpellList LiberationDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("73406d3b9f6290e499c5fa3951a24234");
            public static BlueprintSpellList LichMythicSpelllist => Resources.GetBlueprint<BlueprintSpellList>("a06bcb035c214ad4db605491be9da13d");
            public static BlueprintSpellList LichSkeletalIBardMinorSpelllist => Resources.GetBlueprint<BlueprintSpellList>("0fbd00235f7042841ba1a3fcae0dbc58");
            public static BlueprintSpellList LichSkeletalInquisitorMinorSpelllist => Resources.GetBlueprint<BlueprintSpellList>("c7970aa2f3c94e245942369d348c0a1f");
            public static BlueprintSpellList LichWizardSpelllist => Resources.GetBlueprint<BlueprintSpellList>("7d5987082120bb943ac96cde7b2257ad");
            public static BlueprintSpellList LifeSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("1e6ea0d1d642c8c43ab7e72dd8f607a9");
            public static BlueprintSpellList LuckDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("9e756552e9b05ce459feac658dd2d8fb");
            public static BlueprintSpellList MadnessDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("1d1638d47e7f8404baeed23bc35ec2f2");
            public static BlueprintSpellList MagicDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("f997258a28a9e5f4192d973433edec5f");
            public static BlueprintSpellList MagusSpellList => Resources.GetBlueprint<BlueprintSpellList>("4d72e1e7bd6bc4f4caaea7aa43a14639");
            public static BlueprintSpellList MonsterEmptySpellllist => Resources.GetBlueprint<BlueprintSpellList>("9e4658592e5f66146a6826120e21ed26");
            public static BlueprintSpellList NatureSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("810e2a7009ed3a84c81b398b2763e7a8");
            public static BlueprintSpellList NobilityDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("3de1e283971828f4896a4140acd3c84c");
            public static BlueprintSpellList PaladinSpellList => Resources.GetBlueprint<BlueprintSpellList>("9f5be2f7ea64fe04eb40878347b147bc");
            public static BlueprintSpellList PlantDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("bd7b088a54b79434f90ed53551ca2189");
            public static BlueprintSpellList ProtectionDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("93228f4df23d2d448a0db59141af8aed");
            public static BlueprintSpellList RangerSpellList => Resources.GetBlueprint<BlueprintSpellList>("29f3c338532390546bc5347826a655c4");
            public static BlueprintSpellList ReposeDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("5376474a39713514ca2135d6f9584563");
            public static BlueprintSpellList RuneDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("30076fe3d5f4ef845a7bafb0be57fe44");
            public static BlueprintSpellList ShamanSpelllist => Resources.GetBlueprint<BlueprintSpellList>("c0c40e42f07ff104fa85492da464ac69");
            public static BlueprintSpellList SpiritWardenSpellList => Resources.GetBlueprint<BlueprintSpellList>("767db8f0d64e5b048a8cf4d2ddc10521");
            public static BlueprintSpellList StoneSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("87a3e296757412e45910493e5fed1417");
            public static BlueprintSpellList StrengthDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("03db76fd27428004482f9314c334d1ab");
            public static BlueprintSpellList SunDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("600ffed45d0c3ec43a75dc76bb9377b6");
            public static BlueprintSpellList ThassilonianAbjurationSpellList => Resources.GetBlueprint<BlueprintSpellList>("280dd5167ccafe449a33fbe93c7a875e");
            public static BlueprintSpellList ThassilonianConjurationSpellList => Resources.GetBlueprint<BlueprintSpellList>("5b154578f228c174bac546b6c29886ce");
            public static BlueprintSpellList ThassilonianEnchantmentSpellList => Resources.GetBlueprint<BlueprintSpellList>("ac551db78c1baa34eb8edca088be13cb");
            public static BlueprintSpellList ThassilonianEvocationSpellList => Resources.GetBlueprint<BlueprintSpellList>("17c0bfe5b7c8ac3449da655cdcaed4e7");
            public static BlueprintSpellList ThassilonianIllusionSpellList => Resources.GetBlueprint<BlueprintSpellList>("c311aed33deb7a346ab715baef4a0572");
            public static BlueprintSpellList ThassilonianNecromancySpellList => Resources.GetBlueprint<BlueprintSpellList>("5c08349132cb6b04181797f58ccf38ae");
            public static BlueprintSpellList ThassilonianTransmutationSpellList => Resources.GetBlueprint<BlueprintSpellList>("f3a8f76b1d030a64084355ba3eea369a");
            public static BlueprintSpellList TravelDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("ab90308db82342f47bf0d636fe941434");
            public static BlueprintSpellList TrickeryDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("2c09ae283ea3e41408665c46fcf0303f");
            public static BlueprintSpellList TricksterSpelllist => Resources.GetBlueprint<BlueprintSpellList>("40f8cfe98ca4ebd43894267dbd3fc3ae");
            public static BlueprintSpellList TricksterSpelllistMythic => Resources.GetBlueprint<BlueprintSpellList>("7a5ea54564c7d494794f34d0f5a9abb3");
            public static BlueprintSpellList WarDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("e3c54da90a2b54b4a975a80b5d39c361");
            public static BlueprintSpellList WarpriestSpelllist => Resources.GetBlueprint<BlueprintSpellList>("c5a1b8df32914d74c9b44052ba3e686a");
            public static BlueprintSpellList WaterDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("35e15cd1b353e2d47b507c445d2f8c6f");
            public static BlueprintSpellList WavesSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("190ebde2d9d80c94783bcb73b9106d99");
            public static BlueprintSpellList WeatherDomainSpellList => Resources.GetBlueprint<BlueprintSpellList>("eba577470b8ee8443bb4552433451990");
            public static BlueprintSpellList WindSpiritSpellList => Resources.GetBlueprint<BlueprintSpellList>("0bf6f90fdcb864b4486344100391b478");
            public static BlueprintSpellList WitchSpellList => Resources.GetBlueprint<BlueprintSpellList>("e17df9977b879b64e8a8cbb4b3569f19");
            public static BlueprintSpellList WizardAbjurationSpellList => Resources.GetBlueprint<BlueprintSpellList>("c7a55e475659a944f9229d89c4dc3a8e");
            public static BlueprintSpellList WizardConjurationSpellList => Resources.GetBlueprint<BlueprintSpellList>("69a6eba12bc77ea4191f573d63c9df12");
            public static BlueprintSpellList WizardDivinationSpellList => Resources.GetBlueprint<BlueprintSpellList>("d234e68b3d34d124a9a2550fdc3de9eb");
            public static BlueprintSpellList WizardEnchantmentSpellList => Resources.GetBlueprint<BlueprintSpellList>("c72836bb669f0c04680c01d88d49bb0c");
            public static BlueprintSpellList WizardEvocationSpellList => Resources.GetBlueprint<BlueprintSpellList>("79e731172a2dc1f4d92ba229c6216502");
            public static BlueprintSpellList WizardIllusionSpellList => Resources.GetBlueprint<BlueprintSpellList>("d74e55204daa9b14993b2e51ae861501");
            public static BlueprintSpellList WizardNecromancySpellList => Resources.GetBlueprint<BlueprintSpellList>("5fe3acb6f439db9438db7d396f02c75c");
            public static BlueprintSpellList WizardSpellList => Resources.GetBlueprint<BlueprintSpellList>("ba0401fdeb4062f40a7aa95b6f07fe89");
            public static BlueprintSpellList WizardTransmutationSpellList => Resources.GetBlueprint<BlueprintSpellList>("becbcfeca9624b6469319209c2a6b7f1");
        }
    }
}