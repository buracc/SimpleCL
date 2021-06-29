using System;

namespace SimpleCL.Services
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketHandlerAttribute : Attribute {
        public readonly ushort Opcode;

        public PacketHandlerAttribute(ushort opcode) {
            Opcode = opcode;
        }
    }
}