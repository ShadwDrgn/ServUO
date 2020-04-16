namespace Server.Items
{
    [FlipableAttribute(0x1450, 0x1455)]
    public class BoneGloves : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 3; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 4; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 4; } }
        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 30; } }
        public override int StrReq { get { return 55; } }
        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Bone; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        [Constructable]
        public BoneGloves()
            : base(0x1450)
        {
            Weight = 2.0;
        }

        public BoneGloves(Serial serial)
            : base(serial)
        {
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
