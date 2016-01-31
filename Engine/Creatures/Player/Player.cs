using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using System;

using Engine.Items;
using Engine.Items.Player;
using Engine.Creatures.Monsters;

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

        public BindingList<InventoryItem> inventory { get; set; }
        public BindingList<PlayerQuest> quests { get; set; }

        private Location _currentLocation;
        public Location currentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged("currentLocation");
            }
        }

        private Monster _currentMonster;

        public Weapon currentWeapon { get; set; }

        public event EventHandler<MessageEventArgs> OnMessage;

        public List<Weapon> weapons
        {
            get { return inventory.Where(x => x.details is Weapon).Select(x => x.details as Weapon).ToList(); }
        }

        public List<HealingPotion> potions
        {
            get { return inventory.Where(x => x.details is HealingPotion).Select(x => x.details as HealingPotion).ToList(); }
        }

        public Player(int currentHitPoints, int maxHitPoints, int str, int dexterity, int con, int intel, int wis, int charisma, string name) : base (currentHitPoints, maxHitPoints, name)
        {
            this.currentHitPoints = currentHitPoints;
            maximumHitPoints = maxHitPoints;
            SetAttributes(str, dexterity, con, intel, wis, charisma);
            AC = 10 + FindAttModifier(dexterity);

            inventory = new BindingList<InventoryItem>();
            quests = new BindingList<PlayerQuest>();
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

        private void RaiseInventoryChangedEvent(Item item)
        {
            if(item is Weapon)
            {
                OnPropertyChanged("weapons");
            }
            if(item is HealingPotion)
            {
                OnPropertyChanged("potions");
            }
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = inventory.SingleOrDefault(ii => ii.details.ID == itemToRemove.ID);

            if(item == null)
            {

            }
            else
            {
                item.quantity -= quantity;

                if(item.quantity < 0)
                {
                    item.quantity = 0;
                }

                if(item.quantity == 0)
                {
                    inventory.Remove(item);
                }

                RaiseInventoryChangedEvent(itemToRemove);
            }
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.itemRequiredToEnter == null)
            {
                return true;
            }

            return inventory.Any(ii => ii.details.ID == location.itemRequiredToEnter.ID);
        }

        public bool HasThisQuest(Quest quest)
        {
            return quests.All(pq => pq.details.ID == quest.ID);
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
                if(!inventory.Any(ii => ii.details.ID == qci.details.ID && ii.quantity >= qci.quantity))
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
                    RemoveItemFromInventory(item.details, qci.quantity);
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = inventory.SingleOrDefault(ii => ii.details.ID == itemToAdd.ID);

            if(item == null)
            {
                inventory.Add(new InventoryItem(itemToAdd, 1));
            }
            else
            {
                item.quantity += quantity;
            }

            RaiseInventoryChangedEvent(itemToAdd);
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
                return CreateDefaultPlayer();
            }
        }

        private void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if(OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }

        public void MoveTo(Location newLocation)
        {
            //Does the location have any required items
            if (!HasRequiredItemToEnterThisLocation(newLocation))
            {
                RaiseMessage("You must have a " + newLocation.itemRequiredToEnter.name + " to enter this location.");
                return;
            }

            // Update the player's current location
            currentLocation = newLocation;

            // Does the location have a quest?
            if (newLocation.questAvailableHere != null)
            {
                // See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = HasThisQuest(newLocation.questAvailableHere);
                bool playerAlreadyCompletedQuest = CompletedThisQuest(newLocation.questAvailableHere);

                // See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if (!playerAlreadyCompletedQuest)
                    {
                        // See if the player has all the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest = HasAllQuestCompletionItems(newLocation.questAvailableHere);

                        // The player has all items required to complete the quest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Display message
                            RaiseMessage("");
                            RaiseMessage("You complete the '" + newLocation.questAvailableHere.name + "' quest.");

                            // Remove quest items from inventory
                            RemoveQuestCompletionItems(newLocation.questAvailableHere);

                            // Give quest rewards
                            RaiseMessage("You receive: ");
                            RaiseMessage(newLocation.questAvailableHere.rewardExperiencePoints + " experience points");
                            RaiseMessage(newLocation.questAvailableHere.rewardGold + " gold");
                            RaiseMessage(newLocation.questAvailableHere.rewardItem.name, true);

                            AddExperiencePoints(newLocation.questAvailableHere.rewardExperiencePoints);
                            gold += newLocation.questAvailableHere.rewardGold;

                            // Add the reward item to the player's inventory
                            AddItemToInventory(newLocation.questAvailableHere.rewardItem);

                            // Mark the quest as completed
                            MarkQuestCompleted(newLocation.questAvailableHere);
                        }
                    }
                }
                else
                {
                    // The player does not already have the quest

                    // Display the messages
                    RaiseMessage("You receive the " + newLocation.questAvailableHere.name + " quest.");
                    RaiseMessage(newLocation.questAvailableHere.description);
                    RaiseMessage("To complete it, return with:");
                    foreach (QuestCompletionItem qci in newLocation.questAvailableHere.questCompletionItems)
                    {
                        if (qci.quantity == 1)
                        {
                            RaiseMessage(qci.quantity + " " + qci.details.name);
                        }
                        else
                        {
                            RaiseMessage(qci.quantity + " " + qci.details.namePlural);
                        }
                    }
                    RaiseMessage("");

                    // Add the quest to the player's quest list
                    quests.Add(new PlayerQuest(newLocation.questAvailableHere));
                }
            }

            // Does the location have a monster?
            if (newLocation.monsterLivingHere != null)
            {
                RaiseMessage("You see a " + newLocation.monsterLivingHere.name);

                // Make a new monster, using the values from the standard monster in the World.Monster list
                Monster standardMonster = World.World.MonsterByID(newLocation.monsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.name, standardMonster.AC, standardMonster.attackMod, standardMonster.minimumDamage, standardMonster.maximumDamage,
                    standardMonster.rewardExperiencePoints, standardMonster.rewardGold, standardMonster.maximumHitPoints);

                foreach (LootItem lootItem in standardMonster.lootTable)
                {
                    _currentMonster.lootTable.Add(lootItem);
                }
            }
            else
            {
                _currentMonster = null;
            }
        }

        public void UseWeapon(Weapon weapon)
        {
            // Determine the amount of damage to do to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(weapon.minimumDamage, weapon.maximumDamage) + damageMod;

            // Apply the damage to the monster's CurrentHitPoints
            _currentMonster.currentHitPoints -= damageToMonster;

            // Display message
            RaiseMessage("You hit the " + _currentMonster.name + " for " + damageToMonster + " points.");

            // Check if the monster is dead
            if (_currentMonster.currentHitPoints <= 0)
            {
                // Monster is dead
                RaiseMessage("");
                RaiseMessage("You defeated the " + _currentMonster.name);

                // Give player experience points for killing the monster
                AddExperiencePoints(_currentMonster.rewardExperiencePoints);
                RaiseMessage("You receive " + _currentMonster.rewardExperiencePoints + " experience points");

                // Give player gold for killing the monster 
                gold += _currentMonster.rewardGold;
                RaiseMessage("You receive " + _currentMonster.rewardGold + " gold");

                // Get random loot items from the monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem lootItem in _currentMonster.lootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.dropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.details, 1));
                    }
                }

                // If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.lootTable)
                    {
                        if (lootItem.isDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.details, 1));
                        }
                    }
                }

                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    AddItemToInventory(inventoryItem.details);

                    if (inventoryItem.quantity == 1)
                    {
                        RaiseMessage("You loot " + inventoryItem.quantity + " " + inventoryItem.details.name);
                    }
                    else
                    {
                        RaiseMessage("You loot " + inventoryItem.quantity + " " + inventoryItem.details.namePlural);
                    }
                }

                // Add a blank line to the messages box, just for appearance.
                RaiseMessage("");

                // Move player to current location (to heal player and create a new monster to fight)
                MoveTo(currentLocation);
            }
            else
            {
                // Monster is still alive

                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.maximumDamage);

                // Display message
                RaiseMessage("The " + _currentMonster.name + " did " + damageToPlayer + " points of damage.");

                // Subtract damage from player
                currentHitPoints -= damageToPlayer;

                if (currentHitPoints <= 0)
                {
                    // Display message
                    RaiseMessage("The " + _currentMonster.name + " killed you.");

                    // Move player to "Home"
                    MoveHome();
                }
            }
        }

        public void UsePotion(HealingPotion potion)
        {
            // Add healing amount to the player's current hit points
            currentHitPoints = (currentHitPoints + potion.amountToHeal);

            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (currentHitPoints > maximumHitPoints)
            {
                currentHitPoints = maximumHitPoints;
            }

            // Remove the potion from the player's inventory
            RemoveItemFromInventory(potion, 1);

            // Display message
            RaiseMessage("You drink a " + potion.name);

            // Monster gets their turn to attack

            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.maximumDamage);

            // Display message
            RaiseMessage("The " + _currentMonster.name + " did " + damageToPlayer + " points of damage.");

            // Subtract damage from player
            currentHitPoints -= damageToPlayer;

            if (currentHitPoints <= 0)
            {
                // Display message
                RaiseMessage("The " + _currentMonster.name + " killed you.");

                // Move player to "Home"
                MoveHome();
            }
        }

        private void MoveHome()
        {
            MoveTo(World.World.LocationByID(World.World.LOCATION_ID_HOME));
        }

        public void MoveNorth()
        {
            if (currentLocation.locationToNorth != null)
            {
                MoveTo(currentLocation.locationToNorth);
            }
        }

        public void MoveEast()
        {
            if (currentLocation.locationToEast != null)
            {
                MoveTo(currentLocation.locationToEast);
            }
        }

        public void MoveSouth()
        {
            if (currentLocation.locationToSouth != null)
            {
                MoveTo(currentLocation.locationToSouth);
            }
        }

        public void MoveWest()
        {
            if (currentLocation.locationToWest != null)
            {
                MoveTo(currentLocation.locationToWest);
            }
        }
    }
}
