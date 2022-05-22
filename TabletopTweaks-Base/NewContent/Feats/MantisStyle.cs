using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class MantisStyle {
        public static void AddMantisStyle() {
            var Icon_MantisStyle = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_MantisStyle.png");
            var ImprovedUnarmedStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");
            var StunningFist = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
            var StunningFistResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("d2bae584db4bf4f4f86dd9d15ae56558");
            var StunningFistAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("732ae7773baf15447a6737ae6547fc1e");
            var StunningFistSickenedFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d256ab3837538cc489d4b571e3a813eb");
            var StunningFistFatigueFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("819645da2e446f84d9b168ed1676ec29");
            var StunningFistFatigueAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("32f92fea1ab81c843a436a49f522bfa1");
            var StunningFistSickenedAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c81906c75821cbe4c897fa11bdaeee01");

            var MantisStyleFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MantisStyleFeature", bp => {
                bp.SetName(TTTContext, "Mantis Style");
                bp.SetDescription(TTTContext, "You have learned to target vital areas with crippling accuracy.\n" +
                    "You gain one additional Stunning Fist attempt per day. While using this style, you gain a +2 bonus to the DC of effects you deliver with your Stunning Fist.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.StyleFeat };
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = StunningFistResource;
                    c.Value = 1;
                });
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrike);
                bp.AddPrerequisiteFeature(StunningFist);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillLoreReligion;
                    c.Value = 3;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific;
                });
            });
            var MantisWisdomFeature = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "MantisWisdomFeature", bp => {
                bp.SetName(TTTContext, "Mantis Wisdom");
                bp.SetDescription(TTTContext, "Your knowledge of vital areas allows you to land debilitating strikes with precision.\n" +
                    "Treat half your levels in classes other than monk as monk levels for determining effects you can apply to a target of your Stunning Fist per the Stunning Fist monk class feature. " +
                    "While using Mantis Style, you gain a +2 bonus on unarmed attack rolls with which you are using Stunning Fist attempts.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddClass(ClassTools.Classes.MonkClass);
                bp.AlternateProgressionType = AlternateProgressionType.Div2;
                bp.ForAllOtherClasses = true;
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(4, StunningFistSickenedFeature),
                    Helpers.CreateLevelEntry(8, StunningFistFatigueFeature),
                };
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrike);
                bp.AddPrerequisiteFeature(StunningFist);
                bp.AddPrerequisiteFeature(MantisStyleFeature);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillLoreReligion;
                    c.Value = 6;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific;
                });
            });
            var MantisTormentFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MantisTormentFeature", bp => {
                bp.SetName(TTTContext, "Mantis Torment");
                bp.SetDescription(TTTContext, "Your knowledge of the mysteries of anatomy allows you to cause debilitating pain with a simple touch.\n" +
                    "You gain one additional Stunning Fist attempt per day. " +
                    "While using Mantis Style, you make an unarmed attack that expends two daily attempts of your Stunning Fist. " +
                    "If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with " +
                    "crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = StunningFistResource;
                    c.Value = 1;
                });
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrike);
                bp.AddPrerequisiteFeature(StunningFist);
                bp.AddPrerequisiteFeature(MantisStyleFeature);
                bp.AddPrerequisiteFeature(MantisWisdomFeature);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillLoreReligion;
                    c.Value = 9;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific;
                });
            });
            var MantisStyleBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MantisStyleBuff", bp => {
                bp.SetName(TTTContext, "Ability Focus — Stunning Fist");
                bp.SetDescription(TTTContext, "Add +2 to the DC for all saving throws against your stunning fist.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistFatigueAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistSickenedAbility;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
            });
            var MantisTormentAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "MantisTormentAbility", bp => {
                bp.SetName(TTTContext, "Ability Focus — Stunning Fist");
                bp.SetDescription(TTTContext, "Add +2 to the DC for all saving throws against your stunning fist.");
                bp.m_Icon = Icon_MantisStyle;
            });
            var MantisStyleToggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "MantisStyleToggle", bp => {
                bp.SetName(TTTContext, "Ability Focus — Stunning Fist");
                bp.SetDescription(TTTContext, "Add +2 to the DC for all saving throws against your stunning fist.");
                bp.m_Icon = Icon_MantisStyle;
                bp.Group = ActivatableAbilityGroup.CombatStyle;
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistFatigueAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistSickenedAbility;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("MantisStyle")) { return; }
            FeatTools.AddAsFeat(MantisStyleFeature);
        }
    }
}