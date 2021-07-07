using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Skills;

namespace SimpleCL.Models.Entities
{
    public class Actor : Entity
    {
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public Health.BadStatusFlag BadStatus { get; set; }
        public float WalkSpeed { get; set; }
        public float RunSpeed { get; set; }
        public float ZerkSpeed { get; set; }
        public ushort Angle { get; set; }
        public BindingList<Buff> Buffs = new();
        public Movement.Motion Motion { get; set; }
        public Movement.Mode WalkMode { get; set; }
        public Health.LifeState LifeState { get; set; }

        private Timer _movementTimer = new(50);

        public Actor(uint id) : base(id)
        {
        }

        public void StopMovementTimer()
        {
            _movementTimer?.Dispose();
        }

        public void StartMovement(LocalPoint destination)
        {
            StopMovementTimer();

            var oldPos = WorldPoint;
            var newPos = WorldPoint.FromLocal(destination);

            var timeToDestination =
                GetMillisPerTile() * oldPos.DistanceTo(newPos);

            var xDiff = newPos.X - oldPos.X;
            var yDiff = newPos.Y - oldPos.Y;
            
            _movementTimer = new Timer(50);

            var intervalMs = _movementTimer.Interval;
            var totalIntervals = (float) (timeToDestination / intervalMs);
            var intervals = 0;
            
            _movementTimer.Elapsed += (_, _) =>
            {
                if (++intervals > totalIntervals)
                {
                    _movementTimer.Dispose();
                    LocalPoint = destination;
                    return;
                }

                var newWorldLoc = WorldPoint.Translate(xDiff / totalIntervals,
                    yDiff / totalIntervals);
                LocalPoint = LocalPoint.FromWorld(newWorldLoc);
            };

            _movementTimer.Start();
        }

        public float GetTilesPerMillis(int interval = 50)
        {
            if (WalkMode == Movement.Mode.Running)
            {
                return RunSpeed / 10000 * interval;
            }

            return WalkSpeed / 10000 * interval;
        }

        public float GetMillisPerTile()
        {
            // Speed unit is tiles per 10 seconds

            if (WalkMode == Movement.Mode.Running)
            {
                var tilesPerSecond = RunSpeed / 10;
                var millisPerTile = 1000 / tilesPerSecond;

                return millisPerTile;
            }

            var tilesPerSec = WalkSpeed / 10;
            var msPerTile = 1000 / tilesPerSec;

            return msPerTile;
        }

        public bool IsPlayer()
        {
            return TypeId2 == 1;
        }

        public bool IsNpc()
        {
            return TypeId2 == 2;
        }

        public static class Movement
        {
            public enum Mode : byte
            {
                Walking = 0,
                Running = 1
            }

            public enum Action : byte
            {
                Spinning = 0,
                KeyWalking = 1
            }

            public enum Motion : byte
            {
                StandUp = 0,
                Walking = 2,
                Running = 3,
                Sitting = 4
            }
        }

        public static class Health
        {
            public enum LifeState : byte
            {
                Unknown = 0,
                Alive = 1,
                Dead = 2
            }

            public enum GameState : byte
            {
                None = 0,
                Berserk = 1,
                Untouchable = 2,
                GameMasterInvincible = 3,
                GameMasterUntouchable = 4,
                GameMasterInvisible = 5,
                Stealth = 6,
                Invisible = 7
            }

            public enum BadStatusFlag : uint
            {
                None = 0,
                Freezing = 0x1, // Universal
                Frostbite = 0x2, // None
                ElectricShock = 0x4, // Universal
                Burn = 0x8, // Universal
                Poisoning = 0x10, // Universal
                Zombie = 0x20, // Universal
                Sleep = 0x40, // None
                Bind = 0x80, // None
                Dull = 0x100, // Purification
                Fear = 0x200, // Purification
                ShortSight = 0x400, // Purification
                Bleed = 0x800, // Purification
                Petrify = 0x1000, // None
                Darkness = 0x2000, // Purification
                Stun = 0x4000, // None
                Disease = 0x8000, // Purification
                Confusion = 0x10000, // Purification
                Decay = 0x20000, // Purification
                Weaken = 0x40000, // Purification
                Impotent = 0x80000, // Purification
                Division = 0x100000, // Purification
                Panic = 0x200000, // Purification
                Combustion = 0x400000, // Purification
                Unk01 = 0x800000,
                Hidden = 0x1000000, // Purification
                Unk02 = 0x2000000,
                Unk03 = 0x4000000,
                Unk04 = 0x8000000,
                Unk05 = 0x10000000,
                Unk06 = 0x20000000,
                Unk07 = 0x40000000,
                Unk08 = 0x80000000
            }
        }
    }
}