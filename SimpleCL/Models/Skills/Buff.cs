using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using SimpleCL.Enums;
using SimpleCL.Enums.Skills;

namespace SimpleCL.Models.Skills
{
    public class Buff : Skill, IDisposable
    {
        public Image Icon { get; set; }
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
            Icon = Image.FromFile(Directory.GetCurrentDirectory() + Constants.Paths.Icons +
                                  IconPath.Replace(Constants.Strings.Ddj, Constants.Strings.Png));
        }

        public bool IsRecoveryDivision()
        {
            return Attributes.Contains(SkillData.SkillParam.Timed) 
                   && Attributes.Contains(SkillData.SkillParam.OverTime)
                   && Attributes.Contains(SkillData.SkillParam.AreaEffect)
                   && Attributes.Contains(SkillData.SkillParam.HpMpRecovery)
                   && Attributes.Contains(SkillData.SkillParam.HealWeaponReflect);
        }

        public bool IsBardAreaBuff()
        {
            return Attributes.Contains(SkillData.SkillParam.ActiveMpConsumed) 
                   && Attributes.Contains(SkillData.SkillParam.AreaEffect)
                   && Attributes.Contains(SkillData.SkillParam.RequiredItem);
        }

        public bool IsTimed()
        {
            return Attributes.Contains(SkillData.SkillParam.Timed);
        }

        public void Dispose()
        {
            Icon?.Dispose();
        }

        ~Buff()
        {
            Dispose();
        }
    }
}