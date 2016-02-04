using System.Collections.Generic;

using Engine.Items;
using Engine.Items.Player;
using Engine.Creatures.Monsters;

namespace Engine.Systems
{
    public static class World
    {
        public static readonly List<Item> items = new List<Item>();
        public static readonly List<Monster> monsters = new List<Monster>();
        public static readonly List<Quest> quests = new List<Quest>();
        public static readonly List<Location[,]> dungeonFloors = new List<Location[,]>();

        public const int WEAPON_ID_SHORT_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int WEAPON_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;

        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            items.Add(new Weapon(WEAPON_ID_SHORT_SWORD, "Shortsword", "Shortswords", 1, 6, true));
            items.Add(new Weapon(WEAPON_ID_CLUB, "Club", "Clubs", 3, 10, false));

            items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails"));
            items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur"));
            items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs"));
            items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
            items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5));
            items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs"));
            items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks"));
            items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes"));
        }

        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 10, 0, 1, 1, 10, 0, RandomNumberGenerator.NumberBetween(1, 3));
            rat.SetAttributes(2, 11, 9, 2, 10, 4);
            rat.lootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.lootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 12, 4, 3, 8, 50, 0, RandomNumberGenerator.NumberBetween(4, 22));
            snake.SetAttributes(15, 14, 12, 1, 10, 3);
            snake.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant spider", 12, 4, 1, 1, 10, 00, RandomNumberGenerator.NumberBetween(1, 3));
            giantSpider.SetAttributes(2, 14, 8, 1, 10, 2);
            giantSpider.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.lootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));

            monsters.Add(rat);
            monsters.Add(snake);
            monsters.Add(giantSpider);
        }

        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "Clear the alchemist's garden",
                    "Kill rats in the alchemist's garden and bring back 3 rat tails. You will receive a healing potion and 10 gold pieces.", 20, 10);

            clearAlchemistGarden.questCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.rewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Clear the farmer's field",
                    "Kill snakes in the farmer's field and bring back 3 snake fangs. You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

            clearFarmersField.questCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.rewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            quests.Add(clearAlchemistGarden);
            quests.Add(clearFarmersField);
        }

        private static void PopulateLocations()
        {
            // floor 1
            Location[,] firstFloor = new Location[4, 6];

            // Create each location
            Location home = new Location(LOCATION_ID_HOME, "Home", "Your house. You really need to clean up the place.");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town square", "You see a fountain.");

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's hut", "There are many strange plants on the shelves.");
            alchemistHut.questAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Alchemist's garden", "Many plants are growing here.");
            alchemistsGarden.monsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a small farmhouse, with a farmer in front.");
            farmhouse.questAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's field", "You see rows of vegetables growing here.");
            farmersField.monsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a large, tough-looking guard here.", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wide river.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "You see spider webs covering covering the trees in this forest.");
            spiderField.monsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            // Link the locations together
            firstFloor[3, 2] = home;

            firstFloor[2, 2] = townSquare;
            firstFloor[2, 3] = guardPost;
            firstFloor[2, 4] = bridge;
            firstFloor[2, 5] = spiderField;

            firstFloor[1, 2] = alchemistHut;
            firstFloor[1, 1] = farmhouse;
            firstFloor[1, 0] = farmersField;

            firstFloor[0, 2] = alchemistsGarden;

            foreach (Location loc in firstFloor)
            {
                for(int i = 0; i < 4; i++)
                {
                    for(int j = 0; j < 6; j++)
                    {
                        if(firstFloor[i,j] == null)
                        {
                            continue;
                        }
                        else if (firstFloor[i,j] == loc)
                        {
                            loc.GenerateLocationLinks(firstFloor, i, j, 3, 5);
                        }
                    }
                }
            }

            // Add the locations to the static list
            dungeonFloors.Add(firstFloor);
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }

            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach(Location[,] locAr in dungeonFloors)
            {
                foreach(Location location in locAr)
                {
                    if(location == null)
                    {
                        continue;
                    }
                    else
                    {
                        if(location.ID == id)
                        {
                            return location;
                        }
                    }
                }
            }

            return null;
        }
    }
}
