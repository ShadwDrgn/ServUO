using System;

namespace Server.Items
{
    public class NewAccountTicket : Item
    {
        [Constructable]
        public NewAccountTicket()
            : base(0x14EF)
        {
            this.Weight = 1.0;
            this.Hue = 0x0;
            this.Name = "New Account Ticket (Throw this away for 300,000 clean up points)";
        }

        public NewAccountTicket(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Throw this away for 300,000 community points towards the clean up britannia system!");
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
 
            writer.Write( (int) 0 ); // version
        }
 
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
 
            int version = reader.ReadInt();
        }
    }
}
