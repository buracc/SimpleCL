using System.Collections.Generic;
using SimpleCL.Database;
using SimpleCL.Enums;

namespace SimpleCL.Models.Entities.Teleporters
{
    public class Teleport : Entity
    {
        public List<TeleportLink> Links = new();
        public Teleport(uint id) : base(id)
        {
            var links = GameDatabase.Get.GetTeleportLinks(id);
            foreach (var link in links)
            {
                Links.Add(new TeleportLink(uint.Parse(link[Constants.Strings.DestinationId]), link[Constants.Strings.Destination]));
            }
        }

        public bool IsNpc()
        {
            return TypeId1 == 1 && TypeId2 == 2 && TypeId3 == 2 && TypeId4 == 0;
        }

        public bool IsGate()
        {
            return TypeId1 == 4 && TypeId2 == 0 && TypeId3 == 0 && TypeId4 == 0;
        }

        public enum Type : byte
        {
            None = 0,
            Regular = 1,
            Dimensional = 6
        }
    }
}