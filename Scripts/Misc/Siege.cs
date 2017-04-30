using System;
using Server.Spells;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using System.Collections.Generic;
using Server.Commands;
using System.Linq;

namespace Server
{
    public static class Siege
    {
        public static bool SiegeShard = Config.Get("Siege.IsSiege", false);
        public static int CharacterSlots = Config.Get("Siege.CharacterSlots", 1);

        public static int StatsPerDay = 15;

        public static void Configure()
        {
            ROTTable = new Dictionary<PlayerMobile, Dictionary<Skill, DateTime>>();
            StatsTable = new Dictionary<PlayerMobile, int>();

            EventSink.AfterWorldSave += OnAfterSave;
        }

        public static void Initialize()
        {
            if (SiegeShard)
            {
                CommandSystem.Register("ResetROT", AccessLevel.GameMaster, e =>
                    {
                        LastReset = DateTime.UtcNow;

                        e.Mobile.SendMessage("Rate over Time reset!");
                    });

                CommandSystem.Register("GetROTInfo", AccessLevel.GameMaster, e =>
                    {
                        Mobile m = e.Mobile;

                        foreach (KeyValuePair<PlayerMobile, Dictionary<Skill, DateTime>> kvp in ROTTable)
                        {
                            Console.WriteLine("Player: {0}", kvp.Key.Name);
                            int stats = 0;

                            if (StatsTable.ContainsKey(kvp.Key))
                            {
                                stats = StatsTable[kvp.Key];
                            }

                            Console.WriteLine("Stats gained today: {0} of {1}", stats, StatsPerDay.ToString());

                            Utility.PushColor(ConsoleColor.Magenta);
                            foreach (KeyValuePair<Skill, DateTime> kvp2 in kvp.Value)
                            {
                                int pergain = MinutesPerGain(kvp.Key, kvp2.Key);
                                DateTime last = kvp2.Value;
                                DateTime next = last + TimeSpan.FromMinutes(pergain);

                                string nextg = next < DateTime.UtcNow ? "now" : "in " + ((int)(next - DateTime.UtcNow).TotalMinutes).ToString() + " minutes";

                                Console.WriteLine("   {0}: last gained {1}, can gain {2} (every {3} minutes)", kvp2.Key.ToString(), last.ToShortTimeString(), nextg, pergain.ToString());
                            }
                            Utility.PopColor();
                        }

                        Console.WriteLine("---");
                        Console.WriteLine("Next Reset: {0} minutes", ((LastReset + TimeSpan.FromHours(24) - DateTime.Now)).TotalMinutes.ToString());
                    });

                Utility.PushColor(ConsoleColor.Red);
                Console.Write("Initializing Siege Perilous Shard...");

                long tick = Core.TickCount;

                List<XmlSpawner> toReset = new List<XmlSpawner>();

                foreach (var item in World.Items.Values.OfType<XmlSpawner>().Where(sp => sp.Map == Map.Trammel && sp.Running))
                {
                    toReset.Add(item);
                }

                foreach (var item in toReset)
                {
                    item.DoReset = true;
                }

                Console.WriteLine("Reset {1} trammel spawners in {0} milliseconds!", Core.TickCount - tick, toReset.Count);
                Utility.PopColor();

                ColUtility.Free(toReset);
            }
        }

        /// <summary>
        /// Called in SpellHelper.cs CheckTravel method
        /// </summary>
        /// <param name="m"></param>
        /// <param name="type"></param>
        /// <returns>False fails travel check. True must pass other travel checks in SpellHelper.cs</returns>
        public static bool CheckTravel(Mobile m, Point3D p, Map map, TravelCheckType type)
        {
            if (m.AccessLevel > AccessLevel.Player)
                return true;

            switch (type)
            {
                case TravelCheckType.RecallFrom:
                case TravelCheckType.RecallTo:
                    {
                        return false;
                    }
                case TravelCheckType.GateFrom:
                case TravelCheckType.GateTo:
                case TravelCheckType.Mark:
                    {
                        return CanTravelTo(m, p, map);
                    }
                case TravelCheckType.TeleportFrom:
                case TravelCheckType.TeleportTo:
                    {
                        return true;
                    }
            }

            return true;
        }

        public static bool CanTravelTo(Mobile m, Point3D p, Map map)
        {
            return !(Region.Find(p, map) is DungeonRegion) && !SpellHelper.IsAnyT2A(map, p) && !SpellHelper.IsIlshenar(map, p);
        }

        public static Dictionary<PlayerMobile, Dictionary<Skill, DateTime>> ROTTable { get; set; }
        public static Dictionary<PlayerMobile, int> StatsTable { get; set; }

        public static DateTime LastReset { get; set; }

