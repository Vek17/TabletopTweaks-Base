using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Skald {
        private static readonly BlueprintGuid GreatKenningMasterID = TTTContext.Blueprints.GetDerivedMaster("GreatKenningMasterID");
        public static BlueprintFeatureSelection SkaldAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var SkaldSpellKenning = BlueprintTools.GetBlueprint<BlueprintFeature>("d385b8c302e720c43aa17b8170bc6ae2");
            var SkaldSpellKenningResource = BlueprintTools.GetModBlueprintReference<BlueprintAbilityResourceReference>(TTTContext, "SkaldSpellKenningResource");
            var MasterSkald = BlueprintTools.GetBlueprint<BlueprintFeature>("ae4d45a39a91dee4fb4200d7a677d9a7");

            var GreatKenning = CreateGreatKenning();
            SkaldAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "SkaldAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = MasterSkald.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    MasterSkald,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
                if (TTTContext.Fixes.Skald.Base.IsEnabled("SpellKenning")) {
                    bp.AddFeatures(GreatKenning);
                }
            });

            BlueprintFeatureSelection CreateGreatKenning() {
                var GreatKenning = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "GreatKenning", bp => {
                    bp.SetName(TTTContext, "Great Kenning");
                    bp.SetDescription(TTTContext, "At 20th level, the skald’s knowledge of other magic grows ever wider.\n" +
                        "The skald can use spell kenning three additional times per day and can select one additional spell list from which he can cast spells with spell kenning.");
                    bp.IsClassFeature = true;
                    bp.AddComponent<IncreaseResourceAmount>(c => {
                        c.m_Resource = SkaldSpellKenningResource;
                        c.Value = 3;
                    });
                    bp.IsClassFeature = true;
                    bp.Obligatory = true;
                    bp.Ranks = 1;
                    bp.Groups = new FeatureGroup[] { };
                    bp.AddPrerequisiteFeature(SkaldSpellKenning);
                });
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.BloodragerSpellList, SpellTools.SpellCastingClasses.BloodragerClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.DruidSpellList, SpellTools.SpellCastingClasses.DruidClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.HunterSpelllist, SpellTools.SpellCastingClasses.HunterClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.InquisitorSpellList, SpellTools.SpellCastingClasses.InquisitorClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.MagusSpellList, SpellTools.SpellCastingClasses.MagusClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.PaladinSpellList, SpellTools.SpellCastingClasses.PaladinClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.RangerSpellList, SpellTools.SpellCastingClasses.RangerClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.ShamanSpelllist, SpellTools.SpellCastingClasses.ShamanClass, GreatKenning));
                GreatKenning.AddFeatures(CreateGreatKenningFeature(SpellTools.SpellList.WitchSpellList, SpellTools.SpellCastingClasses.WitchClass, GreatKenning));
                return GreatKenning;
            }
            BlueprintFeature CreateGreatKenningFeature(BlueprintSpellList spellList, BlueprintCharacterClass characterClass, BlueprintFeatureSelection selection) {
                return Helpers.CreateDerivedBlueprint<BlueprintFeature>(TTTContext, $"GreatKenning{spellList.name.Replace("Spelllist", "")}",
                    GreatKenningMasterID,
                    new SimpleBlueprint[] { spellList },
                    bp => {
                        bp.SetName(characterClass.LocalizedName);
                        bp.SetDescription(selection.m_Description);
                        bp.IsClassFeature = true;
                        bp.Groups = new FeatureGroup[] { FeatureGroup.ArcaneTricksterSpellbook };
                        bp.HideInUI = true;
                        bp.HideNotAvailibleInUI = true;
                        bp.AddComponent<AddSpellKenningSpellList>(c => {
                            c.m_SpellLists = new BlueprintSpellListReference[] { spellList.ToReference<BlueprintSpellListReference>() };
                        });
                    });
            }
        }
    }
}
