using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SimpleCL.Models;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Models.Skills;

namespace SimpleCL.Interaction.Providers
{
    public static class Entities
    {
        public static readonly Dictionary<uint, Entity> AllEntities = new();
        public static readonly BindingList<ITargetable> TargetableEntities = new();
        private static readonly Dictionary<uint, uint> Buffs = new();

        public static void Respawn()
        {
            AllEntities.Clear();
            TargetableEntities.Clear();
            Buffs.Clear();
        }

        public static void Spawned(Entity e)
        {
            if (AllEntities.ContainsKey(e.Uid))
            {
                return;
            }

            AllEntities[e.Uid] = e;
            if (e is ITargetable targetable)
            {
                TargetableEntities.Add(targetable);
            }

            Program.Gui.AddMinimapMarker(e);
        }

        public static void Despawned(uint uid)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }

            AllEntities.TryGetValue(uid, out var removed);
            if (removed is ITargetable targetable)
            {
                TargetableEntities.Remove(targetable);
            }

            AllEntities.Remove(uid);
            Program.Gui.RemoveMinimapMarker(uid);
        }

        public static void Moved(uint uid, LocalPoint destination = null, ushort angle = 0)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }

            var entity = AllEntities[uid];
            if (entity is Actor actor)
            {
                if (destination != null)
                {
                    actor.StartMovement(destination);
                }

                if (angle > 0)
                {
                    actor.Angle = angle;
                }
            }
        }

        public static void Buffed(Buff buff)
        {
            if (!AllEntities.ContainsKey(buff.TargetUid))
            {
                return;
            }

            var entity = AllEntities[buff.TargetUid];
            if (entity is not Actor actor)
            {
                return;
            }

            actor.Buffs.Add(buff);
            Buffs[buff.Uid] = buff.TargetUid;
        }

        public static void BuffEnded(uint buffUid)
        {
            if (!Buffs.ContainsKey(buffUid))
            {
                return;
            }

            var entity = AllEntities[Buffs[buffUid]];
            if (entity is not Actor actor)
            {
                return;
            }

            var buffsToRemove = actor.Buffs.Where(x => x.Uid == buffUid).ToList();
            foreach (var buff in buffsToRemove)
            {
                actor.Buffs.Remove(buff);
                Buffs.Remove(buff.Uid);
            }
        }

        public static void SpeedChanged(uint uid, float walkSpeed, float runSpeed)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }

            var entity = AllEntities[uid];
            if (entity is not Actor actor)
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