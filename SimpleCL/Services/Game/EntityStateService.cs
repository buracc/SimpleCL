using SimpleCL.Enums.Commons;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Entities;
using SimpleCL.Network;
using SimpleCL.SecurityApi;

namespace SimpleCL.Services.Game
{
    public class EntityStateService : Service
    {
        [PacketHandler(Opcodes.Agent.Response.ENTITY_CHANGE_STATUS)]
        public void StateChange(Server server, Packet packet)
        {
            if (!Entities.AllEntities.TryGetValue(packet.ReadUInt(), out var entity) || entity is not Actor actor)
            {
                return;
            }
            
            var updateType = packet.ReadByte();
            var updateState = packet.ReadByte();

            switch (updateType)
            {
                case 0:
                    actor.LifeState = (Actor.Health.LifeState) updateState;    
                    break;
                
                case 1:
                    actor.Motion = (Actor.Movement.Motion) updateState;
                    actor.WalkMode = actor.Motion switch
                    {
                        Actor.Movement.Motion.Running => Actor.Movement.Mode.Running,
                        Actor.Movement.Motion.Walking => Actor.Movement.Mode.Walking,
                        _ => actor.WalkMode
                    };
                    
                    break;
            }
        }
    }
}