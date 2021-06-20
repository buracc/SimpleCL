using System;

namespace SimpleCL.Service
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketHandlerAttribute : Attribute {
        public readonly ushort Opcode;

        public PacketHandlerAttribute(ushort opcode) {
            this.Opcode = opcode;
        }
    }
}