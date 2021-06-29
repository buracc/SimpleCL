using SilkroadSecurityApi;
using SimpleCL.Enums.Chatter;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Chatter;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game.Chatter
{
    public class ChatService : Service
    {
        [PacketHandler(Opcodes.Agent.Response.CHAT_UPDATE)]
        public void ChatUpdated(Server server, Packet packet)
        {
            ChatChannel channel = (ChatChannel) packet.ReadByte();
            ChatMessage chatMessage = new ChatMessage(channel);

            if (channel == ChatChannel.General || channel == ChatChannel.GM ||
                channel == ChatChannel.NPC)
            {
                uint senderId = packet.ReadUInt();
                chatMessage.SenderId = senderId;
            }
            else
            {
                string senderName = packet.ReadAscii();
                chatMessage.SenderName = senderName;
            }

            string message = packet.ReadUnicode();
            chatMessage.Message = message;

            SimpleCL.Gui.AddChatMessage(chatMessage.ToString());
        }
    }
}