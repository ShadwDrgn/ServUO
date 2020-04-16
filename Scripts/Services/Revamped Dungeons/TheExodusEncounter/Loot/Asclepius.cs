namespace Server.Items
{
    public class Asclepius : GnarledStaff
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public Asclepius()
        {
        }

        public override bool CanFortify { get { return false; } }

        public Asclepius(Serial serial) : base(serial)
        {
        }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override int LabelNumber { get { return 1153526; } } // Asclepius [Replica]

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            list.Add(1153525); // 15% Bandage Healing Bonus 
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
