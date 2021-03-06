﻿using System.Collections.Generic;
using static RaylibWindowNamespace.Objects;

namespace CRPGNamespace
{
    //IDs for all of the stuff in the world
    #region Object IDs
    public enum ItemIDs
    {
        STICK = 1 << 0,
        CLOTHES = 1 << 1,
        FANG = 1 << 2,
        MERMAIDSPEAR = 1 << 3,
        BANDITGARB = 1 << 4,
        LESSERHEALINGPOTION = 1 << 5,
        HEALINGPOTION = 1 << 6,
        BERRIES = 1 << 7
    }
    public enum NPCIDs
    {
        STEVE,
        KVORKYSINN,
        KVORKYSSHOP,
        KVORKYSFOLKS
    }
    public enum MonsterIDs
    {
        WOLF,
        MERMAID,
        LOOTER,
        TROLL,
        BANDITS
    }
    public enum LocationIDs
    {
        CLEARING = 1 << 0,
        PATH = 1 << 1,
        BUSHES = 1 << 2,
        LAKE = 1 << 3,
        FORESTEDGE = 1 << 4,
        PAVEDROAD = 1 << 5,
        TOWNENTRANCE = 1 << 6,
        TOWNSHOP = 1 << 7,
        TOWNINN = 1 << 8,
        TOWNSQUARE = 1 << 9,
        ROADNORTH = 1 << 10,
        ROAD = 1 << 11,
        ROADTOCITY = 1 << 12
    }
    public enum QuestIDs
    {
        TUTORIALQUEST = 1 << 0,
        MAINQUEST = 1 << 1
    }
    #endregion
    /// <summary>
    /// Stores all CRPG related objects
    /// </summary>
    static class World
    {
        //Lists with all of the stuff in the world: Items, NPCs, Monsters, Locations, Quests
        #region Object Lists
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<NPC> NPCs = new List<NPC>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Quest> Quests = new List<Quest>();
        #endregion


        /// <summary>
        /// World constructor, never called just initializes everything
        /// </summary>
        static World()
        {
            PopulateItems();
            PopulateQuests();
            PopulateNPCs();
            PopulateMonsters();
            PopulateLocations();
        }
        /// <summary>
        /// Same as wolrd constructor, called on load to reset and cleanup world
        /// </summary>
        public static void Reload()
        {
            PopulateItems();
            PopulateQuests();
            PopulateNPCs();
            PopulateMonsters();
            PopulateLocations();
        }

        //Populates all of the Lists with their respective stuff
        #region Population
        /// <summary>
        /// Populates all of the Items
        /// </summary>
        private static void PopulateItems()
        {
            Items.Clear();
            //Parameters: Name,Plural Name,Description,Weight(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Items
            //Weapons: Parameters, Weapon Attack
            //Armor: Parameters, AC
            Items.Add(new Weapon((int)ItemIDs.STICK, "Stick", "Sticks", "Long narrow stick with a stick like texture", 3, stickAttack));
            Items.Add(new Armor((int)ItemIDs.CLOTHES, "Clothes", "Clothes", "Some pretty normal clothes, without these you'd be naked!", 5, 2));
            Items.Add(new Item((int)ItemIDs.FANG, "Wolf Fang", "Wolf Fangs", "The fang of a wolf, pretty useless", 6));
            Items.Add(new Weapon((int)ItemIDs.MERMAIDSPEAR, "Mermaid Spear", "Mermaid Spears", "A simple yet elegant spear", 7, mermaidSpearAttack));
            Items.Add(new Armor((int)ItemIDs.BANDITGARB, "Bandit Garb", "Bandit Garb", "A simple set of thrown together armor", 10, 5));
            Items.Add(new HealingPotion((int)ItemIDs.LESSERHEALINGPOTION, "Lesser Healing Potion", "Lesser Healing Potions", "A cheap healing potion", 25, 8));
            Items.Add(new HealingPotion((int)ItemIDs.HEALINGPOTION, "Healing Potion", "Healing Potions", "A decent healing potion", 50, 18));
            Items.Add(new HealingPotion((int)ItemIDs.BERRIES, "Berry", "Berries", "Some strange berries, whne did they get in your pocket?", 1, 2));
        }

