using System;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Misc
{
    public static class InscriptionCustomCraftData
    {

        public static void Initialize()
        {
            CraftSystem cs = DefInscription.CraftSystem;
            int index;
            index = cs.AddCraft(typeof(EnchantedScrollBinderDeed), 1044294, "Enchanted Scroll Binder", 75.0, 125.0, typeof(WoodPulp), 1113136, 1, 1044253);
            cs.AddRes(index, typeof(RelicFragment), "Relic Fragments", 5);
            cs.SetItemHue(index, 1641);
        }
    }
}
