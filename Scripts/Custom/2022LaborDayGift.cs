using Server.Items;
using System;

namespace Server.Misc
{
    public class LaborDay2022Gift : GiftGiver
    {
        public static void Initialize()
        {
            GiftGiving.Register(new LaborDay2022Gift());
        }

        public override DateTime Start => new DateTime(2022, 09, 05); // When do you want your gift hand out?
        public override DateTime Finish => new DateTime(2022, 09, 06); // When do you want your gift hand out to stop?
        public override TimeSpan MinimumAge => TimeSpan.FromDays(1); // How old does a character have to be to obtain?

        public override void GiveGift(Mobile mob)
        {
            GiftBox box = new GiftBox();

            box.DropItem(new LaborDay2022GiftToken());
            box.DropItem(new HeritageToken());

            switch (GiveGift(mob, box))
            {
                case GiftResult.Backpack:
                    mob.SendMessage(0x482, "Happy Labor Day from the team!  Gift items have been placed in your backpack.");
                    break;
                case GiftResult.BankBox:
                    mob.SendMessage(0x482, "Happy Holidays from the team!  Gift items have been placed in your bank box.");
                    break;
            }
        }
    }
}
