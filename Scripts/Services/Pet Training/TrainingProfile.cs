using System;
using Server;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Network;
using Server.Gumps;

namespace Server.Mobiles
{

    public enum TrainingMode
    {
        Regular,
        Planning
    }

    [PropertyObject]
    public class TrainingProfile
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public TrainingMode TrainingMode { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasBegunTraining { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasIncreasedControlSlot { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasRecievedControlSlotWarning { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double TrainingProgress { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double TrainingProgressMax { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseCreature Creature { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double TrainingProgressPercentile { get { return TrainingProgress / TrainingProgressMax; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ControlSlots { get { return Creature.ControlSlots; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ControlSlotsMin { get { return Creature.ControlSlotsMin; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ControlSlotsMax { get { return Creature.ControlSlotsMax; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanApplyOptions { get { return HasBegunTraining && TrainingProgressPercentile >= 1.0; } }

        private int _TrainingPoints;
        private int _StartingTrainingPoints;
        private PlanningProfile _Plan;

        [CommandProperty(AccessLevel.GameMaster)]
        public int TrainingPoints
        {
            get { return _TrainingPoints; }
            set
            {
                if (value <= 0)
                {
                    EndTraining();
                }
                else
                {
                    _TrainingPoints = value;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StartingTrainingPoints
        {
            get { return _StartingTrainingPoints; }
            set { _StartingTrainingPoints = value; }
        }

        public PlanningProfile PlanningProfile
        {
            get
            {
                if (_Plan == null)
                    _Plan = new PlanningProfile(Creature);

                return _Plan;
            }
        }

        public static TimeSpan PowerHourDuration = TimeSpan.FromHours(1);

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime PowerHourBegin { get; set; }

        public bool InPowerHour { get { return PowerHourBegin + PowerHourDuration > DateTime.UtcNow; } }

        public TrainingProfile(BaseCreature bc)
        {
            Creature = bc;
        }

        private void AssignStartingTrainingPoints()
        {
            if (ControlSlotsMin == 1 && ControlSlotsMax == 2)
            {
                _StartingTrainingPoints = 2556;
            }
            else if (ControlSlotsMin == 1 && ControlSlotsMax == 3)
            {
                _StartingTrainingPoints = 2381;
            }
            else
            {
                _StartingTrainingPoints = 1501;
            }
        }

        public void BeginTraining()
        {
            if (_StartingTrainingPoints == 0)
            {
                AssignStartingTrainingPoints();
            }

            if (ControlSlots < ControlSlotsMax)
            {
                TrainingPoints = StartingTrainingPoints;
                HasBegunTraining = true;

                TrainingProgress = 0;
                TrainingProgressMax = 100;
            }
        }

        public void OnTrain(PlayerMobile pm, int points)
        {
            if (!HasIncreasedControlSlot)
            {
                Creature.RemoveFollowers();
                Creature.ControlSlots++;
                Creature.AddFollowers();

                pm.SendLocalizedMessage(1157537); // Your pet's control slot have been updated.

                HasIncreasedControlSlot = true;
            }

            TrainingPoints -= points;
        }

        public void EndTraining()
        {
            HasBegunTraining = false;
            _TrainingPoints = 0;

            HasRecievedControlSlotWarning = false;
            HasIncreasedControlSlot = false;
        }

        public void CheckProgress(BaseCreature bc)
        {
            if (ControlSlots >= ControlSlotsMax || !HasBegunTraining || TrainingProgress >= TrainingProgressMax)
                return;

            int dif = (int)(Creature.BardingDifficulty - bc.BardingDifficulty);
            int level = 1 + (ControlSlots - ControlSlotsMin);

            if (Utility.Random(100) < (6 - level))
            {
                if (dif <= 50)
                {
                    double toAdd = Math.Round(.25 + (Math.Max(2, (bc.BardingDifficulty / Creature.BardingDifficulty)) * 2.5), 2);

                    TrainingProgress = Math.Min(TrainingProgressMax, TrainingProgress + toAdd);

                    if (Creature.ControlMaster != null)
                    {
                        int cliloc = 1157574; // *The pet's battle experience has greatly increased!*

                        if (toAdd < 1.3)
                            cliloc = 1157565; // *The pet's battle experience has slightly increased!*
                        else if (toAdd < 2.5)
                            cliloc = 1157573; // *The pet's battle experience has fairly increased!*

                        if (Creature.ControlMaster.HasGump(typeof(PetTrainingProgressGump)))
                        {
                            ResendProgressGump(Creature.ControlMaster);
                        }

                        Creature.PrivateOverheadMessage(MessageType.Regular, 0x59, cliloc, Creature.ControlMaster.NetState);

                        if (TrainingProgress >= TrainingProgressMax)
                        {
                            Creature.PrivateOverheadMessage(MessageType.Regular, 0x59, 1157543, Creature.ControlMaster.NetState); // *The creature surges with battle experience and is ready to train!*

                            Server.Engines.Quests.LeadingIntoBattleQuest.CheckComplete(Creature.ControlMaster as PlayerMobile);
                        }
                    }
                }
                else if (Creature.ControlMaster != null)
                {
                    Creature.PrivateOverheadMessage(MessageType.Regular, 0x59, 1157564, Creature.ControlMaster.NetState); // *The pet does not appear to train from that*
                }
            }
        }

        public void ResendProgressGump(Mobile m)
        {
            if (m == null || m.NetState == null || !(m is PlayerMobile))
            {
                return;
            }

            PetTrainingProgressGump g = m.FindGump(typeof(PetTrainingProgressGump)) as PetTrainingProgressGump;

            if (g == null)
            {
                Server.Gumps.BaseGump.SendGump(new PetTrainingProgressGump((PlayerMobile)m, Creature));
            }
            else
            {
                g.Refresh();
            }
        }

        public override string ToString()
        {
            return "...";
        }

        public TrainingProfile(BaseCreature bc, GenericReader reader)
        {
            int version = reader.ReadInt();

            Creature = bc;

            if (reader.ReadInt() == 1)
            {
                _Plan = new PlanningProfile(bc, reader);
            }

            TrainingMode = (TrainingMode)reader.ReadInt();
            HasBegunTraining = reader.ReadBool();
            HasIncreasedControlSlot = reader.ReadBool();
            HasRecievedControlSlotWarning = reader.ReadBool();
            TrainingProgress = reader.ReadDouble();
            TrainingProgressMax = reader.ReadDouble();

            _StartingTrainingPoints = reader.ReadInt();
            _TrainingPoints = reader.ReadInt();

        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            if (_Plan != null)
            {
                writer.Write(1);
                _Plan.Serialize(writer);
            }
            else
            {
                writer.Write(0);
            }

            writer.Write((int)TrainingMode);
            writer.Write(HasBegunTraining);
            writer.Write(HasIncreasedControlSlot);
            writer.Write(HasRecievedControlSlotWarning);
            writer.Write(TrainingProgress);
            writer.Write(TrainingProgressMax);

            writer.Write(_StartingTrainingPoints);
            writer.Write(_TrainingPoints);
        }
    }
}
