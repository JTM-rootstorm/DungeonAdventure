using System.Collections.Generic;

using Engine.Items;

namespace Engine.Creatures.Monsters
{
    public class Monster : Creature
    {
        public int ID { get; set; }
        public int AC { get; set; }
        public int maximumDamage { get; set; }
        public int minimumDamage { get; set; }
        public int rewardExperiencePoints { get; set; }
        public int rewardGold { get; set; }

        public List<LootItem> lootTable { get; set; }

        public Monster(int ID, string name, int AC, int attackBonus, int minimumDamage, int maximumDamage, 
            int rewardExperiencePoints, int rewardGold, int maximumHitPoints) : base(maximumHitPoints, maximumHitPoints, attackBonus, name)
        {
            this.ID = ID;
            this.AC = AC;
            this.minimumDamage = minimumDamage;
            this.maximumDamage = maximumDamage;
            this.rewardExperiencePoints = rewardExperiencePoints;
            this.rewardGold = rewardGold;

            lootTable = new List<LootItem>();
        }        
    }
}
