using System;
using System.Linq;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Models.Character;
using SimpleCL.Models.Items;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class InventoryService : Service
    {
        private LocalPlayer _localPlayer = LocalPlayer.Get;

        #region Item usage

        [PacketHandler(Opcode.Agent.Response.INVENTORY_ITEM_USE)]
        public void ItemUse(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }

            var slot = packet.ReadByte();
            var quantity = packet.ReadUShort();

            var inventory = _localPlayer.Inventory;
            var changedItem = inventory.FirstOrDefault(x => x.Slot == slot);
            if (changedItem == null)
            {
                return;
            }

            if (quantity == 0)
            {
                inventory.Remove(changedItem);
            }
            else
            {
                changedItem.Quantity = quantity;
            }
        }

        #endregion

        [PacketHandler(Opcode.Agent.Response.INVENTORY_OPERATION)]
        public void InventoryMove(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }

            var type = (InventoryAction) packet.ReadByte();

            Program.Gui.InvokeLater(() =>
            {
                switch (type)
                {
                    case InventoryAction.InventoryToInventory:
                        InventoryToInventory(packet);
                        break;
                    case InventoryAction.AvatarToInventory:
                        AvatarToInventory(packet);
                        break;
                    case InventoryAction.InventoryToAvatar:
                        InventoryToAvatar(packet);
                        break;
                    case InventoryAction.JobToInventory:
                        JobToInventory(packet);
                        break;
                    case InventoryAction.InventoryToJob:
                        InventoryToJob(packet);
                        break;
                }
            });
            
            Program.Gui.InvokeLater(() => { Program.Gui.RefreshInventories(); });
        }

        public void InventoryToInventory(Packet packet)
        {
            var startSlot = packet.ReadByte();
            var endSlot = packet.ReadByte();
            var quantity = packet.ReadUShort();
            var doubleMovement = packet.ReadBool();

            var movedItem = _localPlayer.Inventory.FirstOrDefault(x => x.Slot == startSlot) ??
                            _localPlayer.EquipmentInventory.FirstOrDefault(x => x.Slot == startSlot);

            if (movedItem == null)
            {
                return;
            }

            var itemAtTargetSlot = _localPlayer.Inventory.FirstOrDefault(x => x.Slot == endSlot);
            if (itemAtTargetSlot != null)
            {
                if (itemAtTargetSlot.Id.Equals(movedItem.Id) && itemAtTargetSlot.Quantity < itemAtTargetSlot.Stack)
                {
                    var remainingQuantity = movedItem.Quantity - quantity;
                    if (remainingQuantity <= 0)
                    {
                        if (_localPlayer.Inventory.Remove(movedItem) ||
                            _localPlayer.EquipmentInventory.Remove(movedItem))
                        {
                            movedItem.Dispose();
                        }

                        itemAtTargetSlot.Quantity += quantity;
                        return;
                    }

                    itemAtTargetSlot.Quantity += quantity;
                    movedItem.Quantity -= quantity;
                    return;
                }

                itemAtTargetSlot.Slot = startSlot;
                movedItem.Slot = endSlot;
                return;
            }

            if (quantity < movedItem.Quantity)
            {
                movedItem.Quantity -= quantity;
                var item = InventoryItem.FromId(movedItem.Id);
                item.Quantity = quantity;
                item.Slot = endSlot;
                _localPlayer.Inventory.Add(item);
                return;
            }

            movedItem.Slot = endSlot;
            if (_localPlayer.EquipmentInventory.Contains(movedItem))
            {
                _localPlayer.Inventory.Add(movedItem);
                _localPlayer.EquipmentInventory.Remove(movedItem);
                return;
            }

            if (movedItem.Slot < 13)
            {
                _localPlayer.EquipmentInventory.Add(movedItem);
                _localPlayer.Inventory.Remove(movedItem);
            }
        }

        public void AvatarToInventory(Packet packet)
        {
            var avatarSlot = packet.ReadByte();
            var targetSlot = packet.ReadByte();

            var movedItem = _localPlayer.AvatarInventory.FirstOrDefault(x => x.Slot == avatarSlot);

            if (movedItem == null)
            {
                return;
            }

            movedItem.Slot = targetSlot;
            _localPlayer.Inventory.Add(movedItem);
            _localPlayer.AvatarInventory.Remove(movedItem);
        }

        public void InventoryToAvatar(Packet packet)
        {
            var inventorySlot = packet.ReadByte();
            var targetSlot = packet.ReadByte();

            var movedItem = _localPlayer.Inventory.FirstOrDefault(x => x.Slot == inventorySlot);

            if (movedItem == null)
            {
                return;
            }

            movedItem.Slot = targetSlot;
            _localPlayer.Inventory.Remove(movedItem);
            _localPlayer.AvatarInventory.Add(movedItem);
        }

        public void JobToInventory(Packet packet)
        {
            var jobSlot = packet.ReadByte();
            var targetSlot = packet.ReadByte();

            var movedItem = _localPlayer.JobEquipmentInventory.FirstOrDefault(x => x.Slot == jobSlot);

            if (movedItem == null)
            {
                return;
            }

            movedItem.Slot = targetSlot;
            _localPlayer.JobEquipmentInventory.Remove(movedItem);
            _localPlayer.Inventory.Add(movedItem);
        }

        public void InventoryToJob(Packet packet)
        {
            var invSlot = packet.ReadByte();
            var targetSlot = packet.ReadByte();

            var movedItem = _localPlayer.Inventory.FirstOrDefault(x => x.Slot == invSlot);

            if (movedItem == null)
            {
                return;
            }

            movedItem.Slot = targetSlot;
            _localPlayer.Inventory.Remove(movedItem);
            _localPlayer.JobEquipmentInventory.Add(movedItem);
        }
    }
}