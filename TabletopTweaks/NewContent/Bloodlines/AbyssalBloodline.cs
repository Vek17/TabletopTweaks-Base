using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Bloodlines {
    class AbyssalBloodline {
        public static void AddBloodragerAbyssalDemonicBulkEnlargeBuff() {
            var BloodragerAbyssalDemonicBulkEnlargeBuff = Helpers.CreateBuff("BloodragerAbyssalDemonicBulkEnlargeBuff", bp => {
                bp.SetName("Abyssal Bulk");
                bp.SetDescription("At 4th level, when entering a bloodrage, you can choose to grow one size category larger than your base size (as enlarge person) even if you aren't humanoid.");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent(Helpers.Create<ChangeUnitSize>(c => {
                    c.SizeDelta = 1;
                }));
                bp.AddComponent(Helpers.Create<AddGenericStatBonus>(c => {
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Size;
                    c.Value = 2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.Strength;
                }));
                bp.AddComponent(Helpers.Create<AddGenericStatBonus>(c => {
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Size;
                    c.Value = -2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;
                }));
            });
            Resources.AddBlueprint(BloodragerAbyssalDemonicBulkEnlargeBuff);
        }
    }
}
