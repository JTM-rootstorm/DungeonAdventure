using System.Collections.Generic;

using Engine.Items;

namespace Engine.Creatures.Player
{
    public class Player : Creature
    {
        public int gold { get; set; }
        public int experiencePoints { get; set; }
        public int level { get; set; }

        public string pClass { get; set; }

        public List<InventoryItem> inventory { get; set; }
        public List<PlayerQuest> quests { get; set; }

        public Location currentLocation { get; set; }

        public Player(int currentHitPoints, int maxHitPoints, int gold, int experiencePoints, int level) : base(currentHitPoints, maxHitPoints)
        {
            this.gold = gold;
            this.experiencePoints = experiencePoints;
            this.level = level;

            inventory = new List<InventoryItem>();
            quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.itemRequiredToEnter == null)
            {
                return true;
            }

            foreach(InventoryItem ii in inventory)
            {
                if(ii.details.ID == location.itemRequiredToEnter.ID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in quests)
            {
                if(playerQuest.details.ID == quest.ID)
                {
                    return true;
                }
            }

            return false;
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
                bool foundItemInPlayersInventory = false;

                foreach(InventoryItem ii in inventory)
                {
                    if(ii.details.ID == qci.details.ID)
                    {
                        foundItemInPlayersInventory = true;

                        if(ii.quantity < qci.quantity)
                        {
                            return false;
                        }
                    }
                }

                if (!foundItemInPlayersInventory)
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
                foreach(InventoryItem ii in inventory)
                {
                    if(ii.details.ID == qci.details.ID)
                    {
                        ii.quantity -= qci.quantity;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach(InventoryItem ii in inventory)
            {
                if(ii.details.ID == itemToAdd.ID)
                {
                    ii.quantity++;
                    return;
                }
            }

            inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            foreach(PlayerQuest pq in quests)
            {
                if(pq.details.ID == quest.ID)
                {
                    pq.isCompleted = true;

                    return;
                }
            }
        }
    }
}
