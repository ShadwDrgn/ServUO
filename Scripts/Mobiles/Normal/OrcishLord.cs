using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an orcish corpse")]
    public class OrcishLord : BaseCreature
    {
        [Constructable]
        public OrcishLord()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an orcish lord";
            this.Body = 138;
            this.BaseSoundID = 0x45A;

            this.SetStr(147, 215);
            this.SetDex(91, 115);
            this.SetInt(61, 85);

            this.SetHits(95, 123);

            this.SetDamage(4, 14);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 35);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 70.1, 85.0);
            this.SetSkill(SkillName.Swords, 60.1, 85.0);
            this.SetSkill(SkillName.Tactics, 75.1, 90.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 85.0);

            this.Fame = 2500;
            this.Karma = -2500;

            this.PackItem(new RingmailChest());

            if (0.3 > Utility.RandomDouble())
                this.PackItem(Loot.RandomPossibleReagent());

            if (0.2 > Utility.RandomDouble())
                this.PackItem(new BolaBall());

            if (0.5 > Utility.RandomDouble())
                PackItem(new Yeast());
        }

        public OrcishLord(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 1;

        public override TribeType Tribe => TribeType.Orc;

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Meager);
            this.AddLoot(LootPack.Average);
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            Item item = aggressor.FindItemOnLayer(Layer.Helm);

            if (item is OrcishKinMask)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
