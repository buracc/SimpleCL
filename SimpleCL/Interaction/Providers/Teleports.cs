using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Providers
{
    public class Teleports
    {
        private static readonly List<Teleport> EmptyList = new();

        public static List<Teleport> GetAll(Func<Teleport, bool> filter = null)
        {
            var all = Entities.GetTeleports();
            if (all.IsEmpty())
            {
                return EmptyList;
            }
            
            return filter != null ? all.Where(filter).ToList() : all;
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