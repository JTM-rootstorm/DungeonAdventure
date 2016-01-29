using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Creatures
{
    public class Creature
    {
        public int ID { get; set; }
        public string name { get; set; }

        public string race { get; set; }

        public int strength { get; set; } = 8;
        public int dexterity { get; set; } = 8;
        public int constitution { get; set; } = 8;
        public int intelligence { get; set; } = 8;
        public int wisdom { get; set; } = 8;
        public int charisma { get; set; } = 8;
    }
}
