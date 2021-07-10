using System.IO;

namespace SimpleCL.Enums
{
    public static class Constants
    {
        public static class Paths
        {
            public const string Icons = "/Icon/";
        }
        public static class Strings
        {
            #region Icons

            public const string Ddj = ".ddj";
            public const string Png = ".png";

            #endregion
            
            #region Entities

            public const string Id = "id";
            public const string Tid1 = "tid1";
            public const string Tid2 = "tid2";
            public const string Tid3 = "tid3";
            public const string Tid4 = "tid4";

            public const string Name = "name";
            public const string ServerName = "servername";

            #endregion
            
            #region Items

            public const string CashItem = "cash_item";
            public const string Icon = "icon";
            public const string Slot = "slot";

            #endregion

            #region Shops

            public const string Item = "item";
            public const string Price = "price";
            public const string Tab = "tab";

            #endregion

            #region Servernames

            public const string Warehouse = "WAREHOUSE";
            public const string Accessory = "ACCESSORY";
            public const string Smith = "SMITH";
            public const string Potion = "POTION";
            public const string Armor = "ARMOR";
            public const string Horse = "HORSE";

            #endregion

            #region Npc icons

            public const string GuildStorageIcon = "xy_guild";
            public const string EtcIcon = "xy_etc";
            public const string SmithIcon = "xy_weapon";
            public const string PotionIcon = "xy_potion";
            public const string ArmorIcon = "xy_armor";
            public const string StableIcon = "xy_stable";
            public const string NpcIcon = "mm_sign_npc";

            #endregion

            #region Teleport link

            public const string DestinationId = "destinationid";
            public const string Destination = "destination";

            #endregion

            #region Stalls

            public const string Opened = "Opened";
            public const string Modifying = "Modifying";

            #endregion

            public const string GroupId = "group_id";
            public const string CastTime = "casttime";
            public const string Cooldown = "cooldown";
            public const string Duration = "duration";
            public const string Mastery = "mastery";
            public const string SkillGroup = "skillgroup";
            public const string SkillGroupIndex = "skillgroup_index";
            public const string Sp = "sp";
            public const string Mp = "mp";
            public const string Level = "level";
            public const string Description = "description";
            public const string Attributes = "attributes";
            public const string RequiredLearn1 = "requiredlearn_1";
            public const string RequiredLearn2 = "requiredlearn_2";
            public const string RequiredLearn3 = "requiredlearn_3";
            public const string RequiredLevel1 = "requiredlearnlevel_1";
            public const string RequiredLevel2 = "requiredlearnlevel_2";
            public const string RequiredLevel3 = "requiredlearnlevel_3";
            public const string Weapon1 = "weapon_1";
            public const string Weapon2 = "weapon_2";
            public const string TargetRequired = "target_required";
            public const string Range = "range";
        }
    }
}