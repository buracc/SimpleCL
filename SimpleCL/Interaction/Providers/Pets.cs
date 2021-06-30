using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Providers
{
    public class Pets
    {
        private static readonly List<CharacterPet> EmptyList = new List<CharacterPet>();

        public static List<CharacterPet> GetAll(Func<CharacterPet, bool> filter = null)
        {
            var all = Entities.GetPets().ToList();
            if (all.IsEmpty())
            {
                return EmptyList;
            }

            return filter != null ? all.Where(filter).ToList() : all;
        }

        public static CharacterPet GetFirst(Func<CharacterPet, bool> filter = null)
        {
            var all = Entities.GetPets().ToList();
            if (all.IsEmpty())
            {
                return null;
            }

            return filter != null ? all.FirstOrDefault(filter) : all.FirstOrDefault();
        }
    }
}