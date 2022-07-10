using System;
namespace Server.Items
{
    public class DungeonTreasure1 : BaseDungeonChest
    {
        public override int DefaultGumpID => 0x49;

        [Constructable]
        public DungeonTreasure1() : base(Utility.RandomList(0xE3C, 0xE3E, 0x9a9)) // Large, Medium and Small Crate
        {
            RequiredSkill = 51;
            LockLevel = RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 1 * Utility.Random(1, 25);

            DropItem(new Gold(200, 600));
            if (Utility.RandomBool()) {
                DropItem(new Bolt(LockLevel));
            } else {
                DropItem(new Arrow(LockLevel));
            }
            DropItem(Loot.RandomClothing());

            AddLoot(Loot.RandomWeapon());
            AddLoot(Loot.RandomArmorOrShield());
            AddLoot(Loot.RandomJewelry());

            for (int i = Utility.Random(1, (int)(LockLevel/5)) + 1; i > 0; i--)
                DropItem(Loot.RandomGem());
        }

        public DungeonTreasure1(Serial serial) : base(serial)
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

    public class DungeonTreasure2 : BaseDungeonChest
    {
        [Constructable]
        public DungeonTreasure2() : base(Utility.RandomList(0xe42, 0xe77)) // various container IDs
        {
            RequiredSkill = 72;
            LockLevel = RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 2 * Utility.Random(1, 25);

            DropItem(new Gold(400, 900));
            if (Utility.RandomBool()) {
                DropItem(new Bolt(LockLevel));
            } else {
                DropItem(new Arrow(LockLevel));
            }
            DropItem(Loot.RandomPotion());
            for (int i = Utility.Random(1, 2); i > 1; i--)
            {
                Item ReagentLoot = Loot.RandomReagent();
                ReagentLoot.Amount = Utility.Random(10, 20);
                DropItem(ReagentLoot);
            }
            if (Utility.RandomBool()) //50% chance
                for (int i = Utility.Random(4) + 1; i > 0; i--)
                    DropItem(Loot.RandomScroll(0, 39, SpellbookType.Regular));

            if (Utility.RandomBool()) //50% chance
                for (int i = Utility.Random(6) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());
        }

        public DungeonTreasure2(Serial serial) : base(serial)
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

    public class DungeonTreasure3 : BaseDungeonChest
    {
        public override int DefaultGumpID => 0x4A;

        [Constructable]
        public DungeonTreasure3() : base(0x9ab) // Wooden, Metal and Metal Golden Chest
        {
            RequiredSkill = 84;
            LockLevel = RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 3 * Utility.Random(1, 25);

            DropItem(new Gold(600, 2100));
            if (Utility.RandomBool()) {
                DropItem(new Bolt(LockLevel));
            } else {
                DropItem(new Arrow(LockLevel));
            }

            for (int i = Utility.Random(1, 3); i > 1; i--)
            {
                Item ReagentLoot = Loot.RandomReagent();
                ReagentLoot.Amount = Utility.Random(10, 30);
                DropItem(ReagentLoot);
            }

            for (int i = Utility.Random(1, 3); i > 1; i--)
                DropItem(Loot.RandomPotion());

            if (0.67 > Utility.RandomDouble()) //67% chance = 2/3
                for (int i = Utility.Random(4) + 1; i > 0; i--)
                    DropItem(Loot.RandomScroll(0, 47, SpellbookType.Regular));

            if (0.67 > Utility.RandomDouble()) //67% chance = 2/3
                for (int i = Utility.Random(9) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());

            // Magical ArmorOrWeapon
            for (int i = Utility.Random(3, 6); i > 1; i--)
            {
                Item item = Loot.RandomArmorOrShieldOrWeapon();

                AddLoot(item);
            }

            for (int i = Utility.Random(1, 2); i > 1; i--)
                AddLoot(Loot.RandomClothing());

            for (int i = Utility.Random(1, 2); i > 1; i--)
                AddLoot(Loot.RandomJewelry());
        }

        public DungeonTreasure3(Serial serial) : base(serial)
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

    public class DungeonTreasure4 : BaseDungeonChest
    {
        [Constructable]
        public DungeonTreasure4() : base(0xe40) // Wooden, Metal and Metal Golden Chest
        {
            RequiredSkill = 92;
            LockLevel = RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 4 * Utility.Random(1, 25);

            DropItem(new Gold(900, 3200));
            if (Utility.RandomBool()) {
                DropItem(new Bolt(LockLevel));
            } else {
                DropItem(new Arrow(LockLevel));
            }

            for (int i = Utility.Random(1, 4); i > 1; i--)
            {
                Item ReagentLoot = Loot.RandomReagent();
                ReagentLoot.Amount = Utility.Random(10, 40);
                DropItem(ReagentLoot);
            }

            for (int i = Utility.Random(1, 4); i > 1; i--)
                DropItem(Loot.RandomPotion());

            if (0.75 > Utility.RandomDouble()) //75% chance = 3/4
                for (int i = Utility.Random(4) + 1; i > 0; i--)
                    DropItem(Loot.RandomScroll(0, 47, SpellbookType.Regular));

            if (0.75 > Utility.RandomDouble()) //75% chance = 3/4
                for (int i = Utility.RandomMinMax(9, 16) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());

            // Magical ArmorOrWeapon
            for (int i = Utility.Random(4, 8); i > 1; i--)
            {
                Item item = Loot.RandomArmorOrShieldOrWeapon();

                AddLoot(item);
            }

            for (int i = Utility.Random(1, 2); i > 1; i--)
                AddLoot(Loot.RandomClothing());

            for (int i = Utility.Random(1, 2); i > 1; i--)
                AddLoot(Loot.RandomJewelry());
        }

        public DungeonTreasure4(Serial serial) : base(serial)
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
