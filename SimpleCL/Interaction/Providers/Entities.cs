using System.Collections.Generic;
using System.Linq;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Models.Skills;

namespace SimpleCL.Interaction.Providers
{
    public static class Entities
    {
        public static readonly Dictionary<uint, Entity> AllEntities = new Dictionary<uint, Entity>();
        private static readonly Dictionary<uint, uint> Buffs = new Dictionary<uint, uint>();

        public static void Respawn()
        {
            AllEntities.Clear();
            Buffs.Clear();
        }

        public static void Spawn(Entity e)
        {
            if (AllEntities.ContainsKey(e.Uid))
            {
                return;
            }
            
            AllEntities[e.Uid] = e;
            SimpleCL.Gui.AddMinimapMarker(e);
        }

        public static void Despawn(uint uid)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }
            
            AllEntities.Remove(uid);
            SimpleCL.Gui.RemoveMinimapMarker(uid);
        }

        public static void Moved(uint uid, LocalPoint point = null, ushort angle = 0)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }
            
            var entity = AllEntities[uid];
            if (entity is Actor actor)
            {
                if (point != null)
                {
                    actor.LocalPoint = point;
                }
                    
                if (angle > 0)
                {
                    actor.Angle = angle;
                }
            }

            SimpleCL.Gui.RefreshMap();
        }

        public static void Buffed(Buff buff)
        {
            if (!AllEntities.ContainsKey(buff.TargetUid))
            {
                return;
            }

            var entity = AllEntities[buff.TargetUid];
            if (!(entity is Actor actor))
            {
                return;
            }
            
            actor.Buffs.Add(buff);
            Buffs[buff.TargetUid] = buff.Uid;
        }

        public static void BuffEnded(uint buffUid)
        {
            if (!Buffs.ContainsKey(buffUid))
            {
                return;
            }

            var entity = AllEntities[Buffs[buffUid]];
            if (!(entity is Actor actor))
            {
                return;
            }
            
            actor.Buffs.RemoveAll(x => x.Uid == buffUid);
            Buffs.Remove(buffUid);
        }

        public static void SpeedChanged(uint uid, float walkSpeed, float runSpeed)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }
            
            var entity = AllEntities[uid];
            if (!(entity is Actor actor))
            {
                return;
            }
            
            actor.WalkSpeed = walkSpeed;
            actor.RunSpeed = runSpeed;
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