﻿using System;

namespace SimpleCL.Models.Coordinates
{
    public class LocalPoint : ICoord
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public ushort Region { get; set; }

        public LocalPoint(ushort region, float x, float z, float y)
        {
            X = x;
            Y = y;
            Z = z;
            Region = region;
        }

        public override string ToString()
        {
            return "X: " + (int) X + " | Y: " + (int) Y + " | Z: " + (int) Z + " | Region: " + Region;
        }

        public LocalPoint Translate(float x = 0, float y = 0)
        {
            var world = WorldPoint.FromLocal(this).Translate(x, y);
            return FromWorld(world);
        }

        public bool InCave()
        {
            return Region > short.MaxValue;
        }

        public static LocalPoint FromWorld(WorldPoint worldPoint)
        {
            if (worldPoint == null)
            {
                return null;
            }
            
            if (worldPoint.InCave())
            {
                return new LocalPoint(worldPoint.Region, worldPoint.X, worldPoint.Z, worldPoint.Y);
            }

            var x = Math.Abs(worldPoint.X % 192.0f * 10.0f);
            if (worldPoint.X < 0.0)
            {
                x = 1920f - x;
            }

            var y = Math.Abs(worldPoint.Y % 192.0f * 10.0f);
            if (worldPoint.Y < 0.0)
            {
                y = 1920f - y;
            }

            var xSector = (byte) Math.Round((worldPoint.X - x / 10) / 192.0 + 135);
            var ySector = (byte) Math.Round((worldPoint.Y - y / 10) / 192.0 + 92);

            var region = (ushort) ((ySector << 8) | xSector);

            return new LocalPoint(region, x, worldPoint.Z, y);
        }

        public double DistanceTo(LocalPoint point)
        {
            return WorldPoint.FromLocal(this).DistanceTo(WorldPoint.FromLocal(point));
        }
    }
}