using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Player(int currentHitPoints, int maxHitPoints, int gold, int experiencePoints, int level) : base(currentHitPoints, maxHitPoints)
        {
            this.gold = gold;
            this.experiencePoints = experiencePoints;
            this.level = level;

            inventory = new List<InventoryItem>();
            quests = new List<PlayerQuest>();
        }
    }
}
