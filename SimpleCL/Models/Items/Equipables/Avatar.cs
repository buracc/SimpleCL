using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Models.Items.Equipables
{
    public class Avatar : Equipment
    {
        public new EquipmentSlot SlotType
        {
            get
            {
                if (TypeId3 == 14)
                {
                    return EquipmentSlot.Devil;
                }

                return (EquipmentSlot) TypeId4;
            }
        }

        public Avatar(uint id, uint rentTypeId) : base(id, rentTypeId)
        {
        }

        public new enum EquipmentSlot
        {
            Devil,
            Hat,
            Dress,
            Attachment,
            Flag,
            Unknown
        }
    }
}