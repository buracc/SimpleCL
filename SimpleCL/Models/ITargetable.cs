using SimpleCL.Models.Skills;

namespace SimpleCL.Models
{
    public interface ITargetable
    {
        void Attack(Skill skill);
    }
}