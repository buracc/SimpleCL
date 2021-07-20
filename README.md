# SimpleCL
An attempt at creating a simple clientless login framework for Silkroad Online, using SilkroadSecurityApi. At the same time becoming familiar with C#.

Preview:
- ![Preview](https://i.imgur.com/V9Q4rXt.png)
- ![Preview](https://i.imgur.com/ExOLUUr.png)
- ![Preview](https://i.imgur.com/KgociE2.png)
- ![Preview](https://i.imgur.com/VUaEjor.png)

# Current features:
- Attaching sro_client
- Login
- Server selection
- Passcode entry
- Character selection
- Display and update character information
- Buffs display and cancelling buffs
- Inventory
    - Use consumables (Potions, scrolls, pets)
    - (Un)Equip equipment
    - Move items to slot
    - Split stackables
- Display in-game chat
- Attack nearby entities (& with skills)
- Minimap
    - Show players (+ special icon for dead players), monster, shops, npc, pets, teleporters, stalls
    
    Right click menus:
    - Local player: Cast self buff
    - Players: Trace, Attack with skill, Cast buff, Resurrect dead player with skill
    - Stalls: Open/Close stall
    - Shops: Open/Close shop
    - Monsters: Attack with skill
    - Summons/COS: Unsummon
    - Teleporters: Use teleporter
- View stalls and shops, purchase items
- Simple packet logger

# Credits:
- [JellyBitz](https://github.com/JellyBitz), his xBot project is a great, helpful resource. Check it out.
- [zefiers](https://github.com/zefiers), his Silkroad.Net project has helped me understand the basics.
- And of course Pushedx for providing the SilkroadSecurityApi.