using System;

namespace Server.Items
{
    public class ElvenSpinningwheelSouthAddon : BaseAddon, ISpinningWheel
    {
        private Timer m_Timer;
        [Constructable]
        public ElvenSpinningwheelSouthAddon()
        {
            AddComponent(new AddonComponent(0x2E3F), 0, 0, 0);
        }

        public ElvenSpinningwheelSouthAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed => new ElvenSpinningwheelSouthDeed();
        public bool Spinning => m_Timer != null;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }

        public override void OnComponentLoaded(AddonComponent c)
        {
            switch (c.ItemID)
            {
                case 0x2E3E:
                    ++c.ItemID;
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
                    case 0x2E3F:
                        --c.ItemID;
                        break;
                }
            }
        }

        public void EndSpin(SpinCallback callback, Mobile from, int hue, int amount)
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;

            foreach (AddonComponent c in Components)
            {
                switch (c.ItemID)
                {
                    case 0x2E3E:
                        ++c.ItemID;
                        break;
                }
            }

            if (callback != null)
                callback(this, from, hue, amount);
        }

        private class SpinTimer : Timer
        {
            private readonly ElvenSpinningwheelSouthAddon m_Wheel;
            private readonly SpinCallback m_Callback;
            private readonly Mobile m_From;
            private readonly int m_Hue;
            private readonly int m_Amount;
            public SpinTimer(ElvenSpinningwheelSouthAddon wheel, SpinCallback callback, Mobile from, int hue, int amount)
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

    public class ElvenSpinningwheelSouthDeed : BaseAddonDeed
    {
        [Constructable]
        public ElvenSpinningwheelSouthDeed()
        {
        }

        public ElvenSpinningwheelSouthDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon => new ElvenSpinningwheelSouthAddon();
        public override int LabelNumber => 1072878;// spinning wheel (south)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
