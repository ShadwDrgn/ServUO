using Server.Engines.BulkOrders;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class Scribe : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public Scribe()
            : base("the scribe")
        {
            this.SetSkill(SkillName.EvalInt, 60.0, 83.0);
            this.SetSkill(SkillName.Inscribe, 90.0, 100.0);
        }

        public Scribe(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild => NpcGuild.MagesGuild;
        public override VendorShoeType ShoeType => Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals;
        protected override List<SBInfo> SBInfos => this.m_SBInfos;
        public override void InitSBInfo()
        {
            this.m_SBInfos.Add(new SBScribe(this));
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            this.AddItem(new Server.Items.Robe(Utility.RandomNeutralHue()));
        }

        #region Bulk Orders
        public override BODType BODType => BODType.Inscription;

        public override bool IsValidBulkOrder(Item item)
        {
            return (item is SmallInscriptionBOD || item is LargeInscriptionBOD);
        }

        public override bool SupportsBulkOrders(Mobile from)
        {
            return BulkOrderSystem.NewSystemEnabled && from is PlayerMobile && from.Skills[SkillName.Inscribe].Base > 0;
        }

        public override void OnSuccessfulBulkOrderReceive(Mobile from)
        {
            if (from is PlayerMobile)
                ((PlayerMobile)from).NextInscriptionBulkOrder = TimeSpan.Zero;
        }

        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}