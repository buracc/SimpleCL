using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Server;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Exchange;
using SimpleCL.Models.Items;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Ui;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class StallService : Service
    {
        private readonly SilkroadServer _silkroadServer;

        public StallService(SilkroadServer silkroadServer)
        {
            _silkroadServer = silkroadServer;
        }

        #region Opened

        [PacketHandler(Opcodes.Agent.Response.STALL_TALK)]
        public void StallOpen(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }

            var playerUid = packet.ReadUInt();
            if (!Entities.AllEntities.TryGetValue(playerUid, out var entity)
                || entity is not Player player
                || player.Stall == null)
            {
                return;
            }

            player.Stall.Description = packet.ReadUnicode();
            player.Stall.Opened = packet.ReadBool();
            var mode = packet.ReadByte();

            while (packet.ReadByte() != byte.MaxValue)
            {
                var stallItem = new StallItem();
                var item = ParseStallItem(packet, _silkroadServer.Locale);
                if (item == null)
                {
                    return;
                }

                stallItem.Item = item;
                stallItem.Slot = packet.ReadByte();
                stallItem.Quantity = packet.ReadUShort();
                stallItem.Price = packet.ReadULong();
                player.Stall.Items.Add(stallItem);
            }

            Application.Run(new StallWindow(player));
        }

        #endregion

        [PacketHandler(Opcodes.Agent.Response.STALL_ENTITY_CREATE)]
        public void StallCreate(Server server, Packet packet)
        {
            if (!Entities.AllEntities.TryGetValue(packet.ReadUInt(), out var entity) || entity is not Player player)
            {
                return;
            }
        
            var stall = new Stall
            {
                Title = packet.ReadUnicode()
            };
        
            player.Stall = stall;
            player.InteractionType = Player.Interaction.OnStall;
            Program.Gui.RefreshPlayerMarker(player.Uid);
        }

        #region Destroy

        [PacketHandler(Opcodes.Agent.Response.STALL_ENTITY_DESTROY)]
        public void StallDestroy(Server server, Packet packet)
        {
            if (!Entities.AllEntities.TryGetValue(packet.ReadUInt(), out var entity) || entity is not Player player)
            {
                return;
            }
        
            player.Stall = null;
            Program.Gui.RefreshPlayerMarker(player.Uid);
        }

        #endregion

        #region Utility

        public static InventoryItem ParseStallItem(Packet packet, Locale locale)
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
            var inventoryItem = InventoryItem.FromId(refItemId);

            switch (inventoryItem.TypeId2)
            {
                case 1:
                case 4: // job gear
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
                        var socketId = packet.ReadUInt();
                        var socketParam = packet.ReadByte();
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

                case 2:
                    switch (inventoryItem.TypeId3)
                    {
                        case 1:
                            var state = packet.ReadByte();
                            var refObjId = packet.ReadUInt();
                            var name = packet.ReadAscii();

                            if (inventoryItem.TypeId4 == 2)
                            {
                                var rentTimeEndSeconds = packet.ReadUInt();
                            }

                            if (locale.IsInternational())
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

                case 3:
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

        #endregion
    }
}