using System;

namespace SimpleCL.Util.Extension
{
    public static class NumberExt
    {
        public static void Repeat(this byte times, Action<int> action)
        {
            for (var i = 0; i < times; i++)
            {
                action(i);
            }
        }

        public static void Repeat(this uint times, Action<int> action)
        {
            for (var i = 0; i < times; i++)
            {
                action(i);
            }
        }

        public static void Repeat(this ushort times, Action<int> action)
        {
            for (var i = 0; i < times; i++)
            {
                action(i);
            }
        }
        
        public static bool HasFlags(this ulong flags, ulong desiredFlags)
        {
            return (flags & desiredFlags) != 0;
        }
        
        public static bool HasFlags(this uint flags, uint desiredFlags)
        {
            return (flags & desiredFlags) != 0;
        }
        
        public static bool HasFlags(this ushort flags, ushort desiredFlags)
        {
            return (flags & desiredFlags) != 0;
        }
        
        public static bool HasFlags(this byte flags, byte desiredFlags)
        {
            return (flags & desiredFlags) != 0;
        }
    }
}