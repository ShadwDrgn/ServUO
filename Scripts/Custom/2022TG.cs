using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Misc
{
    public class TG2022Gift : GiftGiver
    {
        public static void Initialize()
        {
            GiftGiving.Register(new TG2022Gift());
        }

        public override DateTime Start => new DateTime(2022, 11, 20); // When do you want your gift hand out?
        public override DateTime Finish => new DateTime(2022, 12, 04); // When do you want your gift hand out to stop?
        public override TimeSpan MinimumAge => TimeSpan.FromDays(7); // How old does a character have to be to obtain?

        public override void GiveGift(Mobile mob)
        {
            GiftBox box = new GiftBox();

            box.DropItem(new ChargerOfTheFallen());
            box.DropItem(new TurkeyPlatter());
            box.DropItem(new HeritageToken());
            box.DropItem(new HeritageToken());
            box.DropItem(new HeritageToken());
            box.DropItem(new HeritageToken());
            box.DropItem(new HeritageToken());

            switch (GiveGift(mob, box))
            {
                case GiftResult.Backpack:
                    mob.SendMessage(0x482, "Happy Holidays from the team(all one of us)!  Gift items have been placed in your backpack.");
                    break;
                case GiftResult.BankBox:
                    mob.SendMessage(0x482, "Happy Holidays from the team!  Gift items have been placed in your bank box.");
                    break;
            }
        }
    }
}
