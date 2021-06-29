using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Entities
{
    public class Monsters
    {
        private static readonly List<Monster> EmptyList = new List<Monster>();

        public static List<Monster> GetAll(Func<Monster, bool> filter = null)
        {
            var all = Entities.GetMonsters().ToList();
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

        public static Monster GetFirst(Func<Monster, bool> filter = null)
        {
            var all = Entities.GetMonsters().ToList();
            if (all.IsEmpty())
            {
                return null;
            }

            return filter != null ? all.FirstOrDefault(filter) : all.FirstOrDefault();
        }
    }
}