using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;

namespace SimpleCL.Interaction.Entities
{
    public static class Entities
    {
        public static readonly Dictionary<uint, Entity> AllEntities = new Dictionary<uint, Entity>();

        public static void Spawn(Entity e)
        {
            if (!AllEntities.ContainsKey(e.Uid))
            {
                AllEntities[e.Uid] = e;
                SimpleCL.Gui.AddMinimapMarker(e);
            }
        }

        public static void Despawn(uint uid)
        {
            if (AllEntities.ContainsKey(uid))
            {
                AllEntities.Remove(uid);
                SimpleCL.Gui.RemoveMinimapMarker(uid);
            }
        }

        public static void Moved(uint uid, LocalPoint point)
        {
            if (AllEntities.ContainsKey(uid))
            {
                AllEntities[uid].LocalPoint = point;
                SimpleCL.Gui.RefreshMap();
            }
        }

        public static List<Player> GetPlayers()
        {
            return AllEntities.Values.Where(x => x is Player).Cast<Player>().ToList();
        }
        
        public static List<Monster> GetMonsters()
        {
            return AllEntities.Values.Where(x => x is Monster).Cast<Monster>().ToList();
        }

        public static List<TalkNpc> GetNpcs()
        {
            return AllEntities.Values.Where(x => x is TalkNpc).Cast<TalkNpc>().ToList();
        }
        
        public static List<CharacterPet> GetPets()
        {
            return AllEntities.Values.Where(x => x is CharacterPet).Cast<CharacterPet>().ToList();
        }

        public static List<Teleport> GetTeleports()
        {
            return AllEntities.Values.Where(x => x is Teleport).Cast<Teleport>().ToList();
        }
    }
}