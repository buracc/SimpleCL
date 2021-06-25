namespace SimpleCL.Enums.Item.Type
{
    // ItemCategory == 3
    public static class ConsumableData
    {
        public enum Type : byte
        {
            Potion = 1,
            BadStatus = 2,
            Scroll = 3,
            Ammo = 4,
            Currency = 5,
            Firework = 6,
            Campfire = 7,
            TradeGoods = 8,
            Quest = 9,
            Enhancement = 10,
            Alchemy = 11,
            Etc = 12,
            MallScroll = 13,
            MagicPop = 14,
            SummonScroll = 15,
            Fellow = 16
        }

        public static class SubType
        {
            public enum Potion : byte
            {
                Health = 1,
                Mana = 2,
                Vigor = 3,
                RecoveryKit = 4,
                PetResurrection = 6,
                BerserkerRegeneration = 8,
                HGP = 9,
                FortressRepair = 10,
                FellowSupplement = 12,
                BattleNostrum = 13
            }
            
            public enum BadStatus : byte
            {
                PurificationPill = 1,
                UniversalPill = 6,
                AbnormalStateRecoveryPotion = 7,
                MallCurePotion = 8,
                AbnormalStatePill = 9
            }
            
            public enum Scroll : byte
            {
                Return = 1,
                Transport = 2,
                ReverseReturn = 3,
                StallDecoration = 4,
                GlobalChat = 5,
                FortressMonsterSummon = 6,
                FortressNpcSummon = 7,
                FortressManual = 8,
                FortressFlag = 9,
                BeginnerExp = 10,
                FortressUniqueSummon = 11,
                Skillpoint = 12,
                EquipmentEnhancement = 14,
                PremiumQuestTicket = 15,
                Coupon = 16,
                DevilEnhancement = 17,
                ExperienceStone = 18,
                UnionPartyTicket = 19,
                BuffFlower = 20,
                RankUpStone = 21,
                VipGlobalChat = 22
            }
            
            public enum Ammo : byte
            {
                Arrow = 1,
                Bolt = 2
            }
            
            public enum Currency : byte
            {
                Gold = 0,
                Coin = 1
            }
            
            public enum Firework : byte
            {
                Cosmetic = 1,
                FortressBomb = 2
            }
            
            public enum Campfire : byte
            {
                Campfire = 1    
            }
            
            public enum TradeGoods : byte
            {
                Consumable = 0,
                Regular = 1,
                Special = 2,
                SpecialtyGoodsBox = 3
            }
            
            public enum Quest : byte
            {
                All = 0
            }
            
            public enum Enhancement : byte
            {
                Elixir = 1,
                LuckyPowder = 2,
                AdvancedElixir = 4,
                JobReinforcement = 5,
                Enhancer = 6,
                ProofStone = 8,
                LevelElixir = 9,
                MaxEnhancer = 10,
                MaxProtector = 11
            }
            
            public enum Alchemy : byte
            {
                MagicStone = 1,
                AttributeStone = 2,
                Tablet = 3,
                Material = 4,
                Element = 5,
                Rondo = 6,
                SpecialMagicStone = 7,
                SocketStone = 8,
                JobRondo = 9,
                SocketOpener = 10,
                FortressRecipe = 11,
                JobRecipe = 12,
                JobBlue = 13,
                AwakenStone = 14,
                SpecialAwakenStone = 15,
                AlchemyCatalyst = 17,
                Wheel = 18
            }
            
            public enum Etc : byte
            {
                MercenaryScroll = 1,
                EmblemScroll = 2,
                MemberSummonScroll = 3,
                BattleArenaTicket = 4,
                FgwTalisman = 5,
                NameChangeTicket = 6,
                DimensionHole = 7,
                Gemstone = 8,
                SurvivalArenaBuff = 9
            }
            
            public enum MallScroll : byte
            {
                SkillEdit = 0,
                BuffScroll = 1,
                ExpTicket = 4,
                SkillTicket = 5,
                ResScroll = 6,
                RepairHammer = 7,
                GenderSwitch = 8,
                SkinChange = 9,
                StorageTicket = 10,
                PetClock = 12,
                SkillStatReset = 13,
                Premium = 14,
                PetBuff = 15,
                DevilExtension = 16,
                ExpansionScroll = 17,
                Indulgence = 18,
                OpenMarketTicket = 19,
                FreeAccessTicket = 20,
                Booster = 21,
                DungeonTicket = 22,
                FgwTicket = 23,
                ItemLock = 24,
                VisualStimulation = 26
            }
            
            public enum MagicPop : byte
            {
                Card = 1,
                PrizeBox = 2,
                VipCard = 4
            }
            
            public enum SummonScroll : byte
            {
                Pandora = 1,
                MonsterScroll = 2
            }
            
            public enum Fellow : byte
            {
                Skill = 1,
                NamingScroll = 3,
                TransferStone = 4,
                EvolutionPotion = 5
            }
        }
    }
}