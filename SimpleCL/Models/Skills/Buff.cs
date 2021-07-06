using System;
using System.ComponentModel;

namespace SimpleCL.Models.Skills
{
    public class Buff : Skill
    {
        public string BuffName => Name;
        [Browsable(false)]
        public uint Uid { get; set; }
        [Browsable(false)]
        public uint TargetUid { get; set; }
        [Browsable(false)]
        public uint CasterUid { get; set; }
        [Browsable(false)]
        public readonly long AddedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        
        [Browsable(false)]
        public uint RemainingDuration { get; set; }
        // public DateTimeOffset EndTimeStamp => DateTimeOffset.FromUnixTimeMilliseconds(AddedAt + RemainingDuration);

        public Buff(uint id) : base(id)
        {
        }
    }
}