        /// <summary>
        /// Populates all of the NPCs
        /// </summary>
        private static void PopulateNPCs()
        {
            NPCs.Clear();
            //Parameters: Name, Talk Line, Description
            //QuestNPC: Parameters, post quest line, relating quest, objective marker
            //QueryNPC: Parameters, Question to ask
            //Inn: QueryNPC, Cost to stay at the inn
            //shop: QueryNPC, Price augement
            NPCs.Add(new NPC(new Name("Steve"), "'ello, welcome to Kvorkys", "The entrance guard to Kvorkys", false, true));
            NPCs.Add(new Inn(new Name("Mileena"), "'ello", "Keeper of the town inn", "Would you like to rent a room for the night?", 10, false, true));
            Shop kvorkysShop = new Shop(new Name("Markus"), "'ello", "Owner of the town shop", "Buyin' or sellin'?", 1.02f, false, true);
            kvorkysShop.stock.Add(new InventoryItem(ItemByID((int)ItemIDs.LESSERHEALINGPOTION), 5));
            kvorkysShop.stock.Add(new InventoryItem(ItemByID((int)ItemIDs.HEALINGPOTION), 5));
            kvorkysShop.SortByPrice();
            NPCs.Add(kvorkysShop);
            NPCs.Add(new QuestNPC(new Name("Townsfolk"), "'ello", "was there anything else you needed?", "Townsfolk of Kvorkys, hangin' out in the town square", QuestByID((int)QuestIDs.TUTORIALQUEST), 2, true));
        }

        /// <summary>
        /// Populates all of the Monsters
        /// </summary>
        private static void PopulateMonsters()
        {
            Monsters.Clear();
            //Parameters: Name, Description, Health, Max Damage, Min Damage, Reward XP, Reward Gold
            //QuestMonster: Parameters, relating quest, objective marker it calls
            Monster wolf = new Monster(new Name("Wolf"), "A lone wolf prowling", 12, WolfAttack, 11, 5)
            {
                lootTable = new LootItem[] { new LootItem(ItemByID((int)ItemIDs.FANG), 100, true) }
            };
            Monster mermaid = new Monster(new Name("Mermaid"), "a mermaid sitting on the edge of the lake", 15, mermaidAttack, 23, 3)
            {
                lootTable = new LootItem[] { new LootItem(ItemByID((int)ItemIDs.MERMAIDSPEAR), 100, true) }
            };
            Monster looter = new Monster(new Name("Looter"), "A looter hiding in the bushes", 20, looterAttack, 18, 7)
            {
                lootTable = new LootItem[] { new LootItem(ItemByID((int)ItemIDs.BANDITGARB), 100, true) }
            };
            QuestMonster troll = new QuestMonster(new Name("Troll"), "A troll trying to attack the town", 30, trollAttack, 40, 10, QuestByID((int)QuestIDs.TUTORIALQUEST), 1)
            {
                lootTable = new LootItem[] { }
            };
            Monster bandits = new Monster(new Name("Bandits"), "A pair of bandits, you don't feel like giving them your money", 32, banditsAttack, 12, 25, false, true)
            {
                lootTable = new LootItem[] { }
            };

            Monsters.Add(wolf);
            Monsters.Add(mermaid);
            Monsters.Add(looter);
            Monsters.Add(troll);
            Monsters.Add(bandits);
        }

