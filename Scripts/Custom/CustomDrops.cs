using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Items
{
    public class CustomDrops
    {
        public static void Initialize()
        {
            EventSink.CreatureDeath += OnCreatureDeath;
        }

        public static void OnCreatureDeath(CreatureDeathEventArgs e)
        {
            BaseCreature bc = e.Creature as BaseCreature;
            Container c = e.Corpse;
            Mobile killer = e.Killer;
            int luck = LootPack.GetEffectiveLuck(e.Killer, bc);
            
            if (bc != null && c != null && !c.Deleted && !bc.Controlled && !bc.Summoned)
            {
                CheckDrop(bc, c, luck);
            }
        }

        public static void CheckDrop(BaseCreature bc, Container c, int luck)
        {
            const int MINIMUM_FAME = 3000;
            const int HIGHEST_FAME = 21000;
            const double MAX_CHANCE = 0.015;
            const double BOSS_BONUS = 0.2;
            List<Type> bossTypes = new List<Type>() { typeof(BaseChampion), typeof(BasePeerless) };
            double bonusChance = 0.0;
            double luckScalar = 1.0;


            // Don't give potential on low fame creatures.
            if (bc.Fame <= MINIMUM_FAME) return;

            foreach (Type t in bossTypes) {
                if ((bc.GetType() == t || bc.GetType().IsSubclassOf(t)) && bc.Fame > 15000)
                    bonusChance = BOSS_BONUS;
            }
            luckScalar = Math.Max(1, Math.Pow(luck,0.536)/10); // Diminishing returns on luck.

            double toBeat = ((Math.Min(1, (double)bc.Fame / HIGHEST_FAME) * MAX_CHANCE) * luckScalar) + bonusChance;

            if (Utility.RandomDouble() <= toBeat)
            {
                Item item = new UntappedPotential();
                c.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "You sense a powerful essence from inside the corpse");
                c.DropItem(item);
            }
        }
    }

    public class UntappedPotential : Item
    {
        [Constructable]
        public UntappedPotential() : base()
        {
            Weight = 0.5;
            ItemID = 0x400B;
            Name = "Untapped Potential";
            Stackable = true;
        }

        public UntappedPotential( Serial serial ) : base( serial ) { }

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
