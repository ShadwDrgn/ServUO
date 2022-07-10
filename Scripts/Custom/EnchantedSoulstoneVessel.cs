using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class EnchantedSoulstoneVessel : Container
    {

        [Constructable]
        public EnchantedSoulstoneVessel()
            : base(0xA73F)
        {
            Name = "Enchanted Soulstone Vessel";
        }
        
        public override void OnDoubleClick(Mobile from)
        {
            // Send the Gump
            from.SendGump(new EnchantedSoulstoneVesselGump(from, this));
        }

        private class EnchantedSoulstoneVesselGump : Gump
        {
            private Mobile m_From;

            public EnchantedSoulstoneVesselGump(Mobile from, EnchantedSoulstoneVessel vessel) : base(25, 50)
            {
                AddPage(0);
                AddBackground(0, 0, 520, 310, 5054);
                AddImageTiled(10, 10, 500, 290, 2624);
                AddImageTiled(10, 30, 500, 10, 5058);
                AddImageTiled(10, 270, 500, 10, 5058);
                AddAlphaRegion(10, 10, 520, 310);

                AddHtml(10, 12, 520, 20, "<basefont color=#FFFFFF><CENTER>SOULSTONES</CENTER></basefont>", false, false);

                AddButton(15, 60, 4005, 4007, 10005, GumpButtonType.Reply, 0);
                AddHtml(50, 60, 430, 20, "<basefont color=#FFFFFF>Empty</basefont>" , false, false);

                AddButton(15, 90, 4005, 4007, 10006, GumpButtonType.Reply, 0);
                AddHtml(50, 90, 430, 20, "<basefont color=#FFFFFF>Blacksmithing 120.0</basefont>", false, false);

                AddButton(15, 120, 4005, 4007, 10007, GumpButtonType.Reply, 0);
                AddHtml(50, 120, 430, 20, "<basefont color=#FFFFFF>Claim 110 Scrolls (Cost: 24)</basefont>", false, false);

                AddButton(15, 150, 4005, 4007, 10008, GumpButtonType.Reply, 0);
                AddHtml(50, 150, 430, 20, "<basefont color=#FFFFFF>Claim 115 Scroll (Cost: 288)</basefont>", false, false);

                AddButton(15, 180, 4005, 4007, 10010, GumpButtonType.Reply, 0);
                AddHtml(50, 180, 470, 20, "<basefont color=#FFFFFF>Claim 120 Scroll (Cost: 2880)</basefont>", false, false);

                AddHtml(50, 210, 470, 20, "<basefont color=#FFFFFF>STUFF</basefont>", false, false);

                AddButton(15, 280, 4017, 4019, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(50, 280, 50, 20, 1011012, 0x7FFF, false, false); //CANCEL
            }
            
            public override void OnResponse(NetState sender, RelayInfo info)
            {
                switch (info.ButtonID)
                {
                    case 0: // Close
                    case 1:
                    {
                        break;
                    }

                }
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    
    }

}
