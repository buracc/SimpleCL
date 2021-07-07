using System.Collections.Generic;

namespace SimpleCL.Models.Entities.Exchange
{
    public class Stall
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ulong PlayerUid { get; set; }
        public readonly List<StallItem> Items = new();

        public void Visit()
        {
            
        }
    }
}