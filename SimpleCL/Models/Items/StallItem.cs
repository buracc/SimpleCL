using System.Drawing;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Items;
using SimpleCL.Interaction;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Models.Items
{
    public class StallItem : InventoryItem
    {
        public ulong Price { get; set; }
        
        public StallItem(uint id) : base(id)
        {
        }

        public void Purchase()
        {
            Program.Gui.Log("Purchasing: " + Name + " for " + Price);
            var buyPacket = new Packet(Opcode.Agent.Request.STALL_BUY);
            buyPacket.WriteByte(Slot);
            buyPacket.Send();
        }
        
        public override string ToString()
        {
            return Quantity + "x " + Name + " [" + Price.ToString("N") + "]";
        }
        
        // todo: this is a temp solution until i find a better way to do this
        public static StallItem ParseItem(Packet packet, Locale locale, bool inventory = true)
        {
            var rentType = packet.ReadUInt();

            switch (rentType)
            {
                case 1:
                    var canDelete = packet.ReadUShort();
                    var beginPeriod = packet.ReadULong();
                    var endPeriod = packet.ReadULong();
                    break;

                case 2:
                    var canDelete2 = packet.ReadUShort();
                    var canRecharge = packet.ReadUShort();
                    var meterRateTime = packet.ReadUInt();
                    break;

                case 3:
                    var canDelete3 = packet.ReadUShort();
                    var canRecharge2 = packet.ReadUShort();
                    var beginPeriod2 = packet.ReadUInt();
                    var endPeriod2 = packet.ReadUInt();
                    var packingTime = packet.ReadUInt();
                    break;
            }

            var refItemId = packet.ReadUInt();
            var inventoryItem = new StallItem(refItemId);

            switch (inventoryItem.Category)
            {
                case ItemCategory.Equipment:
                case ItemCategory.JobEquipment: // job gear
                case ItemCategory.FellowEquipment:
                    var plus = packet.ReadByte();
                    var variance = packet.ReadULong();
                    var dura = packet.ReadUInt();

                    var magicOptions = packet.ReadByte();
                    magicOptions.Repeat(j =>
                    {
                        var paramType = packet.ReadUInt();
                        var paramValue = packet.ReadUInt();
                    });

                    // 1 = sockets
                    packet.ReadByte();
                    var sockets = packet.ReadByte();
                    sockets.Repeat(j =>
                    {
                        var socketSlot = packet.ReadByte();
                        var socketParam = packet.ReadUInt();
                        var socketId = packet.ReadUInt();
                    });

                    // 2 = adv elixirs
                    packet.ReadByte();
                    var advElixirs = packet.ReadByte();
                    advElixirs.Repeat(j =>
                    {
                        var advElixirSlot = packet.ReadByte();
                        var advElixirId = packet.ReadUInt();
                        var advElixirValue = packet.ReadUInt();
                    });

                    if (locale.IsInternational())
                    {
                        // 3 = ??
                        packet.ReadByte();
                        var unk01 = packet.ReadByte();
                        unk01.Repeat(j =>
                        {
                            var unkSlot = packet.ReadByte();
                            var unkParam1 = packet.ReadUInt();
                            var unkParam2 = packet.ReadUInt();
                        });

                        // 4 = ??
                        packet.ReadByte();
                        var unk02 = packet.ReadByte();
                        unk02.Repeat(j =>
                        {
                            var unkSlot = packet.ReadByte();
                            var unkParam1 = packet.ReadUInt();
                            var unkParam2 = packet.ReadUInt();
                        });
                    }

                    break;

                case ItemCategory.Summon:
                    switch (inventoryItem.TypeId3)
                    {
                        case 1:
                            if (inventoryItem.TypeId4 == 3)
                            {
                                var spawnState = packet.ReadByte();
                                if (spawnState == 2)
                                {
                                    var modelId = packet.ReadUInt();
                                    var fellowName = packet.ReadAscii();
                                    var level = packet.ReadByte();
                                    packet.ReadByte();
                                }

                                break;
                            }

                            var state = packet.ReadByte();
                            var refObjId = packet.ReadUInt();
                            var name = packet.ReadAscii();

                            switch (inventoryItem.TypeId4)
                            {
                                case 1:
                                    var level = packet.ReadByte();
                                    break;

                                case 2:
                                    var rentTimeEndSeconds = packet.ReadUInt();
                                    break;
                            }

                            if (locale.IsInternational() && inventory)
                            {
                                packet.ReadByte();
                            }

                            break;

                        case 2:
                            var refObjId2 = packet.ReadUInt();
                            break;

                        case 3:
                            var quantity = packet.ReadUInt();
                            break;
                    }

                    break;

                case ItemCategory.Consumable:
                    var stackCount = packet.ReadUShort();

                    inventoryItem.Quantity = stackCount;

                    if (inventoryItem.TypeId3 == 11 && inventoryItem.TypeId4 is 1 or 2)
                    {
                        var assimilationProb = packet.ReadByte();
                        break;
                    }

                    if (inventoryItem.TypeId3 == 14 && inventoryItem.TypeId4 == 2)
                    {
                        var magParams = packet.ReadByte();
                        magParams.Repeat(j =>
                        {
                            var paramType = packet.ReadUInt();
                            var paramValue = packet.ReadUInt();
                        });
                    }

                    break;
            }

            return inventoryItem;
        }
    }
}