using System.Text;
using SimpleCL.Enums;

namespace SimpleCL.Model
{
    public class ChatMessage
    {
        public ChatChannel Channel { get; }
        public string SenderName { get; set; }
        public uint SenderId { get; set; }
        public string Message { get; set; }

        public ChatMessage(ChatChannel channel)
        {
            Channel = channel;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("[" + Channel + "] ");
            if (SenderName != null)
            {
                sb.Append(SenderName);
            }

            if (SenderId != 0)
            {
                sb.Append(SenderId);
            }
            
            return sb.Append(": ").Append(Message).ToString();
        }
    }
}