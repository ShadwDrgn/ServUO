using System;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Misc
{
    public static class AlchemyCustomCraftData
    {
        public static void Initialize()
        {
            CraftSystem cs = DefAlchemy.CraftSystem;
            int index;
            index = cs.AddCraft(typeof(PetBondingPotion), 1116353, 1152921, 75.0, 125.0, typeof(RawRibs), 1044485, 100, 1044253);
            cs.AddRes(index, typeof(UntappedPotential), "Untapped Potential", 1);
            cs.AddCreateItem(index, CraftPetBondingPotion);
        }
        private static Item CraftPetBondingPotion(Mobile m, CraftItem craftItem, ITool tool)
        {
            var bait = new PetBondingPotion
            {
                Amount = 2
            };

            return bait;
        }
    }
}
