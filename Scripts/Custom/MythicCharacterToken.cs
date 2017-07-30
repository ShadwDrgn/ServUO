using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Accounting;
using Server.Engines.VeteranRewards;
using Server.Multis;
using Server.Mobiles;
using System.Collections.Generic;

// KAW:  Added

namespace Server.Items
{
    public class MythicCharacterToken : Item
    {
        // http://uo2.stratics.com/technical-information/in-game-services/mythic-character-token
        // Set up to 5 skills to 90, and sets stats to the overall cap.

        public override int LabelNumber { get { return 1152353; } }

        [Constructable]
        public MythicCharacterToken()
            : base(0x2AAA)
        {
            LootType = LootType.Blessed;
			Weight = 5.0;
        }

        public MythicCharacterToken(Serial serial)
            : base(serial)
		{
		}

        public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties(list);

			list.Add(1070998, String.Format("#{0}", 1152353));  // Use this to redeem<br>Your Mythic Character Token
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                if (from.SkillsTotal < 200) {
                    from.SendMessage("You must have less than 200 total skills to use this");
                }
                else {
                    from.CloseGump(typeof(MythicCharacterTokenGump));
                    from.SendGump(new MythicCharacterTokenGump(this, from));
                }
            }
            else
            {
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

        private const int _XButtonNormal = 0xFB1;
        private const int _XButtonPressed = 0xFB3;
        private const int _NextButtonNormal = 0xFA5;
        private const int _NextButtonPressed = 0xFA7;
        private const int _BackButtonNormal = 0xFAE;
        private const int _BackButtonPressed = 0xFB0;

        private const int _CancelButtonId = 1;
        private const int _NextButtonId = 2;
        private const int _BackButtonId = 3;
        private const int _EditButtonId = 4;
        private const int _ContinueButtonId = 5;

        private const int _MinStatValue = 10;
        private const int _MaxSelectedSkills = 5;

        private const int _TextColor = 1153;
        private const int _FontColor = 0xFFFFFF;
        private const int _AlternateFontColor = 31463;
        private const int _GreenFontColor = 5057;
        private const int _DisabledFontColor = 12684;
        private const int _ConfirmColor = 18375;
        private const int _ErrorColor = 23682;

        #region CliLocs

        /*
         * 1152352 = <center>Mythic Character Skill Selection</center>
         * 1152354 = <CENTER>Set Attributes</CENTER>
         * 1152355 = <CENTER>Total Must Equal ~1_VAL~</CENTER>
         * 1152356 = <CENTER>Selected Skills</CENTER>
         * 1152357 = <CENTER>Select Five Skills to Advance</CENTER>
         * 1152358 = Please confirm that you wish to set your attributes as indicated in the upper left area of this window. If you wish to change these values, edit them and click the EDIT button below.<br><br>Please confirm that you wish to set the five skills selected on the left to 90.0 skill. If you wish to make changes, click the [X] button next to a skill name to remove it from the list.<br><br>If are sure you wish to apply the selected skills and attributes, click the CONTINUE button below.<br><br>If you wish to abort the application of the Mythic Character Token, click the CANCEL button below.
         * 1152359 = Your Strength, Dexterity, and Intelligence values do not add up to the total indicated in the upper left area of this window. Before continuing, you must adjust these values so their total adds up to exactly the displayed value. Please edit your desired attribute values and click the EDIT button below to continue.
         * 
         * EDIT = 1150647
         * CONTINUE = 1153390
         * 
         * 1061146 = Strength
         * 1061147 = Dexterity
         * 1061148 = Intelligence
         * 
         * Magic = 1078593
         * Combat = 1078592
         * Wilderness = 1078595
         * Bard = 1078590
         * Trade Skills = 1078591
         * Thievery = 1078594
         * Miscellaneous = 1078596
         * 
         * Alchemy = 1044060
         * Anatomy = 1044061
         * Animal Lore = 1044062
         * Animal Taming = 1044095
         * Archery = 1044091
         * Arms Lore = 1044064
         * Begging = 1044066
         * Blacksmithing = 1044067
         * Bushido = 1044112
         * Camping = 1044070
         * Carpentry = 1044071
         * Cartography = 1044072
         * Chivalry = 1044111
         * Cooking = 1044073
         * Detecting Hidden = 1044074
         * Discordance = 1044075
         * Eval Intelligence = 1044076
         * Fencing = 1044102
         * Fishing = 1044078
         * Fletching = 1044068
         * Focus = 1044110
         * Forensics = 1044079
         * Healing = 1044077
         * Herding = 1044080
         * Hiding = 1044081
         * Imbuing = 1044116
         * Inscription = 1044083
         * Item Identification = 1044063
         * Lock Picking = 1044084
         * Lumberjacking = 1044104
         * Mace Fighting = 1044101
         * Magery = 1044085
         * Meditation = 1044106
         * Mining = 1044105
         * Musicianship = 1044089
         * Mysticism = 1044115
         * Necromancy = 1044109
         * Ninjitsu = 1044113
         * Parrying = 1044065
         * Peacemaking = 1044069
         * Poisoning = 1044090
         * Provocation = 1044082
         * Remove Trap = 1044108
         * Resisting Spells = 1044086
         * Snooping = 1044088
         * Spellweaving = 1044114
         * Spirit Speak = 1044092
         * Stealing = 1044093
         * Stealth = 1044107
         * Swordsmanship = 1044100
         * Tactics = 1044087
         * Tailoring = 1044094
         * Taste Identification = 1044096
         * Throwing = 1044117
         * Tinkering = 1044097
         * Tracking = 1044098
         * Veterinary = 1044099
         * Wrestling = 1044103
         */

        #endregion

        private class MythicCharacterTokenGump : Gump
        {
            private static readonly Dictionary<int, string> _SkillMap = new Dictionary<int, string>
            {
                {1044060, "Alchemy"}, {1044061, "Anatomy"}, {1044062, "Animal Lore"}, {1044095, "Animal Taming"}, {1044091, "Archery"},
                {1044064, "Arms Lore"}, {1044066, "Begging"}, {1044067, "Blacksmithy"}, {1044112, "Bushido"}, {1044070, "Camping"},
                {1044071, "Carpentry"}, {1044072, "Cartography"}, {1044111, "Chivalry"}, {1044073, "Cooking"}, {1044074, "Detecting Hidden"},
                {1044075, "Discordance"}, {1044076, "Evaluating Intelligence"}, {1044102, "Fencing"}, {1044078, "Fishing"}, {1044068, "Bowcraft/Fletching"},
                {1044110, "Focus"}, {1044079, "Forensic Evaluation"}, {1044077, "Healing"}, {1044080, "Herding"}, {1044081, "Hiding"},
                {1044116, "Imbuing"}, {1044083, "Inscription"}, {1044063, "Item Identification"}, {1044084, "Lockpicking"}, {1044104, "Lumberjacking"},
                {1044101, "Mace Fighting"}, {1044085, "Magery"}, {1044106, "Meditation"}, {1044105, "Mining"}, {1044089, "Musicianship"},
                {1044115, "Mysticism"}, {1044109, "Necromancy"}, {1044113, "Ninjitsu"}, {1044065, "Parrying"}, {1044069, "Peacemaking"},
                {1044090, "Poisoning"}, {1044082, "Provocation"}, {1044108, "Remove Trap"}, {1044086, "Resisting Spells"}, {1044088, "Snooping"},
                {1044114, "Spellweaving"}, {1044092, "Spirit Speak"}, {1044093, "Stealing"}, {1044107, "Stealth"}, {1044100, "Swordsmanship"},
                {1044087, "Tactics"}, {1044094, "Tailoring"}, {1044096, "Taste Identification"}, {1044117, "Throwing"}, {1044097, "Tinkering"},
                {1044098, "Tracking"}, {1044099, "Veterinary"}, {1044103, "Wrestling"}
            };

            private static readonly List<int> _MagicSkills = new List<int> { 1044112, 1044111, 1044076, 1044116, 1044085, 1044106, 1044115, 1044109, 1044113, 1044086, 1044114, 1044092 };
            private static readonly List<int> _CombatSkills = new List<int> { 1044061, 1044091, 1044102, 1044110, 1044077, 1044101, 1044065, 1044100, 1044087, 1044117, 1044103 };
            private static readonly List<int> _BardSkills = new List<int> { 1044075, 1044089, 1044069, 1044082 };
            private static readonly List<int> _WildernessSkills = new List<int> { 1044062, 1044095, 1044078, 1044080, 1044098, 1044099 };
            private static readonly List<int> _TradeSkills = new List<int> { 1044060, 1044067, 1044068, 1044071, 1044073, 1044083, 1044104, 1044105, 1044094, 1044097 };
            private static readonly List<int> _MiscellaneousSkills = new List<int> { 1044064, 1044066, 1044070, 1044072, 1044079, 1044063, 1044096 };
            private static readonly List<int> _ThieverySkills = new List<int> { 1044074, 1044081, 1044084, 1044090, 1044108, 1044088, 1044093, 1044107 };

            private readonly Item _Item;
            private readonly Mobile _Mobile;
            private readonly List<int> _SelectedSkills;
            private readonly int _Page;
            private readonly int _Strength;
            private readonly int _Dexterity;
            private readonly int _Intelligence;

            private void AddSharedItems()
            {
                AddBackground(0, 0, 520, 482, 5054);

                AddImageTiled(10, 8, 500, 22, 2624);
                AddImageTiled(10, 38, 500, 404, 2624);
                AddImageTiled(10, 450, 500, 22, 2624);
                
                AddAlphaRegion(10, 10, 500, 462);
                AddHtmlLocalized(10, 8, 500, 20, 1152352, _FontColor, false, false);  // Mythic Character Skill Selection

                AddHtmlLocalized(10, 44, 180, 20, 1152354, _AlternateFontColor, false, false);  // Set Attributes
                AddHtmlLocalized(10, 64, 180, 20, 1152355, _Mobile.StatCap.ToString(), _AlternateFontColor, false, false);  // Total Must Equal ~1_VAL~
                AddHtmlLocalized(10, 165, 180, 20, 1152356, _AlternateFontColor, false, false);  // Selected Skills

                AddBackground(11, 81, 57, 20, 0x2486);
                AddTextEntry(11, 81, 57, 20, _TextColor, 0, _Strength.ToString(), 3);
                AddBackground(11, 102, 57, 20, 0x2486);
                AddTextEntry(11, 102, 57, 20, _TextColor, 1, _Dexterity.ToString(), 3);
                AddBackground(11, 123, 57, 20, 0x2486);
                AddTextEntry(11, 123, 57, 20, _TextColor, 2, _Intelligence.ToString(), 3);

                AddHtmlLocalized(70, 81, 100, 20, 1061146, _FontColor, false, false);  // Strength
                AddHtmlLocalized(70, 102, 100, 20, 1061147, _FontColor, false, false);  // Dexterity
                AddHtmlLocalized(70, 123, 100, 20, 1061148, _FontColor, false, false);  // Intelligence

                if (_Page < 2)
                {
                    AddHtmlLocalized(175, 44, 295, 20, 1152357, _AlternateFontColor, false, false);  // Select Five Skills to Advance
                }

                var offsetY = 183;

                foreach (var selectedSkill in _SelectedSkills)
                {
                    AddButton(14, offsetY, _XButtonNormal, _XButtonPressed, selectedSkill, GumpButtonType.Reply, 0);
                    AddHtmlLocalized(44, offsetY, 120, 20, selectedSkill, _GreenFontColor, false, false);
                    offsetY += 20;
                }

                AddButton(10, 450, _XButtonNormal, _XButtonPressed, _CancelButtonId, GumpButtonType.Reply, 0);
                AddHtmlLocalized(45, 450, 50, 20, 1011012, _FontColor, false, false);  // CANCEL
            }

            public MythicCharacterTokenGump(Item item, Mobile from, List<int> selectedSkills, int strength, int dexterity, int intelligence, int page)
                : base(200, 50)
            {
                _Item = item;
                _Mobile = from;
                _SelectedSkills = new List<int>();
                
                if (selectedSkills != null)
                {
                    _SelectedSkills.AddRange(selectedSkills);
                }

                _Strength = strength;
                _Dexterity = dexterity;
                _Intelligence = intelligence;
                _Page = page;
                
                AddPage(0);
                AddSharedItems();

                int offsetY;

                switch (_Page)
                {
                    case 1:
                        // Main Page, 2
                        if (_SelectedSkills.Count >= 5)
                        {
                            AddButton(400, 450, _NextButtonNormal, _NextButtonPressed, _NextButtonId, GumpButtonType.Reply, 0);
                            AddHtmlLocalized(435, 450, 50, 20, 1043353, _FontColor, false, false);  // Next
                        }
                        else
                        {
                            AddButton(300, 450, _BackButtonNormal, _BackButtonPressed, _BackButtonId, GumpButtonType.Reply, 0);
                            AddHtmlLocalized(335, 450, 50, 20, 1005007, _FontColor, false, false);  // Back
                        }

                        offsetY = 82;
                        AddHtml(175, 65, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Trade Skills</CENTER></BASEFONT>", false, false);  // Trade Skills

                        foreach (var skill in _TradeSkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(175, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(210, offsetY, 120, 22, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        offsetY = 300;
                        AddHtml(175, 282, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Miscellaneous</CENTER></BASEFONT>", false, false);  // Miscellaneous
                        
                        foreach (var skill in _MiscellaneousSkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(175, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(210, offsetY, 120, 20, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        offsetY = 170;
                        AddHtml(335, 152, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Thievery</CENTER></BASEFONT>", false, false);  // Thievery

                        foreach (var skill in _ThieverySkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(335, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(370, offsetY, 120, 20, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        break;
                    case 2:
                        // Confirm Page/Error
                        var isInError = (_Strength + _Dexterity + _Intelligence != _Mobile.StatCap)
                            || (_Strength < _MinStatValue || _Dexterity < _MinStatValue || _Intelligence < _MinStatValue);

                        AddHtmlLocalized(200, 38, 300, 345, isInError ? 1152359 : 1152358, isInError ? _ErrorColor : _ConfirmColor, false, true);

                        AddButton(200, 382, _NextButtonNormal, _NextButtonPressed, _EditButtonId, GumpButtonType.Reply, 0);
                        AddHtmlLocalized(235, 382, 80, 20, 1150647, _FontColor, false, false);  // EDIT
                        
                        AddButton(200, 402, _NextButtonNormal, _NextButtonPressed, _ContinueButtonId, GumpButtonType.Reply, 0);
                        AddHtmlLocalized(235, 402, 80, 20, 1153390, _FontColor, false, false);  // CONTINUE

                        break;
                    default:
                        // Main Page, 1
                        AddButton(400, 450, _NextButtonNormal, _NextButtonPressed, _NextButtonId, GumpButtonType.Reply, 0);
                        AddHtmlLocalized(435, 450, 55, 20, 1043353, _FontColor, false, false);  // Next

                        offsetY = 82;
                        AddHtml(175, 65, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Magic</CENTER></BASEFONT>", false, false);  // Magic

                        foreach (var skill in _MagicSkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(175, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(210, offsetY, 120, 22, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        offsetY = 82;
                        AddHtml(335, 65, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Combat</CENTER></BASEFONT>", false, false);  // Combat

                        foreach (var skill in _CombatSkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(335, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(370, offsetY, 120, 20, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        offsetY = 360;
                        AddHtml(175, 340, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Bard</CENTER></BASEFONT>", false, false);  // Bard
                        
                        foreach (var skill in _BardSkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(175, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(210, offsetY, 120, 20, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        offsetY = 320;
                        AddHtml(335, 300, 160, 20, "<BASEFONT COLOR=#FFFFFF><CENTER>Wilderness</CENTER></BASEFONT>", false, false);  // Wilderness

                        foreach (var skill in _WildernessSkills)
                        {
                            var canTrainSkill = CanTrainSkill(skill);

                            if (canTrainSkill)
                            {
                                AddButton(335, offsetY, _NextButtonNormal, _NextButtonPressed, skill, GumpButtonType.Reply, 0);
                            }

                            AddHtmlLocalized(370, offsetY, 120, 20, skill, canTrainSkill ? _GreenFontColor : _DisabledFontColor, false, false);  // Skill Name
                            offsetY += 20;
                        }

                        break;
                }
            }

            public MythicCharacterTokenGump(Item item, Mobile from)
                : this(item, from, null, from.RawStr, from.RawDex, from.RawInt, 0)
            {
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                // If the item is gone, or an invalid button was pressed,
                // close the gump.
                if (_Item == null || _Item.Deleted || info.ButtonID == 0)
                {
                    _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                    return;
                }

                // If the item is no longer in the plyaer's backpack,
                // close the gump, and send them a message.
                if (!_Item.IsChildOf(_Mobile.Backpack))
                {
                    _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                    _Mobile.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
                    return;
                }

                // Set stats.
                var strengthString = info.GetTextEntry(0).Text;
                int strength;
                var dexterityString = info.GetTextEntry(1).Text;
                int dexterity;
                var intelligenceString = info.GetTextEntry(2).Text;
                int intelligence;

                if (!int.TryParse(strengthString, out strength))
                {
                    strength = _Mobile.RawStr;
                }

                if (!int.TryParse(dexterityString, out dexterity))
                {
                    dexterity = _Mobile.RawDex;
                }

                if (!int.TryParse(intelligenceString, out intelligence))
                {
                    intelligence = _Mobile.RawInt;
                }

                switch (info.ButtonID)
                {
                    case _CancelButtonId:
                        _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                        break;
                    case _NextButtonId:
                        _Mobile.CloseGump(typeof(MythicCharacterTokenGump));

                        if (_SelectedSkills.Count == _MaxSelectedSkills)
                        {
                            _Mobile.SendGump(new MythicCharacterTokenGump(_Item, _Mobile, _SelectedSkills, strength, dexterity, intelligence, 2));
                        }
                        else
                        {
                            _Mobile.SendGump(new MythicCharacterTokenGump(_Item, _Mobile, _SelectedSkills, strength, dexterity, intelligence, _Page + 1));
                        }
                        break;
                    case _BackButtonId:
                        _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                        _Mobile.SendGump(new MythicCharacterTokenGump(_Item, _Mobile, _SelectedSkills, strength, dexterity, intelligence, _Page - 1));
                        break;
                    case _ContinueButtonId:
                        // Set Stats
                        _Mobile.RawStr = _Strength;
                        _Mobile.RawDex = _Dexterity;
                        _Mobile.RawInt = _Intelligence;

                        // Set Skills
                        foreach (var skillId in _SelectedSkills)
                        {
                            var name = _SkillMap[skillId];
                            var internalSkillId = 0;

                            foreach (var skillName in SkillInfo.Table)
                            {
                                if (skillName.Name != name) continue;

                                internalSkillId = skillName.SkillID;
                                break;
                            }

                            var skill = _Mobile.Skills[internalSkillId];

                            // Make sure the lock is up
                            skill.SetLockNoRelay(SkillLock.Up);
                            skill.Base = 90;
                        }

                        _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                        _Item.Delete();
                        break;
                    case _EditButtonId:
                        _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                        _Mobile.SendGump(new MythicCharacterTokenGump(_Item, _Mobile, _SelectedSkills, strength, dexterity, intelligence, _Page));
                        break;
                    default:
                        if (_MagicSkills.Contains(info.ButtonID) || _CombatSkills.Contains(info.ButtonID) || _BardSkills.Contains(info.ButtonID)
                            || _WildernessSkills.Contains(info.ButtonID) || _TradeSkills.Contains(info.ButtonID) || _MiscellaneousSkills.Contains(info.ButtonID)
                            || _ThieverySkills.Contains(info.ButtonID))
                        {
                            if (_SelectedSkills.Contains(info.ButtonID))
                            {
                                _SelectedSkills.Remove(info.ButtonID);
                            }
                            else
                            {
                                _SelectedSkills.Add(info.ButtonID);
                            }
                        }

                        var page = _Page;

                        if (_SelectedSkills.Count == _MaxSelectedSkills)
                        {
                            page = 2;
                        }
                        else if (_SelectedSkills.Count < _MaxSelectedSkills && _Page > 1)
                        {
                            page = 0;
                        }

                        _Mobile.CloseGump(typeof(MythicCharacterTokenGump));
                        _Mobile.SendGump(new MythicCharacterTokenGump(_Item, _Mobile, _SelectedSkills, strength, dexterity, intelligence, page));
                        break;
                }
            }

            private bool CanTrainSkill(int skillId)
            {
                // Disable Spellweaving if they haven't trained it.
                // (if it's >= 90, code below will handle that)
                if (skillId == 1044114 && _Mobile.Skills.Spellweaving.Base == 0) return false;

                // Gargoyles cannot learn Archery
                if (_Mobile.Race == Race.Gargoyle && skillId == 1044091) return false;

                // Non-Gargoyles cannot learn Throwing
                if (_Mobile.Race != Race.Gargoyle && skillId == 1044117) return false;

                // If the skill is >= 90, disable.
                var name = _SkillMap[skillId];
                var internalSkillId = 0;

                foreach (var skillName in SkillInfo.Table)
                {
                    if (skillName.Name != name) continue;

                    internalSkillId = skillName.SkillID;
                    break;
                }

                if (_Mobile.Skills[internalSkillId].Base >= 90) return false;

                return true;
            }
        }
    }
}

