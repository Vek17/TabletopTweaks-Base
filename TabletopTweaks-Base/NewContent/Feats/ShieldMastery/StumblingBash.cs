using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ShieldMastery {
    static class StumblingBash {
        internal static void AddStumblingBash() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ShieldFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("ac57069b6bf8c904086171683992a92a");
            var ShieldBashFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("121811173a614534e8720d7550aae253");
            var ShieldBashBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5566971fdebf7fe468a497bbee0d3ed5");

            var StumblingBashBuff = Helpers.CreateBuff(TTTContext, "StumblingBashBuff", bp => {
                bp.SetName(TTTContext, "Stumbling Bash");
                bp.SetDescription(TTTContext, "Creature's AC is reduced by –2 after being hit by a shield bash.");
                bp.m_Icon = ShieldBashFeature.Icon;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Value = -2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });
            var StumblingBashEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "StumblingBashEffect", bp => {
                bp.SetName(TTTContext, "Stumbling Bash");
                bp.SetDescription(TTTContext, "Your shield bash causes your enemies to falter.\n" +
                    "Benefit: Creatures struck by your shield bash take a –2 penalty to their AC until the end of your next turn.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.WeaponLightShield;
                    c.Action = CreateStumblingBashActions();
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.SpikedLightShield;
                    c.Action = CreateStumblingBashActions();
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.WeaponHeavyShield;
                    c.Action = CreateStumblingBashActions();
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.SpikedHeavyShield;
                    c.Action = CreateStumblingBashActions();
                });
                ActionList CreateStumblingBashActions() {
                    return Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = StumblingBashBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            };
                        }));
                }
            });
            var StumblingBashFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "StumblingBashFeature", bp => {
                bp.SetName(StumblingBashEffect.m_DisplayName);
                bp.SetDescription(StumblingBashEffect.m_Description);
                bp.m_Icon = ShieldBashFeature.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = StumblingBashEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Buckler,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield,
                    };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 4;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 6;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeature(ShieldBashFeature);
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ShieldFocus);
            });

            if (TTTContext.AddedContent.ShieldMasteryFeats.IsDisabled("StumblingBash")) { return; }
            ShieldMastery.AddToShieldMasterySelection(StumblingBashFeature);
        }
    }
}
