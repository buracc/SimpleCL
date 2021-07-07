using System;

namespace SimpleCL.Models.Entities
{
    public class TalkNpc : Npc
    {
        public string MapIcon
        {
            get
            {
                return ServerName switch
                {
                    _ when ServerName.Contains("WAREHOUSE") => "xy_guild",
                    _ when ServerName.Contains("ACCESSORY") => "xy_etc",
                    _ when ServerName.Contains("SMITH") => "xy_weapon",
                    _ when ServerName.Contains("POTION") => "xy_potion",
                    _ when ServerName.Contains("ARMOR") => "xy_armor",
                    _ when ServerName.Contains("HORSE") => "xy_stable",
                    _ => "mm_sign_npc"
                };
            }
        }

        public TalkNpc(uint id) : base(id)
        {
        }
    }
}