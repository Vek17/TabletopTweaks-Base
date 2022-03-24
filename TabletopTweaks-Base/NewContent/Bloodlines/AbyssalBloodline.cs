using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Bloodlines {
    class AbyssalBloodline {
        public static void AddBloodragerAbyssalDemonicBulkEnlargeBuff() {
            var BloodragerAbyssalDemonicBulkEnlargeBuff = Helpers.CreateBuff(TTTContext, "BloodragerAbyssalDemonicBulkEnlargeBuff", bp => {
                bp.SetName(TTTContext, "Abyssal Bulk");
                bp.SetDescription(TTTContext, "At 4th level, when entering a bloodrage, you can choose to grow one size category larger than your base size (as enlarge person) even if you aren't humanoid.");
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
        }
    }
}
