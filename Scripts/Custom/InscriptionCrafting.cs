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
            index = cs.AddCraft(typeof(RefreshPotion), 1116348, 1044538, -25, 25.0, typeof(BlackPearl), 1044353, 1, 1044361);
            index = cs.AddCraft(typeof(EnchantedScrollBinderDeed), 1044294, "Enchanted Scroll Binder", 75.0, 125.0, typeof(WoodPulp), 1113136, 1, 1044253);
            cs.AddRes(index, typeof(UntappedPotential), "Untapped Potential", 1);
            cs.SetItemHue(index, 1641);
        }
    }
}
