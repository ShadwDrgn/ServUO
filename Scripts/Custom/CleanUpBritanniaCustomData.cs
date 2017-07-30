using System;
using Server;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.Points;
using Server.Engines.CleanUpBritannia;

namespace Server.Misc
{
    public static class CleanUpBritanniaCustomData
    {

        public static void Initialize()
        {
            Dictionary<Type, double> Entries = CleanUpBritanniaData.Entries;
            Entries[typeof(NewAccountTicket)] = 300000.0;
            Console.WriteLine("Added NewAccountTicket to Cleanup system");
        }
    }

}
