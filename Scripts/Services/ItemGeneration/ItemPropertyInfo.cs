using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.SkillHandlers;
using Server.Mobiles;

namespace Server.Items
{
    public enum ItemType
    {
        Invalid,
        Melee,
        Ranged,
        Armor,
        Shield,
        Hat,
        Jewel
    }

    public class PropInfo
    {
        public ItemType ItemType { get; set; }          // Identifies the loot type
        public int Scale { get; set; }                  // scale, ie increments of 3 for regen on weapons. Most will not use this propery and will only use default of ItemPropertyInfo.Scale
        public int StandardMax { get; set; }            // Max Intensity for old loot system
        public int LootMax { get; set; }                // Max Intensity for new loot system
        public int[] PowerfulLootRange { get; set; }    // Range of over-cappeed for new loot system

        public PropInfo(int itemRef, int sMax, int lMax)
            : this((ItemType)itemRef, -1, sMax, lMax, null)
        {
        }

        public PropInfo(int itemRef, int scale, int sMax, int lMax)
            : this((ItemType)itemRef, scale, sMax, lMax, null)
        {
        }

        public PropInfo(int itemRef, int sMax, int lMax, int[] powerfulRange)
            : this((ItemType)itemRef, -1, sMax, lMax, powerfulRange)
        {
        }

        public PropInfo(ItemType type, int scale, int sMax, int lMax, int[] powerfulRange)
        {
            ItemType = type;
            Scale = scale;
            StandardMax = sMax;
            LootMax = lMax;
            PowerfulLootRange = powerfulRange;
        }
    }

    public class ItemPropertyInfo
    {
        public static readonly bool NewLootSystem = RandomItemGenerator.Enabled;

        public int ID { get; set; }

        public bool Imbuable { get; set; }

        public object Attribute { get; set; }
        public TextDefinition AttributeName { get; set; }
        public int Weight { get; set; }

        public Type PrimaryRes { get; set; }
        public Type GemRes { get; set; }
        public Type SpecialRes { get; set; }

        public TextDefinition PrimaryName { get; set; }
        public TextDefinition GemName { get; set; }
        public TextDefinition SpecialName { get; set; }

        public int Scale { get; set; }
        public int Start { get; set; }
        public int MaxIntensity { get; set; }
        public int Description { get; set; }

        public PropInfo[] PropCategories { get; set; } = new PropInfo[7];

        public ItemPropertyInfo(object attribute, TextDefinition attributeName, int weight, int scale, int start, int maxInt)
            : this(attribute, attributeName, weight, null, null, null, null, null, null, scale, start, maxInt, -1, null)
        {
        }

        public ItemPropertyInfo(object attribute, TextDefinition attributeName, int weight, int scale, int start, int maxInt, int description)
            : this(attribute, attributeName, weight, null, null, null, null, null, null, scale, start, maxInt, description, null)
        {
        }

        public ItemPropertyInfo(object attribute, TextDefinition attributeName, int weight, int scale, int start, int maxInt, int description, params PropInfo[] categories)
            : this(attribute, attributeName, weight, null, null, null, scale, start, maxInt, description, categories)
        {
        }

        public ItemPropertyInfo(object attribute, TextDefinition attributeName, int weight, Type pRes, Type gRes, Type spRes, int scale, int start, int maxInt, int desc, params PropInfo[] categories)
            : this(attribute, attributeName, weight, pRes, null, gRes, null, spRes, null, scale, start, maxInt, desc, categories)
        {
        }

        public ItemPropertyInfo(
            object attribute,
            TextDefinition attributeName,
            int weight,
            Type pRes,
            TextDefinition pResName,
            Type gRes,
            TextDefinition gResName,
            Type spRes,
            TextDefinition spResName,
            int scale,
            int start,
            int maxInt,
            int desc, 
            params PropInfo[] categories)
        {
            Attribute = attribute;
            AttributeName = attributeName;
            Weight = weight;
            PrimaryRes = pRes;
            GemRes = gRes;
            SpecialRes = spRes;
            Scale = scale;
            Start = start;
            MaxIntensity = maxInt;
            Description = desc;

            if (categories != null)
            {
                foreach (var cat in categories)
                {
                    if (PropCategories[(int)cat.ItemType] == null)
                    {
                        PropCategories[(int)cat.ItemType] = cat;
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("Property Category {0} already exists for {1}!", cat.ItemType.ToString(), attribute.ToString())); 
                    }
                }
            }

            if (pRes != null && spRes != null && gRes != null)
            {
                Imbuable = true;
            }

            if (pResName == null)
            {
                PrimaryName = GetName(pRes);
            }
            else
            {
                PrimaryName = pResName;
            }

            if (gResName == null)
            {
                GemName = GetName(gRes);
            }
            else
            {
                GemName = gResName;
            }

            if (spResName == null)
            {
                SpecialName = GetName(spRes);
            }
            else
            {
                SpecialName = spResName;
            }
        }

        public virtual TextDefinition GetName(Type type)
        {
            if (type == null)
                return 0;

            if (type == typeof(Tourmaline)) return 1023864;
            if (type == typeof(Ruby)) return 1023859;
            if (type == typeof(Diamond)) return 1023878;
            if (type == typeof(Sapphire)) return 1023857;
            if (type == typeof(Citrine)) return 1023861;
            if (type == typeof(Emerald)) return 1023856;
            if (type == typeof(StarSapphire)) return 1023855;
            if (type == typeof(Amethyst)) return 1023862;

            if (type == typeof(RelicFragment)) return 1031699;
            if (type == typeof(EnchantedEssence)) return 1031698;
            if (type == typeof(MagicalResidue)) return 1031697;

            if (type == typeof(DarkSapphire)) return 1032690;
            if (type == typeof(Turquoise)) return 1032691;
            if (type == typeof(PerfectEmerald)) return 1032692;
            if (type == typeof(EcruCitrine)) return 1032693;
            if (type == typeof(WhitePearl)) return 1032694;
            if (type == typeof(FireRuby)) return 1032695;
            if (type == typeof(BlueDiamond)) return 1032696;
            if (type == typeof(BrilliantAmber)) return 1032697;

            if (type == typeof(ParasiticPlant)) return 1032688;
            if (type == typeof(LuminescentFungi)) return 1032689;

            if (type == typeof(CrystallineBlackrock)) return 1077568;
            if (type == typeof(BouraPelt)) return 1113355;

            if (LocBuffer.ContainsKey(type))
                return LocBuffer[type];
 
            Item item = Loot.Construct(type);

            if (item != null)
            {
                var localization = item.LabelNumber;

                if (localization <= 0)
                {
                    var label = item.Name ?? String.Empty;

                    LocBuffer[type] = label;
                    item.Delete();

                    return label;
                }

                LocBuffer[type] = localization;
                item.Delete();

                return localization;
            }

            if(type != null)
                Console.WriteLine("Warning, missing text defintion for type {0}.", type.Name);

            return -1;
        }

