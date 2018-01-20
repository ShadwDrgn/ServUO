using Server;
using System;

namespace Server.Items
{
    public class TunicBearingTheCrestOfBlackthorn : Tunic
    {
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 150; } }
        public override int InitMaxHits { get { return 150; } }

        [Constructable]
        public TunicBearingTheCrestOfBlackthorn()
            : base()
        {
            ReforgedSuffix = ReforgedSuffix.Blackthorn;
            Attributes.BonusInt = 5;
            Attributes.RegenMana = 2;
            Attributes.LowerRegCost = 10;
            StrRequirement = 10;
            Hue = 0xe8;
        }

        public TunicBearingTheCrestOfBlackthorn(Serial serial)
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