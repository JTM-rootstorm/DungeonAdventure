using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int rewardExperiencePoints { get; set; }
        public int rewardGold { get; set; }
    }
}
