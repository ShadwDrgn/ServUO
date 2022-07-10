using System;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Misc
{
    public static class TailoringCustomCraftData
    {

        public static void Initialize()
        {
            CraftSystem cs = DefTailoring.CraftSystem;
            int index;
            index = cs.AddCraft(typeof(EnchantedBagOfSending), 1015283, "Enchanted Bag of Sending", 75.0, 125.0, typeof(BagOfSending), 1054104, 1, 1044253);
            cs.AddRes(index, typeof(RelicFragment), "Relic Fragments", 1);
        }
    }
}
