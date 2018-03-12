using System;
using Server;
using System.Collections.Generic;
using System.Linq;
using Server.Items;

// Notes:
/* 1. combo of 3. Once 3 is chosen, only magical abilty can replace another magical ability.
 * 2, combat check when adding stuff 1156876
 * 
 * 
 * 
 */

namespace Server.Mobiles
{
    [Flags]
    public enum PetStat
    {
        Str,
        Dex,
        Int,
        Hits,
        Stam,
        Mana,
        RegenHits,
        RegenStam,
        RegenMana,
        BaseDamage
    }

    [Flags]
    public enum Class
    {
        None        = 0x00000000,
        Untrainable = 0x00000001,
        Magical     = 0x00000002,
        Necromantic = 0x00000004,
        Tokuno      = 0x00000008,
        StickySkin  = 0x00000010,
        Clawed      = 0x00000020,
        Tailed      = 0x00000040,
        Insectoid   = 0x00000080,
        Restricted  = 0x00000100,

        MagicalAndNecromantic = Magical | Necromantic,
        MagicalAndClawed = Magical | Clawed,
        MagicalAndTailed = Magical | Tailed,
        ClawedAndTailed = Clawed | Tailed,
        MagicalAndInsectoid = Magical | Insectoid,
        StickySkinAndTailed = StickySkin | Tailed,
        TailedAndNecromantic = Tailed | Necromantic,

        ClawedNecromanticAndTokuno = Clawed | Necromantic | Tokuno,
        MagicalClawedAndTailed = MagicalAndClawed | Tailed,
        MagicalNecromanticAndTokuno = MagicalAndNecromantic | Tokuno,
        ClawedTailedAndTokuno = ClawedAndTailed | Tokuno,
        ClawedTailedAndNecromantic = ClawedAndTailed | Necromantic,
        
        MagicalClawedTailedAndNecromantic = MagicalClawedAndTailed | Necromantic,
        ClawedTailedNecromanticAndTokuno = ClawedNecromanticAndTokuno | Tailed,
        ClawedTailedMagicalAndTokuno = MagicalClawedAndTailed | Tokuno,

        MagicalClawedTailedNecromanticAndTokuno = MagicalClawedTailedAndNecromantic | Tokuno,
    }

    [Flags]
    public enum MagicalAbility
    {
        None = 0x00000000,

        // Magical Ability
        Piercing            = 0x00000001,
        Bashing             = 0x00000002,
        Slashing            = 0x00000004,
        BattleDefense       = 0x00000008,
        WrestlingMastery    = 0x00000010,

        // Magical Schools
        Poisoning           = 0x00000020,
        Bushido             = 0x00000040,
        Ninjitsu            = 0x00000080,
        Discordance         = 0x00000100,
        MageryMastery       = 0x00000200,
        Mysticism           = 0x00000400,
        Spellweaving        = 0x00000800,
        Chivalry            = 0x00001000,
        Necromage           = 0x00002000,
        Necromancy          = 0x00004000,
        Magery              = 0x00008000,

        Tokuno = Bushido | Ninjitsu,
        SabreToothedTiger = Bashing | Piercing | Poisoning,

        Vartiety = Piercing | Slashing | WrestlingMastery,
        Variety1 = Bashing | Vartiety,
        Variety2 = Bashing | Chivalry | Discordance | MageryMastery | Mysticism | Necromage | Necromancy | Piercing | Poisoning | Slashing | Spellweaving | WrestlingMastery,

        StandardClawedOrTailed = SabreToothedTiger | Slashing | WrestlingMastery,
        Tokuno1 = Tokuno | Chivalry | Discordance | MageryMastery | Mysticism | Poisoning | Spellweaving,
        Dragon1 = Bashing | BattleDefense | Chivalry | Discordance | MageryMastery | Mysticism | Piercing | Poisoning | Slashing | Spellweaving | WrestlingMastery,
        Dragon2 = Bashing | Chivalry | Discordance | MageryMastery | Mysticism | Piercing | Poisoning | Slashing | Spellweaving | WrestlingMastery, 
        Cusidhe = Chivalry | Discordance | Mysticism | Poisoning | Spellweaving | WrestlingMastery,
        Wolf = Bashing | Tokuno | Necromage | Piercing | Poisoning | Slashing | WrestlingMastery,
        DragonWolf = Bashing | BattleDefense | Piercing | Poisoning | Slashing | WrestlingMastery,
        DreadSpider = Bashing | Tokuno | Chivalry | Discordance | MageryMastery | Mysticism | Necromage | Necromancy | Piercing | Slashing | Spellweaving | WrestlingMastery,
        DreadWarhorse = Bashing | BattleDefense | Chivalry | Discordance | MageryMastery | Mysticism | Necromage | Necromancy | Piercing | Poisoning | Slashing | Spellweaving | WrestlingMastery,
        GreaterDragon = Chivalry | Discordance | MageryMastery | Mysticism | Poisoning | Spellweaving,
        Hellcat = Bashing | Necromage | Necromancy | Piercing | Poisoning | Slashing | WrestlingMastery,
        Hiryu = Tokuno | Chivalry | Discordance | Poisoning | Spellweaving | WrestlingMastery,
        LavaLizard = Tokuno | Chivalry | Bashing,
        RuneBeetle = Chivalry | Discordance | MageryMastery | Mysticism | Spellweaving,
        StygianDrake = Bashing | Chivalry | Discordance | Mysticism | Piercing | Poisoning | Slashing | Spellweaving | WrestlingMastery,
        Triceratops = Bashing | Poisoning | Slashing | WrestlingMastery,
        TsukiWolf = Tokuno | Chivalry | Discordance | Mysticism | Necromage | Necromancy | Poisoning | Spellweaving | WrestlingMastery
    }

    public static class PetTrainingHelper
    {
        public static bool Enabled { get { return Core.TOL; } }

        public static List<TrainingPoint> TrainingPoints { get { return _TrainingPoints; } }
        public static List<TrainingPoint> _TrainingPoints;

