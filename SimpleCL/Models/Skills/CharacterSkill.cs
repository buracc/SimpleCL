namespace SimpleCL.Models.Skills
{
    public class CharacterSkill : Skill
    {
        public bool Enabled { get; set; }
        public CharacterSkill(uint id) : base(id)
        {
        }
    }
}