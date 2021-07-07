using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using SimpleCL.Enums.Skills;

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

        public bool IsAutoTransfer()
        {
            return Attributes.Any(x => x == (uint) SkillData.Attribute.AutoTransferEffect);
        }
    }
}