using SilkroadSecurityApi;

namespace SimpleCL.Util.Extension
{
    public static class PacketWriteExt
    {
        public static void WriteByte(this Packet packet, byte value)
        {
            packet.WriteUInt8(value);
        }

        public static void WriteByte(this Packet packet, object value)
        {
            packet.WriteUInt8(value);
        }

        public static void WriteSByte(this Packet packet, sbyte value)
        {
            packet.WriteInt8(value);
        }

        public static void WriteSByte(this Packet packet, object value)
        {
            packet.WriteInt8(value);
        }

        public static void WriteUShort(this Packet packet, ushort value)
        {
            packet.WriteUInt16(value);
        }

        public static void WriteUShort(this Packet packet, object value)
        {
            packet.WriteUInt16(value);
        }

        public static void WriteShort(this Packet packet, short value)
        {
            packet.WriteInt16(value);
        }

        public static void WriteShort(this Packet packet, object value)
        {
            packet.WriteInt16(value);
        }

        public static void WriteUInt(this Packet packet, uint value)
        {
            packet.WriteUInt32(value);
        }

        public static void WriteUInt(this Packet packet, object value)
        {
            packet.WriteUInt32(value);
        }

        public static void WriteInt(this Packet packet, int value)
        {
            packet.WriteInt32(value);
        }

        public static void WriteInt(this Packet packet, object value)
        {
            packet.WriteInt32(value);
        }

        public static void WriteFloat(this Packet packet, float value)
        {
            packet.WriteSingle(value);
        }

        public static void WriteFloat(this Packet packet, object value)
        {
            packet.WriteSingle(value);
        }

        public static void WriteULong(this Packet packet, ulong value)
        {
            packet.WriteUInt64(value);
        }

        public static void WriteULong(this Packet packet, object value)
        {
            packet.WriteUInt64(value);
        }

        public static void WriteLong(this Packet packet, long value)
        {
            packet.WriteInt64(value);
        }

        public static void WriteLong(this Packet packet, object value)
        {
            packet.WriteInt64(value);
        }

        public static void WriteByteArray(this Packet packet, byte[] value)
        {
            packet.WriteUInt8Array(value);
        }

         public static void WriteByteArray(this Packet packet, object[] value)
        {
            packet.WriteUInt8Array(value);
        }

        public static void WriteSByteArray(this Packet packet, object[] value)
        {
            packet.WriteInt8Array(value);
        }

        public static void WriteUShortArray(this Packet packet, ushort[] value)
        {
            packet.WriteUInt16Array(value);
        }

        public static void WriteUShortArray(this Packet packet, object[] value)
        {
            packet.WriteUInt16Array(value);
        }

        public static void WriteShortArray(this Packet packet, short[] value)
        {
            packet.WriteInt16Array(value);
        }

        public static void WriteShortArray(this Packet packet, object[] value)
        {
            packet.WriteInt16Array(value);
        }

        public static void WriteUIntArray(this Packet packet, uint[] value)
        {
            packet.WriteUInt32Array(value);
        }

        public static void WriteUIntArray(this Packet packet, object[] value)
        {
            packet.WriteUInt32Array(value);
        }

        public static void WriteIntArray(this Packet packet, int[] value)
        {
            packet.WriteInt32Array(value);
        }

        public static void WriteIntArray(this Packet packet, object[] value)
        {
            packet.WriteInt32Array(value);
        }

        public static void WriteFloatArray(this Packet packet, float[] value)
        {
            packet.WriteSingleArray(value);
        }

        public static void WriteFloatArray(this Packet packet, object[] value)
        {
            packet.WriteSingleArray(value);
        }

        public static void WriteULongArray(this Packet packet, ulong[] value)
        {
            packet.WriteUInt64Array(value);
        }

        public static void WriteULongArray(this Packet packet, object[] value)
        {
            packet.WriteUInt64Array(value);
        }

        public static void WriteLongArray(this Packet packet, long[] value)
        {
            packet.WriteInt64Array(value);
        }
        
        public static void WriteLongArray(this Packet packet, object[] value)
        {
            packet.WriteInt64Array(value);
        }
    }
}