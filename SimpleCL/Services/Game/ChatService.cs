using SimpleCL.Enums.Chatter;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Chatter;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class ChatService : Service
    {
        #region OnChatMessage

        [PacketHandler(Opcode.Agent.Response.CHAT_UPDATE)]
        public void ChatUpdated(Server server, Packet packet)
        {
            var channel = (ChatChannel) packet.ReadByte();
            var chatMessage = new ChatMessage(channel);

            if (channel is ChatChannel.General or ChatChannel.GameMaster or ChatChannel.Npc)
            {
                var senderId = packet.ReadUInt();
                chatMessage.SenderId = senderId;
            }
            else
            {
                var senderName = packet.ReadAscii();
                chatMessage.SenderName = senderName;
            }

            var message = packet.ReadUnicode();
            chatMessage.Message = message;

            Program.Gui.AddChatMessage(chatMessage.ToString());
        }

        #endregion
    }
}