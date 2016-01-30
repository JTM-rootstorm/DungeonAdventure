using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

using Engine.Items;

namespace Engine.Creatures.Player
{
    public class Player : Creature
    {
        private int _gold = 0;
        public int gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("gold");
            }
        }
        private int _experiencePoints = 0;
        public int experiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("experiencePoints");

                if(experiencePoints >= levelArray[level, 0])
                {
                    level++;
                }
            }
        }
        private int _level = 1;
        public int level
        {
            get { return _level; }
            private set
            {
                _level = value;
                OnPropertyChanged("level");
            }
        }

        public int[,] levelArray = new int[20, 2] { { 0, 2 }, { 300, 2 }, { 900, 2 }, { 2700, 2 }, { 6500, 3 }, { 14000, 3 }, { 23000, 3 }, { 34000, 3 }, { 48000, 4 }, { 64000, 4 },
        { 85000, 4 }, { 100000, 4 }, { 120000, 5 }, { 140000, 5 }, { 165000, 5 }, { 195000, 5 }, { 225000, 6 }, { 265000, 6 }, { 305000, 6 }, { 355000, 6 }};

        public int AC { get; set; }
        public int profBonus
        {
          get { return levelArray[level - 1, 1]; }
          set
            {
                profBonus = value;
            }
        }
        public int damageMod { get; set; }

        public string race { get; set; }
        public string pClass { get; set; }
        private int hitDie { get; set; }

        public List<InventoryItem> inventory { get; set; }
        public List<PlayerQuest> quests { get; set; }

        public Location currentLocation { get; set; }

        public Weapon currentWeapon { get; set; }

        public Player(int currentHitPoints, int maxHitPoints, int str, int dexterity, int con, int intel, int wis, int charisma, string name) : base (currentHitPoints, maxHitPoints, name)
        {
            this.currentHitPoints = currentHitPoints;
            maximumHitPoints = maxHitPoints;
            SetAttributes(str, dexterity, con, intel, wis, charisma);
            AC = 10 + FindAttModifier(dexterity);

            inventory = new List<InventoryItem>();
            quests = new List<PlayerQuest>();
        }

        public void AddExperiencePoints(int experiencePointsToAdd)
        {
            experiencePoints += experiencePointsToAdd;

        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 8, 8, 8, 8, 8, 8, "Bob");
            player.inventory.Add(new InventoryItem(World.World.ItemByID(World.World.WEAPON_ID_SHORT_SWORD), 1));
            player.currentLocation = World.World.LocationByID(World.World.LOCATION_ID_HOME);

            return player;
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.itemRequiredToEnter == null)
            {
                return true;
            }

            return inventory.Exists(ii => ii.details.ID == location.itemRequiredToEnter.ID);
        }

        public bool HasThisQuest(Quest quest)
        {
            return quests.Exists(pq => pq.details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in quests)
            {
                if(playerQuest.details.ID == quest.ID)
                {
                    return playerQuest.isCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.questCompletionItems)
            {
                if(!inventory.Exists(ii => ii.details.ID == qci.details.ID && ii.quantity >= qci.quantity))
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.questCompletionItems)
            {
                InventoryItem item = inventory.SingleOrDefault(ii => ii.details.ID == qci.details.ID);

                if(item != null)
                {
                    item.quantity -= qci.quantity;
                    break;
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            InventoryItem item = inventory.SingleOrDefault(ii => ii.details.ID == itemToAdd.ID);

            if(item == null)
            {
                inventory.Add(new InventoryItem(itemToAdd, 1));
            }
            else
            {
                item.quantity++;
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            PlayerQuest playerQuest = quests.SingleOrDefault(pq => pq.details.ID == quest.ID);

            if(playerQuest != null)
            {
                playerQuest.isCompleted = true;
            }
        }

        public string ToXMLString()
        {
            XmlDocument playerData = new XmlDocument();

            // Create the top-level XML node
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);

            // Create the "Stats" child node to hold the other player statistics nodes
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            // Create the child nodes for the "Stats" node
            XmlNode name = playerData.CreateElement("Name");
            name.AppendChild(playerData.CreateTextNode(this.name));
            stats.AppendChild(name);

            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.currentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.maximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode str = playerData.CreateElement("Strength");
            str.AppendChild(playerData.CreateTextNode(strength.ToString()));
            stats.AppendChild(str);

            XmlNode dex = playerData.CreateElement("Dexterity");
            dex.AppendChild(playerData.CreateTextNode(dexterity.ToString()));
            stats.AppendChild(dex);

            XmlNode con = playerData.CreateElement("Constitution");
            con.AppendChild(playerData.CreateTextNode(constitution.ToString()));
            stats.AppendChild(con);

            XmlNode intel = playerData.CreateElement("Intelligence");
            intel.AppendChild(playerData.CreateTextNode(intelligence.ToString()));
            stats.AppendChild(intel);

            XmlNode wis = playerData.CreateElement("Wisdom");
            wis.AppendChild(playerData.CreateTextNode(wisdom.ToString()));
            stats.AppendChild(wis);

            XmlNode charisma = playerData.CreateElement("Charisma");
            charisma.AppendChild(playerData.CreateTextNode(this.charisma.ToString()));
            stats.AppendChild(charisma);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.experiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.currentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);

            if (currentWeapon != null)
            {
                XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
                currentWeapon.AppendChild(playerData.CreateTextNode(this.currentWeapon.ID.ToString()));
                stats.AppendChild(currentWeapon);
            }

            // Create the "InventoryItems" child node to hold each InventoryItem node
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            // Create an "InventoryItem" node for each item in the player's inventory
            foreach (InventoryItem item in this.inventory)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = item.details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);

                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);

                inventoryItems.AppendChild(inventoryItem);
            }

            // Create the "PlayerQuests" child node to hold each PlayerQuest node
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            // Create a "PlayerQuest" node for each quest the player has acquired
            foreach (PlayerQuest quest in this.quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = quest.details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);

                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = quest.isCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);

                playerQuests.AppendChild(playerQuest);
            }

            return playerData.InnerXml; // The XML document, as a string, so we can save the data to disk
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();

                playerData.LoadXml(xmlPlayerData);

                string name = playerData.SelectSingleNode("/Player/Stats/Name").InnerText;

                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);

                int str = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Strength").InnerText);
                int dex = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Dexterity").InnerText);
                int con = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Constitution").InnerText);
                int intel = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Intelligence").InnerText);
                int wis = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Wisdom").InnerText);
                int charisma = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Charisma").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, str, dex, con, intel, wis, charisma, name);

                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                player.currentLocation = World.World.LocationByID(currentLocationID);

                if(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
                {
                    int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                    player.currentWeapon = (Weapon)World.World.ItemByID(currentWeaponID);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    for (int i = 0; i < quantity; i++)
                    {
                        player.AddItemToInventory(World.World.ItemByID(id));
                    }
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);

                    PlayerQuest playerQuest = new PlayerQuest(World.World.QuestByID(id));
                    playerQuest.isCompleted = isCompleted;

                    player.quests.Add(playerQuest);
                }

                return player;
            }
            catch
            {
                // If there was an error with the XML data, return a default player object
                return Player.CreateDefaultPlayer();
            }
        }
    }
}
