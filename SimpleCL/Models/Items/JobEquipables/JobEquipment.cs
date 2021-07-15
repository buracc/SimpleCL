using System;
using System.Linq;
using SimpleCL.Enums.Events;
using SimpleCL.Models.Character;
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
            var secondRing = LocalPlayer.Get.JobEquipmentInventory.FirstOrDefault(x => x.Slot == 10);

            byte targetSlot = SlotType switch
            {
                Equipment.EquipmentSlot.Weapon => 6,
                Equipment.EquipmentSlot.Head => 0,
                Equipment.EquipmentSlot.Shoulders => 2,
                Equipment.EquipmentSlot.Chest => 1,
                Equipment.EquipmentSlot.Legs => 4,
                Equipment.EquipmentSlot.Hands => 3,
                Equipment.EquipmentSlot.Feet => 5,
                Equipment.EquipmentSlot.Earring => 7,
                Equipment.EquipmentSlot.Necklace => 8,
                Equipment.EquipmentSlot.Ring => (byte) (secondRing == null ? 10 : 9),
                Equipment.EquipmentSlot.Cape => 8,
                Equipment.EquipmentSlot.Unknown or _ => 255
            };

            if (targetSlot == 255)
            {
                return;
            }
            
            Move(Slot, targetSlot, InventoryAction.InventoryToJob);
        }

        public void UnEquip()
        {
            Move(Slot, 0x0D, InventoryAction.JobToInventory);
        }

        public enum Job
        {
            Hunter,
            Thief,
            Unknown
        }
    }
}