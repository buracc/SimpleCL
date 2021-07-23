using System.Collections.Immutable;

namespace SimpleCL.Enums.Skills
{
    public class SkillData
    {
        public enum SkillParam : uint
        {
            Attack = 6386804,
            HpMpRecovery = 1751474540,
            Resurrection = 1919251317,
            DamageAbsorb = 1868849522,
            ActiveMpConsumed = 1869506150,
            RequiredItem = 1919250793,
            AreaEffect = 6645362,
            OverTime = 1886743667,
            HealWeaponReflect = 1836542056,
            Timed = 1685418593,
            AutoTransferEffect = 1701213281,
            LinkedSkill = 1819175795,
            DamageDivide = 1818977380,
            MultiHit = 28003,
            PhyMagDefenseIncrease = 1684366960,
            BlockingRatio = 25202,
            KnockDown = 27503,
            DownAttack = 25697,
            Stun = 29556,
            KnockBack = 27490,
            HpIncrease = 6844521,
            CritIncrease = 25458,
            HitRatio = 26738,
            HelperSummon = 1937075565,
            ShotRangeIncrease = 29301,
            Freeze = 26234,
            Frostbite = 26210,
            ProtectionWall = 28791,
            ElectricShock = 25971,
            DamagePercentIncrease = 6582901,
            MovementSpeed = 1752396901,
            BlinkDash = 1952803941,
            WizardTeleport = 1952803890,
            ParryRatio = 25970,
            Burn = 25205,
            CurseReduce = 1650946657,
            CurseProbabilityReduce = 1919246700,
            CurseRemove = 1668641396, // Socket stones do not have this one
            CurseRemove2 = 1668641388, // Socket stones also have this one
            BadStatusRemoveAmount = 1919120754,
            PreventAttack = 1886350433,
            ReincarnationIncrease = 1769105251,
            ConditionalCast = 1919250787,
            GetVariable = 1734702198,
            ConsumeItem = 1668182893,
            Debuff = 1952542324,
            Unk1 = 1296122196,
            Unk2 = 1650619750,
            Unk3 = 1851946342,
            ChParryDebuff = 1952805476,
            ChHitRatioDebuff = 1953002084,
            IncreaseMp = 7172201,
            NonPlayerBuff = 1667396966, // Items, Npcs, Events
            AlchemyRatioIncrease = 1634493301,
            StrIncrease = 1937011305,
            IntIncrease = 1768846441,
            MonsterCapture = 1902474100,
            Trap = 1953653104,
            GoldDropRatioIncrease = 6775922,
            Detect = 6583412,
            LuckIncrease = 1819632491,
            SkinChange = 1836278632,
            WarriorOneHand = 1160860481,
            WarriorTwoHand = 1160926017,
            WarriorAxe = 1160921416,
            WarriorAxe2 = 1160921409,
            AggroIncrease = 1953395762,
            WarriorWeaponPhyReflect = 1886876788,
            ReduceSelfPhyMagDmg = 1886217319,
            AoeDetect = 1685353584,
            TransferDamageAbsorb = 1818977394,
            AggroAbsorb = 1818976615,
            EuPassiveSkill = 1936028790,
            EuPassiveSkill2 = 1886613351,
            CritEvasion = 1684238953,
            Dull = 29548,
            DebuffedDamageIncrease = 1635017569,
            DamageReturn = 1684891506,
            IncreaseAttack = 1634754933,
            CurseSeriesReduce = 1919246708,
            Bleed = 25196,
            CrossbowDamage = 1128415572,
            CrossbowRangeIncrease = 1128419905,
            DaggerDamage = 1145520468,
            DaggerHitRate = 1145522258,
            DaggerStealthDamage = 1145520449,
            RogueStealth = 1398031445,
            Disorient = 1751741549,
            Invisible = 1751737445,
            Stealth = 1398035280,
            Cancelable = 7564131,
            TagPoint = 1752069232,
            Poison = 28787,
            RoguePoison = 1380992085,
            RoguePoison2 = 1380996181,
            PoisonImbue = 1380991573,
            MonsterMask = 1835229552,
            ReduceSelfMaxHp = 1886218352,
            ReduceSelfPhyMagDef = 1886217328,
            WizardManaAttack = 1464421700,
            WizardRangedAttack = 1464422997,
            WizardEarthDamage = 1161904468,
            WizardColdDamage = 1129267540,
            WizardFireDamage = 1179205972,
            WizardLightDamage = 1279869268,
            Root = 29300,
            Overlap = 1870031922,
            Combustion = 1668509040,
            Fear = 26213,
            WarlockDotDamage = 1146372436,
            WarlockZerkIncrease = 1146373202,
            ZerkObtainIncrease = 1752656242,
            Connection = 1818981170,
            WarlockManaUsageDecrease = 1380535620,
            Decay = 1668509796,
            Weaken = 1668509028,
            Impotent = 1668510578,
            Division = 1668508020,
            WarlockNukeDamage = 1112293716,
            WarlockTrapDamageIncrease = 1414676801,
            Hidden = 29794,
            WarlockTrap = 84545280,
            ShortSight = 28025,
            WarlockVampIncrease = 1112754256,
            WarlockTrueDamage = 1396785473,
            WarlockVamp = 1818653556,
            WarlockVampWeaponReflect = 1836542067,
            Disease = 25715,
            TrueDamage = 1885629799,
            ScreamMask = 1633840738,
            Sleep = 29541,
            Panic = 1668507760,
            MobAggroReduce = 1685352052,
            AggroWeaponReflect = 1836541044,
            Phantasma = 25325,
            Phantasma2 = 29982,
            BardDamage = 1297432916,
            BardRange = 1297433938,
            BardManaUsageDecrease = 1111772484,
            BardResistanceIncrease = 1297433426,
            BardMusic = 1935895667,
            Confusion = 25441,
            StealMp = 1684891508,
            ManaRecovery = 1836543336,
            DanceRangeIncrease = 1146307922,
            DanceResistanceIncrease = 1146307410,
            BardDance = 1919250540,
            HealingDance = 1919447669,
            ManaDance = 1684237680,
            ClericDamage = 1212957012,
            ClericHealIncrease = 1212961365,
            ClericManaUsageReduce = 1212960068,
            ClericPassive = 1919250798,
            AllyHeal = 1702062192,
            BlackResBuffApplication = 1919776116,
            BlackResHpMpReduce = 1667785586,
            BlackResBuffHpMpReduce = 1668113266,
            ClericStrIncrease = 1212958291,
            ClericIntIncrease = 1212960073,
            PhyDefenseIncrease = 1212957264,
            MagDefenseIncrease = 1212961613,
            TargetDash = 1952803891,
            Darkness = 25710,
            DamageMpAbsorption = 1684499824,
            DaggerSpeed = 1752396850,
            VipHonorBuffs = 1986164070,
            MovementIncrease = 1752396851,
            FortressRepair = 1919970164,
            StructureAoe = 1885629746,
            PetExpPotion = 1702391913,
            ManaSwitch = 1818977384,
            BicheonSkill = 1936745569,
            IgnoreDefense = 1836280164,
            SocketStoneSp = 7565417,
            SocketStoneReflect = 1651270770,
            SocketStoneStunRemove = 7236966,
            SocketStoneHeal = 7236968,
            SocketStoneFreeMp = 7172978,
            ZerkDurationIncrease = 1752656244,
            ExpSpIncrease = 1702391925,
            RamadanExpSpIncrease = 1886219632,
            JobSkill = 1785684595,
            JobPipe = 1668511085,
            TraderPouch = 1650683508,
            TransportHpIncrease = 1667788905,
            TransportSpeedIncrease = 1667789684,
            DemonBless = 1685287020,
            FellowSkill = 1885697074,
            FellowAttack = 6384748,
            FellowIncreaseAttack = 7365222,
            FellowZerk = 1752654190,
            FellowBuff = 7168614,
            FellowIncreaseSelfAttack = 1634759285,
            RedBlueFellowAttack = 1835295334,
            RedBlueFellowBuff = 7366768,
            FellowMpRecovery = 7171440,
            PartyZerkIncrease = 1752654965,
            AwesomeWorld = 2036556899,
            DanceClone = 1919968115, // Some type of dance skill clone, links to original dance skill id
            FlowerSkill = 7630179
        }

