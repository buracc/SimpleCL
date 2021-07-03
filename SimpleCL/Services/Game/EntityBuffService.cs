using SimpleCL.Enums.Commons;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Skills;
using SimpleCL.Network;
using SimpleCL.SilkroadSecurityApi;

namespace SimpleCL.Services.Game
{
    public class EntityBuffService : Service
    {
        [PacketHandler(Opcodes.Agent.Response.SKILL_BUFF_START)]
        public void BuffStart(Server server, Packet packet)
        {
            var targetUid = packet.ReadUInt();
            var buff = new Buff(packet.ReadUInt());
            buff.Uid = packet.ReadUInt();
            buff.TargetUid = targetUid;
            Entities.Buffed(buff);
        }

        [PacketHandler(Opcodes.Agent.Response.SKILL_BUFF_END)]
        public void BuffEnd(Server server, Packet packet)
        {
            if (packet.ReadBool())
            {
                Entities.BuffEnded(packet.ReadUInt());
            }
        }
    }
}