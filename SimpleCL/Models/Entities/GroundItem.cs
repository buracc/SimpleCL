using SimpleCL.Enums.Items;

namespace SimpleCL.Models.Entities
{
    public class GroundItem : Entity
    {
        public GroundItem(uint id) : base(id)
        {
        }

        public bool IsEquipment()
        {
            return TypeId2 == (byte) ItemCategory.Equipment;
        }

        public bool IsSummon()
        {
            return TypeId2 == (byte) ItemCategory.Summon;
        }

        public bool IsConsumable()
        {
            return TypeId2 == (byte) ItemCategory.Consumable;
        }

        public bool IsJobEquipment()
        {
            return TypeId2 == (byte) ItemCategory.JobEquipment;
        }

        public bool IsFellowEquipment()
        {
            return TypeId2 == (byte) ItemCategory.FellowEquipment;
        }

        public bool IsGold()
        {
            return IsConsumable() && TypeId3 == 5 && TypeId4 == 0;
        }

        public bool IsTradeGoods()
        {
            return TypeId3 == 8;
        }

        public bool IsQuest()
        {
            return IsConsumable() && TypeId3 == 9;
        }
    }
}