        // These have 3 params
        private readonly ImmutableList<SkillParam> _euClassPassiveSkills = ImmutableList.Create(
            SkillParam.CrossbowDamage,
            SkillParam.CrossbowRangeIncrease,
            SkillParam.DaggerDamage,
            SkillParam.DaggerHitRate,
            SkillParam.DaggerStealthDamage,
            SkillParam.RogueStealth,
            SkillParam.WizardEarthDamage,
            SkillParam.WizardColdDamage,
            SkillParam.WizardFireDamage,
            SkillParam.WizardLightDamage,
            SkillParam.WarlockDotDamage,
            SkillParam.WarlockNukeDamage,
            SkillParam.WarlockTrueDamage,
            SkillParam.BardDamage,
            SkillParam.BardRange,
            SkillParam.ClericDamage
        );

        // These have 2 params
        private readonly ImmutableList<SkillParam> _etcPassiveSkills = ImmutableList.Create(
            SkillParam.RoguePoison,
            SkillParam.RoguePoison2,
            SkillParam.PoisonImbue,
            SkillParam.WizardManaAttack,
            SkillParam.WizardRangedAttack,
            SkillParam.WarlockZerkIncrease,
            SkillParam.WarlockManaUsageDecrease,
            SkillParam.WarlockTrapDamageIncrease,
            SkillParam.WarlockVampIncrease,
            SkillParam.BardManaUsageDecrease,
            SkillParam.BardResistanceIncrease,
            SkillParam.ClericHealIncrease,
            SkillParam.ClericManaUsageReduce,
            SkillParam.ClericStrIncrease,
            SkillParam.ClericIntIncrease,
            SkillParam.PhyDefenseIncrease,
            SkillParam.MagDefenseIncrease
        );

