using System;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Items
{
    public class EnchantedScrollBinderDeed : Item
    {

        private static readonly SkillName[] m_Restricted = new SkillName[]
        {
            SkillName.Blacksmith,
            SkillName.Tailoring,
            SkillName.Imbuing
        };

        private double m_Has;
        private bool m_Locked;
        private int m_Claimed;

        [CommandProperty(AccessLevel.GameMaster)]
        public double Has { get { return m_Has; } set { m_Has = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Locked { get { return m_Locked; } set { m_Locked = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Claimed { get { return m_Claimed; } set { m_Claimed = value; InvalidateProperties(); } }

        public override int LabelNumber => 1113135;  // Scroll Binder

        [Constructable]
        public EnchantedScrollBinderDeed()
            : base(0x14F0)
        {
            Name = "Enchanted Scroll Binder";
            LootType = LootType.Cursed;
            Hue = 1636;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(string.Format("Points: {0}\n Claimed: {1}/3", ((int)Has).ToString(), ((int)Claimed).ToString()));
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new EnchantedScrollBinderGump(from, this));
            }
        }

        public void OnTarget(Mobile from, object targeted)
        {
            if (targeted is Item && !((Item)targeted).IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640); // The item must be in your backpack to use it.
                return;
            }

            if (!(targeted is PowerScroll))
            {
                from.SendMessage("You must target a powerscroll");
                return;
            }
            PowerScroll ps = (PowerScroll)targeted;
	    if (Array.IndexOf(m_Restricted, ps.Skill) > -1) {
	        from.SendMessage("The Scrolls magic is unable to absorb crafting or imbuing scrolls.");
                return;
            }

            double value = ps.Value;
            switch (value)
            {
                case 105:
                    Has++;
                    break;
                case 110:
                    Has += 8;
                    break;
                case 115:
                    Has += 96;
                    break;
                case 120:
                    Has += 960;
                    break;
            }
            ps.Delete();
            from.Target = new InternalTarget(this);
        }

        public void GiveItem(Mobile from, Item item)
        {
            Container pack = from.Backpack;

            if (pack == null || !pack.TryDropItem(from, item, false))
                item.MoveToWorld(from.Location, from.Map);
        }

        private class InternalTarget : Target
        {
            private readonly EnchantedScrollBinderDeed m_Binder;

            public InternalTarget(EnchantedScrollBinderDeed binder) : base(-1, false, TargetFlags.None)
            {
                m_Binder = binder;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Binder != null && !m_Binder.Deleted && m_Binder.IsChildOf(from.Backpack))
                    m_Binder.OnTarget(from, targeted);
            }
        }

        private class EnchantedScrollBinderGump : Gump
        {
            private Mobile m_From;
            private EnchantedScrollBinderDeed m_Deed;

            public EnchantedScrollBinderGump(Mobile from, EnchantedScrollBinderDeed deed) : base(25, 50)
            {
                m_From = from;
                m_Deed = deed;

                AddPage(0);
                AddBackground(0, 0, 520, 310, 5054);
                AddImageTiled(10, 10, 500, 290, 2624);
                AddImageTiled(10, 30, 500, 10, 5058);
                AddImageTiled(10, 270, 500, 10, 5058);
                AddAlphaRegion(10, 10, 520, 310);

                AddHtml(10, 12, 520, 20, "<basefont color=#FFFFFF><CENTER>POWERSCROLL SELECTION</CENTER></basefont>", false, false);

                AddButton(15, 60, 4005, 4007, 10005, GumpButtonType.Reply, 0);
                AddHtml(50, 60, 430, 20, "<basefont color=#FFFFFF>Absorb Scroll (All scrolls value = 1/3rd their cost)</basefont>" , false, false);

                AddButton(15, 90, 4005, 4007, 10006, GumpButtonType.Reply, 0);
                AddHtml(50, 90, 430, 20, "<basefont color=#FFFFFF>Claim 105 Scroll (Cost: 3)</basefont>", false, false);

                AddButton(15, 120, 4005, 4007, 10007, GumpButtonType.Reply, 0);
                AddHtml(50, 120, 430, 20, "<basefont color=#FFFFFF>Claim 110 Scrolls (Cost: 24)</basefont>", false, false);

                AddButton(15, 150, 4005, 4007, 10008, GumpButtonType.Reply, 0);
                AddHtml(50, 150, 430, 20, "<basefont color=#FFFFFF>Claim 115 Scroll (Cost: 288)</basefont>", false, false);

                AddButton(15, 180, 4005, 4007, 10010, GumpButtonType.Reply, 0);
                AddHtml(50, 180, 470, 20, "<basefont color=#FFFFFF>Claim 120 Scroll (Cost: 2880)</basefont>", false, false);
                
                AddHtml(50, 210, 470, 20, "<basefont color=#FFFFFF>Current Points: " + ((int)m_Deed.Has).ToString() + "</basefont>", false, false);

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
                    case 10005: // Absorb Scroll
                    {
                        if (m_Deed.Locked)
                        {
                            m_From.SendMessage("This scroll has been locked and cannot absorb more.");
                            return;
                        }
                        m_From.SendLocalizedMessage(1113138); // Target the powerscroll you wish to bind.
                        m_From.Target = new InternalTarget(m_Deed);
                        return;
                    }
                    case 10006: // Claim 105
                    {
                        if (m_Deed.Has < 3)
                        {
                            m_From.SendMessage( "This Scroll does not have enough stored value" );
                            break;
                        }
                        m_Deed.Has -= 3;
                        m_Deed.Locked = true;
                        m_From.Backpack.DropItem( new PowerScrollChoice(105.0) );
                        m_From.SendMessage( "A Powerscroll has been added to your backpack!" );
                        m_Deed.Claimed++;
                        break;
                    }
                    case 10007: // Claim 110
                    {
                        if (m_Deed.Has < 24)
                        {
                            m_From.SendMessage( "This Scroll does not have enough stored value" );
                            break;
                        }
                        m_Deed.Has -= 24;
                        m_Deed.Locked = true;
                        m_From.Backpack.DropItem( new PowerScrollChoice(110.0) );
                        m_From.SendMessage( "A Powerscroll has been added to your backpack!" );
                        m_Deed.Claimed++;
                        break;
                    }
                    case 10008: // Claim 115
                    {
                        if (m_Deed.Has < 288)
                        {
                            m_From.SendMessage( "This Scroll does not have enough stored value" );
                            break;
                        }
                        m_Deed.Has -= 288;
                        m_Deed.Locked = true;
                        m_From.Backpack.DropItem( new PowerScrollChoice(115.0) );
                        m_From.SendMessage( "A Powerscroll has been added to your backpack!" );
                        m_Deed.Claimed++;
                        break;
                    }
                    case 10010: // Claim 120
                    {
                        if (m_Deed.Has < 2880)
                        {
                            m_From.SendMessage( "This Scroll does not have enough stored value" );
                            break;
                        }
                        m_Deed.Has -= 2880;
                        m_Deed.Locked = true;
                        m_From.Backpack.DropItem( new PowerScrollChoice(120.0) );
                        m_From.SendMessage( "A Powerscroll has been added to your backpack!" );
                        m_Deed.Claimed++;
                        break;
                    }
                }
                if (m_Deed.Claimed >= 3) m_Deed.Delete();
                if (m_Deed.Locked && m_Deed.Has <=0) m_Deed.Delete();
            }
        }


        public EnchantedScrollBinderDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(2);
            writer.Write(m_Has);
            writer.Write(m_Locked);
            writer.Write(m_Claimed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();
            m_Has = reader.ReadDouble();
            m_Locked = reader.ReadBool();
            if (v < 2)
            {
                m_Claimed = 0;
            }
            else
            {
                m_Claimed = reader.ReadInt();
            }

        }
    }
}
