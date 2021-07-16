using SimpleCL.Enums.Commons;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities
{
    public class Cos : Npc
    {
        public Cos(uint id) : base(id)
        {
        }

        public bool IsHorse()
        {
            return TypeId4 == 1;
        }

        public bool IsTransport()
        {
            return TypeId4 == 2;
        }

        public bool IsAttackPet()
        {
            return TypeId4 == 3;
        }

        public bool IsPickPet()
        {
            return TypeId4 == 4;
        }

        public bool IsGuildGuard()
        {
            return TypeId4 == 5;
        }

        public bool IsQuestPet()
        {
            return TypeId4 == 6;
        }

        public bool IsFellowPet()
        {
            return TypeId4 == 9;
        }

        public bool IsRam()
        {
            return TypeId4 == 10;
        }

        public bool IsCatapult()
        {
            return TypeId4 == 11;
        }

        public bool IsCharacterPet()
        {
            return IsAttackPet() || IsFellowPet() || IsPickPet() || IsHorse() || IsTransport();
        }

        public void Terminate()
        {
            if (this is CharacterPet pet)
            {
                pet.Unsummon();
                return;
            }
            
            var packet = new Packet(Opcode.Agent.Request.COS_TERMINATE);
            packet.WriteUInt(Uid);
            packet.Send();
        }
    }
}