// 
//		Originally by: Methril
//			Last Edited:
//
//		Version 1.0.1
//
//     GumpArt - By The Art
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class PowerScrollChoice : Item
	{
		private double m_DeedLevel = 120.0;
		
		[CommandProperty( AccessLevel.GameMaster )]
        public double DeedLevel
        {
            get { return m_DeedLevel; }
            set { m_DeedLevel = value; InvalidateProperties(); }
        }
		
		[Constructable]
		public PowerScrollChoice() : this ( 120.0 )
		{
		}
		
		[Constructable]
		public PowerScrollChoice(double SkillLvl) : base( 0x14F0 )
		{
			m_DeedLevel = SkillLvl;
			
			Name = "Power Scroll";
			Weight = 1.0;
			Hue = 1797;
			ItemID = 5360;
		}
		
		public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
			list.Add(String.Format("Scroll: +" + m_DeedLevel));
        }
		
		public PowerScrollChoice( Serial serial ) : base( serial ) { }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) ) from.SendLocalizedMessage( 1042001 );
			else from.SendGump( new PowerScrollPicker( from, this, m_DeedLevel ) );
		}
		
		private class PowerScrollPicker : Gump
		{
			private Mobile m_From;
			private PowerScrollChoice m_Deed;
			private double m_DeedLevel;

			public PowerScrollPicker(Mobile from , PowerScrollChoice deed, double DeedLevel) : base( 50, 50 )
			{
				m_From = from;
				m_Deed = deed;
				m_DeedLevel = DeedLevel;
				
				from.CloseGump( typeof( PowerScrollPicker ) );
				
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
			AddBackground(1, 12, 653, 580, 9390);
			AddImageTiled(79, 94, 488, 4, 9151);
			AddImageTiled(79, 526, 488, 4, 9151);
			AddHtml(224, 63, 200, 35, "<basefont size=10 color=#002905>Choose a Powerscroll", (bool)false, (bool)false);
//			AddHtml(94, 116, 110, 20, "<basefont size=5 color=#2e2a27>Alchemy", (bool)false, (bool)false);
//			AddButton(73, 116, 2118, 2117, 1, GumpButtonType.Reply, 0);
			AddHtml(94, 136, 110, 20, "<basefont size=5 color=#2e2a27>Anatomy", (bool)false, (bool)false);
			AddButton(73, 136, 2118, 2117, 2, GumpButtonType.Reply, 0);
			AddHtml(94, 156, 110, 20, "<basefont size=5 color=#2e2a27>AnimalLore", (bool)false, (bool)false);
			AddButton(73, 156, 2118, 2117, 3, GumpButtonType.Reply, 0);
			AddHtml(94, 176, 110, 20, "<basefont size=5 color=#2e2a27>AnimalTaming", (bool)false, (bool)false);
			AddButton(73, 176, 2118, 2117, 4, GumpButtonType.Reply, 0);
			AddHtml(94, 196, 110, 20, "<basefont size=5 color=#2e2a27>Archery", (bool)false, (bool)false);
			AddButton(73, 196, 2118, 2117, 5, GumpButtonType.Reply, 0);
//			AddHtml(94, 216, 110, 20, "<basefont size=5 color=#2e2a27>ArmsLore", (bool)false, (bool)false);
//			AddButton(73, 216, 2118, 2117, 6, GumpButtonType.Reply, 0);
//			AddHtml(94, 236, 110, 20, "<basefont size=5 color=#2e2a27>Begging", (bool)false, (bool)false);
//			AddButton(73, 236, 2118, 2117, 7, GumpButtonType.Reply, 0);
//			AddHtml(93, 256, 110, 20, "<basefont size=5 color=#2e2a27>Blacksmith", (bool)false, (bool)false);
//			AddButton(73, 256, 2118, 2117, 8, GumpButtonType.Reply, 0);
			AddHtml(94, 276, 110, 20, "<basefont size=5 color=#2e2a27>Bushido", (bool)false, (bool)false);
			AddButton(73, 276, 2118, 2117, 9, GumpButtonType.Reply, 0);
//			AddHtml(94, 296, 110, 20, "<basefont size=5 color=#2e2a27>Camping", (bool)false, (bool)false);
//			AddButton(73, 296, 2118, 2117, 10, GumpButtonType.Reply, 0);
//			AddHtml(94, 316, 110, 20, "<basefont size=5 color=#2e2a27>Carpentry", (bool)false, (bool)false);
//			AddButton(73, 316, 2118, 2117, 11, GumpButtonType.Reply, 0);
//			AddHtml(94, 336, 110, 20, "<basefont size=5 color=#2e2a27>Cartography", (bool)false, (bool)false);
//			AddButton(73, 336, 2118, 2117, 12, GumpButtonType.Reply, 0);
			AddHtml(94, 356, 110, 20, "<basefont size=5 color=#2e2a27>Chivalry", (bool)false, (bool)false);
			AddButton(73, 356, 2118, 2117, 13, GumpButtonType.Reply, 0);
//			AddHtml(94, 376, 110, 20, "<basefont size=5 color=#2e2a27>Cooking", (bool)false, (bool)false);
//			AddButton(73, 376, 2118, 2117, 14, GumpButtonType.Reply, 0);
//			AddHtml(94, 396, 110, 20, "<basefont size=5 color=#2e2a27>DetectHidden", (bool)false, (bool)false);
//			AddButton(73, 396, 2118, 2117, 15, GumpButtonType.Reply, 0);
			AddHtml(94, 416, 110, 20, "<basefont size=5 color=#2e2a27>Discordance", (bool)false, (bool)false);
			AddButton(73, 416, 2118, 2117, 16, GumpButtonType.Reply, 0);
			AddHtml(94, 436, 110, 20, "<basefont size=5 color=#2e2a27>EvalInt", (bool)false, (bool)false);
			AddButton(73, 436, 2118, 2117, 17, GumpButtonType.Reply, 0);
			AddHtml(94, 456, 110, 20, "<basefont size=5 color=#2e2a27>Fencing", (bool)false, (bool)false);
			AddButton(73, 456, 2118, 2117, 18, GumpButtonType.Reply, 0);
			AddHtml(94, 476, 110, 20, "<basefont size=5 color=#2e2a27>Fishing", (bool)false, (bool)false);
			AddButton(73, 476, 2118, 2117, 19, GumpButtonType.Reply, 0);
			AddHtml(92, 496, 110, 20, "<basefont size=5 color=#2e2a27>Fletching", (bool)false, (bool)false);
			AddButton(73, 496, 2118, 2117, 20, GumpButtonType.Reply, 0);
			AddHtml(283, 116, 110, 20, "<basefont size=5 color=#2e2a27>Focus", (bool)false, (bool)false);
			AddButton(261, 116, 2118, 2117, 21, GumpButtonType.Reply, 0);
//			AddHtml(283, 136, 110, 20, "<basefont size=5 color=#2e2a27>Forensics", (bool)false, (bool)false);
//			AddButton(261, 136, 2118, 2117, 22, GumpButtonType.Reply, 0);
			AddHtml(283, 156, 110, 20, "<basefont size=5 color=#2e2a27>Healing", (bool)false, (bool)false);
			AddButton(261, 156, 2118, 2117, 23, GumpButtonType.Reply, 0);
//			AddHtml(283, 176, 110, 20, "<basefont size=5 color=#2e2a27>Herding", (bool)false, (bool)false);
//			AddButton(261, 176, 2118, 2117, 24, GumpButtonType.Reply, 0);
//			AddHtml(283, 196, 110, 20, "<basefont size=5 color=#2e2a27>Hiding", (bool)false, (bool)false);
//			AddButton(261, 196, 2118, 2117, 25, GumpButtonType.Reply, 0);
//			AddHtml(283, 216, 110, 20, "<basefont size=5 color=#2e2a27>Imbuing", (bool)false, (bool)false);
//			AddButton(261, 216, 2118, 2117, 26, GumpButtonType.Reply, 0);
//			AddHtml(283, 236, 110, 20, "<basefont size=5 color=#2e2a27>Inscribe", (bool)false, (bool)false);
//			AddButton(261, 236, 2118, 2117, 27, GumpButtonType.Reply, 0);
//			AddHtml(282, 256, 110, 20, "<basefont size=5 color=#2e2a27>ItemID", (bool)false, (bool)false);
//			AddButton(261, 256, 2118, 2117, 28, GumpButtonType.Reply, 0);
//			AddHtml(282, 276, 110, 20, "<basefont size=5 color=#2e2a27>Lockpicking", (bool)false, (bool)false);
//			AddButton(261, 276, 2118, 2117, 29, GumpButtonType.Reply, 0);
//			AddHtml(282, 296, 110, 20, "<basefont size=5 color=#2e2a27>Lumperjacking", (bool)false, (bool)false);
//			AddButton(261, 296, 2118, 2117, 30, GumpButtonType.Reply, 0);
			AddHtml(283, 316, 110, 20, "<basefont size=5 color=#2e2a27>Macing", (bool)false, (bool)false);
			AddButton(261, 316, 2118, 2117, 31, GumpButtonType.Reply, 0);
			AddHtml(283, 336, 110, 20, "<basefont size=5 color=#2e2a27>Magery", (bool)false, (bool)false);
			AddButton(261, 336, 2118, 2117, 32, GumpButtonType.Reply, 0);
			AddHtml(283, 356, 110, 20, "<basefont size=5 color=#2e2a27>MagicResist", (bool)false, (bool)false);
			AddButton(261, 356, 2118, 2117, 33, GumpButtonType.Reply, 0);
			AddHtml(283, 376, 110, 20, "<basefont size=5 color=#2e2a27>Meditation", (bool)false, (bool)false);
			AddButton(261, 376, 2118, 2117, 34, GumpButtonType.Reply, 0);
//			AddHtml(283, 396, 110, 20, "<basefont size=5 color=#2e2a27>Mining", (bool)false, (bool)false);
//			AddButton(261, 396, 2118, 2117, 35, GumpButtonType.Reply, 0);
			AddHtml(283, 416, 110, 20, "<basefont size=5 color=#2e2a27>Musicianship", (bool)false, (bool)false);
			AddButton(261, 416, 2118, 2117, 36, GumpButtonType.Reply, 0);
			AddHtml(283, 436, 110, 20, "<basefont size=5 color=#2e2a27>Mysticism", (bool)false, (bool)false);
			AddButton(261, 436, 2118, 2117, 37, GumpButtonType.Reply, 0);
			AddHtml(283, 456, 110, 20, "<basefont size=5 color=#2e2a27>Necromancy", (bool)false, (bool)false);
			AddButton(261, 456, 2118, 2117, 38, GumpButtonType.Reply, 0);
			AddHtml(283, 476, 110, 20, "<basefont size=5 color=#2e2a27>Ninjitsu", (bool)false, (bool)false);
			AddButton(261, 476, 2118, 2117, 39, GumpButtonType.Reply, 0);
			AddHtml(283, 496, 110, 20, "<basefont size=5 color=#2e2a27>Parry", (bool)false, (bool)false);
			AddButton(261, 496, 2118, 2117, 40, GumpButtonType.Reply, 0);
			AddHtml(473, 116, 110, 20, "<basefont size=5 color=#2e2a27>Peacemaking", (bool)false, (bool)false);
			AddButton(452, 116, 2118, 2117, 41, GumpButtonType.Reply, 0);
//			AddHtml(474, 136, 110, 20, "<basefont size=5 color=#2e2a27>Poisoning", (bool)false, (bool)false);
//			AddButton(452, 136, 2118, 2117, 42, GumpButtonType.Reply, 0);
			AddHtml(474, 156, 110, 20, "<basefont size=5 color=#2e2a27>Provocation", (bool)false, (bool)false);
			AddButton(452, 156, 2118, 2117, 43, GumpButtonType.Reply, 0);
//			AddHtml(474, 176, 110, 20, "<basefont size=5 color=#2e2a27>RemoveTrap", (bool)false, (bool)false);
//			AddButton(452, 176, 2118, 2117, 44, GumpButtonType.Reply, 0);
//			AddHtml(474, 196, 110, 20, "<basefont size=5 color=#2e2a27>Snooping", (bool)false, (bool)false);
//			AddButton(452, 196, 2118, 2117, 45, GumpButtonType.Reply, 0);
			AddHtml(475, 216, 110, 20, "<basefont size=5 color=#2e2a27>SpellWeaving", (bool)false, (bool)false);
			AddButton(453, 216, 2118, 2117, 46, GumpButtonType.Reply, 0);
			AddHtml(474, 236, 110, 20, "<basefont size=5 color=#2e2a27>SpiritSpeak", (bool)false, (bool)false);
			AddButton(452, 236, 2118, 2117, 47, GumpButtonType.Reply, 0);
			AddHtml(474, 256, 110, 20, "<basefont size=5 color=#2e2a27>Stealing", (bool)false, (bool)false);
			AddButton(452, 256, 2118, 2117, 48, GumpButtonType.Reply, 0);
			AddHtml(473, 276, 110, 20, "<basefont size=5 color=#2e2a27>Stealth", (bool)false, (bool)false);
			AddButton(452, 276, 2118, 2117, 49, GumpButtonType.Reply, 0);
			AddHtml(474, 296, 110, 20, "<basefont size=5 color=#2e2a27>Swords", (bool)false, (bool)false);
			AddButton(452, 296, 2118, 2117, 50, GumpButtonType.Reply, 0);
			AddHtml(474, 316, 110, 20, "<basefont size=5 color=#2e2a27>Tactics", (bool)false, (bool)false);
			AddButton(452, 316, 2118, 2117, 51, GumpButtonType.Reply, 0);
//			AddHtml(474, 336, 110, 20, "<basefont size=5 color=#2e2a27>Tailoring", (bool)false, (bool)false);
//			AddButton(452, 336, 2118, 2117, 52, GumpButtonType.Reply, 0);
//			AddHtml(474, 356, 110, 20, "<basefont size=5 color=#2e2a27>TasteID", (bool)false, (bool)false);
//			AddButton(452, 356, 2118, 2117, 53, GumpButtonType.Reply, 0);
			AddHtml(474, 376, 110, 20, "<basefont size=5 color=#2e2a27>Throwing", (bool)false, (bool)false);
			AddButton(452, 376, 2118, 2117, 54, GumpButtonType.Reply, 0);
//			AddHtml(474, 396, 110, 20, "<basefont size=5 color=#2e2a27>Tinkering", (bool)false, (bool)false);
//			AddButton(452, 396, 2118, 2117, 55, GumpButtonType.Reply, 0);
//			AddHtml(474, 416, 110, 20, "<basefont size=5 color=#2e2a27>Tracking", (bool)false, (bool)false);
//			AddButton(452, 416, 2118, 2117, 56, GumpButtonType.Reply, 0);
			AddHtml(474, 436, 110, 20, "<basefont size=5 color=#2e2a27>Veterinary", (bool)false, (bool)false);
			AddButton(452, 436, 2118, 2117, 57, GumpButtonType.Reply, 0);
			AddHtml(474, 456, 110, 20, "<basefont size=5 color=#2e2a27>Wrestling", (bool)false, (bool)false);
			AddButton(452, 456, 2118, 2117, 58, GumpButtonType.Reply, 0);
			AddHtml(112, 536, 110, 20, "<basefont size=5 color=#2e2a27>Cancel", (bool)false, (bool)false);
			AddButton(74, 536, 1151, 1152, 59, GumpButtonType.Reply, 0);
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if ( m_Deed.Deleted ) return;

				PowerScroll scroll = null;
				
				switch(info.ButtonID)
				{
					case 0: return;
					case 1: scroll = new PowerScroll(SkillName.Alchemy, m_DeedLevel) ; break;
					case 2: scroll = new PowerScroll(SkillName.Anatomy, m_DeedLevel) ; break;
					case 3: scroll = new PowerScroll(SkillName.AnimalLore, m_DeedLevel) ; break;
					case 4: scroll = new PowerScroll(SkillName.AnimalTaming, m_DeedLevel) ; break;
					case 5: scroll = new PowerScroll(SkillName.Archery, m_DeedLevel) ; break;
					case 6: scroll = new PowerScroll(SkillName.ArmsLore, m_DeedLevel) ; break;
					case 7: scroll = new PowerScroll(SkillName.Begging, m_DeedLevel) ; break;
					case 8: scroll = new PowerScroll(SkillName.Blacksmith, m_DeedLevel) ; break;
					case 9: scroll = new PowerScroll(SkillName.Bushido, m_DeedLevel) ; break;
					case 10: scroll = new PowerScroll(SkillName.Camping, m_DeedLevel) ; break;
					case 11: scroll = new PowerScroll(SkillName.Carpentry, m_DeedLevel) ; break;
					case 12: scroll = new PowerScroll(SkillName.Cartography, m_DeedLevel) ; break;
					case 13: scroll = new PowerScroll(SkillName.Chivalry, m_DeedLevel) ; break;
					case 14: scroll = new PowerScroll(SkillName.Cooking, m_DeedLevel) ; break;
					case 15: scroll = new PowerScroll(SkillName.DetectHidden, m_DeedLevel) ; break;
					case 16: scroll = new PowerScroll(SkillName.Discordance, m_DeedLevel) ; break;
					case 17: scroll = new PowerScroll(SkillName.EvalInt, m_DeedLevel) ; break;
					case 18: scroll = new PowerScroll(SkillName.Fencing, m_DeedLevel) ; break;
					case 19: scroll = new PowerScroll(SkillName.Fishing, m_DeedLevel) ; break;
					case 20: scroll = new PowerScroll(SkillName.Fletching, m_DeedLevel) ; break;
					case 21: scroll = new PowerScroll(SkillName.Focus, m_DeedLevel) ; break;
					case 22: scroll = new PowerScroll(SkillName.Forensics, m_DeedLevel) ; break;
					case 23: scroll = new PowerScroll(SkillName.Healing, m_DeedLevel) ; break;
					case 24: scroll = new PowerScroll(SkillName.Herding, m_DeedLevel) ; break;
					case 25: scroll = new PowerScroll(SkillName.Hiding, m_DeedLevel) ; break;
					case 26: scroll = new PowerScroll(SkillName.Imbuing, m_DeedLevel) ; break;
					case 27: scroll = new PowerScroll(SkillName.Inscribe, m_DeedLevel) ; break;
					case 28: scroll = new PowerScroll(SkillName.ItemID, m_DeedLevel) ; break;
					case 29: scroll = new PowerScroll(SkillName.Lockpicking, m_DeedLevel) ; break;
					case 30: scroll = new PowerScroll(SkillName.Lumberjacking, m_DeedLevel) ; break;
					case 31: scroll = new PowerScroll(SkillName.Macing, m_DeedLevel) ; break;
					case 32: scroll = new PowerScroll(SkillName.Magery, m_DeedLevel) ; break;
					case 33: scroll = new PowerScroll(SkillName.MagicResist, m_DeedLevel) ; break;
					case 34: scroll = new PowerScroll(SkillName.Meditation, m_DeedLevel) ; break;
					case 35: scroll = new PowerScroll(SkillName.Mining, m_DeedLevel) ; break;
					case 36: scroll = new PowerScroll(SkillName.Musicianship, m_DeedLevel) ; break;
					case 37: scroll = new PowerScroll(SkillName.Mysticism, m_DeedLevel) ; break;
					case 38: scroll = new PowerScroll(SkillName.Necromancy, m_DeedLevel) ; break;
					case 39: scroll = new PowerScroll(SkillName.Ninjitsu, m_DeedLevel) ; break;
					case 40: scroll = new PowerScroll(SkillName.Parry, m_DeedLevel) ; break;
					case 41: scroll = new PowerScroll(SkillName.Peacemaking, m_DeedLevel) ; break;
					case 42: scroll = new PowerScroll(SkillName.Poisoning, m_DeedLevel) ; break;
					case 43: scroll = new PowerScroll(SkillName.Provocation, m_DeedLevel) ; break;
					case 44: scroll = new PowerScroll(SkillName.RemoveTrap, m_DeedLevel) ; break;
					case 45: scroll = new PowerScroll(SkillName.Snooping, m_DeedLevel) ; break;
					case 46: scroll = new PowerScroll(SkillName.Spellweaving, m_DeedLevel) ; break;
					case 47: scroll = new PowerScroll(SkillName.SpiritSpeak, m_DeedLevel) ; break;
					case 48: scroll = new PowerScroll(SkillName.Stealing, m_DeedLevel) ; break;
					case 49: scroll = new PowerScroll(SkillName.Stealth, m_DeedLevel) ; break;
					case 50: scroll = new PowerScroll(SkillName.Swords, m_DeedLevel) ; break;
					case 51: scroll = new PowerScroll(SkillName.Tactics, m_DeedLevel) ; break;
					case 52: scroll = new PowerScroll(SkillName.Tailoring, m_DeedLevel) ; break;
					case 53: scroll = new PowerScroll(SkillName.TasteID, m_DeedLevel) ; break;
					case 54: scroll = new PowerScroll(SkillName.Throwing, m_DeedLevel) ; break;
					case 55: scroll = new PowerScroll(SkillName.Tinkering, m_DeedLevel) ; break;
					case 56: scroll = new PowerScroll(SkillName.Tracking, m_DeedLevel) ; break;
					case 57: scroll = new PowerScroll(SkillName.Veterinary, m_DeedLevel) ; break;
					case 58: scroll = new PowerScroll(SkillName.Wrestling, m_DeedLevel) ; break;
					case 59: return;
				}
				
				if ( scroll != null )
				{
					m_From.Backpack.DropItem( scroll );
					m_From.SendMessage( "A" + m_DeedLevel + " Powerscroll has been added to your backpack!" );
					m_Deed.Delete();
				}
			}
		}
		
        public override void Serialize(GenericWriter writer)
        {
	    base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_DeedLevel);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_DeedLevel = reader.ReadDouble();
        }
    }
	
}