        private int GetParamAmount(SkillParam param)
        {
            switch (param)
            {
                case SkillParam.Timed: // Duration
                case SkillParam.HelperSummon: // Duration
                case SkillParam.ChParryDebuff:
                case SkillParam.ChHitRatioDebuff:
                case SkillParam.AlchemyRatioIncrease: // % increase
                case SkillParam.GoldDropRatioIncrease: // Amount increase %
                case SkillParam.LuckIncrease: // % increase
                case SkillParam.WarriorWeaponPhyReflect: // Amount %
                case SkillParam.CritEvasion: // Amount
                case SkillParam.Overlap: // No clue, idk if this is even the correct name
                case SkillParam.ZerkObtainIncrease: // Increase %
                case SkillParam.WarlockVamp: // Amount
                case SkillParam.WarlockVampWeaponReflect: // Amount %
                case SkillParam.TrueDamage: // Damage
                case SkillParam.ScreamMask: // Distance
                case SkillParam.AggroWeaponReflect: // Amount %
                case SkillParam.Phantasma: // Unk
                case SkillParam.Phantasma2: // Unk
                case SkillParam.BardMusic: // Unk
                case SkillParam.ManaRecovery: // Weapon mag reflect %
                case SkillParam.DanceRangeIncrease: // Distance
                case SkillParam.DanceResistanceIncrease: // Amount
                case SkillParam.BardDance: // Always 1
                case SkillParam.BlackResBuffApplication: // Buff Id
                case SkillParam.BlackResHpMpReduce: // Amount %
                case SkillParam.DamageMpAbsorption: // Amount %
                case SkillParam.DaggerSpeed: // Movement speed %
                case SkillParam.MovementIncrease: // Movement speed %
                case SkillParam.StructureAoe: // Radius
                case SkillParam.IgnoreDefense: // Ignored %
                case SkillParam.SocketStoneSp: // Extra SP
                case SkillParam.ZerkDurationIncrease: // Increase %
                case SkillParam.JobSkill: // Level required
                case SkillParam.JobPipe: // Pipe level
                case SkillParam.TraderPouch: // Slots
                case SkillParam.TransportHpIncrease: // Increase %
                case SkillParam.TransportSpeedIncrease: // Increase %
                case SkillParam.DemonBless: // Unk (must be some percentage value)
                case SkillParam.FellowIncreaseSelfAttack: // Increase %
                case SkillParam.PartyZerkIncrease: // Increase %
                case SkillParam.DanceClone: // Original dance skill id
                case SkillParam.FlowerSkill: // 1
                case SkillParam.OverTime: // Time
                case SkillParam.HealWeaponReflect: // Reflect %
                    return 1;
                case SkillParam.StrIncrease: // Amount, Limit on current Str %
                case SkillParam.IntIncrease: // Amount, Limit on current Int %
                case SkillParam.IncreaseMp: // Amount, Amount %
                case SkillParam.Detect: // Type?, Level
                case SkillParam.SkinChange: // Type, Max level
                case SkillParam.AggroIncrease: // Taunt amount, Aggro amount
                case SkillParam.AoeDetect: // Unk1, Unk2
                case SkillParam.AggroAbsorb: // Aggro absorb %, absorb %
                case SkillParam.DebuffedDamageIncrease: // Unk, % increase
                case SkillParam.IncreaseAttack: // Phy increase %, Mag increase %
                case SkillParam.CurseSeriesReduce: // Type?, Effect
                case SkillParam.MonsterMask: // Monster level, Item Id
                case SkillParam.WizardTeleport: // Time, Distance
                case SkillParam.MobAggroReduce: // Reduction amount, Reduction %
                case SkillParam.StealMp: // Unk, Amount
                case SkillParam.HealingDance: // Recovery %, Unk
                case SkillParam.ManaDance: // Recovery %, Unk
                case SkillParam.TargetDash: // Unk, Unk
                case SkillParam.PetExpPotion: // Unk, Increase %
                case SkillParam.ManaSwitch: // Unk, Damage MP conversion %
                case SkillParam.BicheonSkill: // Shield def reduction %, phy increase amount
                case SkillParam.SocketStoneReflect: // Phy ratio %, Mag ratio %
                case SkillParam.SocketStoneFreeMp: // Mp refunded
                case SkillParam.ExpSpIncrease: // Exp increase %, Sp increase %
                case SkillParam.Resurrection: // Max lvl, recovered exp
                case SkillParam.DamageAbsorb: // Phy %, Mag %
                case SkillParam.ActiveMpConsumed: // Mp consumed / 1000, Base mp consumed
                case SkillParam.RequiredItem: // Tid3, tid4
                    return 2;
                case SkillParam.CurseProbabilityReduce: // Unk, Probability %, Level 
                case SkillParam.CurseRemove2:
                case SkillParam.TransferDamageAbsorb: // Type, absorb %, Unk
                case SkillParam.Invisible: // Type, Level, Movement speed reduction %
                case SkillParam.Cancelable: // Unk1, Unk2 (2 = when attacked), Unk3
                case SkillParam.Poison: // Effect, Probability %, Unk
                case SkillParam.Root: // Duration, Probability, Unk
                case SkillParam.Fear: // Duration, Probability %, Level
                case SkillParam.WarlockTrap: // Unk, Unk, Unk
                case SkillParam.Confusion: // Duration, Probability %, Level
                case SkillParam.FellowAttack:
                case SkillParam.FellowIncreaseAttack:
                case SkillParam.FellowZerk:
                case SkillParam.FellowBuff:
                case SkillParam.RedBlueFellowBuff:
                case SkillParam.FellowMpRecovery:
                    return 3;
                case SkillParam.MonsterCapture: // Type, MobId1, mobId2, mobId3
                case SkillParam.ReduceSelfPhyMagDmg:
                case SkillParam.DamageReturn: // Probability %, Phy return %, Mag return %, Radius
                case SkillParam.LinkedSkill: // Type, Distance, Number of connections, Link owner (bool)
                case SkillParam.ReduceSelfMaxHp: // Duration?, Unk, Amount %, Unk2
                case SkillParam.ReduceSelfPhyMagDef: // Duration?, Phy %, Mag %, Unk2
                case SkillParam.Decay: // Duration, Probability %, Level, Effect
                case SkillParam.Weaken: // Duration, Probability %, Level, Effect
                case SkillParam.Division: // Duration, Probability %, Level, Effect
                case SkillParam.Impotent: // Duration, Probability %, Level, Decrease %
                case SkillParam.Hidden: // Damage, Probability, Level, Unk
                case SkillParam.ShortSight: // Duration, Probability %, Level, Effect
                case SkillParam.Disease: // Duration, Probability %, Level, Effect
                case SkillParam.Darkness: // Duration, Probability %, Level, Effect
                case SkillParam.RamadanExpSpIncrease: // Duration, Unk, Increase %, Unk
                case SkillParam.HpMpRecovery: // Hp amount, Hp %, Mp amount, Mp %
                    return 4;
                case SkillParam.Sleep: // Duration, Probability %, Level, Effect, Times
                case SkillParam.Dull: // Duration, Probability %, Level, Effect, Times
                case SkillParam.Bleed:
                case SkillParam.Attack: // Dmg type, dmg %, min dmg, max dmg, dmg % players
                    return 5;
                case SkillParam.Combustion: // Duration, Probability, Level, Reduce MaxMP %, MP Regen %, Current MP %
                case SkillParam.Panic: // Duration, Probability, Level, Reduce MaxHP %, HP Regen %, Current HP %
                case SkillParam.AreaEffect: // Base range, Range type, Distance, Amount affected, Reduce per target, Unk
                    return 6;
                default:
                    return 0;
            }
        }
    }
}