using SimpleCL.Enums.Server;

namespace SimpleCL.Models.Server
{
    public class GameServer
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public ServerCapacity ServerCapacity { get; set; }
        public bool Available { get; set; }
        
        public GameServer(ushort id, string name, ServerCapacity serverCapacity, bool available)
        {
            Id = id;
            Name = name;
            ServerCapacity = serverCapacity;
            Available = available;
        }

        public override string ToString()
        {
            return "[" + Name + " (" + Id + ")] - Status: " + (Available ? ServerCapacity.ToString() : "Offline");
        }
    }
}