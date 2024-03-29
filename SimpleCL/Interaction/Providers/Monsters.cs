﻿using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Providers
{
    public class Monsters
    {
        private static readonly List<Monster> EmptyList = new();

        public static List<Monster> GetAll(Func<Monster, bool> filter = null)
        {
            var all = Entities.GetMonsters().ToList();
            if (all.IsEmpty())
            {
                return EmptyList;
            }

            return filter != null ? all.Where(filter).ToList() : all;
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