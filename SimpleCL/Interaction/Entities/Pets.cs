using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Model.Entity;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Entities
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

            if (filter != null)
            {
                return all.Where(filter).ToList();
            }

            return all;
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