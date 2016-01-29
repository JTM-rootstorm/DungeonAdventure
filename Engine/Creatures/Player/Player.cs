using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Creatures;

namespace Engine.Creatures.Player
{
    public class Player : Creature
    {
        public int currentHitPoints { get; set; }
        public int maximumHitPoints { get; set; }
        public int gold { get; set; }
        public int experiencePoints { get; set; }
        public int level { get; set; }

        public string pClass { get; set; }
    }
}
