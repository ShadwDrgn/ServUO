using System;
using Reward = Server.Engines.Quests.BaseReward;

namespace Server.Items
{
    public class BaseCraftsmanSatchel : Backpack
    {
        public BaseCraftsmanSatchel()
            : base()
        {
            Hue = Reward.SatchelHue();
			
            int count = 1;
            if (0.15 > Utility.RandomDouble())
                count = 2;
			
            if (0.33 > Utility.RandomDouble())
                DropItem(Loot.RandomTalisman());

            while (Items.Count < count)
            { 
                if (0.4 > Utility.RandomDouble())
                {
                    DropItem(RandomItem());		
                }
                else if (0.88 > Utility.RandomDouble())
                {
                    DropItem(Reward.Jewlery());
                }
            }
        }

        public BaseCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public virtual Item RandomItem()
        {
            return null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class AlchemistCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public AlchemistCraftsmanSatchel()
            : base()
        {
            if (0.5 > Utility.RandomDouble())
            {
                var recipe = Reward.AlchemyRecipe();

                if (recipe != null)
                {
                    DropItem(recipe);
                }
            }
        }

        public AlchemistCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.RangedWeapon();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class FletcherCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public FletcherCraftsmanSatchel()
            : base()
        {
            if (0.5 > Utility.RandomDouble())
            {
                var recipe = Reward.FletcherRecipe();

                if (recipe != null)
                {
                    DropItem(recipe);
                }
            }

            var runic = Reward.FletcherRunic();

            if (runic != null)
            {
                DropItem(runic);
            }
        }

        public FletcherCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.RangedWeapon();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TailorsCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public TailorsCraftsmanSatchel()
            : base()
        {
            if (0.5 > Utility.RandomDouble())
            {
                var recipe = Reward.TailorRecipe();

                if (recipe != null)
                {
                    DropItem(recipe);
                }
            }
        }

        public TailorsCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Armor();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmithsCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public SmithsCraftsmanSatchel()
            : base()
        {
            if (0.5 > Utility.RandomDouble())
            {
                var recipe = Reward.SmithRecipe();

                if (recipe != null)
                {
                    DropItem(recipe);
                }
            }
        }

        public SmithsCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Weapon();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TinkersCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public TinkersCraftsmanSatchel()
            : base()
        {
            if (0.5 > Utility.RandomDouble())
            {
                var recipe = Reward.TinkerRecipe();

                if (recipe != null)
                {
                    DropItem(recipe);
                }
            }
        }

        public TinkersCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Weapon();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CarpentersCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public CarpentersCraftsmanSatchel()
            : base()
        {
            if (0.5 > Utility.RandomDouble())
            {
                var recipe = Reward.CarpentryRecipe();

                if (recipe != null)
                {
                    DropItem(recipe);
                }
            }

            var runic = Reward.CarpenterRunic();

            if (runic != null)
            {
                DropItem(runic);
            }

            var furniture = Reward.RandomFurniture();

            if (furniture != null)
            {
                DropItem(furniture);
            }
        }

        public CarpentersCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Weapon();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
