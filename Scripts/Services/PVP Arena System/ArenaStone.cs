using Server;
using System;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.ArenaSystem
{
    public class ArenaStone : Item
    {
        public override bool ForceShowProperties { get { return true; } }
        public override int LabelNumber { get { return 1115878; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public PVPArena Arena { get; set; }

        [Constructable]
        public ArenaStone(PVPArena arena)
            : base(0xEDD)
        {
            Arena = arena;

            Movable = false;
            Hue = 1194;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(Location, 10))
            {
                if (Arena != null && PVPArenaSystem.Enabled)
                {
                    var duel = Arena.GetPendingDuel(from);

                    if (duel == null)
                    {
                        BaseGump.SendGump(new ArenaStoneGump(from as PlayerMobile, Arena));
                    }
                    else
                    {
                        BaseGump.SendGump(new PendingDuelGump(from as PlayerMobile, duel, Arena));
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(502138); // That is too far away for you to use.
            }
        }

        public ArenaStone(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}