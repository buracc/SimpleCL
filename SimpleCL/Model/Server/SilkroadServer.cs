namespace SimpleCL.Model.Server
{
    public class SilkroadServer
    {
        private ushort id;
        private string name;
        private ServerCapacity serverCapacity;
        private bool available;

        public SilkroadServer(ushort id, string name, ServerCapacity serverCapacity, bool available)
        {
            this.id = id;
            this.name = name;
            this.serverCapacity = serverCapacity;
            this.available = available;
        }

        public ushort Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public ServerCapacity ServerCapacity
        {
            get => serverCapacity;
            set => serverCapacity = value;
        }

        public bool Available
        {
            get => available;
            set => available = value;
        }

        public override string ToString()
        {
            return "[" + name + " (" + id + ")] - Status: " + (available ? serverCapacity.ToString() : "Offline");
        }
    }
}