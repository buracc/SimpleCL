namespace SimpleCL.Enums.Events
{
    public enum InventoryAction : byte
    {
        InventoryToInventory = 0,
        StorageToStorage = 1,
        InventoryToStorage = 2,
        StorageToInventory = 3,

        InventoryToExchange = 4,
        ExchangeToInventory = 5,

        GroundToInventory = 6,
        InventoryToGround = 7,

        ShopToInventory = 8,
        InventoryToShop = 9,

        InventoryGoldToGround = 10,
        StorageGoldToInventory = 11,
        InventoryGoldToStorage = 12,
        InventoryGoldToExchange = 13,

        QuestToInventory = 14,
        InventoryToQuest = 15,

        TransportToTransport = 16,
        GroundToPet = 17,
        ShopToTransport = 19,
        TransportToShop = 20,

        PetToPet = 25,
        PetToInventory = 26,
        InventoryToPet = 27,
        GroundToPetToInventory = 28,

        GuildToGuild = 29,
        InventoryToGuild = 30,
        GuildToInventory = 31,
        InventoryGoldToGuild = 32,
        GuildGoldToInventory = 33,

        ShopBuyBack = 34,

        AvatarToInventory = 35,
        InventoryToAvatar = 36,
        
        InventoryToJob = 47,
        JobToInventory = 48,
        
        Unknown = 255
    }
}