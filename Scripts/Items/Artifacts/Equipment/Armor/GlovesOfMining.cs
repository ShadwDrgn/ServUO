using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x13c6, 0x13ce)]
    public class LeatherGlovesOfMining : BaseGlovesOfMining
    {
        public override bool IsArtifact => true;
        [Constructable]
        public LeatherGlovesOfMining(int bonus)
            : base(bonus, 0x13C6)
        {
            Weight = 1;
        }

        public LeatherGlovesOfMining(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 2;
        public override int BaseFireResistance => 4;
        public override int BaseColdResistance => 3;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 30;
        public override int InitMaxHits => 40;
        public override int StrReq => 20;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.All;
        public override int LabelNumber => 1045122;// leather blacksmith gloves of mining
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

    [FlipableAttribute(0x13d5, 0x13dd)]
    public class StuddedGlovesOfMining : BaseGlovesOfMining
    {
        [Constructable]
        public StuddedGlovesOfMining(int bonus)
            : base(bonus, 0x13D5)
        {
            Weight = 2;
        }

        public StuddedGlovesOfMining(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 2;
        public override int BaseFireResistance => 4;
        public override int BaseColdResistance => 3;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 4;
        public override int InitMinHits => 35;
        public override int InitMaxHits => 45;
        public override int StrReq => 25;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Studded;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override int LabelNumber => 1045123;// studded leather blacksmith gloves of mining
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

    [Alterable(typeof(DefBlacksmithy), typeof(GargishKiltOfMining))]
    [FlipableAttribute(0x13eb, 0x13f2)]
    public class RingmailGlovesOfMining : BaseGlovesOfMining
    {
        [Constructable]
        public RingmailGlovesOfMining(int bonus)
            : base(bonus, 0x13EB)
        {
            Weight = 1;
        }

        public RingmailGlovesOfMining(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 3;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 1;
        public override int BasePoisonResistance => 5;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 40;
        public override int InitMaxHits => 50;
        public override int StrReq => 40;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Ringmail;
        public override int LabelNumber => 1045124;// ringmail blacksmith gloves of mining
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

    [FlipableAttribute(0x13eb, 0x13f2)]
    public class GargishKiltOfMining : BaseGlovesOfMining
    {
        public override Race RequiredRace => Race.Gargoyle;
        public override bool CanBeWornByGargoyles => true;

        [Constructable]
        public GargishKiltOfMining() : this(5)
        {
        }

        [Constructable]
        public GargishKiltOfMining(int bonus)
            : base(bonus, 0x030C)
        {
            Weight = 1;
        }

        public GargishKiltOfMining(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 3;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 1;
        public override int BasePoisonResistance => 5;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 40;
        public override int InitMaxHits => 50;
        public override int StrReq => 40;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Ringmail;
        public override int LabelNumber => 1045124;// ringmail blacksmith gloves of mining
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

    public abstract class BaseGlovesOfMining : BaseArmor
    {
        private int m_Bonus;
        private SkillMod m_SkillMod;
        public BaseGlovesOfMining(int bonus, int itemID)
            : base(itemID)
        {
            this.m_Bonus = bonus;

            this.Hue = CraftResources.GetHue((CraftResource)Utility.RandomMinMax((int)CraftResource.DullCopper, (int)CraftResource.Valorite));
        }

        public BaseGlovesOfMining(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bonus
        {
            get
            {
                return this.m_Bonus;
            }
            set
            {
                this.m_Bonus = value;
                this.InvalidateProperties();

                if (this.m_Bonus == 0)
                {
                    if (this.m_SkillMod != null)
                        this.m_SkillMod.Remove();

                    this.m_SkillMod = null;
                }
                else if (this.m_SkillMod == null && this.Parent is Mobile)
                {
                    this.m_SkillMod = new DefaultSkillMod(SkillName.Mining, true, this.m_Bonus);
                    ((Mobile)this.Parent).AddSkillMod(this.m_SkillMod);
                }
                else if (this.m_SkillMod != null)
                {
                    this.m_SkillMod.Value = this.m_Bonus;
                }
            }
        }
        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (this.m_Bonus != 0 && parent is Mobile)
            {
                if (this.m_SkillMod != null)
                    this.m_SkillMod.Remove();

                this.m_SkillMod = new DefaultSkillMod(SkillName.Mining, true, this.m_Bonus);
                ((Mobile)parent).AddSkillMod(this.m_SkillMod);
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (this.m_SkillMod != null)
                this.m_SkillMod.Remove();

            this.m_SkillMod = null;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (this.m_Bonus != 0)
                list.Add(1062005, this.m_Bonus.ToString()); // mining bonus +~1_val~
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Bonus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        this.m_Bonus = reader.ReadInt();
                        break;
                    }
            }

            if (this.m_Bonus != 0 && this.Parent is Mobile)
            {
                if (this.m_SkillMod != null)
                    this.m_SkillMod.Remove();

                this.m_SkillMod = new DefaultSkillMod(SkillName.Mining, true, this.m_Bonus);
                ((Mobile)this.Parent).AddSkillMod(this.m_SkillMod);
            }
        }
    }
}
