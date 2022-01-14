using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewContent.MechanicsChanges;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewContent.MechanicsChanges.MetamagicExtention;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.Feats {
    static class RimeSpell {
        public static void AddRimeSpell() {
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var EntangleBuff = Resources.GetBlueprint<BlueprintBuff>("f7f6330726121cf4b90a6086b05d2e38");
            var IcyPrisonEntangledBuff = Resources.GetBlueprint<BlueprintBuff>("c53b286bb06a0544c85ca0f8bcc86950");
            var Icon_RimeSpellFeat = AssetLoader.LoadInternal("Feats", "Icon_RimeSpellFeat.png");
            var Icon_RimeSpellMetamagic = AssetLoader.LoadInternal("Metamagic", "Icon_RimeSpellMetamagic.png", 128);

            var RimeSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>("RimeSpellFeat", bp => {
                bp.SetName("Metamagic (Rime Spell)");
                bp.SetDescription("Creatures damaged by your spells with the cold descriptor become entangled.\n" +
                    "Benefit: The frost of your cold spell clings to the target, impeding it for a short time. " +
                    "A rime spell causes creatures that takes cold damage from the spell to become entangled " +
                    "for a number of rounds equal to the original level of the spell.\n" +
                    "This feat only affects spells with the cold descriptor.\n" +
                    "Level Increase: +1 (a rime spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_RimeSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Rime;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic | FeatureTag.Metamagic;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 3;
                });
                bp.AddComponent<RecommendationRequiresSpellbook>();
            });

            var FavoriteMetamagicRime = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicRime", bp => {
                bp.SetName("Favorite Metamagic — Rime");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicRime;
                });
                bp.AddPrerequisiteFeature(RimeSpellFeat);
            });

            var RimeEntagledBuff = Helpers.CreateBuff("RimeEntagledBuff", bp => {
                bp.m_DisplayName = EntangleBuff.m_DisplayName;
                bp.m_Description = EntangleBuff.m_Description;
                bp.m_Icon = IcyPrisonEntangledBuff.Icon;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.ResourceAssetIds = IcyPrisonEntangledBuff.ResourceAssetIds;
                bp.FxOnStart = IcyPrisonEntangledBuff.FxOnStart;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Cold | SpellDescriptor.MovementImpairing;
                });
                bp.AddComponent<AddCondition>(c => {
                    c.Condition = UnitCondition.Entangled;
                });
                bp.AddComponent<RemoveWhenCombatEnded>();
            });
            
            if (ModSettings.AddedContent.Feats.IsDisabled("MetamagicRimeSpell")) { return; }
            MetamagicExtention.RegisterMetamagic(
                metamagic: (Metamagic)CustomMetamagic.Rime,
                name: "Rime",
                icon: Icon_RimeSpellMetamagic,
                defaultCost: 1,
                favoriteMetamagic: CustomMechanicsFeature.FavoriteMetamagicRime
            );
            UpdateSpells();
            FeatTools.AddAsFeat(RimeSpellFeat);
            FeatTools.AddAsMetamagicFeat(RimeSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicRime);
        }
        private static void UpdateSpells() {
            var spells = SpellTools.SpellList.AllSpellLists
                .SelectMany(list => list.SpellsByLevel)
                .Where(spellList => spellList.SpellLevel != 0)
                .SelectMany(level => level.Spells)
                .Distinct()
                .OrderBy(spell => spell.Name)
                .ToArray();
            foreach (var spell in spells) {
                bool isColdSpell = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .Any(s => s.GetComponent<SpellDescriptorComponent>()?
                        .Descriptor.HasAnyFlag(SpellDescriptor.Cold) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>();
                if (isColdSpell) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Rime)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Rime;
                        Main.LogPatch("Enabled Rime Metamagic", spell);
                    }
                };
            }
        }
    }
}
