using System;
using System.Linq;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Models.Character;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Items.Equipables
{
    public class Equipment : InventoryItem
    {
        public EquipmentSlot SlotType
        {
            get
            {
                return TypeId3 switch
                {
                    5 or 12 => TypeId4 switch
                    {
                        1 => EquipmentSlot.Earring,
                        2 => EquipmentSlot.Necklace,
                        3 => EquipmentSlot.Ring,
                        _ => EquipmentSlot.Unknown
                    },
                    4 => EquipmentSlot.Shield,
                    6 => EquipmentSlot.Weapon,
                    7 => EquipmentSlot.Cape,
                    _ => TypeId4 switch
                    {
                        1 => EquipmentSlot.Head,
                        2 => EquipmentSlot.Shoulders,
                        3 => EquipmentSlot.Chest,
                        4 => EquipmentSlot.Legs,
                        5 => EquipmentSlot.Hands,
                        6 => EquipmentSlot.Feet,
                        _ => EquipmentSlot.Unknown
                    }
                };
            }
        }

        public Type EquipmentType
        {
            get
            {
                if (IsRegular())
                {
                    return Type.Regular;
                }

                return IsAvatar() ? Type.Avatar : Type.Unknown;
            }
        }

        public Equipment(uint id, uint rentTypeId) : base(id, rentTypeId)
        {
        }

        private bool IsRegular()
        {
            return TypeId3 is >= 1 and <= 12;
        }

        private bool IsAvatar()
        {
            return TypeId3 is 13 or 14;
        }

        public void Equip()
        {
            var action = this is Avatar ? InventoryAction.InventoryToAvatar : InventoryAction.InventoryToInventory;
            var secondRing = LocalPlayer.Get.EquipmentInventory.FirstOrDefault(x => x.Slot == 12);
            byte targetSlot = EquipmentType switch
            {
                Type.Regular => SlotType switch
                {
                    EquipmentSlot.Weapon => 6,
                    EquipmentSlot.Shield => 7,
                    EquipmentSlot.Head => 0,
                    EquipmentSlot.Shoulders => 2,
                    EquipmentSlot.Chest => 1,
                    EquipmentSlot.Legs => 4,
                    EquipmentSlot.Hands => 3,
                    EquipmentSlot.Feet => 5,
                    EquipmentSlot.Earring => 9,
                    EquipmentSlot.Necklace => 10,
                    EquipmentSlot.Ring => (byte) (secondRing == null ? 12 : 11),
                    EquipmentSlot.Cape => 8,
                    EquipmentSlot.Unknown or _ => 255
                },
                Type.Avatar => ((Avatar) this).SlotType switch
                {
                    Avatar.EquipmentSlot.Hat => 0,
                    Avatar.EquipmentSlot.Attachment => 2,
                    Avatar.EquipmentSlot.Dress => 1,
                    Avatar.EquipmentSlot.Devil => 4,
                    Avatar.EquipmentSlot.Flag => 3,
                    Avatar.EquipmentSlot.Unknown or _ => 255
                },
                Type.Unknown or _ => 255
            };

            if (targetSlot == 255)
            {
                return;
            }
            
            Move(Slot, targetSlot, action);
        }

        public void UnEquip()
        {
            Move(Slot, 0x0D, this is Avatar ? InventoryAction.AvatarToInventory : InventoryAction.InventoryToInventory);
        }

        public enum Type
        {
            Regular,
            Avatar,
            Unknown
        }

        public enum EquipmentSlot
        {
            Weapon,
            Shield,
            Head,
            Shoulders,
            Chest,
            Legs,
            Hands,
            Feet,
            Earring,
            Necklace,
            Ring,
            Cape,
            Unknown
        }
    }
}