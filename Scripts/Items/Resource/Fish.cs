using System;

namespace Server.Items
{
    public class Fish : Item, ICarvable
    {
        [Constructable]
        public Fish()
            : this(1)
        {
        }

        [Constructable]
        public Fish(int amount)
            : base(Utility.Random(0x09CC, 4))
        {
            this.Stackable = true;
            this.Weight = 1.0;
            this.Amount = amount;
        }

        public Fish(Serial serial)
            : base(serial)
        {
        }

        public void Carve(Mobile from, Item item)
        {
            if (item is IUsesRemaining && Siege.SiegeShard)
            {
                IUsesRemaining uses = item as IUsesRemaining;

                uses.ShowUsesRemaining = true;
                uses.UsesRemaining--;

                if (uses.UsesRemaining <= 0)
                {
                    item.Delete();
                    from.SendLocalizedMessage(1044038); // You have worn out your tool!
                }
            }

            base.ScissorHelper(from, new RawFishSteak(), 4);
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