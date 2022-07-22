using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.RogueTalents {
    internal static class RogueTalentProperties {
        public static BlueprintUnitProperty RougeLevelProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "RougeLevelProperty", bp => {
            bp.AddComponent<SummClassLevelGetter>(c => {
                c.m_Class = new BlueprintCharacterClassReference[] {
                        ClassTools.Classes.RogueClass.ToReference<BlueprintCharacterClassReference>(),
                        ClassTools.Classes.SlayerClass.ToReference<BlueprintCharacterClassReference>(),
                        ClassTools.Classes.DruidClass.ToReference<BlueprintCharacterClassReference>(),
                    };
                c.m_Archetypes = new BlueprintArchetypeReference[] {
                        BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(TTTContext, "NatureFangArcehtype")
                    };
                c.Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(TTTContext, "NatureFangArcehtype");
            });
        });
    }
}
