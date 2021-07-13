using System.Linq;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Models.Character;
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

            switch (type)
            {
                case InventoryAction.InventoryToInventory:
                    InventoryToInventory(packet);
                    break;
                case InventoryAction.StorageToStorage:
                    break;
                case InventoryAction.InventoryToStorage:
                    break;
                case InventoryAction.StorageToInventory:
                    break;
                case InventoryAction.InventoryToExchange:
                    break;
                case InventoryAction.ExchangeToInventory:
                    break;
                case InventoryAction.GroundToInventory:
                    break;
                case InventoryAction.InventoryToGround:
                    break;
                case InventoryAction.ShopToInventory:
                    break;
                case InventoryAction.InventoryToShop:
                    break;
                case InventoryAction.InventoryGoldToGround:
                    break;
                case InventoryAction.StorageGoldToInventory:
                    break;
                case InventoryAction.InventoryGoldToStorage:
                    break;
                case InventoryAction.InventoryGoldToExchange:
                    break;
                case InventoryAction.QuestToInventory:
                    break;
                case InventoryAction.InventoryToQuest:
                    break;
                case InventoryAction.TransportToTransport:
                    break;
                case InventoryAction.GroundToPet:
                    break;
                case InventoryAction.ShopToTransport:
                    break;
                case InventoryAction.TransportToShop:
                    break;
                case InventoryAction.PetToPet:
                    break;
                case InventoryAction.PetToInventory:
                    break;
                case InventoryAction.InventoryToPet:
                    break;
                case InventoryAction.GroundToPetToInventory:
                    break;
                case InventoryAction.GuildToGuild:
                    break;
                case InventoryAction.InventoryToGuild:
                    break;
                case InventoryAction.GuildToInventory:
                    break;
                case InventoryAction.InventoryGoldToGuild:
                    break;
                case InventoryAction.GuildGoldToInventory:
                    break;
                case InventoryAction.ShopBuyBack:
                    break;
                case InventoryAction.AvatarToInventory:
                    break;
                case InventoryAction.InventoryToAvatar:
                    break;
                default:
                    return;
            }
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

            Program.Gui.InvokeLater(() =>
            {
                var itemAtTargetSlot = _localPlayer.Inventory.FirstOrDefault(x => x.Slot == endSlot);
                if (itemAtTargetSlot != null)
                {
                    if (itemAtTargetSlot.Id.Equals(movedItem.Id) && itemAtTargetSlot.Quantity < itemAtTargetSlot.Stack)
                    {
                        var remainingQuantity = quantity + itemAtTargetSlot.Quantity - itemAtTargetSlot.Stack;
                        if (remainingQuantity > 0)
                        {
                            movedItem.Quantity -= (ushort) remainingQuantity;
                            itemAtTargetSlot.Quantity = itemAtTargetSlot.Stack;
                            return;
                        }

                        itemAtTargetSlot.Quantity += quantity;
                        if (_localPlayer.Inventory.Remove(movedItem) || _localPlayer.EquipmentInventory.Remove(movedItem))
                        {
                            movedItem.Dispose();
                        }

                        return;
                    }

                    itemAtTargetSlot.Slot = startSlot;
                    movedItem.Slot = endSlot;
                    return;
                }

                movedItem.Slot = endSlot;
                if (_localPlayer.EquipmentInventory.Contains(movedItem))
                {
                    _localPlayer.Inventory.Add(movedItem);
                    _localPlayer.EquipmentInventory.Remove(movedItem);
                    return;
                }

                _localPlayer.EquipmentInventory.Add(movedItem);
                _localPlayer.Inventory.Remove(movedItem);
            });
        }
    }
}