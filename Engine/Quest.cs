using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Items;

namespace Engine
{
    public class Quest
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int rewardExperiencePoints { get; set; }
        public int rewardGold { get; set; }

        public Item rewardItem { get; set; }

        public List<QuestCompletionItem> questCompletionItems { get; set; }

        public Quest(int ID, string name, string description, int rewardEXP, int rewardGold)
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            rewardExperiencePoints = rewardEXP;
            this.rewardGold = rewardGold;

            questCompletionItems = new List<QuestCompletionItem>();
        }
    }
}
