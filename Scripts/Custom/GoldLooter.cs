using Server;
using Server.Mobiles;
using Server.Items;
using System;
using Server.ContextMenus;
using System.Collections.Generic;

namespace GoldLooter
{
    public class GoldLooter : Item
    {
        [Constructable]
        public GoldLooter() : base()
        {
            Weight = 1.0;
            Hue = 1169;
            ItemID = 0x1870;
            Name = "Looter";
            LootType = LootType.Blessed;
        }

        public GoldLooter( Serial serial ) : base( serial ) { }

        public override void OnSingleClick( Mobile from )
        {
            this.LabelTo(from, "Account Balance: " + Banker.GetBalance(from));
        }
        
        public override void OnDoubleClick( Mobile from )
        {
            int looted = 0;

            if ( !IsChildOf( from.Backpack ) )
                from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
            else
            {
                PlayerMobile player = from as PlayerMobile;
                List<Gold> items = new List<Gold>();
                List<Corpse> corpses = new List<Corpse>();

                foreach ( Item item in player.GetItemsInRange(3) )
                {
                    if ( item is Corpse )
                    {
                        Corpse corpse = item as Corpse;
                        if ( isCorpseLootable(player, corpse) )
                        {
                            corpses.Add(corpse);
                        }
                    }
                    else if ( item != null && item.Movable && item.IsAccessibleTo(player) && !item.Deleted && item.Movable && item is Gold)
                    {
                        items.Add(item as Gold);
                    }
                }

                foreach ( Gold gold in items ) {
                    if (player.Account.DepositGold(gold.Amount)) {
                        looted += gold.Amount;
                        gold.Amount = 0;
                        gold.Consume();
                    }
                }

                foreach ( Corpse corpse in corpses )
                {
                    looted += lootContainer(player, corpse);
                }
                if (looted > 0)
                {
                    player.SendMessage(1173, looted + " gold has been deposited into your account.");
                }
            }
        }
        private int lootContainer( PlayerMobile player, Container container )
        {
            int looted = 0;
            List<Gold> items = new List<Gold>( container.Items.Count );
            foreach ( Item item in container.Items )
                if ( item != null && !item.Deleted && item.Movable && item is Gold)
                    items.Add( item as Gold );

            foreach ( Gold gold in items )
            {
                if (player.Account.DepositGold(gold.Amount)) {
                    looted += gold.Amount;
                    gold.Amount = 0;
                    gold.Consume();
                }
            }
            return looted;
        }
        private bool isCorpseLootable(PlayerMobile player, Corpse corpse)
        {
            if ( corpse.Owner == null || corpse.Deleted || corpse.Owner is PlayerMobile
                || (corpse.Owner is BaseCreature && ((BaseCreature)corpse.Owner).IsBonded)
                || !corpse.CheckLoot(player, null) || corpse.IsCriminalAction(player)
                )
                    return false;
            return true;
        }
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 1 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
                base.Deserialize( reader );

                int version = reader.ReadInt();
        }

    }
}
