using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Entities
{
    public class Teleports
    {
        private static readonly List<Teleport> EmptyList = new List<Teleport>();

        public static List<Teleport> GetAll(Func<Teleport, bool> filter = null)
        {
            var all = Entities.GetTeleports();
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

        public static Teleport GetFirst(Func<Teleport, bool> filter = null)
        {
            var all = Entities.GetTeleports();
            if (all.IsEmpty())
            {
                return null;
            }
            
            return filter != null ? all.FirstOrDefault(filter) : all.FirstOrDefault();
        }
    }
}