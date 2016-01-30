using System.Collections.Generic;
using System.Linq;

using Engine.Items;

namespace Engine.Creatures.Player
{
    public class Player : Creature
    {
        public int gold { get; set; } = 0;
        public int experiencePoints { get; set; } = 0;
        public int level
        {
            get { return ((experiencePoints / 100) + 1); }
        }

        public int AC { get; set; }
        public int profBonus { get; set; }
        public int damageMod { get; set; }

        public string race { get; set; }
        public string pClass { get; set; }

        public List<InventoryItem> inventory { get; set; }
        public List<PlayerQuest> quests { get; set; }

        public Location currentLocation { get; set; }

        public Player(int currentHitPoints, int maxHitPoints, int gold, int experiencePoints) : base(currentHitPoints, maxHitPoints)
        {
            this.gold = gold;
            this.experiencePoints = experiencePoints;

            inventory = new List<InventoryItem>();
            quests = new List<PlayerQuest>();
        }

        public Player(int maxHitPoints, int str, int dexterity, int con, int intel, int wis, int charisma) : base (maxHitPoints, maxHitPoints)
        {
            currentHitPoints = maxHitPoints;
            maximumHitPoints = maxHitPoints;
            SetAttributes(str, dexterity, con, intel, wis, charisma);
            AC = 10 + FindAttModifier(dexterity);
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
    }
}
