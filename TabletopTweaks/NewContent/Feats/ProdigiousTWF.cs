using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats
{
    static class ProdigiousTWF
    {

        public static void AddProdigiousTWF()
        {
            var ProdigiousTWF = Helpers.CreateBlueprint<BlueprintFeature>("ProdigiousTWF", bp =>
            {
                bp.SetName("Prodigious Two-Weapon Fighting");
                bp.SetDescription("You may fight with a one-handed weapon in your offhand as if it were a light weapon. In addition, you may use your Strength score instead of your Dexterity score for the purpose of qualifying for Two-Weapon Fighting and any feats with Two-Weapon Fighting as a prerequisite.");
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.AddComponent(Helpers.Create<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Strength;
                    c.Value = 13;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee  ;
                }));
                if (bp.IsPrerequisiteFor == null)
                    bp.IsPrerequisiteFor = new List<BlueprintFeatureReference>();

                

                
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ProdigiousTWF")) { return; }

            FeatTools.AddAsFeat(ProdigiousTWF);
            AlterTWFFeatCascade();    

        }
       
        public static void AlterTWFFeatCascade()
        {
            var Prodigious = Resources.GetModBlueprint<BlueprintFeature>("ProdigiousTWF");

            var TWF = Resources.GetBlueprint<BlueprintFeature>("ac8aaf29054f5b74eb18f2af950e752d");
           
            var BashingFinish = Resources.GetBlueprint<BlueprintFeature>("0b442a7b4aa598d4e912a4ecee0500ff");
            var DoubleSlice = Resources.GetBlueprint<BlueprintFeature>("8a6a1920019c45d40b4561f05dcb3240");
            var ShieldMaster = Resources.GetBlueprint<BlueprintFeature>("dbec636d84482944f87435bd31522fcc");
            var ITWF = Resources.GetBlueprint<BlueprintFeature>("9af88f3ed8a017b45a6837eab7437629");
            var GTWF = Resources.GetBlueprint<BlueprintFeature>("c126adbdf6ddd8245bda33694cd774e8");
            var MTWF = Resources.GetBlueprint<BlueprintFeature>("c6afbb8c1a36a704a8041f35498f41a4");

            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(TWF, StatType.Dexterity, Prodigious, StatType.Strength);
            
            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(BashingFinish, StatType.Dexterity, Prodigious, StatType.Strength);
            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(DoubleSlice, StatType.Dexterity, Prodigious, StatType.Strength);
            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(ShieldMaster, StatType.Dexterity, Prodigious, StatType.Strength);
            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(ITWF, StatType.Dexterity, Prodigious, StatType.Strength);
            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(GTWF, StatType.Dexterity, Prodigious, StatType.Strength);
            FeatTools.PatchFeatWithFeatLockedAlternateAbilityPrereqSimple(MTWF, StatType.Dexterity, Prodigious, StatType.Strength);

            

            
        }
        


    }
}