        /// <summary>
        /// Populates all of the Locations
        /// </summary>
        private static void PopulateLocations()
        {
            Locations.Clear();
            //Parameters: Name, Description
            //QuestLocation: Parameters, relating quest, objective marker to be called
            //LockedLocation: Parameters, Location Index, Locked text
            QuestLocation clearing = new QuestLocation((int)LocationIDs.CLEARING, "Clearing", "A small clearing, forest surrounds you", QuestByID((int)QuestIDs.TUTORIALQUEST), -1)
            {
                monsterLivingHere = MonsterByID((int)MonsterIDs.WOLF)
            };
            Location path = new Location((int)LocationIDs.PATH, "Path", "A small path from the clearing, where does it lead?");
            Location bushes = new Location((int)LocationIDs.BUSHES, "Rustling Bushes", "Some rustling bushes", false, true)
            {
                monsterLivingHere = MonsterByID((int)MonsterIDs.LOOTER)
            };
            Location smallLake = new Location((int)LocationIDs.LAKE, "Lake", "A small lake, seems peaceful")
            {
                monsterLivingHere = MonsterByID((int)MonsterIDs.MERMAID)
            };
            LockedLocation forestEdge = new LockedLocation((int)LocationIDs.FORESTEDGE, "Forest Edge", "You can see the forest give way to what looks like grasslands", LockedLocationIndex.FORESTEDGE, "Something tells you, you need to be stronger before you can move on");
            QuestLocation pavedRoad = new QuestLocation((int)LocationIDs.PAVEDROAD, "Paved Road", "A paved road the first sign of civilization", QuestByID((int)QuestIDs.TUTORIALQUEST), 0)
            {
                monsterLivingHere = MonsterByID((int)MonsterIDs.TROLL)
            };
            Location townEntrance = new Location((int)LocationIDs.TOWNENTRANCE, "Town Entrance", "Entrance to the town of Kvorkys")
            {
                npcLivingHere = NPCByID((int)NPCIDs.STEVE)
            };
            Location townShop = new Location((int)LocationIDs.TOWNSHOP, "Town Shop", "Town's gneral store")
            {
                npcLivingHere = NPCByID((int)NPCIDs.KVORKYSSHOP)
            };
            Location townInn = new Location((int)LocationIDs.TOWNINN, "Town Inn", "Town's Inn")
            {
                npcLivingHere = NPCByID((int)NPCIDs.KVORKYSINN)
            };
            Location townSquare = new Location((int)LocationIDs.TOWNSQUARE, "Town Square", "The Town Square, where a lot of people spend their time")
            {
                npcLivingHere = NPCByID((int)NPCIDs.KVORKYSFOLKS)
            };
            LockedLocation roadNorth = new LockedLocation((int)LocationIDs.ROADNORTH, "Road North", "A road leading north", LockedLocationIndex.NORTH, "You should head to the town first");
            NestLocation road = new NestLocation((int)LocationIDs.ROAD, "Road", "The road that connects Kvorkys to the capital")
            {
                monsterToLiveHere = MonsterByID((int)MonsterIDs.BANDITS)
            };
            Location roadToTheCapital = new Location((int)LocationIDs.ROADTOCITY, "Road to the Capital", "The road which leads to the capital");


            #region Intro Forest
            clearing.locationToNorth = path;

            path.locationToNorth = forestEdge;
            path.locationToEast = bushes;
            path.locationToWest = smallLake;
            path.locationToSouth = clearing;

            bushes.locationToWest = path;

            smallLake.locationToEast = path;

            forestEdge.locationToSouth = path;
            forestEdge.locationToNorth = pavedRoad;
            #endregion

            pavedRoad.locationToNorth = roadNorth;
            pavedRoad.locationToEast = townEntrance;

            #region Kvorkys Town
            townEntrance.locationToWest = pavedRoad;
            townEntrance.locationToNorth = townShop;
            townEntrance.locationToSouth = townInn;
            townEntrance.locationToEast = townSquare;

            townShop.locationToSouth = townEntrance;

            townInn.locationToNorth = townEntrance;

            townSquare.locationToWest = townEntrance;
            #endregion

            #region Road
            roadNorth.locationToSouth = pavedRoad;
            roadNorth.locationToNorth = road;

            road.locationToSouth = roadNorth;
            road.locationToNorth = roadToTheCapital;


            roadToTheCapital.locationToSouth = road;
            #endregion

            Locations.Add(clearing);
            Locations.Add(path);
            Locations.Add(bushes);
            Locations.Add(smallLake);
            Locations.Add(forestEdge);
            Locations.Add(pavedRoad);
            Locations.Add(townEntrance);
            Locations.Add(townShop);
            Locations.Add(townInn);
            Locations.Add(townSquare);
            Locations.Add(roadNorth);
            Locations.Add(road);
            Locations.Add(roadToTheCapital);
        }