        public static void OnAfterSave(AfterWorldSaveEventArgs e)
        {
            CheckTime();
        }

        public static void CheckTime()
        {
            if (LastReset + TimeSpan.FromHours(24) < DateTime.Now)
            {
                DateTime reset = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);

                if (DateTime.Now < reset)
                {
                    LastReset = reset - TimeSpan.FromHours(24);
                }
                else
                {
                    ROTTable.Clear();
                    StatsTable.Clear();

                    LastReset = reset;
                }
            }
        }

        public static bool CheckSkillGain(PlayerMobile pm, int minutesPerSkill, Skill sk)
        {
            if (minutesPerSkill == 0)
            {
                return true;
            }

            if (ROTTable.ContainsKey(pm))
            {
                if (ROTTable[pm].ContainsKey(sk))
                {
                    DateTime lastGain = ROTTable[pm][sk];

                    if (lastGain + TimeSpan.FromMinutes(minutesPerSkill) < DateTime.UtcNow)
                    {
                        ROTTable[pm][sk] = DateTime.UtcNow;
                        return true;
                    }

                    return false;
                }
                else
                {
                    ROTTable[pm][sk] = DateTime.UtcNow;
                    return true;
                }
            }
            else
            {
                ROTTable[pm] = new Dictionary<Skill, DateTime>();
            }

            ROTTable[pm][sk] = DateTime.UtcNow;
            return true;
        }

        public static int MinutesPerGain(Mobile m, Skill sk)
        {
            double value = sk.Base;

            if (value < 70.0)
            {
                return 0;
            }

            if (value <= 79.9)
            {
                return 5;
            }

            if (value <= 89.9)
            {
                return 8;
            }

            if (value <= 99.9)
            {
                return 12;
            }

            return 15;
        }

        public static bool CanGainStat(PlayerMobile m)
        {
            if (!StatsTable.ContainsKey(m))
            {
                return true;
            }

            return StatsTable[m] < StatsPerDay;
        }

        public static void IncreaseStat(PlayerMobile m)
        {
            if (!StatsTable.ContainsKey(m))
            {
                StatsTable[m] = 1;
            }
            else
            {
                StatsTable[m]++;
            }
        }

        public static bool VendorCanSell(Type t)
        {
            foreach (var type in _NoSellList)
            {
                if (t == type || t.IsSubclassOf(type))
                    return false;
            }

            return true;
        }

        private static Type[] _NoSellList =
        {
            typeof(BaseIngot),
            typeof(BaseWoodBoard),
            typeof(BaseLog),
            typeof(BaseLeather),
            typeof(BaseHides),
            typeof(Cloth),
            typeof(BoltOfCloth),
            typeof(UncutCloth),
            typeof(Wool),
            typeof(Cotton),
            typeof(Flax),
            typeof(SpoolOfThread),
            typeof(Feather),
            typeof(Shaft)
        };

        public static void TryBlessItem(PlayerMobile pm, object targeted)
        {
            Item item = targeted as Item;

            if (item != null)
            {
                if (pm.Items.Contains(item) || (pm.Backpack != null && pm.Backpack.Items.Contains(item)))
                {
                    if (pm.BlessedItem != null && pm.BlessedItem == item)
                    {
                        pm.BlessedItem.LootType = LootType.Regular;
                        pm.SendLocalizedMessage(1075292, pm.BlessedItem.Name != null ? pm.BlessedItem.Name : "#" + pm.BlessedItem.LabelNumber.ToString()); // ~1_NAME~ has been unblessed.

                        pm.BlessedItem = null;
                    }
                    else if (item.LootType == LootType.Regular && !(item is Container))
                    {
                        Item old = pm.BlessedItem;

                        pm.BlessedItem = item;
                        pm.BlessedItem.LootType = LootType.Blessed;

                        pm.SendLocalizedMessage(1075293, pm.BlessedItem.Name != null ? pm.BlessedItem.Name : "#" + pm.BlessedItem.LabelNumber.ToString()); // ~1_NAME~ has been blessed.

                        if (old != null)
                        {
                            old.LootType = LootType.Regular;
                            pm.SendLocalizedMessage(1075292, old.Name != null ? old.Name : "#" + old.LabelNumber.ToString()); // ~1_NAME~ has been unblessed.
                        }
                    }
                }
            }
        }

        public static void CheckUsesRemaining(Mobile from, Item item)
        {
            IUsesRemaining uses = item as IUsesRemaining;

            if (uses != null)
            {
                uses.ShowUsesRemaining = true;
                uses.UsesRemaining--;

                if (uses.UsesRemaining <= 0)
                {
                    item.Delete();
                    from.SendLocalizedMessage(1044038); // You have worn out your tool!
                }
            }
        }
    }
}