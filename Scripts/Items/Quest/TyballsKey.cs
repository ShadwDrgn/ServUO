namespace Server.Items
{
    public class TyballsKey : AbyssKey
    {
        [Constructable]
        public TyballsKey()
            : base(0x1012)
        {
            this.Hue = 0x489;
            this.Weight = 1.0;
            this.Name = "Tyball's Key";
            this.Movable = false;
        }

        public TyballsKey(Serial serial)
            : base(serial)
        {
        }

        // public override int LabelNumber { get { return 1111648; } } //Yellow Key
        public override int Lifespan
        {
            get
            {
                return 21600;
            }
        }
        public override void OnDoubleClick(Mobile m)
        {
            Item a = m.Backpack.FindItemByType(typeof(RedKey1));
            if (a != null)
            {
                Item b = m.Backpack.FindItemByType(typeof(BlueKey1));
                if (b != null)
                {
                    m.AddToBackpack(new TripartiteKey());
                    a.Delete();
                    b.Delete();
                    this.Delete();
                    m.SendLocalizedMessage(1111649);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}