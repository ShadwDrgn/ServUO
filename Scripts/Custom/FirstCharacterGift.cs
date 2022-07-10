using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;

namespace Server.Misc
{
    public class FirstCharacterGift
    {
        public static void Initialize()
        {
            // Register our event handler
            EventSink.MobileCreated += new MobileCreatedEventHandler(EventSink_MobileCreated);
        }
        private static void EventSink_MobileCreated(MobileCreatedEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)e.Mobile;
                Mobile m_Mobile = e.Mobile;
                Account a_Account = ((Account)m_Mobile.Account);
                Container pack = m_Mobile.Backpack;
                if (!Convert.ToBoolean(a_Account.GetTag("FirstChar"))) {
                        Console.WriteLine("Account: {0}", a_Account.Username);
                        if (pack != null)
                        {
                                Spellbook book = new NecromancerSpellbook();
                                book.Content = book.BookCount == 64 ? book.Content = ulong.MaxValue : (1ul << book.BookCount) - 1;
                                pack.DropItem(book);
                                book = new Spellbook();
                                book.Content = book.BookCount == 64 ? book.Content = ulong.MaxValue : (1ul << book.BookCount) - 1;
                                pack.DropItem(book);
                                book = new MysticBook();
                                book.Content = book.BookCount == 64 ? book.Content = ulong.MaxValue : (1ul << book.BookCount) - 1;
                                pack.DropItem(book);

                                pack.DropItem( new Runebook( 10 ) );
                                pack.DropItem( new MythicCharacterToken() );
                                pack.DropItem( new SoulstoneFragmentToken() );
                                pack.DropItem( new SoulstoneFragmentToken() );
                                a_Account.SetTag("FirstChar", "true");
                        }
                }
            }
        }
    }
}

