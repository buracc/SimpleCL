using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Providers
{
    public class Npcs
    {
        private static readonly List<TalkNpc> EmptyList = new();

        public static List<TalkNpc> GetAll(Func<TalkNpc, bool> filter = null)
        {
            var all = Entities.GetNpcs();
            if (all.IsEmpty())
            {
                return EmptyList;
            }
            
            return filter != null ? all.Where(filter).ToList() : all;
        }

        public static TalkNpc GetFirst(Func<TalkNpc, bool> filter = null)
        {
            var all = Entities.GetNpcs();
            if (all.IsEmpty())
            {
                return null;
            }
            
            return filter != null ? all.FirstOrDefault(filter) : all.FirstOrDefault();
        }
    }
}