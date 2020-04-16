using System;

namespace Server.Items
{
    public class CandelabraStand : BaseLight
    {
        [Constructable]
        public CandelabraStand()
            : base(0xA29)
        {
            this.Duration = TimeSpan.Zero; // Never burnt out
            this.Burning = false;
            this.Light = LightType.Circle225;
            this.Weight = 20.0;
        }

        public CandelabraStand(Serial serial)
            : base(serial)
        {
        }

        public override int LitItemID
        {
            get
            {
                return 0xB26;
            }
        }
        public override int UnlitItemID
        {
            get
            {
                return 0xA29;
            }
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