        public Dictionary<Type, TextDefinition> LocBuffer { get; set; } = new Dictionary<Type, TextDefinition>();
        public static Dictionary<int, ItemPropertyInfo> Table { get; set; } = new Dictionary<int, ItemPropertyInfo>();
        public static Dictionary<ItemType, List<int>> LootTable { get; set; } = new Dictionary<ItemType, List<int>>();

        static ItemPropertyInfo()
        {
            // i = runic, r = reforg, l = loot
            // 1 = melee, 2 = ranged, 3 = armor, 4 = sheild, 5 = hat, 6 = jewels
			Register(1, new ItemPropertyInfo(AosAttribute.DefendChance, 1075620, 110, typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceSingularity), 1, 1, 15, 1111947, 
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 25, 25, new int[] { 30, 35 }), new PropInfo(3, 0, 5), new PropInfo(4, 15, 15, new int[] { 20 }), new PropInfo(5, 0, 5), new PropInfo(6, 15, 15, new int[] { 20 })));

			Register(2, new ItemPropertyInfo(AosAttribute.AttackChance, 1075616, 130, typeof(EnchantedEssence), typeof(Tourmaline), typeof(EssencePrecision), 1, 1, 15, 1111958,
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 25, 25, new int[] { 30, 35 }), new PropInfo(3, 0, 5), new PropInfo(4, 15, 15, new int[] { 20 }), new PropInfo(5, 0, 5), new PropInfo(6, 15, 15, new int[] { 20 })));

            Register(3, new ItemPropertyInfo(AosAttribute.RegenHits, 1075627, 100, typeof(EnchantedEssence), typeof(Tourmaline), typeof(SeedOfRenewal), 1, 1, 2, 1111994,
                new PropInfo(1, 3, 0, 9), new PropInfo(2, 3, 0, 9), new PropInfo(3, 2, 2, new int[] { 4 }), new PropInfo(4, 0, 2, new int[] { 4 }), new PropInfo(5, 2, 2, new int[] { 4 })));

            Register(4, new ItemPropertyInfo(AosAttribute.RegenStam, 1079411, 100, typeof(EnchantedEssence), typeof(Diamond), typeof(SeedOfRenewal), 1, 1, 3, 1112043,
                new PropInfo(1, 3, 0, 9), new PropInfo(2, 3, 0, 9), new PropInfo(3, 3, 3, new int[] { 4 }), new PropInfo(4, 0, 3, new int[] { 4 }), new PropInfo(5, 3, 3, new int[] { 4 })));

            Register(5, new ItemPropertyInfo(AosAttribute.RegenMana, 1079410, 100, typeof(EnchantedEssence), typeof(Sapphire), typeof(SeedOfRenewal), 1, 1, 2, 1112003,
                new PropInfo(1, 3, 0, 9), new PropInfo(2, 3, 0, 9), new PropInfo(3, 2, 2, new int[] { 4 }), new PropInfo(4, 0, 2, new int[] { 4 }), new PropInfo(5, 2, 2, new int[] { 4 }), new PropInfo(6, 0, 2, 4)));

            Register(6, new ItemPropertyInfo(AosAttribute.BonusStr, 1079767, 110, typeof(EnchantedEssence), typeof(Diamond), typeof(FireRuby), 1, 1, 8, 1112044,
                new PropInfo(1, 0, 5), new PropInfo(2, 0, 5), new PropInfo(3, 0, 5), new PropInfo(4, 0, 5), new PropInfo(5, 0, 5), new PropInfo(6, 8, 8, new int[] { 9, 10 })));

            Register(7, new ItemPropertyInfo(AosAttribute.BonusDex, 1079732, 110, typeof(EnchantedEssence), typeof(Ruby), typeof(BlueDiamond), 1, 1, 8, 1111948,
                 new PropInfo(1, 0, 5), new PropInfo(2, 0, 5), new PropInfo(3, 0, 5), new PropInfo(4, 0, 5), new PropInfo(5, 0, 5), new PropInfo(6, 8, 8, new int[] { 9, 10 })));

            Register(8, new ItemPropertyInfo(AosAttribute.BonusInt, 1079756, 110, typeof(EnchantedEssence), typeof(Tourmaline), typeof(Turquoise), 1, 1, 8, 1111995,
                new PropInfo(1, 0, 5), new PropInfo(2, 0, 5), new PropInfo(3, 0, 5), new PropInfo(4, 0, 5), new PropInfo(5, 0, 5), new PropInfo(6, 8, 8, new int[] { 9, 10 })));

            Register(9, new ItemPropertyInfo(AosAttribute.BonusHits, 1075630, 110, typeof(EnchantedEssence), typeof(Ruby), typeof(LuminescentFungi), 1, 1, 5, 1111993,
                new PropInfo(1, 0, 5, new int[] { 6, 7 }), new PropInfo(2, 0, 5, new int[] { 6, 7 }), new PropInfo(3, 5, 5, new int[] { 6, 7 }), new PropInfo(4, 0, 5, new int[] { 6, 7 }), new PropInfo(5, 5, 5, new int[] { 6, 7 })));

            Register(10, new ItemPropertyInfo(AosAttribute.BonusStam, 1075632, 110, typeof(EnchantedEssence), typeof(Diamond), typeof(LuminescentFungi), 1, 1, 8, 1112042,
                new PropInfo(1, 0, 5), new PropInfo(2, 0, 5), new PropInfo(3, 8, 8, new int[] { 9, 10 }), new PropInfo(4, 0, 5), new PropInfo(5, 8, 8, new int[] { 9, 10 }), new PropInfo(6, 0, 5)));

            Register(11, new ItemPropertyInfo(AosAttribute.BonusMana, 1075631, 110, typeof(EnchantedEssence), typeof(Sapphire), typeof(LuminescentFungi), 1, 1, 8, 1112002,
                new PropInfo(1, 0, 5), new PropInfo(2, 0, 5), new PropInfo(3, 8, 8, new int[] { 9, 10 }), new PropInfo(4, 0, 5), new PropInfo(5, 8, 8, new int[] { 9, 10 }), new PropInfo(6, 0, 5)));

            Register(12, new ItemPropertyInfo(AosAttribute.WeaponDamage, 1079399, 100, typeof(EnchantedEssence), typeof(Citrine), typeof(CrystalShards), 5, 1, 50, 1112005,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(4, 0, 35), new PropInfo(6, 25, 25, new int[] { 30, 35 })));

            Register(13, new ItemPropertyInfo(AosAttribute.WeaponSpeed, 1075629, 110, typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceControl), 5, 5, 30, 1112045,
                new PropInfo(1, 30, 30, new int[] { 35, 40 }), new PropInfo(4, 0, 10), new PropInfo(6, 0, 10)));

            Register(14, new ItemPropertyInfo(AosAttribute.SpellDamage, 1075628, 100, typeof(EnchantedEssence), typeof(Emerald), typeof(CrystalShards), 1, 1, 12, 1112041,
                new PropInfo(6, 12, 12, new int[] { 14, 16, 18 })));

            Register(15, new ItemPropertyInfo(AosAttribute.CastRecovery, 1075618, 120, typeof(RelicFragment), typeof(Amethyst), typeof(EssenceDiligence), 1, 1, 3, 1111952,
                new PropInfo(6, 3, 3, new int[] { 4 })));

            Register(16, new ItemPropertyInfo(AosAttribute.CastSpeed, 1075617, 140,typeof(RelicFragment), typeof(Ruby), typeof(EssenceAchievement), 0, 1, 1, 1111951,
                new PropInfo(6, 1, 1)));

            Register(17, new ItemPropertyInfo(AosAttribute.LowerManaCost, 1075621, 110, typeof(RelicFragment), typeof(Tourmaline), typeof(EssenceOrder), 1, 1, 8, 1111996,
                new PropInfo(1, 0, 5), new PropInfo(2, 0, 5), new PropInfo(3, 8, 8, new int[] { 10 }), new PropInfo(4, 0, 5), new PropInfo(5, 8, 8, new int[] { 10 }), new PropInfo(6, 8, 8, new int[] { 10 })));

            Register(18, new ItemPropertyInfo(AosAttribute.LowerRegCost, 1075625, 100, typeof(MagicalResidue), typeof(Amber), typeof(FaeryDust), 1, 1, 20, 1111997,
                new PropInfo(3, 20, 20, new int[] { 25 }), new PropInfo(5, 20, 20, new int[] { 25 }), new PropInfo(6, 20, 20, new int[] { 25 })));

            Register(19, new ItemPropertyInfo(AosAttribute.ReflectPhysical, 1075626, 100, typeof(MagicalResidue), typeof(Citrine), typeof(ReflectiveWolfEye), 1, 1, 15, 1112006,
                new PropInfo(1, 0, 15), new PropInfo(2, 0, 15), new PropInfo(3, 15, 15, new int[] { 20 }), new PropInfo(4, 15, 15, new int[] { 20 }), new PropInfo(5, 15, 15, new int[] { 20 })));

            Register(20, new ItemPropertyInfo(AosAttribute.EnhancePotions, 1075624, 100, typeof(EnchantedEssence), typeof(Citrine), typeof(CrushedGlass), 5, 5, 25, 1111950,
                new PropInfo(1, 0, 15), new PropInfo(2, 0, 15), new PropInfo(3, 0, 5), new PropInfo(5, 0, 5), new PropInfo(6, 25, 25, new int[] { 30, 35 })));

            Register(21, new ItemPropertyInfo(AosAttribute.Luck, 1061153, 100, typeof(MagicalResidue), typeof(Citrine), typeof(ChagaMushroom), 10, 1, 100, 1111999, 
                new PropInfo(1, 100, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(2, 120, 120, new int[] { 130, 140, 150, 160, 170 }), new PropInfo(3, 100, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(4, 100, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(5, 100, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(6, 100, 100, new int[] { 110, 120, 130, 140, 150 })));

            Register(22, new ItemPropertyInfo(AosAttribute.SpellChanneling, 1079766, 100, typeof(MagicalResidue), typeof(Diamond), typeof(SilverSnakeSkin), 0, 1, 1, 1112040,
                new PropInfo(1, 1, 1), new PropInfo(2, 1, 1), new PropInfo(4, 1, 1)));

            Register(23, new ItemPropertyInfo(AosAttribute.NightSight, 1015168, 50, typeof(MagicalResidue), typeof(Tourmaline), typeof(BottleIchor), 0, 1, 1, 1112004,
                new PropInfo(3, 1, 1), new PropInfo(5, 1, 1), new PropInfo(6, 1, 1)));

            Register(24, new ItemPropertyInfo(AosWeaponAttribute.LowerStatReq, 1079757, 100, typeof(EnchantedEssence), typeof(Amethyst), typeof(ElvenFletching), 10, 10, 100, 1111998,
                new PropInfo(1, 0, 100), new PropInfo(2, 1, 1), new PropInfo(3, 0, 100), new PropInfo(4, 100, 100), new PropInfo(5, 0, 100)));

            Register(25, new ItemPropertyInfo(AosWeaponAttribute.HitLeechHits, 1079698, 110, typeof(MagicalResidue), typeof(Ruby), typeof(VoidOrb), 1, 2, 50, 1111964,
                new PropInfo(1, 10, 50, 50), new PropInfo(2, 10, 50, 50)));

            Register(26, new ItemPropertyInfo(AosWeaponAttribute.HitLeechStam, 1079707, 100, typeof(MagicalResidue), typeof(Diamond), typeof(VoidOrb), 1, 2, 50, 1111992,
                new PropInfo(1, 10, 50, 50), new PropInfo(2, 10, 50, 50)));

            Register(27, new ItemPropertyInfo(AosWeaponAttribute.HitLeechMana, 1079701, 110, typeof(MagicalResidue), typeof(Sapphire), typeof(VoidOrb), 1, 2, 50, 1111967,
                new PropInfo(1, 10, 50, 50), new PropInfo(2, 10, 50, 50)));

            Register(28, new ItemPropertyInfo(AosWeaponAttribute.HitLowerAttack,  1079699, 110, 	typeof(EnchantedEssence), typeof(Emerald),typeof(ParasiticPlant), 1, 2, 50, 1111965,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(29, new ItemPropertyInfo(AosWeaponAttribute.HitLowerDefend, 1079700, 130, typeof(EnchantedEssence), typeof(Tourmaline), typeof(ParasiticPlant), 1, 2, 50, 1111966,
                 new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(30, new ItemPropertyInfo(AosWeaponAttribute.HitPhysicalArea, 1079696, 100, typeof(MagicalResidue), typeof(Diamond), typeof(RaptorTeeth), 1, 2, 50, 1111956,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(31, new ItemPropertyInfo(AosWeaponAttribute.HitFireArea, 1079695, 100, typeof(MagicalResidue), typeof(Ruby), typeof(RaptorTeeth), 1, 2, 50, 1111955,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(32, new ItemPropertyInfo(AosWeaponAttribute.HitColdArea, 1079693, 100, typeof(MagicalResidue), typeof(Sapphire), typeof(RaptorTeeth), 1, 2, 50, 1111953,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(33, new ItemPropertyInfo(AosWeaponAttribute.HitPoisonArea, 1079697, 100, typeof(MagicalResidue), typeof(Emerald), typeof(RaptorTeeth), 1, 2, 50, 1111957,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(34, new ItemPropertyInfo(AosWeaponAttribute.HitEnergyArea, 1079694, 100, typeof(MagicalResidue), typeof(Amethyst), typeof(RaptorTeeth), 1, 2, 50, 1111954,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(35, new ItemPropertyInfo(AosWeaponAttribute.HitMagicArrow, 1079706, 120, typeof(RelicFragment), typeof(Amber), typeof(EssenceFeeling), 1, 2, 50, 1111963,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(36, new ItemPropertyInfo(AosWeaponAttribute.HitHarm, 1079704, 110, typeof(EnchantedEssence), typeof(Emerald), typeof(ParasiticPlant), 1, 2, 50, 1111961,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(37, new ItemPropertyInfo(AosWeaponAttribute.HitFireball, 1079703, 140, typeof(EnchantedEssence), typeof(Ruby), typeof(FireRuby), 1, 2, 50, 1111960,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(38, new ItemPropertyInfo(AosWeaponAttribute.HitLightning, 1079705, 140, typeof(RelicFragment), typeof(Amethyst), typeof(EssencePassion), 1, 2, 50, 1111962,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(39, new ItemPropertyInfo(AosWeaponAttribute.HitDispel, 1079702, 100, typeof(MagicalResidue), typeof(Amber), typeof(SlithTongue), 1, 2, 50, 1111959,
                new PropInfo(1, 50, 50, new int[] { 55, 60, 65, 70 }), new PropInfo(2, 50, 50, new int[] { 55, 60, 65, 70 })));

            Register(40, new ItemPropertyInfo(AosWeaponAttribute.UseBestSkill, 1079592, 150, typeof(EnchantedEssence), typeof(Amber), typeof(DelicateScales), 0, 1, 1, 1111946,
                new PropInfo(1, 1, 1)));

            Register(41, new ItemPropertyInfo(AosWeaponAttribute.MageWeapon, 1079759, 100, typeof(EnchantedEssence), typeof(Emerald), typeof(ArcanicRuneStone), 1, 1, 10, 1112001,
                new PropInfo(1, 10, 10, new int[] { 11, 12, 13, 14, 15 }), new PropInfo(2, 10, 10, new int[] { 11, 12, 13, 14, 15 })));

            Register(42, new ItemPropertyInfo(AosWeaponAttribute.DurabilityBonus, 1017323, 100, typeof(EnchantedEssence), typeof(Diamond), typeof(PowderedIron), 10, 10, 100, 1111949,
                new PropInfo(1, 0, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(2, 0, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(3, 0, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(4, 100, 100, new int[] { 110, 120, 130, 140, 150 }), new PropInfo(5, 0, 100, new int[] { 110, 120, 130, 140, 150 })));

            Register(49, new ItemPropertyInfo(AosArmorAttribute.MageArmor, 1079758, 0, typeof(EnchantedEssence), typeof(Diamond), typeof(AbyssalCloth), 0, 1, 1, 1112000,
                new PropInfo(3, 1, 1)));

            Register(51, new ItemPropertyInfo(AosElementAttribute.Physical, 1061158, 100, typeof(MagicalResidue), typeof(Diamond), typeof(BouraPelt), 1, 1, 15, 1112010,
                new PropInfo(3, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(4, 15, 15), new PropInfo(5, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(6, 15, 15, new int[] { 20 })));

            Register(52, new ItemPropertyInfo(AosElementAttribute.Fire, 1061159, 100, typeof(MagicalResidue), typeof(Ruby), typeof(BouraPelt), 1, 1, 15, 1112009,
                new PropInfo(3, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(4, 15, 15), new PropInfo(5, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(6, 15, 15, new int[] { 20 })));

            Register(53, new ItemPropertyInfo(AosElementAttribute.Cold, 1061160, 100, typeof(MagicalResidue), typeof(Sapphire), typeof(BouraPelt), 1, 1, 15, 1112007,
                new PropInfo(3, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(4, 15, 15), new PropInfo(5, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(6, 15, 15, new int[] { 20 })));

            Register(54, new ItemPropertyInfo(AosElementAttribute.Poison, 1061161, 100, typeof(MagicalResidue), typeof(Emerald), typeof(BouraPelt), 1, 1, 15, 1112011,
                new PropInfo(3, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(4, 15, 15), new PropInfo(5, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(6, 15, 15, new int[] { 20 })));

            Register(55, new ItemPropertyInfo(AosElementAttribute.Energy, 1061162, 100, typeof(MagicalResidue), typeof(Amethyst), typeof(BouraPelt), 1, 1, 15, 1112008,
                new PropInfo(3, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(4, 15, 15), new PropInfo(5, 15, 15, new int[] { 20, 25, 30 }), new PropInfo(6, 15, 15, new int[] { 20 })));

            Register(60, new ItemPropertyInfo("WeaponVelocity", 1080416, 130, typeof(RelicFragment), typeof(Tourmaline),	typeof(EssenceDirection), 1, 2, 15, 1112048,
                new PropInfo(1, 50, 50), new PropInfo(2, 50, 50)));

            Register(61, new ItemPropertyInfo(AosAttribute.BalancedWeapon, 1072792, 150, typeof(RelicFragment), typeof(Amber), typeof(EssenceBalance), 0, 1, 1, 1112047,
                new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));

            // Non-Imbuable, Non-Loot
            Register(62, new ItemPropertyInfo("SearingWeapon", 1151183, 150, 0, 1, 1));

            // Slayers
            Register(101, new ItemPropertyInfo(SlayerName.OrcSlaying,         1079741, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111977, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(102, new ItemPropertyInfo(SlayerName.TrollSlaughter,     1079754, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111990, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(103, new ItemPropertyInfo(SlayerName.OgreTrashing,       1079739, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111975, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(104, new ItemPropertyInfo(SlayerName.DragonSlaying,      1061284, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111970, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(105, new ItemPropertyInfo(SlayerName.Terathan,           1079753, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111989, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(106, new ItemPropertyInfo(SlayerName.SnakesBane,         1079744, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111980, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(107, new ItemPropertyInfo(SlayerName.LizardmanSlaughter, 1079738, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111974, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(108, new ItemPropertyInfo(SlayerName.GargoylesFoe,       1079737, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111973, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(111, new ItemPropertyInfo(SlayerName.Ophidian,           1079740, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111976, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(112, new ItemPropertyInfo(SlayerName.SpidersDeath,       1079746, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111982, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(113, new ItemPropertyInfo(SlayerName.ScorpionsBane,      1079743, 100,	    typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111979, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(114, new ItemPropertyInfo(SlayerName.FlameDousing,       1079736, 100,	    typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111972, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(115, new ItemPropertyInfo(SlayerName.WaterDissipation,   1079755, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111991, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(116, new ItemPropertyInfo(SlayerName.Vacuum,             1079733, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111968, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(117, new ItemPropertyInfo(SlayerName.ElementalHealth,    1079742, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111978, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(118, new ItemPropertyInfo(SlayerName.EarthShatter,       1079735, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111971, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(119, new ItemPropertyInfo(SlayerName.BloodDrinking,      1079734, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111969, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(120, new ItemPropertyInfo(SlayerName.SummerWind,         1079745, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         0, 1, 1, 1111981, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
		
            //Super Slayers
			Register(121, new ItemPropertyInfo(SlayerName.Silver,             1079752, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(UndyingFlesh), 	    0, 1, 1, 1111988, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(122, new ItemPropertyInfo(SlayerName.Repond,             1079750, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(GoblinBlood), 	    0, 1, 1, 1111986, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
            Register(123, new ItemPropertyInfo(SlayerName.ReptilianDeath,     1079751, 130,     typeof(RelicFragment),      typeof(Ruby),           typeof(LavaSerpentCrust),   0, 1, 1, 1111987, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(124, new ItemPropertyInfo(SlayerName.Exorcism,           1079748, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(DaemonClaw), 	    0, 1, 1, 1111984, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(125, new ItemPropertyInfo(SlayerName.ArachnidDoom,       1079747, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(SpiderCarapace),     0, 1, 1, 1111983, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
			Register(126, new ItemPropertyInfo(SlayerName.ElementalBan,       1079749, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(VialOfVitriol), 	    0, 1, 1, 1111985, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));
            Register(127, new ItemPropertyInfo(SlayerName.Fey,                1154652, 130,     typeof(RelicFragment),      typeof(Ruby),           typeof(FeyWings),           0, 1, 1, 1154652, new PropInfo(1, 1, 1), new PropInfo(2, 1, 1)));

            // Non-Imbuable, non-Loot
            Register(128, new ItemPropertyInfo(SlayerName.Dinosaur,           1156240, 130, 0, 1, 1));
            Register(129, new ItemPropertyInfo(SlayerName.Myrmidex,           1156241, 130, 0, 1, 1));
            Register(130, new ItemPropertyInfo(SlayerName.Eodon,              1156126, 130, 0, 1, 1));
            Register(131, new ItemPropertyInfo(SlayerName.EodonTribe,         1156347, 130, 0, 1, 1));

            // Talisman Slayers, non-Imbuable, non-Loot
            Register(135, new ItemPropertyInfo(TalismanSlayerName.Bear,       1072504, 130, 0, 1, 1));
            Register(136, new ItemPropertyInfo(TalismanSlayerName.Vermin,     1072505, 130, 0, 1, 1));
            Register(137, new ItemPropertyInfo(TalismanSlayerName.Bat,        1072506, 130, 0, 1, 1));
            Register(138, new ItemPropertyInfo(TalismanSlayerName.Mage,       1072507, 130, 0, 1, 1));
            Register(139, new ItemPropertyInfo(TalismanSlayerName.Beetle,     1072508, 130, 0, 1, 1));
            Register(140, new ItemPropertyInfo(TalismanSlayerName.Bird,       1072509, 130, 0, 1, 1));
            Register(141, new ItemPropertyInfo(TalismanSlayerName.Ice,        1072510, 130, 0, 1, 1));
            Register(142, new ItemPropertyInfo(TalismanSlayerName.Flame,      1072511, 130, 0, 1, 1));
            Register(143, new ItemPropertyInfo(TalismanSlayerName.Bovine,     1072512, 130, 0, 1, 1));
            Register(144, new ItemPropertyInfo(TalismanSlayerName.Wolf,       1075462, 130, 0, 1, 1));
            Register(145, new ItemPropertyInfo(TalismanSlayerName.Undead,     1079752, 130, 0, 1, 1));
            Register(146, new ItemPropertyInfo(TalismanSlayerName.Goblin,     1095010, 130, 0, 1, 1));

            Register(151, new ItemPropertyInfo(SkillName.Fencing,            1044102, 140, typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock), 5, 1, 15, 1112012, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(152, new ItemPropertyInfo(SkillName.Macing,             1044101, 140, typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock), 5, 1, 15, 1112013, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(153, new ItemPropertyInfo(SkillName.Swords,             1044100, 140, typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock), 5, 1, 15, 1112016, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(154, new ItemPropertyInfo(SkillName.Musicianship,       1044089, 140, typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock), 5, 1, 15, 1112015, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(155, new ItemPropertyInfo(SkillName.Magery,             1044085, 140, typeof(EnchantedEssence), typeof(StarSapphire), typeof(CrystallineBlackrock), 5, 1, 15, 1112014, new PropInfo(6, 15, 15, new int[] { 20 })));
			
			Register(156, new ItemPropertyInfo(SkillName.Wrestling,		    1044103, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112021, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(157, new ItemPropertyInfo(SkillName.AnimalTaming, 	    1044095, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112017, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(158, new ItemPropertyInfo(SkillName.SpiritSpeak,		1044092, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112019, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(159, new ItemPropertyInfo(SkillName.Tactics,			1044087, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112020, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(160, new ItemPropertyInfo(SkillName.Provocation,		1044082, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112018, new PropInfo(6, 15, 15, new int[] { 20 })));
			
			Register(161, new ItemPropertyInfo(SkillName.Focus,			    1044110, 140, 	typeof(EnchantedEssence),	typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112024, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(162, new ItemPropertyInfo(SkillName.Parry, 		    1044065, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112026, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(163, new ItemPropertyInfo(SkillName.Stealth,		    1044107, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112027, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(164, new ItemPropertyInfo(SkillName.Meditation,		1044106, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112025, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(165, new ItemPropertyInfo(SkillName.AnimalLore,	    1044062, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112022, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(166, new ItemPropertyInfo(SkillName.Discordance,	    1044075, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112023, new PropInfo(6, 15, 15, new int[] { 20 })));
			
            Register(167, new ItemPropertyInfo(SkillName.Mysticism,			1044115, 140, 	typeof(EnchantedEssence),	typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1115213, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(168, new ItemPropertyInfo(SkillName.Bushido,			1044112, 140, 	typeof(EnchantedEssence),	typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112029, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(169, new ItemPropertyInfo(SkillName.Necromancy, 		1044109, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112031, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(170, new ItemPropertyInfo(SkillName.Veterinary,	    1044099, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112033, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(171, new ItemPropertyInfo(SkillName.Stealing,		    1044093, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112032, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(172, new ItemPropertyInfo(SkillName.EvalInt, 		    1044076, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112030, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(173, new ItemPropertyInfo(SkillName.Anatomy,			1044061, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112028, new PropInfo(6, 15, 15, new int[] { 20 })));
			
			Register(174, new ItemPropertyInfo(SkillName.Peacemaking,		1044069, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112038, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(175, new ItemPropertyInfo(SkillName.Ninjitsu, 		    1044113, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112037, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(176, new ItemPropertyInfo(SkillName.Chivalry,		    1044111, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112035, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(177, new ItemPropertyInfo(SkillName.Archery,			1044091, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112034, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(178, new ItemPropertyInfo(SkillName.MagicResist,	    1044086, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112039, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(179, new ItemPropertyInfo(SkillName.Healing,			1044077, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1112036, new PropInfo(6, 15, 15, new int[] { 20 })));
            Register(180, new ItemPropertyInfo(SkillName.Throwing,			1044117, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1115212, new PropInfo(6, 15, 15, new int[] { 20 })));
            
            Register(181, new ItemPropertyInfo(SkillName.Lumberjacking,	    1002100, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1002101, new PropInfo(6, 15, 15, new int[] { 20 })));
			Register(182, new ItemPropertyInfo(SkillName.Snooping,			1002138, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1002139, new PropInfo(6, 15, 15, new int[] { 20 })));
            Register(183, new ItemPropertyInfo(SkillName.Mining,			1002111, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock), 5, 1, 15, 1002112, new PropInfo(6, 15, 15, new int[] { 20 })));

            // Non-Imbuables for getting item intensity only
            Register(200, new ItemPropertyInfo(AosWeaponAttribute.BloodDrinker, 1017407, 140, 0, 1, 1, 1152387,
                new PropInfo(1, 0, 1)));

            Register(201, new ItemPropertyInfo(AosWeaponAttribute.BattleLust, 1113710, 140,  0, 1, 1, 1152385,
                new PropInfo(1, 0, 1)));

            Register(202, new ItemPropertyInfo(AosWeaponAttribute.HitCurse, 1154673, 140, 1, 2, 50, 1152438));

            Register(203, new ItemPropertyInfo(AosWeaponAttribute.HitFatigue, 1154668, 140, 1, 2, 50, 1152437,
                 new PropInfo(1, 0, 70), new PropInfo(2, 0, 70)));

            Register(204, new ItemPropertyInfo(AosWeaponAttribute.HitManaDrain, 1154669, 140, 1, 2, 50, 1152436,
                 new PropInfo(1, 0, 70), new PropInfo(2, 0, 70)));

            Register(205, new ItemPropertyInfo(AosWeaponAttribute.SplinteringWeapon, 1154670, 140, 1, 1, 50, 1152396, // TODO: Check MaxInt
                 new PropInfo(1, 0, 70)));

            Register(206, new ItemPropertyInfo(AosWeaponAttribute.ReactiveParalyze, 1154660, 140, 0, 1, 1, 1152400,
                 new PropInfo(1, 0, 1)));

            Register(233, new ItemPropertyInfo(AosWeaponAttribute.ResistPhysicalBonus, 1061158, 100, typeof(MagicalResidue), typeof(Diamond), typeof(BouraPelt), 1, 1, 15, 1112010,
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 15, 15, new int[] { 20 })));

            Register(234, new ItemPropertyInfo(AosWeaponAttribute.ResistFireBonus, 1061159, 100, typeof(MagicalResidue), typeof(Ruby), typeof(BouraPelt), 1, 1, 15, 1112009,
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 15, 15, new int[] { 20 })));

            Register(235, new ItemPropertyInfo(AosWeaponAttribute.ResistColdBonus, 1061160, 100, typeof(MagicalResidue), typeof(Sapphire), typeof(BouraPelt), 1, 1, 15, 1112007,
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 15, 15, new int[] { 20 })));

            Register(236, new ItemPropertyInfo(AosWeaponAttribute.ResistPoisonBonus, 1061161, 100, typeof(MagicalResidue), typeof(Emerald), typeof(BouraPelt), 1, 1, 15, 1112011,
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 15, 15, new int[] { 20 })));

            Register(237, new ItemPropertyInfo(AosWeaponAttribute.ResistEnergyBonus, 1061162, 100, typeof(MagicalResidue), typeof(Amethyst), typeof(BouraPelt), 1, 1, 15, 1112008,
                new PropInfo(1, 15, 15, new int[] { 20 }), new PropInfo(2, 15, 15, new int[] { 20 })));

            Register(208, new ItemPropertyInfo(SAAbsorptionAttribute.EaterFire, 1154662, 140, 1, 1, 15, 1152390,
                new PropInfo(3, 0, 15), new PropInfo(4, 0, 15), new PropInfo(5, 0, 15)));

            Register(209, new ItemPropertyInfo(SAAbsorptionAttribute.EaterCold, 1154663, 140, 1, 1, 15, 1152390,
                new PropInfo(3, 0, 15), new PropInfo(4, 0, 15), new PropInfo(5, 0, 15)));

            Register(210, new ItemPropertyInfo(SAAbsorptionAttribute.EaterPoison, 1154664, 140, 1, 1, 15, 1152390,
                new PropInfo(3, 0, 15), new PropInfo(4, 0, 15), new PropInfo(5, 0, 15)));

            Register(211, new ItemPropertyInfo(SAAbsorptionAttribute.EaterEnergy, 1154665, 140, 1, 1, 15, 1152390,
                new PropInfo(3, 0, 15), new PropInfo(4, 0, 15), new PropInfo(5, 0, 15)));

            Register(212, new ItemPropertyInfo(SAAbsorptionAttribute.EaterKinetic, 1154666, 140, 1, 1, 15, 1152390,
                new PropInfo(3, 0, 15), new PropInfo(4, 0, 15), new PropInfo(5, 0, 15)));

            Register(213, new ItemPropertyInfo(SAAbsorptionAttribute.EaterDamage, 1154667, 140, 1, 1, 15, 1152390,
                new PropInfo(3, 0, 15), new PropInfo(4, 0, 15), new PropInfo(5, 0, 15)));

            // Non-Imbuable, non-loot
            Register(214, new ItemPropertyInfo(SAAbsorptionAttribute.ResonanceFire,       1154655, 140, 1, 1, 20, 1152391));
            Register(215, new ItemPropertyInfo(SAAbsorptionAttribute.ResonanceCold,       1154656, 140, 1, 1, 20, 1152391));
            Register(216, new ItemPropertyInfo(SAAbsorptionAttribute.ResonancePoison,     1154657, 140, 1, 1, 20, 1152391));
            Register(217, new ItemPropertyInfo(SAAbsorptionAttribute.ResonanceEnergy,     1154658, 140, 1, 1, 20, 1152391));
            Register(218, new ItemPropertyInfo(SAAbsorptionAttribute.ResonanceKinetic,    1154659, 140, 1, 1, 20, 1152391));

            Register(219, new ItemPropertyInfo(SAAbsorptionAttribute.CastingFocus, 1116535, 140, 1, 1, 3, 1116535,
                new PropInfo(3, 0, 3), new PropInfo(5, 0, 3)));

            Register(220, new ItemPropertyInfo(AosArmorAttribute.ReactiveParalyze, 1154660, 140, 0, 1, 1, 1152400,
                new PropInfo(1, 0, 1), new PropInfo(4, 0, 1)));

            Register(221, new ItemPropertyInfo(AosArmorAttribute.SoulCharge, 1116536, 140, 5, 5, 30, 1152391,
                new PropInfo(4, 0, 30)));

            Register(500, new ItemPropertyInfo(AosArmorAttribute.SelfRepair, 1079709, 100, 1, 1, 5, 1079709,
                new PropInfo(3, 5, 5), new PropInfo(4, 5, 5), new PropInfo(5, 5, 5)));

            Register(501, new ItemPropertyInfo(AosWeaponAttribute.SelfRepair, 1079709, 100, 1, 1, 5, 1079709,
                new PropInfo(1, 5, 5), new PropInfo(2, 5, 5)));

            // Non-Imbuable, non-loot
            Register(600, new ItemPropertyInfo(ExtendedWeaponAttribute.BoneBreaker,       1157318, 140, 1,  1, 1,  1157319));
            Register(601, new ItemPropertyInfo(ExtendedWeaponAttribute.HitSwarm,          1157328, 140, 1,  1, 20, 1157327));
            Register(602, new ItemPropertyInfo(ExtendedWeaponAttribute.HitSparks,         1157330, 140, 1,  1, 20,  1157329));
            Register(603, new ItemPropertyInfo(ExtendedWeaponAttribute.Bane,              1154671, 140, 1,  1, 1,  1154570));

            BuildLootTables();
        }

        private static void Register(int id, ItemPropertyInfo info)
        {
            if (Table.ContainsKey(id))
            {
                throw new ArgumentException(String.Format("ID Already Exists: {0}", id));
            }
            else
            {
                info.ID = id;
                Table[id] = info;
            }
        }

        public PropInfo GetItemTypeInfo(ItemType type)
        {
            return PropCategories.FirstOrDefault(prop => prop != null && prop.ItemType == type);
        }

        public bool CanImbue(ItemType type)
        {
            var info = GetItemTypeInfo(type);

            if (info != null && info.StandardMax > 0)
            {
                return true;
            }

            return false;
        }

        public static ItemPropertyInfo GetInfo(object attribute)
        {
            var id = GetID(attribute);

            if (Table.ContainsKey(id))
            {
                return Table[id];
            }

            return null;
        }

        public static ItemPropertyInfo GetInfo(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table[id];
            }

            return null;
        }

        public static TextDefinition GetAttributeName(object o)
        {
            return GetAttributeName(GetID(o));
        }

        public static TextDefinition GetAttributeName(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table[id].AttributeName;
            }

            return null;
        }

        /// <summary>
        /// Maximum intensity in regards to imbuing weight calculation. Some items may be over this 'cap'
        /// </summary>
        /// <param name="item"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static int GetMaxIntensity(Item item, object attribute)
        {
            return GetMaxIntensity(item, GetID(attribute));
        }

        public static int GetMaxIntensity(Item item, int id)
        {
            if (Table.ContainsKey(id))
            {
                var info = Table[id].GetItemTypeInfo(GetItemType(item));

                // First, we try to get the max intensity from the PropInfo. If null, we go to the default MaxIntenity
                if (info == null)
                {
                    if (Core.SA && item is BaseWeapon && (id == 25 || id == 27))
                    {
                        return GetSpecialMaxIntensity((BaseWeapon)item);
                    }

                    return Table[id].MaxIntensity;
                }
                else
                {
                    if (Core.SA && item is BaseWeapon && (id == 25 || id == 27))
                    {
                        return GetSpecialMaxIntensity((BaseWeapon)item);
                    }

                    return NewLootSystem ? info.LootMax : info.StandardMax;
                }
            }

            return 0;
        }

        public static int GetSpecialMaxIntensity(BaseWeapon wep)
        {
            int max = (int)(wep.MlSpeed * 2500 / (100 + wep.Attributes.WeaponSpeed));

            if (wep is BaseRanged)
            {
                max /= 2;
            }

            return max;
        }

        public static int GetMinIntensity(Item item, object attribute, bool loot = false)
        {
            return GetMinIntensity(item, GetID(attribute), loot);
        }

        /// <summary>
        /// Minimum intensity. An item property will never start lower than this value.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="attribute"></param>
        /// <param name="loot">this will determine if default min intensity is used, or if it uses its min scaled value</param>
        /// <returns></returns>
        public static int GetMinIntensity(Item item, int id, bool loot = false)
        {
            if (Table.ContainsKey(id))
            {
                if (loot)
                {
                    // Loot min intensity is always the lowest scale value.
                    return GetScale(item, id);
                }
                else
                {
                    // Not so with imbuing, for most properties.
                    return Table[id].Start;
                }
            }

            return 0;
        }

        /// <summary>
        /// Item property incremental scale, ie 10, 20, 30 luck
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int GetScale(Item item, object attribute)
        {
            return GetScale(item, GetID(attribute));
        }

        public static int GetScale(Item item, int id)
        {
            if (Table.ContainsKey(id))
            {
                var info = Table[id].GetItemTypeInfo(GetItemType(item));

                // returns default value
                if (info == null || info.Scale < 1)
                {
                    return Table[id].Scale;
                }
                else
                {
                    return info.Scale;
                }
            }

            return 1;
        }

        /// <summary>
        /// Gets teh associated attribute, ie AosAttribute, etc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object GetAttribute(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table[id].Attribute;
            }

            return null;
        }

        public static int GetTotalWeight(Item item, object attribute, int value)
        {
            var info = GetInfo(attribute);
            int max = GetMaxIntensity(item, attribute);

            if (info != null && max > 0)
            {
                return (int)(((double)info.Weight / (double)max) * (double)value);
            }

            return 0;
        }

        public static ItemType GetItemType(Item item)
        {
            if (item is BaseRanged)
                return ItemType.Ranged;
            if (item is BaseWeapon)
                return ItemType.Melee;
            if (item is BaseShield)
                return ItemType.Shield;
            if (item is BaseArmor)
                return ItemType.Armor;
            if (item is BaseHat)
                return ItemType.Hat;
            if (item is BaseJewel)
                return ItemType.Jewel;

            return ItemType.Invalid;
        }

        public static int GetID(object attr)
        {
            int mod = -1;

            if (attr is AosAttribute)
                mod = GetIDForAttribute((AosAttribute)attr);

            else if (attr is AosWeaponAttribute)
                mod = GetIDForAttribute((AosWeaponAttribute)attr);

            else if (attr is ExtendedWeaponAttribute)
                mod = GetIDForAttribute((ExtendedWeaponAttribute)attr);

            else if (attr is SkillName)
                mod = GetIDForAttribute((SkillName)attr);

            else if (attr is SlayerName)
                mod = GetIDForAttribute((SlayerName)attr);

            else if (attr is SAAbsorptionAttribute)
                mod = GetIDForAttribute((SAAbsorptionAttribute)attr);

            else if (attr is AosArmorAttribute)
                mod = GetIDForAttribute((AosArmorAttribute)attr);

            else if (attr is AosElementAttribute)
                mod = GetIDForAttribute((AosElementAttribute)attr);

            else if (attr is TalismanSlayerName)
                mod = GetIDForAttribute((TalismanSlayerName)attr);

            else if (attr is string)
                mod = GetIDForAttribute((string)attr);

            return mod;
        }

        public static int GetIDForAttribute(AosAttribute attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is AosAttribute && (AosAttribute)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(AosWeaponAttribute attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is AosWeaponAttribute && (AosWeaponAttribute)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(ExtendedWeaponAttribute attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is ExtendedWeaponAttribute && (ExtendedWeaponAttribute)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(SAAbsorptionAttribute attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is SAAbsorptionAttribute && (SAAbsorptionAttribute)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(AosArmorAttribute attr)
        {
            if (attr == AosArmorAttribute.LowerStatReq)
                return GetIDForAttribute(AosWeaponAttribute.LowerStatReq);

            if (attr == AosArmorAttribute.DurabilityBonus)
                return GetIDForAttribute(AosWeaponAttribute.DurabilityBonus);

            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is AosArmorAttribute && (AosArmorAttribute)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(SkillName attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is SkillName && (SkillName)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(SlayerName attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is SlayerName && (SlayerName)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(TalismanSlayerName attr)
        {
            foreach (var kvp in Table)
            {
                var info = kvp.Value;

                if (info.Attribute is TalismanSlayerName && (TalismanSlayerName)info.Attribute == attr)
                    return kvp.Key;
            }

            return -1;
        }

        public static int GetIDForAttribute(AosElementAttribute type)
        {
            switch (type)
            {
                case AosElementAttribute.Physical: return 51;
                case AosElementAttribute.Fire: return 52;
                case AosElementAttribute.Cold: return 53;
                case AosElementAttribute.Poison: return 54;
                case AosElementAttribute.Energy: return 55;
            }

            return -1;
        }

        public static int GetIDForAttribute(string str)
        {
            if (str == "WeaponVelocity")
                return 60;

            if (str == "SearingWeapon")
                return 62;

            return -1;
        }

        public static bool ValidateProperty(object attribute)
        {
            return GetID(attribute) > 0;
        }

        public static void BuildLootTables()
        {
            foreach (var i in Enum.GetValues(typeof(ItemType)))
            {
                var type = (ItemType)i;

                if (type == ItemType.Invalid)
                {
                    continue;
                }

                var list = new List<int>();

                foreach (var prop in Table.Values)
                {
                    var info = prop.GetItemTypeInfo(type);

                    if (info != null)
                    {
                        if (NewLootSystem && info.LootMax > 0)
                        {
                            list.Add(prop.ID);
                        }
                        else if (!NewLootSystem && info.StandardMax > 0)
                        {
                            list.Add(prop.ID);
                        }
                    }
                }

                if (list.Count > 0)
                {
                    LootTable[type] = list;
                }
            }
        }
    }
}
