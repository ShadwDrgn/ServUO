using System;
using Server.Spells;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using System.Collections.Generic;

namespace Server
{
    public static class Siege
    {
        public static bool SiegeShard = Config.Get("Siege.IsSiege", false);
        public static int StatsPerDay = 15;

        public static void Configure()
        {
            ROTTable = new Dictionary<PlayerMobile, Dictionary<Skill, DateTime>>();
            StatsTable = new Dictionary<PlayerMobile, int>();

            EventSink.AfterWorldSave += OnAfterSave;
        }

        /// <summary>
        /// Called in SpellHelper.cs CheckTravel method
        /// </summary>
        /// <param name="m"></param>
        /// <param name="type"></param>
        /// <returns>False fails travel check. True must pass other travel checks in SpellHelper.cs</returns>
        public static bool CheckTravel(Mobile m, Point3D p, Map map, TravelCheckType type)
        {
            switch (type)
            {
                case TravelCheckType.RecallFrom:
                case TravelCheckType.RecallTo:
                    {
                        m.SendLocalizedMessage(501802); // Thy spell doth not appear to work...
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
            return Region.Find(p, map) is DungeonRegion || SpellHelper.IsAnyT2A(map, p) || SpellHelper.IsIlshenar(map, p);
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
            if (LastReset + TimeSpan.FromHours(24) < DateTime.UtcNow)
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
            typeof(BaseBoard),
            typeof(BaseLog),
            typeof(BaseLeather),
            typeof(BaseHides),
            typeof(Cloth),
            typeof(BoltOfCloth),
            typeof(UncutCloth)
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
    }
}