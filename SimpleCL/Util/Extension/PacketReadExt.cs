using SilkroadSecurityApi;

namespace SimpleCL.Util.Extension
{
    public static class PacketReadExt
    {
        public static byte ReadByte(this Packet packet)
        {
            return packet.ReadUInt8();
        }
        
        public static sbyte ReadSByte(this Packet packet)
        {
            return packet.ReadInt8();
        }
        
        public static ushort ReadUShort(this Packet packet)
        {
            return packet.ReadUInt16();
        }
        
        public static short ReadShort(this Packet packet)
        {
            return packet.ReadInt16();
        }
        
        public static uint ReadUInt(this Packet packet)
        {
            return packet.ReadUInt32();
        }
        
        public static int ReadInt(this Packet packet)
        {
            return packet.ReadInt32();
        }
        
        public static float ReadFloat(this Packet packet)
        {
            return packet.ReadSingle();
        }

        public static ulong ReadULong(this Packet packet)
        {
            return packet.ReadUInt64();
        }
        
        public static long ReadLong(this Packet packet)
        {
            return packet.ReadInt64();
        }
        
        public static byte[] ReadByteArray(this Packet packet, int count)
        {
            return packet.ReadUInt8Array(count);
        }
        
        public static sbyte[] ReadSByteArray(this Packet packet, int count)
        {
            return packet.ReadInt8Array(count);
        }

        public static ushort[] ReadUShortArray(this Packet packet, int count)
        {
            return packet.ReadUInt16Array(count);
        }
        
        public static short[] ReadShortArray(this Packet packet, int count)
        {
            return packet.ReadInt16Array(count);
        }
        
        public static uint[] ReadUIntArray(this Packet packet, int count)
        {
            return packet.ReadUInt32Array(count);
        }
        
        public static int[] ReadIntArray(this Packet packet, int count)
        {
            return packet.ReadInt32Array(count);
        }
        
        public static float[] ReadFloatArray(this Packet packet, int count)
        {
            return packet.ReadSingleArray(count);
        }
        
        public static ulong[] ReadULongArray(this Packet packet, int count)
        {
            return packet.ReadUInt64Array(count);
        }
        
        public static long[] ReadLongArray(this Packet packet, int count)
        {
            return packet.ReadInt64Array(count);
        }
    }
}