using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System.Linq;
using Kingmaker.RuleSystem.Rules;
using HarmonyLib;
using Kingmaker.RuleSystem;
using System;

namespace TabletopTweaks {
    static class BalanceAdjustments {

        public static void patchNaturalArmorEffects() {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            patchAnimalCompanionFeatures();
            patchItemBuffs();
            patchClassFeatures();
            patchFeats();

            void patchAnimalCompanionFeatures() {
                BlueprintFeature animalCompanionNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0d20d88abb7c33a47902bd99019f2ed1");
                BlueprintFeature animalCompanionStatFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                BlueprintFeature[] animalCompanionUpgrades = Resources.Blueprints
                    .OfType<BlueprintFeature>()
                    .Where(bp => bp.name.Contains("AnimalCompanionUpgrade"))
                    .ToArray();

                animalCompanionNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
                animalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                Main.Log($"animalCompanionStatFeature NaturalArmor: {animalCompanionStatFeature.GetComponents<AddContextStatBonus>().Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor).Count()}");
                Main.Log($"animalCompanionStatFeature ArmorFocus: {animalCompanionStatFeature.GetComponents<AddContextStatBonus>().Where(c => c.Descriptor == ModifierDescriptor.ArmorFocus).Count()}");
                animalCompanionUpgrades.ForEach(bp => {
                    bp.GetComponents<AddStatBonus>()
                    .Where(c => c.Descriptor == ModifierDescriptor.NaturalArmor)
                    .ForEach(c => c.Descriptor = ModifierDescriptor.ArmorFocus);
                });
            }
            void patchClassFeatures() {
                BlueprintFeature dragonDiscipleNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa4f9fd22a07ddb49982500deaed88f9");
                dragonDiscipleNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
            void patchFeats() {
                BlueprintFeature improvedNaturalArmor = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("fe9482aad88e5a54682d47d1df910ce8");
                improvedNaturalArmor.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
            void patchItemBuffs() {
                //Icy Protector
                BlueprintBuff protectionOfColdBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f592ecdb8045d7047a21b20ffee72afd");
                protectionOfColdBuff.GetComponent<AddStatBonus>().Descriptor = ModifierDescriptor.ArmorFocus;
            }
        }
        public static void patchPolymorphEffects() {
            if (!Resources.Settings.DisablePolymorphStacking) { return; }
        }
    }

    //Patch Natural Armor Stacking
    [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
    static class ModifierDescriptorHelper_IsStackable_Patch {

        static void Postfix(ref bool __result, ref ModifierDescriptor descriptor) {
            if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
            if (descriptor == ModifierDescriptor.NaturalArmor) {
                __result = descriptor == ModifierDescriptor.NaturalArmor ? false : __result;
            }
        }
    }

    //Patch Polymorph Stacking
    [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
    static class RuleCanApplyBuff_OnTrigger_Patch {
        static private string[] polymorphBuffs = {
            "00d8fbe9cf61dc24298be8d95500c84b",
            "02611a12f38bed340920d1d427865917",
            "02a143c601ee28b479dd409012779056",
            "051c8dea7acf6aa41b8b1c1f65cda421",
            "070543328d3e9af49bb514641c56911d",
            "082caf8c1005f114ba6375a867f638cf",
            "0a52d8761bfd125429842103aed48b90",
            "0c0afabcfddeecc40a1545a282f2bec8",
            "0d29c50c956e82d4eae56710987de9f7",
            "0d516ffcd8a663d4aa9c418cd380a7f1",
            "1032d4ffb1c56444ca5bfce2c778614d",
            "103a680886ba18742a40b840c3b237f6",
            "1334f902291e41241ad1421e2470aa50",
            "13c8e843d01eef5479efcd6a9adac432",
            "16857109dafc2b94eafd1e888552ef76",
            "17d330af03f5b3042a4417ab1d45e484",
            "187f88d96a0ef464280706b63635f2af",
            "1a482859d9513e4418f57abcd396d315",
            "1d3d388fd7b740842bde43dfb0aa56bb",
            "1d498104f8e35e246b5d8180b0faed43",
            "200bd9b179ee660489fe88663115bcbc",
            "216343c3fcd06a847b2b696dc8ad6fb9",
            "234543d86f2229f4bb5620b64f61dbf9",
            "2641f73f8d7864f4bba0bd6134018094",
            "2652c61dff50a24479520c84005ede8b",
            "268fafac0a5b78c42a58bd9c1ae78bcf",
            "2814792301df65b4e866acaac9864256",
            "294cbb3e1d547f341a5d7ec8500ffa44",
            "2b0b1321fdc53df4dabae1cbf33d46f4",
            "2d294863adf81f944a7558f7ae248448",
            "31e87010b70f3374a83819a3f3578dad",
            "37cad1e28c452ec48810cfcf342bffd7",
            "3ad9580c0043da6408712483f5d9bfa5",
            "3af4d4bc55fa0ae4e851708d7395f1dd",
            "3c7c12df25d21b344b7cbe12a60038d8",
            "3e3f33fb3e581ab4e8923a5eabd15923",
            "40a96969339f3c241b4d989910f255e1",
            "4122d10b461a30e4289031087e814fa3",
            "4300f60c00ecabc439deab11ce6d738a",
            "469a412c607bf4f43aabe62c2de22837",
            "470fb1a22e7eb5849999f1101eacc5dc",
            "49a77c5c5266c42429f7afbb038ada60",
            "4f37fc07fe2cf7f4f8076e79a0a3bfe9",
            "50ab9c820eb9cf94d8efba3632ad5ce2",
            "50e01ac78d63e544ca881ae89d23ee3a",
            "5134534352b09884fb0495c36585aabc",
            "53808be3c2becd24dbe572f77a7f44f8",
            "53e408cab2331bd48a3db846e531dfe8",
            "56b844a14fcce03429e3e8a2a26cf595",
            "5993b78c793667e45bf0380e9275fab7",
            "5d2f3863ead92824ab47b11ef949c611",
            "5da0aab9f4d98b143b63aae9997b84a7",
            "640cba8527ea0c34d8b012a9a2a07f0f",
            "65fdf187fea97c94b9cf4ff6746901a6",
            "69c8bf56daedae0469614ed172ce592b",
            "6b1bb8f9879c15e48a3a696d4b221f24",
            "6ba1229b016317041b17f75e7b0fc686",
            "6be582eb1f6df4f41875c16d919e3b12",
            "6bf59a6fca16f6d4887e3544e5c2f689",
            "6e45ce33323b48c4fb2048d50ec52d37",
            "70828d40058f2d3428bb767eb6e3e561",
            "71fe5a730db173243bda4901bc074780",
            "782d09044e895fa44b9b6d9cca3a52b5",
            "799c8b6ae43c7d741ac7887c984f2aa2",
            "7c547fe47c399fe429e574f86d2b7618",
            "7f30b0f7f3c4b6748a2819611fb236f8",
            "80babfb32011f384ea865d768857da79",
            "814bc75e74f969641bf110addf076ff9",
            "81e3330720af3d04eb65d9a2e7d92abb",
            "82d638a78c1a7704684555189ba85d88",
            "8431e0229c76af74b9b517fdfeb87766",
            "86adb6961605b0a4788bdc688843801d",
            "89669cfba3d9c15448c23b79dd604c41",
            "8abf1c437ebee8048a4a3335efc27eb3",
            "8acd6ac6f89c73b4180191eb63768009",
            "8ad008ee838d9054f8b022a92cd106cb",
            "8dae421e48035a044a4b1a7b9208c5db",
            "933c5cd1113d2ef4a84f55660a744008",
            "95588ef4c39bc044291e2f433c29c247",
            "9c58cfcad11f7fd4cb85e22187fddac7",
            "9e6b7b058bc74fc45903679adcab8553",
            "9eb5ba8c396d2c74c8bfabd3f5e91050",
            "a4993affb4c4ad6429eca6daeb7b86a8",
            "a4cc7169fb7e64a4a8f53bdc774341b1",
            "a6acd3ad1e9fa6c45998d43fd5dcd86d",
            "aa0eb6875ef10de4a9d4db519aa45365",
            "add5378a75feeaf4384766da10ddc40d",
            "adf61123d3dcce14baf1dd6ffe0b2062",
            "b117bc8b41735924dba3fb23318f39ff",
            "b6048f8b7b1598141a66c99df1011eee",
            "ba06b8cff52da9e4d8432144ed6a6d19",
            "bb81fc5f8520dfd45b538df0e0a70eab",
            "bc09681834490cd4e92ec4883f7a9220",
            "bf145574939845d43b68e3f4335986b4",
            "c0e8f767f87ac354495865ce3dc3ee46",
            "c231e0cf7c203644d81e665d6115ae69",
            "c38def68f6ce13b4b8f5e5e0c6e68d08",
            "c3fad2e285b70664c80fc63f4de1c7e9",
            "c5925e7b9e7fc2e478526b4cfc8c6427",
            "cd90f378bacb1e848971e9137fc5a507",
            "cf8b4e861226e0545a6805036ab2a21b",
            "cfb58f71515d6fd49893a10de7984a43",
            "d47f45f29c4cfc0469f3734d02545e0b",
            "d5640d96888dd20489922045fde35059",
            "d8e651cbe94b9b1468dc1118d20d87c7",
            "dae2d173d9bd5b14dbeb4a1d9d9b0edc",
            "db00581a03e6947419648dfba6aa03b2",
            "dc1ef6f6d52b9fd49bc0696ab1a4f18b",
            "e1c5725668f48df48a9676d26aa57fbf",
            "e1c8467e6acca5e4ba1dc5854bc1b41f",
            "e24ea1f5005649846b798318b5238e34",
            "e6f2fc5d73d88064583cb828801212f4",
            "e76500bc1f1f269499bf027a5aeb1471",
            "e85abd773dbce30498efa8da745d7ca7",
            "ea2cd08bdf2ca1c4f8a8870804790cd7",
            "eb52d24d6f60fc742b32fe943b919180",
            "ec6ad3612c4f0e340b54a11a0e78793d",
            "f048ee68bc72da447970025667a77b12",
            "f0826c3794c158c4cbbe9ceb4210d6d6",
            "f0abf98bb3bce4f4e877a8e8c2eccf41",
            "f1a47ec9041f17147adfc17156e05822",
            "f65726a206c68af4085af036f58aca45",
            "f7fdc15aa0219104a8b38c9891cac17b",
            "f8e5768d3306c5a4c815b84f21e7823b",
            "f9b392e80df99734eb689cbb08cadba5",
            "feb2ab7613e563e45bcf9f7ffe4e05c6"
        };

        static void Postfix(RuleCanApplyBuff __instance) {
            if (!Resources.Settings.DisablePolymorphStacking) { return; }
            if (!Array.Exists(polymorphBuffs, id => id.Equals(__instance.Blueprint.AssetGuid))) { return; }
            if (__instance.CanApply && ((__instance.Context.MaybeCaster.Faction == __instance.Initiator.Faction) || __instance.Context.MaybeCaster == __instance.Initiator)) {
                var intesection = __instance.Initiator.Buffs.Enumerable.Select(b => b.Blueprint.AssetGuid).Intersect(polymorphBuffs);
                if (intesection.Any()) {
                    foreach(var buff in intesection.ToArray()) {
                        BlueprintBuff buffToRemove = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(buff);
                        __instance.Initiator.Buffs.GetBuff(buffToRemove).Remove();
                        Main.Log($"Removed Buff: {buffToRemove.Name}");
                    }
                    Main.Log($"Applied Polymorph Buff: {__instance.Context.Name}");
                }
            } else { 
                Main.Log($"Blocked Polymorph Buff (Hostile): {__instance.Context.Name}");
                __instance.CanApply = false;
            }
        }
    }
}
