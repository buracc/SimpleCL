using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Providers
{
    public class Players
    {
        private static readonly List<Player> EmptyList = new List<Player>();

        public static List<Player> GetAll(Func<Player, bool> filter = null)
        {
            var all = Entities.GetPlayers();
            if (all.IsEmpty())
            {
                return EmptyList;
            }
            
            return filter != null ? all.Where(filter).ToList() : all;
        }

        public static Player GetFirst(Func<Player, bool> filter = null)
        {
            var all = Entities.GetPlayers();
            if (all.IsEmpty())
            {
                return null;
            }
            
            return filter != null ? all.FirstOrDefault(filter) : all.FirstOrDefault();
        }
    }
}