        /// <summary>
        /// Populates all of the Quests
        /// </summary>
        public static void PopulateQuests()
        {
            Quests.Clear();
            //Parameters: Name, Description, Objectives, Reward Gold, Reward XP, Text played when quest gained, Text played when quest completed, quest following current quest-presumed null, Whther quest is main quest-presumed false, Quest complete-presumed false

            Quest tutorialQuest = new Quest((int)QuestIDs.TUTORIALQUEST, "Lost in the Forest!", "You've no idea where you are, you should start looking around for anything familiar", 7, 20, "You wake up in a clearing, where is this place? You should figure out where you are.", "well, looks like it's time to adventure")
            {
                objectives = new Objective[]
            {
                new Objective("Figure out where you are", 0, "As you walk along the road you see a city being attacked by a troll!\nDefeat the troll and protect the villagers!"),
                new Objective("Defeat the troll", 1, "With the troll defeated the town is now safe, perhaps there you can find answers"),
                new Objective("Talk with the townsfolk", 2, "\"You're lost? Oh well this is Kvorkys, a small town in the southern portion of Bjork.\nYou may be able to find out more at the capital, if you head out of the town you should see a road north,\nthat'll lead you straight there\"")
            }
            };

            Quest mainQuest = new Quest((int)QuestIDs.MAINQUEST, "To the Capital!", "In order to figure out more about where you are and where you're going you need to head to the capital", 25, 56, "Alright, now you have a location, head out on a journey to reach the capital", "", true)
            {
                objectives = new Objective[]
            {
                new Objective("Make your way to the capital", 0, "")
            }
            };

            tutorialQuest.followUpQuest = mainQuest;

            Quests.Add(tutorialQuest);
            Quests.Add(mainQuest);
        }
        #endregion

        //Methods for Lists to get their respective things by their IDs
        #region GetByID
        /// <summary>
        /// Gets an Item with the specified ID
        /// </summary>
        /// <param name="ID">ID of the Item you want</param>
        /// <returns>Returns an Item of a specified ID if there is one</returns>
        public static Item ItemByID(int ID)
        {
            for (int x = 0; x < Items.Count; x++)
            {
                if (Items[x].ID == ID)
                {
                    return Items[x];
                }
            }
            Utils.Add("Invalid Item ID");
            return null;
        }

        /// <summary>
        /// Gets an NPC with the specified ID
        /// </summary>
        /// <param name="ID">ID of the NPC you want</param>
        /// <returns>Returns an NPC of a specified ID if there is one</returns>
        public static NPC NPCByID(int ID)
        {
            if (ID >= NPCs.Count)
            {
                Utils.Add("Invalid NPC ID");
                return null;
            }

            return NPCs[ID];
        }

        /// <summary>
        /// Gets a Monster with the specified ID
        /// </summary>
        /// <param name="ID">ID of the Monster you want</param>
        /// <returns>Returns a Monster of a specified ID if there is one</returns>
        public static Monster MonsterByID(int ID)
        {
            if (ID >= Monsters.Count)
            {
                Utils.Add("Invalid Monster ID");
                return null;
            }

            return Monsters[ID];
        }

        /// <summary>
        /// Gets a Location with the specified ID
        /// </summary>
        /// <param name="ID">ID of the Location you want</param>
        /// <returns>Returns a Location of a specified ID if there is one</returns>
        public static Location LocationByID(int ID)
        {
            for (int x = 0; x < Locations.Count; x++)
            {
                if (Locations[x].ID == ID)
                {
                    return Locations[x];
                }
            }
            Utils.Add("Invalid Location ID");
            return null;
        }

        /// <summary>
        /// Gets a Quest with the specified ID
        /// </summary>
        /// <param name="ID">ID of the Quest you want</param>
        /// <returns>Returns a Quest of a specified ID if there is one</returns>
        public static Quest QuestByID(int ID)
        {
            for (int x = 0; x < Quests.Count; x++)
            {
                if (Quests[x].ID == ID)
                {
                    return Quests[x];
                }
            }
            Utils.Add("Invalid Quest ID");
            return null;
        }
        #endregion

