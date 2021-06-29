using System;
using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Enums.Items
{
    // I really need to find a better way to do this
    public class ItemParser
    {
        public static object GetItemSubType(ItemCategory category, byte typeId3, byte typeId4)
        {
            switch (category)
            {
                case ItemCategory.Equipment:
                    var equipmentType = (EquipmentData.Type) typeId3;

                    switch (equipmentType)
                    {
                        case EquipmentData.Type.Garment:
                        case EquipmentData.Type.Protector:
                        case EquipmentData.Type.Armor:
                        case EquipmentData.Type.Robe:
                        case EquipmentData.Type.LightArmor:
                        case EquipmentData.Type.HeavyArmor:
                            return (EquipmentData.SubType.Protector) typeId4;
                        
                        case EquipmentData.Type.Shield:
                            return (EquipmentData.SubType.Shield) typeId4;
                        
                        case EquipmentData.Type.Weapon:
                            return (EquipmentData.SubType.Weapon) typeId4;
                        
                        case EquipmentData.Type.Job:
                            return (EquipmentData.SubType.Job) typeId4;

                        case EquipmentData.Type.ChineseAccessory:
                        case EquipmentData.Type.EuropeanAccessory:
                            return (EquipmentData.SubType.Accessory) typeId4;
                        
                        case EquipmentData.Type.Avatar:
                            return (EquipmentData.SubType.Avatar) typeId4;
                        
                        case EquipmentData.Type.DevilSpirit:
                            return (EquipmentData.SubType.DevilSpirit) typeId4;
                    }

                    break;

                case ItemCategory.Summon:
                    var cosType = (CosData.Type) typeId3;

                    switch (cosType)
                    {
                        case CosData.Type.Summon:
                            return (CosData.SubType.Summon) typeId4;

                        case CosData.Type.Mask:
                            return (CosData.SubType.Mask) typeId4;
                    }

                    break;

                case ItemCategory.Consumable:
                    var consumableType = (ConsumableData.Type) typeId3;

                    switch (consumableType)
                    {
                        case ConsumableData.Type.Potion:
                            return (ConsumableData.SubType.Potion) typeId4;

                        case ConsumableData.Type.BadStatus:
                            return (ConsumableData.SubType.BadStatus) typeId4;

                        case ConsumableData.Type.Scroll:
                            return (ConsumableData.SubType.Scroll) typeId4;

                        case ConsumableData.Type.Ammo:
                            return (ConsumableData.SubType.Ammo) typeId4;

                        case ConsumableData.Type.Currency:
                            return (ConsumableData.SubType.Currency) typeId4;

                        case ConsumableData.Type.Firework:
                            return (ConsumableData.SubType.Firework) typeId4;

                        case ConsumableData.Type.Campfire:
                            return (ConsumableData.SubType.Campfire) typeId4;

                        case ConsumableData.Type.TradeGoods:
                            return (ConsumableData.SubType.TradeGoods) typeId4;

                        case ConsumableData.Type.Quest:
                            return (ConsumableData.SubType.Quest) typeId4;

                        case ConsumableData.Type.Enhancement:
                            return (ConsumableData.SubType.Enhancement) typeId4;

                        case ConsumableData.Type.Alchemy:
                            return (ConsumableData.SubType.Alchemy) typeId4;

                        case ConsumableData.Type.Etc:
                            return (ConsumableData.SubType.Etc) typeId4;

                        case ConsumableData.Type.MallScroll:
                            return (ConsumableData.SubType.MallScroll) typeId4;

                        case ConsumableData.Type.MagicPop:
                            return (ConsumableData.SubType.MagicPop) typeId4;

                        case ConsumableData.Type.SummonScroll:
                            return (ConsumableData.SubType.SummonScroll) typeId4;

                        case ConsumableData.Type.Fellow:
                            return (ConsumableData.SubType.Fellow) typeId4;
                    }

                    break;

                case ItemCategory.JobEquipment:
                    var jobEquipmentType = (JobEquipmentData.Type) typeId3;

                    switch (jobEquipmentType)
                    {
                        case JobEquipmentData.Type.HunterArmor:
                            return (JobEquipmentData.SubType.Protector) typeId4;

                        case JobEquipmentData.Type.HunterWeapon:
                            return (JobEquipmentData.SubType.Weapon) typeId4;
                        
                        case JobEquipmentData.Type.HunterAccessory:
                            return (JobEquipmentData.SubType.Accessory) typeId4;
                        
                        case JobEquipmentData.Type.ThiefArmor:
                            return (JobEquipmentData.SubType.Protector) typeId4;

                        case JobEquipmentData.Type.ThiefWeapon:
                            return (JobEquipmentData.SubType.Weapon) typeId4;
                        
                        case JobEquipmentData.Type.ThiefAccessory:
                            return (JobEquipmentData.SubType.Accessory) typeId4;
                    }
                    
                    break;
                
                case ItemCategory.FellowEquipment:
                    return (FellowEquipmentData.SubType.All) typeId4;
            }

            throw new SystemException(category + ", " + typeId3 +  ", " + typeId4 + " unrecognized item");
        }

        public static object GetItemType(ItemCategory category, byte typeId3)
        {
            switch (category)
            {
                case ItemCategory.Equipment:
                    return (EquipmentData.Type) typeId3;
                
                case ItemCategory.Summon:
                    return (CosData.Type) typeId3;
                
                case ItemCategory.Consumable:
                    return (ConsumableData.Type) typeId3;
                
                case ItemCategory.JobEquipment:
                    return (JobEquipmentData.Type) typeId3;
                
                case ItemCategory.FellowEquipment:
                    return (FellowEquipmentData.Type) typeId3;
            }

            throw new SystemException(category + ", " + typeId3 + " unrecognized item");
        }
    }
}