        public static TrainingDefinition[] Definitions { get { return _Defs; } }
        private static TrainingDefinition[] _Defs =
        {
            new TrainingDefinition(typeof(Alligator), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 4),  
            new TrainingDefinition(typeof(BakeKitsune), Class.ClawedTailedAndTokuno, MagicalAbility.Tokuno1, SpecialAbilityClawedTailedAndMagical2, WepAbility2, AreaEffectArea1, 3, 5), 
            new TrainingDefinition(typeof(BaneDragon), Class.MagicalClawedAndTailed, MagicalAbility.Dragon1, SpecialAbilityBaneDragon, WepAbility2, AreaEffectArea2, 4, 5), 
            new TrainingDefinition(typeof(BattleChickenLizard), Class.Untrainable, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1), 
            new TrainingDefinition(typeof(Bird), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 2), 
            new TrainingDefinition(typeof(BlackBear), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(BloodFox), Class.None, MagicalAbility.Poisoning, SpecialAbilityNone, WepAbility3, AreaEffectNone, 2, 3),
            new TrainingDefinition(typeof(Boar), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(BrownBear), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(Bull), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1 ,4), 
            new TrainingDefinition(typeof(BullFrog), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Cat), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility4, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(Chicken), Class.Clawed, MagicalAbility.None, SpecialAbilityClawed, WepAbilityNone, AreaEffectNone, 1, 2), 
            new TrainingDefinition(typeof(ChickenLizard), Class.Untrainable, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1),
            new TrainingDefinition(typeof(LeatherWolf), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1),
            new TrainingDefinition(typeof(ColdDrake), Class.None, MagicalAbility.Dragon1, SpecialAbilityNone, WepAbility2, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(CorrosiveSlime), Class.StickySkin, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Cougar), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Cow), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(CrimsonDrake), Class.None, MagicalAbility.Dragon2, SpecialAbilityNone, WepAbility2, AreaEffectArea1, 2, 5), // CrimsonDrake[Poison] = AreaEffectArea2
            new TrainingDefinition(typeof(CuSidhe), Class.MagicalClawedAndTailed, MagicalAbility.Cusidhe, SpecialAbilityClawedTailedAndMagical2, WepAbility5, AreaEffectArea1, 3, 5), 
            //new TrainingDefinition(typeof(DarkSteed), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone), 
            new TrainingDefinition(typeof(DeathwatchBeetle), Class.Insectoid, MagicalAbility.Poisoning, SpecialAbilityMagicalInsectoid, WepAbility5, AreaEffectNone, 1, 4), 
            new TrainingDefinition(typeof(DesertOstard), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(Dimetrosaur), Class.None, MagicalAbility.Poisoning, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 3, 5), 
            new TrainingDefinition(typeof(DireWolf), Class.ClawedNecromanticAndTokuno, MagicalAbility.Wolf, SpecialAbilityClawedAndNecromantic, WepAbility1, AreaEffectNone, 1, 4), 
            new TrainingDefinition(typeof(Dog), Class.Tailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityTailed, WepAbility1, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(Dragon), Class.MagicalClawedAndTailed, MagicalAbility.Dragon2, SpecialAbilityNone, WepAbility2, AreaEffectArea1, 4, 5), 
            new TrainingDefinition(typeof(DragonTurtleHatchling), Class.MagicalAndTailed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 4, 5), 
            new TrainingDefinition(typeof(DragonWolf), Class.None, MagicalAbility.DragonWolf, SpecialAbilityNone, WepAbility1, AreaEffectNone, 4, 5), 
            new TrainingDefinition(typeof(Drake), Class.MagicalAndTailed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 2, 5), 
            new TrainingDefinition(typeof(DreadSpider), Class.MagicalNecromanticAndTokuno, MagicalAbility.DreadSpider, SpecialAbilityDreadSpider, WepAbility2, AreaEffectArea2, 3, 5), 
            new TrainingDefinition(typeof(DreadWarhorse), Class.MagicalAndNecromantic, MagicalAbility.DreadWarhorse, SpecialAbilityNone, WepAbility2, AreaEffectArea2, 3, 5), 
            new TrainingDefinition(typeof(Eagle), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3), 
            new TrainingDefinition(typeof(Ferret), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 2), 
            new TrainingDefinition(typeof(FireBeetle), Class.MagicalAndInsectoid, MagicalAbility.StandardClawedOrTailed, SpecialAbilityMagicalInsectoid, WepAbility1, AreaEffectNone, 1 ,5),
            new TrainingDefinition(typeof(FireSteed), Class.Magical, MagicalAbility.Dragon2, SpecialAbilityNone, WepAbility2, AreaEffectArea1, 2, 5),
            new TrainingDefinition(typeof(ForestOstard), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(FrenziedOstard), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(FrostDragon), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 5, 5),
            new TrainingDefinition(typeof(FrostDrake), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(FrostMite), Class.Insectoid, MagicalAbility.Poisoning, SpecialAbilityMagicalInsectoid, WepAbility1, AreaEffectNone, 2, 5),
            new TrainingDefinition(typeof(FrostSpider), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility2, AreaEffectNone, 1, 4),
            new TrainingDefinition(typeof(Gallusaurus), Class.None, MagicalAbility.Poisoning, SpecialAbilityNone, WepAbility1, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(Gaman), Class.Tailed, MagicalAbility.Poisoning, SpecialAbilityTailed, WepAbility1, AreaEffectNone, 1, 4),
            new TrainingDefinition(typeof(Beetle), Class.Insectoid, MagicalAbility.StandardClawedOrTailed, SpecialAbilityMagicalInsectoid, WepAbility1, AreaEffectNone, 1, 5),
            new TrainingDefinition(typeof(IceWyrm), Class.Tailed, MagicalAbility.Variety1, SpecialAbilityBitingTailed, WepAbility2, AreaEffectArea3, 1, 2),
            new TrainingDefinition(typeof(GiantRat), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(GiantSpider), Class.None, MagicalAbility.Variety1, SpecialAbilityBitingAnimal, WepAbility1, AreaEffectArea3, 1, 3),
            new TrainingDefinition(typeof(GiantToad), Class.StickySkin, MagicalAbility.StandardClawedOrTailed, SpecialAbilityStickySkin, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Goat), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Gorilla), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(GreatHart), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            //new TrainingDefinition(typeof(GreaterChicken), Class.Clawed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone),
            new TrainingDefinition(typeof(GreaterDragon), Class.MagicalClawedAndTailed, MagicalAbility.GreaterDragon, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 4, 5),
            new TrainingDefinition(typeof(GreaterMongbat), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(GreyWolf), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(GrizzlyBear), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(HellHound), Class.ClawedTailedNecromanticAndTokuno, MagicalAbility.Wolf, SpecialAbilityNone, WepAbility1, AreaEffectExplosiveGoo, 2, 4),
            new TrainingDefinition(typeof(HellCat), Class.ClawedTailedAndNecromantic, MagicalAbility.Hellcat, SpecialAbilityNone, WepAbility1, AreaEffectExplosiveGoo, 2, 3),
            new TrainingDefinition(typeof(HighPlainsBoura), Class.Tailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityNone, WepAbility1, AreaEffectNone, 2, 5),
            new TrainingDefinition(typeof(Hind), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Hiryu), Class.ClawedTailedMagicalAndTokuno, MagicalAbility.Hiryu, SpecialAbilityNone, WepAbility7, AreaEffectArea1, 3, 5),
            new TrainingDefinition(typeof(Horse), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Imp), Class.MagicalClawedTailedAndNecromantic, MagicalAbility.Variety2, SpecialAbilityImp, WepAbility2, AreaEffectArea2, 3, 4),
            new TrainingDefinition(typeof(IronBeetle), Class.Insectoid, MagicalAbility.StandardClawedOrTailed, SpecialAbilityMagicalInsectoid, WepAbility1, AreaEffectNone, 2, 5),
            new TrainingDefinition(typeof(JackRabbit), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(Kirin), Class.MagicalClawedAndTailed, MagicalAbility.Dragon2, SpecialAbilityClawedTailedAndMagical2, WepAbility2, AreaEffectArea1, 2, 5),
            new TrainingDefinition(typeof(Lasher), Class.Magical, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(LavaLizard), Class.ClawedTailedMagicalAndTokuno, MagicalAbility.LavaLizard, SpecialAbilityNone, WepAbility6, AreaEffectArea1, 1, 4),
            new TrainingDefinition(typeof(LesserHiryu), Class.ClawedTailedMagicalAndTokuno, MagicalAbility.Tokuno1, SpecialAbilityNone, WepAbility7, AreaEffectArea1, 1, 5),
            new TrainingDefinition(typeof(Lion), Class.ClawedAndTailed, MagicalAbility.Poisoning, SpecialAbilityBitingClawedAndTailed, WepAbility1, AreaEffectArea3, 2, 5),
            new TrainingDefinition(typeof(Llama), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility9, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(LowlandBoura), Class.Tailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityTailed, WepAbility1, AreaEffectNone, 2, 3),
            //new TrainingDefinition(typeof(MisterGobbles), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone),
            new TrainingDefinition(typeof(Mongbat), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(MountainGoat), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Najasaurus), Class.StickySkinAndTailed, MagicalAbility.Variety1, SpecialAbilityTailedAndStickySkin, WepAbility1, AreaEffectArea3, 2, 5),
            new TrainingDefinition(typeof(Nightmare), Class.MagicalAndNecromantic, MagicalAbility.Variety2, SpecialAbilityNone, WepAbility2, AreaEffectArea1, 2, 5),
            new TrainingDefinition(typeof(OsseinRam), Class.None, MagicalAbility.Variety2, SpecialAbilityNone, WepAbilityNone, AreaEffectArea1, 2, 5),
            new TrainingDefinition(typeof(PackHorse), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(PackLlama), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Palomino), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Panther), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Paralithode), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1),
            new TrainingDefinition(typeof(ParoxysmusSwampDragon), Class.Untrainable, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1),
            new TrainingDefinition(typeof(Phoenix), Class.MagicalAndClawed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(Pig), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(PlatinumDrake), Class.None, MagicalAbility.Dragon2, SpecialAbilityNone, WepAbility2, AreaEffectArea1,2, 5), // PlatinumDrake[Poison] = AreaEffectArea2
            new TrainingDefinition(typeof(PolarBear), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(PredatorHellCat), Class.ClawedTailedAndNecromantic, MagicalAbility.Hellcat, SpecialAbilityNone, WepAbility1, AreaEffectExplosiveGoo, 2, 3),
            new TrainingDefinition(typeof(Rabbit), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(Raptor), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 2, 5),
            new TrainingDefinition(typeof(Rat), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(Reptalon), Class.MagicalAndTailed, MagicalAbility.Cusidhe, SpecialAbilityNone, WepAbility10, AreaEffectArea1, 2, 5),
            new TrainingDefinition(typeof(RidableLlama), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Ridgeback), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(RuddyBoura), Class.Tailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityTailed, WepAbility1, AreaEffectNone, 2, 3),
            new TrainingDefinition(typeof(RuneBeetle), Class.Insectoid, MagicalAbility.RuneBeetle, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(SabertoothedTiger), Class.ClawedAndTailed, MagicalAbility.SabreToothedTiger, SpecialAbilitySabreTri, WepAbilityNone, AreaEffectNone, 2, 5),
            //new TrainingDefinition(typeof(SakkhranBirdOfPrey), Class.MagicalAndTailed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1),
            new TrainingDefinition(typeof(Saurosaurus), Class.Tailed, MagicalAbility.Poisoning, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(SavageRidgeback), Class.Clawed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Scorpion), Class.Tailed, MagicalAbility.Variety1, SpecialAbilityBitingTailed, WepAbility1, AreaEffectArea3, 1, 3),
            new TrainingDefinition(typeof(SerpentineDragon), Class.MagicalClawedAndTailed, MagicalAbility.Dragon2, SpecialAbilityNone, WepAbility2, AreaEffectArea2, 3, 5),
            new TrainingDefinition(typeof(Sewerrat), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility3, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(ShadowWyrm), Class.TailedAndNecromantic, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 5, 5),
            new TrainingDefinition(typeof(Sheep), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(SilverSteed), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(SkitteringHopper), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1 , 2),
            new TrainingDefinition(typeof(Skree), Class.MagicalClawedAndTailed, MagicalAbility.Dragon2, SpecialAbilityClawedTailedAndMagical1, WepAbility2, AreaEffectArea1, 3, 5),
            new TrainingDefinition(typeof(Slime), Class.StickySkin, MagicalAbility.Variety1, SpecialAbilityBitingStickySkin, WepAbility1, AreaEffectArea3, 1, 2),
            new TrainingDefinition(typeof(Slith), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityNone, WepAbility1, AreaEffectNone, 1, 4),
            new TrainingDefinition(typeof(Snake), Class.Tailed, MagicalAbility.Variety1, SpecialAbilityBitingTailed, WepAbility1, AreaEffectArea3, 1, 2),
            new TrainingDefinition(typeof(SnowLeopard), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Squirrel), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(StoneSlith), Class.ClawedAndTailed, MagicalAbility.Poisoning, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 5),
            new TrainingDefinition(typeof(StygianDrake), Class.MagicalClawedAndTailed, MagicalAbility.StygianDrake, SpecialAbilityClawedTailedAndMagical1, WepAbility2, AreaEffectArea1, 4, 5),
            new TrainingDefinition(typeof(SwampDragon), Class.Untrainable, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 1),
            new TrainingDefinition(typeof(TimberWolf), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(Triceratops), Class.Tailed, MagicalAbility.Triceratops, SpecialAbilitySabreTri, WepAbilityNone, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(TropicalBird), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(TsukiWolf), Class.MagicalClawedTailedNecromanticAndTokuno, MagicalAbility.TsukiWolf, SpecialAbilityTsukiWolf, WepAbility2, AreaEffectArea1, 3, 5),
            new TrainingDefinition(typeof(Turkey), Class.Clawed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawed, WepAbility1, AreaEffectNone, 1, 2),
            new TrainingDefinition(typeof(Unicorn), Class.Magical, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 2, 5),
            new TrainingDefinition(typeof(Vollem), Class.MagicalAndTailed, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 3, 5),
            new TrainingDefinition(typeof(Walrus), Class.None, MagicalAbility.StandardClawedOrTailed, SpecialAbilityAnimalStandard, WepAbility1, AreaEffectNone, 1, 3),
            //new TrainingDefinition(typeof(WarHorse), Class.None, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone),
            //new TrainingDefinition(typeof(WarOstard), Class.Clawed, MagicalAbility.None, SpecialAbilityClawed, WepAbilityNone, AreaEffectNone),
            new TrainingDefinition(typeof(WhiteWolf), Class.ClawedAndTailed, MagicalAbility.StandardClawedOrTailed, SpecialAbilityClawedAndTailed, WepAbility1, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(WhiteWyrm), Class.MagicalClawedAndTailed, MagicalAbility.Dragon1, SpecialAbilityClawedTailedAndMagical2, WepAbility2, AreaEffectEarthen, 3, 5),
            new TrainingDefinition(typeof(WildTiger), Class.ClawedAndTailed, MagicalAbility.Poisoning, SpecialAbilityNone, WepAbility3, AreaEffectNone, 2, 5),
            new TrainingDefinition(typeof(Windrunner), Class.TailedAndNecromantic, MagicalAbility.None, SpecialAbilityNone, WepAbilityNone, AreaEffectNone, 1, 3),
            new TrainingDefinition(typeof(WolfSpider), Class.None, MagicalAbility.Vartiety, SpecialAbilityBitingAnimal, WepAbility1, AreaEffectDisease, 1, 3),  
        };

        #region Accessors
        public static TrainingDefinition GetTrainingDefinition(BaseCreature bc)
        {
            if (bc == null)
                return null;

            return _Defs.FirstOrDefault(def => def.CreatureType == bc.GetType());
        }

        public static List<object> GivenError = new List<object>();

        public static TrainingPoint GetTrainingPoint(object o)
        {
            foreach (var tp in _TrainingPoints)
            {
                if (tp.TrainPoint is PetStat && o is PetStat && (PetStat)tp.TrainPoint == (PetStat)o)
                    return tp;

                if (tp.TrainPoint is MagicalAbility && o is MagicalAbility && (MagicalAbility)tp.TrainPoint == (MagicalAbility)o)
                    return tp;

                if (tp.TrainPoint is SpecialAbility && o is SpecialAbility && (SpecialAbility)tp.TrainPoint == (SpecialAbility)o)
                    return tp;

                if (tp.TrainPoint is WeaponAbility && o is WeaponAbility && (WeaponAbility)tp.TrainPoint == (WeaponAbility)o)
                    return tp;

                if (tp.TrainPoint is AreaEffect && o is AreaEffect && (AreaEffect)tp.TrainPoint == (AreaEffect)o)
                    return tp;

                if (tp.TrainPoint is SkillName && o is SkillName && (SkillName)tp.TrainPoint == (SkillName)o)
                    return tp;

                if (tp.TrainPoint is ResistanceType && o is ResistanceType && (ResistanceType)tp.TrainPoint == (ResistanceType)o)
                    return tp;
            }

            if (!GivenError.Contains(o))
            {
                Console.WriteLine("Null TP: {0}", o);
                GivenError.Add(o);
            }

            return null;
        }

        public static AbilityProfile GetAbilityProfile(BaseCreature bc, bool create = false)
        {
            var profile = bc.AbilityProfile;

            if (profile == null && create)
                bc.AbilityProfile = profile = new AbilityProfile(bc);

            return profile;
        }

        public static TrainingProfile GetTrainingProfile(BaseCreature bc, bool create = false)
        {
            var profile = bc.TrainingProfile;

            if (profile == null && create)
                bc.TrainingProfile = profile = new TrainingProfile(bc);

            return profile;
        }
        #endregion

        #region Magical Ability Table
        public static MagicalAbility[] MagicalAbilities =
        {
            MagicalAbility.Piercing,
            MagicalAbility.Bashing,
            MagicalAbility.Slashing,
            MagicalAbility.BattleDefense,
            MagicalAbility.WrestlingMastery,
            // Magical Schools
            MagicalAbility.Poisoning,
            MagicalAbility.Bushido,
            MagicalAbility.Ninjitsu,
            MagicalAbility.Discordance,
            MagicalAbility.MageryMastery,
            MagicalAbility.Mysticism,
            MagicalAbility.Spellweaving,
            MagicalAbility.Chivalry,
            MagicalAbility.Necromage,
            MagicalAbility.Necromancy,
            MagicalAbility.Magery,
        };
        #endregion

        #region SpecialAbility Defs
        public static SpecialAbility[] Abilities;

        public static SpecialAbility[] SpecialAbilityNone;
        public static SpecialAbility[] SpecialAbilityMagical1;
        public static SpecialAbility[] SpecialAbilityMagical2;
        public static SpecialAbility[] SpecialAbilityMagical3;
        public static SpecialAbility[] SpecialAbilityMagical4;
        public static SpecialAbility[] SpecialAbilityNecroMagical;
        public static SpecialAbility[] SpecialAbilityBites;
        public static SpecialAbility[] SpecialAbilityAnimalStandard;
        public static SpecialAbility[] SpecialAbilityBitingAnimal;
        public static SpecialAbility[] SpecialAbilityClawed;
        public static SpecialAbility[] SpecialAbilityTailed;
        public static SpecialAbility[] SpecialAbilityClawedAndTailed;
        public static SpecialAbility[] SpecialAbilityInsectoid;
        public static SpecialAbility[] SpecialAbilityMagicalInsectoid;
        public static SpecialAbility[] SpecialAbilityStickySkin;
        public static SpecialAbility[] SpecialAbilityBitingStickySkin;
        public static SpecialAbility[] SpecialAbilityTailedAndStickySkin;
        public static SpecialAbility[] SpecialAbilityBitingTailed;
        public static SpecialAbility[] SpecialAbilityBitingClawedAndTailed;
        public static SpecialAbility[] SpecialAbilityClawedAndNecromantic;
        public static SpecialAbility[] SpecialAbilityClawedTailedAndMagical1;
        public static SpecialAbility[] SpecialAbilityClawedTailedAndMagical2;
        public static SpecialAbility[] SpecialAbilityBaneDragon;
        public static SpecialAbility[] SpecialAbilityDreadSpider;
        public static SpecialAbility[] SpecialAbilityFireBeetle;
        public static SpecialAbility[] SpecialAbilityImp;
        public static SpecialAbility[] SpecialAbilityTsukiWolf;
        public static SpecialAbility[] SpecialAbilitySabreTri;
        #endregion

        #region AreaEffect Defs
        public static AreaEffect[] AreaEffects;

        public static AreaEffect[] AreaEffectNone;
        public static AreaEffect[] AreaEffectExplosiveGoo;
        public static AreaEffect[] AreaEffectEarthen;
        public static AreaEffect[] AreaEffectDisease;
        public static AreaEffect[] AreaEffectArea1;
        public static AreaEffect[] AreaEffectArea2;
        public static AreaEffect[] AreaEffectArea3;
        #endregion

        #region Weapon Ability Defs
        public static WeaponAbility[] WeaponAbilities =
        {
            WeaponAbility.NerveStrike,
            WeaponAbility.WhirlwindAttack,
            WeaponAbility.LightningArrow,
            WeaponAbility.InfusedThrow,
            WeaponAbility.MysticArc,
            WeaponAbility.TalonStrike,
            WeaponAbility.PsychicAttack,
            WeaponAbility.ArmorIgnore,
            WeaponAbility.ArmorPierce,
            WeaponAbility.Bladeweave,
            WeaponAbility.BleedAttack,
            WeaponAbility.Block,
            WeaponAbility.ConcussionBlow,
            WeaponAbility.CrushingBlow,
            WeaponAbility.Disarm,
            WeaponAbility.Dismount,
            WeaponAbility.DoubleStrike,
            WeaponAbility.DualWield,
            WeaponAbility.Feint,
            WeaponAbility.ForceOfNature,
            WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike,
            WeaponAbility.ParalyzingBlow,
            WeaponAbility.ColdWind,
        };

        public static WeaponAbility[] WepAbilityNone = { };

        public static WeaponAbility[] WepAbility1 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ConcussionBlow,
            WeaponAbility.CrushingBlow, WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility2 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ColdWind, WeaponAbility.ConcussionBlow,
            WeaponAbility.CrushingBlow, WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility3 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.ConcussionBlow, WeaponAbility.CrushingBlow, 
            WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind, WeaponAbility.MortalStrike,
            WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility4 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ConcussionBlow, 
            WeaponAbility.CrushingBlow, WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.MortalStrike,
            WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility5 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.ColdWind, WeaponAbility.ConcussionBlow, 
            WeaponAbility.CrushingBlow, WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility6 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ColdWind, 
            WeaponAbility.ConcussionBlow, WeaponAbility.CrushingBlow, WeaponAbility.Disarm, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility7 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ColdWind,
            WeaponAbility.ConcussionBlow, WeaponAbility.CrushingBlow, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility8 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ColdWind,
            WeaponAbility.ConcussionBlow, WeaponAbility.CrushingBlow, WeaponAbility.Feint, WeaponAbility.ForceOfNature, WeaponAbility.FrenziedWhirlwind,
            WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility9 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.Block,
            WeaponAbility.ConcussionBlow, WeaponAbility.CrushingBlow, WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, 
            WeaponAbility.FrenziedWhirlwind, WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.ParalyzingBlow, WeaponAbility.PsychicAttack, 
            WeaponAbility.TalonStrike
        };

        public static WeaponAbility[] WepAbility10 =
        {
            WeaponAbility.ArmorIgnore, WeaponAbility.ArmorPierce, WeaponAbility.Bladeweave, WeaponAbility.BleedAttack, WeaponAbility.ColdWind,
            WeaponAbility.ConcussionBlow, WeaponAbility.CrushingBlow, WeaponAbility.Dismount, WeaponAbility.Feint, WeaponAbility.ForceOfNature, 
            WeaponAbility.FrenziedWhirlwind, WeaponAbility.MortalStrike, WeaponAbility.NerveStrike, WeaponAbility.PsychicAttack, WeaponAbility.TalonStrike
        };
        #endregion

        #region Training Points Configuration

        #region Table Config
        public static void LoadDefinitions()
        {
            Abilities = new SpecialAbility[]
            {
                SpecialAbility.RuneCorruption,
                SpecialAbility.GraspingClaw,
                SpecialAbility.RagingBreath,
                SpecialAbility.ConductiveBlast,
                SpecialAbility.LightningForce,
                SpecialAbility.StealLife,
                SpecialAbility.AngryFire,
                SpecialAbility.DragonBreath,
                SpecialAbility.Inferno,
                SpecialAbility.FlurryForce,
                SpecialAbility.ViciousBite,
                SpecialAbility.SearingWounds,
                SpecialAbility.LifeLeech,
                SpecialAbility.StickySkin,
                SpecialAbility.TailSwipe,
                SpecialAbility.VenomousBite,
                SpecialAbility.ManaDrain,
                SpecialAbility.Repel,
            };

            SpecialAbilityNone = new SpecialAbility[] { };

            SpecialAbilityMagical1 = new SpecialAbility[]
            { 
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath
            };

            SpecialAbilityMagical2 = new SpecialAbility[]
            { 
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.StealLife
            };

            SpecialAbilityMagical3 = new SpecialAbility[]
            { 
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath
            };

            SpecialAbilityMagical4 = new SpecialAbility[]
            { 
                SpecialAbility.AngryFire, SpecialAbility.DragonBreath, SpecialAbility.Inferno, SpecialAbility.RagingBreath
            };

            SpecialAbilityNecroMagical = new SpecialAbility[]
            { 
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.LifeLeech
            };

            SpecialAbilityBites = new SpecialAbility[]
            {
                SpecialAbility.VenomousBite, SpecialAbility.ViciousBite
            };

            SpecialAbilityAnimalStandard = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds
            };

            SpecialAbilityBitingAnimal = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.VenomousBite, SpecialAbility.ViciousBite
            };

            SpecialAbilityClawed = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw
            };

            SpecialAbilityTailed = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.TailSwipe
            };

            SpecialAbilityClawedAndTailed = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, SpecialAbility.TailSwipe
            };

            SpecialAbilityInsectoid = new SpecialAbility[]
            {
                 SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.RuneCorruption
            };

            SpecialAbilityMagicalInsectoid = new SpecialAbility[]
            {
                 SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.RuneCorruption,
                 SpecialAbility.AngryFire, SpecialAbility.DragonBreath, SpecialAbility.Inferno, SpecialAbility.RagingBreath
            };

            SpecialAbilityStickySkin = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds,
                SpecialAbility.StickySkin
            };

            SpecialAbilityBitingStickySkin = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds,
                SpecialAbility.StickySkin, SpecialAbility.VenomousBite, SpecialAbility.ViciousBite
            };

            SpecialAbilityTailedAndStickySkin = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.TailSwipe,
                SpecialAbility.StickySkin, SpecialAbility.VenomousBite, SpecialAbility.ViciousBite
            };

            SpecialAbilityBitingTailed = new SpecialAbility[]
            {
                SpecialAbility.VenomousBite, SpecialAbility.ViciousBite, SpecialAbility.TailSwipe
            };

            SpecialAbilityBitingClawedAndTailed = new SpecialAbility[]
            {
                SpecialAbility.VenomousBite, SpecialAbility.ViciousBite,
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, 
                SpecialAbility.TailSwipe
            };

            SpecialAbilityClawedAndNecromantic = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw,
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.LifeLeech
            };

            SpecialAbilityClawedTailedAndMagical1 = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, SpecialAbility.TailSwipe,
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath
            };

            SpecialAbilityClawedTailedAndMagical2 = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, SpecialAbility.TailSwipe,
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.StealLife
            };

            SpecialAbilityBaneDragon = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, SpecialAbility.TailSwipe,
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.VenomousBite, SpecialAbility.ViciousBite
            };

            SpecialAbilityDreadSpider = new SpecialAbility[]
            {
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.LifeLeech,
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.VenomousBite, SpecialAbility.ViciousBite
            };

            SpecialAbilityFireBeetle = new SpecialAbility[]
            {
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.RuneCorruption,
                SpecialAbility.AngryFire, SpecialAbility.DragonBreath, SpecialAbility.Inferno, SpecialAbility.RagingBreath
            };

            SpecialAbilityImp = new SpecialAbility[]
            {
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.LifeLeech,
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, SpecialAbility.TailSwipe,
                SpecialAbility.ViciousBite
            };

            SpecialAbilityTsukiWolf = new SpecialAbility[]
            {
                SpecialAbility.AngryFire, SpecialAbility.ConductiveBlast, SpecialAbility.DragonBreath, SpecialAbility.Inferno,
                SpecialAbility.LightningForce, SpecialAbility.RagingBreath, SpecialAbility.LifeLeech,
                SpecialAbility.ManaDrain, SpecialAbility.Repel, SpecialAbility.SearingWounds, SpecialAbility.GraspingClaw, SpecialAbility.TailSwipe
            };

            SpecialAbilitySabreTri = new SpecialAbility[]
            { 
                SpecialAbility.SearingWounds, SpecialAbility.TailSwipe
            };

            AreaEffects = new AreaEffect[]
            {
                AreaEffect.AuraOfEnergy,
                AreaEffect.Firestorm,
                AreaEffect.ExplosiveGoo,
                AreaEffect.EssenceOfEarth,
                AreaEffect.AuraOfNausea,
                AreaEffect.EssenceOfDisease,
            };

            AreaEffectNone = new AreaEffect[] { };

            AreaEffectExplosiveGoo = new AreaEffect[]
            {
                AreaEffect.ExplosiveGoo
            };

            AreaEffectEarthen = new AreaEffect[]
            {
                AreaEffect.EssenceOfEarth, AreaEffect.ExplosiveGoo
            };

            AreaEffectDisease = new AreaEffect[]
            {
                AreaEffect.AuraOfNausea, AreaEffect.EssenceOfDisease
            };

            AreaEffectArea1 = new AreaEffect[]
            {
                AreaEffect.EssenceOfEarth, AreaEffect.ExplosiveGoo, AreaEffect.AuraOfEnergy
            };

            AreaEffectArea2 = new AreaEffect[]
            {
                AreaEffect.EssenceOfEarth, AreaEffect.ExplosiveGoo, AreaEffect.AuraOfEnergy, 
                AreaEffect.AuraOfNausea, AreaEffect.EssenceOfDisease,
                AreaEffect.PoisonBreath
            };

            AreaEffectArea3 = new AreaEffect[]
            {
                AreaEffect.AuraOfNausea, AreaEffect.EssenceOfDisease, AreaEffect.PoisonBreath, 
            };
        }
        #endregion

        public static void Configure()
        {
            LoadDefinitions();

            _TrainingPoints = new List<TrainingPoint>();

            _TrainingPoints.Add(new TrainingPoint(PetStat.Str, 3.0, 1, 700, 1061146, 1157507));
            _TrainingPoints.Add(new TrainingPoint(PetStat.Dex, 0.1, 1, 150, 1061147, 1157508));
            _TrainingPoints.Add(new TrainingPoint(PetStat.Int, 0.5, 1, 700, 1061148, 1157509));

            _TrainingPoints.Add(new TrainingPoint(PetStat.Hits, 3.0, 1, 1100, 1061149, 1157510));
            _TrainingPoints.Add(new TrainingPoint(PetStat.Stam, 0.5, 1, 150, 1061150, 1157511));
            _TrainingPoints.Add(new TrainingPoint(PetStat.Mana, 0.5, 1, 1500, 1061151, 1157512));

            _TrainingPoints.Add(new TrainingPoint(PetStat.RegenHits, 18.0, 1, 20, 1075627, 1157513));
            _TrainingPoints.Add(new TrainingPoint(PetStat.RegenStam, 12.0, 1, 30, 1079410, 1157514));
            _TrainingPoints.Add(new TrainingPoint(PetStat.RegenMana, 12.0, 1, 30, 1079411, 1157515));

            // TODO: Damage Per Second 1157516

            _TrainingPoints.Add(new TrainingPoint(ResistanceType.Physical, 3.0, 1, 80, 1061158, 1157517));
            _TrainingPoints.Add(new TrainingPoint(ResistanceType.Fire, 3.0, 1, 80, 1061159, 1157518));
            _TrainingPoints.Add(new TrainingPoint(ResistanceType.Cold, 3.0, 1, 80, 1061160, 1157519));
            _TrainingPoints.Add(new TrainingPoint(ResistanceType.Poison, 3.0, 1, 80, 1061161, 1157520));
            _TrainingPoints.Add(new TrainingPoint(ResistanceType.Energy, 3.0, 1, 80, 1061162, 1157521));

            _TrainingPoints.Add(new TrainingPoint(SkillName.Magery, 0.5, 5, 20, 1002106, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.EvalInt, 1.0, 5, 20, 1044076, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Necromancy, 0.5, 5, 20, 1044109, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.SpiritSpeak, 1.0, 5, 20, 1044092, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Chivalry, 0.5, 5, 20, 1044111, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Focus, 0.1, 5, 20, 1044110, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Bushido, 0.5, 5, 20, 1044112, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Ninjitsu, 0.5, 5, 20, 1044113, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Spellweaving, 0.5, 5, 20, 1044114, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Mysticism, 0.5, 5, 20, 1044115, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Meditation, 0.1, 5, 20, 1044106, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.MagicResist, 0.1, 5, 20, 1044086, 1157522));

            _TrainingPoints.Add(new TrainingPoint(SkillName.Wrestling, 1.0, 5, 20, 1044103, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Tactics, 1.0, 5, 20, 1044087, 1157522));
            _TrainingPoints.Add(new TrainingPoint(SkillName.Anatomy, 0.1, 5, 20, 1044061, 1157522));

            TextDefinition[][] loc = _MagicalAbilityLocalizations;

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Piercing, 1.0, 1, 1, loc[0][0], loc[0][1],
                new TrainingPointRequirement(WeaponAbility.ArmorIgnore, 100, 1028838),
                new TrainingPointRequirement(WeaponAbility.ParalyzingBlow, 100, 1028848),
                new TrainingPointRequirement(WeaponAbility.BleedAttack, 100, 1028839)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Bashing, 1.0, 1, 1, loc[1][0], loc[1][1],
                new TrainingPointRequirement(WeaponAbility.MortalStrike, 100, 1028846),
                new TrainingPointRequirement(WeaponAbility.ConcussionBlow, 100, 1028840),
                new TrainingPointRequirement(WeaponAbility.Disarm, 100, 1028842)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Slashing, 1.0, 1, 1, loc[2][0], loc[2][1],
                new TrainingPointRequirement(SkillName.Bushido, 500, 1044112),
                new TrainingPointRequirement(WeaponAbility.ArmorIgnore, 100, 1028838),
                new TrainingPointRequirement(WeaponAbility.Disarm, 100, 1028842),
                new TrainingPointRequirement(WeaponAbility.NerveStrike, 100, 1028855)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.BattleDefense, 1.0, 1, 1, loc[3][0], loc[3][1],
                new TrainingPointRequirement(WeaponAbility.Disarm, 100, 1028842),
                new TrainingPointRequirement(WeaponAbility.ParalyzingBlow, 100), 1028848));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.WrestlingMastery, 1.0, 1, 1, loc[4][0], loc[4][1],
                new TrainingPointRequirement(WeaponAbility.Disarm, 100, 1028842),
                new TrainingPointRequirement(WeaponAbility.ParalyzingBlow, 100, 1028848)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Poisoning, 1.0, 1, 1, loc[5][0], loc[5][1],
                new TrainingPointRequirement(SkillName.Chivalry, 100, 1044111)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Bushido, 1.0, 1, 1, loc[6][0], loc[6][1],
                new TrainingPointRequirement(SkillName.Bushido, 500, 1044112)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Ninjitsu, 1.0, 1, 1, loc[7][0], loc[7][1],
                new TrainingPointRequirement(SkillName.Ninjitsu, 500, 1044113)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Discordance, 1.0, 1, 1, loc[8][0], loc[8][1],
                new TrainingPointRequirement(SkillName.Discordance, 500, 1044075)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.MageryMastery, 1.0, 1, 1, loc[9][0], loc[9][1],
                new TrainingPointRequirement(SkillName.Magery, 100, 1044085),
                new TrainingPointRequirement(SkillName.EvalInt, 100, 1044076)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Mysticism, 1.0, 1, 1, loc[10][0], loc[10][1],
                new TrainingPointRequirement(SkillName.Mysticism, 500, 1044115)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Spellweaving, 1.0, 1, 1, loc[11][0], loc[11][1],
                new TrainingPointRequirement(SkillName.Spellweaving, 50, 1044114)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Chivalry, 1.0, 1, 1, loc[12][0], loc[12][1],
                new TrainingPointRequirement(SkillName.Chivalry, 500, 1044111)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Necromage, 1.0, 1, 1, loc[13][0], loc[13][1],
                new TrainingPointRequirement(SkillName.Magery, 100, 1044085),
                new TrainingPointRequirement(SkillName.Necromancy, 100, 1044109),
                new TrainingPointRequirement(SkillName.SpiritSpeak, 100, 1044092),
                new TrainingPointRequirement(SkillName.EvalInt, 100, 1044076)));

            _TrainingPoints.Add(new TrainingPoint(MagicalAbility.Necromancy, 1.0, 1, 1, loc[14][0], loc[14][1],
                new TrainingPointRequirement(SkillName.Necromancy, 100, 1044109),
                new TrainingPointRequirement(SkillName.SpiritSpeak, 100, 1044092)));

            loc = _SpecialAbilityLocalizations;
            int index = 0;

            foreach (var abil in Abilities)
            {
                _TrainingPoints.Add(new TrainingPoint(abil, 1.0, 100, 100, loc[index][0], loc[index][1]));
                index++;
            }

            loc = _AreaEffectLocalizations;
            index = 0;

            foreach (var effect in AreaEffects)
            {
                _TrainingPoints.Add(new TrainingPoint(effect, 1.0, 100, 100, loc[index][0], loc[index][1]));
                index++;
            }

            loc = _WeaponAbilityLocalizations;
            index = 0;

            foreach (var ability in WeaponAbilities)
            {
                TrainingPointRequirement requirement = null;

                if(ability == WeaponAbility.NerveStrike)
                    requirement = new TrainingPointRequirement(SkillName.Bushido, 500, 1044112);
                else if (ability == WeaponAbility.TalonStrike)
                    requirement = new TrainingPointRequirement(SkillName.Ninjitsu, 500, 1044113);
                else if (ability == WeaponAbility.Feint)
                    requirement = new TrainingPointRequirement(SkillName.Bushido, 500, 1044112);
                else if (ability == WeaponAbility.FrenziedWhirlwind)
                    requirement = new TrainingPointRequirement(SkillName.Ninjitsu, 500, 1044113);
                else if (ability == WeaponAbility.Bladeweave)
                    requirement = new TrainingPointRequirement(SkillName.Bushido, 500, 1044112);

                _TrainingPoints.Add(new TrainingPoint(ability, 1.0, 100, 100, loc[index][0], loc[index][1], requirement));
                index++;
            }
        }
        #endregion

        #region Training Helpers
        public static int GetTrainingCapTotal(PetStat stat)
        {
            switch (stat)
            {
                case PetStat.Str:
                case PetStat.Int:
                case PetStat.Dex: return 2300;
                case PetStat.Hits:
                case PetStat.Stam:
                case PetStat.Mana: return 3300;
            }

            return 0;
        }

        public static int GetTrainingCapTotal(ResistanceType resist)
        {
            return 1095;
        }

        public static int GetTotalStatWeight(BaseCreature bc)
        {
            var str = GetTrainingPoint(PetStat.Str);
            var dex = GetTrainingPoint(PetStat.Dex);
            var intel = GetTrainingPoint(PetStat.Int);

            return (int)(((double)bc.RawStr * str.Weight) + 
                ((double)bc.RawDex * dex.Weight) + 
                ((double)bc.RawInt * intel.Weight));
        }

        public static int GetTotalAttributeWeight(BaseCreature bc)
        {
            var hits = GetTrainingPoint(PetStat.Hits);
            var stam = GetTrainingPoint(PetStat.Stam);
            var mana = GetTrainingPoint(PetStat.Mana);

            return (int)(((double)bc.HitsMax * hits.Weight) + 
                ((double)bc.StamMax * stam.Weight) + 
                ((double)bc.ManaMax * mana.Weight));
        }

        public static int GetTotalResistWeight(BaseCreature bc)
        {
            var phys = GetTrainingPoint(ResistanceType.Physical);
            var fire = GetTrainingPoint(ResistanceType.Fire);
            var cold = GetTrainingPoint(ResistanceType.Cold);
            var pois = GetTrainingPoint(ResistanceType.Poison);
            var nrgy = GetTrainingPoint(ResistanceType.Energy);

            return (int)(((double)bc.PhysicalResistanceSeed * phys.Weight) +
                   ((double)bc.FireResistSeed * fire.Weight) +
                   ((double)bc.ColdResistSeed * cold.Weight) +
                   ((double)bc.PoisonResistSeed * pois.Weight) +
                   ((double)bc.EnergyResistSeed * nrgy.Weight));
        }

        public static bool ApplyTrainingPoint(BaseCreature bc, TrainingPoint trainingPoint, int value)
        {
            var profile = GetAbilityProfile(bc, true);

            if (trainingPoint.TrainPoint is PetStat)
            {
                PetStat stat = (PetStat)trainingPoint.TrainPoint;

                switch (stat)
                {
                    case PetStat.Str: bc.SetStr(value); break;
                    case PetStat.Dex: bc.SetDex(value); break;
                    case PetStat.Int: bc.SetInt(value); break;
                    case PetStat.Hits: bc.SetHits(value); break;
                    case PetStat.Stam: bc.SetStam(value); break;
                    case PetStat.Mana: bc.SetMana(value); break;
                    case PetStat.RegenHits: profile.RegenHits = value; break;
                    case PetStat.RegenStam: profile.RegenStam = value; break;
                    case PetStat.RegenMana: profile.RegenMana = value; break;
                    //TODO: Damage Per Second
                }

                return true;
            }

            else if (trainingPoint.TrainPoint is MagicalAbility)
            {
                MagicalAbility ability = (MagicalAbility)trainingPoint.TrainPoint;

                if (ValidateTrainingPoint(bc, ability) && profile.AddAbility(ability))
                {
                    return true;
                }
            }

            else if (trainingPoint.TrainPoint is SpecialAbility)
            {
                SpecialAbility ability = trainingPoint.TrainPoint as SpecialAbility;

                if (ValidateTrainingPoint(bc, ability) && profile.AddAbility(ability))
                {
                    return true;
                }
            }

            else if (trainingPoint.TrainPoint is AreaEffect)
            {
                AreaEffect ability = trainingPoint.TrainPoint as AreaEffect;

                if (ValidateTrainingPoint(bc, ability) && profile.AddAbility(ability))
                {
                    return true;
                }
            }

            else if (trainingPoint.TrainPoint is WeaponAbility)
            {
                WeaponAbility ability = trainingPoint.TrainPoint as WeaponAbility;

                if (ValidateTrainingPoint(bc, ability) && profile.AddAbility(ability))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ValidateTrainingPoint(BaseCreature bc, MagicalAbility ability)
        {
            var def = GetTrainingDefinition(bc);

            if (def == null)
                return false;

            return (def.MagicalAbilities & ability) != 0;
        }

        public static bool ValidateTrainingPoint(BaseCreature bc, SpecialAbility ability)
        {
            var def = GetTrainingDefinition(bc);

            if (def == null)
                return false;

            return def.SpecialAbilities.FirstOrDefault(a => a == ability) != null;
        }

        public static bool ValidateTrainingPoint(BaseCreature bc, AreaEffect ability)
        {
            var def = GetTrainingDefinition(bc);

            if (def == null)
                return false;

            return def.AreaEffects.FirstOrDefault(a => a == ability) != null;
        }

        public static bool ValidateTrainingPoint(BaseCreature bc, WeaponAbility ability)
        {
            var def = GetTrainingDefinition(bc);

            if (def == null)
                return false;

            return def.WeaponAbilities.FirstOrDefault(a => a == ability) != null;
        }
        #endregion

        #region Skill Categories
        public static SkillName[] MagerySkills =
        {
            SkillName.Magery,
            SkillName.EvalInt,
            SkillName.Necromancy,
            SkillName.SpiritSpeak,
            SkillName.Chivalry,
            SkillName.Focus,
            SkillName.Bushido,
            SkillName.Ninjitsu,
            SkillName.Spellweaving,
            SkillName.Mysticism,
            SkillName.Meditation,
            SkillName.MagicResist
        };

        public static SkillName[] CombatSkills =
        {
            SkillName.Wrestling,
            SkillName.Tactics,
            SkillName.Anatomy
        };
        #endregion

        #region Localizations
        public static TextDefinition[][] MagicalAbilityLocalizations { get { return _MagicalAbilityLocalizations; } }
        private static TextDefinition[][] _MagicalAbilityLocalizations =
        {
            new TextDefinition[] { 1157559, 1157392 }, // piercing
            new TextDefinition[] { 1157560, 1157471 }, // bashing
            new TextDefinition[] { 1157561, 1157396 }, // slashing
            new TextDefinition[] { 1157562, 1157395 }, // battle defense
            new TextDefinition[] { 1155784, 1157397 }, // wrestling mastery
            new TextDefinition[] { 1002122, 1157389 }, // poisoning
            new TextDefinition[] { 1044112, 1157383 }, // bushido
            new TextDefinition[] { 1044113, 1157388 }, // ninjitsu
            new TextDefinition[] { 1044075, 1157385 }, // discordance
            new TextDefinition[] { 1155771, 1157400 }, // magery mastery
            new TextDefinition[] { 1044115, 1157386 }, // mysticism
            new TextDefinition[] { 1044114, 1157390 }, // spellweaving
            new TextDefinition[] { 1044111, 1157384 }, // chivalry - must have pos karma
            new TextDefinition[] { 1157473, 1157474 }, // necromage
            new TextDefinition[] { 1044109, 1157387 }, // necromancy - must have neg karma
            new TextDefinition[] { 1002106, 1157391 }  // magery
        };

        public static TextDefinition[][] SpecialAbilityLocalizations { get { return _SpecialAbilityLocalizations; } }
        private static TextDefinition[][] _SpecialAbilityLocalizations =
        {
            new TextDefinition[] { 1157398, 1157399 }, // Rune Corruption
            new TextDefinition[] { 1157400, 1157401 }, // Grasping Claw
            new TextDefinition[] { 1157404, 1157405 }, // Raging Breath
            new TextDefinition[] { 1157406, 1157407 }, // Conductive Blast
            new TextDefinition[] { 1157408, 1157409 }, // Lightning Force
            new TextDefinition[] { 1157410, 1157411 }, // Steal Life
            new TextDefinition[] { 1157412, 1157413 }, // Angry Fire
            new TextDefinition[] { 1157414, 1157415 }, // Dragon Breath
            new TextDefinition[] { 1157416, 1157417 }, // Inferno
            new TextDefinition[] { 1157418, 1157419 }, // Flurry Force
            new TextDefinition[] { 1157420, 1157421 }, // Vicious Bite
            new TextDefinition[] { 1157422, 1157423 }, // Searing Wounds
            new TextDefinition[] { 1157424, 1157425 }, // Life Leech
            new TextDefinition[] { 1157426, 1157427 }, // Sticky Skin
            new TextDefinition[] { 1157428, 1157429 }, // Tail Swipe
            new TextDefinition[] { 1157430, 1157431 }, // Venomous Bite
            new TextDefinition[] { 1157432, 1157433 }, // Mana Drain
            new TextDefinition[] { 1157434, 1157435 }, // Repel
        };

        public static TextDefinition[][] AreaEffectLocalizations { get { return _AreaEffectLocalizations; } }
        private static TextDefinition[][] _AreaEffectLocalizations =
        {
            new TextDefinition[] { 1157459, 1157460 }, // Aura of Energy
            new TextDefinition[] { 1157461, 1157462 }, // Firestorm
            new TextDefinition[] { 1157463, 1157464 }, // Explosive Goo
            new TextDefinition[] { 1157465, 1157466 }, // Essence of Earth
            new TextDefinition[] { 1157467, 1157468 }, // Aura of Nausea
            new TextDefinition[] { 1157469, 1157470 }, // Essence of Disease
        };

        public static TextDefinition[][] WeaponAbilityLocalizations { get { return _WeaponAbilityLocalizations; } }
        private static TextDefinition[][] _WeaponAbilityLocalizations =
        {
            new TextDefinition[] { 1028855, 1157436 }, // Nerve Strike
            new TextDefinition[] { 1028850, 1157437 }, // Whirlwind Attack
            new TextDefinition[] { 1028863, 1157438 }, // Lightning Arrow
            new TextDefinition[] { 1113283, 1157439 }, // Infused Throw
            new TextDefinition[] { 1113282, 1157440 }, // Mystic Arc
            new TextDefinition[] { 1028856, 1157441 }, // Talon Strike
            new TextDefinition[] { 1028864, 1157442 }, // Psychic Attack
            new TextDefinition[] { 1028838, 1157443 }, // Armor Ignore
            new TextDefinition[] { 1028860, 1157444 }, // Armor Pierce
            new TextDefinition[] { 1028861, 1157445 }, // Bladeweave
            new TextDefinition[] { 1028839, 1157446 }, // Bleed Attack
            new TextDefinition[] { 1028853, 1157447 }, // Block
            new TextDefinition[] { 1028840, 1157448 }, // Concussion Blow
            new TextDefinition[] { 1028841, 1157449 }, // Crushing Blow
            new TextDefinition[] { 1028842, 1157450 }, // Disarm
            new TextDefinition[] { 1028843, 1157451 }, // Dismount
            new TextDefinition[] { 1028844, 1157452 }, // Double Strike
            new TextDefinition[] { 1028858, 1157453 }, // Dual Wield
            new TextDefinition[] { 1028857, 1157454 }, // Feint
            new TextDefinition[] { 1028866, 1157455 }, // Force of Nature
            new TextDefinition[] { 1028852, 1157456 }, // Frenzied Whirlwind
            new TextDefinition[] { 1028846, 1157457 }, // Mortal Strike
            new TextDefinition[] { 1028848, 1157458 }, // Paralyzing Blow
            new TextDefinition[] { 1157402, 1157403 }, // Cold Wind
        };

        public static int GetLocalization(BaseCreature pet, SkillName sk)
        {
            return pet.Skills[sk].Info.Localization;
        }

        public static TextDefinition[] GetLocalization(object o)
        {
            if (o is MagicalAbility)
                return GetLocalization((MagicalAbility)o);

            if(o is SpecialAbility)
                return GetLocalization((SpecialAbility)o);

            if (o is AreaEffect)
                return GetLocalization((AreaEffect)o);

            if(o is WeaponAbility)
                return GetLocalization((WeaponAbility)o);

            return new TextDefinition[] { 0, 0 };
        }

        public static TextDefinition[] GetLocalization(MagicalAbility ability)
        {
            switch (ability)
            {
                case MagicalAbility.Piercing: return _MagicalAbilityLocalizations[0];
                case MagicalAbility.Bashing: return _MagicalAbilityLocalizations[1];
                case MagicalAbility.Slashing: return _MagicalAbilityLocalizations[2];
                case MagicalAbility.BattleDefense: return _MagicalAbilityLocalizations[3];
                case MagicalAbility.WrestlingMastery: return _MagicalAbilityLocalizations[4];
                case MagicalAbility.Poisoning: return _MagicalAbilityLocalizations[5];
                case MagicalAbility.Bushido: return _MagicalAbilityLocalizations[6];
                case MagicalAbility.Ninjitsu: return _MagicalAbilityLocalizations[7];
                case MagicalAbility.Discordance: return _MagicalAbilityLocalizations[8];
                case MagicalAbility.MageryMastery: return _MagicalAbilityLocalizations[9];
                case MagicalAbility.Mysticism: return _MagicalAbilityLocalizations[10];
                case MagicalAbility.Spellweaving: return _MagicalAbilityLocalizations[11];
                case MagicalAbility.Chivalry: return _MagicalAbilityLocalizations[12];
                case MagicalAbility.Necromage: return _MagicalAbilityLocalizations[13];
                case MagicalAbility.Necromancy: return _MagicalAbilityLocalizations[14];
                case MagicalAbility.Magery: return _MagicalAbilityLocalizations[15];
            }

            return null;
        }

        public static TextDefinition[] GetLocalization(SpecialAbility ability)
        {
            int index = Array.IndexOf(Abilities, ability);

            if (index < 0 || index >= Abilities.Length)
                return null;

            return _SpecialAbilityLocalizations[index];
        }

        public static TextDefinition[] GetLocalization(WeaponAbility effect)
        {
            int index = Array.IndexOf(WeaponAbilities, effect);

            if (index < 0 && index >= AreaEffects.Length)
                return null;

            return _WeaponAbilityLocalizations[index];
        }

        public static TextDefinition[] GetLocalization(AreaEffect effect)
        {
            int index = Array.IndexOf(AreaEffects, effect);

            if (index < 0 && index >= AreaEffects.Length)
                return null;

            return _AreaEffectLocalizations[index];
        }

        public static int GetCategoryLocalization(object o)
        {
            if (o is MagicalAbility)
            {
                return 1157481;
            }

            if (o is SpecialAbility)
            {
                return 1157480;
            }

            if (o is AreaEffect)
            {
                return 1157482;
            }

            if (o is WeaponAbility)
            {
                return 1157479;
            }

            return 0;
        }
        #endregion
    }
}
