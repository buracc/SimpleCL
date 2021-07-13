using SimpleCL.Models.Items.Equipables;

namespace SimpleCL.Models.Items.JobEquipables
{
    public class JobEquipment : InventoryItem
    {
        public Job JobType
        {
            get
            {
                return TypeId3 switch
                {
                    1 or 2 or 3 => Job.Hunter,
                    4 or 5 or 6 => Job.Thief,
                    _ => Job.Unknown
                };
            }
        }

        public Equipment.EquipmentSlot SlotType
        {
            get
            {
                return TypeId3 switch
                {
                    1 or 4 => TypeId4 switch
                    {
                        1 => Equipment.EquipmentSlot.Head,
                        2 => Equipment.EquipmentSlot.Shoulders,
                        3 => Equipment.EquipmentSlot.Chest,
                        4 => Equipment.EquipmentSlot.Legs,
                        5 => Equipment.EquipmentSlot.Hands,
                        6 => Equipment.EquipmentSlot.Feet,
                        _ => Equipment.EquipmentSlot.Unknown
                    },
                    2 or 5 => Equipment.EquipmentSlot.Weapon,
                    3 or 6 => TypeId4 switch
                    {
                        1 => Equipment.EquipmentSlot.Earring,
                        2 => Equipment.EquipmentSlot.Necklace,
                        3 => Equipment.EquipmentSlot.Ring,
                        _ => Equipment.EquipmentSlot.Unknown
                    },
                    _ => Equipment.EquipmentSlot.Unknown
                };
            }
        }

        public JobEquipment(uint id) : base(id)
        {
        }
        
        public void Equip()
        {
            
        }

        public void UnEquip()
        {
            
        }

        public enum Job
        {
            Hunter,
            Thief,
            Unknown
        }
    }
}