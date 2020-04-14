namespace Server.Items
{
    public class EthologistTitleDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title { get { return 1157594; } } // Ethologist

        [Constructable]
        public EthologistTitleDeed()
        {
        }

        public EthologistTitleDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();
        }
    }
}