        //Help methods that add the helps lines to be printed
        #region Help
        /// <summary>
        /// Adds all of the general help commands to the string to upload
        /// </summary>
        public static void Help()
        {
            Utils.Add("Possible Commands:", TextColor.LIME);
            Utils.Add("\thelp color: explains the text color scheme", TextColor.LIME);
            Utils.Add("\thelp combat: explains how combat works", TextColor.LIME);
            Utils.Add("\tsave <name of save file>: creates a save file with the specified name", TextColor.LIME);
            Utils.Add("\tload <(Optional)name of save file>: Enters the load loop\n\tand attempts to load a save file with the specified name if given", TextColor.LIME);
            Utils.Add("\twho am i: tells you your name", TextColor.LIME);
            Utils.Add("\tlook <thing to look at>: lets you look at things\n\tSubCommands: \n\t\tNorth; East; South; West: lets you look at locations in that direction" +
                "\n\t\there: lets you look here and tells you of everything in this area\n\t\tItem Names: lets you look at the specified item" +
                "\n\t\tMonster Names: lets you look at the specified monster\n\t\tQuest Names: lets you look at the specified quest", TextColor.LIME);
            Utils.Add("\tstats: shows you your current stats", TextColor.LIME);
            Utils.Add("\tinventory: shows you all the items in your inventory", TextColor.LIME);
            Utils.Add("\tquests: shows you your current quests", TextColor.LIME);
            Utils.Add("\tmove <direction to move in>: lets you move in a specified direction", TextColor.LIME);
            Utils.Add("\tequip <item to equip>: lets you equip a specified item", TextColor.LIME);
            Utils.Add("\tunequip <item to unequip>: lets you unequip a specified item", TextColor.LIME);
            Utils.Add("\tuse <item to use>: lets you use a specified item", TextColor.LIME);
            Utils.Add("\tattack: attacks the current monster in the area", TextColor.LIME);
            Utils.Add("\ttalk: talks to the current NPC in the area", TextColor.LIME);
            Utils.Add("\tquit: quits the game", TextColor.LIME);
        }
        /// <summary>
        /// Adds all of the Color help commands to the string to upload
        /// </summary>
        public static void HelpColor()
        {
            Utils.Add("Color Reference:", TextColor.LIME);
            Utils.Add("\tWhite" + Utils.ColorText(": normal text", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Red", TextColor.RED) + Utils.ColorText(": Monster", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Sea Green", TextColor.SEAGREEN) + Utils.ColorText(": NPC", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Magenta", TextColor.MAGENTA) + Utils.ColorText(": quest objective", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Blue", TextColor.BLUE) + Utils.ColorText(": Damage", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Purple", TextColor.PURPLE) + Utils.ColorText(": Healing", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Yellow", TextColor.YELLOW) + Utils.ColorText(": Gold", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Green", TextColor.GREEN) + Utils.ColorText(": XP", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Dark Red", TextColor.DARKRED) + Utils.ColorText(": player death", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Salmon", TextColor.SALMON) + Utils.ColorText(": Weapon", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Light Blue", TextColor.LIGHTBLUE) + Utils.ColorText(": Armor", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Pink", TextColor.PINK) + Utils.ColorText(": Consumable", TextColor.LIME));
            Utils.Add("\t" + Utils.ColorText("Gold", TextColor.GOLD) + Utils.ColorText(": General Item", TextColor.LIME));
            Utils.Add("\tLime: Help", TextColor.LIME);
        }
        /// <summary>
        /// gives a basic combat tutorial
        /// </summary>
        public static void HelpCombat()
        {
            Utils.Add("Combat:", TextColor.LIME);
            Utils.Add("When you type the 'attack' command and there is a monster in the area a window will pop up,\n" +
                "in this window you'll have a chance to attack.\n" +
                "Each enemy has a unique attack and so does each weapon!\n" +
                "To find out how your attacks work look at your weapon,\n" +
                "you can also get a simple description of the enemy's attack by looking at them.\n" +
                "attack phases last around 20 seconds each", TextColor.LIME);
        }
        #endregion
    }
}
