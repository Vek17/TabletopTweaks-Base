using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    internal class Skald {
        public static void AddSkaldFeatures() {
            var SkaldProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("26418fed2bc153245972a5b54204ed75");
            var SkaldSpellKenning = BlueprintTools.GetBlueprint<BlueprintFeature>("d385b8c302e720c43aa17b8170bc6ae2");
            var SkaldSpellKenningExtraUse = BlueprintTools.GetBlueprint<BlueprintFeature>("590d0f09d7da13d4a9382d144b8439f6");

            var SkaldSpellKenningResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "SkaldSpellKenningResource", bp => {
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount() {
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    BaseValue = 1,
                    LevelIncrease = 0,
                    IncreasedByLevel = false,
                    IncreasedByStat = false
                };
            });

            SkaldSpellKenningExtraUse.TemporaryContext(bp => {
                bp.Ranks = 2;
                bp.SetComponents();
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = SkaldSpellKenningResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 1;
                });
            });

            SkaldSpellKenning.TemporaryContext(bp => {
                bp.Ranks = 1;
                bp.SetComponents();
                bp.AddComponent<SpellKenningComponent>(c => {
                    c.m_Resource = SkaldSpellKenningResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_SpellLists = new BlueprintSpellListReference[] { 
                        SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.ClericSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.BardSpellList.ToReference<BlueprintSpellListReference>()
                    };
                    c.m_Spellbook = SpellTools.Spellbook.SkaldSpellbook.ToReference<BlueprintSpellbookReference>();
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = SkaldSpellKenningResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
            });

            SkaldProgression.UIGroups = SkaldProgression.UIGroups.AppendToArray(Helpers.CreateUIGroup(SkaldSpellKenning, SkaldSpellKenningExtraUse));
        }
    }
}
