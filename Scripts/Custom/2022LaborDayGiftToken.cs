using Server.Gumps;

namespace Server.Items
{
    public class LaborDay2022GiftToken : Item, IRewardOption
    {

        [Constructable]
        public LaborDay2022GiftToken()
            : base(19398)
        {
            Hue = 2113;
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.CloseGump(typeof(RewardOptionGump));
                from.SendGump(new RewardOptionGump(this));
            }
            else
            {
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
            }
        }

        public void GetOptions(RewardOptionList list)
        {
            list.Add(1, 1151679); // Scroll of Transcendence Book
            list.Add(2, 1154321); // Scroll of Alactrity Book
        }


        public void OnOptionSelected(Mobile from, int choice)
        {
            Bag bag = new Bag
            {
                Hue = 2720
            };

            switch (choice)
            {
                default:
                    bag.Delete();
                    break;
                case 1:
                    bag.DropItem(new ScrollOfTranscendenceBook());
                    from.AddToBackpack(bag); Delete();
                    break;
                case 2:
                    bag.DropItem(new ScrollOfAlacrityBook());
                    from.AddToBackpack(bag);
                    Delete(); break;
            }
        }

        public LaborDay2022GiftToken(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
