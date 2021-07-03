using SimpleCL.Models.Skills;

namespace SimpleCL.Models.Entities
{
    public class SkillAoe : Entity
    {
        public Skill Skill { get; set; }
        public SkillAoe(uint id) : base(id)
        {
        }
    }
}