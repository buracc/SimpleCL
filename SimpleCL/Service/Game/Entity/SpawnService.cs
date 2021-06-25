using SilkroadSecurityApi;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Server;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Service.Game.Entity
{
    public class SpawnService : Service
    {
        private readonly SilkroadServer _silkroadServer;

        public SpawnService(SilkroadServer silkroadServer)
        {
            _silkroadServer = silkroadServer;
        }

        [PacketHandler(Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)]
        public void EntitySpawn(Server server, Packet packet)
        {
            var refObjId = packet.ReadUInt();
            var entity = Model.Entity.Entity.FromId(refObjId);
            
        }
    }
}