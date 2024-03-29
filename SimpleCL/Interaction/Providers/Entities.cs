﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SimpleCL.Models;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Npcs;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Models.Skills;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Providers
{
    public static class Entities
    {
        public static readonly ConcurrentDictionary<uint, Entity> AllEntities = new();

        public static BindingList<Entity> TargetableEntities
        {
            get
            {
                return new(AllEntities.Values.Where(x => x is ITargetable).ToList());
            }
        }

        private static readonly Dictionary<uint, uint> Buffs = new();

        public static void Respawn()
        {
            AllEntities.DisposeAll();
            AllEntities.Clear();
            
            Buffs.DisposeAll();
            Buffs.Clear();
            
            LocalPlayer.Get.Dispose();
            
            Program.Gui.ClearMarkers();
            Program.Gui.ClearTiles();
        }

        public static void Spawned(Entity e)
        {
            if (AllEntities.ContainsKey(e.Uid))
            {
                return;
            }

            AllEntities[e.Uid] = e;
            Program.Gui.AddMinimapMarker(e);
        }

        public static void Despawned(uint uid)
        {
            if (!AllEntities.ContainsKey(uid))
            {
                return;
            }

            var remove = AllEntities.TryRemove(uid, out var removedEntity);
            if (!remove)
            {
                return;
            }
            
            Program.Gui.RemoveMinimapMarker(uid);
            
            removedEntity.Dispose();
        }

        public static void Moved(uint uid, LocalPoint destination = null, ushort angle = 0)
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

            if (destination != null)
            {
                actor.StartMovement(destination);
            }

            if (angle > 0)
            {
                actor.Angle = angle;
            }

            if (uid == LocalPlayer.Get.Uid && angle > 0)
            {
                Program.Gui.SetLocalPlayerMarkerAngle();
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

            Program.Gui.InvokeLater(() => { actor.Buffs.Add(buff); });

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
                buff.Dispose();
                Program.Gui.InvokeLater(() =>
                {
                    var removed = actor.Buffs[actor.Buffs.IndexOf(buff)];
                    removed?.Dispose();
                    actor.Buffs.Remove(buff);
                });
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