using System;

namespace SimpleCL.Util.Extension
{
    public static class NumberExt
    {
        public static void Repeat(this byte times, Action<int> action)
        {
            for (int i = 0; i < times; i++)
            {
                action(i);
            }
        }

        public static void Repeat(this uint times, Action<int> action)
        {
            for (int i = 0; i < times; i++)
            {
                action(i);
            }
        }

        public static void Repeat(this ushort times, Action<int> action)
        {
            for (int i = 0; i < times; i++)
            {
                action(i);
            }
        }
    }
}