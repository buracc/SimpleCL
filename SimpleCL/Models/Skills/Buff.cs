namespace SimpleCL.Models.Skills
{
    public class Buff : Skill
    {
        public uint Uid { get; set; }
        public uint TargetUid { get; set; }
        public uint CasterUid { get; set; }
        public uint RemainingDuration { get; set; }
        
        public Buff(uint id) : base(id)
        {
        }
    }
}