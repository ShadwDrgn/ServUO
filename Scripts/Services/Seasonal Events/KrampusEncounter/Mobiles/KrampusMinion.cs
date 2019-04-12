using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a minion corpse")]
    public class KrampusMinion : BaseCreature
    {
        [Constructable]
        public KrampusMinion()
           : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Minion";
            Body = 1485;
            BaseSoundID = 422;

            SetStr(125);
            SetDex(125);
            SetInt(250);

            SetHits(1400);

            SetDamage(15, 21);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Tactics, 110, 120);
            SetSkill(SkillName.Wrestling, 100, 110);
            SetSkill(SkillName.DetectHidden, 60.0, 70.0);
            SetSkill(SkillName.MagicResist, 120);
            SetSkill(SkillName.Parry, 60, 70);

            Fame = 3000;
            Karma = -3000;
        }

        public KrampusMinion(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
