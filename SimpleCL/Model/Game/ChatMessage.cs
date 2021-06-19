using System.Text;

namespace SimpleCL.Model.Game
{
    public class ChatMessage
    {
        public ChatChannel Channel { get; }
        public string SenderName { get; set; }
        public uint SenderID { get; set; }
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

            if (SenderID != 0)
            {
                sb.Append(SenderID);
            }
            
            return sb.Append(": ").Append(Message).ToString();
        }
    }
}