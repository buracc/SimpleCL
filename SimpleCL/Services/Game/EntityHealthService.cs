using SimpleCL.SilkroadSecurityApi;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Entities;
using SimpleCL.Network;
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

            if (!(entity is Actor actor))
            {
                return;
            }
            
            packet.ReadByte();
            packet.ReadByte();
            
            EntityStateEvent.Health eventType = (EntityStateEvent.Health) packet.ReadByte();
            
            switch (eventType)
            {
                case EntityStateEvent.Health.HP:
                    actor.Hp = packet.ReadUInt();
                    if (actor is Monster monster && monster.Hp == 0)
                    {
                        Entities.Despawned(actor.Uid);
                    }
                    break;

                case EntityStateEvent.Health.MP:
                    actor.Mp = packet.ReadUInt();
                    break;

                case EntityStateEvent.Health.EntityHPMP:
                case EntityStateEvent.Health.HPMP:
                    actor.Hp = packet.ReadUInt();
                    if (actor is Monster m && m.Hp == 0)
                    {
                        Entities.Despawned(actor.Uid);
                    }
                    
                    actor.Mp = packet.ReadUInt();
                    break;
                
                case EntityStateEvent.Health.BadStatus:
                    actor.BadStatus = (Actor.BadStatusFlag) packet.ReadUInt();
                    break;
            }
        }

        #endregion
    }
}