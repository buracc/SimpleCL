using SimpleCL.Enums;

namespace SimpleCL.Model
{
    public class GameServer
    {
        private ushort _id;
        private string _name;
        private ServerCapacity _serverCapacity;
        private bool _available;

        public GameServer(ushort id, string name, ServerCapacity serverCapacity, bool available)
        {
            _id = id;
            _name = name;
            _serverCapacity = serverCapacity;
            _available = available;
        }

        public ushort Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public ServerCapacity ServerCapacity
        {
            get => _serverCapacity;
            set => _serverCapacity = value;
        }

        public bool Available
        {
            get => _available;
            set => _available = value;
        }

        public override string ToString()
        {
            return "[" + _name + " (" + _id + ")] - Status: " + (_available ? _serverCapacity.ToString() : "Offline");
        }
    }
}