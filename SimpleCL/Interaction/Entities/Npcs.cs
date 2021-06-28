using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Model.Entity;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Entities
{
    public class Npcs
    {
        private static readonly List<TalkNpc> EmptyList = new List<TalkNpc>();

        public static List<TalkNpc> GetAll(Func<TalkNpc, bool> filter = null)
        {
            var all = Entities.GetNpcs();
            if (all.IsEmpty())
            {
                return EmptyList;
            }
            
            if (filter != null)
            {
                return all.Where(filter).ToList();
            }
            
            return all;
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