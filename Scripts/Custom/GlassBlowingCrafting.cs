using System;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Misc
{
    public static class GlassBlowingCustomCraftData
    {

    public class CraftableSoulstoneToken : SoulstoneToken
    {
        public CraftableSoulstoneToken(Serial serial)
            : base(serial)
        {
        }

        [Constructable]
        public CraftableSoulstoneToken()
            : base(SoulstoneType.White)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }
        public static void Initialize()
        {
            CraftSystem cs = DefGlassblowing.CraftSystem;
            int index;
            index = cs.AddCraft(typeof(CraftableSoulstoneToken), 1044050, 1030899, 75.0, 125.0, typeof(WorkableGlass), 1154170, 3, 1044253);
            cs.AddRes(index, typeof(UntappedPotential), "Untapped Potential", 1);
        }
    }
}
