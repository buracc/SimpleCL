using System;
using System.Threading.Tasks;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Entities;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Ui;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class NpcService : Service
    {
        private ShopWindow ShopWindow { get; set; }
        public static Shop SelectedShop = null;

        [PacketHandler(Opcode.Agent.Response.ENTITY_SELECT_OBJECT)]
        public void NpcSelect(Server server, Packet packet)
        {
            while (SelectedShop == null)
            {
            }
            
            SelectedShop.Open();
        }
        
        [PacketHandler(Opcode.Agent.Response.ENTITY_NPC_OPEN)]
        public void NpcOpen(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }
            
            while (SelectedShop == null)
            {
            }

            Task.Run(() =>
            {
                Program.Gui.InvokeLater(() =>
                {
                    ShopWindow = new ShopWindow(SelectedShop);
                    ShopWindow.ShowDialog();
                });
            });
        }
        
        [PacketHandler(Opcode.Agent.Response.ENTITY_NPC_CLOSE)]
        public void NpcClose(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }
            
            ShopWindow?.InvokeLater(() => ShopWindow?.Close());
        }
    }
}