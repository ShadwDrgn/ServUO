using System;

namespace Server.Items
{
    public class SpinningwheelSouthAddon : BaseAddon, ISpinningWheel
    {
        private Timer m_Timer;
        [Constructable]
        public SpinningwheelSouthAddon()
        {
            AddComponent(new AddonComponent(0x1015), 0, 0, 0);
        }

        public SpinningwheelSouthAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed => new SpinningwheelSouthDeed();
        public bool Spinning => m_Timer != null;
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

        public override void OnComponentLoaded(AddonComponent c)
        {
            switch (c.ItemID)
            {
                case 0x1016:
                case 0x101A:
                case 0x101D:
                case 0x10A5:
                    --c.ItemID;
                    break;
            }
        }

        public void BeginSpin(SpinCallback callback, Mobile from, Item m_Wool)
        {
            m_Timer = new SpinTimer(this, callback, from, m_Wool.Hue, m_Wool.Amount);
            m_Timer.Start();
            m_Wool.Delete();


            foreach (AddonComponent c in Components)
            {
                switch (c.ItemID)
                {
                    case 0x1015:
                    case 0x1019:
                    case 0x101C:
                    case 0x10A4:
                        ++c.ItemID;
                        break;
                }
            }
        }

        public void EndSpin(SpinCallback callback, Mobile from, int hue)
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;

            foreach (AddonComponent c in Components)
            {
                switch (c.ItemID)
                {
                    case 0x1016:
                    case 0x101A:
                    case 0x101D:
                    case 0x10A5:
                        --c.ItemID;
                        break;
                }
            }

            if (callback != null)
                callback(this, from, hue);
        }

        private class SpinTimer : Timer
        {
            private readonly SpinningwheelSouthAddon m_Wheel;
            private readonly SpinCallback m_Callback;
            private readonly Mobile m_From;
            private readonly int m_Hue;
            private readonly int m_Amount;
            public SpinTimer(SpinningwheelEastAddon wheel, SpinCallback callback, Mobile from, int hue, int amount)
                : base(TimeSpan.FromSeconds(6.0))
            {
                m_Wheel = wheel;
                m_Callback = callback;
                m_From = from;
                m_Hue = hue;
                Priority = TimerPriority.TwoFiftyMS;
                m_Amount = amount;
            }

            protected override void OnTick()
            {
                m_Wheel.EndSpin(m_Callback, m_From, m_Hue, m_Amount);
            }
        }
    }

    public class SpinningwheelSouthDeed : BaseAddonDeed
    {
        [Constructable]
        public SpinningwheelSouthDeed()
        {
        }

        public SpinningwheelSouthDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon => new SpinningwheelSouthAddon();
        public override int LabelNumber => 1044342;// spining wheel (south)
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
