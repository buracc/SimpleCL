using SimpleCL.Enums.Commons;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities
{
    public class CharacterPet : Cos
    {
        public CharacterPet(uint id) : base(id)
        {
        }
        
        public void Unsummon()
        {
            var packet = new Packet(Opcode.Agent.Request.COS_UNSUMMON);
            packet.WriteUInt(Uid);
            packet.Send();
        }
    }
}