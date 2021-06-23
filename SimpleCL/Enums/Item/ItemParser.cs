using System;
using SimpleCL.Enums.Item.Type;

namespace SimpleCL.Enums.Item
{
    // I really need to find a better way to do this
    public class ItemParser
    {
        public static object GetItemSubType(ItemCategory category, byte typeId3, byte typeId4)
        {
            switch (category)
            {
                case ItemCategory.Equipment:
                    var equipmentType = (Equipment.Type) typeId3;

                    switch (equipmentType)
                    {
                        case Equipment.Type.Garment:
                        case Equipment.Type.Protector:
                        case Equipment.Type.Armor:
                        case Equipment.Type.Robe:
                        case Equipment.Type.LightArmor:
                        case Equipment.Type.HeavyArmor:
                            return (Equipment.SubType.Protector) typeId4;
                        
                        case Equipment.Type.Shield:
                            return (Equipment.SubType.Shield) typeId4;
                        
                        case Equipment.Type.Weapon:
                            return (Equipment.SubType.Weapon) typeId4;
                        
                        case Equipment.Type.Job:
                            return (Equipment.SubType.Job) typeId4;

                        case Equipment.Type.ChineseAccessory:
                        case Equipment.Type.EuropeanAccessory:
                            return (Equipment.SubType.Accessory) typeId4;
                        
                        case Equipment.Type.Avatar:
                            return (Equipment.SubType.Avatar) typeId4;
                        
                        case Equipment.Type.DevilSpirit:
                            return (Equipment.SubType.DevilSpirit) typeId4;
                    }

                    break;

                case ItemCategory.COS:
                    var cosType = (COS.Type) typeId3;

                    switch (cosType)
                    {
                        case COS.Type.Summon:
                            return (COS.SubType.Summon) typeId4;

                        case COS.Type.Mask:
                            return (COS.SubType.Mask) typeId4;
                    }

                    break;

                case ItemCategory.Consumable:
                    var consumableType = (Consumable.Type) typeId3;

                    switch (consumableType)
                    {
                        case Consumable.Type.Potion:
                            return (Consumable.SubType.Potion) typeId4;

                        case Consumable.Type.BadStatus:
                            return (Consumable.SubType.BadStatus) typeId4;

                        case Consumable.Type.Scroll:
                            return (Consumable.SubType.Scroll) typeId4;

                        case Consumable.Type.Ammo:
                            return (Consumable.SubType.Ammo) typeId4;

                        case Consumable.Type.Currency:
                            return (Consumable.SubType.Currency) typeId4;

                        case Consumable.Type.Firework:
                            return (Consumable.SubType.Firework) typeId4;

                        case Consumable.Type.Campfire:
                            return (Consumable.SubType.Campfire) typeId4;

                        case Consumable.Type.TradeGoods:
                            return (Consumable.SubType.TradeGoods) typeId4;

                        case Consumable.Type.Quest:
                            return (Consumable.SubType.Quest) typeId4;

                        case Consumable.Type.Enhancement:
                            return (Consumable.SubType.Enhancement) typeId4;

                        case Consumable.Type.Alchemy:
                            return (Consumable.SubType.Alchemy) typeId4;

                        case Consumable.Type.Etc:
                            return (Consumable.SubType.Etc) typeId4;

                        case Consumable.Type.MallScroll:
                            return (Consumable.SubType.MallScroll) typeId4;

                        case Consumable.Type.MagicPop:
                            return (Consumable.SubType.MagicPop) typeId4;

                        case Consumable.Type.SummonScroll:
                            return (Consumable.SubType.SummonScroll) typeId4;

                        case Consumable.Type.Fellow:
                            return (Consumable.SubType.Fellow) typeId4;
                    }

                    break;

                case ItemCategory.JobEquipment:
                    var jobEquipmentType = (JobEquipment.Type) typeId3;

                    switch (jobEquipmentType)
                    {
                        case JobEquipment.Type.HunterArmor:
                            return (JobEquipment.SubType.Protector) typeId4;

                        case JobEquipment.Type.HunterWeapon:
                            return (JobEquipment.SubType.Weapon) typeId4;
                        
                        case JobEquipment.Type.HunterAccessory:
                            return (JobEquipment.SubType.Accessory) typeId4;
                        
                        case JobEquipment.Type.ThiefArmor:
                            return (JobEquipment.SubType.Protector) typeId4;

                        case JobEquipment.Type.ThiefWeapon:
                            return (JobEquipment.SubType.Weapon) typeId4;
                        
                        case JobEquipment.Type.ThiefAccessory:
                            return (JobEquipment.SubType.Accessory) typeId4;
                    }
                    
                    break;
                
                case ItemCategory.FellowEquipment:
                    return (FellowEquipment.SubType.All) typeId4;
            }

            throw new SystemException(category + ", " + typeId3 +  ", " + typeId4 + " unrecognized item");
        }

        public static object GetItemType(ItemCategory category, byte typeId3)
        {
            switch (category)
            {
                case ItemCategory.Equipment:
                    return (Equipment.Type) typeId3;
                
                case ItemCategory.COS:
                    return (COS.Type) typeId3;
                
                case ItemCategory.Consumable:
                    return (Consumable.Type) typeId3;
                
                case ItemCategory.JobEquipment:
                    return (JobEquipment.Type) typeId3;
                
                case ItemCategory.FellowEquipment:
                    return (FellowEquipment.Type) typeId3;
            }

            throw new SystemException(category + ", " + typeId3 + " unrecognized item");
        }
    }
}