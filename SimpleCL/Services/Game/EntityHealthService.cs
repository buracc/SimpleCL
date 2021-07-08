using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Entities;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class EntityHealthService : Service
    {
        #region HealthChanged

        [PacketHandler(Opcodes.Agent.Response.ENTITY_POTION_UPDATE)]
        public void HealthChanged(Server server, Packet packet)
        {
            var uid = packet.ReadUInt();

            if (!Entities.AllEntities.ContainsKey(uid))
            {
                return;
            }

            var entity = Entities.AllEntities[uid];

            if (entity is not Actor actor)
            {
                return;
            }
            
            packet.ReadByte();
            packet.ReadByte();
            
            var eventType = (EntityStateEvent.Health) packet.ReadByte();
            
            switch (eventType)
            {
                case EntityStateEvent.Health.Hp:
                    actor.Hp = packet.ReadUInt();
                    break;

                case EntityStateEvent.Health.Mp:
                    actor.Mp = packet.ReadUInt();
                    break;

                case EntityStateEvent.Health.EntityHpMp:
                case EntityStateEvent.Health.HpMp:
                    actor.Hp = packet.ReadUInt();
                    actor.Mp = packet.ReadUInt();
                    break;
                
                case EntityStateEvent.Health.BadStatus:
                    actor.BadStatus = (Actor.Health.BadStatusFlag) packet.ReadUInt();
                    break;
            }
        }

        #endregion
    }
}