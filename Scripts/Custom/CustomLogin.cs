using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Misc
{
    public class CustomLogin
    {
        public static void Initialize()
        {
            // Register our event handler
            EventSink.Login += new LoginEventHandler( EventSink_Login );
        }

        private static void EventSink_Login( LoginEventArgs args )
        {
            Mobile m = args.Mobile;
            m.CloseGump( typeof( WelcomeGump ) );
            m.SendGump( new WelcomeGump() );
        }
        private class WelcomeGump : Gump
        {
            public WelcomeGump()
                : base( 0, 0 )
            {
                this.Closable=true;
                this.Disposable=true;
                this.Dragable=true;
                this.Resizable=false;
                this.AddPage(0);
                this.AddBackground(128, 205, 532, 384, 9250);
                this.AddImage(243, -24, 1418);
                this.AddBackground(144, 220, 501, 353, 9350);
                this.AddBackground(166, 295, 457, 264, 2620);
                this.AddAlphaRegion(172, 302, 444, 249);
                this.AddImage(76, 174, 10440);
                this.AddImage(627, 174, 10441);
                this.AddImageTiled(280, 242, 226, 31, 87);
                this.AddHtml( 172, 302, 444, 249,@"
Welcome and thanks for joining Ultima Online Draconia!

Some simple info for beginners:

Use right click to close this and other dialogs.

You have a New Account Ticket if you're a new player. This is a very valuable item. Mouse over it in your backpack to see what it doees.

Hold right click to move.

Double click opens doors, containers, and activates objects (including yellow quest NPCs)

Single clicking accesses item's and NPC's context menus when available. This is one way to buy and sell from vendors.

If you find a bank (There is one to the west of the New Haven starting location and one in the middle of Luna) you can say the word bank while near a minter or banker to access your bank box.

You will find a Moongate South of town (South is Diagonally down and to the left).

Step into the Moongate to use it for travel.

Malas/Luna is a popular bank and has many useful vendors.

Type vendor buy while standing near a vendor to see their inventory or vendor sell to sell to him.

Many of the people here in New Haven will offer you quests to learn skills in the game. Double click them to see their quest!

Please enjoy your stay and if you have any suggestions for more things to add to this help dialog, please tell Craig.", (bool)false, (bool)true);
                this.AddImage(268, 228, 83);
                this.AddImageTiled(283, 226, 222, 20, 84);
                this.AddLabel(318, 236, 1577, @"Welcome To Draconia!");
                this.AddImage(305, 259, 96);
                this.AddImage(484, 250, 97);
                this.AddImage(296, 250, 95);
                this.AddImage(505, 228, 85);
                this.AddImageTiled(266, 244, 14, 31, 86);
                this.AddImageTiled(506, 243, 14, 26, 88);
                this.AddImage(268, 267, 89);
                this.AddImage(505, 267, 91);
                this.AddImageTiled(284, 269, 222, 12, 90);

            }
        }
    }
}
